using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class HandVisual : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Component References")]
    public SameDistanceChildren slots;

    [Header("Transform References")]
    public Transform DeckTransform;
    public Transform DiscardPileTransform;
    public Transform NonDeckCardCreationTransform;

    [Header("Variables")]
    private List<GameObject> cardsInHand = new List<GameObject>();
    #endregion

    // Get + Remove Cards
    #region
    public void AddCard(GameObject card)
    {
        // we allways insert a new card as 0th element in CardsInHand List 
        cardsInHand.Insert(0, card);

        // parent this card to our Slots GameObject
        card.transform.SetParent(slots.transform);

        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }
    public void RemoveCard(GameObject card)
    {
        if (cardsInHand.Contains(card))
        {
            // remove a card from the list
            cardsInHand.Remove(card);

            // re-calculate the position of the hand
            PlaceCardsOnNewSlots();
            UpdatePlacementOfSlots();
        }
    }
    #endregion

    // Slot Logic
    #region
    private void UpdatePlacementOfSlots()
    {
        float posX;
        if (cardsInHand.Count > 0)
            posX = (slots.Children[0].transform.localPosition.x - slots.Children[cardsInHand.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        // tween Slots GameObject to new position in 0.3 seconds
        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);  
    }
    private void PlaceCardsOnNewSlots()
    {
        foreach (GameObject g in cardsInHand)
        {
            // tween this card to a new Slot
            g.transform.DOLocalMoveX(slots.Children[cardsInHand.IndexOf(g)].transform.localPosition.x, 0.3f);

            // apply correct sorting order and HandSlot value for later 
            CardLocationTracker w = g.GetComponent<CardLocationTracker>();
            if(w == null)
            {
                Debug.Log("CardLocationTracker was null on game object card, searching in children...");
                w = g.GetComponentInChildren<CardLocationTracker>();
            }
            w.Slot = cardsInHand.IndexOf(g);
            w.SetHandSortingOrder();
        }
    }
    #endregion




}
