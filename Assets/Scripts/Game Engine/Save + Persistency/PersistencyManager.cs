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

        // Clear any previous roster data
        CharacterDataController.Instance.ClearCharacterRoster();
        CharacterDataController.Instance.ClearCharacterDeck();

        // Build characters
        CharacterData chosenCharacter = MainMenuController.Instance.GetChosenCharacter();
        Debug.Log("BuildNewSaveFileOnNewGameStarted() chosen character = " + chosenCharacter.myName);

        // should randomize character?
        if (MainMenuController.Instance.randomizeCharacters)
        {
            List<CharacterData> randomCharacters = MainMenuController.Instance.GetThreeRandomAndDifferentTemplates();
            chosenCharacter = randomCharacters[0];
        }       

        // Create new character from selected data
        CharacterData newCharacter = CharacterDataController.Instance.CloneCharacterData(chosenCharacter);
        CharacterDataController.Instance.AutoAddCharactersRacialCard(newCharacter);
        CharacterDataController.Instance.AddCharacterToRoster(newCharacter);

        // Build each character data object
        /*
        foreach (CharacterData data in chosenCharacters)
        {
            // Create new character from data
            CharacterData newCharacter = CharacterDataController.Instance.CloneCharacterData(data);
            CharacterDataController.Instance.AutoAddCharactersRacialCard(newCharacter);
            CharacterDataController.Instance.AddCharacterToRoster(newCharacter);
        }*/

        // Build general data
        PlayerDataManager.Instance.ModifyCurrentGold(-PlayerDataManager.Instance.CurrentGold);
        PlayerDataManager.Instance.ModifyCurrentGold(GlobalSettings.Instance.startingGold);
        PlayerDataManager.Instance.SaveMyDataToSaveFile(newSave);

        // Build Camp site data
        CampSiteController.Instance.BuildPropertiesFromStandardSettings();
        CampSiteController.Instance.SaveMyDataToSaveFile(newSave);

        // Set up map
        MapManager.Instance.SetCurrentMap(MapManager.Instance.GenerateNewMap());
        MapManager.Instance.SaveMyDataToSaveFile(newSave);
        MapPlayerTracker.Instance.LockMap();

        // Set starting journey state
        newSave.currentJourneyPosition = 0;
        newSave.currentEncounter = EncounterType.KingsBlessingEvent;
        newSave.saveCheckPoint = SaveCheckPoint.KingsBlessingStart;

        // Generate KBC choices
        newSave.kbcChoices = KingsBlessingController.Instance.GenerateFourRandomChoices();

        // DECK MODIFIER SETUP
        // Randomize decks
        if (MainMenuController.Instance.randomizeDecks)
        {
            foreach(CharacterData character in newSave.characters)
            {
                // empty deck
                character.deck.Clear();

                // Get viable random cards
                List<CardDataSO> viableCards = new List<CardDataSO>();
                foreach(CardDataSO cardData in CardController.Instance.AllCardScriptableObjects)
                {
                    if(cardData.rarity != Rarity.None && cardData.talentSchool != TalentSchool.None)
                    {
                        viableCards.Add(cardData);
                    }
                }

                // Choose 9 random cards rom viable cards lists
                for(int i = 0; i < 9; i++)
                {
                    int randomIndex = RandomGenerator.NumberBetween(0, viableCards.Count - 1);
                    CardData randomCard = CardController.Instance.BuildCardDataFromScriptableObjectData(viableCards[randomIndex]);
                    CharacterDataController.Instance.AddCardToCharacterDeck(character, randomCard);
                }
            }
        }

        // Improvise decks
        else if (MainMenuController.Instance.improviseDecks)
        {
            foreach (CharacterData character in newSave.characters)
            {
                // empty deck
                character.deck.Clear();

                // Get improvise card
                CardDataSO improviseCardData = CardController.Instance.GetCardDataSOFromLibraryByName("Improvise");

                // Fill deck with 10 improvise cards
                for (int i = 0; i < 5; i++)
                {
                    CharacterDataController.Instance.AddCardToCharacterDeck(character, CardController.Instance.BuildCardDataFromScriptableObjectData(improviseCardData));
                }
            }
        }

        // Generate recruitable characters deck + save characters.
        CharacterDataController.Instance.AutoGenerateAndCacheNewCharacterDeck();
        CharacterDataController.Instance.SaveMyDataToSaveFile(newSave);

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
        JourneyManager.Instance.SaveMyDataToSaveFile(newSave);

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

        // Mystery + Story
        StoryEventController.Instance.SaveMyDataToSaveFile(newSave);

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
        JourneyManager.Instance.BuildMyDataFromSaveFile(newLoad);

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

        // Mystery + Story
        StoryEventController.Instance.BuildMyDataFromSaveFile(newLoad);
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
