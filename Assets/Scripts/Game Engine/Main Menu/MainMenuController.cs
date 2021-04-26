using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Sirenix.OdinInspector;

public class MainMenuController : Singleton<MainMenuController>
{
    // Properties + Component References
    #region
    [Header("Front Screen Components")]
    [SerializeField] private GameObject frontScreenParent;
    public GameObject frontScreenBgParent;
    public CanvasGroup frontScreenGuiCg;
    [SerializeField] private GameObject newGameButtonParent;
    [SerializeField] private GameObject continueButtonParent;
    [SerializeField] private GameObject abandonRunButtonParent;
    [SerializeField] private GameObject abandonRunPopupParent;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("In Game Menu Components")]
    [SerializeField] private GameObject inGameMenuScreenParent;
    [SerializeField] private CanvasGroup inGameMenuScreenCg;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Run Modifier Menu Components")]
    [SerializeField] private GameObject runModifierScreenParent;
    [SerializeField] private RunModifierButton randomizeCharactersButton;
    [SerializeField] private RunModifierButton randomizeDecksButton;
    [SerializeField] private RunModifierButton improviseDecksButton;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Run Modifier Properties")]
    [HideInInspector] public bool randomizeStartingCharacter = false;
    [HideInInspector] public bool randomizeDecks = false;
    [HideInInspector] public bool improviseDecks = false;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("New Game Screen Components/Properties")]
    [SerializeField] private GameObject newGameScreenVisualParent;
    [SerializeField] private GameObject newGameScreenCg;
    [SerializeField] private RectTransform characterInfoRect;
    [SerializeField] private Scrollbar characterInfoScrollBar;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI characterClassNameText;
    [SerializeField] private UniversalCharacterModel characterModel;
    [SerializeField] private CardInfoPanel[] cardInfoPanels;  
    [SerializeField] private TalentInfoPanel[] talentInfoPanels;
    [SerializeField] private CardInfoPanel racialCardInfoPanel;
    [SerializeField] private TextMeshProUGUI racialNameText;
    [SerializeField] private TextMeshProUGUI racialDescriptionText;
    [SerializeField] private CanvasGroup previewCardCg;
    [SerializeField] private CardViewModel previewCardVM;
    [HideInInspector] public CharacterData currentTemplateSelection;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Attribute Components")]
    [SerializeField] private TextMeshProUGUI strengthText;
    [SerializeField] private TextMeshProUGUI intelligenceText;
    [SerializeField] private TextMeshProUGUI dexterityText;
    [SerializeField] private TextMeshProUGUI witsText;
    [SerializeField] private TextMeshProUGUI constitutionText;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    #endregion

    // Initialization 
    #region
    private void Start()
    {
        RenderMenuButtons();
        SetRunModifiersToDefaults();
        SetChoosenCharacterStartingState();

    }
    #endregion

    // On Buttons Clicked
    #region
    // Front menu screen
    public void OnMenuNewGameButtonClicked()
    {
        // disable button highlight
        EventSystem.current.SetSelectedGameObject(null);
        BlackScreenController.Instance.FadeOutAndBackIn(0.5f, 0f, 0.5f, () =>
        {
            ShowNewGameScreen();
            HideFrontScreen();
        });
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
        CanvasGroup cg = runModifierScreenParent.GetComponent<CanvasGroup>();

        if (runModifierScreenParent.activeSelf)
        {
            runModifierScreenParent.SetActive(false);
            cg.alpha = 0;
        }
        else
        {
            cg.alpha = 0;
            runModifierScreenParent.SetActive(true);
            cg.DOFade(1, 0.2f);
        }
    }
    public void OnRunModifiersBackButtonClicked()
    {
        runModifierScreenParent.SetActive(false);
    }
    public void OnRandomizeCharactersButtonClicked()
    {
        if (randomizeStartingCharacter)
        {
            randomizeStartingCharacter = false;
            randomizeCharactersButton.CrossMe();
        }
        else if (!randomizeStartingCharacter)
        {
            randomizeStartingCharacter = true;
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
       // EventSystem.current.SetSelectedGameObject(null);

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
        AutoSetNewGameButtonState();
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
    private bool ShouldShowNewGameButton()
    {
        return PersistencyManager.Instance.DoesSaveFileExist() == false;
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
    private void AutoSetNewGameButtonState()
    {
        if (ShouldShowNewGameButton())
        {
            ShowNewGameButton();
        }
        else
        {
            HideNewGameButton();
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
    private void ShowNewGameButton()
    {
        newGameButtonParent.SetActive(true);
    }
    private void HideNewGameButton()
    {
        newGameButtonParent.SetActive(false);
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

    // Build Views Logic
    #region
    public void ShowNewGameScreen()
    {
        newGameScreenVisualParent.SetActive(true);
        SetChoosenCharacterStartingState();
        SetRunModifiersToDefaults();
    }
    public void HideNewGameScreen()
    {
        newGameScreenVisualParent.SetActive(false);
    }
    public void BuildNewGameWindowFromCharacterTemplateData(CharacterData data)
    {
        // Reset scroll window 
        characterInfoScrollBar.value = 1;

        // Set Texts
        characterNameText.text = data.myName;
        characterClassNameText.text = "The " + data.myClassName;

        // Build model
        CharacterModelController.Instance.BuildModelFromStringReferences(characterModel, data.modelParts);
        CharacterModelController.Instance.ApplyItemManagerDataToCharacterModelView(data.itemManager, characterModel);

        // Build race section
        BuildRacialInfoPanel(data);

        // Build talent info panels
        BuildTalentInfoPanels(data);

        // Build attributes
        BuildAttributeInfoPanels(data);

        // Build card info panels
        BuildCardInfoPanels(data);

        // Rebuild layout
        LayoutRebuilder.ForceRebuildLayoutImmediate(characterInfoRect);
    }
    private void BuildCardInfoPanels(CharacterData data)
    {
        // Disable + Reset all card info panels
        for (int i = 0; i < cardInfoPanels.Length; i++)
        {
            cardInfoPanels[i].gameObject.SetActive(false);
            cardInfoPanels[i].copiesCount = 0;
            cardInfoPanels[i].cardDataRef = null;
        }

        // Rebuild panels
        for (int i = 0; i < data.deck.Count; i++)
        {
            CardInfoPanel matchingPanel = null;
            foreach (CardInfoPanel panel in cardInfoPanels)
            {
                /*
                if (panel.cardDataRef == data.deck[i])
                {
                    matchingPanel = panel;
                    break;
                }
                */
                if (panel.cardDataRef != null && panel.cardDataRef.cardName == data.deck[i].cardName)
                {
                    matchingPanel = panel;
                    break;
                }
            }

            if (matchingPanel != null)
            {
                matchingPanel.copiesCount++;
                matchingPanel.copiesCountText.text = "x" + matchingPanel.copiesCount.ToString();
            }
            else
            {
                cardInfoPanels[i].gameObject.SetActive(true);
                cardInfoPanels[i].BuildCardInfoPanelFromCardData(data.deck[i]);
            }
        }

    }
    private void BuildTalentInfoPanels(CharacterData data)
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
    private void BuildAttributeInfoPanels(CharacterData data)
    {
        strengthText.text = data.strength.ToString();
        if (data.strength > 20)
            strengthText.text = TextLogic.ReturnColoredText(data.strength.ToString(), TextLogic.neutralYellow);
        else if (data.strength < 20)
            strengthText.text = TextLogic.ReturnColoredText(data.strength.ToString(), TextLogic.redText);

        intelligenceText.text = data.intelligence.ToString();
        if (data.intelligence > 20)
            intelligenceText.text = TextLogic.ReturnColoredText(data.intelligence.ToString(), TextLogic.neutralYellow);
        else if (data.strength < 20)
            intelligenceText.text = TextLogic.ReturnColoredText(data.intelligence.ToString(), TextLogic.redText);

        dexterityText.text = data.dexterity.ToString();
        if (data.dexterity > 20)
            dexterityText.text = TextLogic.ReturnColoredText(data.dexterity.ToString(), TextLogic.neutralYellow);
        else if (data.strength < 20)
            dexterityText.text = TextLogic.ReturnColoredText(data.dexterity.ToString(), TextLogic.redText);

        witsText.text = data.wits.ToString();
        if (data.wits > 20)
            witsText.text = TextLogic.ReturnColoredText(data.wits.ToString(), TextLogic.neutralYellow);
        else if (data.strength < 20)
            witsText.text = TextLogic.ReturnColoredText(data.wits.ToString(), TextLogic.redText);

        constitutionText.text = data.constitution.ToString();
        if (data.constitution > 20)
            constitutionText.text = TextLogic.ReturnColoredText(data.constitution.ToString(), TextLogic.neutralYellow);
        else if (data.strength < 20)
            constitutionText.text = TextLogic.ReturnColoredText(data.constitution.ToString(), TextLogic.redText);


        /*
        strengthText.text = data.strength.ToString();
        if (data.strength > 10)
        {
            strengthText.text = TextLogic.ReturnColoredText(data.strength.ToString(),TextLogic.neutralYellow);
        }

       
        intelligenceText.text = data.intelligence.ToString();
        if (data.intelligence > 10)
        {
            intelligenceText.text = TextLogic.ReturnColoredText(data.intelligence.ToString(), TextLogic.neutralYellow);
        }

        dexterityText.text = data.dexterity.ToString();
        if (data.dexterity > 10)
        {
            dexterityText.text = TextLogic.ReturnColoredText(data.dexterity.ToString(), TextLogic.neutralYellow);
        }

        witsText.text = data.wits.ToString();
        if (data.wits > 10)
        {
            witsText.text = TextLogic.ReturnColoredText(data.wits.ToString(), TextLogic.neutralYellow);
        }

        constitutionText.text = data.constitution.ToString();
        if (data.constitution > 10)
        {
            constitutionText.text = TextLogic.ReturnColoredText(data.constitution.ToString(), TextLogic.neutralYellow);
        }
        */
    }
    private void BuildRacialInfoPanel(CharacterData data)
    {
        racialNameText.text = data.race.ToString();
        racialCardInfoPanel.BuildCardInfoPanelFromCardData(CardController.Instance.FindBaseRacialCardData(data.race));
        racialDescriptionText.text = KeywordLibrary.Instance.GetRacialData(data.race).raceDescription;
    }
    public void BuildAndShowCardViewModelPopup(CardData data)
    {
        previewCardCg.gameObject.SetActive(true);
        CardController.Instance.BuildCardViewModelFromCardData(data, previewCardVM);
        previewCardCg.alpha = 0;
        previewCardCg.DOFade(1f, 0.25f);
    }
    public void HidePreviewCard()
    {
        previewCardCg.gameObject.SetActive(false);
        previewCardCg.alpha = 0;
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
        BlackScreenController.Instance.FadeOutAndBackIn(0.5f, 0f, 0.5f, () =>
        {
            HideNewGameScreen();
            ShowFrontScreen();
        });
    }
    public void OnNextCharacterButtonClicked()
    {
        EventSystem.current.SetSelectedGameObject(null);
        GetAndSetNextAvailableCharacter();
    }
    public void OnPreviousCharacterButtonClicked()
    {
        EventSystem.current.SetSelectedGameObject(null);
        GetAndSetPreviousAvailableCharacter();
    }
    #endregion

    // Get + Set Character Templates Logic
    #region
    public CharacterData GetChosenCharacter()
    {
        return currentTemplateSelection;
    }
    public void SetChoosenCharacterStartingState()
    {
        currentTemplateSelection = CharacterDataController.Instance.AllCharacterTemplates[0];
        BuildNewGameWindowFromCharacterTemplateData(currentTemplateSelection);

    }
    public void GetAndSetNextAvailableCharacter()
    {
        currentTemplateSelection = GetNextTemplate(currentTemplateSelection);
        BuildNewGameWindowFromCharacterTemplateData(currentTemplateSelection);
    }
    public void GetAndSetPreviousAvailableCharacter()
    {
        currentTemplateSelection = GetPreviousTemplate(currentTemplateSelection);
        BuildNewGameWindowFromCharacterTemplateData(currentTemplateSelection);

    }
    private CharacterData GetNextTemplate(CharacterData currentTemplate)
    {
        CharacterData templateReturned = null;
        int currentIndex = Array.IndexOf(CharacterDataController.Instance.AllCharacterTemplates, currentTemplate);

        if (currentIndex == CharacterDataController.Instance.AllCharacterTemplates.Length - 1)
        {
            templateReturned = CharacterDataController.Instance.AllCharacterTemplates[0];
        }
        else
        {
            templateReturned = CharacterDataController.Instance.AllCharacterTemplates[currentIndex + 1];
        }

        return templateReturned;
    }
    private CharacterData GetPreviousTemplate(CharacterData currentTemplate)
    {
        CharacterData templateReturned = null;
        int currentIndex = Array.IndexOf(CharacterDataController.Instance.AllCharacterTemplates, currentTemplate);

        if (currentIndex == 0)
        {
            templateReturned = CharacterDataController.Instance.AllCharacterTemplates[CharacterDataController.Instance.AllCharacterTemplates.Length - 1];
        }
        else
        {
            templateReturned = CharacterDataController.Instance.AllCharacterTemplates[currentIndex - 1];
        }

        return templateReturned;
    }
    
    #endregion

    #endregion

    // In Game Menu Logic
    #region
    private void ShowInGameMenuView()
    {
        inGameMenuScreenCg.alpha = 0;
        inGameMenuScreenParent.SetActive(true);
        inGameMenuScreenCg.DOFade(1, 0.25f);
    }
    public void HideInGameMenuView()
    {
        inGameMenuScreenParent.SetActive(false);
        inGameMenuScreenCg.alpha = 0;
    }
    #endregion

    // Misc
    #region
    public bool AnyMenuScreenIsActive()
    {
        if(inGameMenuScreenParent.activeSelf == true ||
            LootController.Instance.LootScreenIsActive() ||
            CardController.Instance.GridCardScreenIsActive() ||
             BlackScreenController.Instance.FadeInProgress ||
             MapSystem.MapView.Instance.MasterMapParent.activeSelf ||
             CharacterRosterViewController.Instance.MainVisualParent.activeSelf)
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
        randomizeStartingCharacter = false;
        randomizeDecks = false;
        improviseDecks = false;
        randomizeCharactersButton.CrossMe();
        randomizeDecksButton.CrossMe();
        improviseDecksButton.CrossMe();
    }
    public List<CharacterData> GetThreeRandomAndDifferentTemplates()
    {
        List<CharacterData> possibleTemplates = new List<CharacterData>();
        List<CharacterData> listReturned = new List<CharacterData>();
        possibleTemplates.AddRange(CharacterDataController.Instance.AllCharacterTemplates);

        for(int i = 0; i < 3; i++)
        {
            int randomIndex = RandomGenerator.NumberBetween(0, possibleTemplates.Count - 1);
            listReturned.Add(possibleTemplates[randomIndex]);
            possibleTemplates.RemoveAt(randomIndex);
        }

        return listReturned;

    }
    #endregion

   


}
