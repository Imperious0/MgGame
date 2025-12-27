using Game.Runtime.InitializeHelper;
using Game.SingletonHelper;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Runtime.PlayerData
{
    public class PlayerDataController : SingletonBehaviour<PlayerDataController>, IInitializable
    {
        Currencies _currencies;
        public Currencies Currencies {
            get
            {
                if(_currencies == null)
                {
                    string jsonString = PlayerPrefs.GetString("Currencies", string.Empty);
                    if (string.IsNullOrWhiteSpace(jsonString))
                    {
                        _currencies = Currencies.CreateDefault();
                        SaveCurrencies();
                    }
                    else
                    {
                        _currencies = JsonConvert.DeserializeObject<Currencies>(jsonString);
                    }
                }

                return _currencies;
            } 
        }

        public void SaveCurrencies()
        {
            string jsonString = JsonConvert.SerializeObject(_currencies);
            PlayerPrefs.SetString("Currencies", jsonString);
        }
        protected override void OnAwake()
        {
            DontDestroyOnLoad(gameObject);

            InitializeController.Instance.RegisterInitialize(this);
        }

        public void Initialize()
        {

        }

        public void Dispose()
        {

        }
    }
}
