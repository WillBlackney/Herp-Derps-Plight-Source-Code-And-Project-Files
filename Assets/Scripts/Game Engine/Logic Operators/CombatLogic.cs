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
            card.cardName == "Fire Ball")
        {
            baseDamageValueReturned += entity.pManager.fireBallBonusDamageStacks;
            Debug.Log("Card base damage after bonus fire ball damage added: " + baseDamageValueReturned.ToString());
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

        // Get total resistance
        //targetResistance = EntityLogic.GetTotalResistance(target, attackDamageType);

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
            if (target.pManager.vulnerableStacks > 0)
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
            if (target.pManager.gritStacks > 0)
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
    public int GetFinalDamageValueAfterAllCalculations(CharacterEntityModel attacker, CharacterEntityModel target, DamageType damageType, bool critical, int baseDamage = 0, Card card = null, CardEffect cardEffect = null, EnemyActionEffect enemyAction = null)
    {
        Debug.Log("CombatLogic.GetFinalDamageValueAfterAllCalculations() called...");
        int finalDamageValueReturned = 0;

        if (enemyAction == null)
        {
            Debug.Log("CombatLogic.GetFinalDamageValueAfterAllCalculations() was given a null EnemyAction argument...");
        }
        else if (enemyAction != null)
        {
            Debug.Log("CombatLogic.GetFinalDamageValueAfterAllCalculations() was NOT given a null EnemyAction argument...");
        }

        // calculate base damage
        finalDamageValueReturned = GetBaseDamageValue(attacker, baseDamage, damageType, card, cardEffect, enemyAction);
        Debug.Log("CombatLogic.GetFinalDamageValueAfterAllCalculations() finalDamageValueReturned value after base calculations: " + finalDamageValueReturned.ToString());

        // calculate damage after standard modifiers
        finalDamageValueReturned = GetDamageValueAfterNonResistanceModifiers(finalDamageValueReturned, attacker, target, damageType, card, cardEffect, enemyAction);
        Debug.Log("CombatLogic.GetFinalDamageValueAfterAllCalculations() finalDamageValueReturned value after non resistance modifier calculations: " + finalDamageValueReturned.ToString());

        // calculate damage after resistances
        finalDamageValueReturned = GetDamageValueAfterResistances(finalDamageValueReturned, damageType, target);
        Debug.Log("CombatLogic.GetFinalDamageValueAfterAllCalculations() finalDamageValueReturned value after resitance type calculations: " + finalDamageValueReturned.ToString());

        // calcualte damage value after resistances
        /*
        if (attacker.defender &&
            StateManager.Instance.DoesPlayerAlreadyHaveState("Godly"))
        {
            Debug.Log("CombatLogic.GetFinalDamageValueAfterAllCalculations() detected that attacker is defender and has state 'Godly', ignoring target resistances...");
        }

    else
    {
        finalDamageValueReturned = GetDamageValueAfterResistances(finalDamageValueReturned, damageType, target);
        Debug.Log("CombatLogic.GetFinalDamageValueAfterAllCalculations() finalDamageValueReturned value after resitance type calculations: " + finalDamageValueReturned.ToString());
    }
    */

        // return final value
        Debug.Log("CombatLogic.GetFinalDamageValueAfterAllCalculations() finalDamageValueReturned final value returned: " + finalDamageValueReturned.ToString());
        return finalDamageValueReturned;

    }
    public DamageType CalculateFinalDamageTypeOfAttack(CharacterEntityModel entity, CardEffect cardEffect = null, Card card = null, EnemyActionEffect enemyAction = null)
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

        // Third, if character has a 'permanent' imbuement, get damage type from that passive
        /*
        if (
            (cardEffect != null && card != null && (card.cardType == CardType.MeleeAttack || card.cardType == CardType.RangedAttack)) ||
             entity.enemy
            )
        {
            Debug.Log("CombatLogic.CalculateFinalDamageTypeOfAttack() checking for damage type imbuements...");
            if (entity.myPassiveManager.airImbuement)
            {
                Debug.Log(entity.name + " has Air Imbuement!");
                damageTypeReturned = "Air";
            }
            else if (entity.myPassiveManager.fireImbuement)
            {
                Debug.Log(entity.name + " has Fire Imbuement!");
                damageTypeReturned = "Fire";
            }
            else if (entity.myPassiveManager.poisonImbuement)
            {
                Debug.Log(entity.name + " has Poison Imbuement!");
                damageTypeReturned = "Poison";
            }
            else if (entity.myPassiveManager.frostImbuement)
            {
                Debug.Log(entity.name + " has Frost Imbuement!");
                damageTypeReturned = "Frost";
            }
            else if (entity.myPassiveManager.shadowImbuement)
            {
                Debug.Log(entity.name + " has Shadow Imbuement!");
                damageTypeReturned = "Shadow";
            }
        }
        */
        /*
        // Fourth, if character has a temporary imbuement, get damage type from that (override permanent imbuement)
        if (
            (cardEffect != null && card != null && (card.cardType == CardType.MeleeAttack || card.cardType == CardType.RangedAttack)) ||
             entity.enemy
            )
        {
            Debug.Log("CombatLogic.CalculateFinalDamageTypeOfAttack() checking for TEMPORARY damage type imbuements...");

            if (entity.myPassiveManager.temporaryAirImbuement)
            {
                Debug.Log(entity.name + " has Temporary Air Imbuement!");
                damageTypeReturned = "Air";
            }
            else if (entity.myPassiveManager.temporaryFireImbuement)
            {
                Debug.Log(entity.name + " has Temporary Fire Imbuement!");
                damageTypeReturned = "Fire";
            }
            else if (entity.myPassiveManager.temporaryPoisonImbuement)
            {
                Debug.Log(entity.name + " has Temporary Poison Imbuement!");
                damageTypeReturned = "Poison";
            }
            else if (entity.myPassiveManager.temporaryFrostImbuement)
            {
                Debug.Log(entity.name + " has Temporary Frost Imbuement!");
                damageTypeReturned = "Frost";
            }
            else if (entity.myPassiveManager.temporaryShadowImbuement)
            {
                Debug.Log(entity.name + " has Temporary Shadow Imbuement!");
                damageTypeReturned = "Shadow";
            }
        }
        */

        Debug.Log("CombatLogic.CalculateFinalDamageTypeOfAttack() final damage type returned: " + damageTypeReturned);
        return damageTypeReturned;
    }
    #endregion

    // Calculate Block Gain
    #region
    public int CalculateBlockGainedByEffect(int baseBlockGain, CharacterEntityModel caster, CharacterEntityModel target, EnemyActionEffect enemyEffect = null, CardEffect cardEffect = null)
    {
        Debug.Log("CombatLogic.CalculateBlockGainedByEffect() called for " + caster.myName + " against target: " + target.myName);

        int valueReturned = baseBlockGain;
        Debug.Log("Base block gain value: " + valueReturned);

        // Dexterity bonus only applies when playing a card,
        // or from enemy abilities (passives like 'Shield Wall' dont 
        // get the dexterity bonus
        if (cardEffect != null ||
            enemyEffect != null)
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
        if(entity.allegiance == Allegiance.Enemy)
        {
            CharacterEntityController.Instance.RemoveEnemyFromPersistency(entity);
        }
        else if (entity.allegiance == Allegiance.Player)
        {
            CharacterEntityController.Instance.RemoveDefenderFromPersistency(entity);

            // if an enemy was targetting the player character that just died, find a new target
            foreach(CharacterEntityModel enemy in CharacterEntityController.Instance.AllEnemies)
            {
                CharacterEntityController.Instance.AutoAquireNewTargetOfCurrentAction(enemy);
            }           
        }

        // Disable character's level node anims and targetting path
        VisualEventManager.Instance.CreateVisualEvent(() =>
        {
            CharacterEntityController.Instance.DisableAllDefenderTargetIndicators();
            view.character.levelNode.SetMouseOverViewState(false);
        });        

        // Fade out world space GUI
        VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.FadeOutCharacterWorldCanvas(view, null));

        // Play death animation
        VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.PlayDeathAnimation(view), QueuePosition.Back, 0f, 1f);

        // Fade out UCM
        CoroutineData fadeOutCharacter = new CoroutineData();
        VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.FadeOutCharacterModel(view, fadeOutCharacter));

        // Destroy characters activation window and update other window positions
        CharacterEntityModel currentlyActivatedEntity = ActivationManager.Instance.EntityActivated;
        VisualEventManager.Instance.CreateVisualEvent(() => ActivationManager.Instance.OnCharacterKilledVisualEvent(window, currentlyActivatedEntity, null), QueuePosition.Back, 0, 1f);

        // Destroy view and break references
        VisualEventManager.Instance.CreateVisualEvent(() =>
        {
            // Break references
            LevelManager.Instance.DisconnectEntityFromNode(entity);
            CharacterEntityController.Instance.DisconnectModelFromView(entity);

            // Destroy view gameobject
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

        // Check if the game over event should be triggered
        if (CharacterEntityController.Instance.AllDefenders.Count == 0)
        {
            StartGameOverDefeatProcess();
        }

        if (CharacterEntityController.Instance.AllEnemies.Count == 0 &&
            currentCombatState == CombatGameState.CombatActive)
        {
            StartGameOverVictoryProcess();
        }



        // do we destroy the characters model script as well as the view/gameobject?



        // Check volatile
        /*
        if (entity.myPassiveManager.Volatile)
        {
            // Notification
            VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Volatile");

            // Calculate which characters are hit by the aoe
            List<LivingEntity> targetsInRange = GetAllLivingEntitiesWithinAoeEffect(entity, entity.tile, 1, true, true);

            // Damage all targets hit
            foreach (LivingEntity targetInBlast in targetsInRange)
            {
                if (targetInBlast.inDeathProcess == false)
                {
                    int finalDamageValue = GetFinalDamageValueAfterAllCalculations(entity, targetInBlast, null, "Physical", false, entity.myPassiveManager.volatileStacks);
                    OldCoroutineData volatileExplosion = HandleDamage(finalDamageValue, null, targetInBlast, "Physical");
                    yield return new WaitUntil(() => volatileExplosion.ActionResolved() == true);
                }
            }

            yield return new WaitForSeconds(1);
        }

        // Check unstable
        if (entity.myPassiveManager.unstable)
        {
            // Notification
            VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Unstable");

            List<LivingEntity> targetsInRange = new List<LivingEntity>();
            foreach (LivingEntity entityy in LivingEntityManager.Instance.allLivingEntities)
            {
                if (!IsTargetFriendly(entity, entityy) && entityy.inDeathProcess == false)
                {
                    targetsInRange.Add(entityy);
                }
            }

            // Poison all targets hit
            foreach (LivingEntity targetInBlast in targetsInRange)
            {
                StatusController.Instance.ApplyStatusToLivingEntity(targetInBlast, StatusIconLibrary.Instance.GetStatusIconByName("Poisoned"), entity.myPassiveManager.unstableStacks);
            }

            yield return new WaitForSeconds(1);
        }
        */

        /*
        // Depending on the state of the combat, decide which ending or continuation occurs

        // check if the player has lost all characters and thus the game
        if (DefenderManager.Instance.allDefenders.Count == 0 &&
            EventManager.Instance.currentCombatEndEventTriggered == false)
        {
            Debug.Log("CombatLogic.HandleDeath() detected player has lost all defenders...");
            EventManager.Instance.currentCombatEndEventTriggered = true;
            EventManager.Instance.StartNewGameOverDefeatedEvent();
        }

        // check if this was the last enemy in the encounter
        else if (EnemyManager.Instance.allEnemies.Count == 0 &&
            DefenderManager.Instance.allDefenders.Count >= 1 &&
            EventManager.Instance.currentCombatEndEventTriggered == false)
        {
            Debug.Log("CombatLogic.HandleDeath() detected that all enemies have been killed...");

            // Trigger combat victory event depending on current encounter type            
            if (EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.EliteEnemy)
            {
                EventManager.Instance.currentCombatEndEventTriggered = true;
                EventManager.Instance.StartNewEndEliteEncounterEvent();
            }
            else if (EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.BasicEnemy)
            {
                EventManager.Instance.currentCombatEndEventTriggered = true;
                EventManager.Instance.StartNewEndBasicEncounterEvent();
            }
            else if (EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.Boss)
            {
                EventManager.Instance.currentCombatEndEventTriggered = true;
                EventManager.Instance.StartNewEndBossEncounterEvent();
            }


        }

        */

        // Destroy character GO
        //Debug.Log("Destroying " + entity.myName + " game object");
        // Destroy(entity.gameObject);

    }
    public void HandleDamage(int damageAmount, CharacterEntityModel attacker, CharacterEntityModel victim, DamageType damageType, Card card = null, EnemyActionEffect enemyEffect = null, bool ignoreBlock = false)
    {
        // Debug setup
        string cardNameString = "None";
        string attackerName = "No Attacker";
        string victimName = "No Victim";

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
            "), victim (" + victimName +"), damageType (" + damageType.ToString() + "), card (" + cardNameString + "), ignoreBlock (" + ignoreBlock.ToString()
            );

        // Cancel this if character is already in death process
        if (victim.livingState == LivingState.Dead)
        {
            Debug.Log("CombatLogic.HandleDamage() detected that " + victim.myName + " is already in death process, exiting damage event...");
            return;
        }

        // Establish properties for this damage event
        int totalLifeLost = 0;
        int adjustedDamageValue = damageAmount;
        int startingBlock = victim.block;
        int blockAfter = victim.block;
        int healthAfter = victim.health;

        // check for pierce
        /*
        if (
            attacker != null &&
            abilityUsed != null &&
            (abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack || abilityUsed.abilityType == AbilityDataSO.AbilityType.RangedAttack) &&
            attacker.myPassiveManager.pierce
            )
        {
            ignoreBlock = true;
        }
        */

        // play impact VFX        
        // TO DO: implement logic that makes enemies also consider if their attack is a melee attack
        if (card != null &&
            card.cardType == CardType.MeleeAttack)
        {
            Debug.Log("MELEE ATTACK VFX START");
            VisualEventManager.Instance.CreateVisualEvent(()=> VisualEffectManager.Instance.CreateSmallMeleeImpact(victim.characterEntityView.transform.position));
        }
        

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
        /*
        if (victim.myPassiveManager.barrier || victim.myPassiveManager.transcendence)
        {
            // Check for transcendence
            if (victim.myPassiveManager.transcendence)
            {
                //yield return new WaitForSeconds(0.5f);
                VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "Transcendence!");
                adjustedDamageValue = 0;
                healthAfter = victim.currentHealth;
            }

            // Check for barrier
            else if (victim.myPassiveManager.barrier && healthAfter < victim.currentHealth)
            {
                //yield return new WaitForSeconds(0.5f);
                VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "Barrier!");
                adjustedDamageValue = 0;
                healthAfter = victim.currentHealth;
                victim.myPassiveManager.ModifyBarrier(-1);
            }

        }
        */

        // Finished calculating the final damage, health lost and armor lost: p
        totalLifeLost = victim.health - healthAfter;
        CharacterEntityController.Instance.ModifyHealth(victim, -totalLifeLost);
        CharacterEntityController.Instance.SetBlock(victim, blockAfter);

        // Play VFX depending on whether the victim lost health, block, or was damaged by poison
        if (adjustedDamageValue > 0)
        {
            if (totalLifeLost == 0 && blockAfter < startingBlock)
            {
                // Create Lose Armor Effect
                VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateLoseBlockEffect(victim.characterEntityView.transform.position, adjustedDamageValue));
            }
            else if (totalLifeLost > 0)
            {  
                // Play hurt animation
                if (victim.health > 0 && totalLifeLost > 0)
                {
                    VisualEventManager.Instance.CreateVisualEvent(()=> CharacterEntityController.Instance.PlayHurtAnimation(victim.characterEntityView));
                }

                // Create Lose hp / damage effect
                VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateDamageEffect(victim.characterEntityView.transform.position, totalLifeLost), QueuePosition.Back, 0f, 0.5f);
            }
        }

        // Card 'on damaged' event
        if(totalLifeLost > 0 && victim.controller == Controller.Player)
        {
            CardController.Instance.HandleOnCharacterDamagedCardListeners(victim);
        }       

        // EVALUATE DAMAGE RELATED PASSIVE EFFECTS

        // Enrage
        if (victim.pManager.enrageStacks > 0 && totalLifeLost > 0)
        {
            Debug.Log(victim.myName + " 'Enrage' triggered, gaining " + victim.pManager.enrageStacks.ToString() + " bonus power");
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(victim.pManager, "Power", victim.pManager.enrageStacks, true, 0.5f);
        }

        // Poisonous 
        if (attacker != null &&
            attacker.pManager.poisonousStacks > 0)
        {
            if (card != null && 
               (card.cardType == CardType.MeleeAttack || card.cardType == CardType.RangedAttack))
            {
                PassiveController.Instance.ModifyPoisoned(attacker, victim.pManager, attacker.pManager.poisonousStacks, true, 0.5f);
            }
            else if (enemyEffect != null && 
               (enemyEffect.actionType == ActionType.AttackTarget || 
                enemyEffect.actionType == ActionType.AttackAll || 
                enemyEffect.actionType == ActionType.AttackTargetAndBuffSelf ||
                enemyEffect.actionType == ActionType.AttackTargetAndDefendSelf)
                )
            {
                PassiveController.Instance.ModifyPoisoned(attacker, victim.pManager, attacker.pManager.poisonousStacks, true, 0.5f);
            }
        }
        


        // Update character data if victim is a defender
        /*
        if (victim.defender != null && totalLifeLost > 0)
        {
            victim.defender.myCharacterData.ModifyCurrentHealth(-totalLifeLost);

            // flick bool for scoring
            EventManager.Instance.damageTakenThisEncounter = true;
        }
        */

        // PASSIVE STUFF
        /*
        // Remove camoflage from victim if damage was taken
        if (victim.myPassiveManager.camoflage)
        {
            //yield return new WaitForSeconds(0.5f);
            victim.myPassiveManager.ModifyCamoflage(-1);
        }

        // Life steal
        if (attacker != null &&
            attacker.myPassiveManager.lifeSteal && totalLifeLost > 0 &&
            abilityUsed != null &&
            abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            //yield return new WaitForSeconds(0.5f);
            Debug.Log(attacker.name + " has 'Life Steal', healing for " + totalLifeLost.ToString() + " damage");
            attacker.ModifyCurrentHealth(totalLifeLost);
        }

        // Poisonous trait
        if (attacker != null &&
            attacker.myPassiveManager.poisonous && totalLifeLost > 0 &&
            abilityUsed != null &&
            (abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack ||
            abilityUsed.abilityType == AbilityDataSO.AbilityType.RangedAttack))
        {
            Debug.Log(attacker.name + " has 'Poisonous', applying " + attacker.myPassiveManager.poisonousStacks.ToString() + " 'Poisoned'");
            //yield return new WaitForSeconds(0.5f);
            victim.myPassiveManager.ModifyPoisoned(attacker.myPassiveManager.poisonousStacks, attacker);
        }

        // Immolation trait
        if (attacker != null &&
            attacker.myPassiveManager.immolation && totalLifeLost > 0 &&
            abilityUsed != null &&
            abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            Debug.Log(attacker.name + " has 'Immolation', applying " + attacker.myPassiveManager.immolationStacks.ToString() + " 'Burning'");
            //yield return new WaitForSeconds(0.5f);
            victim.myPassiveManager.ModifyBurning(attacker.myPassiveManager.immolationStacks, attacker);
        }

        // Remove sleeping
        if (victim.myPassiveManager.sleep && totalLifeLost > 0)
        {
            Debug.Log(victim.name + " took damage and is sleeping, removing sleep");
            //yield return new WaitForSeconds(0.5f);
            victim.myPassiveManager.ModifySleep(-victim.myPassiveManager.sleepStacks);
        }

        // Enrage
        if (victim.myPassiveManager.enrage && totalLifeLost > 0)
        {
            Debug.Log(victim.name + " 'Enrage' triggered, gaining " + victim.myPassiveManager.enrage.ToString() + " bonus strength");
            //yield return new WaitForSeconds(0.5f);
            victim.myPassiveManager.ModifyBonusStrength(victim.myPassiveManager.enrageStacks);
        }

        // Tenacious
        if (victim.myPassiveManager.tenacious && totalLifeLost > 0)
        {
            Debug.Log(victim.name + " 'Tenacious' triggered, gaining" + (CalculateBlockGainedByEffect(victim.myPassiveManager.tenaciousStacks, victim).ToString() + " block"));
            //yield return new WaitForSeconds(0.5f);
            VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "Tenacious!");
            //yield return new WaitForSeconds(0.5f);
            victim.ModifyCurrentBlock(CalculateBlockGainedByEffect(victim.myPassiveManager.tenaciousStacks, victim));
        }

        // Thorns
        if (victim.myPassiveManager.thorns && attacker != null)
        {
            if (abilityUsed != null &&
                abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
            {
                //yield return new WaitForSeconds(0.5f);
                VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "Thorns");
                Debug.Log(victim.name + " has thorns and was struck by a melee attack, returning damage...");
                int finalThornsDamageValue = GetFinalDamageValueAfterAllCalculations(victim, attacker, null, "Physical", false, victim.myPassiveManager.thornsStacks);
                OldCoroutineData thornsDamage = HandleDamage(finalThornsDamageValue, victim, attacker, "Physical");
               // yield return new WaitUntil(() => thornsDamage.ActionResolved() == true);
            }
        }
        */

        // Increment times attack counter
        /*
        if (abilityUsed != null &&
           abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            victim.timesMeleeAttackedThisTurnCycle++;
        }
        */

        // Check if the victim was killed by the damage
        if (victim.health <= 0 && victim.livingState == LivingState.Alive)
        {
            Debug.Log(victim.myName + " has lost enough health to be killed by this damage event...");

            HandleDeath(victim);

            // the victim was killed, start death process
            //OldCoroutineData deathAction = HandleDeath(victim);
            //yield return new WaitUntil(() => deathAction.ActionResolved() == true);
            //action.coroutineCompleted = true;

            /*
            // Check for last stand passive
            if (victim.myPassiveManager.lastStand)
            {
                Debug.Log(victim.name + " has 'Last Stand' passive, preventing death...");

                // VFX Notification
                //yield return new WaitForSeconds(0.5f);
                VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "Last Stand!");
                //yield return new WaitForSeconds(0.5f);

                // Set victim at 1hp
                victim.ModifyCurrentHealth(1);

                // Remove last stand
                victim.myPassiveManager.ModifyLastStand(-victim.myPassiveManager.lastStandStacks);

                // Gain 5 strength
                victim.myPassiveManager.ModifyBonusStrength(5);
                //yield return new WaitForSeconds(0.5f);
            }

            // Check for Blessing of Undeath 
            else if (victim.defender && StateManager.Instance.DoesPlayerAlreadyHaveState("Blessing Of Undeath"))
            {
                Debug.Log(victim.name + " is protected by 'Blessing Of Undeath' state, preventing death...");

                // VFX Notification
                //yield return new WaitForSeconds(0.5f);
                VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "Blessing Of Undeath!");
                //yield return new WaitForSeconds(0.5f);

                // Set victim at 50% HP
                victim.ModifyCurrentHealth(victim.currentMaxHealth / 2);

                // Reduce blessing of undeath counter, check for removal
                State blessingOfUndeathState = StateManager.Instance.GetActiveStateByName("Blessing Of Undeath");

                blessingOfUndeathState.ModifyCountdown(-1);
                //yield return new WaitForSeconds(0.5f);

            }

            else
            {
                Debug.Log(victim.name + " has no means to prevent death, starting death process...");

                // check for coup de grace passive on attacker
                if (attacker != null &&
                    attacker.myPassiveManager.coupDeGrace)
                {
                    Debug.Log(attacker.myName + " killed " + victim.myName +
                        " and has 'Coup De Grace' passive, gaining max energy...");

                    VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "Coup De Grace!");
                    attacker.ModifyCurrentEnergy(attacker.currentMaxEnergy);
                }

                // check for gnollish blood lust passive on attacker
                else if (attacker != null &&
                   attacker.myPassiveManager.gnollishBloodLust)
                {
                    Debug.Log(attacker.myName + " killed " + victim.myName +
                        " and has 'Gnollish Blood Lust' passive, gaining 40 energy...");

                    VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "Gnollish Blood Lust!");
                    attacker.ModifyCurrentEnergy(40);
                }

                // the victim was killed, start death process
                OldCoroutineData deathAction = HandleDeath(victim);
                //yield return new WaitUntil(() => deathAction.ActionResolved() == true);
                //action.coroutineCompleted = true;
            }
            */

        }

        //yield return new WaitForSeconds(0.5f);
        //action.coroutineCompleted = true;
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
    private void StartGameOverDefeatProcess()
    {
        Debug.Log("CombatLogic.StartGameOverDefeatProcess() called...");
        currentCombatState = CombatGameState.DefeatTriggered;
        UIManager.Instance.defeatPopup.SetActive(true);
    }
    private void StartGameOverVictoryProcess()
    {
        Debug.Log("CombatLogic.StartGameOverVictoryProcess() called...");
        currentCombatState = CombatGameState.VictoryTriggered;
        UIManager.Instance.victoryPopup.SetActive(true);
    }
    #endregion
}
