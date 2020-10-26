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
    public void BuildNewSaveFileOnNewGameStarted()
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
        List<CharacterTemplateSO> chosenCharacters = new List<CharacterTemplateSO>();

        // should randomize character?
        if (MainMenuController.Instance.randomizeCharacters)
        {
            List<CharacterTemplateSO> randomCharacters = MainMenuController.Instance.GetThreeRandomAndDifferentTemplates();
            chosenCharacters.Add(randomCharacters[0]);
        }

        // else, just use player selected template
        else
        {
            chosenCharacters.Add(MainMenuController.Instance.GetChosenCharacter());
        }
        
        // build each character data object
        foreach (CharacterTemplateSO data in chosenCharacters)
        {
            newSave.characters.Add(CharacterDataController.Instance.ConverCharacterTemplateToCharacterData(data));
        }

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

                // Choose 10 random cards rom viable cards lists
                for(int i = 0; i < 10; i++)
                {
                    int randomIndex = RandomGenerator.NumberBetween(0, viableCards.Count - 1);
                    CardData randomCard = CardController.Instance.BuildCardDataFromScriptableObjectData(viableCards[randomIndex]);
                    CharacterDataController.Instance.AddCardToCharacterDeck(character, randomCard);
                    //character.deck.Add(randomCard);
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
                    //character.deck.Add(CardController.Instance.BuildCardDataFromScriptableObjectData(improviseCardData));
                }
            }
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
