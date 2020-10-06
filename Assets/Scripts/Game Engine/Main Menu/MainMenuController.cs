using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuController : Singleton<MainMenuController>
{
    [Header("Character Template Data")]
    public List<CharacterTemplateSO> selectableCharacterTemplates;

    [Header("Front Screen Components")]
    public GameObject frontScreenParent;

    [Header("New Game Screen Components")]
    public GameObject newGameScreenVisualParent;
    public ChooseCharacterWindow[] chooseCharacterWindows;


    // On Menu Buttons Clicked
    #region
    public void OnNewGameButtonClicked()
    {
        // disable button highlight
        EventSystem.current.SetSelectedGameObject(null);
        ShowNewGameScreen();
        HideFrontScreen();
    }
    public void OnContinueButtonClicked()
    {
        // disable button highlight
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void OnSettingsButtonClicked()
    {
        // disable button highlight
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
    #endregion

    // Front Screen Logic
    #region
    public void ShowFrontScreen()
    {
        frontScreenParent.SetActive(true);
    }
    public void HideFrontScreen()
    {
        frontScreenParent.SetActive(false);
    }
    #endregion

    // New Game Screen Logic
    #region

    // View Logic
    #region
    public void ShowNewGameScreen()
    {
        newGameScreenVisualParent.SetActive(true);
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
}
