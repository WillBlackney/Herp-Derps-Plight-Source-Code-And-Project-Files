using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSequenceController : Singleton<EventSequenceController>
{
    // Game Start + Application Entry Points
    #region
    private void Start()
    {
        RunApplication();
    }
    private void RunApplication()
    {
        Debug.Log("CombatTestSceneController.Start() called...");

        // Establish settings

        // this prevents mobiles from sleeping due to inactivity
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // Start application type
        if (GlobalSettings.Instance.gameMode == StartingSceneSetting.CombatSceneSingle)
        {
            StartCoroutine(RunCombatSceneStartup());
        }
        else if (GlobalSettings.Instance.gameMode == StartingSceneSetting.Standard)
        {
            StartCoroutine(RunStandardGameModeSetup());
        }
    }
    #endregion

    // Specific Game Set up style logic
    #region
    private IEnumerator RunStandardGameModeSetup()
    {
        yield return null;

        MainMenuController.Instance.ShowFrontScreen();
    }
    private IEnumerator RunCombatSceneStartup()
    {
        yield return null;

        // Play battle theme music
        AudioManager.Instance.PlaySound(Sound.Music_Battle_Theme_1);

        // Build character data
        CharacterDataController.Instance.BuildAllCharactersFromCharacterTemplateList(GlobalSettings.Instance.testingCharacterTemplates);

        // Create player characters in scene
        CharacterEntityController.Instance.CreateAllPlayerCombatCharacters();
        //CreateTestingPlayerCharacters();

        // Spawn enemies
        EnemySpawner.Instance.SpawnEnemyWave("Basic", GlobalSettings.Instance.testingEnemyWave);

        // Start a new combat event
        ActivationManager.Instance.OnNewCombatEventStarted();

    }
    #endregion

    // Setup and Transitions from save files
    #region
    public void HandleStartNewGameFromMainMenuEvent()
    {
        // TO DO: should put a validation check here before running the game

        // Hide Main Menu
        MainMenuController.Instance.HideNewGameScreen();
        MainMenuController.Instance.HideFrontScreen();

        // Create new save file
        PersistencyManager.Instance.BuildNewSaveFileOnNewGameStarted(MainMenuController.Instance.GetChosenTemplatesFromChooseCharacterWindows());

        // Build and prepare all session data
        PersistencyManager.Instance.SetUpGameSessionDataFromSaveFile(PersistencyManager.Instance.CurrentSaveFile);

        // Start the first encounter set up sequence
        HandleLoadEncounter(JourneyManager.Instance.Encounters[0]);
    }
    public void HandleLoadSavedGameFromMainMenuEvent()
    {
        // Hide Main Menu
        MainMenuController.Instance.HideNewGameScreen();
        MainMenuController.Instance.HideFrontScreen();

        // Build and prepare all session data
        PersistencyManager.Instance.SetUpGameSessionDataFromSaveFile(PersistencyManager.Instance.CurrentSaveFile);

        // Start the first encounter set up sequence
        HandleLoadEncounter(JourneyManager.Instance.Encounters[JourneyManager.Instance.CurrentJourneyPosition]);
    }
    #endregion

    // Handle Specific Events
    #region
    public void HandleLoadEncounter(EncounterData encounter)
    {
        if (encounter.encounterType == EncounterType.BasicEnemy)
        {
            HandleLoadBasicCombatEncounter(encounter);
        }
        else if (encounter.encounterType == EncounterType.EliteEnemy)
        {
            HandleLoadEliteCombatEncounter(encounter);
        }
    }
    public void HandleLoadNextEncounter()
    {
        // TO DO: in future, there will be more sophisticated visual 
        // transistions from ecnounter to encounter, and the transisitions
        // will differ depending on the encounters they transisition from
        // and to (e.g. screen fade outs, moving characters off screen, etc).
        // The logic to decide which transisition will occur will go here.

        EncounterData previousEncounter = JourneyManager.Instance.GetCurrentEncounter();
        JourneyManager.Instance.SetJourneyPosition(JourneyManager.Instance.CurrentJourneyPosition + 1);

        // Destroy all characters and activation windows if the previous 
        // encounter was a combat event
        if (previousEncounter.encounterType == EncounterType.BasicEnemy ||
            previousEncounter.encounterType == EncounterType.EliteEnemy)
        {
            HandleCombatSceneTearDown();
        }

        HandleLoadEncounter(JourneyManager.Instance.GetCurrentEncounter());
    }
    private void HandleLoadBasicCombatEncounter(EncounterData encounter)
    {
        // Play battle theme music
        AudioManager.Instance.PlaySound(Sound.Music_Battle_Theme_1);

        // Create player characters in scene
        CharacterEntityController.Instance.CreateAllPlayerCombatCharacters();

        // Setup enemy wave data
        EnemyWaveSO eData = JourneyManager.Instance.GetRandomEnemyWaveFromEncounterData(encounter);

        // Mark wave as seen
        JourneyManager.Instance.AddEnemyWaveToAlreadyEncounteredList(eData);

        // Spawn enemies
        EnemySpawner.Instance.SpawnEnemyWave(eData.combatDifficulty.ToString(), eData);

        // Start a new combat event
        ActivationManager.Instance.OnNewCombatEventStarted();
    }
    private void HandleLoadEliteCombatEncounter(EncounterData encounter)
    {
        // Play battle theme music
        AudioManager.Instance.PlaySound(Sound.Music_Battle_Theme_1);

        // Create player characters in scene
        CharacterEntityController.Instance.CreateAllPlayerCombatCharacters();

        // Setup enemy wave data
        EnemyWaveSO eData = JourneyManager.Instance.GetRandomEnemyWaveFromEncounterData(encounter);

        // Mark wave as seen
        JourneyManager.Instance.AddEnemyWaveToAlreadyEncounteredList(eData);

        // Spawn enemies
        EnemySpawner.Instance.SpawnEnemyWave(eData.combatDifficulty.ToString(), eData);

        // Start a new combat event
        ActivationManager.Instance.OnNewCombatEventStarted();
    }
    #endregion

    // Handle Teardown Encounters
    #region
    public void HandleCombatSceneTearDown()
    {
        CharacterEntityController.Instance.DestroyAllCombatCharacters();
        ActivationManager.Instance.DestroyAllActivationWindows();
        LevelManager.Instance.ClearAndResetAllNodes();
        UIManager.Instance.continueToNextEncounterButtonParent.SetActive(false);
        UIManager.Instance.victoryPopup.SetActive(false);
    }
    #endregion
}
