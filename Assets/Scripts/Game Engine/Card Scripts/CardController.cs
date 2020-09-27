using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class CardController : Singleton<CardController>
{
    // Properties + Component References
    #region
    [Header("Card Properties")]
    [SerializeField] private float cardTransistionSpeed;
    [SerializeField] private BoxCollider2D tableCollider;
    [SerializeField] private bool mouseIsOverTable;

    [Header("Card Library Properties")]
    [SerializeField] private List<CardDataSO> allCards;
    public List<CardDataSO> AllCards
    {
        get { return allCards; }
        private set { allCards = value; }
    }
    #endregion

    // Card Library Logic
    #region
    public CardDataSO GetCardFromLibraryByName(string name)
    {
        CardDataSO cardReturned = null;

        foreach(CardDataSO card in AllCards)
        {
            if(card.cardName == name)
            {
                cardReturned = card;
                break;
            }
        }

        if(cardReturned == null)
        {
            Debug.Log("WARNING! CardController.GetCardFromLibraryByName() could not find a card " +
                "with a matching name of " + name + ", returning null...");
        }
        return cardReturned;
    }
    #endregion

    // Build Cards, Decks, View Models and Data
    #region
    public void BuildCharacterEntityDeckFromDeckData(CharacterEntityModel defender, List<CardDataSO> deckData)
    {
        Debug.Log("CardController.BuildDefenderDeckFromDeckData() called...");

        // Convert each cardDataSO into a card object
        foreach (CardDataSO cardData in deckData)
        {
            AddCardToDrawPile(defender, BuildCardFromCardData(cardData, defender));
        }

        // Shuffle the characters draw pile
        ShuffleCards(defender.drawPile);
    }
    private Card BuildCardFromCardData(CardDataSO data, CharacterEntityModel owner)
    {
        Debug.Log("CardController.BuildCardFromCardData() called...");

        Card card = new Card();

        // Core data
        card.owner = owner;
        card.cardName = data.cardName;
        card.cardDescription = data.cardDescription;
        card.cardBaseEnergyCost = data.cardEnergyCost;
        card.cardSprite = data.cardSprite;
        card.cardType = data.cardType;
        card.rarity = data.rarity;
        card.targettingType = data.targettingType;
        card.talentSchool = data.talentSchool;

        // key words
        card.expend = data.expend;
        card.fleeting = data.fleeting;
        card.opener = data.opener;
        card.unplayable = data.unplayable;

        // lists
        card.cardEventListeners.AddRange(data.cardEventListeners);
        card.cardEffects.AddRange(data.cardEffects);

        return card;
    }    
    public CardViewModel BuildCardViewModelFromCard(Card card, Vector3 position)
    {
        Debug.Log("CardController.BuildCardViewModelFromCard() called...");

        CardViewModel cardVM = null;
        if(card.targettingType == TargettingType.NoTarget)
        {
            cardVM = Instantiate(PrefabHolder.Instance.noTargetCard, position, Quaternion.identity).GetComponentInChildren<CardViewModel>();
        }
        else
        {
            cardVM = Instantiate(PrefabHolder.Instance.targetCard, position, Quaternion.identity).GetComponentInChildren<CardViewModel>();
        }

        // Set main parent 
        cardVM.mainParent = cardVM.transform.parent.transform;

        // Cache references
        ConnectCardWithCardViewModel(card, cardVM);

        // Set texts and images
        SetCardViewModelNameText(cardVM, card.cardName);
        SetCardViewModelDescriptionText(cardVM, card.cardDescription);
        SetCardViewModelEnergyText(card, cardVM, GetCardEnergyCost(card).ToString());
        SetCardViewModelGraphicImage(cardVM, card.cardSprite);
        SetCardViewModelTalentSchoolImage(cardVM, SpriteLibrary.Instance.GetTalentSchoolSpriteFromEnumData(card.talentSchool));
        ApplyCardViewModelTalentColoring(cardVM, ColorLibrary.Instance.GetTalentColor(card.talentSchool));
        ApplyCardViewModelRarityColoring(cardVM, ColorLibrary.Instance.GetRarityColor(card.rarity));
        SetCardViewModelCardTypeImage(cardVM, SpriteLibrary.Instance.GetCardTypeImageFromTypeEnumData(card.cardType));
        return cardVM;
    }
    private void ConnectCardWithCardViewModel(Card card, CardViewModel cardVM)
    {
        card.cardVM = cardVM;
        cardVM.card = card;
    }
    private void DisconnectCardAndCardViewModel(Card card, CardViewModel cardVM)
    {
        if(card != null)
        {
            card.cardVM = null;
        }
        if(cardVM != null)
        {
            cardVM.card = null;
        }       
        
    }
    #endregion

    // Card View Model Specific Logic
    #region
    public void SetCardViewModelNameText(CardViewModel cvm, string name)
    {
        cvm.nameText.text = name;
        if (cvm.myPreviewCard != null)
        {
            Debug.Log("SETTING CARD VIEW MODEL PREVIEW NAME!!");
            SetCardViewModelNameText(cvm.myPreviewCard, name);
        }
    }
    public void SetCardViewModelDescriptionText(CardViewModel cvm, string description)
    {
        cvm.descriptionText.text = description;
        if (cvm.myPreviewCard != null)
        {
            SetCardViewModelDescriptionText(cvm.myPreviewCard, description);
        }
    }
    public void SetCardViewModelEnergyText(Card card, CardViewModel cvm, string energyCost)
    {
        cvm.energyText.text = energyCost;
        cvm.energyText.color = Color.white;

        // color text if cost is more or less then base.
        if(card != null)
        {
            int currentCost = GetCardEnergyCost(card);

            if(currentCost > card.cardBaseEnergyCost)
            {
                cvm.energyText.color = Color.red;
            }
            else if (currentCost < card.cardBaseEnergyCost)
            {
                cvm.energyText.color = Color.green;
            }
        }

        if (cvm.myPreviewCard != null)
        {
            SetCardViewModelEnergyText(card, cvm.myPreviewCard, energyCost);
        }
    }
    public void SetCardViewModelGraphicImage(CardViewModel cvm, Sprite sprite)
    {
        cvm.graphicImage.sprite = sprite;
        if (cvm.myPreviewCard != null)
        {
            SetCardViewModelGraphicImage(cvm.myPreviewCard, sprite);
        }
    }
    public void SetCardViewModelTalentSchoolImage(CardViewModel cvm, Sprite sprite)
    {
        if (sprite)
        {
            cvm.talentSchoolParent.SetActive(true);
            cvm.talentSchoolImage.sprite = sprite;
            if (cvm.myPreviewCard != null)
            {
                SetCardViewModelTalentSchoolImage(cvm.myPreviewCard, sprite);
            }
        }
    }
    public void ApplyCardViewModelTalentColoring(CardViewModel cvm, Color color)
    {
        foreach (Image sr in cvm.talentRenderers)
        {
            sr.color = color;
        }
        if (cvm.myPreviewCard != null)
        {
            ApplyCardViewModelTalentColoring(cvm.myPreviewCard, color);
        }
    }
    public void ApplyCardViewModelRarityColoring(CardViewModel cvm, Color color)
    {
        foreach (Image sr in cvm.rarityRenderers)
        {
            sr.color = color;
        }
        if (cvm.myPreviewCard != null)
        {
            ApplyCardViewModelRarityColoring(cvm.myPreviewCard, color);
        }
    }
    public void SetCardViewModelCardTypeImage(CardViewModel cvm, Sprite sprite)
    {
        cvm.cardTypeImage.sprite = sprite;

        // do for card preview also
        if (cvm.myPreviewCard != null)
        {
            SetCardViewModelCardTypeImage(cvm.myPreviewCard, sprite);
        }
    }

    public void SetUpCardViewModelPreviewCanvas(CardViewModel cvm)
    {
        cvm.canvas.overrideSorting = true;
        cvm.canvas.sortingOrder = 1000;
    }
    #endregion

    // Card draw Logic
    #region
    public Card DrawACardFromDrawPile(CharacterEntityModel defender, int drawPileIndex = 0)
    {
        Debug.Log("CardController.DrawACardFromDrawPile() called...");
        Card cardDrawn = null;

        // Shuffle discard pile back into draw pile if draw pile is empty
        if (IsDrawPileEmpty(defender))
        {
            MoveAllCardsFromDiscardPileToDrawPile(defender);
        }
        if (IsCardDrawValid(defender))
        {
            // Get card and remove from deck
            cardDrawn = defender.drawPile[drawPileIndex];
            RemoveCardFromDrawPile(defender, cardDrawn);

            // Add card to hand
            AddCardToHand(defender, cardDrawn);

            // Create and queue card drawn visual event
            VisualEventManager.Instance.CreateVisualEvent(() => DrawCardFromDeckVisualEvent(cardDrawn, defender), QueuePosition.Back, 0, 0.2f, EventDetail.CardDraw);
        }

        return cardDrawn;
    }
    public void DrawCardsOnActivationStart(CharacterEntityModel defender)
    {
        Debug.Log("CardController.DrawCardsOnActivationStart() called...");

        for (int i = 0; i < EntityLogic.GetTotalDraw(defender); i++)
        {
            DrawACardFromDrawPile(defender);
        }
    }
    #endregion

    // Gain card not from deck logic
    #region
    public void CreateAndAddNewCardToCharacterHand(CharacterEntityModel defender, CardDataSO data)
    {
        if (!IsHandFull(defender))
        {
            // Get card and remove from deck
            Card newCard = BuildCardFromCardData(data, defender);

            // Add card to hand
            AddCardToHand(defender, newCard);

            // Create and queue card drawn visual event
            VisualEventManager.Instance.CreateVisualEvent(() => CreateAndAddNewCardToCharacterHandVisualEvent(newCard, defender), QueuePosition.Back, 0, 0.2f, EventDetail.CardDraw);
        }
    }
    #endregion

    // Card Discard + Removal Logic
    #region
    public void DiscardHandOnActivationEnd(CharacterEntityModel defender)
    {
        Debug.Log("CardController.DiscardHandOnActivationEnd() called, hand size = " + defender.hand.Count.ToString());

        List<Card> cardsToDiscard = new List<Card>();
        cardsToDiscard.AddRange(defender.hand);

        foreach(Card card in cardsToDiscard)
        {
            DiscardCardFromHand(card);
        }
    }
    private void DiscardCardFromHand(Card card)
    {
        Debug.Log("CardController.DiscardCardFromHand() called...");

        // Get handle to the card VM
        CardViewModel cvm = card.cardVM;
        CharacterEntityModel owner = card.owner;

        // remove from hand
        RemoveCardFromHand(owner, card);

        // place on top of discard pile
        AddCardToDiscardPile(owner, card);

        // does the card have a cardVM linked to it?
        if (cvm)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => DiscardCardFromHandVisualEvent(cvm, owner), QueuePosition.Back, 0f, 0.1f);
        }                         

    }
    private void ExpendCard(Card card)
    {
        Debug.Log("CardController.ExpendCard() called...");

        // Get handle to the card VM
        CardViewModel cvm = card.cardVM;
        CharacterEntityModel owner = card.owner;

        // Remove card from which ever collection its in
        if (owner.hand.Contains(card))
        {
            RemoveCardFromHand(owner, card);
        }
        else if (owner.discardPile.Contains(card))
        {
            RemoveCardFromDiscardPile(owner, card);
        }
        else if (owner.drawPile.Contains(card))
        {
            RemoveCardFromDrawPile(owner, card);
        }

        // place in the expend pile
        AddCardToExpendPile(owner, card);

        // does the card have a cardVM linked to it?
        if (cvm)
        {
            // VisualEventManager.Instance.CreateVisualEvent(() => ExpendCardVisualEvent(cvm, owner), QueuePosition.Front);
            ExpendCardVisualEvent(cvm, owner);
        }

        OnCardExpended(card);
    }
    private void DestroyCardViewModel(CardViewModel cvm)
    {
        Debug.Log("CardController.DestroyCardViewModel() called...");

        // Destoy script + GO
        Destroy(cvm.mainParent.gameObject);
    }
    #endregion

    // Conditional Checks
    #region
    private bool IsCardDrawValid(CharacterEntityModel defender)
    {
        if(IsDrawPileEmpty(defender))
        {
            return false;
        }
        else if(IsHandFull(defender))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool IsCardPlayable(Card card, CharacterEntityModel owner)
    {
        Debug.Log("CardController.IsCardPlayable() called, checking if '" +
            card.cardName + "' is playable by '" + owner.myName +"'");

        bool boolReturned = false;

        if(HasEnoughEnergyToPlayCard(card, owner) &&
            CombatLogic.Instance.CurrentCombatState == CombatGameState.CombatActive &&
            card.unplayable == false)

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
    private bool HasEnoughEnergyToPlayCard(Card card, CharacterEntityModel owner)
    {
        Debug.Log("CardController.HasEnoughEnergyToPlayCard(), checking '" +
            card.cardName +"' owned by '" + owner.myName +"'");
        return GetCardEnergyCost(card) <= owner.energy;
    }
    private bool IsDrawPileEmpty(CharacterEntityModel character)
    {
        return character.drawPile.Count == 0;
    }
    private bool IsHandFull(CharacterEntityModel character)
    {
        return character.hand.Count >= 10;
    }
    private bool DoesCardEffectMeetWeaponRequirement(CardEffect ce, CharacterEntityModel owner)
    {
        bool boolReturned = false;

        if(ce.weaponRequirement == CardWeaponRequirement.None)
        {
            boolReturned = true;
        }
        else if(ce.weaponRequirement == CardWeaponRequirement.DW &&
            ItemController.Instance.IsDualWielding(owner.iManager))
        {
            boolReturned = true;
        }
        else if (ce.weaponRequirement == CardWeaponRequirement.Shielded &&
            ItemController.Instance.IsShielded(owner.iManager))
        {
            boolReturned = true;
        }
        else if (ce.weaponRequirement == CardWeaponRequirement.TwoHanded &&
            ItemController.Instance.IsTwoHanding(owner.iManager))
        {
            boolReturned = true;
        }
        else if (ce.weaponRequirement == CardWeaponRequirement.Ranged &&
           ItemController.Instance.IsRanged(owner.iManager))
        {
            boolReturned = true;
        }

        return boolReturned;
    }
   
    #endregion

    // Playing Cards Logic
    #region
    private void OnCardPlayedStart(Card card)
    {
        // Setup
        CharacterEntityModel owner = card.owner;

        // Pay Energy Cost
        CharacterEntityController.Instance.ModifyEnergy(owner, -GetCardEnergyCost(card));

        // check for specific on card play effects 
        // Remove Melee Attack reduction passive
        if (card.cardType == CardType.MeleeAttack &&
            owner.pManager.meleeAttackReductionStacks > 0)
        {
            PassiveController.Instance.ModifyMeleeAttackReduction(owner.pManager, -owner.pManager.meleeAttackReductionStacks, false);
        }

        // Infuriated 
        if (card.cardType == CardType.Skill)
        {
            /*
            foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
            {
                if (enemy.myPassiveManager.infuriated)
                {
                    StatusController.Instance.ApplyStatusToLivingEntity(enemy, StatusIconLibrary.Instance.GetStatusIconByName("Bonus Strength"), enemy.myPassiveManager.infuriatedStacks);
                }
            }
            */
        }

        // Where should this card be sent to?
        if (card.expend)
        {
            ExpendCard(card);
        }

        else if(card.cardType == CardType.Power)
        {
            CardViewModel cardVM = card.cardVM;
            if (owner.hand.Contains(card))
            {
                RemoveCardFromHand(owner, card);
            }

            if (cardVM)
            {
                // to do: create 'play power' anim
                //VisualEventManager.Instance.CreateVisualEvent(() => PlayACardFromHandVisualEvent(cardVM, owner.characterEntityView), QueuePosition.Front);
                PlayACardFromHandVisualEvent(cardVM, owner.characterEntityView);
            }
        }

        else
        {
            // Do normal 'play from hand' stuff
            CardViewModel cardVM = card.cardVM;
            if (owner.hand.Contains(card))
            {
                RemoveCardFromHand(owner, card);
                AddCardToDiscardPile(owner, card);
            }

            if (cardVM)
            {
                //VisualEventManager.Instance.CreateVisualEvent(() => PlayACardFromHandVisualEvent(cardVM, owner.characterEntityView), QueuePosition.Front);
                PlayACardFromHandVisualEvent(cardVM, owner.characterEntityView);
            }
           
        }
        // to do: what happens to power cards???

        // Add to discard pile
        //AddCardToDiscardPile(owner, card);
    }
    private void OnCardPlayedFinish(Card card)
    {
        // called at the very end of card play
    }
    private void OnCardExpended(Card card)
    {
        // TO DO: in the future, additonal effects that occur
        // when an expend happens will go here e.g. an item
        // that reads 'whenever you expend a card, gain 5 block',
        // the gain block logic will go here
    }
    public void PlayCardFromHand(Card card, CharacterEntityModel target = null)
    {
        Debug.Log("CardController.PlayCardFromHand() called, playing: " + card.cardName);

        // Setup
        CharacterEntityModel owner = card.owner;
        CardViewModel cardVM = card.cardVM;

        // Pay energy cost, remove from hand, etc
        OnCardPlayedStart(card);

        // Remove references between card and its view
        DisconnectCardAndCardViewModel(card, cardVM);

        // Trigger all effects on card
        foreach (CardEffect effect in card.cardEffects)
        {
            if(DoesCardEffectMeetWeaponRequirement(effect, owner))
            {
                TriggerEffectFromCard(card, effect, target);
            }

            // Move back to home node early if declarded to do so
            /*
            if (owner.hasMovedOffStartingNode && owner.livingState == LivingState.Alive &&
                effect.animationEventData.returnToMyNodeOnCardEffectResolved)
            {
                owner.hasMovedOffStartingNode = false;
                CoroutineData cData = new CoroutineData();
                LevelNode node = owner.levelNode;
                VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.MoveEntityToNodeCentre(owner, node, cData), cData, QueuePosition.Back, 0.3f, 0);
            }
            */
        }

        // If character moved off node, move back after all card effects resolved
        if (owner.hasMovedOffStartingNode && owner.livingState == LivingState.Alive)
        {
            owner.hasMovedOffStartingNode = false;
            CoroutineData cData = new CoroutineData();
            LevelNode node = owner.levelNode;
            VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.MoveEntityToNodeCentre(owner, node, cData), cData, QueuePosition.Back, 0.3f, 0);
        }

        // Brief pause at the of all effects
        VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);        

        // On end events
        OnCardPlayedFinish(card);
       
    }
    private void TriggerEffectFromCard(Card card, CardEffect cardEffect, CharacterEntityModel target)
    {
        // Stop and return if target of effect is dying        
        if(target != null && target.livingState == LivingState.Dead)
        {
            Debug.Log("CardController.TriggerEffectFromCardCoroutine() cancelling: target is dying");
            return;
        }        

        Debug.Log("CardController.PlayCardFromHand() called, effect: '" + cardEffect.cardEffectType.ToString() + 
        "' from card: '" + card.cardName);

        CharacterEntityModel owner = card.owner;

        // Queue starting anims and particles
        foreach (AnimationEventData vEvent in cardEffect.visualEventsOnStart)
        {
            AnimationEventController.Instance.PlayAnimationEvent(vEvent, owner, target);
        }

        // RESOLVE EFFECT LOGIC START!
        // Gain Block Self
        if (cardEffect.cardEffectType == CardEffectType.GainBlockSelf)
        {
            CharacterEntityController.Instance.ModifyBlock(owner, CombatLogic.Instance.CalculateBlockGainedByEffect(cardEffect.blockGainValue, owner, owner, null, cardEffect));
        }

        // Gain Block Target
        else if (cardEffect.cardEffectType == CardEffectType.GainBlockTarget)
        {
            CharacterEntityController.Instance.ModifyBlock(target, CombatLogic.Instance.CalculateBlockGainedByEffect(cardEffect.blockGainValue, owner, target, null, cardEffect));
        }

        // Gain Block All Allies
        else if (cardEffect.cardEffectType == CardEffectType.GainBlockAllAllies)
        {
            foreach (CharacterEntityModel ally in CharacterEntityController.Instance.GetAllAlliesOfCharacter(owner))
            {
                CharacterEntityController.Instance.ModifyBlock(ally, CombatLogic.Instance.CalculateBlockGainedByEffect(cardEffect.blockGainValue, owner, ally, null, cardEffect));
            }            
        }

        // Deal Damage Target
        else if (cardEffect.cardEffectType == CardEffectType.DamageTarget)
        {
            // Calculate damage
            DamageType damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(owner, cardEffect, card);
            int baseDamage;

            // Do normal base damage, or draw base damage from another source?
            if (cardEffect.drawBaseDamageFromCurrentBlock) 
            {
                baseDamage = owner.block;
            }
            else if (cardEffect.drawBaseDamageFromTargetPoisoned)
            {
                baseDamage = target.pManager.poisonedStacks;
            }
            else
            {
                baseDamage = cardEffect.baseDamageValue;
            }        
                            
            // Calculate the end damage value
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(owner, target, damageType, false, baseDamage, card, cardEffect);

            // Start damage sequence
            CombatLogic.Instance.HandleDamage(finalDamageValue, owner, target, damageType, card);
        }

        // Deal Damage All Enemies
        else if (cardEffect.cardEffectType == CardEffectType.DamageAllEnemies)
        {

            VisualEvent batchedEvent = VisualEventManager.Instance.InsertTimeDelayInQueue(0f);

            foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(owner))
            {
                // Calculate damage
                DamageType damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(owner, cardEffect, card);
                int baseDamage;

                // Do normal base damage, or draw base damage from another source?
                if (cardEffect.drawBaseDamageFromCurrentBlock)
                {
                    baseDamage = owner.block;
                }
                else if (cardEffect.drawBaseDamageFromTargetPoisoned)
                {
                    baseDamage = target.pManager.poisonedStacks;
                }
                else
                {
                    baseDamage = cardEffect.baseDamageValue;
                }

                // Calculate the end damage value
                int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(owner, enemy, damageType, false, baseDamage, card, cardEffect);

                // Start damage sequence
                CombatLogic.Instance.HandleDamage(finalDamageValue, owner, enemy, damageType, card, null, false, QueuePosition.BatchedEvent, batchedEvent);
            }            
        }

        // Deal Damage Self
        else if (cardEffect.cardEffectType == CardEffectType.DamageSelf)
        {
            // Calculate damage
            DamageType damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(owner, cardEffect, card);
            int baseDamage;

            // Do normal base damage, or draw base damage from another source?
            if (cardEffect.drawBaseDamageFromCurrentBlock)
            {
                baseDamage = owner.block;
            }
            else if (cardEffect.drawBaseDamageFromTargetPoisoned)
            {
                baseDamage = target.pManager.poisonedStacks;
            }
            else
            {
                baseDamage = cardEffect.baseDamageValue;
            }

            // Calculate the end damage value
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(owner, target, damageType, false, baseDamage, card, cardEffect);

            // Start damage sequence
            CombatLogic.Instance.HandleDamage(finalDamageValue, owner, target, damageType, card);
        }

        // Lose Health
        else if (cardEffect.cardEffectType == CardEffectType.LoseHP)
        {    

            // Start self damage sequence
            CombatLogic.Instance.HandleDamage(cardEffect.healthLost, owner, owner, DamageType.None, card, null, true);
        }

        // Gain Energy
        else if (cardEffect.cardEffectType == CardEffectType.GainEnergy)
        {
            // Gain Energy
            CharacterEntityController.Instance.ModifyEnergy(owner, cardEffect.energyGained, true);
        }

        // Draw Cards
        else if (cardEffect.cardEffectType == CardEffectType.DrawCards)
        {
            // Draw cards
            for(int draws = 0; draws < cardEffect.cardsDrawn; draws++)
            {
                Card cardDrawn = DrawACardFromDrawPile(owner);
                if(cardEffect.extraDrawEffect == ExtraDrawEffect.ReduceEnergyCostThisCombat)
                {
                    ReduceCardEnergyCostThisCombat(cardDrawn, cardEffect.cardEnergyReduction);
                }
                else if (cardEffect.extraDrawEffect == ExtraDrawEffect.SetEnergyCostToZeroThisCombat)
                {
                    ReduceCardEnergyCostThisCombat(cardDrawn, cardDrawn.cardBaseEnergyCost);
                }
            }           
        }

        // Apply passive to self
        else if (cardEffect.cardEffectType == CardEffectType.ApplyPassiveToSelf)
        {
            int stacks = cardEffect.passivePairing.passiveStacks;
            if (cardEffect.drawStacksFromOverload)
            {
                stacks = owner.pManager.overloadStacks;
            }
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(owner.pManager, cardEffect.passivePairing.passiveData.passiveName, stacks, true, 0.5f);
        }

        // Apply passive to target
        else if (cardEffect.cardEffectType == CardEffectType.ApplyPassiveToTarget)
        {
            int stacks = cardEffect.passivePairing.passiveStacks;
            if (cardEffect.drawStacksFromOverload)
            {
                stacks = owner.pManager.overloadStacks;
            }
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(target.pManager, cardEffect.passivePairing.passiveData.passiveName, stacks, true, 0.5f);
        }

        // Apply passive to all enemies
        else if (cardEffect.cardEffectType == CardEffectType.ApplyPassiveToAllEnemies)
        {
            int stacks = cardEffect.passivePairing.passiveStacks;
            if (cardEffect.drawStacksFromOverload)
            {
                stacks = owner.pManager.overloadStacks;
            }

            foreach(CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(owner))
            {
                PassiveController.Instance.ModifyPassiveOnCharacterEntity(enemy.pManager, cardEffect.passivePairing.passiveData.passiveName, stacks, true);
                VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
            }           
        }

        // Apply passive to all allies
        else if (cardEffect.cardEffectType == CardEffectType.ApplyPassiveToAllEnemies)
        {
            int stacks = cardEffect.passivePairing.passiveStacks;
            if (cardEffect.drawStacksFromOverload)
            {
                stacks = owner.pManager.overloadStacks;
            }

            foreach (CharacterEntityModel ally in CharacterEntityController.Instance.GetAllAlliesOfCharacter(owner))
            {
                PassiveController.Instance.ModifyPassiveOnCharacterEntity(ally.pManager, cardEffect.passivePairing.passiveData.passiveName, stacks, true);
                VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
            }
        }

        // Remove overload from self
        else if (cardEffect.cardEffectType == CardEffectType.RemoveAllOverloadFromSelf)
        {
            PassiveController.Instance.ModifyOverload(owner.pManager, -owner.pManager.overloadStacks, true);
        }

        // Remove poisoned from self
        else if (cardEffect.cardEffectType == CardEffectType.RemoveAllPoisonedFromSelf)
        {
            PassiveController.Instance.ModifyPoisoned(null, owner.pManager, -owner.pManager.poisonedStacks, true);
        }

        // Remove poisoned from target
        else if (cardEffect.cardEffectType == CardEffectType.RemoveAllPoisonedFromTarget)
        {
            PassiveController.Instance.ModifyPoisoned(null, target.pManager, -target.pManager.poisonedStacks, true);
        }

        // Taunt Target
        else if (cardEffect.cardEffectType == CardEffectType.TauntTarget)
        {
            CharacterEntityController.Instance.HandleTaunt(owner, target);
        }

        // Taunt all enemies
        else if (cardEffect.cardEffectType == CardEffectType.TauntAllEnemies)
        {
            // get all enemies
            foreach(CharacterEntityModel character in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(owner))
            {
                // taunt each enemy
                CharacterEntityController.Instance.HandleTaunt(owner, character);
            }            
        }

        // Add new non deck card to hand
        else if(cardEffect.cardEffectType == CardEffectType.AddCardsToHand)
        {
            for(int i = 0; i < cardEffect.copiesAdded; i++)
            {
                CreateAndAddNewCardToCharacterHand(owner, cardEffect.cardAdded);
            }            
        }

        // CONCLUDING VISUAL EVENTS!
        if(CombatLogic.Instance.CurrentCombatState == CombatGameState.CombatActive &&
            owner.livingState == LivingState.Alive)
        {
            // cancel if the target was killed
            if(target != null && target.livingState == LivingState.Dead)
            {
                return;
            }

            foreach (AnimationEventData vEvent in cardEffect.visualEventsOnFinish)
            {
                AnimationEventController.Instance.PlayAnimationEvent(vEvent, owner, target);
            }
        }
              
    }
    #endregion

    // Hand, Draw Pile + Discard Pile Functions
    #region
    private List<Card> GetAllCharacterCardsInHandDrawAndDiscard(CharacterEntityModel model)
    {
        Debug.Log("CardController.GetAllCharacterCardsInHandDrawAndDiscard() called for character: " + model.myName);

        List<Card> listReturned = new List<Card>();
        listReturned.AddRange(model.hand);
        listReturned.AddRange(model.drawPile);
        listReturned.AddRange(model.discardPile);

        return listReturned;
    }
    private void ShuffleCards(List<Card> cards)
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
    private void MoveAllCardsFromDiscardPileToDrawPile(CharacterEntityModel defender)
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
    private void AddCardToDrawPile(CharacterEntityModel defender, Card card)
    {
        defender.drawPile.Add(card);
        string drawPileCount = defender.drawPile.Count.ToString();
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateDrawPileCountText(defender.characterEntityView, drawPileCount));
    }
    private void RemoveCardFromDrawPile(CharacterEntityModel defender, Card card)
    {
        defender.drawPile.Remove(card);
        string drawPileCount = defender.drawPile.Count.ToString();
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateDrawPileCountText(defender.characterEntityView, drawPileCount));
    }
    private void AddCardToDiscardPile(CharacterEntityModel defender, Card card)
    {
        defender.discardPile.Add(card);
        string discardPileCount = defender.discardPile.Count.ToString();
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateDiscardPileCountText(defender.characterEntityView, discardPileCount));
    }
    private void RemoveCardFromDiscardPile(CharacterEntityModel defender, Card card)
    {
        defender.discardPile.Remove(card);
        string discardPileCount = defender.discardPile.Count.ToString();
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateDiscardPileCountText(defender.characterEntityView, discardPileCount));
    }
    private void AddCardToHand(CharacterEntityModel defender, Card card)
    {
        defender.hand.Add(card);
    }
    private void RemoveCardFromHand(CharacterEntityModel defender, Card card)
    {
        defender.hand.Remove(card);
    }
    private void AddCardToExpendPile(CharacterEntityModel defender, Card card)
    {
        defender.expendPile.Add(card);
    }
    #endregion

    // Card Event Listener Logic
    #region
    private void RunCardEventListenerFunction(Card card, CardEventListener e)
    {
        Debug.Log("CardController.RunCardEventListenerFunction() called...");

        // TO DO: Create a small visual event dotween sequence
        // on card VM's when they trigger on listener event,
        // something like scales up and then back down quickly

        // Reduce energy cost of card
        if (e.cardEventListenerFunction == CardEventListenerFunction.ReduceCardEnergyCost)
        {
            // Reduce cost this combat
            ReduceCardEnergyCostThisCombat(card, e.energyReductionAmount);
        }

        // Apply passive
        else if (e.cardEventListenerFunction == CardEventListenerFunction.ApplyPassiveToSelf)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => PlayCardBreathAnimationVisualEvent(card.cardVM));
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(card.owner.pManager, e.passivePairing.passiveData.passiveName, e.passivePairing.passiveStacks, true, 0.5f);
        }
    }
    public void HandleOnCharacterDamagedCardListeners(CharacterEntityModel character)
    {
        Debug.Log("CardController.HandleOnCharacterDamagedCardListeners() called...");

        foreach(Card card in GetAllCharacterCardsInHandDrawAndDiscard(character))
        {
            foreach(CardEventListener e in card.cardEventListeners)
            {
                if(e.cardEventListenerType == CardEventListenerType.OnLoseHealth)
                {
                    RunCardEventListenerFunction(card, e);
                }
            }
        }
    }
    public void HandleOnCharacterActivationEndCardListeners(CharacterEntityModel character)
    {
        foreach (Card card in character.hand)
        {
            foreach (CardEventListener e in card.cardEventListeners)
            {
                if (e.cardEventListenerType == CardEventListenerType.OnActivationEnd)
                {
                    RunCardEventListenerFunction(card, e);
                }
            }
        }
    }
    public void OnMeleeAttackReductionModified(CharacterEntityModel model)
    {
        foreach (Card card in model.hand)
        {
            if (card.cardType == CardType.MeleeAttack)
            {
                // Update card vm energy text, if not null
                CardViewModel cvm = card.cardVM;
                int newCostTextValue = GetCardEnergyCost(card);
                if (cvm)
                {
                    // Update energy cost text
                    VisualEventManager.Instance.CreateVisualEvent(() => SetCardViewModelEnergyText(card, cvm, newCostTextValue.ToString()));

                    // only play breath if cost of card is reduced, not increased
                    if (model.pManager.meleeAttackReductionStacks > 0)
                    {
                        VisualEventManager.Instance.CreateVisualEvent(() => PlayCardBreathAnimationVisualEvent(cvm));
                    }
                }                
            }
        }
    }
    #endregion

    // Misc + Calculators + Events
    #region
    public int GetCardEnergyCost(Card card)
    {
        Debug.Log("CardController.GetCardEnergyCost() called for card: " + card.cardName);

        int costReturned = card.cardBaseEnergyCost;

        costReturned -= card.energyReductionPermanent;
        costReturned -= card.energyReductionThisCombatOnly;
        costReturned -= card.energyReductionUntilPlayed;

        
        if(card.owner.pManager != null && 
            card.cardType == CardType.MeleeAttack &&
            card.owner.pManager.meleeAttackReductionStacks > 0)
        {
            costReturned -= card.owner.pManager.meleeAttackReductionStacks;
        }

        // Prevent cost going negative
        if(costReturned < 0)
        {
            costReturned = 0;
        }

        return costReturned;
    }
    private void ReduceCardEnergyCostThisCombat(Card card, int reductionAmount)
    {
        // Setup
        CardViewModel cvm = card.cardVM;

        // Reduce cost this combat
        card.energyReductionThisCombatOnly += reductionAmount;

        // Update card vm energy text, if not null
        int newCostTextValue = GetCardEnergyCost(card);
        if (cvm)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => PlayCardBreathAnimationVisualEvent(cvm));
            VisualEventManager.Instance.CreateVisualEvent(() => SetCardViewModelEnergyText(card, cvm, newCostTextValue.ToString()));
        }
       
    }
    #endregion

    // Table Logic
    #region
    public bool IsCursorOverTable()
    {
        return mouseIsOverTable;
    }
    private void OnMouseOver()
    {
        mouseIsOverTable = true;
    }
    private void OnMouseExit()
    {
        mouseIsOverTable = false;
    }
    #endregion


    // Visual Events
    #region
    private void CreateAndAddNewCardToCharacterHandVisualEvent(Card card, CharacterEntityModel character)
    {
        Debug.Log("CardController.CreateAndAddNewCardToCharacterHandVisualEvent() called...");
        CharacterEntityView characterView = character.characterEntityView;

        CardViewModel cardVM;
        cardVM = BuildCardViewModelFromCard(card, characterView.handVisual.NonDeckCardCreationTransform.position);

        // pass this card to HandVisual class
        characterView.handVisual.AddCard(cardVM.mainParent.gameObject);

        // Bring card to front while it travels from draw spot to hand
        CardLocationTracker clt = cardVM.locationTracker;
        clt.BringToFront();
        clt.Slot = 0;
        clt.VisualState = VisualStates.Transition;

        // Start SFX
        AudioManager.Instance.PlaySound(Sound.Card_Draw);

        // Shrink card, then scale up as it moves to hand

        // Get starting scale
        Vector3 originalScale = new Vector3
            (cardVM.mainParent.transform.localScale.x, cardVM.mainParent.transform.localScale.y, cardVM.mainParent.transform.localScale.z);

        // Shrink card
        cardVM.mainParent.transform.localScale = new Vector3(0.1f, 0.1f, cardVM.mainParent.transform.localScale.z);

        // Scale up
        cardVM.mainParent.DOScale(originalScale, cardTransistionSpeed).SetEase(Ease.OutQuint);

        // move card to the hand;
        Sequence s = DOTween.Sequence();

        // displace the card so that we can select it in the scene easier.
        s.Append(cardVM.mainParent.DOLocalMove(characterView.handVisual.slots.Children[0].transform.localPosition, cardTransistionSpeed));

        s.OnComplete(() => clt.SetHandSortingOrder());
    }
    private void DrawCardFromDeckVisualEvent(Card card, CharacterEntityModel character)
    {
        Debug.Log("CardController.DrawCardFromDeckVisualEvent() called...");
        CharacterEntityView characterView = character.characterEntityView;

        CardViewModel cardVM;        
        cardVM = BuildCardViewModelFromCard(card, characterView.handVisual.DeckTransform.position);

        // pass this card to HandVisual class
        characterView.handVisual.AddCard(cardVM.mainParent.gameObject);

        // Bring card to front while it travels from draw spot to hand
        CardLocationTracker clt = cardVM.locationTracker;
        clt.BringToFront();
        clt.Slot = 0;
        clt.VisualState = VisualStates.Transition;

        // Start SFX
        AudioManager.Instance.PlaySound(Sound.Card_Draw);

        // Shrink card, then scale up as it moves to hand

        // Get starting scale
        Vector3 originalScale = new Vector3
            (cardVM.mainParent.transform.localScale.x, cardVM.mainParent.transform.localScale.y, cardVM.mainParent.transform.localScale.z);

        // Shrink card
        cardVM.mainParent.transform.localScale = new Vector3(0.1f, 0.1f, cardVM.mainParent.transform.localScale.z);

        // Scale up
        cardVM.mainParent.DOScale(originalScale, cardTransistionSpeed).SetEase(Ease.OutQuint);

        // move card to the hand;
        Sequence s = DOTween.Sequence();

        // displace the card so that we can select it in the scene easier.
        s.Append(cardVM.mainParent.DOLocalMove(characterView.handVisual.slots.Children[0].transform.localPosition, cardTransistionSpeed));

        s.OnComplete(() => clt.SetHandSortingOrder());
    }
    private void DiscardCardFromHandVisualEvent(CardViewModel cvm, CharacterEntityModel character)
    {
        // remove from hand visual
        character.characterEntityView.handVisual.RemoveCard(cvm.mainParent.gameObject);

        // SFX
        AudioManager.Instance.PlaySound(Sound.Card_Discarded);

        float startScale = cvm.mainParent.localScale.x;
        float endScale = 0.5f;
        float animSpeed = 0.5f;
        cvm.mainParent.DOScale(endScale, animSpeed).SetEase(Ease.OutQuint);

        // move card to the discard pile
        Sequence s = MoveCardVmFromHandToDiscardPile(cvm, character.characterEntityView.handVisual.DiscardPileTransform);

        // Once the anim is finished, destroy the CVM 
        s.OnComplete(() => DestroyCardViewModel(cvm));
    }
    private Sequence MoveCardVmFromHandToDiscardPile(CardViewModel cvm, Transform discardPileLocation)
    {
        Debug.Log("CardController.MoveCardVmFromHandToDiscardPile() called...");

        // move card to the hand;
        Sequence s = DOTween.Sequence();
        // displace the card so that we can select it in the scene easier.
        s.Append(cvm.mainParent.DOMove(discardPileLocation.position, 0.5f));

        return s;
    }   
    private void ExpendCardVisualEvent(CardViewModel cvm, CharacterEntityModel character)
    {
        // remove from hand visual
        character.characterEntityView.handVisual.RemoveCard(cvm.mainParent.gameObject);

        // SFX
        AudioManager.Instance.PlaySound(Sound.Explosion_Fire_1);

        // TO DO: fade out card canvas gradually
        FadeOutCardViewModel(cvm, null, ()=> DestroyCardViewModel(cvm));

        // Create smokey effect
        VisualEffectManager.Instance.CreateExpendEffect(cvm.mainParent.transform.position);
    }
    private void FadeOutCardViewModel(CardViewModel cvm, CoroutineData cData, Action onCompleteCallBack = null)
    {
        StartCoroutine(FadeOutCardViewModelCoroutine(cvm, cData, onCompleteCallBack));
    }
    private IEnumerator FadeOutCardViewModelCoroutine(CardViewModel cvm, CoroutineData cData, Action onCompleteCallBack)
    {
        float fadeSpeed = 1f;

        while(cvm.cg.alpha > 0)
        {
            cvm.cg.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
        }

        if(cData != null)
        {
            cData.MarkAsCompleted();
        }

        if (onCompleteCallBack != null)
        {
            onCompleteCallBack.Invoke();
        }
       
    }
    public void MoveCardVMToPlayPreviewSpot(CardViewModel cvm)
    {
        Transform location = cvm.card.owner.characterEntityView.handVisual.PlayPreviewSpot;
        Sequence s = DOTween.Sequence();
        s.Append(cvm.mainParent.DOMove(location.position, 0.25f));
    }
    private void PlayCardBreathAnimationVisualEvent(CardViewModel cvm)
    {
        StartCoroutine(PlayCardBreathAnimationVisualEventCoroutine(cvm));
    }
    private IEnumerator PlayCardBreathAnimationVisualEventCoroutine(CardViewModel cvm)
    {
        if(cvm != null)
        {
            float currentScale = cvm.mainParent.localScale.x;
            float endScale = currentScale * 1.5f;
            float animSpeed = 0.25f;

            cvm.mainParent.DOScale(endScale, animSpeed).SetEase(Ease.OutQuint);
            yield return new WaitForSeconds(animSpeed);
            cvm.mainParent.DOScale(currentScale, animSpeed).SetEase(Ease.OutQuint);
        }
        
    }
    private void PlayACardFromHandVisualEvent(CardViewModel cvm, CharacterEntityView view)
    {
        Debug.Log("CardController.PlayACardFromHandVisualEvent() called...");

        // Set state and remove from hand visual
        cvm.locationTracker.VisualState = VisualStates.Transition;
        view.handVisual.RemoveCard(cvm.mainParent.gameObject);
        cvm.mainParent.SetParent(null);

        // SFX
        AudioManager.Instance.PlaySound(Sound.Card_Discarded);

        // Create Glow Trail
        ToonEffect glowTrail = VisualEffectManager.Instance.CreateGlowTrailEffect(cvm.mainParent.position);

        float startScale = cvm.mainParent.localScale.x;
        float endScale = 0.1f;
        float animSpeed = 0.5f;
        cvm.mainParent.DOScale(endScale, animSpeed).SetEase(Ease.OutQuint);

        // Move card
        Sequence cardSequence = DOTween.Sequence();
        cardSequence.Append(cvm.mainParent.DOMove(view.handVisual.DiscardPileTransform.position, 0.5f));
        cardSequence.OnComplete(() =>
        {
            DestroyCardViewModel(cvm);
        });

        // Move glow trail
        Sequence glowFollow = DOTween.Sequence();
        glowFollow.Append(glowTrail.transform.DOMove(view.handVisual.DiscardPileTransform.position, 0.5f));
        glowFollow.OnComplete(() =>
        {
            glowTrail.StopAllEmissions();
            Destroy(glowTrail, 3);
        });
    }
    private void UpdateDiscardPileCountText(CharacterEntityView vm, string newValue)
    {
        if (vm)
        {
            vm.discardPileCountText.text = newValue;
        }
    }
    private void UpdateDrawPileCountText(CharacterEntityView vm, string newValue)
    {
        if (vm)
        {
            vm.drawPileCountText.text = newValue;
        }
    }

    #endregion

}
