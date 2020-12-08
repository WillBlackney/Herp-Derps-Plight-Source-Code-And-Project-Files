using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardInfoPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    // Proeprties + Component References
    #region
    [Header("Properties")]
    [HideInInspector] public CardData cardDataRef;
    [HideInInspector] public int copiesCount = 0;
    [SerializeField] private CardPanelLocation location;
    [HideInInspector] public InventoryCardSlot myInventorySlot;

    [Header("Text Components")]
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI energyCostText;
    public TextMeshProUGUI copiesCountText;

    [Header("Image Components")]
    [SerializeField] private Image talentOverlay;
    [SerializeField] private Image[] rarityOverlays;
    [SerializeField] private Image cardTypeImage;

    [Header("Drag Components")]
    private Canvas dragCanvas;
    private RectTransform dragTransform;
    private bool currentlyBeingDragged = false;
    #endregion

    // Setup + Initialization
    #region
    public void BuildCardInfoPanelFromCardData(CardData data)
    {
        cardDataRef = data;
        cardNameText.text = data.cardName;
        if(data.upgradeLevel >= 1)
        {
            cardNameText.color = ColorLibrary.Instance.cardUpgradeFontColor;
        }
        else
        {
            cardNameText.color = Color.white;
        }

        energyCostText.text = data.cardBaseEnergyCost.ToString();

        talentOverlay.color = ColorLibrary.Instance.GetTalentColor(data.talentSchool);
        talentOverlay.color = new Color(talentOverlay.color.r, talentOverlay.color.g, talentOverlay.color.b, 1);

        foreach(Image i in rarityOverlays)
        {
            i.color = ColorLibrary.Instance.GetRarityColor(data.rarity);
            i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        }       

        cardTypeImage.sprite = SpriteLibrary.Instance.GetCardTypeImageFromTypeEnumData(data.cardType);     

        copiesCount++;
        copiesCountText.text = "x" + copiesCount.ToString();
    }
    #endregion

    // Input listeners
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(cardDataRef != null)
        {
            KeyWordLayoutController.Instance.BuildAllViewsFromKeyWordModels(cardDataRef.keyWordModels);
        }       

        if (location == CardPanelLocation.CharacterInfoWindow)
        {
            MainMenuController.Instance.BuildAndShowCardViewModelPopup(cardDataRef);
        }
        else if (location == CardPanelLocation.ChooseCardScreen)
        {
            LootController.Instance.BuildAndShowCardViewModelPopup(cardDataRef);
        }
        else if (location == CardPanelLocation.RecruitCharacterScreen)
        {
            RecruitCharacterController.Instance.BuildAndShowCardViewModelPopup(cardDataRef);
        }
        else if (location == CardPanelLocation.CharacterRosterScreenDeck && CharacterRosterViewController.Instance.currentlyDraggingSomePanel == false)
        {
            CharacterRosterViewController.Instance.BuildAndShowCardViewModelPopupFromDeck(cardDataRef);
        }
        else if (location == CardPanelLocation.CharacterRosterScreenCardInventory && CharacterRosterViewController.Instance.currentlyDraggingSomePanel == false)
        {
            CharacterRosterViewController.Instance.BuildAndShowCardViewModelPopupFromInventory(cardDataRef);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (location == CardPanelLocation.CharacterInfoWindow)
        {
            MainMenuController.Instance.HidePreviewCard();
        }
        else if (location == CardPanelLocation.ChooseCardScreen)
        {
            LootController.Instance.HidePreviewCard();
        }
        else if (location == CardPanelLocation.RecruitCharacterScreen)
        {
            RecruitCharacterController.Instance.HidePreviewCard();
        }
        else if (location == CardPanelLocation.CharacterRosterScreenDeck)
        {
            CharacterRosterViewController.Instance.HidePreviewCardInDeck();
        }
        else if (location == CardPanelLocation.CharacterRosterScreenCardInventory)
        {
            CharacterRosterViewController.Instance.HidePreviewCardInInventory();
        }

        KeyWordLayoutController.Instance.FadeOutMainView();

    }
    #endregion

    // Drag Listeners
    public void OnDrag(PointerEventData eventData)
    {
       if(location == CardPanelLocation.CharacterRosterScreenCardInventory)
       {
            // On drag start logic
            if (currentlyBeingDragged == false)
            {
                currentlyBeingDragged = true;
                CharacterRosterViewController.Instance.currentlyDraggingSomePanel = true;

                // Play dragging SFX
                AudioManager.Instance.FadeInSound(Sound.Card_Dragging, 0.2f);

                // Hide card preview
                CharacterRosterViewController.Instance.HidePreviewCardInInventory();

                // Make deck box glow
                CharacterRosterViewController.Instance.StartDragDropAnimation();
            }           

            // Unparent from vert fitter, so it wont be masked while dragging
            transform.SetParent(CharacterRosterViewController.Instance.DragParent);

            // Get the needec components, if we dont have them already
            if(dragCanvas == null)
            {
                dragCanvas = CharacterRosterViewController.Instance.MainVisualParent.GetComponent<Canvas>();
            }
            if(dragTransform == null)
            {
                dragTransform = CharacterRosterViewController.Instance.MainVisualParent.transform as RectTransform;
            }

            // Weird hoki poki magic for dragging in local space on a non screen overlay canvas
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(dragTransform, Input.mousePosition,
                dragCanvas.worldCamera, out pos);

            // Follow the mouse
            transform.position = dragCanvas.transform.TransformPoint(pos);

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(location != CardPanelLocation.CharacterRosterScreenCardInventory)
        {
            return;
        }

        currentlyBeingDragged = false;
        CharacterRosterViewController.Instance.currentlyDraggingSomePanel = false;

        // Stop dragging SFX
        AudioManager.Instance.FadeOutSound(Sound.Card_Dragging, 0.2f);

        // Stop deck glow
        CharacterRosterViewController.Instance.StopDragDropAnimation();

        // Was the drag succesful?
        if (DragSuccessful())
        {
            // to do: on drag success
            // add card to character deck
            // hide draging card and slot
            // create new card panel slot in deck view 

            // Card added SFX
            AudioManager.Instance.PlaySound(Sound.GUI_Chime_1);

            // Add card to character persisent deck 
            CharacterDataController.Instance.AddCardToCharacterDeck
                (CharacterRosterViewController.Instance.CurrentCharacterViewing, CardController.Instance.CloneCardDataFromCardData(cardDataRef));

            // Remove card from inventory
            InventoryController.Instance.RemoveCardFromInventory(cardDataRef);

            // Hide inventory card and its slot
            transform.SetParent(myInventorySlot.transform);
            transform.localPosition = Vector3.zero;
            myInventorySlot.Hide();

            // Rebuild views
            CharacterRosterViewController.Instance.BuildCharacterDeckBoxFromData(CharacterRosterViewController.Instance.CurrentCharacterViewing);
        }
                
        else
        {
            // Move back towards slot position
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOMove(myInventorySlot.transform.position, 0.25f));

            // Re-parent self on arrival
            s.OnComplete(() => transform.SetParent(myInventorySlot.transform));
        }

    }

    public bool DragSuccessful()
    {
        bool bRet = false;

        if (CharacterRosterViewController.Instance.MouseIsOverDeckView && 
            LootController.Instance.IsCardLootableByCharacter(CharacterRosterViewController.Instance.CurrentCharacterViewing, cardDataRef))
        {
            bRet = true;
        }

        return bRet;
    }

}

public enum CardPanelLocation
{
    None = 0,
    CharacterInfoWindow = 1,
    ChooseCardScreen = 2,
    RecruitCharacterScreen = 3,
    CharacterRosterScreenDeck = 4,
    CharacterRosterScreenCardInventory = 5,
}
