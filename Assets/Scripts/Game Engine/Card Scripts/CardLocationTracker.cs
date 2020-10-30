using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// an enum to store the info about where this object is
public enum VisualStates
{
    Transition,
    Hand, 
    Dragging
}

public class CardLocationTracker : MonoBehaviour {

    // reference to a HoverPreview Component
    [SerializeField] private HoverPreview hoverPreview;

    // reference to a canvas on this object to set sorting order
    [SerializeField] private Canvas canvas;

    // a value for canvas sorting order when we want to show this object above everything
    private int TopSortingOrder = 1100;

    [SerializeField] private int baseHandSortingOrder;

    // PROPERTIES
    private int slot = -1;
    public int Slot
    {
        get{ return slot;}

        set
        {
            slot = value;
        }
    }

    [SerializeField] private VisualStates state;
    public VisualStates VisualState
    {
        get{ return state; }  

        set
        {
            state = value;
            switch (state)
            {
                case VisualStates.Hand:
                    hoverPreview.ThisPreviewEnabled = true;
                    break;
                case VisualStates.Transition:
                    hoverPreview.ThisPreviewEnabled = false;
                    break;
                case VisualStates.Dragging:
                    hoverPreview.ThisPreviewEnabled = false;
                    break;
            }
        }
    }

    public void BringToFront()
    {
        canvas.sortingOrder = TopSortingOrder;
    }

    // not setting sorting order inside of VisualStaes property because when the card is drawn, 
    // we want to set an index first and set the sorting order only when the card arrives to hand. 
    public void SetHandSortingOrder()
    {
        if (slot != -1)
            canvas.sortingOrder = HandSortingOrder(slot);
        //canvas.sortingLayerName = "Cards";
        canvas.overrideSorting = true;

       // SetHandRotation(slot);
    }

    private int HandSortingOrder(int placeInHand)
    {
        return baseHandSortingOrder + (-(placeInHand + 1) * 10); 
    }

}
