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
        else if (type == GameOverEventType.Victory)
            ribbonText.text = "Victory!";

        // COMBAT SCENE TEAR DOWN SEQUENCE
        // wait until v queue count = 0
        yield return new WaitUntil(() => VisualEventManager.Instance.EventQueue.Count == 0);

        // stop combat music
        AudioManager.Instance.FadeOutAllCombatMusic(0.5f);

        if (type == GameOverEventType.Defeat)
            AudioManager.Instance.PlaySound(Sound.Music_Defeat_Fanfare);

        else if (type == GameOverEventType.Victory)
            AudioManager.Instance.PlaySound(Sound.Music_Victory_Fanfare);

        // TO DO: play defeat/victory anthem

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
            tab.descriptionText.text = GetScoreElementDescription(data.type);
            tab.valueText.text = data.totalScore.ToString();
            tab.nameText.text = TextLogic.SplitByCapitals(data.type.ToString());
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
                Debug.Log("FADING IN SCORE TAB!");
                t.gameObject.SetActive(true);
                t.myCg.DOFade(1, 0.5f);
                //t.myCg.alpha = 1;
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


        // CALCULATE SCORING ELEMENTS THAT ONLY OCCUR AT RUN END!

        // Porky
        ScoreElementData porky = CaculatePorkyScore();
        if (porky.totalScore > 0)
            scoreElements.Add(porky);

        // Purist
        ScoreElementData purist = CalculatePuristScore();
        if (purist.totalScore > 0)
            scoreElements.Add(purist);

        // Shrug it off
        ScoreElementData shrugItOff = CalculateShrugItOffScore();
        if (shrugItOff.totalScore > 0)
            scoreElements.Add(shrugItOff);

        // Muscles
        ScoreElementData muscles = CalculateMusclesScore();
        if (muscles.totalScore > 0)
            scoreElements.Add(muscles);

        // Big Brain
        ScoreElementData bigBrain = CalculateBigBrainScore();
        if (bigBrain.totalScore > 0)
            scoreElements.Add(bigBrain);

        // Fat Boi
        ScoreElementData fatBoi = CalculateFatBoiScore();
        if (fatBoi.totalScore > 0)
            scoreElements.Add(fatBoi);

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
        StartCoroutine(HandleReturnFromGameOverToMainMenuCoroutine());
    }
    private IEnumerator HandleReturnFromGameOverToMainMenuCoroutine()
    {
        EventSystem.current.SetSelectedGameObject(null);

        // Fade out battle music + ambience
        AudioManager.Instance.FadeOutAllCombatMusic(2f);
        AudioManager.Instance.FadeOutAllAmbience(2f);

        // Do black screen fade out
        BlackScreenController.Instance.FadeOutScreen(2f);

        yield return new WaitForSeconds(2f);

        // Hide score GUI
        HideAndResetAllScoreTabs();
        scoreScreenVisualParent.SetActive(false);

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
    public void IncrementBossesCleared()
    {
        currentScoreData.bossEnemiesDefeated++;
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

    // Modify score values at run end
    #region
    private string GetScoreElementDescription(ScoreElementType element)
    {
        if(element == ScoreElementType.BigBrain)
        {
            return "Gain " + TextLogic.ReturnColoredText("10", TextLogic.blueNumber) + " points for each character with " +
                TextLogic.ReturnColoredText("30+", TextLogic.blueNumber) + " " + TextLogic.ReturnColoredText("Intelligence", TextLogic.neutralYellow);
        }
        else if (element == ScoreElementType.Muscles)
        {
            return "Gain " + TextLogic.ReturnColoredText("10", TextLogic.blueNumber) + " points for each character with " +
                TextLogic.ReturnColoredText("30+", TextLogic.blueNumber) + " " + TextLogic.ReturnColoredText("Strength", TextLogic.neutralYellow);
        }
        else if (element == ScoreElementType.FatBoi)
        {
            return "Gain " + TextLogic.ReturnColoredText("10", TextLogic.blueNumber) + " points for each character with " +
                TextLogic.ReturnColoredText("30+", TextLogic.blueNumber) + " " + TextLogic.ReturnColoredText("Constitution", TextLogic.neutralYellow);
        }

        else if (element == ScoreElementType.RoomsCleared)
        {
            return "Gain " + TextLogic.ReturnColoredText("5", TextLogic.blueNumber) + " points for room cleared";
        }
        else if (element == ScoreElementType.MonsterSlayer)
        {
            return "Gain " + TextLogic.ReturnColoredText("2", TextLogic.blueNumber) + " points for each " +
                TextLogic.ReturnColoredText("Basic Enemy", TextLogic.neutralYellow) + " encounter defeated";
        }
        else if (element == ScoreElementType.GiantSlayer)
        {
            return "Gain " + TextLogic.ReturnColoredText("10", TextLogic.blueNumber) + " points for each " +
                TextLogic.ReturnColoredText("Mini Boss", TextLogic.neutralYellow) + " defeated";
        }
        else if (element == ScoreElementType.KingSlayer)
        {
            return "Gain " + TextLogic.ReturnColoredText("30", TextLogic.blueNumber) + " points for each " +
                TextLogic.ReturnColoredText("Boss", TextLogic.neutralYellow) + " defeated";
        }


        else if (element == ScoreElementType.Finesse)
        {
            return "Gain " + TextLogic.ReturnColoredText("2", TextLogic.blueNumber) + " points for each time a character took no damage defeating a " +
                TextLogic.ReturnColoredText("Basic Enemy", TextLogic.neutralYellow) + " encounter";
        }
        else if (element == ScoreElementType.ProfessionalKiller)
        {
            return "Gain " + TextLogic.ReturnColoredText("5", TextLogic.blueNumber) + " points for each time a character took no damage defeating a " +
                TextLogic.ReturnColoredText("Mini Boss", TextLogic.neutralYellow) + " encounter";
        }

        else if (element == ScoreElementType.Riches)
        {
            return "Gain " + TextLogic.ReturnColoredText("25", TextLogic.blueNumber) + " points for each " +
                 TextLogic.ReturnColoredText("500", TextLogic.blueNumber) +
                " gold collected";
        }
        else if (element == ScoreElementType.Curator)
        {
            return "Gain " + TextLogic.ReturnColoredText("2", TextLogic.blueNumber) + " points for each " +
                 TextLogic.ReturnColoredText("Trinket", TextLogic.neutralYellow) +
                " collected";
        }

        else if (element == ScoreElementType.Purist)
        {
            return "Gain " + TextLogic.ReturnColoredText("20", TextLogic.blueNumber) + " points for each character that has no rare or epic cards in their deck";
        }
        else if (element == ScoreElementType.ShrugItOff)
        {
            return "Gain " + TextLogic.ReturnColoredText("10", TextLogic.blueNumber) + " points for each character that has "
                + TextLogic.ReturnColoredText("3", TextLogic.blueNumber) + " or more " + TextLogic.ReturnColoredText("Afflictions", TextLogic.neutralYellow) +
                " in their deck";
        }

        else if (element == ScoreElementType.Porky)
        {
            return "Gain " + TextLogic.ReturnColoredText("5", TextLogic.blueNumber) + " points for each character with more than " +
                 TextLogic.ReturnColoredText("120", TextLogic.blueNumber) + " " + TextLogic.ReturnColoredText("Max Health", TextLogic.neutralYellow);
        }
        else
        {
            return "";
        }
    }
    public ScoreElementData CaculatePorkyScore()
    {
        int porkyScore = 0;
        foreach(CharacterData c in CharacterDataController.Instance.AllPlayerCharacters)
        {
            if(c.maxHealth >= 130)
            {
                porkyScore += 5;
            }
        }

        Debug.Log("Porky score calculation = " + porkyScore.ToString());
        return new ScoreElementData(porkyScore, ScoreElementType.Porky);
    }
    public ScoreElementData CalculatePuristScore()
    {
        int puristScore = 0;
        foreach (CharacterData c in CharacterDataController.Instance.AllPlayerCharacters)
        {
            bool onlyCommons = true;
            foreach(CardData card in c.deck)
            {
                if(card.rarity == Rarity.Epic || card.rarity == Rarity.Rare)
                {
                    onlyCommons = false;
                    break;
                }
            }

            if (onlyCommons)
                puristScore += 20;
        }

        Debug.Log("Purist score calculation = " + puristScore.ToString());
        return new ScoreElementData(puristScore, ScoreElementType.Purist);
    }
    public ScoreElementData CalculateShrugItOffScore()
    {
        int shrugItOff = 0;
        foreach (CharacterData c in CharacterDataController.Instance.AllPlayerCharacters)
        {
            int afflictions = 0;
            foreach (CardData card in c.deck)
            {
                if (card.affliction || card.cardType == CardType.Affliction)
                {
                    afflictions++;
                }
            }

            if (afflictions >= 3)
                shrugItOff += 10;            
        }

        Debug.Log("Shrug It Off score calculation = " + shrugItOff.ToString());
        return new ScoreElementData(shrugItOff, ScoreElementType.ShrugItOff);
    }
    public ScoreElementData CalculateMusclesScore()
    {
        int muscles = 0;
        foreach (CharacterData c in CharacterDataController.Instance.AllPlayerCharacters)
        {
            if (c.strength >= 30)
                muscles += 5;
        }

        Debug.Log("Muscles score calculation = " + muscles.ToString());

        return new ScoreElementData(muscles, ScoreElementType.Muscles);
    }
    public ScoreElementData CalculateBigBrainScore()
    {
        int bigBrain = 0;
        foreach (CharacterData c in CharacterDataController.Instance.AllPlayerCharacters)
        {
            if (c.intelligence >= 30)
                bigBrain += 5;
        }

        Debug.Log("Big Brain score calculation = " + bigBrain.ToString());

        return new ScoreElementData(bigBrain, ScoreElementType.BigBrain);
    }
    public ScoreElementData CalculateFatBoiScore()
    {
        int fatBoi = 0;
        foreach (CharacterData c in CharacterDataController.Instance.AllPlayerCharacters)
        {
            if (c.constitution >= 30)
                fatBoi += 5;
        }

        Debug.Log("Big Brain score calculation = " + fatBoi.ToString());

        return new ScoreElementData(fatBoi, ScoreElementType.FatBoi);
    }
    #endregion
}

public enum GameOverEventType
{
    Defeat = 0,
    Victory = 1,
}
