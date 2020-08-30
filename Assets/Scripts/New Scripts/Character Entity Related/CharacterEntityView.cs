using Spriter2UnityDX;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterEntityView : MonoBehaviour
{
    [Header("Properties")]
    [HideInInspector] public CharacterEntityModel character;

    [Header("World Space Canvas References")]   
    public Canvas worldSpaceCanvas;
    public CanvasGroup worldSpaceCG;
    public Transform worldSpaceCanvasParent;

    [Header("GUI Canvas References")]
    public Canvas uiCanvas;
    public GameObject uiCanvasParent;
    public CanvasGroup uiCanvasCg;

    [Header("Block GUI References")]
    public GameObject blockIcon;
    public TextMeshProUGUI blockText;

    [Header("Health GUI References")]
    public Slider healthBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI maxHealthText;

    [Header("Energy GUI References")]
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI staminaText;

    [Header("UCM References")]
    public GameObject ucmParent;
    public UniversalCharacterModel ucm;    
    public Animator ucmAnimator;
    public EntityRenderer entityRenderer;

    [Header("Non Player Components")]
    public IntentViewModel myIntentViewModel;

    [Header("Custom Components")]
    public TargetIndicator myTargetIndicator;
    [HideInInspector] public ActivationWindow myActivationWindow;

    [Header("Card Components")]
    public HandVisual handVisual;

    [Header("Core Component References")]
    public GameObject passiveIconsVisualParent;
    [HideInInspector] public List<PassiveIconView> passiveIcons;

    // Mouse + Input Logic
    private void OnMouseEnter()
    {
        Debug.Log("CharacterEntityView.OnMouseEnter called...");
        CharacterEntityController.Instance.OnCharacterMouseEnter(this);
    }
    private void OnMouseExit()
    {
        Debug.Log("CharacterEntityView.OnMouseExit called...");
        CharacterEntityController.Instance.OnCharacterMouseExit(this);
    }
}
