using Spriter2UnityDX;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatLogic : Singleton<CombatLogic>
{
    // Properties + Variables
    #region
    private CombatGameState currentCombatState;
    public CombatGameState CurrentCombatState
    {
        get { return currentCombatState; }
        private set { currentCombatState = value; }
    }
    #endregion

    // Damage, Damage Type and Resistance Calculators
    #region

    // Entry point for card specific calculations
    public int GetFinalDamageValueAfterAllCalculations(CharacterEntityModel attacker, CharacterEntityModel target, DamageType damageType, int baseDamage, Card card, CardEffect cardEffect)
    {
        return ExecuteGetFinalDamageValueAfterAllCalculations(attacker, target, damageType, baseDamage, card, cardEffect);
    }

    // Entry point for enemy action specific calculations
    public int GetFinalDamageValueAfterAllCalculations(CharacterEntityModel attacker, CharacterEntityModel target, DamageType damageType, int baseDamage, EnemyActionEffect enemyAction)
    {
        return ExecuteGetFinalDamageValueAfterAllCalculations(attacker, target, damageType, baseDamage, null, null, enemyAction);
    }

    // Entry point for non card + enemy action specific calculations
    public int GetFinalDamageValueAfterAllCalculations(CharacterEntityModel attacker, CharacterEntityModel target, DamageType damageType, int baseDamage)
    {
        return ExecuteGetFinalDamageValueAfterAllCalculations(attacker, target, damageType, baseDamage, null, null, null);
    }   

    // Main calculator
    private int ExecuteGetFinalDamageValueAfterAllCalculations(CharacterEntityModel attacker, CharacterEntityModel target, DamageType damageType, int baseDamage = 0, Card card = null, CardEffect cardEffect = null, EnemyActionEffect enemyAction = null)
    {
        Debug.Log("CombatLogic.GetFinalDamageValueAfterAllCalculations() called...");
        int finalDamageValueReturned = 0;

        // calculate base damage
        finalDamageValueReturned = GetBaseDamageValue(attacker, baseDamage, damageType, card, cardEffect, enemyAction);

        // calculate damage after standard modifiers
        finalDamageValueReturned = GetDamageValueAfterNonResistanceModifiers(finalDamageValueReturned, attacker, target, damageType, card, cardEffect, enemyAction);

        // calculate damage after resistances
        finalDamageValueReturned = GetDamageValueAfterResistances(finalDamageValueReturned, damageType, target);

        // return final value
        return finalDamageValueReturned;

    }
    public DamageType GetFinalFinalDamageTypeOfAttack(CharacterEntityModel entity, CardEffect cardEffect = null, Card card = null, EnemyActionEffect enemyAction = null)
    {
        Debug.Log("CombatLogic.CalculateFinalDamageTypeOfAttack() called...");

        DamageType damageTypeReturned = DamageType.None;

        // First, draw damage type from ability
        if (cardEffect != null)
        {
            damageTypeReturned = cardEffect.damageType;
        }

        // draw from enemyAction if enemy
        if (enemyAction != null)
        {
            damageTypeReturned = enemyAction.damageType;
        }

        Debug.Log("CombatLogic.CalculateFinalDamageTypeOfAttack() final damage type returned: " + damageTypeReturned);
        return damageTypeReturned;
    }
    private int GetBaseDamageValue(CharacterEntityModel entity, int baseDamage, DamageType damageType, Card card = null, CardEffect cardEffect = null, EnemyActionEffect enemyAction = null)
    {
        Debug.Log("CombatLogic.GetBaseDamageValue() called...");
        int baseDamageValueReturned = 0;

        baseDamageValueReturned += baseDamage;

        // Add flat damage bonus from modifiers (strength, etc)
        if (card != null || enemyAction != null)
        {
            baseDamageValueReturned += EntityLogic.GetTotalPower(entity);
            Debug.Log("Card base damage after strength and related modifiers added: " + baseDamageValueReturned.ToString());
        }

        // Add flat bonus damage from misc passives
        // Bonus fire ball damage
        if (card != null &&
             (card.cardName == "Fire Ball" || card.cardName == "Fire Ball +1" ||
             card.cardName == "Mega Fire Ball" || card.cardName == "Mega Fire Ball +1"))
        {
            baseDamageValueReturned += entity.pManager.fireBallBonusDamageStacks;
            Debug.Log("Card base damage after bonus fire ball damage added: " + baseDamageValueReturned.ToString());
        }

        // Reflex Shot bonus damage
        if (card != null &&
             (card.cardName == "Reflex Shot" || card.cardName == "Reflex Shot +1"))
        {
            baseDamageValueReturned += entity.pManager.reflexShotBonusDamageStacks;
            Debug.Log("Card base damage after bonus reflex shot damage added: " + baseDamageValueReturned.ToString());
        }

        // Shank/Ruthless damage bonus
        if (card != null &&
            (card.cardName == "Shank" || card.cardName == "Shank +1"))
        {
            baseDamageValueReturned += entity.pManager.ruthlessStacks;
            Debug.Log("Card base damage after bonus Ruthless/Shank damage added: " + baseDamageValueReturned.ToString());
        }

        // Arcane Bolt / Ethereal damage bonus
        if (card != null &&
             (card.cardName == "Arcane Bolt" || card.cardName == "Arcane Bolt +1" ||
             card.cardName == "Mega Arcane Bolt" || card.cardName == "Mega Arcane Bolt +1"))
        {
            baseDamageValueReturned += entity.pManager.etherealStacks;
            Debug.Log("Card base damage after bonus Ethereal damage added: " + baseDamageValueReturned.ToString());
        }

        // return final value
        Debug.Log("Final base damage value of attack returned: " + baseDamageValueReturned.ToString());
        return baseDamageValueReturned;

    }
    private int GetDamageValueAfterResistances(int damageValue, DamageType damageType, CharacterEntityModel target)
    {
        // Debug
        Debug.Log("CombatLogic.GetDamageValueAfterResistances() called...");
        Debug.Log("Damage Type received as argument: " + damageType.ToString());

        // Setup
        int damageValueReturned = damageValue;
        int targetResistance = 0;
        float resistanceMultiplier = 0;

        
        //Get total resistance
        if(target != null)
        {
            targetResistance = GetTotalResistance(target, damageType);
        }
        

        // Debug
        Debug.Log("Target has " + targetResistance + " total " + damageType.ToString() + " Resistance...");

        // Invert the resistance value from 100. (as in, 80% fire resistance means the attack will deal 20% of it original damage
        int invertedResistanceValue = 100 - targetResistance;
        Debug.Log("Resitance value after inversion: " + invertedResistanceValue.ToString());

        // Convert target resistance to float to multiply against base damage value
        resistanceMultiplier = (float)invertedResistanceValue / 100;
        Debug.Log("Resitance multiplier as float value: " + resistanceMultiplier.ToString());

        // Apply final resistance calculations to the value returned
        damageValueReturned = (int)(damageValueReturned * resistanceMultiplier);

        Debug.Log("Final damage value calculated: " + damageValueReturned.ToString());

        return damageValueReturned;
    }
    private int GetDamageValueAfterNonResistanceModifiers(int damageValue, CharacterEntityModel attacker, CharacterEntityModel target, DamageType damageType, Card card = null, CardEffect cardEffect = null, EnemyActionEffect enemyAction = null)
    {
        Debug.Log("CombatLogic.GetDamageValueAfterNonResistanceModifiers() called...");

        int damageValueReturned = damageValue;
        float damageModifier = 1f;

        // These effects only apply to damage from cards, or from enemy abilities
        // they are not triggered by passives like poisoned damage
        if ((card != null && cardEffect != null) ||
            enemyAction != null)
        {
            // vulnerable
            if (target != null && target.pManager.vulnerableStacks > 0)
            {
                damageModifier += 0.3f;
                Debug.Log("Damage percentage modifier after 'Vulnerable' bonus: " + damageModifier.ToString());
            }

            // wrath
            if (attacker.pManager.wrathStacks > 0)
            {
                damageModifier += 0.3f;
                Debug.Log("Damage percentage modifier after 'wrath' bonus: " + damageModifier.ToString());
            }

            // grit
            if (target != null && target.pManager.gritStacks > 0)
            {
                damageModifier -= 0.3f;
                Debug.Log("Damage percentage modifier after 'grit' bonus: " + damageModifier.ToString());
            }

            // weakened
            if (attacker.pManager.weakenedStacks > 0)
            {
                damageModifier -= 0.3f;
                Debug.Log("Damage percentage modifier after 'weakened' reduction: " + damageModifier.ToString());
            }
        }

        // Card specific modifiers

        // Long Draw
        if (card != null && cardEffect != null && card.cardType == CardType.RangedAttack)
        {
            if(attacker.pManager.longDrawStacks > 0)
            {
                damageModifier += 1f;
                Debug.Log("Damage percentage modifier after 'Long Draw' passive: " + damageModifier.ToString());
            }
        }

        // Sharpen Blade
        if (card != null && cardEffect != null && card.cardType == CardType.MeleeAttack)
        {
            if (attacker.pManager.sharpenBladeStacks > 0)
            {
                damageModifier += 1f;
                Debug.Log("Damage percentage modifier after 'Sharpen Blade' passive: " + damageModifier.ToString());
            }
        }


        // TO DO: Damage modifiers related to increasing magical damage by percentage should be moved to a new method (make some like CalculateMagicDamageModifiers())

        // Air Damage bonuses
        /*
        if (damageType == "Air")
        {
            if (attacker.myPassiveManager.stormLord)
            {
                Debug.Log("Damage has a type of 'Air', and attacker has 'Storm Lord' passive, increasing damage by 30%...");
                damageModifier += 0.3f;
            }
        }

        // Fire Damage bonuses
        if (damageType == "Fire")
        {
            if (attacker.myPassiveManager.demon)
            {
                Debug.Log("Damage has a type of 'Fire', and attacker has 'Demon' passive, increasing damage by 30%...");
                damageModifier += 0.3f;
            }
        }

        // Poison Damage bonuses
        if (damageType == "Poison")
        {
            if (attacker.myPassiveManager.toxicity)
            {
                Debug.Log("Damage has a type of 'Poison', and attacker has 'Toxicity' passive, increasing damage by 30%...");
                damageModifier += 0.3f;
            }
        }

        // Frost Damage bonuses
        if (damageType == "Frost")
        {
            if (attacker.myPassiveManager.frozenHeart)
            {
                Debug.Log("Damage has a type of 'Frost', and attacker has 'Frozen Heart' passive, increasing damage by 30%...");
                damageModifier += 0.3f;
            }
        }

        // Frost Damage bonuses
        if (damageType == "Shadow")
        {
            if (attacker.myPassiveManager.shadowForm)
            {
                Debug.Log("Damage has a type of 'Shadow', and attacker has 'Shadow Form' passive, increasing damage by 30%...");
                damageModifier += 0.3f;
            }

            if (attacker.myPassiveManager.pureHate)
            {
                Debug.Log("Damage has a type of 'Shadow', and attacker has 'Pure Hate' passive, increasing damage by 50%...");
                damageModifier += 0.5f;
            }
        } 
        */

        // prevent modifier from going negative
        if (damageModifier < 0)
        {
            Debug.Log("Damage percentage modifier went into negative, setting to 0");
            damageModifier = 0;
        }

        //damageValueReturned = (int)(damageValueReturned * damageModifier);
        damageValueReturned = (int)Math.Ceiling(damageValueReturned * damageModifier);
        Debug.Log("Final damage value returned: " + damageValueReturned);

        return damageValueReturned;

    }
    public int GetTotalResistance(CharacterEntityModel target, DamageType damageType)
    {
        int valueReturned = 0;
        if(damageType == DamageType.Physical)
        {
            valueReturned += target.basePhysicalResistance;
        }
        else if (damageType == DamageType.Magic)
        {
            valueReturned += target.baseMagicResistance;
        }

        return valueReturned;
    }
    #endregion

    // Calculate Block Gain
    #region
    public int CalculateBlockGainedByEffect(int baseBlockGain, CharacterEntityModel caster, CharacterEntityModel target, EnemyActionEffect enemyEffect = null, CardEffect cardEffect = null)
    {
        int valueReturned = baseBlockGain;
        Debug.Log("Base block gain value: " + valueReturned);

        // Dexterity bonus only applies when playing a card,
        // or from enemy abilities (passives like 'Shield Wall' dont 
        // get the dexterity bonus
        if ((cardEffect != null ||
            enemyEffect != null) && valueReturned > 0)
        {
            valueReturned += EntityLogic.GetTotalDexterity(caster);
            Debug.Log("Block gain value after dexterity added: " + valueReturned);
        }


        Debug.Log("Final block gain value calculated: " + valueReturned);
        return valueReturned;
    }
    #endregion

    // Handle damage + death
    #region   

    // Handle damage from card entry points
    public void HandleDamage(int damageAmount, CharacterEntityModel attacker, CharacterEntityModel victim, Card card, DamageType damageType, bool ignoreBlock = false)
    {
        ExecuteHandleDamage(damageAmount, attacker, victim, damageType, card, null, ignoreBlock);
    }
    public void HandleDamage(int damageAmount, CharacterEntityModel attacker, CharacterEntityModel victim, Card card, DamageType damageType, VisualEvent batchedEvent, bool ignoreBlock = false)
    {
        ExecuteHandleDamage(damageAmount, attacker, victim, damageType, card, null, ignoreBlock, batchedEvent);
    }

    // Handle damage from enemy action entry points
    public void HandleDamage(int damageAmount, CharacterEntityModel attacker, CharacterEntityModel victim, EnemyActionEffect enemyEffect, DamageType damageType, bool ignoreBlock = false)
    {
        ExecuteHandleDamage(damageAmount, attacker, victim, damageType, null, enemyEffect, ignoreBlock);
    }
    public void HandleDamage(int damageAmount, CharacterEntityModel attacker, CharacterEntityModel victim, EnemyActionEffect enemyEffect, DamageType damageType, VisualEvent batchedEvent, bool ignoreBlock = false)
    {
        ExecuteHandleDamage(damageAmount, attacker, victim, damageType, null, enemyEffect, ignoreBlock, batchedEvent);
    }

    // Cardless + Enemy action less damage entry points
    public void HandleDamage(int damageAmount, CharacterEntityModel attacker, CharacterEntityModel victim, DamageType damageType, bool ignoreBlock = false)
    {
        ExecuteHandleDamage(damageAmount, attacker, victim, damageType, null, null, ignoreBlock);
    }
    public void HandleDamage(int damageAmount, CharacterEntityModel attacker, CharacterEntityModel victim, DamageType damageType, VisualEvent batchedEvent, bool ignoreBlock = false)
    {
        ExecuteHandleDamage(damageAmount, attacker, victim, damageType, null, null, ignoreBlock, batchedEvent);
    }

    // Main Damage Handler
    private void ExecuteHandleDamage(int damageAmount, CharacterEntityModel attacker, CharacterEntityModel victim,
        DamageType damageType, Card card = null, EnemyActionEffect enemyEffect = null, bool ignoreBlock = false, VisualEvent batchedEvent = null)
    {
        // Debug setup
        string cardNameString = "None";
        string attackerName = "No Attacker";
        string victimName = "No Victim";

        // batched event set up
        QueuePosition queuePosition = QueuePosition.Back;
        if (batchedEvent != null)
        {
            queuePosition = QueuePosition.BatchedEvent;
        }

        if (attacker != null)
        {
            attackerName = attacker.myName;
        }
        if (victim != null)
        {
            victimName = victim.myName;
        }
        if (card != null)
        {
            cardNameString = card.cardName;
        }

        Debug.Log("CombatLogic.HandleDamage() started: damageAmount (" + damageAmount.ToString() + "), attacker (" + attackerName +
            "), victim (" + victimName + "), damageType (" + damageType.ToString() + "), card (" + cardNameString + "), ignoreBlock (" + ignoreBlock.ToString()
            );

        // Cancel this if character is already in death process
        if (victim.livingState == LivingState.Dead)
        {
            Debug.Log("CombatLogic.HandleDamage() detected that victim " + victim.myName + " is already in death process, exiting damage event...");
            return;
        }

        // Cancel if attacker already dead
        if (attacker != null && attacker.livingState != LivingState.Alive)
        {
            Debug.Log("CombatLogic.HandleDamage() detected that attacker " + attacker.myName + " is already in death process, exiting damage event...");
            return;
        }

        // Establish properties for this damage event
        int totalLifeLost = 0;
        int adjustedDamageValue = damageAmount;
        int startingBlock = victim.block;
        int blockAfter = victim.block;
        int healthAfter = victim.health;

        // TO DO IN FUTURE: check for pierce here

        // Check for no block
        if (victim.block == 0)
        {
            healthAfter = victim.health - adjustedDamageValue;
            blockAfter = 0;
        }

        // Check for block
        else if (victim.block > 0)
        {
            if (ignoreBlock == false)
            {
                blockAfter = victim.block;
                Debug.Log("block after = " + blockAfter);
                blockAfter = blockAfter - adjustedDamageValue;
                Debug.Log("block after = " + blockAfter);
                if (blockAfter < 0)
                {
                    healthAfter = victim.health;
                    healthAfter += blockAfter;
                    blockAfter = 0;
                    Debug.Log("block after = " + blockAfter);
                }
            }

            // Check if damage event ignores block (poisoned, burning, pierce, etc)
            else if (ignoreBlock)
            {
                blockAfter = victim.block;
                Debug.Log("block after = " + blockAfter);
                healthAfter = victim.health - adjustedDamageValue;
            }
        }

        // Check for damage immunity passives

        if (victim.pManager.barrierStacks > 0 &&
            healthAfter < victim.health)
        {
            PassiveController.Instance.ModifyBarrier(victim.pManager, -1, true);
            adjustedDamageValue = 0;
            healthAfter = victim.health;

            // Create impact effect
            VisualEventManager.Instance.CreateVisualEvent(() =>
            VisualEffectManager.Instance.CreateSmallMeleeImpact(victim.characterEntityView.WorldPosition, totalLifeLost), queuePosition, 0, 0, EventDetail.None, batchedEvent);

            // Create SFX 
            VisualEventManager.Instance.CreateVisualEvent(() =>
            {
                AudioManager.Instance.PlaySoundPooled(Sound.Ability_Holy_Buff);
                AudioManager.Instance.PlaySoundPooled(Sound.Ability_Damaged_Health_Lost);
            }, queuePosition, 0, 0, EventDetail.None, batchedEvent);
        }


        // Finished calculating the final damage, health lost and armor lost: p
        totalLifeLost = victim.health - healthAfter;
        CharacterEntityController.Instance.ModifyHealth(victim, -totalLifeLost);
        CharacterEntityController.Instance.SetBlock(victim, blockAfter);

        // Play VFX depending on whether the victim lost health, block, or was damaged by poison
        if (adjustedDamageValue > 0)
        {
            if (totalLifeLost == 0 && blockAfter < startingBlock)
            {
                // Create Lose Block Effect
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateLoseBlockEffect(victim.characterEntityView.WorldPosition, adjustedDamageValue), queuePosition, 0, 0, EventDetail.None, batchedEvent);

            }
            else if (totalLifeLost > 0)
            {
                victim.hasLostHealthThisCombat = true;

                // Play hurt animation
                if (victim.health > 0 && totalLifeLost > 0)
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    CharacterEntityController.Instance.PlayHurtAnimation(victim.characterEntityView), queuePosition, 0, 0, EventDetail.None, batchedEvent);
                }

                // Create damage text effect
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateDamageEffect(victim.characterEntityView.WorldPosition, totalLifeLost), queuePosition, 0, 0, EventDetail.None, batchedEvent);

                // Create impact effect
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateSmallMeleeImpact(victim.characterEntityView.WorldPosition, totalLifeLost), queuePosition, 0, 0, EventDetail.None, batchedEvent);

                VisualEventManager.Instance.CreateVisualEvent(() =>
                AudioManager.Instance.PlaySoundPooled(Sound.Ability_Damaged_Health_Lost), queuePosition, 0, 0, EventDetail.None, batchedEvent);

                // Create SFX 
                VisualEventManager.Instance.CreateVisualEvent(() =>
                AudioManager.Instance.PlaySound(victim.audioProfile, AudioSet.Hurt), queuePosition, 0, 0, EventDetail.None, batchedEvent);

            }
        }

        // Card 'on damaged' event
        if (totalLifeLost > 0 && victim.controller == Controller.Player)
        {
            CardController.Instance.HandleOnCharacterDamagedCardListeners(victim);
        }

        // Card lifesteal effect
        if(totalLifeLost > 0 && card != null && card.lifeSteal)
        {
            VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
            CharacterEntityController.Instance.ModifyHealth(attacker, totalLifeLost);

            // Heal VFX
            VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateHealEffect(attacker.characterEntityView.WorldPosition, totalLifeLost), queuePosition, 0, 0, EventDetail.None, batchedEvent);

            // Create heal text effect
            VisualEventManager.Instance.CreateVisualEvent(() =>
            VisualEffectManager.Instance.CreateDamageEffect(attacker.characterEntityView.WorldPosition, totalLifeLost, true), queuePosition, 0, 0, EventDetail.None, batchedEvent);

            // Create SFX
            VisualEventManager.Instance.CreateVisualEvent(() =>
                AudioManager.Instance.PlaySoundPooled(Sound.Passive_General_Buff), queuePosition, 0, 0, EventDetail.None, batchedEvent);
        }

        // Resolve thorns passive before other post damage passive events
        if (victim != null &&
            attacker != null &&
            attacker.health > 0 &&
            victim.pManager.thornsStacks > 0 && 
            victim.livingState == LivingState.Alive && 
            attacker.livingState == LivingState.Alive &&
            (enemyEffect != null || card != null))
        {
            // Brief delay 
            VisualEventManager.Instance.InsertTimeDelayInQueue(0.25f);

            // Calculate and handle damage
            int thornsDamageValue = GetFinalDamageValueAfterAllCalculations(null, attacker, DamageType.Physical, victim.pManager.thornsStacks, null, null);
            HandleDamage(thornsDamageValue, null, attacker, DamageType.Physical);
        }

        // Resolve storm shield passive before other post damage passive events
        if (victim != null &&
            attacker != null &&
            attacker.health > 0 &&
            victim.pManager.stormShieldStacks > 0 &&
            victim.livingState == LivingState.Alive &&
            attacker.livingState == LivingState.Alive &&
            (enemyEffect != null || card != null))
        {
            // Brief delay 
            VisualEventManager.Instance.InsertTimeDelayInQueue(0.25f);

            // Calculate and handle damage
            int ssDamageValue = GetFinalDamageValueAfterAllCalculations(null, attacker, DamageType.Magic, victim.pManager.stormShieldStacks, null, null);
            HandleDamage(ssDamageValue, null, attacker, DamageType.Magic);
        }

        // EVALUATE DAMAGE RELATED PASSIVE EFFECTS (but only if victim is still alive)
        if (victim.health > 0 && victim.livingState == LivingState.Alive)
        {
            // Sleep
            if (victim.pManager.sleepStacks > 0 && totalLifeLost > 0)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
                PassiveController.Instance.ModifySleep(victim.pManager, -victim.pManager.sleepStacks, true);
            }

            // Cautious
            if (victim.pManager.cautiousStacks > 0 && totalLifeLost > 0)
            {
                Debug.Log(victim.myName + " 'Cautious' triggered, gaining " + victim.pManager.enrageStacks.ToString() + " Block");
                VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);

                // Calculate and apply block gain
                CharacterEntityController.Instance.ModifyBlock(victim, CalculateBlockGainedByEffect(victim.pManager.cautiousStacks, victim, victim));

                // Remove cautious
                PassiveController.Instance.ModifyCautious(victim.pManager, -victim.pManager.cautiousStacks, true);
            }

            // Enrage
            if (victim.pManager.enrageStacks > 0 && totalLifeLost > 0)
            {
                Debug.Log(victim.myName + " 'Enrage' triggered, gaining " + victim.pManager.enrageStacks.ToString() + " bonus power");
                VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
                PassiveController.Instance.ModifyBonusPower(victim.pManager, victim.pManager.enrageStacks, true);
            }

            // Battle Trance
            if (victim.pManager.battleTranceStacks > 0 && totalLifeLost > 0)
            {
                Debug.Log(victim.myName + " 'Battle Trance' triggered");
                VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
                PassiveController.Instance.ModifyTemporaryStamina(victim.pManager, victim.pManager.battleTranceStacks, true);

                if (victim.controller == Controller.Player)
                {
                    VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
                    PassiveController.Instance.ModifyTemporaryDraw(victim.pManager, victim.pManager.battleTranceStacks, true);
                }
            }

            // Poisonous 
            if (attacker != null &&
                attacker.pManager.poisonousStacks > 0 &&
                totalLifeLost > 0)
            {
                if (card != null &&
                   (card.cardType == CardType.MeleeAttack || card.cardType == CardType.RangedAttack))
                {
                    VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
                    PassiveController.Instance.ModifyPoisoned(attacker, victim.pManager, attacker.pManager.poisonousStacks, true, 0.5f);
                }
                else if (enemyEffect != null &&
                   (enemyEffect.actionType == ActionType.AttackTarget ||
                    enemyEffect.actionType == ActionType.AttackAllEnemies)
                    )
                {
                    VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
                    PassiveController.Instance.ModifyPoisoned(attacker, victim.pManager, attacker.pManager.poisonousStacks, true, 0.5f);
                }
            }

            // Inflamed 
            if (attacker != null &&
                attacker.pManager.inflamedStacks > 0 &&
                totalLifeLost > 0)
            {
                if (card != null &&
                   (card.cardType == CardType.MeleeAttack || card.cardType == CardType.RangedAttack))
                {
                    VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
                    PassiveController.Instance.ModifyBurning(victim.pManager, attacker.pManager.inflamedStacks, true, 0.5f);
                }
                else if (enemyEffect != null &&
                   (enemyEffect.actionType == ActionType.AttackTarget ||
                    enemyEffect.actionType == ActionType.AttackAllEnemies)
                    )
                {
                    VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
                    PassiveController.Instance.ModifyBurning(victim.pManager, attacker.pManager.inflamedStacks, true, 0.5f);
                }
            }

            // Poison Arrows
            if (attacker != null &&
                card != null)
            {
                int poisonApplied = 0;

                // Find poison arrow cards
                foreach (Card c in attacker.hand)
                {
                    if (c.cardName == "Poison Arrows")
                    {
                        poisonApplied += 1;
                    }
                }

                // Apply poison to target
                if (poisonApplied > 0)
                {
                    VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
                    PassiveController.Instance.ModifyPoisoned(attacker, victim.pManager, poisonApplied, true, 0.5f);
                }
            }
        }           

        // DEATH?!
        // Check if the victim was killed by the damage
        if (victim.health <= 0 && victim.livingState == LivingState.Alive)
        {
            Debug.Log(victim.myName + " has lost enough health to be killed by this damage event...");

            // Check if card has events when target is killed
            if(card != null)
            {
                CardController.Instance.HandleOnTargetKilledEventListeners(card);
            }

            // Check Volatile passive
            if(victim.pManager.volatileStacks > 0)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);

                foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(victim))
                {
                    PassiveController.Instance.ModifyPoisoned(victim, enemy.pManager, victim.pManager.volatileStacks);
                }
            }

            // Check soul collector passive on allies
            List<CharacterEntityModel> victimAllies = CharacterEntityController.Instance.GetAllAlliesOfCharacter(victim, false);
            foreach(CharacterEntityModel ally in victimAllies)
            {
                if(ally.pManager.soulCollectorStacks > 0)
                {
                    PassiveController.Instance.ModifyBonusPower(ally.pManager, ally.pManager.soulCollectorStacks, true);
                }
            }


            HandleDeath(victim);
        }
    }


    private void HandleDeath(CharacterEntityModel entity)
    {
        Debug.Log("CombatLogic.HandleDeath() started for " + entity.myName);

        // Cache relevant references for visual events
        CharacterEntityView view = entity.characterEntityView;
        LevelNode node = entity.levelNode;
        ActivationWindow window = view.myActivationWindow;

        // Mark as dead
        entity.livingState = LivingState.Dead;

        // Remove from persitency
        if (entity.allegiance == Allegiance.Enemy)
        {
            CharacterEntityController.Instance.RemoveEnemyFromPersistency(entity);
        }
        else if (entity.allegiance == Allegiance.Player)
        {
            CharacterEntityController.Instance.RemoveDefenderFromPersistency(entity);
        }

        // Remove from activation order
        ActivationManager.Instance.RemoveEntityFromActivationOrder(entity);

        // If an AI character was targetting the dying character with its next action, aquire a new target
        foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.AllEnemies)
        {
            if (enemy.currentActionTarget == entity)
            {
                CharacterEntityController.Instance.AutoAquireNewTargetOfCurrentAction(enemy);
            }
        }


        // Disable character's level node anims and targetting path
        VisualEventManager.Instance.CreateVisualEvent(() =>
        {
            CharacterEntityController.Instance.DisableAllDefenderTargetIndicators();
            LevelManager.Instance.SetMouseOverViewState(node, false);
        });

        // Fade out world space GUI
        VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.FadeOutCharacterWorldCanvas(view, null));

        // Play death animation
        VisualEventManager.Instance.CreateVisualEvent(() => AudioManager.Instance.PlaySound(entity.audioProfile, AudioSet.Die));
        VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.PlayDeathAnimation(view), QueuePosition.Back, 0f, 1f);

        // Smokey disapear effect
        VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateExpendEffect(view.WorldPosition, 15, 0.2f, false));

        // Fade out UCM
        CoroutineData fadeOutCharacter = new CoroutineData();
        VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.FadeOutEntityRenderer(view.ucm.GetComponent<EntityRenderer>(), 2.5f, fadeOutCharacter));

        // Destroy characters activation window and update other window positions
        CharacterEntityModel currentlyActivatedEntity = ActivationManager.Instance.EntityActivated;
        VisualEventManager.Instance.CreateVisualEvent(() => ActivationManager.Instance.OnCharacterKilledVisualEvent(window, currentlyActivatedEntity, null), QueuePosition.Back, 0, 1f);

        // Break references
        LevelManager.Instance.DisconnectEntityFromNode(entity, node);

        // Destroy view and break references
        VisualEventManager.Instance.CreateVisualEvent(() =>
        {
            // Destroy view gameobject
            CharacterEntityController.Instance.DisconnectModelFromView(entity);
            CharacterEntityController.Instance.DestroyCharacterView(view);
        });

        // If character dying has taunted others, remove taunt from the other characters
        foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(entity))
        {
            if (enemy.pManager.myTaunter == entity)
            {
                PassiveController.Instance.ModifyTaunted(null, enemy.pManager, -enemy.pManager.tauntStacks);
            }
        }

        // Check if the game over defeat event should be triggered
        if (CharacterEntityController.Instance.AllDefenders.Count == 0)
        {
            StartCombatOverDefeatProcess();
        }

        // Check if the combat victory event should be triggered
        if (CharacterEntityController.Instance.AllEnemies.Count == 0 &&
            currentCombatState == CombatGameState.CombatActive)
        {
            StartCombatOverVictoryProcess();
        }

        // If this character died during their turn (but no during end turn phase), 
        // resolve the transition to next character activation
        if (entity == ActivationManager.Instance.EntityActivated)
        {
            ActivationManager.Instance.ActivateNextEntity();
        }

    }
    #endregion

    // Misc Functions
    #region
    public string GetRandomDamageType()
    {
        // Setup
        string damageTypeReturned = "Unassigned";
        List<string> allDamageTypes = new List<string> { "Air", "Fire", "Poison", "Physical", "Shadow", "Frost" };

        // Calculate random damage type
        damageTypeReturned = allDamageTypes[RandomGenerator.NumberBetween(0, allDamageTypes.Count)];
        Debug.Log("CombatLogic.GetRandomDamageType() randomly generated a damage type of: " + damageTypeReturned);

        // return damage type
        return damageTypeReturned;
    }
    public void SetCombatState(CombatGameState newState)
    {
        Debug.Log("CombatLogic.SetCombatState() called, new state: " + newState.ToString());
        CurrentCombatState = newState;
    }
    #endregion

    // Game Over Logic
    #region
    private void StartCombatOverDefeatProcess()
    {
        Debug.Log("CombatLogic.StartCombatOverDefeatProcess() called...");
        currentCombatState = CombatGameState.DefeatTriggered;
    }
    private void StartCombatOverVictoryProcess()
    {
        Debug.Log("CombatLogic.StartCombatOverVictoryProcess() called...");
        currentCombatState = CombatGameState.VictoryTriggered;
        EventSequenceController.Instance.StartCombatVictorySequence(JourneyManager.Instance.CurrentEncounter.encounterType);

    }
    #endregion

}
