using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistencyManager : Singleton<PersistencyManager>
{
    // Properties + Getters
    #region
    private SaveGameData currentSaveFile;
    public SaveGameData CurrentSaveFile
    {
        get { return currentSaveFile; }
        private set { currentSaveFile = value; }
    }
    #endregion

    public void BuildNewSaveFileOnNewGameStarted(List<CharacterTemplateSO> characters)
    {
        CurrentSaveFile = new SaveGameData();

        // Build characters
        CurrentSaveFile.characters = new List<CharacterData>();
        foreach(CharacterTemplateSO data in characters)
        {
            CurrentSaveFile.characters.Add(CharacterDataController.Instance.ConverCharacterTemplateToCharacterData(data));
        }

        // Set journey start position
        CurrentSaveFile.currentJourneyPosition = 0;
    }
    public void SetUpGameSessionDataFromSaveFile(SaveGameData saveData)
    {
        // From here, we should set up all the values and data for 
        // the various controllers and managers

        // Build character data
        CharacterDataController.Instance.BuildAllDataFromSaveFile(CurrentSaveFile);

        // Set journey data
        JourneyManager.Instance.BuildDataFromSaveFile(CurrentSaveFile);

    }
}
