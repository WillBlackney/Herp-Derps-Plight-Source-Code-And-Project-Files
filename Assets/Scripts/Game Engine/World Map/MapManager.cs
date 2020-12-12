using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

namespace MapSystem
{
    public class MapManager : Singleton<MapManager>
    {
        public MapConfig config;
        public MapView view;

        public Map CurrentMap { get; private set; }

        private void Start()
        {
            /*
            if (PlayerPrefs.HasKey("Map"))
            {
                var mapJson = PlayerPrefs.GetString("Map");
                var map = JsonConvert.DeserializeObject<Map>(mapJson);
                // using this instead of .Contains()
                if (map.path.Any(p => p.Equals(map.GetBossNode().point)))
                {
                    // payer has already reached the boss, generate a new map
                    GenerateNewMap();
                }
                else
                {
                    CurrentMap = map;
                    // player has not reached the boss yet, load the current map
                   // view.ShowMap(map);
                }
            }
            else
            {
                GenerateNewMap();
            }
            */
        }

        public Map GenerateNewMap()
        {
            Map map = MapGenerator.GetMap(config);
            Debug.Log(map.ToJson());
            return map;
        }
        public void SetCurrentMap(Map map)
        {
            CurrentMap = map;
        }

        public void SaveMap()
        {
            if (CurrentMap == null) return;

            var json = JsonConvert.SerializeObject(CurrentMap);
            PlayerPrefs.SetString("Map", json);
            PlayerPrefs.Save();
        }

        /*
        private void OnApplicationQuit()
        {
            SaveMap();
        }
        */

        // Save + Load Logic
        #region
        public void SaveMyDataToSaveFile(SaveGameData saveFile)
        {
            saveFile.map = CurrentMap.ToJson();
        }
        public void BuildMyDataFromSaveFile(SaveGameData saveFile)
        {
            CurrentMap = JsonConvert.DeserializeObject<Map>(saveFile.map);
        }
        #endregion
    }

}
