using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    // Transisition Logic
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

    // On click logic
    #region
    public void OnTopBarMainButtonClicked()
    {
        if (currentScreenViewState == ScreenViewState.Town)
            HandleTransistionFromTownToArena();
        else if (currentScreenViewState == ScreenViewState.Arena)
            HandleTransistionFromArenaToTown();
    }
    #endregion

}

public enum ScreenViewState
{
    None = 0,
    Town = 1,
    Arena = 2,
}
