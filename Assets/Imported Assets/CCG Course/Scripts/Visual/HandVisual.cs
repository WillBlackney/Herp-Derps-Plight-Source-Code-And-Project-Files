using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class HandVisual : MonoBehaviour
{
    // PUBLIC FIELDS
    public AreaPosition owner;
    public bool TakeCardsOpenly = true;
    public SameDistanceChildren slots;
    public Defender defenderOwner;

    [Header("Transform References")]
    public Transform DrawPreviewSpot;
    public Transform DeckTransform;
    public Transform DiscardPileTransform;
    public Transform OtherCardDrawSourceTransform;
    public Transform PlayPreviewSpot;

    // PRIVATE : a list of all card visual representations as GameObjects
    public List<GameObject> CardsInHand = new List<GameObject>();

    // Get + Remove Cards
    #region
    public void AddCard(GameObject card)
    {
        // we allways insert a new card as 0th element in CardsInHand List 
        CardsInHand.Insert(0, card);

        // parent this card to our Slots GameObject
        card.transform.SetParent(slots.transform);

        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    // remove a card GameObject from hand
    public void RemoveCard(GameObject card)
    {
        if (CardsInHand.Contains(card))
        {
            // remove a card from the list
            CardsInHand.Remove(card);

            // re-calculate the position of the hand
            PlaceCardsOnNewSlots();
            UpdatePlacementOfSlots();
        }       
    }

    // remove card with a given index from hand
    public void RemoveCardAtIndex(int index)
    {
        CardsInHand.RemoveAt(index);
        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    // get a card GameObject with a given index in hand
    public GameObject GetCardAtIndex(int index)
    {
        return CardsInHand[index];
    }
    #endregion

    // Slot Logic
    #region
    public void UpdatePlacementOfSlots()
    {
        float posX;
        if (CardsInHand.Count > 0)
            posX = (slots.Children[0].transform.localPosition.x - slots.Children[CardsInHand.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        // tween Slots GameObject to new position in 0.3 seconds
        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);  
    }
    public void PlaceCardsOnNewSlots()
    {
        foreach (GameObject g in CardsInHand)
        {
            // tween this card to a new Slot
            g.transform.DOLocalMoveX(slots.Children[CardsInHand.IndexOf(g)].transform.localPosition.x, 0.3f);

            // apply correct sorting order and HandSlot value for later 
            WhereIsTheCardOrCreature w = g.GetComponent<WhereIsTheCardOrCreature>();
            w.Slot = CardsInHand.IndexOf(g);
            w.SetHandSortingOrder();
        }
    }
    #endregion

    // CARD DRAW METHODS
    public void GivePlayerACard(Card c, bool fast = false, bool fromDeck = true)
    {
        GameObject card;
        card = CardController.Instance.BuildCardViewModelFromCard(c, DeckTransform.position).gameObject;

        // pass this card to HandVisual class
        AddCard(card);

        // Bring card to front while it travels from draw spot to hand
        WhereIsTheCardOrCreature w = card.GetComponent<WhereIsTheCardOrCreature>();
        w.BringToFront();
        w.Slot = 0;
        w.VisualState = VisualStates.Transition;

        // move card to the hand;
        Sequence s = DOTween.Sequence();
        if (!fast)
        {
            // Debug.Log ("Not fast!!!");
            s.Append(card.transform.DOMove(DrawPreviewSpot.position, GlobalSettings.Instance.CardTransitionTime));
            if (TakeCardsOpenly)
                s.Insert(0f, card.transform.DORotate(Vector3.zero, GlobalSettings.Instance.CardTransitionTime));
            //else 
            //s.Insert(0f, card.transform.DORotate(new Vector3(0f, -179f, 0f), GlobalSettings.Instance.CardTransitionTime)); 
            s.AppendInterval(GlobalSettings.Instance.CardPreviewTime);
            // displace the card so that we can select it in the scene easier.
            s.Append(card.transform.DOLocalMove(slots.Children[0].transform.localPosition, GlobalSettings.Instance.CardTransitionTime));
        }
        else
        {
            // displace the card so that we can select it in the scene easier.
            s.Append(card.transform.DOLocalMove(slots.Children[0].transform.localPosition, GlobalSettings.Instance.CardTransitionTimeFast));
            if (TakeCardsOpenly)
                s.Insert(0f, card.transform.DORotate(Vector3.zero, GlobalSettings.Instance.CardTransitionTimeFast));
        }

        s.OnComplete(() => ChangeLastCardStatusToInHand(card, w));
    }
    
    // this method will be called when the card arrived to hand 
    public void ChangeLastCardStatusToInHand(GameObject card, WhereIsTheCardOrCreature w)
    {
        // set correct sorting order
        w.SetHandSortingOrder();
        // end command execution for DrawACArdCommand
        Command.CommandExecutionComplete();
    }

    // Play cards from hand
    #region
    public void PlayASpellFromHand(int CardID)
    {
        GameObject card = IDHolder.GetGameObjectWithID(CardID);
        PlayASpellFromHand(card);
    }
    public void PlayASpellFromHand(CardViewModel cardVM)
    {
        Debug.Log("HandVisual.PlayASpellFromHand() called...");

        Command.CommandExecutionComplete();
        cardVM.GetComponent<WhereIsTheCardOrCreature>().VisualState = VisualStates.Transition;
        RemoveCard(cardVM.gameObject);

        cardVM.transform.SetParent(null);

        Sequence seqOne = DOTween.Sequence();
        seqOne.Append(cardVM.transform.DOMove(DiscardPileTransform.position, 0.5f));
        seqOne.OnComplete(() =>
        {
            CardController.Instance.DestroyCardViewModel(cardVM);
        });
        /*
        Sequence seqOne = DOTween.Sequence();
        seqOne.Append(cardVM.transform.DOMove(PlayPreviewSpot.position, 0.5f));
        seqOne.Insert(0f, cardVM.transform.DORotate(Vector3.zero, 0.5f));
        seqOne.AppendInterval(1f);
        seqOne.OnComplete(()=>
            {
                Sequence seqTwo = DOTween.Sequence();
                seqTwo.Append(cardVM.transform.DOMove(DiscardPileTransform.position, 0.5f));
                seqTwo.OnComplete(() =>
                {
                    CardController.Instance.DestroyCardViewModel(cardVM);
                });
            });
            */
    }
    public void PlayASpellFromHand(GameObject cardVMGO)
    {
        CardViewModel cardVM = cardVMGO.GetComponent<CardViewModel>();

        Command.CommandExecutionComplete();
        cardVM.GetComponent<WhereIsTheCardOrCreature>().VisualState = VisualStates.Transition;
        RemoveCard(cardVM.gameObject);

        cardVM.transform.SetParent(null);

        Sequence seqOne = DOTween.Sequence();
        seqOne.Append(cardVM.transform.DOMove(DiscardPileTransform.position, 0.5f));
        seqOne.Insert(0f, cardVM.transform.DORotate(Vector3.zero, 0.5f));
        seqOne.AppendInterval(1f);

        seqOne.OnComplete(() =>
        {
            Sequence seqTwo = DOTween.Sequence();
            seqTwo.Append(cardVM.transform.DOMove(DiscardPileTransform.position, 0.5f));
            seqTwo.OnComplete(() =>
            {
                CardController.Instance.DestroyCardViewModel(cardVM);
            });

        });

      
    }
    #endregion


}
