using DG.Tweening;
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
            StartCoroutine(RunCombatSceneSetup());
        }
        else if (GlobalSettings.Instance.gameMode == StartingSceneSetting.Standard)
        {
            StartCoroutine(RunStandardGameModeSetup());
        }
        else if (GlobalSettings.Instance.gameMode == StartingSceneSetting.CombatEndLootEvent)
        {
            StartCoroutine(RunCombatEndLootEventSetup());
        }
    }
    #endregion

    // Specific Game Set up style logic
    #region
    private IEnumerator RunStandardGameModeSetup()
    {
        // Set starting view state
        BlackScreenController.Instance.DoInstantFadeOut();
        MainMenuController.Instance.ShowFrontScreen();
        MainMenuController.Instance.frontScreenGuiCg.alpha = 0;
        MainMenuController.Instance.frontScreenBgParent.transform.DOScale(1.2f, 0f);
        yield return new WaitForSeconds(1);

        AudioManager.Instance.FadeInSound(Sound.Music_Main_Menu_Theme_1, 3f);
        BlackScreenController.Instance.FadeInScreen(2f);
        yield return new WaitForSeconds(2);

        MainMenuController.Instance.frontScreenBgParent.transform.DOKill();
        MainMenuController.Instance.frontScreenBgParent.transform.DOScale(1f, 1.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1f);

        MainMenuController.Instance.frontScreenGuiCg.DOFade(1f, 1f).SetEase(Ease.OutCubic);
    }
    private IEnumerator RunCombatSceneSetup()
    {
        yield return null;

        // Play battle theme music
        AudioManager.Instance.PlaySound(Sound.Music_Battle_Theme_1);

        // Build character data
        CharacterDataController.Instance.BuildAllCharactersFromCharacterTemplateList(GlobalSettings.Instance.testingCharacterTemplates);

        // Create player characters in scene
        CharacterEntityController.Instance.CreateAllPlayerCombatCharacters();

        // Spawn enemies
        EnemySpawner.Instance.SpawnEnemyWave("Basic", GlobalSettings.Instance.testingEnemyWave);

        // Start a new combat event
        ActivationManager.Instance.OnNewCombatEventStarted();
    }

    private IEnumerator RunCombatEndLootEventSetup()
    {
        yield return null;

        // Build character data
        CharacterDataController.Instance.BuildAllCharactersFromCharacterTemplateList(GlobalSettings.Instance.testingCharacterTemplates);

        // Create player characters in scene
        CharacterEntityController.Instance.CreateAllPlayerCombatCharacters();
        
        StartCombatVictorySequence();
    }
    #endregion

    // Setup from save file
    #region
    public void HandleStartNewGameFromMainMenuEvent()
    {
        // TO DO: should put a validation check here before running the game

        // Hide Main Menu
        MainMenuController.Instance.HideNewGameScreen();
        MainMenuController.Instance.HideFrontScreen();

        // CREATE NEW SAVE FILE

        // Set up characters
        PersistencyManager.Instance.BuildNewSaveFileOnNewGameStarted();

        // Build and prepare all session data
        PersistencyManager.Instance.SetUpGameSessionDataFromSaveFile();

        // Start the first encounter set up sequence
        // HandleLoadEncounter(JourneyManager.Instance.Encounters[0]);
        HandleLoadEncounter(JourneyManager.Instance.CurrentEncounter);
    }
    public void HandleLoadSavedGameFromMainMenuEvent()
    {
        // Hide Main Menu
        MainMenuController.Instance.HideFrontScreen();

        // Build and prepare all session data
        PersistencyManager.Instance.SetUpGameSessionDataFromSaveFile();

        // Load the encounter the player saved at
        HandleLoadEncounter(JourneyManager.Instance.CurrentEncounter);
    }

    #endregion

    // Scene Transitions
    #region
    public void HandleQuitToMainMenuFromInGame()
    {
        StartCoroutine(HandleQuitToMainMenuFromInGameCoroutine());
    }
    private IEnumerator HandleQuitToMainMenuFromInGameCoroutine()
    {
        // Do black screen fade out

        // Hide menus
        MainMenuController.Instance.HideInGameMenuView();

        // Wait for the current visual event to finish playing
        VisualEvent handle = VisualEventManager.Instance.HandleEventQueueTearDown();
        if (handle != null && handle.cData != null)
        {
            yield return new WaitUntil(() => handle.cData.CoroutineCompleted() == true);
        }

        // Destroy game scene
        HandleCombatSceneTearDown();

        // Stop battle music, start menu music
        AudioManager.Instance.FadeOutSound(Sound.Music_Battle_Theme_1, 1f);
        AudioManager.Instance.FadeInSound(Sound.Music_Main_Menu_Theme_1, 1f);

        // when black screen is totally faded out, wait for 2 seconds 
        // for untracked visual event coroutines to finish

        // Show menu screen
        MainMenuController.Instance.ShowFrontScreen();
        MainMenuController.Instance.RenderMenuButtons();

        // Do black screen fade in
    }
    #endregion

    // Load Encounters Logic
    #region
    public void HandleLoadEncounter(EncounterData encounter)
    {
        if (encounter.encounterType == EncounterType.BasicEnemy ||
            encounter.encounterType == EncounterType.EliteEnemy)
        {
            HandleLoadCombatEncounter(JourneyManager.Instance.CurrentEnemyWave);
        }
    }
    public void HandleLoadNextEncounter()
    {
        // TO DO: in future, there will be more sophisticated visual 
        // transistions from ecnounter to encounter, and the transisitions
        // will differ depending on the encounters they transisition from
        // and to (e.g. screen fade outs, moving characters off screen, etc).
        // The logic to decide which transisition will occur will go here.

        // Cache previous encounter data 
        EncounterData previousEncounter = JourneyManager.Instance.CurrentEncounter;
        EnemyWaveSO previousEnemyWave = JourneyManager.Instance.CurrentEnemyWave;

        // Increment world position
        JourneyManager.Instance.SetNextEncounterAsCurrentLocation();

        // Destroy all characters and activation windows if the 
        // previous encounter was a combat event
        if (previousEncounter.encounterType == EncounterType.BasicEnemy ||
            previousEncounter.encounterType == EncounterType.EliteEnemy)
        {
            // Mark wave as seen
            JourneyManager.Instance.AddEnemyWaveToAlreadyEncounteredList(previousEnemyWave);

            // Tear down scene
            HandleCombatSceneTearDown();
        }

        // If next event is a combat, get + set enemy wave before saving to disk
        if(JourneyManager.Instance.CurrentEncounter.encounterType == EncounterType.BasicEnemy ||
            JourneyManager.Instance.CurrentEncounter.encounterType == EncounterType.EliteEnemy)
        {
            // Calculate and cache the next enemy wave group
            JourneyManager.Instance.SetCurrentEnemyWaveData 
                (JourneyManager.Instance.GetRandomEnemyWaveFromEncounterData(JourneyManager.Instance.CurrentEncounter));

            // Auto save
            PersistencyManager.Instance.AutoUpdateSaveFile(SaveCheckPoint.CombatStart);

            HandleLoadCombatEncounter(JourneyManager.Instance.CurrentEnemyWave);
        }
    }
    private void HandleLoadCombatEncounter(EnemyWaveSO enemyWave)
    {
        // Play battle theme music
        AudioManager.Instance.FadeOutSound(Sound.Music_Main_Menu_Theme_1, 1f);
        AudioManager.Instance.FadeInSound(Sound.Music_Battle_Theme_1, 1f);

        // Create player characters in scene
        CharacterEntityController.Instance.CreateAllPlayerCombatCharacters();

        // Spawn enemies
        EnemySpawner.Instance.SpawnEnemyWave(enemyWave.combatDifficulty.ToString(), enemyWave);

        // Start a new combat event
        ActivationManager.Instance.OnNewCombatEventStarted();
    }
    #endregion

    // Handle Post Combat Stuff
    #region
    public void StartCombatVictorySequence()
    {
        StartCoroutine(StartCombatVictorySequenceCoroutine());
    }
    private IEnumerator StartCombatVictorySequenceCoroutine()
    {
        // wait until v queue count = 0
        yield return new WaitUntil(()=> VisualEventManager.Instance.EventQueue.Count == 0);

        AudioManager.Instance.FadeOutSound(Sound.Music_Battle_Theme_1, 1f);

        // fade out combat music
        // play victory music sfx
        // create victory pop up + firework particles + xp gain stuff (in future)

        // Disable any player characteer gui's if they're still active
        foreach (CharacterEntityModel model in CharacterEntityController.Instance.AllDefenders)
        {
            CharacterEntityController.Instance.FadeOutCharacterWorldCanvas(model.characterEntityView, null);
            CharacterEntityController.Instance.FadeOutCharacterUICanvas(model.characterEntityView, null);            
            if (model.characterEntityView.uiCanvasParent.activeSelf == true)
            {
                CharacterEntityController.Instance.FadeOutCharacterUICanvas(model.characterEntityView, null);
            }
        }

        // Destroy Activation windows
        ActivationManager.Instance.DestroyAllActivationWindows();

        // Hide end turn button
        UIManager.Instance.DisableEndTurnButtonView();

        // generate loot result
        LootController.Instance.SetAndCacheNewLootResult();
        LootController.Instance.BuildLootScreenElementsFromLootResultData();

        // save game state + cache loot result
        // fade in loot window
        LootController.Instance.ShowMainLootView();
        LootController.Instance.ShowFrontPageView();
    }
    #endregion

    // Handle Teardown Encounters
    #region
    public void HandleCombatSceneTearDown()
    {
        CharacterEntityController.Instance.DestroyAllCombatCharacters();
        ActivationManager.Instance.DestroyAllActivationWindows();
        LevelManager.Instance.ClearAndResetAllNodes();
        //UIManager.Instance.continueToNextEncounterButtonParent.SetActive(false);
        //UIManager.Instance.victoryPopup.SetActive(false);
        UIManager.Instance.DisableEndTurnButtonView();
    }
    #endregion
}
