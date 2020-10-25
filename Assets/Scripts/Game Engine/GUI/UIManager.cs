using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : Singleton<UIManager>
{
    // Properties + Component References
    #region
    [Header("End Turn Button Component References")]
    public CanvasGroup EndTurnButtonCG;
    public Button EndTurnButton;
    public Image EndTurnButtonBGImage;
    public TextMeshProUGUI EndTurnButtonText;
    public Sprite EndTurnButtonDisabledSprite;
    public Sprite EndTurnButtonEnabledSprite;

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
    public void SetEndTurnButtonSprite(Sprite newSprite)
    {
        EndTurnButtonBGImage.sprite = newSprite;
    }
    public void SetEndTurnButtonText(string newText)
    {
        EndTurnButtonText.text = newText;
    }
    public void SetPlayerTurnButtonState()
    {
        Debug.Log("UIManager.SetPlayerTurnButtonState() called...");
        EnableEndTurnButtonInteractions();
        VisualEventManager.Instance.CreateVisualEvent(() => SetEndTurnButtonText("End Activation"));
        VisualEventManager.Instance.CreateVisualEvent(() => SetEndTurnButtonSprite(EndTurnButtonEnabledSprite));
    }
    public void SetEnemyTurnButtonState()
    {
        Debug.Log("UIManager.SetEnemyTurnButtonState() called...");
        DisableEndTurnButtonInteractions();
        VisualEventManager.Instance.CreateVisualEvent(() => SetEndTurnButtonText("Enemy Activation..."));
        VisualEventManager.Instance.CreateVisualEvent(() => SetEndTurnButtonSprite(EndTurnButtonDisabledSprite));
    }
    #endregion

}
