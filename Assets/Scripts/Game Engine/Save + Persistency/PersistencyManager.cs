using Newtonsoft.Json;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PersistencyManager : Singleton<PersistencyManager>
{

    // Properties + Getters
    #region
    public const string SAVE_DIRECTORY = "/Save Folder/SaveFile.json";
    public string GetSaveDirectory()
    {
        return Application.dataPath + SAVE_DIRECTORY;
    }
    #endregion

    public void BuildNewSaveFileOnNewGameStarted(List<CharacterTemplateSO> characters)
    {
        //Debug.LogWarning("PersistencyManager.BuildNewSaveFileOnNewGameStarted() called...");

        // Setup empty save file
        SaveGameData newSave = new SaveGameData();
        newSave.characters = new List<CharacterData>();

        // BUILD SAVE FILE!
        // Build characters
        foreach (CharacterTemplateSO data in characters)
        {
            newSave.characters.Add(CharacterDataController.Instance.ConverCharacterTemplateToCharacterData(data));
        }             

        // Set journey start position
        newSave.currentJourneyPosition = 0;

        // START SAVE!        
        SaveGameToDisk(newSave);

    }
    public void SetUpGameSessionDataFromSaveFile()
    {
       // Debug.LogWarning("PersistencyManager.SetUpGameSessionDataFromSaveFile() called...");

        SaveGameData newLoad = LoadGameFromDisk();

        // Debug.LogWarning("SaveGameData file properties after load: ");
        // Debug.LogWarning("currentJourneyPosition: " + newLoad.currentJourneyPosition);
        // Debug.LogWarning("total characters: " + newLoad.characters.Count.ToString());
        // Debug.LogWarning("Character 1, card 1 description: " + newLoad.characters[0].deck[0].cardDescription);

        // Build character data
        CharacterDataController.Instance.BuildAllDataFromSaveFile(newLoad);

        // Set journey data
        JourneyManager.Instance.BuildDataFromSaveFile(newLoad);
    }

    // Save + Load From Disk/Text File
    #region
    private void SaveGameToDiskAsJsonTextFile(string saveStringJson)
    {
       // Debug.LogWarning("PersistencyManager.SaveGameToDiskAsJsonTextFile() called...");
        File.WriteAllText(GetSaveDirectory(), saveStringJson);
    }
    private SaveGameData LoadGameFromDisk()
    {
       // Debug.LogWarning("PersistencyManager.LoadGameFromDisk() called...");

        SaveGameData newLoad;

        byte[] bytes = File.ReadAllBytes(GetSaveDirectory());
        newLoad = SerializationUtility.DeserializeValue<SaveGameData>(bytes, DataFormat.Binary);
        return newLoad;

        //return File.ReadAllText(GetSaveDirectory());
    }
    #endregion

    // Save File Conversion
    #region
    private void SaveGameToDisk(SaveGameData saveFile)
    {
        //Debug.LogWarning("PersistencyManager.SaveGameToDisk() called...");

        byte[] bytes = SerializationUtility.SerializeValue(saveFile, DataFormat.Binary);
        File.WriteAllBytes(GetSaveDirectory(), bytes);

        //return JsonConvert.SerializeObject(saveFile);
    }
    #endregion


}
