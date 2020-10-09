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
    public string GetSaveFileDirectory()
    {
        return Application.persistentDataPath + SAVE_DIRECTORY;
    }
    #endregion

    // Conditionals + Checks
    #region
    public bool DoesSaveFileExist()
    {
        if (File.Exists(GetSaveFileDirectory()))
        {
            Debug.Log("PersistencyManager.DoesSaveFileExist() confirmed save file exists, returning true");
            return true;
        }
        else
        {
            Debug.Log("PersistencyManager.DoesSaveFileExist() could not find the save file, returning false");
            return false;
        }
    }
    #endregion

    // Build Save Files Data
    #region
    public void BuildNewSaveFileOnNewGameStarted(List<CharacterTemplateSO> characters)
    {
        // Setup empty save file
        SaveGameData newSave = new SaveGameData();

        // Set starting journey state
        EncounterData ed = JourneyManager.Instance.Encounters[0];

        if (ed.encounterType == EncounterType.BasicEnemy)
        {
            newSave.saveCheckPoint = SaveCheckPoint.CombatStart;
            newSave.currentEncounter = ed;
            newSave.currentEnemyWave = JourneyManager.Instance.GetRandomEnemyWaveFromEncounterData(ed).encounterName;
        }

        // Set journey start position
        newSave.currentJourneyPosition = 0;

        // Build characters
        foreach (CharacterTemplateSO data in characters)
        {
            newSave.characters.Add(CharacterDataController.Instance.ConverCharacterTemplateToCharacterData(data));
        }

        // START SAVE!        
        SaveGameToDisk(newSave);
    }
    public void AutoUpdateSaveFile(SaveCheckPoint checkPointType)
    {
        Debug.Log("PersistencyManager.AutoUpdateSaveFile() called, new check point: " + checkPointType.ToString());

        // Setup empty save file
        SaveGameData newSave = new SaveGameData();
        newSave.characters = new List<CharacterData>();

        // Set checkpoint info
        newSave.saveCheckPoint = checkPointType;

        // Save character data
        CharacterDataController.Instance.SaveMyDataToSaveFile(newSave);

        // Save journey data
        JourneyManager.Instance.SaveMyDataToSaveFile(newSave);

        // START SAVE!        
        SaveGameToDisk(newSave);
    }

    #endregion

    // Build Session Data
    #region
    public void SetUpGameSessionDataFromSaveFile()
    {
        // Build save file from persistency
        SaveGameData newLoad = LoadGameFromDisk();        

        // Build character data
        CharacterDataController.Instance.BuildMyDataFromSaveFile(newLoad);

        // Set journey data
        JourneyManager.Instance.BuildMyDataFromSaveFile(newLoad);
    }
    #endregion

    // Save, Load and Delete From Disk 
    #region
    private void SaveGameToDisk(SaveGameData saveFile)
    {
        byte[] bytes = SerializationUtility.SerializeValue(saveFile, DataFormat.Binary);
        File.WriteAllBytes(GetSaveFileDirectory(), bytes);
    }
    private SaveGameData LoadGameFromDisk()
    {
        SaveGameData newLoad;
        byte[] bytes = File.ReadAllBytes(GetSaveFileDirectory());
        newLoad = SerializationUtility.DeserializeValue<SaveGameData>(bytes, DataFormat.Binary);
        return newLoad;
    }
    private void DeleteSaveFileOnDisk()
    {
        Debug.Log("PersistencyManager.DeleteSaveFileOnDisk() called");

        if (DoesSaveFileExist())
        {
            File.Delete(GetSaveFileDirectory());
        }
    }
    #endregion



}
