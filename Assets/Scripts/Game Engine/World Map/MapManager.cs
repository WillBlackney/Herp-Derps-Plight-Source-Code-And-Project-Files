using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

namespace MapSystem
{
    public class MapManager : Singleton<MapManager>
    {
        // Components + Properties
        #region
        [Header("Components")]
        public MapConfig config;
        public MapView view;
        #endregion

        // Getters + Accessors
        #region
        public Map CurrentMap { get; private set; }
        #endregion

        // Generate + Set Map
        #region
        public Map GenerateNewMap()
        {
            Map map = MapGenerator.GetMap(config);
           // Debug.Log(map.ToJson());
            return map;
        }
        public void SetCurrentMap(Map map)
        {
            CurrentMap = map;
        }
        #endregion

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
