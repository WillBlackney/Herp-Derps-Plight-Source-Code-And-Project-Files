using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuController : Singleton<MainMenuController>
{
    // Properties + Component References
    #region
    [Header("Character Template Data")]
    public List<CharacterTemplateSO> selectableCharacterTemplates;

    [Header("Front Screen Components")]
    public GameObject frontScreenParent;
    public GameObject continueButtonParent;
    public GameObject abandonRunButtonParent;
    public GameObject abandonRunPopupParent;

    [Header("New Game Screen Components")]
    public GameObject newGameScreenVisualParent;
    public ChooseCharacterWindow[] chooseCharacterWindows;

    [Header("In Game Menu Components")]
    public GameObject inGameMenuScreenParent;

    [Header("Run Modifier Menu Components")]
    public GameObject runModifierScreenParent;
    public RunModifierButton randomizeCharactersButton;
    public RunModifierButton randomizeDecksButton;
    public RunModifierButton improviseDecksButton;
    public bool randomizeCharacters = false;
    public bool randomizeDecks = false;
    public bool improviseDecks = false;
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
        runModifierScreenParent.SetActive(true);
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
        if(inGameMenuScreenParent.activeSelf == true)
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

}
