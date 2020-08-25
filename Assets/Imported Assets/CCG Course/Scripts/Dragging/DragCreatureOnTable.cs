using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DragCreatureOnTable : DraggingActions {

    private int savedHandSlot;
    private CardLocationTracker whereIsCard;
    private IDHolder idScript;
    private VisualStates tempState;
    //private OneCardManager manager;

    public override bool CanDrag
    {
        get
        {
            // TEST LINE: this is just to test playing creatures before the game is complete 
            // return true;

            // TODO : include full field check
            // return base.CanDrag && cardVM.CanBePlayedNow;
            return true;
        }
    }

    void Awake()
    {
        whereIsCard = GetComponent<CardLocationTracker>();
        //manager = GetComponent<OneCardManager>();
    }

    public override void OnStartDrag()
    {
        savedHandSlot = whereIsCard.Slot;
        tempState = whereIsCard.VisualState;
        whereIsCard.VisualState = VisualStates.Dragging;
        whereIsCard.BringToFront();

    }

    public override void OnDraggingInUpdate()
    {

    }

    public override void OnEndDrag()
    {
       
    }

    protected override bool DragSuccessful()
    {
        return true;
    }
}
