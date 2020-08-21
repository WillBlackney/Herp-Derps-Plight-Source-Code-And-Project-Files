using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEntityController: Singleton<CharacterEntityController>
{
    [Header("Character Entity Lists")]
    [HideInInspector] public List<CharacterEntityModel> allCharacters;
    [HideInInspector] public List<CharacterEntityModel> allDefenders;
    [HideInInspector] public List<CharacterEntityModel> allEnemies;

    [Header("UCM Colours ")]
    public Color normalColour;
    public Color highlightColour;

    public int nameCounter = 1;

    // Create Characters Logic + Setup
    #region    
    private GameObject CreateCharacterEntityView()
    {
        return Instantiate(PrefabHolder.Instance.characterEntityModel, transform.position, Quaternion.identity);
    }
    private void BuildCharacterViewFromModel(CharacterEntityModel character)
    {

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

        // Set up positioning
        LevelManager.Instance.PlaceEntityAtNode(model, position);

        // Set up view
        SetCharacterViewStartingState(model);

        // Copy data from character data into new model
        SetupCharacterFromCharacterData(model, data);

        return model;
    }
    private void SetupCharacterFromCharacterData(CharacterEntityModel character, CharacterData data)
    {
        // Establish connection from defender script to character data
        //myCharacterData.myDefenderGO = this;

        // Setup Core Stats
        character.myName = ("Character " + nameCounter.ToString());
        nameCounter++;

        ModifyMaxHealth(character, data.maxHealth);
        ModifyHealth(character, data.health);

       


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



}
