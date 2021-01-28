using Newtonsoft.Json;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using MapSystem;

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
    public void BuildNewSaveFileOnNewGameStarted()
    {
        // Setup empty save file
        SaveGameData newSave = new SaveGameData();  

        // Build characters
        List<CharacterData> chosenCharacters = new List<CharacterData>();

        // should randomize character?
        if (MainMenuController.Instance.randomizeCharacters)
        {
            List<CharacterData> randomCharacters = MainMenuController.Instance.GetThreeRandomAndDifferentTemplates();
            chosenCharacters.Add(randomCharacters[0]);
        }

        // else, just use player selected template
        else
        {
            chosenCharacters.Add(MainMenuController.Instance.GetChosenCharacter());
        }       

        // Build each character data object
        foreach (CharacterData data in chosenCharacters)
        {
            // Create new character from data
            CharacterData newCharacter = CharacterDataController.Instance.CloneCharacterData(data);
            newSave.characters.Add(newCharacter);

            CharacterDataController.Instance.AutoAddCharactersRacialCard(newCharacter);
        }

        // TESTING: REMOVE LATER
        foreach (CharacterTemplateSO d in CharacterDataController.Instance.AllCharacterTemplatesSOs)
        {
            // Create new character from data
            CharacterData newCharacter = CharacterDataController.Instance.CloneCharacterData(CharacterDataController.Instance.ConvertCharacterTemplateToCharacterData(d));
            newSave.characters.Add(newCharacter);

            CharacterDataController.Instance.AutoAddCharactersRacialCard(newCharacter);
        }

        // Build general data
        PlayerDataManager.Instance.ModifyCurrentGold(GlobalSettings.Instance.startingGold);
        PlayerDataManager.Instance.SaveMyDataToSaveFile(newSave);
        CharacterDataController.Instance.SetMaxRosterSize(GlobalSettings.Instance.startingMaxRosterSize);

        // Generate first day data
        ProgressionController.Instance.SetDayNumber(1);
        ProgressionController.Instance.SetDailyCombatChoices(CombatGenerationController.Instance.GenerateWeeklyCombatChoices());
        ProgressionController.Instance.SaveMyDataToSaveFile(newSave);

        // generate recruitable characters



        // START SAVE!    
        SaveGameToDisk(newSave);
    }
    public void AutoUpdateSaveFile()
    {
        Debug.Log("PersistencyManager.AutoUpdateSaveFile() called...");

        // Setup empty save file
        SaveGameData newSave = new SaveGameData();
        newSave.characters = new List<CharacterData>();

        // Save character data
        CharacterDataController.Instance.SaveMyDataToSaveFile(newSave);

        // Save Player data
        PlayerDataManager.Instance.SaveMyDataToSaveFile(newSave);

        // Save journey data
        MapManager.Instance.SaveMyDataToSaveFile(newSave);

        // Save journey data
        ProgressionController.Instance.SaveMyDataToSaveFile(newSave);

        // Save recruit data
        RecruitCharacterController.Instance.SaveMyDataToSaveFile(newSave);

        // Save combat end loot data
        LootController.Instance.SaveMyDataToSaveFile(newSave);

        // Save camp properties
        CampSiteController.Instance.SaveMyDataToSaveFile(newSave);

        // KBC
        KingsBlessingController.Instance.SaveMyDataToSaveFile(newSave);

        // Shop
        ShopController.Instance.SaveMyDataToSaveFile(newSave);

        // Inventory
        InventoryController.Instance.SaveMyDataToSaveFile(newSave);

        // States
        StateController.Instance.SaveMyDataToSaveFile(newSave);
        ShrineController.Instance.SaveMyDataToSaveFile(newSave);

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

        // Build player data
        PlayerDataManager.Instance.BuildMyDataFromSaveFile(newLoad);

        // Build character data
        CharacterDataController.Instance.BuildMyDataFromSaveFile(newLoad);

        // Build Map data
        MapManager.Instance.BuildMyDataFromSaveFile(newLoad);

        // Set journey data
        ProgressionController.Instance.BuildMyDataFromSaveFile(newLoad);

        // Set recruit character event 
        RecruitCharacterController.Instance.BuildMyDataFromSaveFile(newLoad);

        // Set recruit character event 
        LootController.Instance.BuildMyDataFromSaveFile(newLoad);

        // Set up camp site data
        CampSiteController.Instance.BuildMyDataFromSaveFile(newLoad);

        // KBC
        KingsBlessingController.Instance.BuildMyDataFromSaveFile(newLoad);

        // Shop
        ShopController.Instance.BuildMyDataFromSaveFile(newLoad);

        // Inventory
        InventoryController.Instance.BuildMyDataFromSaveFile(newLoad);

        // States
        StateController.Instance.BuildMyDataFromSaveFile(newLoad);
        ShrineController.Instance.BuildMyDataFromSaveFile(newLoad);
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
    public void DeleteSaveFileOnDisk()
    {
        Debug.Log("PersistencyManager.DeleteSaveFileOnDisk() called");

        if (DoesSaveFileExist())
        {
            File.Delete(GetSaveFileDirectory());
        }
    }
    #endregion



}
