using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class CardViewModel : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("General Properties")]
    [HideInInspector] public Card card;
    [HideInInspector] public CampCard campCard;
    [HideInInspector] public EventSetting eventSetting;

    [Header("General Components")]
    public Transform movementParent;
    public CardViewModel myPreviewCard;
    public bool isPreviewCard;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Core GUI Components")]
    public CardLocationTracker locationTracker;
    public DraggingActions draggingActions;
    public Draggable draggable;
    public HoverPreview hoverPreview;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Text References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI energyText;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Image References")]
    public Image graphicImage;
    public Image talentSchoolImage;
    public Image[] cardTypeImages;
    public GameObject cardTypeParent;
    public GameObject talentSchoolParent;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Glow Outline References")]
    public Animator glowAnimator;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Colouring References")]
    public Image[] talentRenderers;
    public Image[] rarityRenderers;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Canvas References")]
    public Canvas canvas;
    public CanvasGroup cg;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    #endregion

    private void OnEnable()
    {
        if (isPreviewCard)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = 1000;
        }
    }
}

public enum EventSetting
{
    Combat = 0,
    Camping = 1,
    Shop = 2,
}
