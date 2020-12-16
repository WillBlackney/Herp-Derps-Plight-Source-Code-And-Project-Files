using Sirenix.OdinInspector;
using Spriter2UnityDX;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterEntityView : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Misc Properties")]
    [HideInInspector] public CharacterEntityModel character;
    public CampSiteCharacterView campCharacter;
    [HideInInspector] public bool blockMouseOver = false;
    public EventSetting eventSetting = EventSetting.Combat;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("World Space Canvas References")]   
    public Canvas worldSpaceCanvas;
    public CanvasGroup worldSpaceCG;
    public Transform worldSpaceCanvasParent;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("GUI Canvas References")]
    public Canvas uiCanvas;
    public GameObject uiCanvasParent;
    public CanvasGroup uiCanvasCg;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Block GUI References")]
    public GameObject blockIcon;
    public TextMeshProUGUI blockText;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Health GUI References")]
    public Slider healthBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI maxHealthText;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Resistance GUI References")]
    public GameObject physicalResistanceParent;
    public TextMeshProUGUI physicalResistanceText;
    public GameObject magicResistanceParent;
    public TextMeshProUGUI magicResistanceText;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Energy GUI References")]
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI staminaText;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("UCM References")]
    public GameObject ucmVisualParent;
    public GameObject ucmSizingParent;
    public GameObject ucmShadowParent;
    public CanvasGroup ucmShadowCg;
    public GameObject ucmMovementParent;
    public UniversalCharacterModel ucm;    
    public Animator ucmAnimator;
    public EntityRenderer entityRenderer;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Non Player Components")]
    public IntentViewModel myIntentViewModel;
    public TextMeshProUGUI intentPopUpDescriptionText;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Custom Components")]
    public TargetIndicator myTargetIndicator;
    [HideInInspector] public ActivationWindow myActivationWindow;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Card Components")]
    public HandVisual handVisual;
    public TextMeshProUGUI drawPileCountText;
    public TextMeshProUGUI discardPileCountText;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Core Component References")]
    public GameObject passiveIconsVisualParent;
    [HideInInspector] public List<PassiveIconView> passiveIcons;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    #endregion

    // Getters
    #region
    public Vector3 WorldPosition
    {
        get { return ucmMovementParent.transform.position; }
    }

    #endregion

    // Mouse + Input Logic
    #region
    private void OnMouseEnter()
    {
        Debug.Log("CharacterEntityView.OnMouseEnter called...");
        if(eventSetting == EventSetting.Combat)
        {
            CharacterEntityController.Instance.OnCharacterMouseEnter(this);
        }
        else if (eventSetting == EventSetting.Camping)
        {
            CampSiteController.Instance.OnCampCharacterMouseEnter(campCharacter);
        }
    }
    private void OnMouseExit()
    {
        Debug.Log("CharacterEntityView.OnMouseExit called...");
        if (eventSetting == EventSetting.Combat)
        {
            CharacterEntityController.Instance.OnCharacterMouseExit(this);
        }
        else if (eventSetting == EventSetting.Camping)
        {
            CampSiteController.Instance.OnCampCharacterMouseExit(campCharacter);
        }
    }
    private void OnMouseOver()
    {
        if (GlobalSettings.Instance.deviceMode == DeviceMode.Desktop &&
            Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // did player lift the finger off the screen?
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (eventSetting == EventSetting.Combat)
                {
                    CharacterEntityController.Instance.OnCharacterMouseExit(this);
                }
                else if (eventSetting == EventSetting.Camping)
                {
                    CampSiteController.Instance.OnCampCharacterMouseExit(campCharacter);
                }
            }
        }
    }
    #endregion

    // On Button Clicked
    #region
    public void OnDiscardPileButtonClicked()
    {
        EventSystem.current.SetSelectedGameObject(null);

        if (character != null)
        {
            CardController.Instance.CreateNewShowDiscardPilePopup(character.discardPile);
        }
    }
    public void OnDrawPileButtonClicked()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (character != null)
        {
            CardController.Instance.CreateNewShowDrawPilePopup(character.drawPile);
        }
    }
    #endregion

}
