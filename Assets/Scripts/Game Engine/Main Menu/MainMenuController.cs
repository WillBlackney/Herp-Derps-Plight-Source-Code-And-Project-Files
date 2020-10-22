using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;
using System;
using UnityEngine.UI;

public class MainMenuController : Singleton<MainMenuController>
{
    // Properties + Component References
    #region
    [Header("Character Template Data")]
    public List<CharacterTemplateSO> selectableCharacterTemplates;

    [Header("Front Screen Components")]
    [SerializeField] private GameObject frontScreenParent;
    [SerializeField] private GameObject continueButtonParent;
    [SerializeField] private GameObject abandonRunButtonParent;
    [SerializeField] private GameObject abandonRunPopupParent;

    [Header("New Game Screen Components")]
    [SerializeField] private GameObject newGameScreenVisualParent;
    [SerializeField] private ChooseCharacterWindow[] chooseCharacterWindows;

    [Header("In Game Menu Components")]
    [SerializeField] private GameObject inGameMenuScreenParent;

    [Header("Run Modifier Menu Components")]
    [SerializeField] private GameObject runModifierScreenParent;
    [SerializeField] private RunModifierButton randomizeCharactersButton;
    [SerializeField] private RunModifierButton randomizeDecksButton;
    [SerializeField] private RunModifierButton improviseDecksButton;

    [Header("Run Modifier Properties")]
    [HideInInspector] public bool randomizeCharacters = false;
    [HideInInspector] public bool randomizeDecks = false;
    [HideInInspector] public bool improviseDecks = false;

    [Header("Character Info Popup Components")]
    [SerializeField] private GameObject characterInfoPopupVisualParent;
    [SerializeField] private Scrollbar characterInfoPopupScrollBar;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI characterClassNameText;
    [SerializeField] private UniversalCharacterModel characterModel;
    [SerializeField] private CardInfoPanel[] cardInfoPanels;
    [SerializeField] private TalentInfoPanel[] talentInfoPanels;


    #endregion

    // Initialization 
    #region
    private void Start()
    {
        RenderMenuButtons();
        SetRunModifiersToDefaults();
    }
    #endregion

    // On Buttons Clicked
    #region
    // Front menu screen
    public void OnMenuNewGameButtonClicked()
    {
        // disable button highlight
        EventSystem.current.SetSelectedGameObject(null);
        ShowNewGameScreen();
        HideFrontScreen();
    }
    public void OnMenuContinueButtonClicked()
    {
        // disable button highlight
        EventSystem.current.SetSelectedGameObject(null);
        EventSequenceController.Instance.HandleLoadSavedGameFromMainMenuEvent();
    }
    public void OnMenuSettingsButtonClicked()
    {
        // disable button highlight
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void OnMenuQuitButtonClicked()
    {
        Application.Quit();
    }
    public void OnMenuAbandonRunButtonClicked()
    {
        ShowAbandonRunPopup();
    }
    public void OnAbandonPopupAbandonRunButtonClicked()
    {
        PersistencyManager.Instance.DeleteSaveFileOnDisk();
        RenderMenuButtons();
        HideAbandonRunPopup();
    }
    public void OnAbandonPopupCancelButtonClicked()
    {
        HideAbandonRunPopup();
    }
    public void OnRunModifiersButtonClicked()
    {
        if (runModifierScreenParent.activeSelf)
        {
            runModifierScreenParent.SetActive(false);
        }
        else
        {
            runModifierScreenParent.SetActive(true);
        }
    }
    public void OnRunModifiersBackButtonClicked()
    {
        runModifierScreenParent.SetActive(false);
    }
    public void OnRandomizeCharactersButtonClicked()
    {
        if (randomizeCharacters)
        {
            randomizeCharacters = false;
            randomizeCharactersButton.CrossMe();
        }
        else if (!randomizeCharacters)
        {
            randomizeCharacters = true;
            randomizeCharactersButton.TickMe();
        }
    }
    public void OnRandomizeDecksButtonClicked()
    {
        if (randomizeDecks)
        {
            randomizeDecks = false;
            randomizeDecksButton.CrossMe();
        }
        else if (!randomizeDecks)
        {
            randomizeDecks = true;
            improviseDecks = false;
            randomizeDecksButton.TickMe();
            improviseDecksButton.CrossMe();
        }
    }
    public void OnImproviseDecksButtonClicked()
    {
        if (improviseDecks)
        {
            improviseDecks = false;
            improviseDecksButton.CrossMe();
        }
        else if (!improviseDecks)
        {
            improviseDecks = true;
            randomizeDecks = false;
            randomizeDecksButton.CrossMe();
            improviseDecksButton.TickMe();
        }
    }

    // In Game menu screen
    public void OnInGameBackToGameButtonClicked()
    {
        EventSystem.current.SetSelectedGameObject(null);
        HideInGameMenuView();
    }
    public void OnInGameSettingsButtonClicked()
    {

    }
    public void OnInGameSaveAndQuitClicked()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSequenceController.Instance.HandleQuitToMainMenuFromInGame();
    }
    #endregion

    // Top Bar
    #region
    public void OnTopBarSettingsButtonClicked()
    {
        EventSystem.current.SetSelectedGameObject(null);

        if (inGameMenuScreenParent.activeSelf)
        {
            HideInGameMenuView();
        }
        else
        {
            ShowInGameMenuView();
        }
       
    }
    #endregion

    // Front Screen Logic
    #region
    public void RenderMenuButtons()
    {
        AutoSetAbandonRunButtonViewState();
        AutoSetContinueButtonViewState();
    }
    public void ShowFrontScreen()
    {
        frontScreenParent.SetActive(true);
    }
    public void HideFrontScreen()
    {
        frontScreenParent.SetActive(false);
    }
    private bool ShouldShowContinueButton()
    {
        return PersistencyManager.Instance.DoesSaveFileExist();        
    }
    private void AutoSetContinueButtonViewState()
    {
        if (ShouldShowContinueButton())
        {
            ShowContinueButton();
        }
        else
        {
            HideContinueButton();
        }
    }
    private void AutoSetAbandonRunButtonViewState()
    {
        if (ShouldShowContinueButton())
        {
            ShowAbandonRunButton();
        }
        else
        {
            HideAbandonRunButton();
        }
    }
    private void ShowContinueButton()
    {
        continueButtonParent.SetActive(true);
    }
    private void HideContinueButton()
    {
        continueButtonParent.SetActive(false);
    }
    private void ShowAbandonRunButton()
    {
        abandonRunButtonParent.SetActive(true);
    }
    private void HideAbandonRunButton()
    {
        abandonRunButtonParent.SetActive(false);
    }
    private void ShowAbandonRunPopup()
    {
        abandonRunPopupParent.SetActive(true);
    }
    private void HideAbandonRunPopup()
    {
        abandonRunPopupParent.SetActive(false);
    }
    #endregion

    // New Game Screen Logic
    #region

    // View Logic
    #region
    public void ShowNewGameScreen()
    {
        newGameScreenVisualParent.SetActive(true);
        SetRunModifiersToDefaults();
    }
    public void HideNewGameScreen()
    {
        newGameScreenVisualParent.SetActive(false);
    }
    #endregion

    // On Button Click Logic
    #region
    public void OnStartGameButtonClicked()
    {
        EventSequenceController.Instance.HandleStartNewGameFromMainMenuEvent();
    }
    public void OnMainMenuButtonClicked()
    {
        HideNewGameScreen();
        ShowFrontScreen();
    }
    #endregion

    // Misc
    #region
    public List<CharacterTemplateSO> GetChosenTemplatesFromChooseCharacterWindows()
    {
        List<CharacterTemplateSO> chosenCharacters = new List<CharacterTemplateSO>();

        foreach(ChooseCharacterWindow ccw in chooseCharacterWindows)
        {
            chosenCharacters.Add(ccw.currentTemplateSelection);
        }

        return chosenCharacters;
    }
    #endregion

    #endregion

    // In Game Menu Logic
    #region
    private void ShowInGameMenuView()
    {
        inGameMenuScreenParent.SetActive(true);
    }
    public void HideInGameMenuView()
    {
        inGameMenuScreenParent.SetActive(false);
    }
    #endregion

    // Misc
    #region
    public bool AnyMenuScreenIsActive()
    {
        if(inGameMenuScreenParent.activeSelf == true ||
            CardController.Instance.GridCardScreenIsActive())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    // Apply Run Modifiers Logic
    #region
    public void SetRunModifiersToDefaults()
    {
        randomizeCharacters = false;
        randomizeDecks = false;
        improviseDecks = false;
        randomizeCharactersButton.CrossMe();
        randomizeDecksButton.CrossMe();
        improviseDecksButton.CrossMe();
    }
    public List<CharacterTemplateSO> GetThreeRandomAndDifferentTemplates()
    {
        List<CharacterTemplateSO> possibleTemplates = new List<CharacterTemplateSO>();
        List<CharacterTemplateSO> listReturned = new List<CharacterTemplateSO>();
        possibleTemplates.AddRange(selectableCharacterTemplates);

        for(int i = 0; i < 3; i++)
        {
            int randomIndex = RandomGenerator.NumberBetween(0, possibleTemplates.Count - 1);
            listReturned.Add(possibleTemplates[randomIndex]);
            possibleTemplates.RemoveAt(randomIndex);
        }

        return listReturned;

    }
    #endregion

    // Character Pop Up Screen Logic
    #region
    public void BuildWindowFromCharacterTemplateData(CharacterTemplateSO data)
    {
        // Enable window
        ShowCharacterInfoPopupWindow();

        // Set Texts
        characterNameText.text = data.myName;
        characterClassNameText.text = data.myClassName;

        // Build model
        CharacterModelController.BuildModelFromStringReferences(characterModel, data.modelParts);
        CharacterModelController.ApplyItemManagerDataToCharacterModelView(data.serializedItemManager, characterModel);

        // Build talent info panels
        BuildTalentInfoPanels(data);

        // Build card info panels
        BuildCardInfoPanels(data);
    }
    public void OnCharacterInfoPopUpBackButtonClicked()
    {
        HideCharacterInfoPopupWindow();
    }
    private void HideCharacterInfoPopupWindow()
    {
        characterInfoPopupVisualParent.SetActive(false);
    }
    public void ShowCharacterInfoPopupWindow()
    {
        characterInfoPopupVisualParent.SetActive(true);
        characterInfoPopupScrollBar.value = 1;
    }
    private void BuildCardInfoPanels(CharacterTemplateSO data)
    {
        // Disable + Reset all card info panels
        for(int i = 0; i< cardInfoPanels.Length; i++)
        {
            cardInfoPanels[i].gameObject.SetActive(false);
            cardInfoPanels[i].copiesCount = 0;
            cardInfoPanels[i].dataRef = null;
        }

        // Rebuild panels
        for (int i = 0; i < data.deck.Count; i++)
        {
            CardInfoPanel matchingPanel = null;
            foreach(CardInfoPanel panel in cardInfoPanels)
            {
                if(panel.dataRef == data.deck[i])
                {
                    matchingPanel = panel;
                    break;
                }
            }

            if(matchingPanel != null)
            {
                matchingPanel.copiesCount++;
                matchingPanel.copiesCountText.text = "x" + matchingPanel.copiesCount.ToString();
            }
            else
            {
                cardInfoPanels[i].gameObject.SetActive(true);
                cardInfoPanels[i].BuildCardInfoPanelFromCardDataSO(data.deck[i]);
            }
        }

    }
    private void BuildTalentInfoPanels(CharacterTemplateSO data)
    {
        // Disable + Reset all talent info panels
        for (int i = 0; i < talentInfoPanels.Length; i++)
        {
            talentInfoPanels[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < data.talentPairings.Count; i++)
        {
            talentInfoPanels[i].BuildFromTalentPairingModel(data.talentPairings[i]);
        }

    }

    #endregion


}
