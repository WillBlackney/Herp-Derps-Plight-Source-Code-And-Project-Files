using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class TownViewController : Singleton<TownViewController>
{
    // Properties + Component References
    #region
    [Header("View Parent References")]
    [SerializeField] GameObject townVisualParent;
    [SerializeField] GameObject arenaScreenVisualParent;
    [SerializeField] GameObject chosenCharacterSlotsVisualParent;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    private ScreenViewState currentScreenViewState = ScreenViewState.Town;


    [Header("Colours")]
    [SerializeField] Color basicColour;
    [SerializeField] Color eliteColour;
    [SerializeField] Color bossColour;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Combat Overview Panel Components")]
    [SerializeField] TextMeshProUGUI encounterNameText;
    [SerializeField] TextMeshProUGUI difficultyText;
    [SerializeField] GameObject[] difficultySkulls;
    [SerializeField] TextMeshProUGUI[] enemyNameTexts;
    [SerializeField] TextMeshProUGUI goldRewardText;
    [SerializeField] ChooseCombatButton[] chooseCombatButtons;
    [SerializeField] ChooseCombatCharacterSlot[] chooseCombatCharacterSlots;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Chosen Combat Related Properties")]
    private CombatData selectedCombaEvent;
    private List<CharacterData> selectedCombatCharacters = new List<CharacterData>();

    [Header("Recruit Window Components")]
    [SerializeField] GameObject recruitPageVisualParent;
    [SerializeField] RecruitCharacterTab[] recruitCharacterTabs;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Doctor Window Components")]
    [SerializeField] GameObject doctorPageVisualParent;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    #endregion

    // Properties + Accessors
    #region
    public ChooseCombatCharacterSlot[] ChooseCombatCharacterSlots
    {
        get { return chooseCombatCharacterSlots; }
    }
    public CombatData SelectedCombatEvent
    {
        get { return selectedCombaEvent; }
        private set { selectedCombaEvent = value; }
    }
    public List<CharacterData> SelectedCombatCharacters
    {
        get { return selectedCombatCharacters; }
        private set { selectedCombatCharacters = value; }
    }
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
    public void ShowChosenCharacterSlots()
    {
        chosenCharacterSlotsVisualParent.SetActive(true);
    }
    public void HideChosenCharacterSlots()
    {
        chosenCharacterSlotsVisualParent.SetActive(false);
    }
    public void ShowArenaScreen()
    {
        arenaScreenVisualParent.SetActive(true);
    }
    public void HideArenaScreen()
    {
        arenaScreenVisualParent.SetActive(false);
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
        Debug.LogWarning("TownViewController.HandleTransistionFromTownToArenaCoroutine() called...");

        SetScreenViewState(ScreenViewState.Arena);
        BlackScreenController.Instance.FadeOutScreen(0.5f);
        yield return new WaitForSeconds(0.5f);
        BlackScreenController.Instance.FadeInScreen(0.5f);

        // disable old views
        HideMainTownView();

        // enable new views
        ShowArenaScreen();
        ShowChosenCharacterSlots();

        // set tip bar button text
        TopBarController.Instance.SetNavigationButtonText("To Town");

        // Build combat choice buttons
        BuildAllChooseCombatButtonsFromDataSet(ProgressionController.Instance.DailyCombatChoices.encounters);

        // Auto select first encounter
        OnChooseCombatButtonClicked(chooseCombatButtons[0]);

    }
    public void HandleTransistionFromArenaToTown()
    {
        StartCoroutine(HandleTransistionFromArenaToTownCoroutine());
    }
    private IEnumerator HandleTransistionFromArenaToTownCoroutine()
    {
        SetScreenViewState(ScreenViewState.Town);
        BlackScreenController.Instance.FadeOutScreen(0.5f);
        yield return new WaitForSeconds(0.5f);
        BlackScreenController.Instance.FadeInScreen(0.5f);

        // disable old views
        HideArenaScreen();
        HideChosenCharacterSlots();

        // enable new views
        ShowMainTownView();

        // set top bar button text
        TopBarController.Instance.SetNavigationButtonText("To Arena");

    }
    public void SetScreenViewState(ScreenViewState newState)
    {
        currentScreenViewState = newState;
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
    public void OnChooseCombatButtonClicked(ChooseCombatButton button)
    {
        SelectedCombatEvent = button.combatDataRef;
        BuildCombatOverviewPanelFromCombatData(button.combatDataRef);
    }
    public void OnFightButtonClicked()
    {
        // to do: check validity: player has selected enough characters, characters within level range, etc

        EventSequenceController.Instance.HandleStartCombatFromChooseCombatScreen();
    }
    public void OnOpenRecruitPageButtonClicked()
    {
        BuildRecruitCharactersPageViews();
        ShowRecruitCharactersPage();
    }
    public void OnCloseRecruitPageButtonClicked()
    {
        HideRecruitCharactersPage();
    }
    #endregion

    // Combat Panel Overview Logic
    #region
    private void BuildCombatOverviewPanelFromCombatData(CombatData data)
    {
        Debug.LogWarning("TownViewController.BuildCombatOverviewPanelFromCombatData() called, combat: " + data.encounterName);

        // reset views first
        foreach (GameObject skull in difficultySkulls)
            skull.SetActive(false);

        foreach (TextMeshProUGUI text in enemyNameTexts)
            text.gameObject.SetActive(false);

        // Set encounter name header text
        encounterNameText.text = data.encounterName;

        // Set level text
        if (data.levelRange == CombatLevelRange.ZeroToTwo)
        {
            difficultyText.text = "Level 1";
            difficultyText.color = basicColour;
        }
           
        else if (data.levelRange == CombatLevelRange.ThreeToFour)
        {
            difficultyText.text = "Level 3";
            difficultyText.color = eliteColour;
        }
            
        else if (data.levelRange == CombatLevelRange.FiveToSix)
        {
            difficultyText.text = "Level 5";
            difficultyText.color = bossColour;
        }
           
        else if (data.levelRange == CombatLevelRange.Six)
        {
            difficultyText.text = "Level 6";
            difficultyText.color = bossColour;
        }
            

        // Add combat difficulty type to text
        difficultyText.text += " " + data.combatDifficulty.ToString();

        // Set difficulty skulls
        int skullCount = 1;
        if (data.combatDifficulty == CombatDifficulty.Elite)
            skullCount = 2;
        else if (data.combatDifficulty == CombatDifficulty.Boss)
            skullCount = 3;

        for(int i = 0; i < skullCount; i++)        
            difficultySkulls[i].gameObject.SetActive(true);

        // Build enemy name texts
        for(int i = 0; i < data.enemies.Count; i++)
        {
            enemyNameTexts[i].gameObject.SetActive(true);
            enemyNameTexts[i].text = "- " + data.enemies[i].enemyName;
        }

        // Build reward icons
        goldRewardText.text = data.goldReward.ToString();
        

    }
    #endregion

    // Combat Choose Button Logic
    #region
    private void BuildAllChooseCombatButtonsFromDataSet(List<CombatData> combats)
    {
        Debug.LogWarning("TownViewController.BuildAllChooseCombatButtonFromDataSet() called...");

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
        Debug.LogWarning("TownViewController.BuildChooseCombatButtonFromData() called, combat name: " + data.encounterName);

        // Show button
        button.gameObject.SetActive(true);

        // Cache data ref
        button.combatDataRef = data;

        // Set encounter image + shadow
        foreach (Image i in button.encounterImages)
            i.sprite = data.GetMySprite();

        // Set difficulty colouring
        if (data.combatDifficulty == CombatDifficulty.Basic)
            button.difficultyColourImage.color = basicColour;
        else if (data.combatDifficulty == CombatDifficulty.Elite)
            button.difficultyColourImage.color = eliteColour;
        else if (data.combatDifficulty == CombatDifficulty.Boss)
            button.difficultyColourImage.color = bossColour;


    }
    #endregion

    // Choose Combat Characters Logic
    #region
    public void AddCharacterToSelectedCombatCharacters(CharacterData character)
    {
        Debug.LogWarning("TownViewController.AddCharacterToSelectedCombatCharacters() called, adding " +
            character.myName + ". Total selected characters = " + (SelectedCombatCharacters.Count + 1).ToString());
        SelectedCombatCharacters.Add(character);
    }
    public void RemoveCharacterFromSelectedCombatCharacters(CharacterData character)
    {
        if (SelectedCombatCharacters.Contains(character))
        {
            Debug.LogWarning("TownViewController.RemoveCharacterFromSelectedCombatCharacters() called, removing " +
            character.myName + ". Total selected characters = " + (SelectedCombatCharacters.Count - 1).ToString());
            SelectedCombatCharacters.Remove(character);
        }            
    }
    public void ClearAllSelectedCombatCharactersAndSlots()
    {
        SelectedCombatCharacters.Clear();
        foreach(ChooseCombatCharacterSlot slot in ChooseCombatCharacterSlots)
        {
            ResetChooseCombatSlot(slot);
        }
    }
    public void ResetChooseCombatSlot(ChooseCombatCharacterSlot slot)
    {
        RemoveCharacterFromSelectedCombatCharacters(slot.characterDataRef);
        slot.characterDataRef = null;
        slot.ucmVisualParent.SetActive(false);
    }
    #endregion

    // Recruit Characters Page Logic
    #region
    private void ShowRecruitCharactersPage()
    {
        recruitPageVisualParent.SetActive(true);
    }
    private void BuildRecruitCharactersPageViews()
    {
        BuildAllRecruitCharacterTabsFromDataSet(CharacterDataController.Instance.DailyRecruits);
    }
    public void OnCharacterRecruited(CharacterData data)
    {
        foreach(RecruitCharacterTab tab in recruitCharacterTabs)
        {
            if(tab.characterDataRef == data)
            {
                tab.gameObject.SetActive(false);
            }
        }
    }
    private void HideRecruitCharactersPage()
    {
        recruitPageVisualParent.SetActive(false);
    }
    private void HideAllRecruitTabs()
    {
        foreach (RecruitCharacterTab tab in recruitCharacterTabs)
            tab.gameObject.SetActive(false);
    }
    private void BuildAllRecruitCharacterTabsFromDataSet(List<CharacterData> characters)
    {
        Debug.LogWarning("Characters count: " + characters.Count);
        // Reset tabs
        HideAllRecruitTabs();

        // Build tabs
        for(int i = 0; i < characters.Count; i++)
        {
            BuildRecruitCharacterTabFromCharacterData(characters[i], recruitCharacterTabs[i]);
        }
    }
    private void BuildRecruitCharacterTabFromCharacterData(CharacterData data, RecruitCharacterTab tab)
    {
        tab.gameObject.SetActive(true);

        tab.characterDataRef = data;
        tab.nameText.text = data.myName;
        tab.raceAndClassText.text = data.race.ToString() + " " + data.myClassName;
        CharacterModelController.Instance.BuildModelMugShotFromStringReferences(tab.ucm, data.modelParts);
    }
    #endregion

}

public enum ScreenViewState
{
    None = 0,
    Town = 1,
    Arena = 2,
}
