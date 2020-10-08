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
    public const string SAVE_DIRECTORY = "/SaveFile.json";
    public string GetSaveDirectory()
    {
        return Application.persistentDataPath + SAVE_DIRECTORY;
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
        // Build save file from persistency
        SaveGameData newLoad = LoadGameFromDisk();

        // Build character data
        CharacterDataController.Instance.BuildAllDataFromSaveFile(newLoad);

        // Set journey data
        JourneyManager.Instance.BuildDataFromSaveFile(newLoad);
    }

    // Save + Load From Disk/Text File
    #region
    private SaveGameData LoadGameFromDisk()
    {
        SaveGameData newLoad;

        byte[] bytes = File.ReadAllBytes(GetSaveDirectory());
        newLoad = SerializationUtility.DeserializeValue<SaveGameData>(bytes, DataFormat.Binary);
        return newLoad;
    }
    #endregion

    // Save File Conversion
    #region
    private void SaveGameToDisk(SaveGameData saveFile)
    {
        byte[] bytes = SerializationUtility.SerializeValue(saveFile, DataFormat.Binary);
        File.WriteAllBytes(GetSaveDirectory(), bytes);
    }
    #endregion


}
