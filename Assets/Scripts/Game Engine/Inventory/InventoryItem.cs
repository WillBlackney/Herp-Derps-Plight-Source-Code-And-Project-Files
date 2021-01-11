using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    // Properties + Component Refs
    #region
    [HideInInspector] public bool currentlyBeingDragged = false;
    private Canvas dragCanvas;
    private RectTransform dragTransform;
    [HideInInspector] public ItemData itemDataRef;
    public InventoryItemSlot myInventorySlot;
    public Image itemImage;
    
    #endregion

    // Set up
    #region
    public void BuildInventoryItemFromItemData(ItemData data)
    {
        itemDataRef = data;
        itemImage.sprite = data.itemSprite;
    }
    #endregion

    // Pointer Logic
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemDataRef == null)
            return;

        CharacterRosterViewController.Instance.BuildAndShowCardViewModelPopupFromInventoryItem(itemDataRef);
        KeyWordLayoutController.Instance.BuildAllViewsFromKeyWordModels(itemDataRef.keyWordModels);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        CharacterRosterViewController.Instance.HidePreviewItemCardInInventory();
        KeyWordLayoutController.Instance.FadeOutMainView();
    }
    #endregion

    // Drag Logic
    #region
    public void OnDrag(PointerEventData eventData)
    {
        // On drag start logic
        if (currentlyBeingDragged == false)
        {
            currentlyBeingDragged = true;
            //CharacterRosterViewController.Instance.currentlyDraggingSomePanel = true;

            // Play dragging SFX
            AudioManager.Instance.FadeInSound(Sound.Card_Dragging, 0.2f);

            // Hide card preview
            CharacterRosterViewController.Instance.HidePreviewItemCardInInventory();
        }

        // Unparent from vert fitter, so it wont be masked while dragging
        transform.SetParent(CharacterRosterViewController.Instance.DragParent);

        // Get the needec components, if we dont have them already
        if (dragCanvas == null)
            dragCanvas = CharacterRosterViewController.Instance.MainVisualParent.GetComponent<Canvas>();
        
        if (dragTransform == null)
            dragTransform = CharacterRosterViewController.Instance.MainVisualParent.transform as RectTransform;        

        // Weird hoki poki magic for dragging in local space on a non screen overlay canvas
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(dragTransform, Input.mousePosition,
            dragCanvas.worldCamera, out pos);

        // Follow the mouse
        transform.position = dragCanvas.transform.TransformPoint(pos);

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        currentlyBeingDragged = false;
        //CharacterRosterViewController.Instance.currentlyDraggingSomePanel = false;

        // Stop dragging SFX
        AudioManager.Instance.FadeOutSound(Sound.Card_Dragging, 0.2f);

        // Stop deck glow
        //CharacterRosterViewController.Instance.StopDragDropAnimation();

        // Was the drag succesful?
        if (DragSuccessful())
        {
            // Card added SFX
            AudioManager.Instance.PlaySound(Sound.GUI_Chime_1);

            // add item to player
            ItemController.Instance.HandleGiveItemToCharacterFromInventory
                (CharacterRosterViewController.Instance.CurrentCharacterViewing, itemDataRef, CharacterRosterViewController.Instance.rosterSlotMousedOver);

            // re build roster, inventory and model views
            CharacterRosterViewController.Instance.BuildCharacterItemSlotsFromData(CharacterRosterViewController.Instance.CurrentCharacterViewing);
            CharacterRosterViewController.Instance.BuildInventoryItemSlots();
            CharacterModelController.Instance.ApplyItemManagerDataToCharacterModelView
                (CharacterRosterViewController.Instance.CurrentCharacterViewing.itemManager, CharacterRosterViewController.Instance.CharacterModel);
        }

        else
        {
            // Move back towards slot position
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOMove(myInventorySlot.transform.position, 0.25f));

            // Re-parent self on arrival
            s.OnComplete(() => transform.SetParent(myInventorySlot.transform));
        }

        // Hide card previews, just in case
        CharacterRosterViewController.Instance.HidePreviewItemCardInInventory();
        CharacterRosterViewController.Instance.HidePreviewItemCardInRoster();
    }
    public bool DragSuccessful()
    {
        bool bRet = false;

        if(CharacterRosterViewController.Instance.IsItemValidOnSlot(this, CharacterRosterViewController.Instance.rosterSlotMousedOver))
        {
            bRet = true;
        }
        
        return bRet;
    }
    #endregion
}
