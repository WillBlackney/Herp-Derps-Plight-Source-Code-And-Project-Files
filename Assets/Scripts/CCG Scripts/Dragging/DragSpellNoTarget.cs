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

    }

    public override void OnDraggingInUpdate()
    {
        
    }

    public override void OnEndDrag()
    {
        Debug.Log("DragSpellNoTarget.OnEndDrag() called...");

        // Check if we are holding a card over the table
        if (DragSuccessful())
        {
            Debug.Log("DragSpellNoTarget.OnEndDrag() detected succesful drag and drop, playing the card...");
            CardController.Instance.PlayCardFromHand(cardVM.card);
        }
        else
        {
            // TO DO IN FUTURE: make it so the character can make a no target
            // card return to hand IF IT IS NOT OVER THE TABLE
            // the logic would go here

            /*
            Debug.Log("DragSpellNoTarget.OnEndDrag() detected failed drag and drop, returning the card to hand...");
            // Set old sorting order 
            whereIsCard.Slot = savedHandSlot;
            if (tag.Contains("Low"))
                whereIsCard.VisualState = VisualStates.LowHand;
            else
                whereIsCard.VisualState = VisualStates.TopHand;
            // Move this card back to its slot position
            HandVisual PlayerHand = playerOwner.PArea.handVisual;
            Vector3 oldCardPos = PlayerHand.slots.Children[savedHandSlot].transform.localPosition;
            transform.DOLocalMove(oldCardPos, 1f);
            */
        } 
    }

    protected override bool DragSuccessful()
    {
        Debug.Log("DragSpellNoTarget.DragSuccessful() called...");
        return TableVisual.CursorOverSomeTable; 
    }


}
