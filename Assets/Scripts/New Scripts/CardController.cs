using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardController : MonoBehaviour
{
    [Header("Component References")]
    public Transform discardPilePosition;
    public Transform drawPilePosition;

    // Singleton Pattern
    #region
    public static CardController Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Build Cards, Decks, View Models and Data
    #region
    public void BuildDefenderDeckFromDeckData(Defender defender, List<CardDataSO> deckData)
    {
        Debug.Log("CardController.BuildDefenderDeckFromDeckData() called...");

        // Convert each cardDataSo into a card object
        foreach (CardDataSO cardData in deckData)
        {
            AddCardToDrawPile(defender, BuildCardFromCardData(cardData, defender));
        }

        // Shuffle the characters draw pile
        ShuffleCards(defender.deck);
    }
    public Card BuildCardFromCardData(CardDataSO data, Defender owner)
    {
        Debug.Log("CardController.BuildCardFromCardData() called...");

        Card card = new Card();

        card.cardName = data.cardName;
        card.cardDescription = data.cardDescription;
        card.cardBaseEnergyCost = data.cardEnergyCost;
        card.cardCurrentEnergyCost = data.cardEnergyCost;
        card.cardSprite = data.cardSprite;
        card.cardType = data.cardType;
        card.targettingType = data.targettingType;
        card.owner = owner;
        card.cardEffects.AddRange(data.cardEffects);

        return card;
    }    
    public CardViewModel BuildCardViewModelFromCard(Card card, Vector3 position)
    {
        Debug.Log("CardController.BuildCardViewModelFromCard() called...");

        CardViewModel cardVM = null;
        if(card.targettingType == TargettingType.NoTarget)
        {
            cardVM = Instantiate(GlobalSettings.Instance.CardViewModelPrefab, position, Quaternion.identity).GetComponent<CardViewModel>();
        }
        else
        {
            cardVM = Instantiate(GlobalSettings.Instance.TargettedCardViewModelPrefab, position, Quaternion.identity).GetComponent<CardViewModel>();
        }       

        // Cache references
        ConnectCardWithCardViewModel(card, cardVM);

        // Set texts and images
        cardVM.SetNameText(card.cardName);
        cardVM.SetDescriptionText(card.cardDescription);
        cardVM.SetEnergyText(card.cardCurrentEnergyCost.ToString());
        cardVM.SetGraphicImage(card.cardSprite);
        cardVM.SetCardTypeImage(card.cardType);

        return cardVM;
    }    
    public void ConnectCardWithCardViewModel(Card card, CardViewModel cardVM)
    {
        card.cardVM = cardVM;
        cardVM.card = card;
    }
    public void DisconnectCardAndCardViewModel(Card card, CardViewModel cardVM)
    {
        card.cardVM = null;
        cardVM.card = null;
    }

    #endregion

    // Card draw Logic
    #region
    public void DrawACardFromDrawPile(Defender defender, int index = 0)
    {
        Debug.Log("CardController.DrawACardFromDrawPile() called...");

        // Shuffle discard pile back into draw pile if draw pile is empty
        if (IsDrawPileEmpty(defender))
        {
            MoveAllCardsFromDiscardPileToDrawPile(defender);
        }
        if (IsCardDrawValid(defender))
        {
            // Get card and remove from deck
            Card cardDrawn = defender.deck[index];
            RemoveCardFromDrawPile(defender, cardDrawn);

            // Add card to hand
            defender.hand.Add(cardDrawn);

            // Create and queue card drawn visual event
            new DrawACardCommand(cardDrawn, defender, true, true).AddToQueue();
        }
        
    }   
    public void DrawCardsOnActivationStart(Defender defender)
    {
        Debug.Log("CardController.DrawCardsOnActivationStart() called...");

        for (int i = 0; i < 5; i++)
        {
            DrawACardFromDrawPile(defender);
        }
    }
    #endregion

    // Card Discard + Removal Logic
    #region
    public void DiscardHandOnActivationEnd(Defender defender)
    {
        Debug.Log("CardController.DiscardHandOnActivationEnd() called, hand size = " + defender.hand.Count.ToString());

        List<Card> cardsToDiscard = new List<Card>();
        cardsToDiscard.AddRange(defender.hand);

        foreach(Card card in cardsToDiscard)
        {
            DiscardCardFromHand(defender, card);
        }
    }
    public void DiscardCardFromHand(Defender defender, Card card)
    {
        Debug.Log("CardController.DiscardCardFromHand() called...");

        // Get handle to the card VM
        CardViewModel cvm = card.cardVM;

        // remove from hand
        defender.hand.Remove(card);

        // place on top of discard pile
        AddCardToDiscardPile(defender, card);

        // does the card have a cardVM linked to it?
        if (cvm)
        {
            // remove from hand visual
            defender.handVisual.RemoveCard(cvm.gameObject);

            // Play visual event of card moving to discard pile
            Sequence s = MoveCardVmFromHandToDiscardPile(cvm, defender.handVisual.DiscardPileTransform);

            // Once they anim is finished, destroy the CVM 
            s.OnComplete(() => DestroyCardViewModel(cvm));
        }                         

    }
    public Sequence MoveCardVmFromHandToDiscardPile(CardViewModel cvm, Transform discardPileLocation)
    {
        Debug.Log("CardController.MoveCardVmFromHandToDiscardPile() called...");

        // move card to the hand;
        Sequence s = DOTween.Sequence();
        // displace the card so that we can select it in the scene easier.
        s.Append(cvm.transform.DOMove(discardPileLocation.position, 0.5f));

        return s;
    }
    public void DestroyCardViewModel(CardViewModel cvm)
    {
        Debug.Log("CardController.DestroyCardViewModel() called...");

        // Clear references
        DisconnectCardAndCardViewModel(cvm.card, cvm);

        // Destoy script + GO
        Destroy(cvm.gameObject);
    }

    #endregion

    // Conditional Checks
    #region
    public bool IsCardDrawValid(Defender defender)
    {
        if(IsDrawPileEmpty(defender))
        {
            return false;
        }
        else if(defender.hand.Count >= 10)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool IsCardPlayable(Card card, Defender owner)
    {
        Debug.Log("CardController.IsCardPlayable() called, checking if '" +
            card.cardName + "' is playable by '" + owner.myName +"'");

        bool boolReturned = false;

        if(HasEnoughEnergyToPlayCard(card, owner) &&
           ActivationManager.Instance.IsEntityActivated(owner))
           // TO DO: here we check for specifics on card type 
           // (e.g. M attack cards not playable when disarmed)
        {
            boolReturned = true;
        }

        if (boolReturned == true)
        {
            Debug.Log("CardController.IsCardPlayable() detected that '" +
            card.cardName + "' is playable by '" + owner.myName + "', returning true...");
        }
        else
        {
            Debug.Log("CardController.IsCardPlayable() detected that '" +
            card.cardName + "' is NOT playable by '" + owner.myName + "', returning false...");
        }     

        return boolReturned;
    }
    public bool HasEnoughEnergyToPlayCard(Card card, Defender owner)
    {
        Debug.Log("CardController.HasEnoughEnergyToPlayCard(), checking '" +
            card.cardName +"' owned by '" + owner.myName +"'");
        return card.cardCurrentEnergyCost <= owner.currentEnergy;
    }
    public bool IsDrawPileEmpty(Defender defender)
    {
        return defender.deck.Count == 0;
    }
    #endregion

    // Hand Visual Logic
    #region
    public Action MoveCardVmFromDeckToHand(CardViewModel cardVM, Defender defender)
    {
        Action action = new Action(true);
        StartCoroutine(MoveCardVmFromDeckToHandCoroutine(cardVM, defender, action));
        return action;
    }
    private IEnumerator MoveCardVmFromDeckToHandCoroutine(CardViewModel cardVM, Defender defender, Action action)
    {
        bool tweenFinished = false;
        // Update slot positions
        AddCardVmToDefenderHandVisual(cardVM.gameObject, defender);

        // Bring card to front while it travels from draw spot to hand
        WhereIsTheCardOrCreature w = cardVM.GetComponent<WhereIsTheCardOrCreature>();
        w.BringToFront();
        w.Slot = 0;
        w.VisualState = VisualStates.Transition;

        // Declare new dotween sequence
        Sequence s = DOTween.Sequence();
        
        // Displace the card so that we can select it in the scene easier.
        s.Append(cardVM.transform.DOLocalMove(defender.handVisual.slots.Children[0].transform.localPosition, GlobalSettings.Instance.CardTransitionTimeFast));
        s.Insert(0f, cardVM.transform.DORotate(Vector3.zero, GlobalSettings.Instance.CardTransitionTimeFast));

        // Resolve on anim event finished events
        s.OnComplete(() => tweenFinished = true);
        s.OnComplete(() => defender.handVisual.ChangeLastCardStatusToInHand(cardVM.gameObject, w));

        // Yield until anim sequence is finished
        yield return new WaitUntil(() => tweenFinished == true);

        // Resolve event
        action.actionResolved = true;
    }
    public void AddCardVmToDefenderHandVisual(GameObject card, Defender defender)
    {
        // we always insert a new card as 0th element in CardsInHand List 
        defender.handVisual.CardsInHand.Insert(0, card);

        // parent this card to our Slots GameObject
        card.transform.SetParent(defender.handVisual.slots.transform);

        // re-calculate the position of the hand
        defender.handVisual.PlaceCardsOnNewSlots();
        defender.handVisual.UpdatePlacementOfSlots();
    }
    #endregion

    // Playing Cards Logic
    #region
    public void OnCardPlayedStart(Card card)
    {
        // Setup
        Defender owner = card.owner;

        // Pay Energy Cost
        owner.ModifyCurrentEnergy(-card.cardCurrentEnergyCost);

        // Remove from hand
        owner.hand.Remove(card);

        // TO DO: Add to discard pile, or exhaust pile?

        // Add to discard pile
        AddCardToDiscardPile(owner, card);
    }
    public void OnCardPlayedFinish(Card card)
    {
        // called at the very end of card play
    }
    public Action PlayCardFromHand(Card card, LivingEntity target = null)
    {
        Action action = new Action(true);
        StartCoroutine(PlayCardFromHandCoroutine(card, action, target));
        return action;
    }
    private IEnumerator PlayCardFromHandCoroutine(Card card, Action action, LivingEntity target = null)
    {
        Debug.Log("CardController.PlayCardFromHand() called, playing: " + card.cardName);

        // Setup
        Defender owner = card.owner;

        // Pay energy cost, remove from hand, etc
        OnCardPlayedStart(card);

        // Create visual event and enqueue
        new PlayASpellCardCommand(owner, card).AddToQueue();

        // Trigger all effects on card
        foreach (CardEffect effect in card.cardEffects)
        {
            Action effectEvent = TriggerEffectFromCard(card, effect, target);
            yield return new WaitUntil(() => effectEvent.ActionResolved());
        }

        // On end events
        OnCardPlayedFinish(card);

        // Resolve
        action.actionResolved = true;
       
    }
    public Action TriggerEffectFromCard(Card card, CardEffect cardEffect, LivingEntity target = null)
    {
        Action action = new Action(true);
        StartCoroutine(TriggerEffectFromCardCoroutine(card, cardEffect, action, target));
        return action;
    }
    private IEnumerator TriggerEffectFromCardCoroutine(Card card, CardEffect cardEffect, Action action, LivingEntity target = null)
    {
        Debug.Log("CardController.PlayCardFromHand() called, effect: '" + cardEffect.cardEffectType.ToString() + 
        "' from card: '" + card.cardName);
        Defender owner = card.owner;
        bool hasMovedOffStartingNode = false;

        if (cardEffect.cardEffectType == CardEffectType.GainBlock)
        {
            if(!target)
            {
                target = owner;
            }

            target.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(cardEffect.blockGainValue, owner));
        }

        else if(cardEffect.cardEffectType == CardEffectType.DealDamage)
        {
            // Attack animation stuff
            if(card.cardType == CardType.MeleeAttack && target != null)
            {
                hasMovedOffStartingNode = true;
                Action moveAction = MovementLogic.Instance.MoveAttackerToTargetNodeAttackPosition(owner, target);
                yield return new WaitUntil(() => moveAction.ActionResolved());

                owner.TriggerMeleeAttackAnimation();
            }

            // Calculate damage
            string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(owner, cardEffect, card);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(owner, target, damageType, false, cardEffect.baseDamageValue, card, cardEffect);

            // Start deal damage event
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, owner, target, damageType);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Move back to starting node pos, if we moved off 
            if(hasMovedOffStartingNode && owner.inDeathProcess == false)
            {
                Action moveBackEvent = MovementLogic.Instance.MoveEntityToNodeCentre(owner, owner.levelNode);
                yield return new WaitUntil(() => moveBackEvent.ActionResolved() == true);
            }

        }

        // Resolve event
        action.actionResolved = true;
    }
    #endregion

    // Deck + Discard Pile Functions
    #region
    public void ShuffleCards(List<Card> cards)
    {
        System.Random rng = new System.Random();

        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
    }
    public void MoveAllCardsFromDiscardPileToDrawPile(Defender defender)
    {
        Debug.Log("CardController.MoveAllCardsFromDiscardPileToDrawPile() called for character: " + defender.myName);

        // Create temp list for safe iteration
        List<Card> tempDiscardList = new List<Card>();
        tempDiscardList.AddRange(defender.discardPile);

        // Remove each card from discard pile, then add to draw pile
        foreach (Card card in tempDiscardList)
        {
            RemoveCardFromDiscardPile(defender,card);
            AddCardToDrawPile(defender,card);
        }

        // Re-shuffle the draw pile
        ShuffleCards(defender.deck);

    }
    public void AddCardToDrawPile(Defender defender, Card card)
    {
        defender.deck.Add(card);
    }
    public void RemoveCardFromDrawPile(Defender defender, Card card)
    {
        defender.deck.Remove(card);
    }
    public void AddCardToDiscardPile(Defender defender, Card card)
    {
        defender.discardPile.Add(card);
    }
    public void RemoveCardFromDiscardPile(Defender defender, Card card)
    {
        defender.discardPile.Remove(card);
    }
    #endregion

}
