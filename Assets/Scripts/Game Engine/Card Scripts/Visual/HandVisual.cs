using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;

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
    public Transform PlayPreviewSpot;

    [Header("Variables")]
    private List<GameObject> cardsInHand = new List<GameObject>();
    #endregion

    // Get + Remove Cards
    #region
    public void AddCard(GameObject card)
    {
        // Always insert a new card as 0th element in CardsInHand List 
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
    public void ForceDestroyAllCards()
    {
        GameObject[] cards = cardsInHand.ToArray();

        foreach(GameObject card in cards)
        {
            Destroy(card);
        }

        cardsInHand.Clear();
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

        UpdateCardRotationsAndYDrops();
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

    // Rotation + Spreading Logic
    #region
    public void UpdateCardRotationsAndYDrops()
    {
        foreach (GameObject g in cardsInHand)
        {
            g.GetComponent<CardSlotHelper>().UpdateAngles(cardsInHand.IndexOf(g) + 1, (cardsInHand.Count / 2f) + 0.5f);
        }
    }
    private void UpdateRotationOfCards()
    {
        foreach (GameObject g in cardsInHand)
        {
          // g.GetComponent<CardSlotHelper>().UpdateRotation(cardsInHand.IndexOf(g) + 1, (cardsInHand.Count / 2f) + 0.5f);
        }

        /*
        int totalCards = cardsInHand.Count;
        float middleIndex =  totalCards / 2f;
        middleIndex += 0.5f;

        foreach (GameObject g in cardsInHand)
        {
            int myIndex = cardsInHand.IndexOf(g) + 1;
            float myDif = myIndex - middleIndex;

            // Rotate left or right
            if (myIndex < middleIndex || myIndex > middleIndex)            
                g.transform.DORotate(new Vector3(0, 0, 2.5f * myDif), 0.2f);            

            // Rotate as the centre card
            else            
                g.transform.DORotate(new Vector3(0, 0, 0), 0.2f);     
        }*/



    }
    private void UpdateYDropOfCards()
    {
        foreach (GameObject g in cardsInHand)
        {
            //g.GetComponent<CardSlotHelper>().UpdateYDrop(cardsInHand.IndexOf(g) + 1, (cardsInHand.Count / 2f) + 0.5f);
        }

        /*
        int totalCards = cardsInHand.Count;
        float slotStartY = 0f;
        float yStep = 0.1f;
        float middleIndex = totalCards / 2f;
        middleIndex += 0.5f;

        foreach (GameObject g in cardsInHand)
        {
            Transform t = g.transform.Find("Slot Fitter Parent");

            int myIndex = cardsInHand.IndexOf(g) + 1;
            float myDif = Mathf.Abs(myIndex - middleIndex);
            Debug.Log("myDif = " + myDif.ToString());
            t.DOLocalMoveY(slotStartY - (yStep * myDif), 0.2f);
        }
        */
    }


}
    #endregion
