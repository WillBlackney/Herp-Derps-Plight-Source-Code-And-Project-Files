using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

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
        ShuffleCards(defender.drawPile);
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
        card.talentSchool = data.talentSchool;
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
        cardVM.SetTalentSchoolImage(SpriteLibrary.Instance.GetTalentSchoolSpriteFromEnumData(card.talentSchool));
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
    public OldCoroutineData DrawACardFromDrawPile(Defender defender, int index = 0)
    {
        OldCoroutineData action = new OldCoroutineData(true);
        StartCoroutine(DrawACardFromDrawPileCoroutine(defender, action, index));
        return action;
    }
    private IEnumerator DrawACardFromDrawPileCoroutine(Defender defender, OldCoroutineData action, int index)
    {
        // Shuffle discard pile back into draw pile if draw pile is empty
        if (IsDrawPileEmpty(defender))
        {
            MoveAllCardsFromDiscardPileToDrawPile(defender);
        }
        if (IsCardDrawValid(defender))
        {
            // Get card and remove from deck
            Card cardDrawn = defender.drawPile[index];
            RemoveCardFromDrawPile(defender, cardDrawn);

            // Add card to hand
            defender.hand.Add(cardDrawn);

            // Create and queue card drawn visual event
            new DrawACardCommand(cardDrawn, defender, true, true).AddToQueue();
            yield return null;
        }

        // Resolve
        action.coroutineCompleted = true;
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

        if(HasEnoughEnergyToPlayCard(card, owner))// &&
           //ActivationManager.Instance.IsEntityActivated(owner))

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
        return defender.drawPile.Count == 0;
    }
    #endregion

    // Hand Visual Logic
    #region
    public OldCoroutineData MoveCardVmFromDeckToHand(CardViewModel cardVM, Defender defender)
    {
        OldCoroutineData action = new OldCoroutineData(true);
        StartCoroutine(MoveCardVmFromDeckToHandCoroutine(cardVM, defender, action));
        return action;
    }
    private IEnumerator MoveCardVmFromDeckToHandCoroutine(CardViewModel cardVM, Defender defender, OldCoroutineData action)
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
        action.coroutineCompleted = true;
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

        // check for specific on card play effects 
        // Infuriated 
        if(card.cardType == CardType.Skill)
        {
            foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
            {
                if (enemy.myPassiveManager.infuriated)
                {
                    StatusController.Instance.ApplyStatusToLivingEntity(enemy, StatusIconLibrary.Instance.GetStatusIconByName("Bonus Strength"), enemy.myPassiveManager.infuriatedStacks);
                }
            }
        }
       

        // TO DO: Add to discard pile, or exhaust pile?

        // Add to discard pile
        AddCardToDiscardPile(owner, card);
    }
    public void OnCardPlayedFinish(Card card)
    {
        // called at the very end of card play
    }
    public OldCoroutineData PlayCardFromHand(Card card, LivingEntity target = null)
    {
        OldCoroutineData action = new OldCoroutineData(true);
        StartCoroutine(PlayCardFromHandCoroutine(card, action, target));
        return action;
    }
    private IEnumerator PlayCardFromHandCoroutine(Card card, OldCoroutineData action, LivingEntity target = null)
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
            if(owner != null && owner.inDeathProcess == false)
            {
                OldCoroutineData effectEvent = TriggerEffectFromCard(card, effect, target);
                yield return new WaitUntil(() => effectEvent.ActionResolved());
            }            
        }

        // On end events
        OnCardPlayedFinish(card);

        // Resolve
        action.coroutineCompleted = true;
       
    }
    public OldCoroutineData TriggerEffectFromCard(Card card, CardEffect cardEffect, LivingEntity target = null)
    {
        OldCoroutineData action = new OldCoroutineData(true);
        StartCoroutine(TriggerEffectFromCardCoroutine(card, cardEffect, action, target));
        return action;
    }
    private IEnumerator TriggerEffectFromCardCoroutine(Card card, CardEffect cardEffect, OldCoroutineData action, LivingEntity target)
    {
        // Stop and return if target of effect is dying
        if(target != null && target.inDeathProcess)
        {
            Debug.Log("CardController.TriggerEffectFromCardCoroutine() cancelling: target is dying");
            action.coroutineCompleted = true;
            yield break;
        }

        Debug.Log("CardController.PlayCardFromHand() called, effect: '" + cardEffect.cardEffectType.ToString() + 
        "' from card: '" + card.cardName);
        Defender owner = card.owner;
        bool hasMovedOffStartingNode = false;

        // Gain Block
        if (cardEffect.cardEffectType == CardEffectType.GainBlock)
        {
            if(!target)
            {
                target = owner;
            }

            target.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(cardEffect.blockGainValue, owner));
        }

        // Deal Damage
        else if (cardEffect.cardEffectType == CardEffectType.DealDamage)
        {
            // Attack animation stuff
            if(card.cardType == CardType.MeleeAttack && target != null)
            {
                hasMovedOffStartingNode = true;

                // Move towards target visual event
                CoroutineData cData = new CoroutineData();
                VisualEventManager.Instance.CreateVisualEvent(() => MovementLogic.Instance.MoveAttackerToTargetNodeAttackPosition2(owner, target, cData), cData, QueuePosition.Back, 0, 0);
               
                // Animation visual event
                VisualEventManager.Instance.CreateVisualEvent(() => owner.TriggerMeleeAttackAnimation(), QueuePosition.Back, 0);
              
            }

            // Calculate damage
            string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(owner, cardEffect, card);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(owner, target, damageType, false, cardEffect.baseDamageValue, card, cardEffect);

            // Start deal damage event
            OldCoroutineData abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, owner, target, damageType);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Move back to starting node pos, if we moved off 
            if(hasMovedOffStartingNode && owner.inDeathProcess == false)
            {
                CoroutineData cData = new CoroutineData();
                VisualEventManager.Instance.CreateVisualEvent(() => MovementLogic.Instance.MoveEntityToNodeCentre2(owner, owner.levelNode, cData), cData, QueuePosition.Back, 0, 0);
            }

        }

        // Lose Health
        else if (cardEffect.cardEffectType == CardEffectType.LoseHealth)
        {
            // VFX
            VisualEffectManager.Instance.CreateBloodSplatterEffect(owner.transform.position);

            // Reduce Health
            OldCoroutineData selfDamageAction = CombatLogic.Instance.HandleDamage(cardEffect.healthLost, owner, owner, "None", null, true);
            yield return new WaitUntil(() => selfDamageAction.ActionResolved() == true);
            //yield return new WaitForSeconds(0.5f);
        }

        // Gain Energy
        else if (cardEffect.cardEffectType == CardEffectType.GainEnergy)
        {
            // Gain Energy
            owner.ModifyCurrentEnergy(cardEffect.energyGained);
            VisualEffectManager.Instance.CreateGainEnergyBuffEffect(owner.transform.position);
        }

        // Draw Cards
        else if (cardEffect.cardEffectType == CardEffectType.DrawCards)
        {
            // Determine target
            if (!target)
            {
                target = owner;
            }

            // Draw cards
            for(int draws = 0; draws < cardEffect.cardsDrawn; draws++)
            {
                OldCoroutineData drawAction = DrawACardFromDrawPile(target.defender);
                yield return new WaitUntil(() => drawAction.ActionResolved() == true);
            }           
        }

        // Apply Burning
        else if (cardEffect.cardEffectType == CardEffectType.ApplyBurning)
        {
            StatusController.Instance.ApplyStatusToLivingEntity(target, StatusIconLibrary.Instance.GetStatusIconByName("Burning"), cardEffect.burningApplied);
        }

        // Resolve event
        action.coroutineCompleted = true;
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
        ShuffleCards(defender.drawPile);

    }
    public void AddCardToDrawPile(Defender defender, Card card)
    {
        defender.drawPile.Add(card);
    }
    public void RemoveCardFromDrawPile(Defender defender, Card card)
    {
        defender.drawPile.Remove(card);
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
