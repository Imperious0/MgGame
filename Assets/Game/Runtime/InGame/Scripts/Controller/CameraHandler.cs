using Game.Runtime.InGame.Models.Level;
using Game.Runtime.InGame.Scripts.Interfaces;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Game.Runtime.InGame.Scripts.Controller
{
    internal class CameraHandler : IUpdatable
    {
        private readonly Camera _cam;
        private readonly GameItemData _mapData;
        private readonly float _zoomSpeed = 0.5f;
        private readonly float _availableZoom = 2f;
        private float _maxSize;

        private Vector2 _lastPointerPosition;
        private bool _isDragging;

        public CameraHandler(Camera cam, GameItemData mapData, float availableZoom = 2f)
        {
            _cam = cam;
            _mapData = mapData;
            _availableZoom = availableZoom;

            CalculateMaxZoom();
        }

        public void Initialize()
        {

        }

        public void Dispose()
        {

        }

        private void CalculateMaxZoom()
        {
            float mapHeight = _mapData.Scale.y / 2f;
            float mapWidth = _mapData.Scale.x / (2f * _cam.aspect);
            _maxSize = Mathf.Min(mapHeight, mapWidth);
        }

        public void Tick(float deltaTime, float unscaledDeltaTime)
        {
            if (Camera.main == null) return;

            var touches = Touch.activeTouches;

            if (touches.Count >= 2)
            {
                _isDragging = false;
                HandleZoom(touches[0], touches[1]);
            }
            else if (touches.Count == 1)
            {
                HandleMove(touches[0]);
            }
            else
            {
                _isDragging = false;
            }

            ConstrainCamera();
        }

        private void HandleMove(Touch touch)
        {
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                _lastPointerPosition = touch.screenPosition;
                _isDragging = true;
            }
            else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved && _isDragging)
            {
                Vector3 currentWorldPos = _cam.ScreenToWorldPoint(touch.screenPosition);
                Vector3 lastWorldPos = _cam.ScreenToWorldPoint(_lastPointerPosition);
                Vector3 delta = lastWorldPos - currentWorldPos;

                _cam.transform.position += delta;
                _lastPointerPosition = touch.screenPosition;
            }
        }

        private void HandleZoom(Touch t1, Touch t2)
        {
            float prevMag = (t1.screenPosition - t1.delta - (t2.screenPosition - t2.delta)).magnitude;
            float currentMag = (t1.screenPosition - t2.screenPosition).magnitude;
            float diff = (currentMag - prevMag) * _zoomSpeed * 0.01f;

            _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize - diff, _maxSize / _availableZoom, _maxSize);
        }

        private void ConstrainCamera()
        {
            Vector3 mapPos = _mapData.Position;
            Vector3 mapScale = _mapData.Scale;

            float camHalfHeight = _cam.orthographicSize;
            float camHalfWidth = _cam.orthographicSize * _cam.aspect;

            float mapHalfWidth = mapScale.x / 2f;
            float mapHalfHeight = mapScale.y / 2f;

            float minX = (mapPos.x - mapHalfWidth) + camHalfWidth;
            float maxX = (mapPos.x + mapHalfWidth) - camHalfWidth;
            float minY = (mapPos.y - mapHalfHeight) + camHalfHeight;
            float maxY = (mapPos.y + mapHalfHeight) - camHalfHeight;

            float targetX = (maxX > minX) ? Mathf.Clamp(_cam.transform.position.x, minX, maxX) : mapPos.x;
            float targetY = (maxY > minY) ? Mathf.Clamp(_cam.transform.position.y, minY, maxY) : mapPos.y;

            _cam.transform.position = new Vector3(targetX, targetY, _cam.transform.position.z);
        }
    }
}
