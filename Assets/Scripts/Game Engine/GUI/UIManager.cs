using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    // Properties + Component References
    #region
    [Header("End Turn Button Component References")]
    public CanvasGroup EndTurnButtonCG;
    public Button EndTurnButton;
    public Image EndTurnButtonBGImage;
    public TextMeshProUGUI EndTurnButtonText;
    public Color endTurnButtonDisabledColor;
    public Color endTurnButtonEnabledColor;

    [Header("Testing Game Over Component References")]
    public GameObject victoryPopup;
    public GameObject defeatPopup;
    public GameObject continueToNextEncounterButtonParent;

    #endregion

    // End Turn Button Logic
    #region
    public void DisableEndTurnButtonInteractions()
    {
        EndTurnButton.interactable = false;
    }
    public void EnableEndTurnButtonInteractions()
    {        
        EndTurnButton.interactable = true;
    }
    public void DisableEndTurnButtonView()
    {
        EndTurnButton.gameObject.SetActive(false);
        StartCoroutine(FadeOutEndTurnButton());
    }
    private IEnumerator FadeOutEndTurnButton()
    {
        EndTurnButtonCG.alpha = 1;
        float uiFadeSpeed = 10f;

        while (EndTurnButtonCG.alpha > 0)
        {
            EndTurnButtonCG.alpha -= 0.1f * uiFadeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    public void EnableEndTurnButtonView()
    {
        EndTurnButton.gameObject.SetActive(true);
        StartCoroutine(FadeInEndTurnButton());
    }
    private IEnumerator FadeInEndTurnButton()
    {
        EndTurnButtonCG.alpha = 0;
        float uiFadeSpeed = 10f;

        while (EndTurnButtonCG.alpha < 1)
        {
            EndTurnButtonCG.alpha += 0.1f * uiFadeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    public void SetEndTurnButtonColor(Color newColor)
    {
        EndTurnButtonBGImage.DOColor(newColor, 0.5f);
    }
    public void SetEndTurnButtonText(string newText)
    {
        EndTurnButtonText.text = newText;
    }
    public void SetPlayerTurnButtonState()
    {
        Debug.Log("UIManager.SetPlayerTurnButtonState() called...");
        EnableEndTurnButtonInteractions();
        SetEndTurnButtonText("End Activation");
        SetEndTurnButtonColor(endTurnButtonEnabledColor);
        //VisualEventManager.Instance.CreateVisualEvent(() => SetEndTurnButtonText("End Activation"));
        //VisualEventManager.Instance.CreateVisualEvent(() => SetEndTurnButtonColor(endTurnButtonEnabledColor));
    }
    public void SetEnemyTurnButtonState()
    {
        Debug.Log("UIManager.SetEnemyTurnButtonState() called...");
        DisableEndTurnButtonInteractions();
        SetEndTurnButtonText("Enemy Activation...");
        SetEndTurnButtonColor(endTurnButtonDisabledColor);
        //VisualEventManager.Instance.CreateVisualEvent(() => SetEndTurnButtonText("Enemy Activation..."));
       // VisualEventManager.Instance.CreateVisualEvent(() => SetEndTurnButtonColor(endTurnButtonDisabledColor));
    }
    #endregion

}
