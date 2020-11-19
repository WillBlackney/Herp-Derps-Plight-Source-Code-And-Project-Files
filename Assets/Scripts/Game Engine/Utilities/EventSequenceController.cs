using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventSequenceController : Singleton<EventSequenceController>
{
    // Properties + Component References
    #region
    [SerializeField] private GameObject actNotifVisualParent;
    [SerializeField] private CanvasGroup actNotifCg;
    [SerializeField] private CanvasGroup actNotifTextParentCg;
    [SerializeField] private TextMeshProUGUI actNotifCountText;
    [SerializeField] private TextMeshProUGUI actNotifNameText;
    #endregion

    // Game Start + Application Entry Points
    #region
    private void Start()
    {
        RunApplication();
    }
    private void RunApplication()
    {
        Debug.Log("CombatTestSceneController.Start() called...");

        // this prevents mobiles from sleeping due to inactivity
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // Start application type
        if (GlobalSettings.Instance.gameMode == StartingSceneSetting.CombatSceneSingle)
        {
            StartCoroutine(RunCombatSceneTestSetup());
        }
        else if (GlobalSettings.Instance.gameMode == StartingSceneSetting.Standard)
        {
            StartCoroutine(RunStandardGameModeSetup());
        }
        else if (GlobalSettings.Instance.gameMode == StartingSceneSetting.CombatEndLootEvent)
        {
            StartCoroutine(RunCombatEndLootTestEventSetup());
        }
        else if (GlobalSettings.Instance.gameMode == StartingSceneSetting.RecruitCharacterEvent)
        {
            StartCoroutine(RunRecruitCharacterTestEventSetup());
        }
        else if (GlobalSettings.Instance.gameMode == StartingSceneSetting.KingsBlessingEvent)
        {
            StartCoroutine(RunKingsBlessingTestEventSetup());
        }
        else if (GlobalSettings.Instance.gameMode == StartingSceneSetting.CampSiteEvent)
        {
            StartCoroutine(RunCampSiteTestEventSetup());
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
    private IEnumerator RunCombatSceneTestSetup()
    {
        yield return null;

        // Play battle theme music
        AudioManager.Instance.AutoPlayBasicCombatMusic(1f);

        // Build character data
        CharacterDataController.Instance.BuildCharacterRosterFromCharacterTemplateList(GlobalSettings.Instance.testingCharacterTemplates);

        // Create player characters in scene
        CharacterEntityController.Instance.CreateAllPlayerCombatCharacters();

        // Enable world view
        LevelManager.Instance.EnableDungeonScenery();
        LevelManager.Instance.ShowAllNodeViews();

        // Spawn enemies
        EnemySpawner.Instance.SpawnEnemyWave("Basic", GlobalSettings.Instance.testingEnemyWave);
        JourneyManager.Instance.SetNextEncounterAsCurrentLocation();

        // Start a new combat event
        ActivationManager.Instance.OnNewCombatEventStarted();
    }
    private IEnumerator RunCombatEndLootTestEventSetup()
    {
        yield return null;

        // Build character data
        CharacterDataController.Instance.BuildCharacterRosterFromCharacterTemplateList(GlobalSettings.Instance.testingCharacterTemplates);

        // Create player characters in scene
        CharacterEntityController.Instance.CreateAllPlayerCombatCharacters();
        
        StartCombatVictorySequence(EncounterType.BasicEnemy);
    }
    private IEnumerator RunRecruitCharacterTestEventSetup()
    {
        yield return null;

        // Build character data
        CharacterDataController.Instance.BuildCharacterRosterFromCharacterTemplateList(GlobalSettings.Instance.testingCharacterTemplates);

        // Set up mock data
        EncounterData mockData = new EncounterData();
        mockData.encounterType = EncounterType.RecruitCharacter;
        RecruitCharacterController.Instance.currentChoices = RecruitCharacterController.Instance.GetThreeValidRecruitableCharacters();
        JourneyManager.Instance.SetCheckPoint(SaveCheckPoint.RecruitCharacterStart);

        // Load event
        HandleLoadEncounter(mockData);

    }
    private IEnumerator RunKingsBlessingTestEventSetup()
    {
        Debug.Log("EventSequenceController.RunKingsBlessingTestEventSetup()");

        yield return null;

        // Build character data
        CharacterDataController.Instance.BuildCharacterRosterFromCharacterTemplateList(GlobalSettings.Instance.testingCharacterTemplates);
        HandleLoadKingsBlessingEncounter();
    }
    private IEnumerator RunCampSiteTestEventSetup()
    {
        Debug.Log("EventSequenceController.RunCampSiteTestEventSetup()");

        yield return null;

        // Build character data
        CharacterDataController.Instance.BuildCharacterRosterFromCharacterTemplateList(GlobalSettings.Instance.testingCharacterTemplates);
        CampSiteController.Instance.BuildStartingCampSiteDeck(GlobalSettings.Instance.testingCampDeck);
        CampSiteController.Instance.SetStartingCampPointRegenStat(GlobalSettings.Instance.testingCampPoints);
        CampSiteController.Instance.SetStartingDrawStat(GlobalSettings.Instance.testingCampDraw);
        HandleLoadCampSiteEvent();
    }
    #endregion

    // Setup from save file
    #region
    public void HandleStartNewGameFromMainMenuEvent()
    {
        StartCoroutine(HandleStartNewGameFromMainMenuEventCoroutine());
    }
    public IEnumerator HandleStartNewGameFromMainMenuEventCoroutine()
    {      
        // Fade out screen + audio
        AudioManager.Instance.PlaySound(Sound.Events_New_Game_Started);
        AudioManager.Instance.FadeOutSound(Sound.Music_Main_Menu_Theme_1, 2f);
        BlackScreenController.Instance.FadeOutScreen(2f);
        yield return new WaitForSeconds(2f);

        // Set up characters
        PersistencyManager.Instance.BuildNewSaveFileOnNewGameStarted();

        // Build and prepare all session data
        PersistencyManager.Instance.SetUpGameSessionDataFromSaveFile();

        // Reset Camera
        CameraManager.Instance.ResetMainCameraPositionAndZoom();

        // Hide Main Menu
        MainMenuController.Instance.HideNewGameScreen();
        MainMenuController.Instance.HideFrontScreen();

        // Act start visual sequence
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.FadeInSound(Sound.Ambience_Outdoor_Spooky, 1f);
        yield return new WaitForSeconds(1f);
        PlayActNotificationVisualEvent();
        BlackScreenController.Instance.FadeInScreen(0f);
        yield return new WaitForSeconds(3.5f);

        // Start the first encounter set up sequence
        HandleLoadEncounter(JourneyManager.Instance.CurrentEncounter);        
    }
    public void HandleLoadSavedGameFromMainMenuEvent()
    {
        StartCoroutine(HandleLoadSavedGameFromMainMenuEventCoroutine());
    }
    private IEnumerator HandleLoadSavedGameFromMainMenuEventCoroutine()
    {
        // Build and prepare all session data
        PersistencyManager.Instance.SetUpGameSessionDataFromSaveFile();

        // Fade menu music
        if (AudioManager.Instance.IsSoundPlaying(Sound.Music_Main_Menu_Theme_1))
        {
            AudioManager.Instance.FadeOutSound(Sound.Music_Main_Menu_Theme_1, 2f);
        }

        // Fade out menu scren
        BlackScreenController.Instance.FadeOutScreen(2f);
        yield return new WaitForSeconds(2f);

        // Reset Camera
        CameraManager.Instance.ResetMainCameraPositionAndZoom();

        // Hide Main Menu
        MainMenuController.Instance.HideFrontScreen();

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
        Debug.LogWarning("HandleQuitToMainMenuFromInGameCoroutine");

        // Hide menus + GUI + misc annoying stuff
        MainMenuController.Instance.HideInGameMenuView();
        AudioManager.Instance.StopSound(Sound.Character_Footsteps);

        // Fade out battle music + ambience
        AudioManager.Instance.FadeOutAllCombatMusic(2f);
        AudioManager.Instance.FadeOutAllAmbience(2f);

        // Do black screen fade out
        BlackScreenController.Instance.FadeOutScreen(2f);

        // Wait for the current visual event to finish playing
        VisualEvent handle = VisualEventManager.Instance.HandleEventQueueTearDown();

        // Wait till its safe to tearn down event queue and scene
        yield return new WaitForSeconds(2f);
        AudioManager.Instance.ForceStopAllCombatMusic();

        if (handle != null && handle.cData != null)
        {
            yield return new WaitUntil(() => handle.cData.CoroutineCompleted() == true);
        }

        // Destroy game scene
        HandleCombatSceneTearDown();

        // Hide Recruit character screen
        RecruitCharacterController.Instance.ResetAllViews();

        // Hide Loot screen elements
        LootController.Instance.CloseAndResetAllViews();

        // Hide Kings Blessing screen elements
        LevelManager.Instance.DisableGraveyardScenery();
        KingsBlessingController.Instance.DisableUIView();

        // Hide camp site
        LevelManager.Instance.DisableCampSiteScenery();
        CampSiteController.Instance.DisableCharacterViewParent();
        CampSiteController.Instance.DisableCampGuiViewParent();

        // Fade in menu music
        AudioManager.Instance.FadeInSound(Sound.Music_Main_Menu_Theme_1, 1f);

        // Show menu screen
        MainMenuController.Instance.ShowFrontScreen();
        MainMenuController.Instance.RenderMenuButtons();

        // Do black screen fade in
        BlackScreenController.Instance.FadeInScreen(2f);
    }
    #endregion

    // Load Encounters Logic
    #region
    public void HandleLoadEncounter(EncounterData encounter)
    {
        Debug.LogWarning("EventSequenceController.HandleLoadEncounter()");

        if ((encounter.encounterType == EncounterType.BasicEnemy ||
            encounter.encounterType == EncounterType.EliteEnemy ||
            encounter.encounterType == EncounterType.BossEnemy) &&
            JourneyManager.Instance.CheckPointType == SaveCheckPoint.CombatStart
            )
        {
            HandleLoadCombatEncounter(JourneyManager.Instance.CurrentEnemyWave);
        }

        else if ((encounter.encounterType == EncounterType.BasicEnemy ||
            encounter.encounterType == EncounterType.EliteEnemy) &&
            JourneyManager.Instance.CheckPointType == SaveCheckPoint.CombatEnd
            )
        {
            BlackScreenController.Instance.FadeInScreen(1f);
            LevelManager.Instance.EnableDungeonScenery();
            LevelManager.Instance.ShowAllNodeViews();
            CharacterEntityController.Instance.CreateAllPlayerCombatCharacters();
            StartCombatVictorySequence(encounter.encounterType);
        }

        // TO DO IN FUTURE: boss loot events are different to basic/enemy, and 
        // will require different loading logic here
        else if ((encounter.encounterType == EncounterType.BossEnemy) &&
            JourneyManager.Instance.CheckPointType == SaveCheckPoint.CombatEnd
            )
        {
           
        }

        else if (JourneyManager.Instance.CheckPointType == SaveCheckPoint.RecruitCharacterStart)
        {
            HandleLoadRecruitCharacterEncounter();
        }

        else if (JourneyManager.Instance.CheckPointType == SaveCheckPoint.KingsBlessingStart)
        {
            HandleLoadKingsBlessingEncounter();
        }

        else if (JourneyManager.Instance.CheckPointType == SaveCheckPoint.CampSite)
        {
            HandleLoadCampSiteEvent();
        }
    }
    public void HandleLoadNextEncounter()
    {
        StartCoroutine(HandleLoadNextEncounterCoroutine());
    }
    public IEnumerator HandleLoadNextEncounterCoroutine()
    {
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

            // Fade out visual event
            BlackScreenController.Instance.FadeOutScreen(3f);

            // Fade and close loot screen views
            LootController.Instance.FadeOutMainLootView(()=> LootController.Instance.HideMainLootView());

            // Move characters off screen
            CharacterEntityController.Instance.MoveCharactersToOffScreenRight(CharacterEntityController.Instance.AllDefenders, null);           
            AudioManager.Instance.FadeOutSound(Sound.Environment_Camp_Fire, 3f);

            // Zoom and move camera & Fade foot steps
            yield return new WaitForSeconds(0.5f);
            AudioManager.Instance.FadeOutSound(Sound.Character_Footsteps, 2.5f);
            CameraManager.Instance.DoCameraMove(3, 0, 3f);
            CameraManager.Instance.DoCameraZoom(5, 3, 3f);

            // Wait for visual events
            yield return new WaitForSeconds(4f);

            // Tear down combat scene
            HandleCombatSceneTearDown();
        }

        // TO DO IN FUTURE: Boss victory means loading the next act, so we need
        // different loading/continuation logic here. We also need to end the game,
        // show score, etc, if the boss is last boss (act 3).
        else if (previousEncounter.encounterType == EncounterType.BossEnemy)
        {

        }

        // Tear down recruit character screen
                else if(previousEncounter.encounterType == EncounterType.RecruitCharacter)
        {
            // Fade out visual event
            BlackScreenController.Instance.FadeOutScreen(1f);
            yield return new WaitForSeconds(1f);

            // Teardown recruit event views
            RecruitCharacterController.Instance.ResetAllViews();
        }

        // Do kings blessing end sequence + tear down
        else if (previousEncounter.encounterType == EncounterType.KingsBlessingEvent)
        {
            CoroutineData kbcSequence = new CoroutineData();
            HandleLoadKingsBlessingContinueSequence(kbcSequence);
            yield return new WaitUntil(() => kbcSequence.CoroutineCompleted());
            LevelManager.Instance.DisableGraveyardScenery();
        }

        // Do camp site end sequence + tear down
        else if (previousEncounter.encounterType == EncounterType.CampSite)
        {
            List<CampSiteCharacterView> ccList = new List<CampSiteCharacterView>();
            ccList.AddRange(CampSiteController.Instance.AllCampSiteCharacterViews);

            // Fade and disable out GUI + views
            CampSiteController.Instance.FadeOutNodes();
            CampSiteController.Instance.FadeOutCampGui();
            CampSiteController.Instance.FadeOutAllCharacterGUI();
            yield return new WaitForSeconds(0.5f);
            CampSiteController.Instance.DisableCampGuiViewParent();            

            // Move characters off screen
            CampSiteController.Instance.MoveCharactersToOffScreenRight(ccList, null);
            AudioManager.Instance.FadeOutSound(Sound.Character_Footsteps, 3f);

            // Zoom and move camera
            yield return new WaitForSeconds(0.5f);
            CameraManager.Instance.DoCameraMove(3, 0, 3f);
            CameraManager.Instance.DoCameraZoom(5, 3, 3f);

            // Fade out Screen
            BlackScreenController.Instance.FadeOutScreen(3f);

            // Wait for visual events
            yield return new WaitForSeconds(4f);

            // Disable and close remaining views
            CampSiteController.Instance.DisableCharacterViewParent();
            LevelManager.Instance.DisableCampSiteScenery();
        }

        // Reset Camera
        CameraManager.Instance.ResetMainCameraPositionAndZoom();

        // If next event is a combat, get + set enemy wave before saving to disk
        if (JourneyManager.Instance.CurrentEncounter.encounterType == EncounterType.BasicEnemy ||
            JourneyManager.Instance.CurrentEncounter.encounterType == EncounterType.EliteEnemy ||
            JourneyManager.Instance.CurrentEncounter.encounterType == EncounterType.BossEnemy)
        {
            // Calculate and cache the next enemy wave group
            JourneyManager.Instance.SetCurrentEnemyWaveData 
                (JourneyManager.Instance.GetRandomEnemyWaveFromEncounterData(JourneyManager.Instance.CurrentEncounter));

            // Set check point
            JourneyManager.Instance.SetCheckPoint(SaveCheckPoint.CombatStart);

            // Auto save
            PersistencyManager.Instance.AutoUpdateSaveFile();

            // Start Load combat
            HandleLoadCombatEncounter(JourneyManager.Instance.CurrentEnemyWave);
        }

        // Recruit character event
        else if(JourneyManager.Instance.CurrentEncounter.encounterType == EncounterType.RecruitCharacter)
        {
            // Generate 3 random characters, if we are not loading from save
            if (RecruitCharacterController.Instance.currentChoices.Count == 0)
            {
                RecruitCharacterController.Instance.currentChoices = RecruitCharacterController.Instance.GetThreeValidRecruitableCharacters();
            }

            // Set check point
            JourneyManager.Instance.SetCheckPoint(SaveCheckPoint.RecruitCharacterStart);

            // Auto save
            PersistencyManager.Instance.AutoUpdateSaveFile();

            HandleLoadRecruitCharacterEncounter();
        }

        // Kings Blessing
        else if (JourneyManager.Instance.CurrentEncounter.encounterType == EncounterType.KingsBlessingEvent)
        {
            // Set check point
            JourneyManager.Instance.SetCheckPoint(SaveCheckPoint.KingsBlessingStart);

            // Auto save
            PersistencyManager.Instance.AutoUpdateSaveFile();

            HandleLoadKingsBlessingEncounter();
        }

        // Camp site
        else if (JourneyManager.Instance.CurrentEncounter.encounterType == EncounterType.CampSite)
        {
            // Set check point
            JourneyManager.Instance.SetCheckPoint(SaveCheckPoint.CampSite);

            // Auto save
            PersistencyManager.Instance.AutoUpdateSaveFile();

            HandleLoadCampSiteEvent();
        }
    }
    private void HandleLoadCombatEncounter(EnemyWaveSO enemyWave)
    {
        // Enable ambience if not playing
        if (!AudioManager.Instance.IsSoundPlaying(Sound.Ambience_Crypt))
        {
            AudioManager.Instance.FadeInSound(Sound.Ambience_Crypt, 1f);
        }

        // Enable world BG view + Node visbility
        LevelManager.Instance.EnableDungeonScenery();
        LevelManager.Instance.ShowAllNodeViews();

        // Fade In
        BlackScreenController.Instance.FadeInScreen(1f);

        // Camera Zoom out effect
        CameraManager.Instance.DoCameraZoom(4, 5, 1);

        // Play battle music
        AudioManager.Instance.FadeOutSound(Sound.Music_Main_Menu_Theme_1, 1f);
        if(enemyWave.combatDifficulty == CombatDifficulty.Basic)
        {
            AudioManager.Instance.AutoPlayBasicCombatMusic(1f);
        }
        else if (enemyWave.combatDifficulty == CombatDifficulty.Elite)
        {
            AudioManager.Instance.AutoPlayEliteCombatMusic(1f);
        }

        // Create player characters in scene
        CharacterEntityController.Instance.CreateAllPlayerCombatCharacters();

        // Spawn enemies
        EnemySpawner.Instance.SpawnEnemyWave(enemyWave.combatDifficulty.ToString(), enemyWave);

        // move characters offscreen
        CharacterEntityController.Instance.MoveAllCharactersToOffScreenPosition();

        // make characters move towards start nodes
        CoroutineData cData = new CoroutineData();
        VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.MoveAllCharactersToStartingNodes(cData));       

        // Start a new combat event
        ActivationManager.Instance.OnNewCombatEventStarted();
    }
    private void HandleLoadRecruitCharacterEncounter()
    {
        // Enable ambience if not playing
        if (!AudioManager.Instance.IsSoundPlaying(Sound.Ambience_Crypt))
        {
            AudioManager.Instance.FadeInSound(Sound.Ambience_Crypt, 1f);
        }

        // Fade in
        BlackScreenController.Instance.FadeInScreen(2f);

        // Build + Show views
        RecruitCharacterController.Instance.ResetAllViews();
        RecruitCharacterController.Instance.ShowRecruitCharacterScreen();
        RecruitCharacterController.Instance.BuildRecruitCharacterWindows();
    }
    private void HandleLoadCampSiteEvent()
    {
        StartCoroutine(HandleLoadCampSiteEventCoroutine());
    }
    private IEnumerator HandleLoadCampSiteEventCoroutine()
    {
        // build camp site character data and views from character roster characters
        // move characters to start position (off screen)
        // fade in screen
        // move characters to camp site positions
        // on arrival, fade in character gui (health bars, etc)
        // then fade in card gui, delay a tad, then camp site cards

        // Set Properties
        CampSiteController.Instance.PopulateDrawPile();
        CampSiteController.Instance.GainCampPointsOnNewCampEventStart();

        // View sequence 1
        LevelManager.Instance.EnableCampSiteScenery();
        CampSiteController.Instance.EnableCharacterViewParent();
        CampSiteController.Instance.BuildAllCampSiteCharacterViews(CharacterDataController.Instance.AllPlayerCharacters);
        CampSiteController.Instance.SetAllCampSiteCharacterViewStartStates();
        CampSiteController.Instance.MoveAllCharactersToStartPosition();
        AudioManager.Instance.FadeInSound(Sound.Environment_Camp_Fire, 3f);

        // View sequence 2
        BlackScreenController.Instance.FadeInScreen(1f);
        CameraManager.Instance.DoCameraMove(-3, 0, 0f);
        CameraManager.Instance.DoCameraMove(0, 0, 2f);
        CameraManager.Instance.DoCameraZoom(3, 5, 2f);
        CampSiteController.Instance.MoveAllCharactersToTheirNodes();
        yield return new WaitForSeconds(1 + (CharacterDataController.Instance.AllPlayerCharacters.Count * 0.5f));
        CampSiteController.Instance.EnableCampGuiViewParent();
        CampSiteController.Instance.FadeInAllCharacterGUI();
        CampSiteController.Instance.FadeInNodes();        
        CampSiteController.Instance.FadeInCampGui();
        yield return new WaitForSeconds(0.5f);

        // Draw camp cards
        CampSiteController.Instance.DrawCampCardsOnCampEventStart();
    }
    private void HandleLoadKingsBlessingEncounter()
    {
        StartCoroutine(HandleLoadKingsBlessingEncounterCoroutine());
    }
    private IEnumerator HandleLoadKingsBlessingEncounterCoroutine()
    {
        // Generate random KBC choices, if not loading from a save file
        if(KingsBlessingController.Instance.CurrentChoices.Count == 0)
        {
            Debug.LogWarning("EventSequenceController.HandleLoadKingsBlessingEncounter() detected that 4 KBC choices" +
                " have not already been created and saved to disk, generating new ones now...");
            KingsBlessingController.Instance.SetCharacterChoices(KingsBlessingController.Instance.GenerateFourRandomChoices());
        }
        KingsBlessingController.Instance.BuildAllChoiceButtonViewsFromActiveChoices();

        // Play ambience if not already playing
        if (!AudioManager.Instance.IsSoundPlaying(Sound.Ambience_Outdoor_Spooky))
        {
            AudioManager.Instance.FadeInSound(Sound.Ambience_Outdoor_Spooky, 1f);
        }
        BlackScreenController.Instance.FadeInScreen(1.5f);

        // PREPARE KBC VIEW
        // Disable dungeon view
        LevelManager.Instance.DisableDungeonScenery();

        // Build kbc views
        LevelManager.Instance.EnableGraveyardScenery();
        KingsBlessingController.Instance.BuildAllViews(CharacterDataController.Instance.AllPlayerCharacters[0]);
        KingsBlessingController.Instance.EnableUIView();

        // Set Camera start settings
        CameraManager.Instance.DoCameraZoom(2, 2, 0f);
        CameraManager.Instance.DoCameraMove(-5, -3, 0f);

        // Start moving player model + animate king
        KingsBlessingController.Instance.PlayKingFloatAnim();
        KingsBlessingController.Instance.DoPlayerModelMoveToMeetingSequence();
        yield return new WaitForSeconds(0.5f);

        // Move + Zoom out camera
        CameraManager.Instance.DoCameraZoom(2, 5, 2f);
        CameraManager.Instance.DoCameraMove(0, 0, 2f);
        yield return new WaitForSeconds(1.5f);

        // King greeting
        KingsBlessingController.Instance.DoKingGreeting();
        yield return new WaitForSeconds(1.5f);

        // Fade in choice buttons + health bar
        KingsBlessingController.Instance.FadeInChoiceButtons();
        KingsBlessingController.Instance.FadeHealthBar(1, 1);

    }
    public void HandleLoadKingsBlessingContinueSequence(CoroutineData cData)
    {
        StartCoroutine(HandleLoadKingsBlessingContinueSequenceCoroutine(cData));
    }
    private IEnumerator HandleLoadKingsBlessingContinueSequenceCoroutine(CoroutineData cData)
    {
        // Fade out continue button + health bar.
        KingsBlessingController.Instance.FadeContinueButton(0, 0.5f, true);
        KingsBlessingController.Instance.FadeHealthBar(0, 0.5f, true);

        // King greeting
        KingsBlessingController.Instance.DoKingFarewell();

        // Open door visual sequence
        KingsBlessingController.Instance.DoDoorOpeningSequence();
        yield return new WaitForSeconds(1.75f);

        // start camera zoom + movement here
        CameraManager.Instance.DoCameraZoom(5, 2, 1.5f);
        CameraManager.Instance.DoCameraMove(0, 2, 1.5f);

        // Player moves towards door visual sequence
        KingsBlessingController.Instance.DoPlayerMoveThroughEntranceSequence();
        yield return new WaitForSeconds(1.5f);

        // Fade out outdoor ambience
        AudioManager.Instance.FadeOutSound(Sound.Ambience_Outdoor_Spooky, 1f);

        // Black screen fade out start here
        BlackScreenController.Instance.FadeOutScreen(1f);
        yield return new WaitForSeconds(1f);

        KingsBlessingController.Instance.DisableUIView();

        if (cData != null)
        {
            cData.MarkAsCompleted();
        }
    }
    #endregion

    // Handle Post Combat Stuff
    #region
    public void StartCombatVictorySequence(EncounterType combatType)
    {
        StartCoroutine(StartCombatVictorySequenceCoroutine(combatType));
    }
    private IEnumerator StartCombatVictorySequenceCoroutine(EncounterType combatType)
    {
        // wait until v queue count = 0
        yield return new WaitUntil(()=> VisualEventManager.Instance.EventQueue.Count == 0);

        AudioManager.Instance.FadeOutAllCombatMusic(1f);

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

        // Do post combat mini event + reward xp
        if (JourneyManager.Instance.CheckPointType != SaveCheckPoint.CombatEnd)
        {
            // Cache previous xp of characters for visual events
            List<PreviousXpState> pxsList = new List<PreviousXpState>();
            foreach(CharacterData character in CharacterDataController.Instance.AllPlayerCharacters)
            {
                pxsList.Add(new PreviousXpState(character));
            }

            // Grant xp
            CharacterDataController.Instance.HandleXpRewardPostCombat(combatType);
            
            // Start visual event
            LootController.Instance.PlayNewXpRewardVisualEvent(pxsList);            
        }

        // Generate loot result + Auto save
        if (JourneyManager.Instance.CheckPointType != SaveCheckPoint.CombatEnd)
        {
            // Generate loot
            LootController.Instance.SetAndCacheNewLootResult();

            // Save and set checkpoint + cache loot result
            JourneyManager.Instance.SetCheckPoint(SaveCheckPoint.CombatEnd);
            PersistencyManager.Instance.AutoUpdateSaveFile();

            // Wait for xp reward v event to finish
            yield return new WaitForSeconds(3f);
        }       

        // Build loot screen views
        LootController.Instance.BuildLootScreenElementsFromLootResultData();        

        // fade in loot window
        LootController.Instance.FadeInMainLootView();
        LootController.Instance.ShowFrontPageView();
    }
    #endregion

    // Handle Teardown Encounters
    #region
    public void HandleCombatSceneTearDown()
    {
        CharacterEntityController.Instance.DestroyCharacterViewModelsAndGameObjects(CharacterEntityController.Instance.AllCharacters);
        CharacterEntityController.Instance.ClearAllCharacterPersistencies();
        ActivationManager.Instance.DestroyAllActivationWindows();
        LevelManager.Instance.ClearAndResetAllNodes();
        LevelManager.Instance.HideAllNodeViews();
        LevelManager.Instance.DisableDungeonScenery();
        UIManager.Instance.DisableEndTurnButtonView();
    }
    #endregion

    // Misc Visual Stuff
    #region
    public void PlayActNotificationVisualEvent()
    {
        StartCoroutine(PlayActNotificationVisualEventCoroutine());
    }
    private IEnumerator PlayActNotificationVisualEventCoroutine()
    {
        // Enable ambience if not playing
        if (!AudioManager.Instance.IsSoundPlaying(Sound.Ambience_Outdoor_Spooky))
        {
            AudioManager.Instance.FadeInSound(Sound.Ambience_Outdoor_Spooky, 1f);
        }

        // Set up
        ResetActNotificationViews();
        actNotifCountText.text = "Act One";
        actNotifNameText.text = "Into The Crypt...";
        actNotifVisualParent.SetActive(true);
        actNotifCg.alpha = 1;
        actNotifTextParentCg.alpha = 1;

        // Fade in act count text
        actNotifCountText.gameObject.SetActive(true);
        actNotifCountText.DOFade(1, 1);

        // Wait for player to read
        yield return new WaitForSeconds(1.5f);

        // Fade in act NAME text
        actNotifNameText.gameObject.SetActive(true);
        actNotifNameText.DOFade(1, 1);

        // Wait for player to read
        yield return new WaitForSeconds(1.5f);

        // Fade out text quickly
        actNotifTextParentCg.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);

        // Fade out entire view
        actNotifCg.DOFade(0, 1f);
        yield return new WaitForSeconds(1);
        ResetActNotificationViews();
    }
    private void ResetActNotificationViews()
    {
        actNotifCountText.DOFade(0, 0);
        actNotifNameText.DOFade(0, 0);
        actNotifNameText.gameObject.SetActive(false);
        actNotifCountText.gameObject.SetActive(false);
        actNotifVisualParent.SetActive(false);

    }
    #endregion
}
