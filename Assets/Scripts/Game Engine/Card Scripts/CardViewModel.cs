using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardViewModel : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("General Properties")]
    public Card card;

    [Header("General Components")]
    public Transform mainParent;
    public CardViewModel myPreviewCard;
    public bool isPreviewCard;

    [Header("Core GUI Components")]
    public CardLocationTracker locationTracker;
    public DraggingActions draggingActions;
    public Draggable draggable;
    public HoverPreview hoverPreview;

    [Header("Text References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI energyText;

    [Header("Image References")]
    public Image graphicImage;
    public Image talentSchoolImage;
    public Image cardTypeImage;
    public GameObject talentSchoolParent;

    [Header("Colouring References")]
    public List<Image> talentRenderers;
    public List<Image> rarityRenderers;

    [Header("Canvas References")]
    public Canvas canvas;
    #endregion

    private void OnEnable()
    {
        if (isPreviewCard)
        {
            CardController.Instance.SetUpCardViewModelPreviewCanvas(this);
        }
    }
}
