using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ScoreManager : Singleton<ScoreManager>
{
    // Properties + Components
    #region
    [Header("Game Over Screen Components")]
    [SerializeField] GameObject scoreScreenVisualParent;
    [SerializeField] CanvasGroup scoreScreenCg;
    [SerializeField] ScoreTabView[] allScoreTabs;
    [SerializeField] TextMeshProUGUI ribbonText;
    [SerializeField] TextMeshProUGUI totalScoreValueText;

    private PlayerScoreTracker currentScoreData;
    #endregion

    // Getters + Accessors
    #region
    public PlayerScoreTracker CurrentScoreData
    {
        get { return currentScoreData; }
        private set { currentScoreData = value; }
    }
    #endregion

    // Persistency 
    #region
    public void BuildMyDataFromSaveFile(SaveGameData saveFile)
    {
        CurrentScoreData = saveFile.scoreData;
    }
    public void SaveMyDataToSaveFile(SaveGameData saveFile)
    {
        saveFile.scoreData = CurrentScoreData;
    }
    public void GenerateNewScoreDataOnGameStart()
    {
        currentScoreData = new PlayerScoreTracker();
    }
    #endregion

    // Score Screen Logic
    #region
    public void HandleGameOverSequence(GameOverEventType type)
    {
        StartCoroutine(HandleShowScoreScreenSequence(type));
    }
    private IEnumerator HandleShowScoreScreenSequence(GameOverEventType type)
    {
        // Is victory or defeat??
        if (type == GameOverEventType.Defeat)
            ribbonText.text = "Defeated...";
        if (type == GameOverEventType.Victory)
            ribbonText.text = "Victory!";

        // COMBAT SCENE TEAR DOWN SEQUENCE
        // wait until v queue count = 0
        yield return new WaitUntil(() => VisualEventManager.Instance.EventQueue.Count == 0);

        // stop combat music
        AudioManager.Instance.FadeOutAllCombatMusic(0.5f);
        // to do: play defeat/victory anthem

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

        // Hide level nodes
        LevelManager.Instance.HideAllNodeViews();

        // Get required scoring data from save file, then delete it.
        SaveGameData currentSaveData = PersistencyManager.Instance.LoadGameFromDisk();
        PersistencyManager.Instance.DeleteSaveFileOnDisk();

        // Do main score screen visual sequence
        HideAndResetAllScoreTabs(); 
        List<ScoreElementData> scoreElements = GenerateScoringElements(currentSaveData.scoreData);
        BuildAllScoreTabs(scoreElements);
        totalScoreValueText.text = CalculateFinalScore(scoreElements).ToString();
        FadeInScoreScreen();
        yield return new WaitForSeconds(1f);
        StartCoroutine(FadeInScoreTabs());

    }
    private void BuildAllScoreTabs(List<ScoreElementData> scoreElements)
    {
        for(int i = 0; i < scoreElements.Count; i++)
        {
            // prevent exceeding array bounds accidently (max 15 score tab views, but there could be 15+ score elements)
            if (i > allScoreTabs.Length - 1)
                break;

            ScoreTabView tab = allScoreTabs[i];
            ScoreElementData data = scoreElements[i];

            tab.isActive = true;
            tab.valueText.text = data.totalScore.ToString();
            tab.descriptionText.text = TextLogic.SplitByCapitals(data.type.ToString());
        }
    }
    private void HideAndResetAllScoreTabs()
    {
        foreach (ScoreTabView tab in allScoreTabs)
        {
            tab.gameObject.SetActive(false);
            tab.myCg.alpha = 0;
            tab.isActive = false;
        }
           
    }
    private void FadeInScoreScreen()
    {
        scoreScreenVisualParent.SetActive(true);
        scoreScreenCg.alpha = 0;
        scoreScreenCg.DOFade(1f, 1f);
    }
    private IEnumerator FadeInScoreTabs()
    {
        foreach(ScoreTabView t in allScoreTabs)
        {
            if(t.isActive == true)
            {
                t.myCg.DOFade(1f, 0.5f);
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
    public List<ScoreElementData> GenerateScoringElements(PlayerScoreTracker scoreData)
    {
        // scoring elements done at run end
        /*
         * Librarian: 4 copies of a card
         * Purist: for each character without a rare or epic card
         * Highlander: for each character withou a duplicate card
         * Cursed: for each character with 3 or more afflictions in deck
         * Muscles/Big Brain / Fat Boi: for eeach character with 30+ strength/intelligence/constitution
         * Specialized: for each talent type with 2 points leveled.
         * Porky: for each 10 max health a character has above 100.
         * 
         */


        List<ScoreElementData> scoreElements = new List<ScoreElementData>();

        // Rooms cleared
        if (scoreData.roomsCleared > 0)
            scoreElements.Add(new ScoreElementData(scoreData.roomsCleared * 5, ScoreElementType.RoomsCleared));

        // basics defeated
        if (scoreData.basicEnemiesDefeated > 0)
            scoreElements.Add(new ScoreElementData(scoreData.basicEnemiesDefeated * 2, ScoreElementType.MonsterSlayer));

        // mini bosses defeated
        if (scoreData.miniBossEnemiesDefeated > 0)
            scoreElements.Add(new ScoreElementData(scoreData.miniBossEnemiesDefeated * 10, ScoreElementType.GiantSlayer));

        // basic no damage taken
        if (scoreData.basicNoDamageTakenTicks > 0)
            scoreElements.Add(new ScoreElementData(scoreData.basicNoDamageTakenTicks * 2, ScoreElementType.Finesse));

        // miniboss no damage taken
        if (scoreData.miniBossNoDamageTakenTicks > 0)
            scoreElements.Add(new ScoreElementData(scoreData.miniBossNoDamageTakenTicks * 5, ScoreElementType.ProfessionalKiller));

        // gold gained
        if (scoreData.totalGoldGained > 500)
            scoreElements.Add(new ScoreElementData(scoreData.totalGoldGained / 500 * 25, ScoreElementType.Riches));

        // trinkets collected
        if (scoreData.trinketsCollected > 0)
            scoreElements.Add(new ScoreElementData(scoreData.trinketsCollected * 5, ScoreElementType.Curator));

        return scoreElements;
    }
    public int CalculateFinalScore(List<ScoreElementData> scoreElements)
    {
        int score = 0;
        foreach (ScoreElementData s in scoreElements)
            score += s.totalScore;

        return score;
    }
    public void OnScoreScreenContinueButtonClicked()
    {
        EventSystem.current.SetSelectedGameObject(null);

        // Fade out battle music + ambience
        AudioManager.Instance.FadeOutAllCombatMusic(2f);
        AudioManager.Instance.FadeOutAllAmbience(2f);

        // Do black screen fade out
        BlackScreenController.Instance.FadeOutScreen(2f);

        // Hide Game GUI
        TopBarController.Instance.HideTopBar();
        StateController.Instance.HideStatePanel();

        // Brute force stop all game music
        AudioManager.Instance.ForceStopAllCombatMusic();

        // Destroy game scene
        EventSequenceController.Instance.HandleCombatSceneTearDown();

        // Hide world map + roster
        MapSystem.MapView.Instance.HideMainMapView();
        CharacterRosterViewController.Instance.DisableMainView();

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

        // Hide Shrine
        ChooseStateWindowController.Instance.HideWindowInstant();
        ShrineController.Instance.HideContinueButton();
        ShrineController.Instance.DisableAllViews();
        LevelManager.Instance.DisableShrineScenery();

        // Fade in menu music
        AudioManager.Instance.FadeInSound(Sound.Music_Main_Menu_Theme_1, 1f);

        // Show menu screen
        MainMenuController.Instance.ShowFrontScreen();
        MainMenuController.Instance.RenderMenuButtons();

        // Do black screen fade in
        BlackScreenController.Instance.FadeInScreen(2f);
    }
    #endregion

    // Modify Score values mid journey
    #region
    public void IncrementRoomsCleared()
    {
        currentScoreData.roomsCleared++;
    }
    public void IncrementBasicsCleared()
    {
        currentScoreData.basicEnemiesDefeated++;
    }
    public void IncrementMinibossesCleared()
    {
        currentScoreData.basicEnemiesDefeated++;
    }
    public void IncrementBasicNoDamageTaken()
    {
        currentScoreData.basicNoDamageTakenTicks++;
    }
    public void IncrementMiniBossNoDamageTaken()
    {
        currentScoreData.basicNoDamageTakenTicks++;
    }
    public void IncrementGoldGained(int goldGained)
    {
        currentScoreData.totalGoldGained += goldGained;
    }
    public void IncrementTrinketsCollected()
    {
        currentScoreData.trinketsCollected++;
    }
    #endregion
}

public enum GameOverEventType
{
    Defeat = 0,
    Victory = 1,
}
