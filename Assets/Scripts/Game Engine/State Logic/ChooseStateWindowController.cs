using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class ChooseStateWindowController : Singleton<ChooseStateWindowController>
{
    // Properties + Components
    #region
    [Header("Core Components")]
    [SerializeField] private GameObject mainVisualParent;
    [SerializeField] private CanvasGroup mainCg;
    [SerializeField] private StateCardBox[] stateCardBoxes;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Properties")]
    private bool windowIsInteractable = false;
    #endregion

    // View Logic
    #region
    private void ShowWindow()
    {
        windowIsInteractable = true;
        mainVisualParent.SetActive(true);
        mainCg.alpha = 0;
        mainCg.DOKill();
        mainCg.DOFade(1, 0.25f);
    }
    private void HideWindow()
    {
        windowIsInteractable = false;
        mainCg.alpha = 1;
        mainCg.DOKill();
        Sequence s = DOTween.Sequence();
        s.Append(mainCg.DOFade(0, 0.15f));
        s.OnComplete(() => mainVisualParent.SetActive(false));
    }
    public void HideWindowInstant()
    {
        windowIsInteractable = false;
        mainCg.alpha = 0;
        mainCg.DOKill();
        mainVisualParent.SetActive(false);
    }
    #endregion

    // Build Page Logic
    #region
    public void BuildAndShowWindowInShrineEvent(List<StateData> shrineStates)
    {
        // build window logic here
        BuildStateCardBoxesFromStateDataSet(shrineStates);
        ShowWindow();
    }
    private void BuildStateCardBoxesFromStateDataSet(List<StateData> states)
    {
        Debug.Log("ChooseStateWindowController.BuildStateCardBoxesFromStateDataSet() called...");
        for(int i = 0; i < states.Count; i++)
        {
            BuildStateCardBox(stateCardBoxes[i], states[i]);
        }
    }
    private void BuildStateCardBox(StateCardBox box, StateData data)
    {
        box.myStateData = data;
        CardController.Instance.BuildCardViewModelFromStateData(data, box.cvm);
    }
    #endregion

    // Handle State Card Events
    #region
    public void HandleStateCardInShrineEventClick(StateCardBox stateBoxClicked)
    {
        // Prevent double clicks on state cards
        if (!windowIsInteractable)
            return;
        
        // Add state to player states
        StateController.Instance.GivePlayerState(stateBoxClicked.myStateData);

        // TO DO: Do some nice 'gain new state' visual effects

        // Enable continue button in shrine scene for next event
        ShrineController.Instance.ShowContinueButton();

        // Hide + fade choose states window
        HideWindow();

    }
    #endregion
}
