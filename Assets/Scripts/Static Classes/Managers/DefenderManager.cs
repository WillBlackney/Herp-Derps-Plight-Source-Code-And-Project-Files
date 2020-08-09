using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderManager : MonoBehaviour
{
    [Header("Component References")]
    public GameObject defendersParent;

    [Header("Properties")]
    public Defender selectedDefender;
    public List<Defender> allDefenders = new List<Defender>();
    public List<Tile> spawnLocations = new List<Tile>();

    public static DefenderManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Defender Selection
    #region
    public void SetSelectedDefender(Defender defender)
    {
        // if we have already have a defender selected when we click on another defender, unselect that defender, then select the new defender
        if(selectedDefender != defender && selectedDefender != null)
        {
            Debug.Log("Clearing selected defender");
            selectedDefender.UnselectDefender();
            LevelManager.Instance.UnhighlightAllTiles();
        }

        selectedDefender = defender;
        if (ActivationManager.Instance.IsEntityActivated(selectedDefender) == false)
        {
            UIManager.Instance.SetEndTurnButtonText("Not Your Activation!");
            UIManager.Instance.SetEndTurnButtonSprite(UIManager.Instance.EndTurnButtonDisabledSprite);
            UIManager.Instance.DisableEndTurnButtonInteractions();
        }
        else if (ActivationManager.Instance.IsEntityActivated(selectedDefender))
        {
            UIManager.Instance.SetEndTurnButtonText("End Activation");
            UIManager.Instance.SetEndTurnButtonSprite(UIManager.Instance.EndTurnButtonEnabledSprite);
            UIManager.Instance.EnableEndTurnButtonInteractions();
        }
        CameraManager.Instance.SetCameraLookAtTarget(selectedDefender.gameObject);
        Debug.Log("Selected defender: " + selectedDefender.gameObject.name);
    }
    public void ClearSelectedDefender()
    {
        if(selectedDefender != null)
        {
            selectedDefender.UnselectDefender();
            selectedDefender = null;
        }
        CameraManager.Instance.ClearCameraLookAtTarget();
        LevelManager.Instance.UnhighlightAllTiles();
    }
    #endregion

    // Misc Logic
    #region
    public void DestroyAllDefenders()
    {
        List<Defender> allDefs = new List<Defender>();
        allDefs.AddRange(allDefenders);

        foreach(Defender defender in allDefenders)
        {
            LivingEntityManager.Instance.allLivingEntities.Remove(defender);
        }       

        foreach(Defender defender in allDefs)
        {
            if (allDefenders.Contains(defender))
            {
                allDefenders.Remove(defender);
                Destroy(defender.gameObject);
            }
        }

        allDefenders.Clear();
    }
    #endregion

    // Card Game Logic
    public void DrawCardFromDrawPile(Defender defender)
    {
        /*
        if (defender.deck.cards.Count > 0)
        {
            if (defender.hand.CardsInHand.Count < 10
               // PArea.handVisual.slots.Children.Length
                )
            {
                // 1) logic: add card to hand
                CardLogic newCard = new CardLogic(defender.deck.cards[0]);
                //newCard.owner = this;
                newCard.myDefenderOwner = defender;
                defender.hand.CardsInHand.Insert(0, newCard);
                // Debug.Log(hand.CardsInHand.Count);
                // 2) logic: remove the card from the deck
                defender.deck.cards.RemoveAt(0);
                // 2) create a command
                new DrawACardCommand(defender.hand.CardsInHand[0], defender, true, fromDeck: true).AddToQueue();
            }
        }
        else
        {
            // there are no cards in the deck, take fatigue damage.
        }
        */
    }
    public void DrawCardsOnActivationStart(Defender defender)
    {
        for (int i = 0; i < 5; i++)
        {
            DrawCardFromDrawPile(defender);
        }
    }
    public void PlayCardFromHand(Defender defender, OneCardManager card)
    {
        // need to get the card logic reference

        //ManaLeft -= playedCard.CurrentManaCost;
        // cause effect instantly:
        /*
        if (playedCard.effect != null)
            playedCard.effect.ActivateEffect(playedCard.ca.specialSpellAmount, target);
        else
        {
            Debug.LogWarning("No effect found on card " + playedCard.ca.name);
        }
        // no matter what happens, move this card to PlayACardSpot
        new PlayASpellCardCommand(this, playedCard).AddToQueue();
        // remove this card from hand
        hand.CardsInHand.Remove(playedCard);
        // check if this is a creature or a spell
        */
    }



}
