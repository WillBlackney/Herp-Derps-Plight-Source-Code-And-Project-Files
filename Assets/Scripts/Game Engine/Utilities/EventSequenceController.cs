using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MapSystem;

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
        else if (GlobalSettings.Instance.gameMode == StartingSceneSetting.JourneyStartTest)
        {
            HandleStartNewGameFromMainMenuEvent();
        }
    }
    #endregion

    // Specific Game Set up style logic
    #region
    private IEnumerator RunStandardGameModeSetup()
    {
        // Set starting view state
        TopBarController.Instance.HideTopBar();
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

        // Enable GUI
        TopBarController.Instance.ShowTopBar();

        // Populate inventory with mock cards and items
        InventoryController.Instance.PopulateInventoryWithMockCardData(20);
        InventoryController.Instance.PopulateInventoryWitMockItemData();

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
       // ProgressionController.Instance.IncrementWorldMapPosition();

        // Start a new combat event
        ActivationManager.Instance.OnNewCombatEventStarted();
    }
    private IEnumerator RunCombatEndLootTestEventSetup()
    {
        yield return null;

        // Enable GUI
        TopBarController.Instance.ShowTopBar();

        // Build character data
        CharacterDataController.Instance.BuildCharacterRosterFromCharacterTemplateList(GlobalSettings.Instance.testingCharacterTemplates);

        // Create player characters in scene
        CharacterEntityController.Instance.CreateAllPlayerCombatCharacters();

       // ProgressionController.Instance.IncrementWorldMapPosition();

        StartCombatVictorySequence(CombatDifficulty.Basic);
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

        // Set up top bar
        TopBarController.Instance.ShowTopBar();
        TopBarController.Instance.ShowNavigationButton();
        TopBarController.Instance.SetNavigationButtonText("To Arena");

        // Set up characters
        PersistencyManager.Instance.BuildNewSaveFileOnNewGameStarted();

        // Build and prepare all session data
        PersistencyManager.Instance.SetUpGameSessionDataFromSaveFile();

        // Reset Camera
        CameraManager.Instance.ResetMainCameraPositionAndZoom();

        // Hide Main Menu
        MainMenuController.Instance.HideNewGameScreen();
        MainMenuController.Instance.HideFrontScreen();

        // Setup town view
        TownViewController.Instance.ShowMainTownView();
        TownViewController.Instance.SetScreenViewState(ScreenViewState.Town);
        
        // Set up character panel views
        CharacterPanelViewController.Instance.ShowCharacterRosterPanel();
        CharacterPanelViewController.Instance.RebuildAllViews();

        // Reset chosen combat character slots + data
        TownViewController.Instance.ClearAllSelectedCombatCharactersAndSlots();

        // Act start visual sequence
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.FadeInSound(Sound.Ambience_Outdoor_Spooky, 1f);
        yield return new WaitForSeconds(1f);
        BlackScreenController.Instance.FadeInScreen(1f);       
    }
    public void HandleLoadSavedGameFromMainMenuEvent()
    {
        StartCoroutine(HandleLoadSavedGameFromMainMenuEventCoroutine());
    }
    private IEnumerator HandleLoadSavedGameFromMainMenuEventCoroutine()
    {     
        // Fade menu music
        if (AudioManager.Instance.IsSoundPlaying(Sound.Music_Main_Menu_Theme_1))
        {
            AudioManager.Instance.FadeOutSound(Sound.Music_Main_Menu_Theme_1, 2f);
        }

        // Fade out menu scren
        BlackScreenController.Instance.FadeOutScreen(2f);
        yield return new WaitForSeconds(2f);

        // Build and prepare all session data
        PersistencyManager.Instance.SetUpGameSessionDataFromSaveFile();

        // Enable GUI
        TopBarController.Instance.ShowTopBar();

        // Reset Camera
        CameraManager.Instance.ResetMainCameraPositionAndZoom();

        // Hide Main Menu
        MainMenuController.Instance.HideFrontScreen();

        // Reset chosen combat character slots + data
        TownViewController.Instance.ClearAllSelectedCombatCharactersAndSlots();

        // if in town, load into town
        if (ProgressionController.Instance.CheckPointType == SaveCheckPoint.TownDayStart)
        {
            // Setup town view
            TownViewController.Instance.ShowMainTownView();
            TownViewController.Instance.SetScreenViewState(ScreenViewState.Town);

            // Set up character panel views
            CharacterPanelViewController.Instance.ShowCharacterRosterPanel();
            CharacterPanelViewController.Instance.RebuildAllViews();

            // Act start visual sequence
            yield return new WaitForSeconds(0.5f);
            AudioManager.Instance.FadeInSound(Sound.Ambience_Outdoor_Spooky, 1f);
            yield return new WaitForSeconds(1f);
            BlackScreenController.Instance.FadeInScreen(1f);
        }

        // if at combat start, load combat start sequence
        if (ProgressionController.Instance.CheckPointType == SaveCheckPoint.CombatStart)
        {
            HandleLoadCombatEncounter(ProgressionController.Instance.CurrentCombatData, ProgressionController.Instance.ChosenCombatCharacters);
        }

        // if at combat end event, load combat end.
        if (ProgressionController.Instance.CheckPointType == SaveCheckPoint.CombatEnd)
        {
            // Build level + character views
            LevelManager.Instance.EnableDungeonScenery();
            LevelManager.Instance.ShowAllNodeViews();
            CharacterEntityController.Instance.CreateAllPlayerCombatCharacters();

            // Build and show loot screen views
            LootController.Instance.BuildLootScreenElementsFromLootResultData();
            LootController.Instance.FadeInMainLootView();
            LootController.Instance.ShowFrontPageView();

            // Fade in
            BlackScreenController.Instance.FadeInScreen(1f);
        }
               

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

        // Hide Game GUI
        TopBarController.Instance.HideTopBar();

        // Brute force stop all game music
        AudioManager.Instance.ForceStopAllCombatMusic();

        if (handle != null && handle.cData != null)
        {
            yield return new WaitUntil(() => handle.cData.CoroutineCompleted() == true);
        }

        // Destroy combat scene
        HandleCombatSceneTearDown();

        // Hide town/in-game UI
        CharacterPanelViewController.Instance.HideCharacterRosterPanel();
        TownViewController.Instance.HideArenaScreen();
        TownViewController.Instance.HideChosenCharacterSlots();
        TownViewController.Instance.HideMainTownView();

        // Hide world map + roster
        //MapView.Instance.HideMainMapView();
        //CharacterRosterViewController.Instance.DisableMainView();

        // Hide Recruit character screen
        // RecruitCharacterController.Instance.ResetAllViews();

        // Hide Loot screen elements
        // LootController.Instance.CloseAndResetAllViews();

        // Hide Kings Blessing screen elements

        // LevelManager.Instance.DisableGraveyardScenery();
        // KingsBlessingController.Instance.DisableUIView();

        // Hide camp site
        //LevelManager.Instance.DisableCampSiteScenery();
       // CampSiteController.Instance.DisableCharacterViewParent();
       // CampSiteController.Instance.DisableCampGuiViewParent();

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
    public void HandleStartCombatFromChooseCombatScreen()
    {
        StartCoroutine(HandleStartCombatFromChooseCombatScreenCoroutine());
    }
    private IEnumerator HandleStartCombatFromChooseCombatScreenCoroutine()
    {
        yield return null;
        BlackScreenController.Instance.FadeOutScreen(1f);
        yield return new WaitForSeconds(1f);

        // Hide all town views
        TownViewController.Instance.HideChosenCharacterSlots();
        TownViewController.Instance.HideArenaScreen();
        TownViewController.Instance.HideMainTownView();
        CharacterPanelViewController.Instance.HideCharacterRosterPanel();
        TopBarController.Instance.HideNavigationButton();

        // save stuff
        ProgressionController.Instance.SetCheckPoint(SaveCheckPoint.CombatStart);
        ProgressionController.Instance.SetCurrentCombat(TownViewController.Instance.SelectedCombatEvent);

        // Cache characters selected for the combat        
        ProgressionController.Instance.SetChosenCombatCharacters(TownViewController.Instance.SelectedCombatCharacters);

        PersistencyManager.Instance.AutoUpdateSaveFile();

        // Start load combat process
        HandleLoadCombatEncounter(TownViewController.Instance.SelectedCombatEvent, ProgressionController.Instance.ChosenCombatCharacters);
    }
    public void HandleLoadEncounter(EncounterType encounter)
    {
        Debug.LogWarning("EventSequenceController.HandleLoadEncounter()");

        if ((encounter == EncounterType.BasicEnemy ||
            encounter == EncounterType.EliteEnemy ||
            encounter == EncounterType.BossEnemy) &&
            ProgressionController.Instance.CheckPointType == SaveCheckPoint.CombatStart
            )
        {
           // HandleLoadCombatEncounter(ProgressionController.Instance.CurrentCombatData);
        }

        else if ((encounter == EncounterType.BasicEnemy ||
            encounter == EncounterType.EliteEnemy) &&
            ProgressionController.Instance.CheckPointType == SaveCheckPoint.CombatEnd
            )
        {
            BlackScreenController.Instance.FadeInScreen(1f);
            LevelManager.Instance.EnableDungeonScenery();
            LevelManager.Instance.ShowAllNodeViews();
            CharacterEntityController.Instance.CreateAllPlayerCombatCharacters();
            //StartCombatVictorySequence(encounter);
        }

        // TO DO IN FUTURE: boss loot events are different to basic/enemy, and 
        // will require different loading logic here
        else if ((encounter == EncounterType.BossEnemy) &&
            ProgressionController.Instance.CheckPointType == SaveCheckPoint.CombatEnd
            )
        {
           
        }

    }
    public void HandleLoadNextEncounter(MapNode mapNode)
    {
        StartCoroutine(HandleLoadNextEncounterCoroutine(mapNode));
    }
    public IEnumerator HandleLoadNextEncounterCoroutine(MapNode mapNode)
    {
        // Hide world map view
        MapView.Instance.HideMainMapView();
        yield return null;

        // Cache previous encounter data 
       //// EncounterType previousEncounter = ProgressionController.Instance.CurrentEncounter;
        //EnemyWaveSO previousEnemyWave = ProgressionController.Instance.CurrentCombatData;

        // Increment world position + set next encounter
       // ProgressionController.Instance.IncrementWorldMapPosition();
       // ProgressionController.Instance.SetCurrentEncounterType(mapNode.Node.NodeType);

        // Destroy all characters and activation windows if the 
        // previous encounter was a combat event
        /*
        if (previousEncounter == EncounterType.BasicEnemy ||
            previousEncounter == EncounterType.EliteEnemy)
        {
            // Mark wave as seen
            ProgressionController.Instance.AddEnemyWaveToAlreadyEncounteredList(previousEnemyWave);

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
        else if (previousEncounter == EncounterType.BossEnemy)
        {

        }

        // Tear down recruit character screen
        else if(previousEncounter == EncounterType.RecruitCharacter)
        {
            // Fade out visual event
            BlackScreenController.Instance.FadeOutScreen(1f);
            yield return new WaitForSeconds(1f);

            // Teardown recruit event views
            RecruitCharacterController.Instance.ResetAllViews();
        }

        // Do kings blessing end sequence + tear down
        else if (previousEncounter == EncounterType.KingsBlessingEvent)
        {
            CoroutineData kbcSequence = new CoroutineData();
            HandleLoadKingsBlessingContinueSequence(kbcSequence);
            yield return new WaitUntil(() => kbcSequence.CoroutineCompleted());
            LevelManager.Instance.DisableGraveyardScenery();
        }

        // Do camp site end sequence + tear down
        else if (previousEncounter == EncounterType.CampSite)
        {
            List<CampSiteCharacterView> ccList = new List<CampSiteCharacterView>();
            ccList.AddRange(CampSiteController.Instance.AllCampSiteCharacterViews);

            // Disable continue button interactions
            CampSiteController.Instance.continueButtonEnabled = false;

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

        // Do shop end sequence + tear down
        else if (previousEncounter == EncounterType.Shop)
        {
            // Shop keeper farewell
            ShopController.Instance.DoMerchantFarewell();

            // disable shop keeper clickability + GUI
            ShopController.Instance.SetShopKeeperInteractionState(false);
            ShopController.Instance.SetContinueButtonInteractionState(false);
            ShopController.Instance.HideContinueButton();

            // Clear shop content result data
            ShopController.Instance.ClearShopContentDataSet();

            // Move characters off screen
            ShopController.Instance.MoveCharactersToOffScreenRight();

            // Zoom and move camera
            yield return new WaitForSeconds(0.5f);
            CameraManager.Instance.DoCameraMove(3, 0, 3f);
            CameraManager.Instance.DoCameraZoom(5, 3, 3f);

            // Fade out Screen
            BlackScreenController.Instance.FadeOutScreen(3f);

            // Wait for visual events
            yield return new WaitForSeconds(4f);

            ShopController.Instance.DisableCharacterViewParent();
            LevelManager.Instance.DisableShopScenery();
        }

        // Reset Camera
        CameraManager.Instance.ResetMainCameraPositionAndZoom();

        // If next event is a combat, get + set enemy wave before saving to disk
        if (ProgressionController.Instance.CurrentEncounter == EncounterType.BasicEnemy ||
            ProgressionController.Instance.CurrentEncounter == EncounterType.EliteEnemy ||
            ProgressionController.Instance.CurrentEncounter == EncounterType.BossEnemy)
        {
            // Calculate and cache the next enemy wave group
            //JourneyManager.Instance.SetCurrentEnemyWaveData 
            //  (JourneyManager.Instance.GetRandomEnemyWaveFromEncounterData(JourneyManager.Instance.CurrentEncounter));

            if (ProgressionController.Instance.CurrentEncounter == EncounterType.BasicEnemy)
            {
                ProgressionController.Instance.SetCurrentEnemyWaveData 
                  (ProgressionController.Instance.DetermineAndGetNextBasicEnemyWave());
            }
            else if (ProgressionController.Instance.CurrentEncounter == EncounterType.EliteEnemy)
            {
                ProgressionController.Instance.SetCurrentEnemyWaveData
                  (ProgressionController.Instance.DetermineAndGetNextEliteEnemyWave());
            }
            else if (ProgressionController.Instance.CurrentEncounter == EncounterType.BossEnemy)
            {
                ProgressionController.Instance.SetCurrentEnemyWaveData
                  (ProgressionController.Instance.DetermineAndGetNextBossEnemyWave());
            }

            // Set check point
            ProgressionController.Instance.SetCheckPoint(SaveCheckPoint.CombatStart);

            // Auto save
            PersistencyManager.Instance.AutoUpdateSaveFile();

            // Start Load combat
            HandleLoadCombatEncounter(ProgressionController.Instance.CurrentCombatData);
        }

        // Recruit character event
        else if(ProgressionController.Instance.CurrentEncounter == EncounterType.RecruitCharacter)
        {
            // Generate 3 random characters, if we are not loading from save
            if (RecruitCharacterController.Instance.currentChoices.Count == 0)
            {
                RecruitCharacterController.Instance.currentChoices = RecruitCharacterController.Instance.GetThreeValidRecruitableCharacters();
            }

            // Set check point
            ProgressionController.Instance.SetCheckPoint(SaveCheckPoint.RecruitCharacterStart);

            // Auto save
            PersistencyManager.Instance.AutoUpdateSaveFile();

            HandleLoadRecruitCharacterEncounter();
        }

        // Shop event
        /*
        else if (ProgressionController.Instance.CurrentEncounter == EncounterType.Shop)
        {
            // Generate new shop contents
            if (ShopController.Instance.CurrentShopContentResultData == null)
            {
                ShopController.Instance.SetAndCacheNewShopContentDataSet();
            }

            // Set check point
            ProgressionController.Instance.SetCheckPoint(SaveCheckPoint.Shop);

            // Auto save
            PersistencyManager.Instance.AutoUpdateSaveFile();

            HandleLoadShopEvent();
        }
        */

    }
    private void HandleLoadCombatEncounter(CombatData combatData, List<CharacterData> playerCharacters)
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
        if(combatData.combatDifficulty == CombatDifficulty.Basic)
        {
            AudioManager.Instance.AutoPlayBasicCombatMusic(1f);
        }
        else if (combatData.combatDifficulty == CombatDifficulty.Elite)
        {
            AudioManager.Instance.AutoPlayEliteCombatMusic(1f);
        }
        else if (combatData.combatDifficulty == CombatDifficulty.Boss)
        {
            AudioManager.Instance.AutoPlayBossCombatMusic(1f);
        }

        // Create player characters in scene
        CharacterEntityController.Instance.CreateAllPlayerCombatCharacters(playerCharacters);

        // Spawn enemies
        EnemySpawner.Instance.SpawnEnemiesInCombatData(combatData);
        //EnemySpawner.Instance.SpawnEnemyWave(combatData.combatDifficulty.ToString(), combatData);

        // move characters offscreen
        CharacterEntityController.Instance.MoveAllCharactersToOffScreenPosition();

        // make characters move towards start nodes
        CoroutineData cData = new CoroutineData();
        VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.MoveAllCharactersToStartingNodes(cData));       

        // Start a new combat event
        ActivationManager.Instance.OnNewCombatEventStarted();
    }
   
    #endregion

    // Handle Post Combat Stuff
    #region
    public void StartCombatVictorySequence(CombatDifficulty combatType)
    {
        StartCoroutine(StartCombatVictorySequenceCoroutine(combatType));
    }
    private IEnumerator StartCombatVictorySequenceCoroutine(CombatDifficulty combatType)
    {
        // wait until v queue count = 0
        yield return new WaitUntil(()=> VisualEventManager.Instance.EventQueue.Count == 0);

        // stop combat music
        AudioManager.Instance.FadeOutAllCombatMusic(1f);

        // Tear down summoned characters
        foreach (CharacterEntityModel model in CharacterEntityController.Instance.AllSummonedDefenders)
        {
            // Smokey vanish effect
            VisualEffectManager.Instance.CreateExpendEffect(model.characterEntityView.WorldPosition, 15, 0.2f, false);

            // Fade out character model
            CharacterModelController.Instance.FadeOutCharacterModel(model.characterEntityView.ucm, 1f);

            // Fade out UI elements
            CharacterEntityController.Instance.FadeOutCharacterWorldCanvas(model.characterEntityView, null);
            CharacterEntityController.Instance.FadeOutCharacterUICanvas(model.characterEntityView, null);
            if (model.characterEntityView.uiCanvasParent.activeSelf == true)
            {
                CharacterEntityController.Instance.FadeOutCharacterUICanvas(model.characterEntityView, null);
            }
        }


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
        if (ProgressionController.Instance.CheckPointType != SaveCheckPoint.CombatEnd)
        {
            // Cache previous xp of characters for visual events
            List<PreviousXpState> pxsList = new List<PreviousXpState>();
            foreach(CharacterData character in ProgressionController.Instance.ChosenCombatCharacters)
            {
                pxsList.Add(new PreviousXpState(character));
            }

            // Grant xp
            CharacterDataController.Instance.HandleXpRewardPostCombat(combatType, ProgressionController.Instance.ChosenCombatCharacters);

            // Generate and cache xp rewarded stats for visual events
            List<XpRewardData> xpRewardDataSet = new List<XpRewardData>();
            foreach(CharacterData character in ProgressionController.Instance.ChosenCombatCharacters)
            {
                bool flawless = false;
                int combatXp = 0;
                if(combatType == CombatDifficulty.Basic)
                {
                    combatXp = GlobalSettings.Instance.basicCombatXpReward;
                }
                else if (combatType == CombatDifficulty.Elite)
                {
                    combatXp = GlobalSettings.Instance.eliteCombatXpReward;
                }
                else if (combatType == CombatDifficulty.Boss)
                {
                    combatXp = GlobalSettings.Instance.bossCombatXpReward;
                }

                int totalXp = combatXp;

                // Check for flawless bonus
                foreach(CharacterEntityModel cm in CharacterEntityController.Instance.AllDefenders)
                {
                    if(cm.characterData == character)
                    {
                        if(cm.hasLostHealthThisCombat == false)
                        {
                            flawless = true;                            
                        }
                    }
                }

                xpRewardDataSet.Add(new XpRewardData(character, totalXp, combatXp, flawless, combatType));
            }
            
            // Start visual event
            LootController.Instance.PlayNewXpRewardVisualEvent(pxsList, xpRewardDataSet);

            // Generate loot
            LootController.Instance.SetAndCacheNewLootResult();

            // Save and set checkpoint + cache loot result
            ProgressionController.Instance.SetCheckPoint(SaveCheckPoint.CombatEnd);
            PersistencyManager.Instance.AutoUpdateSaveFile();

            // Wait for xp reward v event to finish
            yield return new WaitForSeconds(5f);

            // Fade out xp screen
            LootController.Instance.FadeOutXpRewardScreen();
        }          

        // Build loot screen views
        LootController.Instance.BuildLootScreenElementsFromLootResultData();        

        // fade in loot window
        LootController.Instance.FadeInMainLootView();
        LootController.Instance.ShowFrontPageView();
    }
    public void HandleReturnToTownPostCombatSequence()
    {
        StartCoroutine(HandleReturnToTownPostCombatSequenceCoroutine());
    }
    private IEnumerator HandleReturnToTownPostCombatSequenceCoroutine()
    {
        // Fade out visual event
        BlackScreenController.Instance.FadeOutScreen(3f);

        // Fade and close loot screen views
        LootController.Instance.FadeOutMainLootView(() => LootController.Instance.HideMainLootView());

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

        // Set check point + new day data generation logic
        ProgressionController.Instance.SetDayNumber(ProgressionController.Instance.DayNumber + 1);
        ProgressionController.Instance.SetDailyCombatChoices(CombatGenerationController.Instance.GenerateWeeklyCombatChoices());
        ProgressionController.Instance.SetCheckPoint(SaveCheckPoint.TownDayStart);
        CharacterDataController.Instance.AutoGenerateAndCacheDailyCharacterRecruits(2);

        // to do: generate and cache new recruitable characters here

        // Auto save
        PersistencyManager.Instance.AutoUpdateSaveFile();

        // Rebuild town views

        // Set up top bar
        TopBarController.Instance.ShowTopBar();
        TopBarController.Instance.ShowNavigationButton();
        TopBarController.Instance.SetNavigationButtonText("To Arena");

        // Reset Camera
        CameraManager.Instance.ResetMainCameraPositionAndZoom();

        // Setup town views
        TownViewController.Instance.ShowMainTownView();
        TownViewController.Instance.SetScreenViewState(ScreenViewState.Town);

        // Set up character panel views
        CharacterPanelViewController.Instance.ShowCharacterRosterPanel();
        CharacterPanelViewController.Instance.RebuildAllViews();

        // Reset chosen combat character slots + data
        TownViewController.Instance.ClearAllSelectedCombatCharactersAndSlots();

        // Fade in screen + town music
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.FadeInSound(Sound.Ambience_Outdoor_Spooky, 1f);
        yield return new WaitForSeconds(1f);
        BlackScreenController.Instance.FadeInScreen(1f);

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
