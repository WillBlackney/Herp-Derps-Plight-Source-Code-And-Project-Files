using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DragSpellNoTarget: DraggingActions{

    private int savedHandSlot;

    public override bool CanDrag
    {
        get
        {
            Debug.Log("DragSpellNoTarget.CanDrag() called...");
            return base.CanDrag; 
        }
    }

    public override void OnStartDrag()
    {
        Debug.Log("DragSpellNoTarget.OnStartDrag() called...");

        savedHandSlot = locationTracker.Slot;

        locationTracker.VisualState = VisualStates.Dragging;
        locationTracker.BringToFront();

        // play sfx
        AudioManager.Instance.FadeInSound(Sound.Card_Dragging, 0.2f);
    }

    public override void OnDraggingInUpdate()
    {
        
    }

    public override void OnEndDrag()
    {
        Debug.Log("DragSpellNoTarget.OnEndDrag() called...");

        // Stop dragging SFX
        AudioManager.Instance.FadeOutSound(Sound.Card_Dragging, 0.2f);

        // Check if we are holding a card over the table
        if (DragSuccessful())
        {
            Debug.Log("DragSpellNoTarget.OnEndDrag() detected succesful drag and drop, playing the card...");

            if (cardVM.eventSetting == EventSetting.Combat)
            {
                CardController.Instance.PlayCardFromHand(cardVM.card);
            }
            else if (cardVM.eventSetting == EventSetting.Camping)
            {
                CampSiteController.Instance.PlayCampCardFromHand(cardVM.campCard);
            }

        }
        else
        {
            // Set old sorting order 
            locationTracker.Slot = savedHandSlot;
            locationTracker.VisualState = VisualStates.Hand;
            locationTracker.SetHandSortingOrder();

            if (cardVM.eventSetting == EventSetting.Combat)
            {
                // Move this card back to its slot position
                HandVisual PlayerHand = cardVM.card.owner.characterEntityView.handVisual;
                Vector3 oldCardPos = PlayerHand.slots.Children[savedHandSlot].transform.localPosition;
                cardVM.movementParent.transform.DOLocalMove(oldCardPos, 0.25f);
            }          

            else if (cardVM.eventSetting == EventSetting.Camping)
            {
                // Move this card back to its slot position
                Vector3 oldCardPos = CampSiteController.Instance.HandVisual.slots.Children[locationTracker.Slot].transform.localPosition;
                cardVM.movementParent.DOLocalMove(oldCardPos, 0.25f);
            }            
        } 
    }

    protected override bool DragSuccessful()
    {
        Debug.Log("DragSpellNoTarget.DragSuccessful() called...");
        return CardController.Instance.IsCursorOverTable();
    }


}


