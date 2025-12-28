#if UNITY_EDITOR
using Game.Runtime.InGame.Models;
using Game.Runtime.InGame.Models.Level;
using Game.Runtime.InGame.Scripts.Interfaces;
using Game.Runtime.InGame.Scripts.Models;
using Game.Runtime.JsonUtils.JsonConverters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Game.Editor.LevelCreation
{
    public class LevelCreationHelperEditor : EditorWindow
    {
        
        private string[] _levelFiles;
        private int _selectedLevelIndex = 0;
        private string _levelNameInput = "Level_1";

        private PrefabDatabase _prefabDatabase;

        private GameObject _mapRoot;
        private Camera _cameraRef;
        private GameObject _environmentRoot;
        private GameObject _collectablesRoot;
        private float _levelDuration;

        [MenuItem("Tools/Level Editor")]
        public static void ShowWindow() => GetWindow<LevelCreationHelperEditor>("Level Editor");

        private void OnEnable()
        {
            if (_prefabDatabase == null)
            {
                _prefabDatabase = Resources.Load<PrefabDatabase>("PrefabDatabase");
                
                if(_prefabDatabase == null)
                {
                    Debug.LogError("PrefabDatabase asset could not be found anywhere in the project! Please create one.");
                }
            }
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                Converters = new JsonConverter[]
                {
                    new Vector3Converter(),
                }
            };
            RefreshFileList();
        }
        void OnGUI()
        {
            GUILayout.Label("Level Manager", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical("box");
            bool hasLevels = _levelFiles != null && _levelFiles.Length > 0;
            if (hasLevels)
            {
                EditorGUILayout.BeginHorizontal();
                _selectedLevelIndex = EditorGUILayout.Popup("Existing Levels:", _selectedLevelIndex, _levelFiles);
                if (GUILayout.Button("Load", GUILayout.Width(60))) LoadLevel(_levelFiles[_selectedLevelIndex]);
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Refresh List")) RefreshFileList();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10);
            _levelNameInput = EditorGUILayout.TextField("Level Name:", _levelNameInput);

            EditorGUILayout.BeginVertical("helpbox");
            GUILayout.Label("Assign Root Objects", EditorStyles.boldLabel);
            _mapRoot = (GameObject)EditorGUILayout.ObjectField("Map Root", _mapRoot, typeof(GameObject), true);
            _cameraRef = (Camera)EditorGUILayout.ObjectField("Main Camera", _cameraRef, typeof(Camera), true);
            _environmentRoot = (GameObject)EditorGUILayout.ObjectField("Environment Root", _environmentRoot, typeof(GameObject), true);
            _collectablesRoot = (GameObject)EditorGUILayout.ObjectField("Collectables Root", _collectablesRoot, typeof(GameObject), true);
            _levelDuration = EditorGUILayout.FloatField("LevelDuration", _levelDuration);
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Save Level", GUILayout.Height(40)))
                SaveLevel(_levelNameInput);
            EditorGUILayout.EndHorizontal();
        }

        void SaveLevel(string fileName, bool asNew = false)
        {
            if (_mapRoot == null || _cameraRef == null || _environmentRoot == null || _collectablesRoot == null)
            {
                Debug.LogError("Please assign all root objects!");
                return;
            }

            string oldFileName = _levelFiles[_selectedLevelIndex];
            string newPath = Path.Combine(GameConstants.LevelsFolder, fileName + ".json");

            GameItemData mapData = ExtractData(_mapRoot.transform);
            CameraData cameraData = new CameraData(ExtractData(_cameraRef.transform), _cameraRef.orthographicSize);

            GameItemData environementRootData = ExtractData(_environmentRoot.transform);
            GameItemData collectablesRootData = ExtractData(_collectablesRoot.transform);

            var envDict = FetchEnvironmentData();
            var colDict = FetchCollectableData();

            LevelData newLevel = new LevelData(mapData, cameraData, environementRootData, collectablesRootData, envDict, colDict, _levelDuration);

            string json = JsonConvert.SerializeObject(newLevel);
            File.WriteAllText(newPath, json);

            AssetDatabase.Refresh();
            RefreshFileList();

            _selectedLevelIndex = System.Array.IndexOf(_levelFiles, fileName);

            Debug.Log($"Level updated and renamed to: {fileName}");
        }

        private Dictionary<EnvironmentId, List<GameItemData>> FetchEnvironmentData()
        {
            var envDict = new Dictionary<EnvironmentId, List<GameItemData>>();
            if (_environmentRoot == null) return envDict;

            foreach (Transform child in _environmentRoot.transform)
            {
                var entity = child.GetComponent<IEnvironment>();
                if (entity == null) continue;

                if (!envDict.ContainsKey(entity.EnvironmentId))
                    envDict[entity.EnvironmentId] = new List<GameItemData>();

                envDict[entity.EnvironmentId].Add(ExtractData(child));
            }
            return envDict;
        }

        private Dictionary<CollectableId, List<GameItemData>> FetchCollectableData()
        {
            var colDict = new Dictionary<CollectableId, List<GameItemData>>();
            if (_collectablesRoot == null) return colDict;

            foreach (Transform child in _collectablesRoot.transform)
            {
                var entity = child.GetComponent<ICollectable>();
                if (entity == null) continue;

                if (!colDict.ContainsKey(entity.CollectableId))
                    colDict[entity.CollectableId] = new List<GameItemData>();

                colDict[entity.CollectableId].Add(ExtractData(child));
            }
            return colDict;
        }

        GameItemData ExtractData(Transform t) => new GameItemData(t.position, t.eulerAngles, t.localScale);

        void RefreshFileList()
        {

            if (!Directory.Exists(GameConstants.LevelsFolder))
            {
                Directory.CreateDirectory(GameConstants.LevelsFolder);
                AssetDatabase.Refresh();
            }

            string[] guids = AssetDatabase.FindAssets("t:TextAsset", new[] { GameConstants.LevelsFolder });

            _levelFiles = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => path.EndsWith(".json"))
                .Select(Path.GetFileNameWithoutExtension)
                .ToArray();
        }

        void LoadLevel(string fileName)
        {
            TextAsset asset = Resources.Load<TextAsset>($"LevelData/{fileName}");
            if (asset == null) return;

            LevelData loadedData = JsonConvert.DeserializeObject<LevelData>(asset.text);

            FindAndAssignReferences();

            SyncSceneWithData(loadedData);

            _levelNameInput = fileName;
            Debug.Log($"Level {fileName} references assigned from scene based on names.");
        }

        void FindAndAssignReferences()
        {
            if (_mapRoot == null) _mapRoot = GameObject.Find("Map"); 

            if (_cameraRef == null) _cameraRef = Camera.main;

            if (_environmentRoot == null) _environmentRoot = GameObject.Find("Environments");

            if (_collectablesRoot == null) _collectablesRoot = GameObject.Find("Collectables");

            if (_mapRoot == null || _environmentRoot == null || _collectablesRoot == null)
            {
                Debug.LogWarning("Some Root objects could not be found automatically. Please assign them manually.");
            }
        }

        void SyncSceneWithData(LevelData data)
        {
            if (_mapRoot != null) ApplyDataToTransform(_mapRoot.transform, data.MapData);
            if (_cameraRef != null)
            {
                ApplyDataToTransform(_cameraRef.transform, data.CameraData.CameraTRS);
                _cameraRef.orthographicSize = data.CameraData.CameraSize;
            }

            ClearScene();

            if (_environmentRoot != null)
            {
                ApplyDataToTransform(_environmentRoot.transform, data.EnvironmentRootData);
                if(data.EnvironmentData != null)
                {
                    RebuildGroup(_environmentRoot.transform, data.EnvironmentData);
                }
                
            }

            if (_collectablesRoot != null)
            {
                ApplyDataToTransform(_collectablesRoot.transform, data.CollectablesRootData);
                if(data.CollectableData != null)
                {
                    RebuildGroup(_collectablesRoot.transform, data.CollectableData);
                }
            }

            _levelDuration = data.LevelDuration;
        }

        void ClearScene()
        {
            if (_environmentRoot != null)
            {
                for (int i = _environmentRoot.transform.childCount - 1; i >= 0; i--)
                {
                    Undo.DestroyObjectImmediate(_environmentRoot.transform.GetChild(i).gameObject);
                }
            }

            if (_collectablesRoot != null)
            {
                for (int i = _collectablesRoot.transform.childCount - 1; i >= 0; i--)
                {
                    Undo.DestroyObjectImmediate(_collectablesRoot.transform.GetChild(i).gameObject);
                }
            }
        }

        void RebuildGroup<TId>(Transform root, IReadOnlyDictionary<TId, List<GameItemData>> dataDict)
        {
            if (_prefabDatabase == null) return;

            foreach (var pair in dataDict)
            {
                TId id = pair.Key;

                GameObject prefab = null;
                if (typeof(TId) == typeof(EnvironmentId))
                    prefab = _prefabDatabase.GetEnvironmentPrefab((EnvironmentId)(object)id).gameObject;
                else if (typeof(TId) == typeof(CollectableId))
                    prefab = _prefabDatabase.GetCollectablePrefab((CollectableId)(object)id).gameObject;

                if (prefab == null)
                {
                    Debug.LogWarning($"[LevelEditor] Prefab missing for ID: {id}");
                    continue;
                }

                foreach (var itemData in pair.Value)
                {
                    GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab, root);
                    Undo.RegisterCreatedObjectUndo(newObj, "Rebuild Level Object");
                    ApplyDataToTransform(newObj.transform, itemData);
                }
            }
        }

        void ApplyDataToTransform(Transform target, GameItemData data)
        {
            Undo.RecordObject(target, "Sync Level Data");
            target.position = data.Position;
            target.eulerAngles = data.Rotation;
            target.localScale = data.Scale;
        }
    }
}
#endif