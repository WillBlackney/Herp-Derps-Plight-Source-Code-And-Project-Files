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
    [Header("UI Component References")]    
    public GameObject GameOverScreenParent;
    public GameObject GameOverScreenCanvasParent;
    public TextMeshProUGUI GameOverScreenTitleText;
    public GameObject charRosterParticleParent;

    [Header("End Turn Button Component References")]
    public CanvasGroup EndTurnButtonCG;
    public Button EndTurnButton;
    public Image EndTurnButtonBGImage;
    public TextMeshProUGUI EndTurnButtonText;
    public Sprite EndTurnButtonDisabledSprite;
    public Sprite EndTurnButtonEnabledSprite;

    [Header("Character Roster Movement References")]
    public Canvas charRosterCanvasComponent;
    public RectTransform characterRosterCentrePosition;
    public RectTransform characterRosterOffScreenPosition;
    public RectTransform characterRosterTransformParent;
    public bool crMovingOnScreen;
    public bool crMovingOffScreen;
    public float characterRosterMoveSpeed;

    [Header("Inventory Movement References")]
    public Canvas inventoryCanvasComponent;
    public RectTransform inventoryCentrePosition;
    public RectTransform inventoryOffScreenPosition;
    public RectTransform inventoryTransformParent;
    public bool inventoryMovingOnScreen;
    public bool inventoryMovingOffScreen;
    public float inventoryMoveSpeed;

    [Header("World Map Movement References")]
    public Canvas worldMapCanvasComponent;
    public RectTransform worldMapCentrePosition;
    public RectTransform worldMapOffScreenPosition;
    public RectTransform worldMapTransformParent;
    public bool worldMapMovingOnScreen;
    public bool worldMapMovingOffScreen;
    public float worldMapMoveSpeed;
    #endregion

    // Visibility + View Logic
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
    #endregion

    // New Logic
    #region
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
