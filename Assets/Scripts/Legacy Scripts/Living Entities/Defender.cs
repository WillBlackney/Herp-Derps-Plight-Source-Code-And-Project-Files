using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Defender : LivingEntity
{
    // Properties + Component References
    #region
    [Header("Defender Card Data")]
    public List<CardDataSO> deckData;
    public List<Card> drawPile = new List<Card>();
    public List<Card> hand = new List<Card>();
    public List<Card> discardPile = new List<Card>();

    [Header("Defender Card GUI References")]
    public HandVisual handVisual;

    [Header("Defender Core GUI Component References ")]
    public GameObject myUIParent;
    public CanvasGroup myUIParentCg;
    public AbilityBar myAbilityBar;
    public TargetIndicator myTargetIndicator;

    [Header("Defender Health Bar Component References ")]
    public Slider myHealthBarStatPanel;
    public TextMeshProUGUI myCurrentHealthTextStatBar;
    public TextMeshProUGUI myCurrentMaxHealthTextStatBar;
    
    [Header("Defender Energy Bar Component References ")]
    public Slider myEnergyBar;
    public TextMeshProUGUI myCurrentEnergy;
    public TextMeshProUGUI myCurrentStamina;

    [Header("Defender Properties")]
    public float uiFadeSpeed;
    [HideInInspector] public OldCharacterData myCharacterData;
    [HideInInspector] public bool fadingIn;
    [HideInInspector] public bool fadingOut;

    [Header("Update Related Properties")]
    [HideInInspector] public bool energyBarPositionCurrentlyUpdating;
    [HideInInspector] public bool healthBarPositionCurrentlyUpdating;

    #endregion

    // Initialization + Setup
    #region
    public override void InitializeSetup(Point startingGridPosition, Tile startingTile)
    {
        // Add to persistent defender data list
        DefenderManager.Instance.allDefenders.Add(this);

        // Build Deck
        //CardController.Instance.BuildDefenderDeckFromDeckData(this, deckData);

        // Set name
        myName = myCharacterData.myName;

        // Auto Get+Set World camera for UI canvas (helps with performance)
        //myUIParent.GetComponent<Canvas>().worldCamera = CameraManager.Instance.cameraMovement.mainCamera;

        // Hide UI
        myUIParent.SetActive(false);

        // Set parent to keep hierachy view clean
        transform.SetParent(DefenderManager.Instance.defendersParent.transform);

        // Perform standard LE setup
        base.InitializeSetup(startingGridPosition, startingTile);
    }
    public override void InitializeSetup(LevelNode position)
    {
        // Add to persistent defender data list
        DefenderManager.Instance.allDefenders.Add(this);

        // Build Deck
        //CardController.Instance.BuildDefenderDeckFromDeckData(this, deckData);

        // Set name
        myName = myCharacterData.myName;

        // Auto Get+Set World camera for UI canvas (helps with performance)
        //myUIParent.GetComponent<Canvas>().worldCamera = CameraManager.Instance.cameraMovement.mainCamera;

        // Hide UI
        myUIParent.SetActive(false);

        // Set parent to keep hierachy view clean
        transform.SetParent(DefenderManager.Instance.defendersParent.transform);

        // Perform standard LE setup
        base.InitializeSetup(position);
    }
    public override void SetBaseProperties()
    {
        // Build view model
        CharacterModelController.BuildModelFromModelClone(myModel, myCharacterData.myCharacterModel);

        // Get and build from relevant character data values
        RunSetupFromCharacterData();

        // Perform standard LE setup
        base.SetBaseProperties();

        // Set up ability button descriptions
        mySpellBook.SetNewAbilityDescriptions();
        
    }
    public void RunSetupFromCharacterData()
    {
        Debug.Log("RunSetupFromCharacterData() called...");

        // Establish connection from defender script to character data
        myCharacterData.myDefenderGO = this;

        // Setup Core Stats
        baseMaxHealth = myCharacterData.maxHealth;
        baseStartingHealth = myCharacterData.currentHealth;
        baseStrength = myCharacterData.strength;
        baseWisdom = myCharacterData.wisdom;
        baseDexterity = myCharacterData.dexterity;
        baseMobility = myCharacterData.mobility;
        baseStamina = myCharacterData.stamina;
        baseInitiative = myCharacterData.initiative;

        // Setup Secondary Stats
        baseCriticalChance = myCharacterData.criticalChance;
        baseDodgeChance = myCharacterData.dodge;
        baseParryChance = myCharacterData.parry;
        baseAuraSize = myCharacterData.auraSize;
        baseMaxEnergy = myCharacterData.maxEnergy;
        baseMeleeRange = myCharacterData.meleeRange;

        // Setup Resistances
        basePhysicalResistance = myCharacterData.physicalResistance;
        baseFireResistance = myCharacterData.fireResistance;
        baseFrostResistance = myCharacterData.frostResistance;
        basePoisonResistance = myCharacterData.poisonResistance;
        baseShadowResistance = myCharacterData.shadowResistance;
        baseAirResistance = myCharacterData.airResistance;

        // Setup Passives
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

    }

    #endregion

    // Mouse + Click Events
    #region
    public void OnMouseDown()
    {
        Defender selectedDefender = DefenderManager.Instance.selectedDefender;

        // this statment prevents the user from clicking through UI elements and selecting a defender
        if (!EventSystem.current.IsPointerOverGameObject() == false)
        {
            Debug.Log("Clicked on the UI, returning...");
            return;
        }

        // Check for consumable orders first
        if (ConsumableManager.Instance.awaitingAdrenalinePotionTarget ||
            ConsumableManager.Instance.awaitingBottledBrillianceTarget ||
            ConsumableManager.Instance.awaitingBottledMadnessTarget ||
            ConsumableManager.Instance.awaitingPotionOfMightTarget ||
            ConsumableManager.Instance.awaitingPotionOfClarityTarget ||
            ConsumableManager.Instance.awaitingVanishPotionTarget)
        {
            ConsumableManager.Instance.ApplyConsumableToTarget(this);
        }

        else if (ConsumableManager.Instance.awaitingFireBombTarget ||
            ConsumableManager.Instance.awaitingDynamiteTarget ||
            ConsumableManager.Instance.awaitingPoisonGrenadeTarget ||
            ConsumableManager.Instance.awaitingBottledFrostTarget)
        {
            ConsumableManager.Instance.ApplyConsumableToTarget(tile);
        }

        else if (ConsumableManager.Instance.awaitingBlinkPotionCharacterTarget)
        {
            ConsumableManager.Instance.StartBlinkPotionLocationSettingProcess(this);
        }
    }
    public void SelectDefender()
    {
        /*
        DefenderManager.Instance.SetSelectedDefender(this);
        myUIParent.SetActive(true);
        StartCoroutine(FadeInUICanvas());
        */
    }
    public void UnselectDefender()
    {
       // ClearAllOrders();
        StartCoroutine(FadeOutUICanvas());
    }

    #endregion

   

    // Text + UI Component Updates
    #region  
   
    public float CalculateEnergyBarPosition()
    {
        float currentAPFloat = currentEnergy;
        float currentMaxAPFloat = currentMaxEnergy;

        return currentAPFloat / currentMaxAPFloat;
    }
    public void UpdateEnergyGUI()
    {
        //float finalValue = CalculateEnergyBarPosition();
        myCurrentEnergy.text = currentEnergy.ToString();
        myCurrentStamina.text = EntityLogic.GetTotalStamina(this).ToString();
        energyBarPositionCurrentlyUpdating = false;
       // StartCoroutine(UpdateEnergyBarPositionCoroutine(finalValue));

    }    
    public IEnumerator UpdateHealthBarPanelPosition(float finalValue)
    {
        float needleMoveSpeed = 0.02f;
        healthBarPositionCurrentlyUpdating = true;

        while (myHealthBarStatPanel.value != finalValue && healthBarPositionCurrentlyUpdating == true)
        {
            if (myHealthBarStatPanel.value > finalValue)
            {
                myHealthBarStatPanel.value -= needleMoveSpeed;
                if (myHealthBarStatPanel.value < finalValue)
                {
                    myHealthBarStatPanel.value = finalValue;
                }
            }
            else if (myHealthBarStatPanel.value < finalValue)
            {
                myHealthBarStatPanel.value += needleMoveSpeed;
                if (myHealthBarStatPanel.value > finalValue)
                {
                    myHealthBarStatPanel.value = finalValue;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator UpdateEnergyBarPositionCoroutine(float finalValue)
    {
        float needleMoveSpeed = 0.04f;
        energyBarPositionCurrentlyUpdating = true;

        while (myEnergyBar.value != finalValue && energyBarPositionCurrentlyUpdating == true)
        {
            if(myEnergyBar.value > finalValue)
            {
                myEnergyBar.value -= needleMoveSpeed;
                if(myEnergyBar.value < finalValue)
                {
                    myEnergyBar.value = finalValue;
                }
            }
            else if (myEnergyBar.value < finalValue)
            {
                myEnergyBar.value += needleMoveSpeed;
                if (myEnergyBar.value > finalValue)
                {
                    myEnergyBar.value = finalValue;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }   

    public IEnumerator FadeInUICanvas()
    {
        myUIParentCg.alpha = 0;

        fadingOut = false;
        fadingIn = true;

        while (fadingIn && myUIParentCg.alpha < 1)
        {
            myUIParentCg.alpha += 0.1f * uiFadeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();            
        }
    }
    public IEnumerator FadeOutUICanvas()
    {
        myUIParentCg.alpha = 1;

        fadingIn = false;
        fadingOut = true;

        while (fadingOut && myUIParentCg.alpha > 0)
        {
            myUIParentCg.alpha -= 0.1f * uiFadeSpeed * Time.deltaTime;

            if(myUIParentCg.alpha == 0)
            {
                myUIParent.SetActive(false);
            }
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion






}
