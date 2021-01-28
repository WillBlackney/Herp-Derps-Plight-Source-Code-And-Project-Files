using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TownViewController : Singleton<TownViewController>
{
    // Properties + Component References
    #region
    [Header("View Parent References")]
    [SerializeField] GameObject townVisualParent;
    [SerializeField] GameObject arenaScreenVisualParent;
    [SerializeField] GameObject chosenCharacterSlotsVisualParent;
    private ScreenViewState currentScreenViewState = ScreenViewState.Town;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI mainTopBarButtonText;

    [Header("Colours")]
    [SerializeField] Color basicColour;
    [SerializeField] Color eliteColour;
    [SerializeField] Color bossColour;

    [Header("Combat Overview Panel Components")]
    [SerializeField] TextMeshProUGUI levelRangeText;
    [SerializeField] GameObject[] difficultySkulls;
    [SerializeField] TextMeshProUGUI[] enemyNameTexts;

    [SerializeField] ChooseCombatButton[] chooseCombatButtons;

    #endregion


    // View Logic
    #region
    public void ShowMainTownView()
    {
        townVisualParent.SetActive(true);
    }
    public void HideMainTownView()
    {
        townVisualParent.SetActive(false);
    }
    #endregion

    // Screen Transisition Logic
    #region
    public void HandleTransistionFromTownToArena()
    {
        StartCoroutine(HandleTransistionFromTownToArenaCoroutine());
    }
    private IEnumerator HandleTransistionFromTownToArenaCoroutine()
    {
        currentScreenViewState = ScreenViewState.Arena;
        BlackScreenController.Instance.FadeOutScreen(0.5f);
        yield return new WaitForSeconds(0.5f);
        BlackScreenController.Instance.FadeInScreen(0.5f);

        // disable old views
        HideMainTownView();

        // enable new views
        arenaScreenVisualParent.SetActive(true);
        chosenCharacterSlotsVisualParent.SetActive(true);

        // set tip bar button text
        mainTopBarButtonText.text = "To Town";

    }
    public void HandleTransistionFromArenaToTown()
    {
        StartCoroutine(HandleTransistionFromArenaToTownCoroutine());
    }
    private IEnumerator HandleTransistionFromArenaToTownCoroutine()
    {
        currentScreenViewState = ScreenViewState.Town;
        BlackScreenController.Instance.FadeOutScreen(0.5f);
        yield return new WaitForSeconds(0.5f);
        BlackScreenController.Instance.FadeInScreen(0.5f);

        // disable old views
        arenaScreenVisualParent.SetActive(false);
        chosenCharacterSlotsVisualParent.SetActive(false);

        // enable new views
        ShowMainTownView();

        // set tip bar button text
        mainTopBarButtonText.text = "To Arena";

    }
    #endregion

    // On Click logic
    #region
    public void OnTopBarMainButtonClicked()
    {
        if (currentScreenViewState == ScreenViewState.Town)
            HandleTransistionFromTownToArena();
        else if (currentScreenViewState == ScreenViewState.Arena)
            HandleTransistionFromArenaToTown();
    }
    #endregion

    // Combat Panel Overview Logic
    #region
    private void BuildCombatOverviewPanelFromCombatData(CombatData data)
    {
        // reset views first
        foreach (GameObject skull in difficultySkulls)
            skull.SetActive(false);

        foreach (TextMeshProUGUI text in enemyNameTexts)
            text.gameObject.SetActive(false);

        // Set level text
        if (data.levelRange == CombatLevelRange.ZeroToTwo)
            levelRangeText.text = "Level 0-2";
        else if (data.levelRange == CombatLevelRange.ThreeToFour)
            levelRangeText.text = "Level 3-4";
        else if (data.levelRange == CombatLevelRange.FiveToSix)
            levelRangeText.text = "Level 5-6";
        else if (data.levelRange == CombatLevelRange.Six)
            levelRangeText.text = "Level 6";

        // Set difficulty skulls
        int skullCount = 1;
        if (data.combatDifficulty == CombatDifficulty.Elite)
            skullCount = 2;
        else if (data.combatDifficulty == CombatDifficulty.Boss)
            skullCount = 3;

        for(int i = 0; i < skullCount; i++)        
            difficultySkulls[i].gameObject.SetActive(true);
        

    }
    #endregion

    // Combat Choose Button Logic
    #region
    private void BuildAllChooseCombatButtonFromDataSet(List<CombatData> combats)
    {
        // Disable + reset all choose buttons
        foreach (ChooseCombatButton button in chooseCombatButtons)
            button.gameObject.SetActive(false);
        
        // Rebuild each button
        for(int i = 0; i < combats.Count; i++)
        {
            BuildChooseCombatButtonFromData(chooseCombatButtons[i], combats[i]);
        }
    }
    private void BuildChooseCombatButtonFromData(ChooseCombatButton button, CombatData data)
    {
        // Show button
        button.gameObject.SetActive(true);

        // Cache data ref
        button.combatDataRef = data;

        // Set encounter image + shadow
        foreach (Image i in button.encounterImages)
            i.sprite = data.encounterSprite;

        // Set difficulty colouring
        if (data.combatDifficulty == CombatDifficulty.Basic)
            button.difficultyColourImage.color = basicColour;
        else if (data.combatDifficulty == CombatDifficulty.Elite)
            button.difficultyColourImage.color = eliteColour;
        else if (data.combatDifficulty == CombatDifficulty.Boss)
            button.difficultyColourImage.color = bossColour;


    }
    #endregion

}

public enum ScreenViewState
{
    None = 0,
    Town = 1,
    Arena = 2,
}
