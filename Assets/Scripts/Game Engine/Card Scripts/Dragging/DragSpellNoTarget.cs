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
            CardController.Instance.PlayCardFromHand(cardVM.card);
        }
        else
        {
            // Set old sorting order 
            locationTracker.Slot = savedHandSlot;
            locationTracker.VisualState = VisualStates.Hand;
            locationTracker.SetHandSortingOrder();

            // Move this card back to its slot position
            HandVisual PlayerHand = cardVM.card.owner.characterEntityView.handVisual;
            Vector3 oldCardPos = PlayerHand.slots.Children[savedHandSlot].transform.localPosition;
            cardVM.movementParent.transform.DOLocalMove(oldCardPos, 0.25f);            
        } 
    }

    protected override bool DragSuccessful()
    {
        Debug.Log("DragSpellNoTarget.DragSuccessful() called...");
        return CardController.Instance.IsCursorOverTable();
    }


}


