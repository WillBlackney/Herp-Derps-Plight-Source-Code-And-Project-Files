using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEntityController: Singleton<CharacterEntityController>
{
    [Header("Character Entity Lists")]
    [HideInInspector] public List<CharacterEntityModel> allCharacters = new List<CharacterEntityModel>();
    [HideInInspector] public List<CharacterEntityModel> allDefenders = new List<CharacterEntityModel>();
    [HideInInspector] public List<CharacterEntityModel> allEnemies = new List<CharacterEntityModel>();

    [Header("UCM Colours ")]
    public Color normalColour;
    public Color highlightColour;

    // Create Characters Logic + Setup
    #region    
    private GameObject CreateCharacterEntityView()
    {
        return Instantiate(PrefabHolder.Instance.characterEntityModel, transform.position, Quaternion.identity);
    }
    private void SetCharacterViewStartingState(CharacterEntityModel character)
    {

    }
    public CharacterEntityModel CreatePlayerCharacter(CharacterData data, LevelNode position)
    {
        // Create GO + View
        CharacterEntityView vm = CreateCharacterEntityView().GetComponent<CharacterEntityView>();

        // Create data object
        CharacterEntityModel model = new CharacterEntityModel();

        // Connect model to view
        model.characterEntityView = vm;

        // Set up positioning in world
        LevelManager.Instance.PlaceEntityAtNode(model, position);

        // Set type + allegiance
        model.controller = Controller.Player;
        model.allegiance = Allegiance.Player;

        // Set up view
        SetCharacterViewStartingState(model);

        // Copy data from character data into new model
        SetupCharacterFromCharacterData(model, data);

        // Add to persistency
        allCharacters.Add(model);
        allDefenders.Add(model);

        return model;
    }
    public CharacterEntityModel CreateEnemyCharacter(EnemyDataSO data, LevelNode position)
    {
        // Create GO + View
        CharacterEntityView vm = CreateCharacterEntityView().GetComponent<CharacterEntityView>();

        // Create data object
        CharacterEntityModel model = new CharacterEntityModel();

        // Connect model to view
        model.characterEntityView = vm;

        // Set up positioning in world
        LevelManager.Instance.PlaceEntityAtNode(model, position);

        // Set type + allegiance
        model.controller = Controller.AI;
        model.allegiance = Allegiance.Enemy;

        // Set up view
        SetCharacterViewStartingState(model);

        // Copy data from character data into new model
        SetupCharacterFromEnemyData(model, data);

        // Add to persistency
        allCharacters.Add(model);
        allEnemies.Add(model);

        return model;
    }
    private void SetupCharacterFromCharacterData(CharacterEntityModel character, CharacterData data)
    {
        // Establish connection from defender script to character data
        //myCharacterData.myDefenderGO = this;

        // Setup Core Stats
        ModifyStamina(character, data.stamina);
        ModifyInitiative(character, data.initiative);
        ModifyDraw(character, data.draw);
        ModifyDexterity(character, data.dexterity);
        ModifyPower(character, data.power);

        // Set up health
        ModifyMaxHealth(character, data.maxHealth);
        ModifyHealth(character, data.health);

        // Build UCM
        // TO DO IN FUTURE: Build from actual character data, not sample test scene data
        CharacterModelController.BuildModelFromModelClone(character.characterEntityView.ucm, CombatTestSceneController.Instance.sampleUCM);

        // Build activation window
        ActivationManager.Instance.CreateActivationWindow(character);

        /*
    baseMaxHealth = myCharacterData.maxHealth;
    baseStartingHealth = myCharacterData.currentHealth;
    baseStrength = myCharacterData.strength;
    baseWisdom = myCharacterData.wisdom;
    baseDexterity = myCharacterData.dexterity;
    baseMobility = myCharacterData.mobility;
    baseStamina = myCharacterData.stamina;
    baseInitiative = myCharacterData.initiative;

    // Setup Resistances
    /*
    basePhysicalResistance = myCharacterData.physicalResistance;
    baseFireResistance = myCharacterData.fireResistance;
    baseFrostResistance = myCharacterData.frostResistance;
    basePoisonResistance = myCharacterData.poisonResistance;
    baseShadowResistance = myCharacterData.shadowResistance;
    baseAirResistance = myCharacterData.airResistance;
    */

        // Setup Passives
        /*
        if (myCharacterData.tenaciousStacks > 0)
        {
            myPassiveManager.ModifyTenacious(myCharacterData.tenaciousStacks);
        }
        if (myCharacterData.enrageStacks > 0)
        {
            myPassiveManager.ModifyEnrage(myCharacterData.enrageStacks);
        }
        if (myCharacterData.masochistStacks > 0)
        {
            myPassiveManager.ModifyMasochist(myCharacterData.masochistStacks);
        }
        if (myCharacterData.lastStandStacks > 0)
        {
            myPassiveManager.ModifyLastStand(myCharacterData.lastStandStacks);
        }
        if (myCharacterData.unstoppableStacks > 0)
        {
            myPassiveManager.ModifyUnstoppable(1);
        }
        if (myCharacterData.slipperyStacks > 0)
        {
            myPassiveManager.ModifySlippery(myCharacterData.slipperyStacks);
        }
        if (myCharacterData.riposteStacks > 0)
        {
            myPassiveManager.ModifyRiposte(myCharacterData.riposteStacks);
        }
        if (myCharacterData.virtuosoStacks > 0)
        {
            myPassiveManager.ModifyVirtuoso(myCharacterData.virtuosoStacks);
        }
        if (myCharacterData.perfectReflexesStacks > 0)
        {
            myPassiveManager.ModifyPerfectReflexes(myCharacterData.perfectReflexesStacks);
        }
        if (myCharacterData.opportunistStacks > 0)
        {
            myPassiveManager.ModifyOpportunist(myCharacterData.opportunistStacks);
        }
        if (myCharacterData.patientStalkerStacks > 0)
        {
            myPassiveManager.ModifyPatientStalker(myCharacterData.patientStalkerStacks);
        }
        if (myCharacterData.stealthStacks > 0)
        {
            myPassiveManager.ModifyStealth(myCharacterData.stealthStacks);
        }
        if (myCharacterData.cautiousStacks > 0)
        {
            myPassiveManager.ModifyCautious(myCharacterData.cautiousStacks);
        }
        if (myCharacterData.guardianAuraStacks > 0)
        {
            myPassiveManager.ModifyGuardianAura(myCharacterData.guardianAuraStacks);
        }
        if (myCharacterData.unwaveringStacks > 0)
        {
            myPassiveManager.ModifyUnwavering(myCharacterData.unwaveringStacks);
        }
        if (myCharacterData.fieryAuraStacks > 0)
        {
            myPassiveManager.ModifyFieryAura(myCharacterData.fieryAuraStacks);
        }
        if (myCharacterData.immolationStacks > 0)
        {
            myPassiveManager.ModifyImmolation(myCharacterData.immolationStacks);
        }
        if (myCharacterData.demonStacks > 0)
        {
            myPassiveManager.ModifyDemon(myCharacterData.demonStacks);
        }
        if (myCharacterData.shatterStacks > 0)
        {
            myPassiveManager.ModifyShatter(myCharacterData.shatterStacks);
        }
        if (myCharacterData.frozenHeartStacks > 0)
        {
            myPassiveManager.ModifyFrozenHeart(myCharacterData.frozenHeartStacks);
        }
        if (myCharacterData.predatorStacks > 0)
        {
            myPassiveManager.ModifyPredator(myCharacterData.predatorStacks);
        }
        if (myCharacterData.hawkEyeStacks > 0)
        {
            myPassiveManager.ModifyHawkEye(myCharacterData.hawkEyeStacks);
        }
        if (myCharacterData.thornsStacks > 0)
        {
            myPassiveManager.ModifyThorns(myCharacterData.thornsStacks);
        }
        if (myCharacterData.trueSightStacks > 0)
        {
            myPassiveManager.ModifyTrueSight(1);
        }
        if (myCharacterData.fluxStacks > 0)
        {
            myPassiveManager.ModifyFlux(myCharacterData.fluxStacks);
        }
        if (myCharacterData.quickDrawStacks > 0)
        {
            myPassiveManager.ModifyQuickDraw(myCharacterData.quickDrawStacks);
        }
        if (myCharacterData.phasingStacks > 0)
        {
            myPassiveManager.ModifyPhasing(myCharacterData.phasingStacks);
        }
        if (myCharacterData.etherealBeingStacks > 0)
        {
            myPassiveManager.ModifyEtherealBeing(myCharacterData.etherealBeingStacks);
        }
        if (myCharacterData.encouragingAuraStacks > 0)
        {
            myPassiveManager.ModifyEncouragingAura(myCharacterData.encouragingAuraStacks);
        }
        if (myCharacterData.radianceStacks > 0)
        {
            myPassiveManager.ModifyRadiance(myCharacterData.radianceStacks);
        }
        if (myCharacterData.sacredAuraStacks > 0)
        {
            myPassiveManager.ModifySacredAura(myCharacterData.sacredAuraStacks);
        }
        if (myCharacterData.shadowAuraStacks > 0)
        {
            myPassiveManager.ModifyShadowAura(myCharacterData.shadowAuraStacks);
        }
        if (myCharacterData.shadowFormStacks > 0)
        {
            myPassiveManager.ModifyShadowForm(myCharacterData.shadowFormStacks);
        }
        if (myCharacterData.poisonousStacks > 0)
        {
            myPassiveManager.ModifyPoisonous(myCharacterData.poisonousStacks);
        }
        if (myCharacterData.venomousStacks > 0)
        {
            myPassiveManager.ModifyVenomous(myCharacterData.venomousStacks);
        }
        if (myCharacterData.toxicityStacks > 0)
        {
            myPassiveManager.ModifyToxicity(myCharacterData.toxicityStacks);
        }
        if (myCharacterData.toxicAuraStacks > 0)
        {
            myPassiveManager.ModifyToxicAura(myCharacterData.toxicAuraStacks);
        }
        if (myCharacterData.stormAuraStacks > 0)
        {
            myPassiveManager.ModifyStormAura(myCharacterData.stormAuraStacks);
        }
        if (myCharacterData.stormLordStacks > 0)
        {
            myPassiveManager.ModifyStormLord(myCharacterData.stormLordStacks);
        }
        if (myCharacterData.fadingStacks > 0)
        {
            myPassiveManager.ModifyFading(myCharacterData.fadingStacks);
        }
        if (myCharacterData.lifeStealStacks > 0)
        {
            myPassiveManager.ModifyLifeSteal(myCharacterData.lifeStealStacks);
        }
        if (myCharacterData.growingStacks > 0)
        {
            myPassiveManager.ModifyGrowing(myCharacterData.growingStacks);
        }
        if (myCharacterData.fastLearnerStacks > 0)
        {
            myPassiveManager.ModifyFastLearner(myCharacterData.fastLearnerStacks);
        }
        if (myCharacterData.pierceStacks > 0)
        {
            myPassiveManager.ModifyPierce(myCharacterData.pierceStacks);
        }

        // Racial traits
        if (myCharacterData.forestWisdomStacks > 0)
        {
            myPassiveManager.ModifyForestWisdom(myCharacterData.forestWisdomStacks);
        }
        if (myCharacterData.satyrTrickeryStacks > 0)
        {
            myPassiveManager.ModifySatyrTrickery(myCharacterData.satyrTrickeryStacks);
        }
        if (myCharacterData.humanAmbitionStacks > 0)
        {
            myPassiveManager.ModifyHumanAmbition(myCharacterData.humanAmbitionStacks);
        }
        if (myCharacterData.orcishGritStacks > 0)
        {
            myPassiveManager.ModifyOrcishGrit(myCharacterData.orcishGritStacks);
        }
        if (myCharacterData.gnollishBloodLustStacks > 0)
        {
            myPassiveManager.ModifyGnollishBloodLust(myCharacterData.gnollishBloodLustStacks);
        }
        if (myCharacterData.freeFromFleshStacks > 0)
        {
            myPassiveManager.ModifyFreeFromFlesh(myCharacterData.freeFromFleshStacks);
        }


        // Set Weapons from character data
        ItemManager.Instance.SetUpDefenderWeaponsFromCharacterData(this);
        */
    }
    private void SetupCharacterFromEnemyData(CharacterEntityModel character, EnemyDataSO data)
    {
        // Set general info
        character.myName = data.enemyName;       

        // Setup Core Stats
        ModifyInitiative(character, data.initiative);
        ModifyDexterity(character, data.dexterity);
        ModifyPower(character, data.power);

        // Set up health + Block
        ModifyMaxHealth(character, data.maxHealth);
        ModifyHealth(character, data.startingHealth);
        ModifyBlock(character, data.startingBlock);

        // Build UCM
        CharacterModelController.BuildModelFromStringReferences(character.characterEntityView.ucm, data.allBodyParts);

        // Build activation window
        ActivationManager.Instance.CreateActivationWindow(character);

        // Set up passive trais
        /*
        foreach (StatusPairing sp in data.allPassives)
        {
            StatusController.Instance.ApplyStatusToLivingEntity(enemy, sp.statusData, sp.statusStacks);
        }
        */
    }
    #endregion

    // Modify Health
    #region
    public void ModifyHealth(CharacterEntityModel character, int healthGainedOrLost)
    {
        Debug.Log("CharacterEntityController.ModifyHealth() called for " + character.myName);

        int originalHealth = character.health;
        character.health += healthGainedOrLost;

        // prevent health increasing over maximum
        if (character.health > character.maxHealth)
        {
            character.health = character.maxHealth;
        }

        // prevent health going less then 0
        if (character.health < 0)
        {
            character.health = 0;
        }

        if (character.health > originalHealth)
        {
           // StartCoroutine(VisualEffectManager.Instance.CreateHealEffect(character.characterEntityView.transform.position, healthGainedOrLost));
        }

        VisualEventManager.Instance.CreateVisualEvent(()=> UpdateHealthGUIElements(character, character.health, character.maxHealth),QueuePosition.Back, 0, 0);
    }
    public void ModifyMaxHealth(CharacterEntityModel character, int maxHealthGainedOrLost)
    {
        Debug.Log("CharacterEntityController.ModifyMaxHealth() called for " + character.myName);

        character.maxHealth += maxHealthGainedOrLost;
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateHealthGUIElements(character, character.health, character.maxHealth), QueuePosition.Back, 0, 0);
    }
    private void UpdateHealthGUIElements(CharacterEntityModel character, int health, int maxHealth)
    {
        Debug.Log("CharacterEntityController.UpdateHealthGUIElements() called, health = " + health.ToString() + ", maxHealth = " + maxHealth.ToString());

        // Convert health int values to floats
        float currentHealthFloat = health;
        float currentMaxHealthFloat = maxHealth;
        float healthBarFloat = currentHealthFloat / currentMaxHealthFloat;

        // Modify health bar slider + health texts
        character.characterEntityView.healthBar.value = healthBarFloat;
        character.characterEntityView.healthText.text = health.ToString();
        character.characterEntityView.maxHealthText.text = maxHealth.ToString();

        //myActivationWindow.myHealthBar.value = finalValue;

    }
    #endregion

    // Modify Core Stats
    #region
    public void ModifyInitiative(CharacterEntityModel character, int initiativeGainedOrLost)
    {
        character.initiative += initiativeGainedOrLost;

        if (character.initiative < 0)
        {
            character.initiative = 0;
        }
    }
    public void ModifyDraw(CharacterEntityModel character, int drawGainedOrLost)
    {
        character.draw += drawGainedOrLost;

        if (character.draw < 0)
        {
            character.draw = 0;
        }
    }
    public void ModifyPower(CharacterEntityModel character, int powerGainedOrLost)
    {
        character.power += powerGainedOrLost;
    }
    public void ModifyDexterity(CharacterEntityModel character, int dexterityGainedOrLost)
    {
        character.dexterity += dexterityGainedOrLost;
    }
    #endregion

    // Modify Energy
    #region
    public void ModifyEnergy(CharacterEntityModel character, int energyGainedOrLost)
    {
        character.energy += energyGainedOrLost;

        if (character.energy < 0)
        {
            character.energy = 0;
        }

        VisualEventManager.Instance.CreateVisualEvent(() => UpdateEnergyGUI(character, character.energy), QueuePosition.Back, 0, 0);
    }
    public void ModifyStamina(CharacterEntityModel character, int staminaGainedOrLost)
    {
        character.energy += staminaGainedOrLost;

        if (character.energy < 0)
        {
            character.energy = 0;
        }

        VisualEventManager.Instance.CreateVisualEvent(() => UpdateStaminaGUI(character, character.energy), QueuePosition.Back, 0, 0);
    }
    private void UpdateEnergyGUI(CharacterEntityModel character, int newValue)
    {
        character.characterEntityView.energyText.text = newValue.ToString();
    }
    private void UpdateStaminaGUI(CharacterEntityModel character, int newValue)
    {
        character.characterEntityView.staminaText.text = newValue.ToString();
    }
    #endregion

    // Modify Block
    #region
    public void ModifyBlock(CharacterEntityModel character, int blockGainedOrLost)
    {
        Debug.Log("LivingEntity.ModifyCurrentBlock() called for " + character.myName);

        int finalBlockGainValue = blockGainedOrLost;

        character.block += blockGainedOrLost;

        if (blockGainedOrLost > 0)
        {
            //StartCoroutine(VisualEffectManager.Instance.CreateGainBlockEffect(transform.position, blockGainedOrLost));
        }

        VisualEventManager.Instance.CreateVisualEvent(() => UpdateBlockGUI(character, character.block), QueuePosition.Back, 0, 0);
    }
    public void ModifyBlockOnActivationStart(CharacterEntityModel character)
    {
        Debug.Log("LivingEntity.ModifyBlockOnActivationStart() called for " + character.myName);

        // Remove all block
        ModifyBlock(character, -character.block);

        /*
        if (myPassiveManager.unwavering)
        {
            Debug.Log(myName + " has 'Unwavering' passive, not removing block");
            return;
        }
        else if (defender && StateManager.Instance.DoesPlayerAlreadyHaveState("Polished Armour"))
        {
            Debug.Log(myName + " has 'Polished Armour' state buff, not removing block");
            return;
        }
        else
        {
            // Remove all block
            ModifyCurrentBlock(-currentBlock);
        }
        */

    }
    public void UpdateBlockGUI(CharacterEntityModel character, int newBlockValue)
    {
        character.characterEntityView.blockText.text = newBlockValue.ToString();
        if(newBlockValue > 0)
        {
            character.characterEntityView.blockIcon.SetActive(true);
        }
        else
        {
            character.characterEntityView.blockIcon.SetActive(false);
        }
    }
    #endregion

    // Activation Related
    #region    
    public void CharacterOnNewTurnCycleStarted(CharacterEntityModel character)
    {
        Debug.Log("CharacterOnNewTurnCycleStartedCoroutine() called for " + character.myName);

        character.hasActivatedThisTurn = false;

        /*
        // Remove Temporary Parry 
        if (myPassiveManager.temporaryBonusParry)
        {
            Debug.Log("OnNewTurnCycleStartedCoroutine() removing Temporary Bonus Parry...");
            myPassiveManager.ModifyTemporaryParry(-myPassiveManager.temporaryBonusParryStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Bonus Dodge
        if (myPassiveManager.temporaryBonusDodge)
        {
            Debug.Log("OnNewTurnCycleStartedCoroutine() removing Temporary Bonus Dodge...");
            myPassiveManager.ModifyTemporaryDodge(-myPassiveManager.temporaryBonusDodgeStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Remove Transcendence
        if (myPassiveManager.transcendence)
        {
            Debug.Log("OnNewTurnCycleStartedCoroutine() removing Transcendence...");
            myPassiveManager.ModifyTranscendence(-myPassiveManager.transcendenceStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Remove Marked
        if (myPassiveManager.marked)
        {
            Debug.Log("OnNewTurnCycleStartedCoroutine() checking Marked...");
            myPassiveManager.ModifyMarked(-myPassiveManager.terrifiedStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // gain camo from satyr trickery
        if (TurnChangeNotifier.Instance.currentTurnCount == 1 && myPassiveManager.satyrTrickery)
        {
            VisualEffectManager.Instance.
                CreateStatusEffect(transform.position, "Satyr Trickery!");
            yield return new WaitForSeconds(0.5f);

            myPassiveManager.ModifyCamoflage(1);
            yield return new WaitForSeconds(0.5f);
        }

        // gain max Energy from human ambition
        if (TurnChangeNotifier.Instance.currentTurnCount == 1 && myPassiveManager.humanAmbition)
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Human Ambition");
            VisualEffectManager.Instance.CreateGainEnergyBuffEffect(transform.position);
            ModifyCurrentEnergy(currentMaxEnergy);
            yield return new WaitForSeconds(0.5f);
        }
        */
    }
    public void CharacterOnActivationStart(CharacterEntityModel character)
    {
        // Debug.Log("LivingEntity.CharacterOnActivationStart() called...");

        //moveActionsTakenThisActivation = 0;
        //meleeAbilityActionsTakenThisActivation = 0;
        //skillAbilityActionsTakenThisActivation = 0;
        //rangedAttackAbilityActionsTakenThisActivation = 0;

        // GainEnergyOnActivationStart();
        ModifyEnergy(character, EntityLogic.GetTotalStamina(character));
        ModifyBlockOnActivationStart(character);

        // enable activated view state
        character.levelNode.SetActivatedViewState(true);

        // Draw cards on turn start
        if (character.controller == Controller.Player)
        {
            //CardController.Instance.DrawCardsOnActivationStart(character);
        }

        character.hasActivatedThisTurn = true;

        /*
        // check if taunted, and if taunter died 
        if (myPassiveManager.taunted && myTaunter == null)
        {
            myPassiveManager.ModifyTaunted(-myPassiveManager.tauntedStacks, null);
        }

        // Remove time warp
        if (myPassiveManager.timeWarp && hasActivatedThisTurn)
        {
            myPassiveManager.ModifyTimeWarp(-myPassiveManager.timeWarpStacks);
        }

        // Cautious
        if (myPassiveManager.cautious)
        {
            Debug.Log("OnActivationEndCoroutine() checking Cautious...");
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Cautious");
            ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(myPassiveManager.cautiousStacks, this));
            //yield return new WaitForSeconds(1f);
        }
        
        // Growing
        if (myPassiveManager.growing)
        {
            myPassiveManager.ModifyBonusStrength(myPassiveManager.growingStacks);
            //yield return new WaitForSeconds(1);
        }

        // Fast Learner
        if (myPassiveManager.fastLearner)
        {
            myPassiveManager.ModifyBonusWisdom(myPassiveManager.fastLearnerStacks);
            //yield return new WaitForSeconds(1);
        }
        */

        //action.coroutineCompleted = true;
    }
    #endregion
}
