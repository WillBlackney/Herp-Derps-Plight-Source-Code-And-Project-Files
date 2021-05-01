using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using Spriter2UnityDX;
using Sirenix.OdinInspector;
using System;

public class CharacterEntityController : Singleton<CharacterEntityController>
{
    // Properties + Component References
    #region
    [Header("Character Entity List Variables")]
    private List<CharacterEntityModel> allCharacters = new List<CharacterEntityModel>();
    private List<CharacterEntityModel> allDefenders = new List<CharacterEntityModel>();
    private List<CharacterEntityModel> allEnemies = new List<CharacterEntityModel>();
    private List<CharacterEntityModel> allSummonedDefenders = new List<CharacterEntityModel>();

    [Header("UCM Colours")]
    public Color normalColour;
    public Color highlightColour;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    #endregion

    // Property Accessors
    #region
    public List<CharacterEntityModel> AllCharacters
    {
        get
        {
            return allCharacters;
        }
        private set
        {
            allCharacters = value;
        }
    }
    public List<CharacterEntityModel> AllDefenders
    {
        get
        {
            return allDefenders;
        }
        private set
        {
            allDefenders = value;
        }
    }
    public List<CharacterEntityModel> AllSummonedDefenders
    {
        get
        {
            return allSummonedDefenders;
        }
        private set
        {
            allSummonedDefenders = value;
        }
    }
    public List<CharacterEntityModel> AllEnemies
    {
        get
        {
            return allEnemies;
        }
        private set
        {
            allEnemies = value;
        }
    }
    #endregion 

    // Create Characters Logic + Setup
    #region    
    private GameObject CreateCharacterEntityView()
    {
        return Instantiate(PrefabHolder.Instance.characterEntityModel, transform.position, Quaternion.identity);
    }
    private void SetCharacterViewStartingState(CharacterEntityModel character)
    {
        CharacterEntityView view = character.characterEntityView;

        // Disable Intent vm for player characters
        if(character.controller == Controller.Player)
        {
            view.myIntentViewModel.gameObject.SetActive(false);
        }

        // Disable block icon
        view.blockIcon.SetActive(false);

        // Get camera references
        view.uiCanvas.worldCamera = CameraManager.Instance.MainCamera;

        // Disable main UI canvas + card UI stuff
        view.uiCanvasParent.SetActive(false);
    }
    public void CreateAllPlayerCombatCharacters()
    {
        foreach (CharacterData data in CharacterDataController.Instance.AllPlayerCharacters)
        {
            // Dont spawn dead characters
            if (data.health > 0)
            {
                CreatePlayerCharacter(data, LevelManager.Instance.GetNextAvailableDefenderNode());
            }          
        }
    }
    public void DestroyCharacterViewModelsAndGameObjects(List<CharacterEntityModel> charactersDestroyed)
    {
        foreach (CharacterEntityModel character in charactersDestroyed)
        {
            Destroy(character.characterEntityView.gameObject);
        }
    }
    public void ClearAllCharacterPersistencies()
    {
        AllCharacters.Clear();
        AllDefenders.Clear();
        AllEnemies.Clear();
        AllSummonedDefenders.Clear();
    }
    public CharacterEntityModel CreateSummonedPlayerCharacter(SummonedCharacterDataSO data, LevelNode position)
    {
        // Create GO + View
        CharacterEntityView vm = CreateCharacterEntityView().GetComponent<CharacterEntityView>();

        // Face enemies
        LevelManager.Instance.SetDirection(vm, FacingDirection.Right);

        // Create data object
        CharacterEntityModel model = new CharacterEntityModel();

        // Connect model to view
        model.characterEntityView = vm;
        vm.character = model;

        // Connect model to character data
        //model.characterData = data;

        // Set up positioning in world
        LevelManager.Instance.PlaceEntityAtNode(model, position);

        // Set type + allegiance
        model.controller = Controller.Player;
        model.allegiance = Allegiance.Player;

        // Set up view
        SetCharacterViewStartingState(model);

        // Copy data from character data into new model
        SetupSummonedCharacterFromCharacterData(model, data);

        // Build deck
        CardController.Instance.BuildCharacterEntityCombatDeckFromCardDataSoSet(model, data.deck);

        // Add to persistency
        AddSummonedDefenderToPersistency(model);

        return model;
    }
    public CharacterEntityModel CreatePlayerCharacter(CharacterData data, LevelNode position)
    {       
        // Create GO + View
        CharacterEntityView vm = CreateCharacterEntityView().GetComponent<CharacterEntityView>();

        // Face enemies
        LevelManager.Instance.SetDirection(vm, FacingDirection.Right);

        // Create data object
        CharacterEntityModel model = new CharacterEntityModel();

        // Connect model to view
        model.characterEntityView = vm;
        vm.character = model;

        // Connect model to character data
        model.characterData = data;

        // Set up positioning in world
        LevelManager.Instance.PlaceEntityAtNode(model, position);

        // Set type + allegiance
        model.controller = Controller.Player;
        model.allegiance = Allegiance.Player;

        // Set up view
        SetCharacterViewStartingState(model);

        // Copy data from character data into new model
        SetupCharacterFromCharacterData(model, model.characterData);

        // Build deck
        CardController.Instance.BuildCharacterEntityCombatDeckFromDeckData(model, data.deck);

        // Add to persistency
        AddDefenderToPersistency(model);

        return model;
    }
    public CharacterEntityModel CreateEnemyCharacter(EnemyDataSO data, LevelNode position)
    {
        // Create GO + View
        CharacterEntityView vm = CreateCharacterEntityView().GetComponent<CharacterEntityView>();

        // Face player characters
        LevelManager.Instance.SetDirection(vm, FacingDirection.Left);

        // Create data object
        CharacterEntityModel model = new CharacterEntityModel();

        // Connect model to view
        model.characterEntityView = vm;
        vm.character = model;

        // Connect model to data
        model.enemyData = data;

        // Set up positioning in world
        LevelManager.Instance.PlaceEntityAtNode(model, position);

        // Set type + allegiance
        model.controller = Controller.AI;
        model.allegiance = Allegiance.Enemy;

        // Set Character Model Size
        SetCharacterModelSize(vm, data.enemyModelSize);

        // Set up view
        SetCharacterViewStartingState(model);

        // Copy data from character data into new model
        SetupCharacterFromEnemyData(model, data);

        // Add to persistency
        AddEnemyToPersistency(model);

        return model;
    }
    private void SetupSummonedCharacterFromCharacterData(CharacterEntityModel character, SummonedCharacterDataSO data)
    {
        // Set general info
        character.myName = data.myName;
        character.audioProfile = data.audioProfile;

        // Setup Core Stats
        ModifyStrength(character, data.strength);
        ModifyIntelligence(character, data.intelligence);
        ModifyWits(character, data.wits);
        ModifyDexterity(character, data.dexterity);
        ModifyConstitution(character, data.constitution);

        // Setup Secondary Stats
        ModifyStamina(character, data.stamina);
        ModifyMaxEnergy(character, data.maxEnergy);
        ModifyInitiative(character, data.initiative);
        ModifyDraw(character, data.draw);
        ModifyPower(character, data.power);
        ModifyBaseCrit(character, data.baseCrit);
        ModifyCritModifier(character, data.critModifier);

        // Set up health
        ModifyMaxHealth(character, data.maxHealth);
        ModifyHealth(character, data.health);

        // TO DO IN FUTURE: We need a better way to track character data's body 
        // parts: strings references are not scaleable
        // Build UCM
        CharacterModelController.Instance.BuildModelFromStringReferences(character.characterEntityView.ucm, data.modelParts);

        // Build activation window
        ActivationManager.Instance.CreateActivationWindow(character);

        // Set up passive traits
        PassiveController.Instance.BuildCharacterEntityPassivesFromSummonedCharacterData(character, data);
    }
    private void SetupCharacterFromCharacterData(CharacterEntityModel character, CharacterData data)
    {
        // Set general info
        character.myName = data.myName;
        character.audioProfile = data.audioProfile;

        // Setup Core Stats
        ModifyStrength(character, data.strength);
        ModifyIntelligence(character, data.intelligence);
        ModifyWits(character, data.wits);
        ModifyDexterity(character, data.dexterity);
        ModifyConstitution(character, data.constitution);

        // Setup Secondary Stats
        ModifyStamina(character, data.stamina);
        ModifyMaxEnergy(character, data.maxEnergy);
        ModifyInitiative(character, data.initiative);
        ModifyDraw(character, data.draw);       
        ModifyPower(character, data.power);
        ModifyBaseCrit(character, data.baseCrit);
        ModifyCritModifier(character, data.critModifier);

        // Set up health
        ModifyMaxHealth(character, data.maxHealth);
        ModifyHealth(character, data.health);

        // TO DO IN FUTURE: We need a better way to track character data's body 
        // parts: strings references are not scaleable
        // Build UCM
        CharacterModelController.Instance.BuildModelFromStringReferences(character.characterEntityView.ucm, data.modelParts);

        // Build activation window
        ActivationManager.Instance.CreateActivationWindow(character);

        // Set up passive traits
        PassiveController.Instance.BuildPlayerCharacterEntityPassivesFromCharacterData(character, data);

        // Set up items
        ItemController.Instance.RunItemSetupOnCharacterEntityFromItemManagerData(character, data.itemManager);
    }
    private void SetupCharacterFromEnemyData(CharacterEntityModel character, EnemyDataSO data)
    {
        // Set general info
        character.myName = data.enemyName;
        character.audioProfile = data.audioProfile;

        // Core Attributes
        ModifyStrength(character, data.strength);
        ModifyDexterity(character, data.dexterity);
        ModifyWits(character, data.wits);
        ModifyIntelligence(character, data.intelligence);
        ModifyConstitution(character, data.constitution);

        // Secondary Attrbibutes
        ModifyInitiative(character, data.initiative);
        ModifyPower(character, data.power);

        // Set Resistances
        ModifyPhysicalResistance(character, data.physicalResistance);
        ModifyMagicResistance(character, data.magicResistance);

        

        // Set up health + max health
        if (data.enableFlexibleMaxHealth)
        {
            int lower = data.maxHealth - data.maxHealthFlexAmount;
            int upper = data.maxHealth + data.maxHealthFlexAmount;
            int randomMaxHealth = RandomGenerator.NumberBetween(lower, upper);
            ModifyMaxHealth(character, randomMaxHealth);
        }
        else        
            ModifyMaxHealth(character, data.maxHealth);        
        
        if (data.overrideStartingHealth)                    
            ModifyHealth(character, data.startingHealth);        
        else
            ModifyHealth(character, character.maxHealth);

        // Set starting block
        GainBlock(character, data.startingBlock, false);

        // Build UCM
        CharacterModelController.Instance.BuildModelFromStringReferences(character.characterEntityView.ucm, data.allBodyParts);

        // Build activation window
        ActivationManager.Instance.CreateActivationWindow(character);

        // Set up passive traits
        PassiveController.Instance.BuildEnemyCharacterEntityPassivesFromEnemyData(character, data);

    }
    #endregion

    // Modify Entity Lists
    #region
    public void AddDefenderToPersistency(CharacterEntityModel character)
    {
        Debug.Log("CharacterEntityController.AddDefenderPersistency() called, adding: " + character.myName);
        AllCharacters.Add(character);
        AllDefenders.Add(character);
    }
    public void AddSummonedDefenderToPersistency(CharacterEntityModel character)
    {
        Debug.Log("CharacterEntityController.AddDefenderPersistency() called, adding: " + character.myName);
        AllCharacters.Add(character);
        AllSummonedDefenders.Add(character);
    }
    public void RemoveDefenderFromPersistency(CharacterEntityModel character)
    {
        Debug.Log("CharacterEntityController.RemoveDefenderFromPersistency() called, removing: " + character.myName);
        AllCharacters.Remove(character);
        AllDefenders.Remove(character);
    }
    public void RemoveSummonedDefenderFromPersistency(CharacterEntityModel character)
    {
        Debug.Log("CharacterEntityController.RemoveDefenderFromPersistency() called, removing: " + character.myName);
        AllCharacters.Remove(character);
        AllSummonedDefenders.Remove(character);
    }
    public void AddEnemyToPersistency(CharacterEntityModel character)
    {
        Debug.Log("CharacterEntityController.AddEnemyToPersistency() called, adding: " + character.myName);
        AllCharacters.Add(character);
        AllEnemies.Add(character);
    }
    public void RemoveEnemyFromPersistency(CharacterEntityModel character)
    {
        Debug.Log("CharacterEntityController.RemoveEnemyFromPersistency() called, removing: " + character.myName);
        AllCharacters.Remove(character);
        AllEnemies.Remove(character);
    }
    #endregion

    // Destroy models and views logic
    #region
    public void DisconnectModelFromView(CharacterEntityModel character)
    {
        Debug.Log("CharacterEntityController.DisconnectModelFromView() called for: " + character.myName);

        character.characterEntityView.character = null;
        character.characterEntityView = null;
    }
    public void DestroyCharacterView(CharacterEntityView view)
    {
        Debug.Log("CharacterEntityController.DestroyCharacterView() called...");
        Destroy(view.gameObject);
    }
    #endregion

    // Modify Health
    #region
    public void ModifyHealth(CharacterEntityModel character, int healthGainedOrLost)
    {
        Debug.Log("CharacterEntityController.ModifyHealth() called for " + character.myName);

        int originalHealth = character.health;
        int finalHealthValue = character.health;

        finalHealthValue += healthGainedOrLost;

        // prevent health increasing over maximum
        if (finalHealthValue > character.MaxHealthTotal)
        {
            finalHealthValue = character.MaxHealthTotal;
        }

        // prevent health going less then 0
        if (finalHealthValue < 0)
        {
            finalHealthValue = 0;
        }

        if (finalHealthValue > originalHealth)
        {
            // StartCoroutine(VisualEffectManager.Instance.CreateHealEffect(character.characterEntityView.transform.position, healthGainedOrLost));
        }

        // Set health after calculation
        character.health = finalHealthValue;

        // relay changes to character data
        if (character.characterData != null)
        {
            CharacterDataController.Instance.SetCharacterHealth(character.characterData, character.health);
        }

        Debug.LogWarning(character.myName + " health value = " + character.health.ToString());

        VisualEventManager.Instance.CreateVisualEvent(() => UpdateHealthGUIElements(character, finalHealthValue, character.MaxHealthTotal), QueuePosition.Back, 0, 0);
    }
    public void ModifyMaxHealth(CharacterEntityModel character, int maxHealthGainedOrLost)
    {
        Debug.Log("CharacterEntityController.ModifyMaxHealth() called for " + character.myName);

        character.maxHealth += maxHealthGainedOrLost;

        // relay changes to character data
        if (character.characterData != null)
        {
            CharacterDataController.Instance.SetCharacterMaxHealth(character.characterData, character.maxHealth);
        }      

        int currentHealth = character.health;
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateHealthGUIElements(character, currentHealth, character.MaxHealthTotal), QueuePosition.Back, 0, 0);

        // Update health if it now excedes max health
        if (character.health > character.MaxHealthTotal)
        {
            ModifyHealth(character, (character.MaxHealthTotal - character.health));
        }
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
    public void ModifyBaseCrit(CharacterEntityModel character, int gainedOrLost)
    {
        character.baseCrit += gainedOrLost;
    }
    public void ModifyCritModifier(CharacterEntityModel character, int gainedOrLost)
    {
        character.critModifier += gainedOrLost;
    }
    public void ModifyPower(CharacterEntityModel character, int powerGainedOrLost)
    {
        character.power += powerGainedOrLost;
    }
    public void ModifyStrength(CharacterEntityModel character, int strengthGainedOrLost)
    {
        character.strength += strengthGainedOrLost;
    }
    public void ModifyWits(CharacterEntityModel character, int witsGainedOrLost)
    {
        character.wits += witsGainedOrLost;
    }
    public void ModifyIntelligence(CharacterEntityModel character, int intelligenceGainedOrLast)
    {
        character.intelligence += intelligenceGainedOrLast;
    }
    public void ModifyDexterity(CharacterEntityModel character, int dexterityGainedOrLost)
    {
        character.dexterity += dexterityGainedOrLost;
    }
    public void ModifyConstitution(CharacterEntityModel character, int constitutionGainedOrLost)
    {
        character.constitution += constitutionGainedOrLost;
    }
    public void ModifyPhysicalResistance(CharacterEntityModel character, int resistGainedOrLost)
    {
        character.basePhysicalResistance += resistGainedOrLost;
        int newValue = CombatLogic.Instance.GetTotalResistance(character, DamageType.Physical);

        // Update GUI
        VisualEventManager.Instance.CreateVisualEvent(() => UpdatePhysicalResistanceGUI(character, newValue), QueuePosition.Back, 0, 0);
    }
    public void ModifyMagicResistance(CharacterEntityModel character, int resistGainedOrLost)
    {
        character.baseMagicResistance += resistGainedOrLost;

        int newValue = CombatLogic.Instance.GetTotalResistance(character, DamageType.Magic);

        // Update GUI
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateMagicResistanceGUI(character, newValue), QueuePosition.Back, 0, 0);
    }
    #endregion

    // Modify Energy
    #region
    public void ModifyEnergy(CharacterEntityModel character, int energyGainedOrLost, bool showVFX = false)
    {
        Debug.Log("CharacterEntityController.ModifyEnergy() called for " + character.myName);
        character.energy += energyGainedOrLost;
        CharacterEntityView view = character.characterEntityView;

        if (character.energy < 0)
        {
            character.energy = 0;
        }

        else if (character.energy > character.maxEnergy)
        {
            character.energy = character.maxEnergy;
        }

        if (showVFX && view != null)
        {
            // Status notification
            VisualEventManager.Instance.CreateVisualEvent(() =>
            VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Energy +" + energyGainedOrLost.ToString()));

            // Buff sparkle VFX
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateGeneralBuffEffect(view.WorldPosition));
        }

        int energyVfxValue = character.energy;
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateEnergyGUI(view, energyVfxValue), QueuePosition.Front, 0, 0);

        CardController.Instance.AutoUpdateCardsInHandGlowOutlines(character);
    }
    public void ModifyMaxEnergy(CharacterEntityModel character, int maxEnergyGainedOrLost)
    {
        Debug.Log("CharacterEntityController.ModifyMaxEnergy() called for " + character.myName);
        character.maxEnergy += maxEnergyGainedOrLost;
        CharacterEntityView view = character.characterEntityView;

        if (character.maxEnergy < 0)
        {
            character.maxEnergy = 0;
        }

        int energyVfxValue = character.maxEnergy;
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateMaxEnergyGUI(view, energyVfxValue), QueuePosition.Front, 0, 0);

    }
    public void ModifyStamina(CharacterEntityModel character, int staminaGainedOrLost)
    {
        Debug.Log("CharacterEntityController.ModifyStamina() called for " + character.myName);
        character.stamina += staminaGainedOrLost;
        CharacterEntityView view = character.characterEntityView;

        if (character.stamina < 0)
        {
            character.stamina = 0;
        }

        int staminaVfxValue = character.stamina;
        if (character.pManager != null)
        {
            staminaVfxValue = EntityLogic.GetTotalStamina(character);
        }

        VisualEventManager.Instance.CreateVisualEvent(() => UpdateStaminaGUI(view, staminaVfxValue), QueuePosition.Back, 0, 0);
    }
    private void UpdateEnergyGUI(CharacterEntityView view, int newValue)
    {
        view.energyText.text = newValue.ToString();
    }
    private void UpdateMaxEnergyGUI(CharacterEntityView view, int newValue)
    {
        view.maxEnergyText.text = newValue.ToString();
    }
    private void UpdateStaminaGUI(CharacterEntityView view, int newValue)
    {
        view.staminaText.text = newValue.ToString();
    }
    #endregion

    // Modify Block
    #region
    public void GainBlock(CharacterEntityModel character, int blockGained, bool showVFX = true)
    {
        Debug.Log("CharacterEntityController.GainBlock() called for " + character.myName);

        int finalBlockGainValue = blockGained;
        int characterFinalBlockValueForVisual = 0;
        CharacterEntityView view = character.characterEntityView;

        // prevent block going negative        
        if (finalBlockGainValue < 0)
        {
            finalBlockGainValue = 0;
        }         

        // Apply block gain
        character.block += finalBlockGainValue;
        character.blockGainedThisTurnCycle += finalBlockGainValue;
        bool didExceedMax = false;

        // prevent block exceeding maximum
        if (GlobalSettings.Instance.enableMaximumBlock && character.block > GlobalSettings.Instance.maximumBlockAmount)
        {
            int previousOverflow = character.block;
            character.block = GlobalSettings.Instance.maximumBlockAmount;
            didExceedMax = true;
            int difference = previousOverflow - GlobalSettings.Instance.maximumBlockAmount;
            character.blockGainedThisTurnCycle -= difference;
        }

        characterFinalBlockValueForVisual = character.block;

        if (finalBlockGainValue > 0 && showVFX)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateGainBlockEffect(view.WorldPosition, finalBlockGainValue), QueuePosition.Back, 0, 0);

            if (didExceedMax)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "MAXIMUM BLOCK!"), QueuePosition.Back, 0, 0);
            }
        }

        if (showVFX)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => UpdateBlockGUI(character, characterFinalBlockValueForVisual), QueuePosition.Back, 0, 0);
        }
        else
        {
            UpdateBlockGUI(character, characterFinalBlockValueForVisual);
        }

        // Resolve Sentinel passive effect
        if(character.pManager != null &&
           character.pManager.sentinelStacks > 0 &&
           finalBlockGainValue > 0)
        {
            // Notification event
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Sentinel!"), QueuePosition.Back, 0, 0.5f);

            // Setup
            List<CharacterEntityModel> validEnemies = new List<CharacterEntityModel>();
            CharacterEntityModel targetHit = null;

            // Get valid enemies
            foreach (CharacterEntityModel enemy in GetAllEnemiesOfCharacter(character))
            {
                if(enemy.livingState == LivingState.Alive)
                {
                    validEnemies.Add(enemy);
                }
            }

            // Determine target hit
            if (validEnemies.Count == 1)
            {
                targetHit = validEnemies[0];
            }
            else if (validEnemies.Count > 1)
            {
                targetHit = validEnemies[RandomGenerator.NumberBetween(0, validEnemies.Count - 1)];
            }

            // Did we find a valid target?
            if(targetHit != null)
            {
                // We did, start the damage process
                VisualEventManager.Instance.CreateVisualEvent(() => CameraManager.Instance.CreateCameraShake(CameraShakeType.Small));

                // Calculate and handle damage
                int sentinelDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(character, targetHit, DamageType.Physical, character.pManager.sentinelStacks);
                CombatLogic.Instance.HandleDamage(sentinelDamageValue, character, targetHit, DamageType.Physical);
            }
        }
    }
    public void SetBlock(CharacterEntityModel character, int newBlockValue, bool trackAmountForThisTurnCycle = true)
    {
        Debug.Log("CharacterEntityController.SetBlock() called for " + character.myName);

        if(newBlockValue < 0)
        {
            newBlockValue = 0;
        }

        if(trackAmountForThisTurnCycle)
            character.blockGainedThisTurnCycle += newBlockValue - character.block;

        // Apply block gain
        character.block = newBlockValue;

        // Update GUI
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateBlockGUI(character, newBlockValue), QueuePosition.Back, 0, 0);
    }
    public void UpdateBlockGUI(CharacterEntityModel character, int newBlockValue)
    {
        character.characterEntityView.blockText.text = newBlockValue.ToString();
        if (newBlockValue > 0)
        {
            character.characterEntityView.blockIcon.SetActive(true);
        }
        else
        {
            character.characterEntityView.blockIcon.SetActive(false);
        }
    }
    public void UpdatePhysicalResistanceGUI(CharacterEntityModel character, int newPhysicalResistance)
    {
        // Enable or Disable view
        if (newPhysicalResistance != 0)
        {
            character.characterEntityView.physicalResistanceParent.SetActive(true);
        }
        else
        {
            character.characterEntityView.physicalResistanceParent.SetActive(false);
        }

        // Set Text + Colour
        if (newPhysicalResistance > 0)
        {
            character.characterEntityView.physicalResistanceText.text = "<color=#9BFF28>" + newPhysicalResistance.ToString() +"%";
        }
        else if (newPhysicalResistance < 0)
        {
            character.characterEntityView.physicalResistanceText.text = "<color=#FF4332>" + newPhysicalResistance.ToString() + "%";
        }
    }
    public void UpdateMagicResistanceGUI(CharacterEntityModel character, int newMagicResistance)
    {
        // Enable or Disable view
        if (newMagicResistance != 0)
        {
            character.characterEntityView.magicResistanceParent.SetActive(true);
        }
        else
        {
            character.characterEntityView.magicResistanceParent.SetActive(false);
        }

        // Set Text + Colour
        if (newMagicResistance > 0)
        {
            character.characterEntityView.magicResistanceText.text = "<color=#9BFF28>" + newMagicResistance.ToString() + "%";
        }
        else if (newMagicResistance < 0)
        {
            character.characterEntityView.magicResistanceText.text = "<color=#FF4332>" + newMagicResistance.ToString() + "%";
        }
    }
    #endregion

    // Activation Related
    #region    
    private void SetCharacterActivationPhase(CharacterEntityModel character, ActivationPhase newPhase)
    {
        Debug.Log("CharacterEntityController.SetCharacterActivationPhase() called for " + character.myName +", new phase = " + newPhase.ToString());
        character.activationPhase = newPhase;
    }
    public void CharacterOnNewTurnCycleStarted(CharacterEntityModel character)
    {
        Debug.Log("CharacterEntityController.CharacterOnNewTurnCycleStartedCoroutine() called for " + character.myName);

        character.hasActivatedThisTurn = false;

        // do block stuff
        character.blockGainedPreviousTurnCycle = character.blockGainedThisTurnCycle;
        character.blockGainedThisTurnCycle = 0;


        
    }
    public void CharacterOnActivationStart(CharacterEntityModel character)
    {
        Debug.Log("CharacterEntityController.CharacterOnActivationStart() called for " + character.myName);

        // Set Phase
        SetCharacterActivationPhase(character, ActivationPhase.StartPhase);

        // Enable activated view state
        LevelNode charNode = character.levelNode;
        VisualEventManager.Instance.CreateVisualEvent(() => LevelManager.Instance.SetActivatedViewState(charNode, true), QueuePosition.Back);

        // PLAYER EXCLUSIVE energy gain logic
        if (character.controller == Controller.Player)
        {
            int energyGain = EntityLogic.GetTotalStamina(character);

            // Check 'Eagerness' state
            if ((StateController.Instance.DoesPlayerHaveState(StateName.Eagerness)|| StateController.Instance.DoesPlayerHaveState(StateName.Ambition)) &&
                ActivationManager.Instance.CurrentTurn == 1)
                energyGain += 1;

            // Gain Energy
            ModifyEnergy(character, energyGain);

            // Update energy text
            VisualEventManager.Instance.CreateVisualEvent(() => UpdateEnergyGUI(character.characterEntityView, character.energy), QueuePosition.Back, 0, 0);
        }

        // Handle remove block from previous turn cycle
        if (GlobalSettings.Instance.blockExpiresOnActivationStart && 
            character.blockGainedPreviousTurnCycle > 0 && 
            character.block > 0)
        {
            if(character.pManager.unbreakableStacks == 0 ||
                !StateController.Instance.DoesPlayerHaveState(StateName.ToughNutz))
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Block Expiry"));
                SetBlock(character, character.block - character.blockGainedPreviousTurnCycle, false);
                VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
            } 
        }

        // Handle remove block from cautious (if character has it and it was triggered)
        if (GlobalSettings.Instance.blockExpiresOnActivationStart &&
           character.didTriggerCautiousPrior &&
           character.blockFromCautiousGained > 0)
        {
            if (character.pManager.unbreakableStacks == 0 ||
                !StateController.Instance.DoesPlayerHaveState(StateName.ToughNutz))
            {
                SetBlock(character, character.block - character.blockFromCautiousGained, false);
                character.blockFromCautiousGained = 0;
                character.didTriggerCautiousPrior = false;
            }                
        }      

        // PLAYER EXCLUSIVE on activation start logic
        if (character.controller == Controller.Player)
        {
            // Remove temp stamina bonus
            if (character.pManager.temporaryBonusStaminaStacks != 0)
            {
                PassiveController.Instance.ModifyTemporaryStamina(character.pManager, -character.pManager.temporaryBonusStaminaStacks, true, 0.5f);
            }

            // Activate main UI canvas view
            CoroutineData cData = new CoroutineData();
            VisualEventManager.Instance.CreateVisualEvent(() => FadeInCharacterUICanvas(character.characterEntityView, cData), cData, QueuePosition.Back);

            // Resolve first turn talent bonuses
            /*
            if(ActivationManager.Instance.CurrentTurn == 1)
            {
                ResolveFirstActivationTalentBonuses(character);
            }*/

            // Before normal card draw, add cards to hand from passive effects (e.g. Fan Of Knives)
            // Fan of Knives
            if (character.pManager.fanOfKnivesStacks > 0)
            {
                for (int i = 0; i < character.pManager.fanOfKnivesStacks; i++)
                {
                    CardController.Instance.CreateAndAddNewCardToCharacterHand(character, CardController.Instance.GetCardDataFromLibraryByName("Shank"));
                }
            }

            // Divine Favour
            if (character.pManager.divineFavourStacks > 0)
            {
                for (int i = 0; i < character.pManager.divineFavourStacks; i++)
                {
                    List<CardData> blessings = CardController.Instance.QueryByBlessing(CardController.Instance.AllCards, true);
                    blessings = CardController.Instance.QueryByNonUpgraded(blessings);

                    CardData randomBlessing = blessings[RandomGenerator.NumberBetween(0, blessings.Count - 1)];
                    CardController.Instance.CreateAndAddNewCardToCharacterHand(character, randomBlessing);
                }
            }

            // Phoenix Form
            if (character.pManager.phoenixFormStacks > 0)
            {
                for (int i = 0; i < character.pManager.phoenixFormStacks; i++)
                {
                    CardController.Instance.CreateAndAddNewCardToCharacterHand(character, CardController.Instance.GetCardDataFromLibraryByName("Fire Ball"));
                }
            }

            // Magic Magnet
            if (character.pManager.magicMagnetStacks > 0)
            {
                for (int i = 0; i < character.pManager.magicMagnetStacks; i++)
                {
                    CardController.Instance.CreateAndAddNewCardToCharacterHand(character, CardController.Instance.GetCardDataFromLibraryByName("Arcane Bolt"));
                }
            }

            // Well Of Souls
            if (character.pManager.wellOfSoulsStacks > 0)
            {
                for (int i = 0; i < character.pManager.wellOfSoulsStacks; i++)
                {
                    CardController.Instance.CreateAndAddNewCardToCharacterHand(character, CardController.Instance.GetCardDataFromLibraryByName("Haunt"));
                }
            }

            // Fast Learner
            if (character.pManager.fastLearnerStacks > 0)
            {
                // Get all common cards
                List<CardData> viableCards = new List<CardData>();        
                foreach(CardData c in CardController.Instance.GetCardsQuery(CardController.Instance.AllCards, TalentSchool.None, Rarity.Common, false, UpgradeFilter.OnlyNonUpgraded))
                {
                    if(c.racialCard == false)
                    {
                        viableCards.Add(c);
                    }
                }

                for (int i = 0; i < character.pManager.fastLearnerStacks; i++)
                {
                    // Create and add new card to hand
                    CardData newCard = viableCards[RandomGenerator.NumberBetween(0, viableCards.Count - 1)];
                    Card card = CardController.Instance.CreateAndAddNewCardToCharacterHand(character, newCard);

                    // Reduce its energy cost by 1
                    CardController.Instance.SetCardEnergyCostThisCombat(card, 0);
                }
            }

            // Check patience passive
            if (character.pManager.patienceStacks > 0 &&
                character.meleeAttacksPlayedLastActivation == 0 &&
                character.rangedAttacksPlayedLastActivation == 0 &&
                ActivationManager.Instance.CurrentTurn > 1)
            {
                // Notication vfx
                VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Patience!"), QueuePosition.Back, 0, 0.5f);

                // Draw extra card and gain extra energy
                CardController.Instance.DrawACardFromDrawPile(character);
                ModifyEnergy(character, 1);
                VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
            }

            // Draw cards on turn start
            CardController.Instance.DrawCardsOnActivationStart(character);

            // Remove temp draw
            if (character.pManager.temporaryBonusDrawStacks != 0)
            {
                PassiveController.Instance.ModifyTemporaryDraw(character.pManager, -character.pManager.temporaryBonusDrawStacks, true, 0.5f);
            }            
        }

        // NON-EXCLUSIVE passives and turn start logic

        // Grit expiry
        if (character.pManager.gritStacks > 0)
        {
            PassiveController.Instance.ModifyGrit(character.pManager, -1, true, 0.5f);
        }      

        // Lord Of Storms
        if (character.pManager.lordOfStormsStacks > 0)
        {
            PassiveController.Instance.ModifyOverload(character.pManager, character.pManager.lordOfStormsStacks, true, 0.5f);
        }

        // Regeneration
        if (character.pManager.regenerationStacks > 0)
        {
            // Notication vfx
            VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Regeneration!"), QueuePosition.Back, 0, 0.5f);

            // Modify health
            ModifyHealth(character, character.pManager.regenerationStacks);

            // Heal VFX
            VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateHealEffect(character.characterEntityView.WorldPosition, character.pManager.regenerationStacks));

            // Create heal text effect
            VisualEventManager.Instance.CreateVisualEvent(() =>
            VisualEffectManager.Instance.CreateDamageEffect(character.characterEntityView.WorldPosition, character.pManager.regenerationStacks, true));

            // Create SFX
            VisualEventManager.Instance.CreateVisualEvent(() => AudioManager.Instance.PlaySoundPooled(Sound.Passive_General_Buff));

            VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
        }

        // Shield Wall
        if (character.pManager.shieldWallStacks > 0)
        {
            // Notication vfx
            VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Shield Wall"), QueuePosition.Back, 0, 0.5f);

            // Apply block gain
            GainBlock(character, CombatLogic.Instance.CalculateBlockGainedByEffect(character.pManager.shieldWallStacks, character, character));
            VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
        }

        // AURAS
        int auraLoops = 1;
        if (StateController.Instance.DoesPlayerHaveState(StateName.Radiance) && character.controller == Controller.Player)
            auraLoops = 2;
        for(int i = 0; i < auraLoops; i++)
        {
            // Shadow Aura        
            if (character.pManager.shadowAuraStacks > 0)
            {
                CharacterEntityModel[] allEnemies = GetAllEnemiesOfCharacter(character).ToArray();
                CharacterEntityModel chosenEnemy = allEnemies[RandomGenerator.NumberBetween(0, allEnemies.Length - 1)];

                if (chosenEnemy != null)
                {
                    // Notification event
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Shadow Aura!"), QueuePosition.Back, 0, 0.5f);

                    // Random enemy is weakened
                    PassiveController.Instance.ModifyWeakened(chosenEnemy.pManager, character.pManager.shadowAuraStacks, character.pManager, true);
                }
            }

            // Intimidating Aura        
            if (character.pManager.intimidatingAuraStacks > 0)
            {
                CharacterEntityModel[] allEnemies = GetAllEnemiesOfCharacter(character).ToArray();
                CharacterEntityModel chosenEnemy = allEnemies[RandomGenerator.NumberBetween(0, allEnemies.Length - 1)];

                if (chosenEnemy != null)
                {
                    // Notification event
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Intimidating Aura!"), QueuePosition.Back, 0, 0.5f);

                    // Random enemy is weakened
                    PassiveController.Instance.ModifyVulnerable(chosenEnemy.pManager, character.pManager.intimidatingAuraStacks, character.pManager, true);
                }
            }

            // Provocative Aura        
            if (character.pManager.provocativeAuraStacks > 0)
            {
                CharacterEntityModel[] allEnemies = GetAllEnemiesOfCharacter(character).ToArray();
                CharacterEntityModel chosenEnemy = allEnemies[RandomGenerator.NumberBetween(0, allEnemies.Length - 1)];

                if (chosenEnemy != null)
                {
                    // Notification event
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Provocative Aura!"), QueuePosition.Back, 0, 0.5f);

                    // Random enemy is weakened
                    HandleTaunt(character, chosenEnemy);
                }
            }

        }
        

        // ENEMY Start turn 
        if (character.controller == Controller.AI &&
            character.allegiance == Allegiance.Enemy)
        {
            VisualEventManager.Instance.InsertTimeDelayInQueue(1f);
            StartEnemyActivation(character);
        }

        // PLAYER Start turn
        if (character.controller == Controller.Player)
        {
            // Set Activated Phase
            SetCharacterActivationPhase(character, ActivationPhase.ActivationPhase);
        }
    }
    public void CharacterOnActivationEnd(CharacterEntityModel entity)
    {
        Debug.Log("CharacterEntityController.CharacterOnActivationEnd() called for " + entity.myName);

        // Set Activated Phase
        SetCharacterActivationPhase(entity, ActivationPhase.EndPhase);

        // Cache refs for visual events
        LevelNode veNode = entity.levelNode;
        CharacterEntityView view = entity.characterEntityView;

        // Disable end turn button clickability
        UIManager.Instance.DisableEndTurnButtonInteractions();

        // reset misc properties
        entity.meleeAttacksPlayedLastActivation = entity.meleeAttacksPlayedThisActivation;
        entity.meleeAttacksPlayedThisActivation = 0;
        entity.rangedAttacksPlayedLastActivation = entity.rangedAttacksPlayedThisActivation;
        entity.rangedAttacksPlayedThisActivation = 0;

        // Brute force disable all activation rings
        VisualEventManager.Instance.CreateVisualEvent(() => LevelManager.Instance.DisableAllActivationRings());

        // Stop if combat has ended
        if (CombatLogic.Instance.CurrentCombatState != CombatGameState.CombatActive)
        {
            Debug.Log("CharacterEntityController.CharacterOnActivationEnd() detected combat state is not active, cancelling early... ");
            return;
        }

        // Do player character exclusive logic
        if (entity.controller == Controller.Player)
        {
            // Remove Dark Bargain
            if (entity.pManager.darkBargainStacks > 0)
            {
                PassiveController.Instance.ModifyDarkBargain(entity.pManager, -entity.pManager.darkBargainStacks, true, 0.5f);
            }

            // Lose unused energy, discard hand
            if (!StateController.Instance.DoesPlayerHaveState(StateName.Endurance))
                ModifyEnergy(entity, -entity.energy);

            // reset activation only energy values on cards
            CardController.Instance.ResetAllCardEnergyCostsOnActivationEnd(entity);

            // Run events on cards with 'OnActivationEnd' listener
            CardController.Instance.HandleOnCharacterActivationEndCardListeners(entity);

            // Expend fleeting cards
            CardController.Instance.ExpendFleetingCardsOnActivationEnd(entity);
            VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);

            // Discard Hand
            CardController.Instance.DiscardHandOnActivationEnd(entity);

            // Fade out view
            CoroutineData fadeOutEvent = new CoroutineData();
            VisualEventManager.Instance.CreateVisualEvent(() => FadeOutCharacterUICanvas(entity.characterEntityView, fadeOutEvent), fadeOutEvent);
        }

        // Do relevant passive expiries and logic
        #region
        // Remove Taunt
        if (entity.pManager.tauntStacks > 0)
        {
            PassiveController.Instance.ModifyTaunted(null, entity.pManager, -entity.pManager.tauntStacks, true, 0.5f);
        }

        // Temp core stats
        if (entity.pManager.temporaryBonusPowerStacks != 0)
        {
            PassiveController.Instance.ModifyTemporaryPower(entity.pManager, -entity.pManager.temporaryBonusPowerStacks, true, 0.5f);
        }
        if (entity.pManager.temporaryBonusDexterityStacks != 0)
        {
            PassiveController.Instance.ModifyTemporaryDexterity(entity.pManager, -entity.pManager.temporaryBonusDexterityStacks, true, 0.5f);
        }

        // Percentage modifiers
        if (entity.pManager.wrathStacks > 0)
        {
            PassiveController.Instance.ModifyWrath(entity.pManager, -1, true, 0.5f);
        }
        if (entity.pManager.weakenedStacks > 0)
        {
            PassiveController.Instance.ModifyWeakened(entity.pManager, -1, null, true, 0.5f);
        }
        if (entity.pManager.vulnerableStacks > 0)
        {
            PassiveController.Instance.ModifyVulnerable(entity.pManager, -1, null, true, 0.5f);
        }

        // Disabling Debuff Expiries
        if (entity.pManager.disarmedStacks > 0)
        {
            PassiveController.Instance.ModifyDisarmed(entity.pManager, -1, true, 0.5f);
        }
        if (entity.pManager.silencedStacks > 0)
        {
            PassiveController.Instance.ModifySilenced(entity.pManager, -1, true, 0.5f);
        }
        if (entity.pManager.sleepStacks > 0)
        {
            PassiveController.Instance.ModifySleep(entity.pManager, -1, true, 0.5f);
        }

        if (entity.pManager.takenAimStacks > 0)
        {
            PassiveController.Instance.ModifyTakenAim(entity.pManager, -entity.pManager.takenAimStacks, true, 0.5f);
        }

        // Buff Passive Triggers
        // Growing
        if (entity.pManager.growingStacks > 0)
        {
            // Notication vfx
            VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateStatusEffect(entity.characterEntityView.transform.position, "Growing!"), QueuePosition.Back, 0, 0.5f);

            // Gain power
            PassiveController.Instance.ModifyBonusPower(entity.pManager, entity.pManager.growingStacks, true, 0.5f);
        }

      

        // AURAS
        int auraLoops = 1;
        if (StateController.Instance.DoesPlayerHaveState(StateName.Radiance) && entity.controller == Controller.Player)
            auraLoops = 2;
        for (int i = 0; i < auraLoops; i++)
        {
            // Encouraging Aura
            if (entity.pManager.encouragingAuraStacks > 0)
            {
                CharacterEntityModel chosenAlly = null;
                CharacterEntityModel[] allAllies = GetAllAlliesOfCharacter(entity, false).ToArray();
                if(allAllies.Length > 0)
                    chosenAlly = allAllies[RandomGenerator.NumberBetween(0, allAllies.Length - 1)];

                if (chosenAlly != null)
                {
                    // Notification event
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Encouraging Aura!"), QueuePosition.Back, 0, 0.5f);

                    // Random ally gains energy
                    ModifyEnergy(chosenAlly, entity.pManager.encouragingAuraStacks, true);
                }
            }

            // Enraging Aura
            if (entity.pManager.enragingAuraStacks > 0)
            {
                CharacterEntityModel chosenAlly = null;
                CharacterEntityModel[] allAllies = GetAllAlliesOfCharacter(entity, false).ToArray();
                if (allAllies.Length > 0)
                    chosenAlly = allAllies[RandomGenerator.NumberBetween(0, allAllies.Length - 1)];

                if (chosenAlly != null)
                {
                    // Notification event
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Enraging Aura!"), QueuePosition.Back, 0, 0.5f);

                    // Random ally gains energy
                    PassiveController.Instance.ModifyBonusPower(chosenAlly.pManager, entity.pManager.enragingAuraStacks, true);
                }
            }

            // Toxic Aura
            if (entity.pManager.toxicAuraStacks > 0)
            {
                // Notification event
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Toxic Aura!"), QueuePosition.Back, 0, 0.5f);

                // Create poison nova on caster
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreatePoisonNova(entity.characterEntityView.WorldPosition));

                // Apply poison to all enemies, and small poison explosion
                foreach (CharacterEntityModel enemy in GetAllEnemiesOfCharacter(entity))
                {
                    PassiveController.Instance.ModifyPoisoned(entity.pManager, enemy.pManager, entity.pManager.toxicAuraStacks, true);
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreatePoisonExplosion(enemy.characterEntityView.WorldPosition));
                }

                // Brief delay
                VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
            }

            // Guardian Aura
            if (entity.pManager.guardianAuraStacks > 0)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Guardian Aura!"), QueuePosition.Back, 0, 0.5f);

                // give all allies (but not self) block.
                foreach (CharacterEntityModel ally in GetAllAlliesOfCharacter(entity, false))
                {
                    GainBlock(ally, CombatLogic.Instance.CalculateBlockGainedByEffect(entity.pManager.guardianAuraStacks, entity, ally));
                }
            }

            // Hateful Aura
            if (entity.pManager.hatefulAuraStacks > 0)
            {
                CharacterEntityModel chosenAlly = null;
                CharacterEntityModel[] allAllies = GetAllAlliesOfCharacter(entity, false).ToArray();
                if (allAllies.Length > 0)
                    chosenAlly = allAllies[RandomGenerator.NumberBetween(0, allAllies.Length - 1)];

                if (chosenAlly != null)
                {
                    // Notification event
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Hateful Aura!"), QueuePosition.Back, 0, 0.5f);

                    // Random ally gains energy
                    PassiveController.Instance.ModifyWrath(chosenAlly.pManager, 1, true, 0.5f);
                }
            }
        }      

        // Overload
        if (entity.pManager.overloadStacks > 0)
        {
            // Notification event
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Overload!"), QueuePosition.Back, 0, 0.5f);

            if (entity.pManager.hurricaneStacks > 0)
            {
                CharacterEntityModel[] enemies = GetAllEnemiesOfCharacter(entity).ToArray();
                VisualEvent batchedEvent = VisualEventManager.Instance.InsertTimeDelayInQueue(0f);

                foreach (CharacterEntityModel enemy in enemies)
                {
                    CharacterEntityView enemyView = enemy.characterEntityView;

                    // VFX Lightning explosion
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateLightningExplosion(enemyView.WorldPosition), QueuePosition.BatchedEvent, 0, 0, EventDetail.None, batchedEvent);

                    // Deal overload damage
                    int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(entity, enemy, DamageType.Magic, entity.pManager.overloadStacks);
                    CombatLogic.Instance.HandleDamage(finalDamageValue, entity, enemy, null, DamageType.Magic, batchedEvent, false, false);
                }
            }
            else
            {
                // Get random enemy
                CharacterEntityModel[] enemies = GetAllEnemiesOfCharacter(entity).ToArray();
                CharacterEntityModel randomEnemy = enemies[RandomGenerator.NumberBetween(0, enemies.Length - 1)];
                CharacterEntityView randomEnemyView = randomEnemy.characterEntityView;

                // Create lightning ball missle
                CoroutineData cData = new CoroutineData();
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.ShootProjectileAtLocation(ProjectileFired.LightningBall1, view.WorldPosition, randomEnemyView.WorldPosition, cData), cData);
                VisualEventManager.Instance.CreateVisualEvent(() => CameraManager.Instance.CreateCameraShake(CameraShakeType.Small));

                // Deal air damage
                int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(entity, randomEnemy, DamageType.Magic, entity.pManager.overloadStacks);
                CombatLogic.Instance.HandleDamage(finalDamageValue, entity, randomEnemy, DamageType.Magic);

                // Brief pause here
                VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
            }

        }

        // DOTS
        // Poisoned
        if (entity.pManager.poisonedStacks > 0 && entity.health > 0)
        {
            // Notification event
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Poisoned!"), QueuePosition.Back, 0, 0.5f);

            // Calculate and deal Poison damage
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(null, entity, DamageType.Physical, entity.pManager.poisonedStacks);
            VisualEventManager.Instance.CreateVisualEvent(() => CameraManager.Instance.CreateCameraShake(CameraShakeType.Small));
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateEffectAtLocation(ParticleEffect.PoisonExplosion, view.WorldPosition));
            CombatLogic.Instance.HandleDamage(finalDamageValue, null, entity, DamageType.Physical, true);           
        }

        // Bleeding
        if (entity.pManager.bleedingStacks > 0 && entity.health > 0)
        {
            // Notification event
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Bleeding!"), QueuePosition.Back, 0, 0.5f);

            // Calculate and deal Poison damage
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(null, entity, DamageType.Physical, entity.pManager.bleedingStacks);
            VisualEventManager.Instance.CreateVisualEvent(() => CameraManager.Instance.CreateCameraShake(CameraShakeType.Small));
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateEffectAtLocation(ParticleEffect.BloodExplosion, view.WorldPosition));
            CombatLogic.Instance.HandleDamage(finalDamageValue, null, entity, DamageType.Physical, true);
        }

        // Burning
        if (entity.pManager.burningStacks > 0 && entity.health > 0)
        {
            // Check demon form passive first
            if(entity.pManager.demonFormStacks > 0)
            {
                // Notification event
                VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Demon Form!"), QueuePosition.Back, 0, 0.5f);

                // Apply block gain
                GainBlock(entity, CombatLogic.Instance.CalculateBlockGainedByEffect(entity.pManager.burningStacks, entity, entity));
            }

            // Otherwise, just handle burning damage normally
            else
            {
                // Notification event
                VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Burning!"), QueuePosition.Back, 0, 0.5f);

                // Calculate and deal Poison damage
                int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(null, entity, DamageType.Magic, entity.pManager.burningStacks);
                VisualEventManager.Instance.CreateVisualEvent(() => CameraManager.Instance.CreateCameraShake(CameraShakeType.Small));
                VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateEffectAtLocation(ParticleEffect.FireExplosion, view.WorldPosition));
                CombatLogic.Instance.HandleDamage(finalDamageValue, null, entity, DamageType.Magic, true);
            }
                   
        }

      

        #endregion


        // Do enemy character exclusive logic
        if (entity.controller == Controller.AI && entity.livingState == LivingState.Alive)
        {
            // Brief pause at the end of enemy action, so player can process whats happened
            VisualEventManager.Instance.InsertTimeDelayInQueue(1f);

            // Set next action + intent
            StartAutoSetEnemyIntentProcess(entity);
        }

        // Activate next character
        if (entity != null &&
           entity.livingState == LivingState.Alive)
        {
            // Set Activated Phase
            SetCharacterActivationPhase(entity, ActivationPhase.NotActivated);

            // Start next entity's activation, or new turn cycle
            ActivationManager.Instance.ActivateNextEntity();
        }
    }
    #endregion

    // Defender Targetting View Logic
    #region
    private void SetEnemyTargettingPathReadyState(CharacterEntityModel enemy, TargettingPathReadyState newState)
    {
        enemy.targettingPathReadyState = newState;
    }
    private void EnableDefenderTargetIndicator(CharacterEntityView view)
    {
        Debug.Log("CharacterEntityController.EnableDefenderTargetIndicator() called...");
        if (view != null)
        {
            view.myTargetIndicator.EnableView();
        }
    }
    private void DisableDefenderTargetIndicator(CharacterEntityView view)
    {
        Debug.Log("CharacterEntityController.DisableDefenderTargetIndicator() called...");
        view.myTargetIndicator.DisableView();
    }
    public void DisableAllDefenderTargetIndicators()
    {
        foreach (CharacterEntityModel defender in AllDefenders)
        {
            DisableDefenderTargetIndicator(defender.characterEntityView);
        }

        foreach (CharacterEntityModel summonedDefender in AllSummonedDefenders)
        {
            DisableDefenderTargetIndicator(summonedDefender.characterEntityView);
        }

        // Disable targeting path lines from all nodes
        foreach (LevelNode node in LevelManager.Instance.AllLevelNodes)
        {
            LevelManager.Instance.DisableAllExtraViews(node);
        }
    }
    #endregion

    // Enemy Intent Logic
    #region
    public void SetAllEnemyIntents()
    {
        Debug.Log("CharacterEntityController.SetAllEnemyIntents() called...");
        foreach (CharacterEntityModel enemy in AllEnemies)
        {
            StartAutoSetEnemyIntentProcess(enemy);
        }
    }
    public void StartAutoSetEnemyIntentProcess(CharacterEntityModel enemy)
    {
        Debug.Log("CharacterEntityController.StartSetEnemyIntentProcess() called...");
        if (CombatLogic.Instance.CurrentCombatState == CombatGameState.CombatActive)
        {
            SetEnemyNextAction(enemy, DetermineNextEnemyAction(enemy));
            SetEnemyTarget(enemy, DetermineTargetOfNextEnemyAction(enemy, enemy.myNextAction));
            UpdateEnemyIntentGUI(enemy);
        }
    }
    public void AutoAquireNewTargetOfCurrentAction(CharacterEntityModel enemy)
    {
        Debug.Log("CharacterEntityController.AutoAquireNewTargetOfCurrentAction() called...");

        // Method is used to find a new target for any enemies action.
        // this should only be called when the previous target for the action
        // is killed, and the enemy needs to find a new target
        if (enemy.currentActionTarget == null ||
            enemy.currentActionTarget.livingState == LivingState.Dead)
        {
            Debug.Log("CharacterEntityController.AutoAquireNewTargetOfCurrentAction() detected character needs a new target, searching...");
            SetEnemyTarget(enemy, DetermineTargetOfNextEnemyAction(enemy, enemy.myNextAction));
            UpdateEnemyIntentGUI(enemy);
        }
    }
    public void UpdateEnemyIntentGUI(CharacterEntityModel enemy)
    {
        Debug.Log("CharacterEntityController.UpdateEnemyIntentGUI() called...");

        // cancel if target of enemy is null, and there are no valid targets left
        // for example, all characters are dead.
        if (enemy.currentActionTarget == null &&
            (enemy.myNextAction.actionType == ActionType.AttackTarget ||
             enemy.myNextAction.actionType == ActionType.DebuffTarget ||
             enemy.myNextAction.actionType == ActionType.DefendTarget ||
              enemy.myNextAction.actionType == ActionType.BuffTarget))
        {
            return;
        }

        // Setup for visual event
        Sprite intentSprite = SpriteLibrary.Instance.GetIntentSpriteFromIntentEnumData(enemy.myNextAction.intentImage);
        string attackDamageString = "";

        // Update intent description
        if(enemy.myNextAction.intentImage == IntentImage.Unknown)
        {
            enemy.characterEntityView.intentPopUpDescriptionText.text = "This character's intent is unknown!...";
        }
        else if (enemy.myNextAction.intentImage == IntentImage.DefendBuff)
        {
            enemy.characterEntityView.intentPopUpDescriptionText.text = "This character intends to gain <color=#F8FF00>Block<color=#FFFFFF> and a buff effect.";
        }
        else if(enemy.myNextAction.actionType == ActionType.BuffAllAllies)
        {
            enemy.characterEntityView.intentPopUpDescriptionText.text = "This character intends to apply a buff to it's allies.";
        }
        else if (enemy.myNextAction.actionType == ActionType.BuffSelf)
        {
            enemy.characterEntityView.intentPopUpDescriptionText.text = "This character intends to apply a buff to itself.";
        }
        else if (enemy.myNextAction.actionType == ActionType.BuffTarget)
        {
            enemy.characterEntityView.intentPopUpDescriptionText.text = "This character intends to apply a buff to <color=#F8FF00>" + enemy.currentActionTarget.myName + "<color=#FFFFFF>.";
        }
        else if (enemy.myNextAction.actionType == ActionType.DebuffAllEnemies)
        {
            enemy.characterEntityView.intentPopUpDescriptionText.text = "This character intends to apply a harmful debuff on ALL enemies.";
        }
        else if (enemy.myNextAction.actionType == ActionType.DebuffTarget)
        {
            enemy.characterEntityView.intentPopUpDescriptionText.text = "This character intends to apply a harmful debuff to <color=#F8FF00>" + enemy.currentActionTarget.myName + "<color=#FFFFFF>.";
        }
        else if (enemy.myNextAction.actionType == ActionType.DefendSelf)
        {
            enemy.characterEntityView.intentPopUpDescriptionText.text = "This character intends to apply <color=#F8FF00>Block<color=#FFFFFF> to itself.";
        }
        else if (enemy.myNextAction.actionType == ActionType.DefendAllAllies)
        {
            enemy.characterEntityView.intentPopUpDescriptionText.text = "This character intends to apply <color=#F8FF00>Block<color=#FFFFFF> to ALL its allies.";
        }
        else if (enemy.myNextAction.actionType == ActionType.HealAllAllies)
        {
            enemy.characterEntityView.intentPopUpDescriptionText.text = "This character intends to <color=#F8FF00>Heal<color=#FFFFFF> its allies";
        }
        else if (enemy.myNextAction.actionType == ActionType.DefendTarget)
        {
            enemy.characterEntityView.intentPopUpDescriptionText.text = "This character intends to apply <color=#F8FF00>Block<color=#FFFFFF> to <color=#F8FF00>" + enemy.currentActionTarget.myName + "<color=#FFFFFF>.";
        }
        else if (enemy.myNextAction.actionType == ActionType.Sleep)
        {
            enemy.characterEntityView.intentPopUpDescriptionText.text = "This character is asleep and unable to take action...";
        }
        else if (enemy.myNextAction.actionType == ActionType.SummonCreature)
        {
            enemy.characterEntityView.intentPopUpDescriptionText.text = "This character intends to summon reinforcements";
        }

        // if attacking, calculate + enable + set damage value text
        if (enemy.myNextAction.actionType == ActionType.AttackTarget ||
            enemy.myNextAction.actionType == ActionType.AttackAllEnemies)
        {
            // Find the attack action effect in the actions lists of effects
            EnemyActionEffect effect = null;
            foreach (EnemyActionEffect effectt in enemy.myNextAction.actionEffects)
            {
                if (effectt.actionType == ActionType.AttackTarget ||
                    effectt.actionType == ActionType.AttackAllEnemies)
                {
                    effect = effectt;
                    break;
                }
            }

            CharacterEntityModel target = enemy.currentActionTarget;

            // Calculate damage to display
            DamageType damageType = CombatLogic.Instance.GetFinalFinalDamageTypeOfAttack(enemy, null, null, effect);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(enemy, target, damageType, effect.baseDamage, effect);

            // Find looping effect data (if it exists)
            EnemyActionEffect eae = null;
            foreach(EnemyActionEffect e in enemy.myNextAction.actionEffects)
            {
                if(e.actionType == ActionType.AttackAllEnemies ||
                    e.actionType == ActionType.AttackTarget)
                {
                    eae = e;
                    break;
                }
            }

            if (eae != null && eae.effectLoops > 1)
            {
                attackDamageString = finalDamageValue.ToString() + " x " + eae.effectLoops.ToString();
            }
            else
            {
                attackDamageString = finalDamageValue.ToString();
            }

            // Update description text
            if(enemy.myNextAction.actionType == ActionType.AttackTarget)
            {
                if(eae != null && eae.effectLoops > 1)
                {
                    enemy.characterEntityView.intentPopUpDescriptionText.text = "This character intends to attack <color=#F8FF00>" + target.myName +
                   "<color=#FFFFFF> for <color=#92E0FF>" + finalDamageValue.ToString() + "<color=#FFFFFF> damage <color=#F8FF00>" + eae.effectLoops.ToString() + "<color=#FFFFFF> times";
                }
                else
                {
                    enemy.characterEntityView.intentPopUpDescriptionText.text = "This character intends to attack <color=#F8FF00>" + target.myName +
                   "<color=#FFFFFF> for <color=#92E0FF>" + attackDamageString + "<color=#FFFFFF> damage.";
                }
            }

            else if (enemy.myNextAction.actionType == ActionType.AttackAllEnemies)
            {
                if (eae != null && eae.effectLoops > 1)
                {
                    enemy.characterEntityView.intentPopUpDescriptionText.text = "This character intends to attack ALL enemies for <color=#92E0FF>" + attackDamageString + 
                        "<color=#FFFFFF> damage <color =#F8FF00>" + eae.effectLoops.ToString() + "<color=#FFFFFF> times";
                }
                else
                {
                    enemy.characterEntityView.intentPopUpDescriptionText.text = "This character intends to attack ALL enemies for <color=#92E0FF>" + attackDamageString + "<color=#FFFFFF> damage.";
                }
            }
        }

        // Enable targetting path visibility
        VisualEventManager.Instance.CreateVisualEvent(() => SetEnemyTargettingPathReadyState(enemy, TargettingPathReadyState.Ready));

        // Create Visual event        
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateEnemyIntentGUIVisualEvent(enemy.characterEntityView.myIntentViewModel, intentSprite, attackDamageString));

    }
    private void SetEnemyNextAction(CharacterEntityModel enemy, EnemyAction action)
    {
        Debug.Log("CharacterEntityController.SetEnemyNextAction() called, setting action '" + action.actionName +
            "' as next action for enemy '" + enemy.myName + "'.");

        enemy.myNextAction = action;
    }
    private void SetEnemyTarget(CharacterEntityModel enemy, CharacterEntityModel target)
    {
        string targetName = "NO TARGET";
        if (target != null)
        {
            targetName = target.myName;
        }

        Debug.Log("CharacterEntityController.SetEnemyTarget() called, setting '" + targetName +
            "' as target for '" + enemy.myName + "'.");

        enemy.currentActionTarget = target;
    }

    // Visual Events 
    #region
    public void FadeInCharacterUICanvas(CharacterEntityView view, CoroutineData cData)
    {
        StartCoroutine(FadeInCharacterUICanvasCoroutine(view, cData));
    }
    private IEnumerator FadeInCharacterUICanvasCoroutine(CharacterEntityView view, CoroutineData cData)
    {
        Debug.Log("CharacterEntityController.FadeInCharacterUICanvasCoroutine() called...");

        view.uiCanvasParent.SetActive(true);
        view.uiCanvasCg.alpha = 0;
        float uiFadeSpeed = 20f;

        while (view.uiCanvasCg.alpha < 1)
        {
            view.uiCanvasCg.alpha += 0.1f * uiFadeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Resolve
        if (cData != null)
        {
            cData.MarkAsCompleted();
        }

    }
    public void FadeOutCharacterUICanvas(CharacterEntityView view, CoroutineData cData)
    {
        StartCoroutine(FadeOutCharacterUICanvasCoroutine(view, cData));
    }
    private IEnumerator FadeOutCharacterUICanvasCoroutine(CharacterEntityView view, CoroutineData cData)
    {
        view.uiCanvasParent.SetActive(true);
        view.uiCanvasCg.alpha = 1;
        float uiFadeSpeed = 20f;

        while (view.uiCanvasCg.alpha > 0)
        {
            view.uiCanvasCg.alpha -= 0.1f * uiFadeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        view.uiCanvasParent.SetActive(false);

        // Resolve
        if (cData != null)
        {
            cData.MarkAsCompleted();
        }

    }
    public void FadeOutCharacterWorldCanvas(CharacterEntityView view, CoroutineData cData, float fadeSpeed = 1f)
    {
        StartCoroutine(FadeOutCharacterWorldCanvasCoroutine(view, cData, fadeSpeed));
    }
    private IEnumerator FadeOutCharacterWorldCanvasCoroutine(CharacterEntityView view, CoroutineData cData, float fadeSpeed)
    {
        view.worldSpaceCanvasParent.gameObject.SetActive(true);
        view.worldSpaceCG.alpha = 1;

        view.worldSpaceCG.DOFade(0, fadeSpeed);

        yield return new WaitForSeconds(fadeSpeed);
        view.worldSpaceCanvasParent.gameObject.SetActive(false);

        if (cData != null)
        {
            cData.MarkAsCompleted();
        }

        /*
        float uiFadeSpeed = 20f;

        while (view.worldSpaceCG.alpha > 0)
        {
            view.worldSpaceCG.alpha -= 0.1f * uiFadeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        view.worldSpaceCanvasParent.gameObject.SetActive(false);
        
        // Resolve
        if (cData != null)
        {
            cData.MarkAsCompleted();
        }
        */

    }
    public void FadeInCharacterWorldCanvas(CharacterEntityView view, CoroutineData cData, float fadeSpeed = 1f)
    {
        StartCoroutine(FadeInCharacterWorldCanvasCoroutine(view, cData, fadeSpeed));
    }
    private IEnumerator FadeInCharacterWorldCanvasCoroutine(CharacterEntityView view, CoroutineData cData, float fadeSpeed)
    {
        view.worldSpaceCanvasParent.gameObject.SetActive(true);
        view.worldSpaceCG.alpha = 0;

        view.worldSpaceCG.DOFade(1, fadeSpeed);

        yield return new WaitForSeconds(fadeSpeed);

        if (cData != null)
        {
            cData.MarkAsCompleted();
        }
    }
    private void UpdateEnemyIntentGUIVisualEvent(IntentViewModel intentView, Sprite intentSprite, string attackDamageString)
    {        
        // Disable text view
        intentView.valueText.gameObject.SetActive(false);

        // Start fade in effect        
        FadeInIntentViewModel(intentView);

        // Set intent image
        intentView.SetIntentSprite(intentSprite);

        if (attackDamageString != "")
        {
            // Enable attack damage value text, if we have value to show
            intentView.valueText.gameObject.SetActive(true);
            intentView.valueText.text = attackDamageString;
        }
    }
    public void FadeInIntentViewModel(IntentViewModel ivm)
    {
        StartCoroutine(FadeInIntentViewModeloroutine(ivm));
    }
    private IEnumerator FadeInIntentViewModeloroutine(IntentViewModel ivm)
    {
        ivm.visualParent.SetActive(true);
        ivm.PlayFloatAnimation();
        ivm.myCg.alpha = 0;

        while (ivm.myCg.alpha < 1)
        {
            ivm.myCg.alpha += 1 * Time.deltaTime;
            yield return null;
        }
    }

    #endregion

    // Color + Highlighting 
    #region
    public void SetCharacterColor(CharacterEntityView view, Color newColor)
    {
        // Prevent this function from interrupting death anim fade out
        if (view.character.livingState == LivingState.Dead)
        {
            return;
        }

        if (view.entityRenderer != null)
        {
            view.entityRenderer.Color = new Color(newColor.r, newColor.g, newColor.b, view.entityRenderer.Color.a);
        }

    }
    private void SetCharacterModelSize(CharacterEntityView view, CharacterModelSize size)
    {
        if(size == CharacterModelSize.Small)
        {
            // Resize model
            view.ucmSizingParent.transform.localScale = new Vector3(0.9f, 0.9f, 1f);

            // Re-position model
            view.ucmSizingParent.transform.localPosition = new Vector3(0f, -0.025f, 0f);

            // scale shadow
            //Transform t = view.ucmShadowParent.transform;
           // t.localScale = new Vector3(0.0045f, 0.0045f, 0.5f);

            // Move intent view model
            view.myIntentViewModel.sizingParent.transform.localPosition = new Vector3(0, -0.2f, 0);
        }
        else if (size == CharacterModelSize.Normal)
        {
            // Resize model
            view.ucmSizingParent.transform.localScale = new Vector3(1, 1, 1f);

            // Re-position model
            view.ucmSizingParent.transform.localPosition = new Vector3(0f, 0f, 0f);

            // scale shadow
            //Transform t = view.ucmShadowParent.transform;
            //t.localScale = new Vector3(0.005f, 0.005f, 0.5f);

            // Move intent view model
            view.myIntentViewModel.sizingParent.transform.localPosition = new Vector3(0, 0, 0);
        }
        else if (size == CharacterModelSize.Large)
        {
            // Resize model
            view.ucmSizingParent.transform.localScale = new Vector3(1.2f, 1.2f, 1f);

            // Re-position model
            view.ucmSizingParent.transform.localPosition = new Vector3(0.05f, 0.05f, 0f);

            // scale shadow
           // Transform t = view.ucmShadowParent.transform;
           // t.localScale = new Vector3(0.006f, 0.006f, 0.5f);

            // Move intent view model
            view.myIntentViewModel.sizingParent.transform.localPosition = new Vector3(0, 0.2f, 0);
        }
        else if (size == CharacterModelSize.Massive)
        {
            // Resize model
            view.ucmSizingParent.transform.localScale = new Vector3(1.4f, 1.4f, 1f);

            // Re-position model
            view.ucmSizingParent.transform.localPosition = new Vector3(0.1f, 0.1f, 0f);

            // scale shadow
           // Transform t = view.ucmShadowParent.transform;
            //t.localScale = new Vector3(0.007f, 0.007f, 0.5f);

            // Move intent view model
            view.myIntentViewModel.sizingParent.transform.localPosition = new Vector3(0, 0.4f, 0);
        }
    }

    #endregion

    // Trigger Animations
    #region
    public void TriggerShootProjectileAnimation(CharacterEntityView view)
    {
        AudioManager.Instance.StopSound(Sound.Character_Footsteps);
        view.ucmAnimator.SetTrigger("Melee Attack");
    }
    public void TriggerMeleeAttackAnimation(CharacterEntityView view, CoroutineData cData)
    {
        AudioManager.Instance.StopSound(Sound.Character_Footsteps);
        StartCoroutine(TriggerMeleeAttackAnimationCoroutine(view, cData));
    }
    private IEnumerator TriggerMeleeAttackAnimationCoroutine(CharacterEntityView view, CoroutineData cData)
    {
        view.ucmAnimator.SetTrigger("Melee Attack");
        float startX = view.WorldPosition.x;
        float forwardPos = 0;
        float moveSpeedTime = 0.25f;
        float distance = 0.75f;

        CharacterEntityModel model = view.character;
        if (model != null)
        {
            if (model.allegiance == Allegiance.Player)
            {
                forwardPos = view.WorldPosition.x + distance;
            }
            else if (model.allegiance == Allegiance.Enemy)
            {
                forwardPos = view.WorldPosition.x - distance;
            }

            // slight movement forward
            view.ucmMovementParent.transform.DOMoveX(forwardPos, moveSpeedTime);
            yield return new WaitForSeconds(moveSpeedTime / 2);

            if (cData != null)
            {
                cData.MarkAsCompleted();
            }

            yield return new WaitForSeconds(moveSpeedTime / 2);

            // move back to start pos
            view.ucmMovementParent.transform.DOMoveX(startX, moveSpeedTime);
            yield return new WaitForSeconds(moveSpeedTime);
        }

    }
    public void TriggerAoeMeleeAttackAnimation(CharacterEntityView view)
    {
        view.ucmAnimator.SetTrigger("Melee Attack");
        AudioManager.Instance.StopSound(Sound.Character_Footsteps);
    }
    public void PlayIdleAnimation(CharacterEntityView view)
    {
        view.ucmAnimator.SetTrigger("Idle");
        AudioManager.Instance.StopSound(Sound.Character_Footsteps);
    }
    public void PlayResurrectAnimation(CharacterEntityView view)
    {
        view.ucmAnimator.SetTrigger("Resurrect");
    }
    public void PlayLayDeadAnimation(CharacterEntityView view)
    {
        view.ucmAnimator.SetTrigger("Lay Dead");
    }
    public void PlaySkillAnimation(CharacterEntityView view)
    {
        if (view)
        {
            view.ucmAnimator.SetTrigger("Skill Two");
            AudioManager.Instance.StopSound(Sound.Character_Footsteps);
        }

    }
    public void PlayMoveAnimation(CharacterEntityView view)
    {
        AudioManager.Instance.PlaySound(Sound.Character_Footsteps);
        view.ucmAnimator.SetTrigger("Move");
    }
    public void PlayWalkAnimation(CharacterEntityView view)
    {
        view.ucmAnimator.SetTrigger("Walk");
    }
    public void PlayHurtAnimation(CharacterEntityView view)
    {
        AudioManager.Instance.StopSound(Sound.Character_Footsteps);
        view.ucmAnimator.SetTrigger("Hurt");
    }
    public void PlayDeathAnimation(CharacterEntityView view)
    {
        AudioManager.Instance.StopSound(Sound.Character_Footsteps);
        view.ucmAnimator.SetTrigger("Die");
    }
    public void PlayShootBowAnimation(CharacterEntityView view, CoroutineData cData)
    {
        Debug.Log("CharacterEntityController.PlayRangedAttackAnimation() called...");
        AudioManager.Instance.StopSound(Sound.Character_Footsteps);
        AudioManager.Instance.PlaySoundPooled(Sound.Character_Draw_Bow);
        StartCoroutine(PlayShootBowAnimationCoroutine(view, cData));
    }
    private IEnumerator PlayShootBowAnimationCoroutine(CharacterEntityView view, CoroutineData cData)
    {
        view.ucmAnimator.SetTrigger("Shoot Bow");
        AudioManager.Instance.StopSound(Sound.Character_Footsteps);
        yield return new WaitForSeconds(0.5f);

        // Resolve
        if (cData != null)
        {
            cData.MarkAsCompleted();
        }
    }
    #endregion

    // Mouse + Input Logic
    #region
    public void OnCharacterMouseEnter(CharacterEntityView view)
    {
        Debug.Log("CharacterEntityController.OnCharacterMouseOver() called...");

        if (view.eventSetting != EventSetting.Combat)
        {
            return;
        }

        // prevent clicking through an active UI screen
        if (CardController.Instance.DiscoveryScreenIsActive ||
            CardController.Instance.ChooseCardScreenIsActive ||
            MainMenuController.Instance.AnyMenuScreenIsActive() ||
            view.blockMouseOver)
        {
            return;
        }

        // Mouse over SFX
        AudioManager.Instance.PlaySoundPooled(Sound.GUI_Button_Mouse_Over);

        // Cancel this if character is dead
        if (view.character == null ||
            view.character.livingState == LivingState.Dead)
        {
            // Prevents GUI bugs when mousing over an enemy that is dying
            DisableAllDefenderTargetIndicators();
            LevelManager.Instance.SetMouseOverViewState(view.character.levelNode, false);
            return;
        }        

        // Enable activation window glow + node glow
        if(view.myActivationWindow != null)
        {
            view.myActivationWindow.myGlowOutline.SetActive(true);
        }
       
        LevelManager.Instance.SetMouseOverViewState(view.character.levelNode, true);

        // Set character highlight color
        SetCharacterColor(view, highlightColour);

        // Update card being dragged text
        if (Draggable.DraggingThis != null &&
            Draggable.DraggingThis.Da is DragSpellOnTarget && 
            view.character != null)
        {
            CardController.Instance.AutoUpdateCardDescriptionText(Draggable.DraggingThis.Da.CardVM().card, view.character);
            CardController.Instance.SetCardViewModelDescriptionText(Draggable.DraggingThis.Da.CardVM(), TextLogic.ConvertCustomStringListToString(Draggable.DraggingThis.Da.CardVM().card.cardDescriptionTwo));
        }

        // AI + Enemy exclusive logic
        if (view.character.controller == Controller.AI)
        {
            CharacterEntityModel enemy = view.character;

            // Clear previous pathing and targetting views
            DisableAllDefenderTargetIndicators();
            DottedLine.Instance.DestroyAllPaths();

            // Single target effect
            if (enemy.currentActionTarget != null && 
                enemy.currentActionTarget.allegiance == Allegiance.Player &&
                enemy.targettingPathReadyState == TargettingPathReadyState.Ready &&
                enemy.levelNode != null && 
                enemy.livingState == LivingState.Alive)
            {
                // Enable targetting square over character
                EnableDefenderTargetIndicator(enemy.currentActionTarget.characterEntityView);

                // Draw targetting path between character
                LevelManager.Instance.ConnectTargetPathToTargetNode(enemy.levelNode, enemy.currentActionTarget.levelNode);
            }

            // AoE target effect
            else if (enemy.targettingPathReadyState == TargettingPathReadyState.Ready &&
                (enemy.myNextAction.actionType == ActionType.AttackAllEnemies || enemy.myNextAction.actionType == ActionType.DebuffAllEnemies) &&
                enemy.levelNode != null && 
                enemy.livingState == LivingState.Alive)
            {
                // get all enemies of enemy
                foreach(CharacterEntityModel enemyCharacter in GetAllEnemiesOfCharacter(enemy))
                {
                    // Enable targetting square over character
                    EnableDefenderTargetIndicator(enemyCharacter.characterEntityView);

                    // Draw targetting path between character
                    LevelManager.Instance.ConnectTargetPathToTargetNode(enemy.levelNode, enemyCharacter.levelNode);
                }                
            }
        }

    }
    public void OnCharacterMouseExit(CharacterEntityView view)
    {
        Debug.Log("CharacterEntityController.OnCharacterMouseExit() called...");

        if(view.eventSetting != EventSetting.Combat)
        {
            return;
        }

        // prevent clicking through an active UI screen
        if (CardController.Instance.DiscoveryScreenIsActive || 
            CardController.Instance.ChooseCardScreenIsActive ||
             MainMenuController.Instance.AnyMenuScreenIsActive() ||
             view.blockMouseOver)
        {
            return;
        }

        // Cancel this if character is dead
        if (view.character.livingState == LivingState.Dead)
        {
            // Prevents GUI bugs when mousing over an enemy that is dying
            DisableAllDefenderTargetIndicators();

            // Disable all node mouse stats
            foreach (LevelNode node in LevelManager.Instance.AllLevelNodes)
            {
                LevelManager.Instance.SetMouseOverViewState(node, false);
            }

            return;
        }

        // Disable activation window glow
        if (view.myActivationWindow != null)
        {
            view.myActivationWindow.myGlowOutline.SetActive(false);
        }

        // Update card being dragged text, reset to targetless calculation
        if (Draggable.DraggingThis != null &&
            Draggable.DraggingThis.Da is DragSpellOnTarget)
        {
            CardController.Instance.AutoUpdateCardDescriptionText(Draggable.DraggingThis.Da.CardVM().card, null);
            CardController.Instance.SetCardViewModelDescriptionText(Draggable.DraggingThis.Da.CardVM(), TextLogic.ConvertCustomStringListToString(Draggable.DraggingThis.Da.CardVM().card.cardDescriptionTwo));
        }

        // Do character vm stuff
        if (view.character != null)
        {
            // Set character highlight color
            SetCharacterColor(view, normalColour);

            // Set character's level node mouse over state
            if (view.character.levelNode != null)
            {
                LevelManager.Instance.SetMouseOverViewState(view.character.levelNode, false);
            }
        }


        // AI + Enemy exclusive logic
        if (view.character.controller == Controller.AI)
        {
            CharacterEntityModel enemy = view.character;
            DisableAllDefenderTargetIndicators();

            if (enemy.livingState == LivingState.Alive && enemy.levelNode != null)
            {
                LevelManager.Instance.SetLineViewState(enemy.levelNode, false);
            }
        }

    }

    #endregion   

    #endregion

    // Determine and Execute Enemy Actions
    #region
    private void AddActionToEnemyPastActionsLog(CharacterEntityModel enemy, EnemyAction action)
    {
        enemy.myPreviousActionLog.Add(action);
    }
    private bool DoesEnemyActionMeetItsRequirements(EnemyAction enemyAction, CharacterEntityModel enemy)
    {
        Debug.Log("CharacterEntityController.DoesEnemyActionMeetItsRequirements() called, checking action '" + enemyAction.actionName +
            "' by enemy " + enemy.myName);

        List<bool> checkResults = new List<bool>();
        bool boolReturned = false;

        foreach (ActionRequirement ar in enemyAction.actionRequirements)
        {
            Debug.Log("Checking requirement of type: " + ar.requirementType.ToString());

            // Check is turn requirement
            if (ar.requirementType == ActionRequirementType.IsTurn &&
                ActivationManager.Instance.CurrentTurn != ar.requirementTypeValue)
            {
                Debug.Log(enemyAction.actionName + " failed 'IsTurn' requirement");
                checkResults.Add(false);
            }

            // Check enough allies alive
            if (ar.requirementType == ActionRequirementType.AtLeastXAlliesAlive &&
                GetAllAlliesOfCharacter(enemy, false).Count < ar.requirementTypeValue)
            {
                Debug.Log(enemyAction.actionName + " failed 'AtLeastXAlliesAlive' requirement");
                checkResults.Add(false);
            }

            // Check allies wounded
            if (ar.requirementType == ActionRequirementType.AtLeastOneAllyWounded)
            {
                Debug.Log(enemyAction.actionName + " failed 'AtLeastOneAllyWounded' requirement");

                bool foundWounded = false;
                foreach (CharacterEntityModel ally in GetAllAlliesOfCharacter(enemy, false))
                {
                    if(ally.health < ally.MaxHealthTotal)
                    {
                        foundWounded = true;
                        break;
                    }
                }             
                
                if(!foundWounded)
                    checkResults.Add(false);
            }

            // Check availble summoning spots
            if (ar.requirementType == ActionRequirementType.AtLeastXAvailableNodes &&
                LevelManager.Instance.GetAllAvailableEnemyNodes().Count < ar.requirementTypeValue)
            {
                Debug.Log(enemyAction.actionName + " failed 'AtLeastXAvailableNodes' requirement");
                checkResults.Add(false);
            }

            // Check havent used abilty for X turns
            if (ar.requirementType == ActionRequirementType.HaventUsedActionInXTurns)
            {
                if (enemy.myPreviousActionLog.Count < ar.requirementTypeValue)
                {
                    Debug.Log(enemyAction.actionName + " failed 'HaventUsedActionInXTurns' requirement");
                    checkResults.Add(false);
                }
                else
                {
                    int loops = ar.requirementTypeValue;
                    int index = enemy.myPreviousActionLog.Count - 1;

                    while (loops > 0)
                    {
                        if (enemy.myPreviousActionLog[index].actionName == enemyAction.actionName)
                        {
                            Debug.Log(enemyAction.actionName + " failed 'HaventUsedActionInXTurns' requirement");
                            checkResults.Add(false);
                            break;
                        }

                        index--;
                        loops--;
                    }
                }
            }

            // Check is more than turn
            if (ar.requirementType == ActionRequirementType.IsMoreThanTurn &&
               ActivationManager.Instance.CurrentTurn <= ar.requirementTypeValue)
            {
                Debug.Log(enemyAction.actionName + " failed 'IsMoreThanTurn' requirement, current turn is " + ActivationManager.Instance.CurrentTurn.ToString() +
                    ", and requirement is " + ar.requirementTypeValue.ToString());
                checkResults.Add(false);
            }

            // Check ActivatedXTimesOrMore
            if (ar.requirementType == ActionRequirementType.ActivatedXTimesOrMore &&
               enemy.myPreviousActionLog.Count < ar.requirementTypeValue)
            {
                Debug.Log(enemyAction.actionName + " failed 'ActivatedXTimesOrMore' requirement, has activated " + enemy.myPreviousActionLog.Count.ToString() +
                    " times, and requirement is " + ar.requirementTypeValue.ToString());
                checkResults.Add(false);
            }

            // Check ActivatedXTimesOrLess
            if (ar.requirementType == ActionRequirementType.ActivatedXTimesOrLess &&
               enemy.myPreviousActionLog.Count > ar.requirementTypeValue)
            {
                Debug.Log(enemyAction.actionName + " failed 'ActivatedXTimesOrMore' requirement, has activated " + enemy.myPreviousActionLog.Count.ToString() +
                    " times, and requirement is " + ar.requirementTypeValue.ToString());
                checkResults.Add(false);
            }

            // Check HasPassive
            if (ar.requirementType == ActionRequirementType.HasPassiveTrait &&
                PassiveController.Instance.IsEntityAffectedByPassive(enemy.pManager, ar.passiveRequired.passiveName) == false)
            {
                Debug.Log(enemyAction.actionName + " failed 'HasPassive' requirement");
                checkResults.Add(false);
            }

        }

        if (checkResults.Contains(false))
        {
            boolReturned = false;
        }
        else
        {
            boolReturned = true;
        }

        return boolReturned;
    }
    private EnemyAction DetermineNextEnemyAction(CharacterEntityModel enemy)
    {
        Debug.Log("CharacterEntityController.DetermineNextEnemyAction() called for enemy: " + enemy.myName);

        List<EnemyAction> viableNextMoves = new List<EnemyAction>();
        EnemyAction actionReturned = null;
        bool foundForcedAction = false;

        // if enemy only knows 1 action, just set that
        if (enemy.enemyData.allEnemyActions.Count == 1)
        {
            actionReturned = enemy.enemyData.allEnemyActions[0];
            Debug.Log("EnemyController.DetermineNextEnemyAction() returning " + actionReturned.actionName);
            return actionReturned;
        }

        // Check if an action is forced on activation one
        foreach (EnemyAction ea in enemy.enemyData.allEnemyActions)
        {
            if (ea.doThisOnFirstActivation &&
                enemy.myPreviousActionLog.Count == 0)
            {
                actionReturned = ea;
                Debug.Log("EnemyController.DetermineNextEnemyAction() returning " + actionReturned.actionName);
                return actionReturned;
            }
        }

        // Determine which actions are viable
        foreach (EnemyAction enemyAction in enemy.enemyData.allEnemyActions)
        {
            List<bool> checkResults = new List<bool>();

            // Check consecutive action use condition
            if (enemyAction.canBeConsecutive == false &&
                enemy.myPreviousActionLog.Count > 0 &&
                enemy.myPreviousActionLog.Last() == enemyAction)
            {
                Debug.Log(enemyAction.actionName + " failed to pass consecutive check: action was performed on the previous turn and cannot be performed consecutively");
                checkResults.Add(false);
            }
            else
            {
                Debug.Log(enemyAction.actionName + " passed consecutive check");
                checkResults.Add(true);
            }

            // Check conditional requirements
            if (enemyAction.actionRequirements.Count > 0)
            {
                if (DoesEnemyActionMeetItsRequirements(enemyAction, enemy) == false)
                {
                    Debug.Log(enemyAction.actionName + " failed its RequirementType checks");
                    checkResults.Add(false);
                }
            }
            else if (enemyAction.actionRequirements.Count == 0)
            {
                checkResults.Add(true);
            }

            // Did the action fail any checks?
            if (checkResults.Contains(false) == false)
            {
                // It didn't, this is a valid action
                Debug.Log(enemyAction.actionName + " passed all validity checks");
                viableNextMoves.Add(enemyAction);
            }
        }

        // Check if any actions should be forced into being used
        foreach (EnemyAction enemyAction in enemy.enemyData.allEnemyActions)
        {
            if (DoesEnemyActionMeetItsRequirements(enemyAction, enemy) &&
                enemyAction.prioritiseWhenRequirementsMet)
            {
                Debug.Log("Detected that " + enemyAction.actionName + " meets its requirements AND" +
                    " is marked as priority when requirements met, setting this as next action...");
                actionReturned = enemyAction;
                foundForcedAction = true;
                break;
            }
        }

        // Randomly decide which next action to take
        if (actionReturned == null && foundForcedAction == false)
        {
            if (viableNextMoves.Count == 1)
            {
                actionReturned = viableNextMoves[0];
            }
            else
            {
                actionReturned = viableNextMoves[UnityEngine.Random.Range(0, viableNextMoves.Count)];
            }
        }

        Debug.Log("EnemyController.DetermineNextEnemyAction() returning " + actionReturned.actionName);
        return actionReturned;
    }
    private CharacterEntityModel DetermineTargetOfNextEnemyAction(CharacterEntityModel enemy, EnemyAction action)
    {
        Debug.Log("CharacterEntityController.DetermineTargetOfNextEnemyAction() called for enemy: " + enemy.myName);

        CharacterEntityModel targetReturned = null;

        // Check taunt first
        if (enemy.pManager.tauntStacks > 0 &&
            enemy.pManager.myTaunter != null &&
            enemy.pManager.myTaunter.livingState == LivingState.Alive &&
            action.actionType == ActionType.AttackTarget)
        {
            targetReturned = enemy.pManager.myTaunter;
        }

        else if (action.actionType == ActionType.AttackTarget ||
            action.actionType == ActionType.DebuffTarget ||
            action.actionType == ActionType.AddCardToTargetCardCollection)
        {
            CharacterEntityModel[] enemies = GetAllEnemiesOfCharacter(enemy).ToArray();

            if (enemies.Length > 1)
            {
                targetReturned = enemies[UnityEngine.Random.Range(0, enemies.Length)];
            }
            else if (enemies.Length == 1)
            {
                targetReturned = enemies[0];
            }

        }
        else if (action.actionType == ActionType.DefendTarget ||
                 action.actionType == ActionType.BuffTarget)
        {
            // Get a valid target
            CharacterEntityModel[] allies = GetAllAlliesOfCharacter(enemy, false).ToArray();

            // if no valid allies, target self
            if (allies.Length == 0)
            {
                targetReturned = enemy;
            }

            // randomly chose enemy from remaining valid choices
            else if (allies.Length > 0)
            {
                targetReturned = allies[UnityEngine.Random.Range(0, allies.Length)];
            }
            else
            {
                // set self as target, if no valid allies
                targetReturned = enemy;
            }
        }

        if (targetReturned != null)
        {
            Debug.Log("CharacterEntityController.DetermineTargetOfNextEnemyAction() setting "
           + targetReturned + " as the target of action " + action.actionName + " by " + enemy.myName);
        }

        return targetReturned;
    }
    private void ExecuteEnemyNextAction(CharacterEntityModel enemy)
    {
        Debug.Log("CharacterEntityController.ExecuteEnemyNextActionCoroutine() called...");

        // Setup
        EnemyAction nextAction = enemy.myNextAction;
        string notificationName = enemy.myNextAction.actionName;

        // Disable targetting path visibility
        VisualEventManager.Instance.CreateVisualEvent(() => SetEnemyTargettingPathReadyState(enemy, TargettingPathReadyState.NotReady));

        // Status Notification
        VisualEventManager.Instance.CreateVisualEvent(() =>
        VisualEffectManager.Instance.CreateStatusEffect(enemy.characterEntityView.WorldPosition, notificationName), QueuePosition.Back, 0, 1f);

        // Trigger and resolve all effects of the action        
        for (int i = 0; i < nextAction.actionLoops; i++)
        {
            if (enemy != null && enemy.livingState == LivingState.Alive)
            {
                foreach (EnemyActionEffect effect in nextAction.actionEffects)
                {
                    TriggerEnemyActionEffect(enemy, effect);
                }
            }
        }

        // POST ACTION STUFF
        // Record action
        AddActionToEnemyPastActionsLog(enemy, nextAction);

        // If character moved off node, move back after all card effects resolved
        if (enemy.hasMovedOffStartingNode && enemy.livingState == LivingState.Alive)
        {
            enemy.hasMovedOffStartingNode = false;
            CoroutineData cData = new CoroutineData();
            LevelNode node = enemy.levelNode;
            VisualEventManager.Instance.CreateVisualEvent(() => MoveEntityToNodeCentre(enemy.characterEntityView, node, cData), cData, QueuePosition.Back, 0.3f, 0);
        }

        // Brief pause at the of all effects
        VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
    }
    private void TriggerEnemyActionEffect(CharacterEntityModel enemy, EnemyActionEffect effect)
    {
        Debug.Log("CharacterEntityController.TriggerEnemyActionEffect() called on enemy " + enemy.myName);

        for (int i = 0; i < effect.effectLoops; i++)
        {
            // Cache refs for visual events
            CharacterEntityModel target = enemy.currentActionTarget;

            // if invalid targetting issues occured before triggering event, return
            if ((target != null && target.livingState == LivingState.Dead) ||
                ((effect.actionType == ActionType.AttackTarget ||
                effect.actionType == ActionType.DebuffTarget ||
                effect.actionType == ActionType.AddCardToTargetCardCollection) && target == enemy))
            {
                return;
            }

            // Prevent targetting self with harmful effects
            if ((effect.actionType == ActionType.AttackTarget ||
                effect.actionType == ActionType.DebuffTarget ||
                effect.actionType == ActionType.AddCardToTargetCardCollection) &&
                target == null)
            {
                return;
            }

            // If summoning allies, but there are no available nodes, cancel
            if (effect.actionType == ActionType.SummonCreature &&
                LevelManager.Instance.GetNextAvailableEnemyNode() == null)
            {
                return;
            }

            // If no target for buff/defend ally effect, set self as target
            if ((effect.actionType == ActionType.DefendTarget ||
                effect.actionType == ActionType.BuffTarget) &&
                target == null)
            {
                target = enemy;
            }

            // Trigger starting visual events
            foreach (AnimationEventData vEvent in effect.visualEventsOnStart)
            {
                AnimationEventController.Instance.PlayAnimationEvent(vEvent, enemy, target);
            }


            // RESOLVE EFFECT LOGIC START!
            // Execute effect based on effect type
            // Attack Target
            if (effect.actionType == ActionType.AttackTarget)
            {
                if (target != null &&
                     target.livingState == LivingState.Alive)
                {
                    // Calculate damage
                    DamageType damageType = CombatLogic.Instance.GetFinalFinalDamageTypeOfAttack(enemy, null, null, effect);
                    int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(enemy, target, damageType, effect.baseDamage, effect);

                    // Start damage sequence
                    CombatLogic.Instance.HandleDamage(finalDamageValue, enemy, target, effect, damageType);
                }
            }

            // Attack All Enemies
            else if (effect.actionType == ActionType.AttackAllEnemies)
            {
                VisualEvent batchedEvent = VisualEventManager.Instance.InsertTimeDelayInQueue(0f);

                foreach (CharacterEntityModel enemyCharacter in GetAllEnemiesOfCharacter(enemy))
                {
                    if (enemyCharacter != null &&
                     enemyCharacter.livingState == LivingState.Alive)
                    {
                        // Calculate damage
                        DamageType damageType = CombatLogic.Instance.GetFinalFinalDamageTypeOfAttack(enemy, null, null, effect);
                        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(enemy, enemyCharacter, damageType, effect.baseDamage, effect);

                        // Start damage sequence
                        CombatLogic.Instance.HandleDamage(finalDamageValue, enemy, enemyCharacter, effect, damageType, batchedEvent);
                    }
                }
            }

            // Summon Creature
            if (effect.actionType == ActionType.SummonCreature)
            {
                // get next available node
                LevelNode node = LevelManager.Instance.GetNextAvailableEnemyNode();

                if (!node)
                {
                    return;
                }

                // Create enemy GO + data, set intent
                CharacterEntityModel newEnemy = CreateEnemyCharacter(effect.characterSummoned, node);
                StartAutoSetEnemyIntentProcess(newEnemy);

                // Disable activation window until ready
                CharacterEntityView view = newEnemy.characterEntityView;
                view.myActivationWindow.gameObject.SetActive(false);
                ActivationManager.Instance.DisablePanelSlotAtIndex(ActivationManager.Instance.ActivationOrder.IndexOf(newEnemy));

                // Hide GUI
                FadeOutCharacterWorldCanvas(view, null, 0);

                // Hide model
                CharacterModelController.Instance.FadeOutCharacterModel(view.ucm, 0);
                CharacterModelController.Instance.FadeOutCharacterShadow(view, 0);
                view.blockMouseOver = true;

                // Set start position
                if (effect.summonedCreatureStartPosition == SummonAtLocation.StartNode)
                {

                }
                else if (effect.summonedCreatureStartPosition == SummonAtLocation.OffScreen)
                {

                }

                // Enable activation window
                int windowIndex = ActivationManager.Instance.ActivationOrder.IndexOf(newEnemy);
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    view.myActivationWindow.gameObject.SetActive(true);
                    view.myActivationWindow.Show();
                    ActivationManager.Instance.EnablePanelSlotAtIndex(windowIndex);
                }, QueuePosition.Back, 0f, 0.1f);

                // Update all window slot positions + activation pointer arrow
                CharacterEntityModel entityActivated = ActivationManager.Instance.EntityActivated;
                VisualEventManager.Instance.CreateVisualEvent(() => ActivationManager.Instance.UpdateWindowPositions());
                VisualEventManager.Instance.CreateVisualEvent(() => ActivationManager.Instance.MoveActivationArrowTowardsEntityWindow(entityActivated));

                // Fade in model + UI
                VisualEventManager.Instance.CreateVisualEvent(() => FadeInCharacterWorldCanvas(view, null, effect.uiFadeInSpeed));
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    CharacterModelController.Instance.FadeInCharacterModel(view.ucm, effect.modelFadeInSpeed);
                    CharacterModelController.Instance.FadeInCharacterShadow(view, 1f, () => view.blockMouseOver = false);
                });

                // Resolve visual events
                foreach (AnimationEventData vEvent in effect.summonedCreatureVisualEvents)
                {
                    AnimationEventController.Instance.PlayAnimationEvent(vEvent, newEnemy, newEnemy);
                }
            }

            // Defend target
            else if (effect.actionType == ActionType.DefendTarget)
            {
                if (target == null)
                {
                    target = enemy;
                }

                GainBlock(target, CombatLogic.Instance.CalculateBlockGainedByEffect(effect.blockGained, enemy, target, effect, null));
            }

            // Defend self
            else if (effect.actionType == ActionType.DefendSelf)
            {
                GainBlock(enemy, CombatLogic.Instance.CalculateBlockGainedByEffect(effect.blockGained, enemy, enemy, effect, null));
            }

            // Defend All
            else if (effect.actionType == ActionType.DefendAllAllies)
            {
                foreach (CharacterEntityModel ally in GetAllAlliesOfCharacter(enemy))
                {
                    GainBlock(ally, CombatLogic.Instance.CalculateBlockGainedByEffect(effect.blockGained, enemy, ally, effect, null));
                }
            }

            // Heal Allies
            else if (effect.actionType == ActionType.HealAllAllies)
            {
                foreach (CharacterEntityModel ally in GetAllAlliesOfCharacter(enemy, effect.alsoHealSelf))
                {
                    // Modify health
                    ModifyHealth(ally, effect.healAmount);

                    // Heal VFX
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                        VisualEffectManager.Instance.CreateHealEffect(ally.characterEntityView.WorldPosition, effect.healAmount));

                    // Create heal text effect
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateDamageEffect(ally.characterEntityView.WorldPosition, effect.healAmount, true));

                    // Create SFX
                    VisualEventManager.Instance.CreateVisualEvent(() => AudioManager.Instance.PlaySoundPooled(Sound.Ability_Heal_Twinkle));
                }
            }

            // Buff Self + Buff Target
            else if (effect.actionType == ActionType.BuffSelf ||
                     effect.actionType == ActionType.BuffTarget)
            {
                // Set self as target if 'BuffSelf' type
                if (effect.actionType == ActionType.BuffSelf)
                {
                    target = enemy;
                }

                PassiveController.Instance.ModifyPassiveOnCharacterEntity(target.pManager, effect.passiveApplied.passiveName, effect.passiveStacks, true, 0f, enemy.pManager);
            }

            // Buff All
            else if (effect.actionType == ActionType.BuffAllAllies)
            {
                foreach (CharacterEntityModel ally in GetAllAlliesOfCharacter(enemy))
                {
                    PassiveController.Instance.ModifyPassiveOnCharacterEntity(ally.pManager, effect.passiveApplied.passiveName, effect.passiveStacks, true, 0, enemy.pManager);
                }

            }

            // Debuff Target
            else if (effect.actionType == ActionType.DebuffTarget)
            {
                PassiveController.Instance.ModifyPassiveOnCharacterEntity(target.pManager, effect.passiveApplied.passiveName, effect.passiveStacks, true, 0f, enemy.pManager);
            }

            // Debuff All
            else if (effect.actionType == ActionType.DebuffAllEnemies)
            {
                foreach (CharacterEntityModel enemyy in GetAllEnemiesOfCharacter(enemy))
                {
                    PassiveController.Instance.ModifyPassiveOnCharacterEntity(enemyy.pManager, effect.passiveApplied.passiveName, effect.passiveStacks, true, 0f, enemy.pManager);
                }

            }

            // Add Card
            else if (effect.actionType == ActionType.AddCardToTargetCardCollection)
            {
                List<Card> cardsAdded = new List<Card>();
                for (int loops = 0; loops < effect.copiesAdded; loops++)
                {
                    if (effect.collection == CardCollection.DiscardPile)
                    {
                        Card card = CardController.Instance.CreateAndAddNewCardToCharacterDiscardPile(enemy.currentActionTarget, effect.cardAdded);
                        cardsAdded.Add(card);
                    }
                    else if (effect.collection == CardCollection.DrawPile)
                    {
                        Card card = CardController.Instance.CreateAndAddNewCardToCharacterDrawPile(enemy.currentActionTarget, effect.cardAdded);
                        cardsAdded.Add(card);
                    }
                }

                CardController.Instance.StartNewShuffleCardsScreenVisualEvent(cardsAdded);

            }

            // CONCLUDING VISUAL EVENTS!
            if (CombatLogic.Instance.CurrentCombatState == CombatGameState.CombatActive &&
                enemy.livingState == LivingState.Alive)
            {
                // cancel if the target was killed
                if (target != null && target.livingState == LivingState.Dead)
                {
                    return;
                }

                foreach (AnimationEventData vEvent in effect.visualEventsOnFinish)
                {
                    AnimationEventController.Instance.PlayAnimationEvent(vEvent, enemy, target);
                }
            }
        }

    }
    private void StartEnemyActivation(CharacterEntityModel enemy)
    {
        Debug.Log("CharacterEntityController.StartEnemyActivation() called ");
        // Set Activated Phase
        SetCharacterActivationPhase(enemy, ActivationPhase.ActivationPhase);

        // Execute activation actions
        ExecuteEnemyNextAction(enemy);

        // Start end activation process
        CharacterOnActivationEnd(enemy);
    }
    #endregion

    // Move Character Visual Events
    #region
    public void MoveAttackerToTargetNodeAttackPosition(CharacterEntityModel attacker, LevelNode node, CoroutineData cData)
    {
        Debug.Log("CharacterEntityController.MoveAttackerToTargetNodeAttackPosition() called...");
        StartCoroutine(MoveAttackerToTargetNodeAttackPositionCoroutine(attacker, node, cData));
    }
    private IEnumerator MoveAttackerToTargetNodeAttackPositionCoroutine(CharacterEntityModel attacker, LevelNode node, CoroutineData cData)
    {
        // Set up
        bool reachedDestination = false;
        Vector3 destination = new Vector3(node.attackPos.position.x, node.attackPos.position.y, 0);
        float moveSpeed = 10;
        attacker.ModifyQueuedMovements(-1);

        // Face direction of destination
        LevelManager.Instance.TurnFacingTowardsLocation(attacker.characterEntityView, node.transform.position);

        // Play movement animation
        PlayMoveAnimation(attacker.characterEntityView);

        while (reachedDestination == false)
        {
            attacker.characterEntityView.ucmMovementParent.transform.position = Vector2.MoveTowards(attacker.characterEntityView.WorldPosition, destination, moveSpeed * Time.deltaTime);

            if (attacker.characterEntityView.WorldPosition == destination)
            {
                Debug.Log("CharacterEntityController.MoveAttackerToTargetNodeAttackPositionCoroutine() detected destination was reached...");
                reachedDestination = true;
            }
            yield return null;
        }

        // Resolve
        if (cData != null)
        {
            cData.MarkAsCompleted();
        }

    }
    public void MoveEntityToNodeCentre(CharacterEntityView view, LevelNode node, CoroutineData data, Action onCompleteCallback = null, float startDelay = 0.3f)
    {
        Debug.Log("CharacterEntityController.MoveEntityToNodeCentre() called...");
        StartCoroutine(MoveEntityToNodeCentreCoroutine(view, node, data, onCompleteCallback, startDelay));
    }
    private IEnumerator MoveEntityToNodeCentreCoroutine(CharacterEntityView view, LevelNode node, CoroutineData cData, Action onCompleteCallback, float startDelay)
    {
        // Set up
        bool reachedDestination = false;
        Vector3 destination = new Vector3(node.transform.position.x, node.transform.position.y, 0);
        float moveSpeed = 10;

        // Brief yield here (incase melee attack anim played and character hasn't returned to attack pos )
        yield return new WaitForSeconds(startDelay);

        if(view == null)
        {
            Debug.LogWarning("character view is null");
        }
        else if (node == null)
        {
            Debug.LogWarning("node is null");
        }

        // Face direction of destination node
        LevelManager.Instance.TurnFacingTowardsLocation(view, node.transform.position);

        // Play movement animation
        PlayMoveAnimation(view);

        // Move
        while (reachedDestination == false)
        {
            view.ucmMovementParent.transform.position = Vector2.MoveTowards(view.WorldPosition, destination, moveSpeed * Time.deltaTime);

            if (view.WorldPosition == destination)
            {
                Debug.Log("CharacterEntityController.MoveEntityToNodeCentreCoroutine() detected destination was reached...");
                reachedDestination = true;
            }
            yield return null;
        }

        // Reset facing, depending on living entity type
        if(view.character != null)
        {
            if (view.character.allegiance == Allegiance.Player)
            {
                LevelManager.Instance.SetDirection(view, FacingDirection.Right);
            }
            else if (view.character.allegiance == Allegiance.Enemy)
            {
                LevelManager.Instance.SetDirection(view, FacingDirection.Left);
            }
        }          

        // Idle anim
        PlayIdleAnimation(view);

        // Resolve event
        if (cData != null)
        {
            cData.MarkAsCompleted();
        }

        if(onCompleteCallback != null)
        {
            onCompleteCallback.Invoke();
        }
    }
    public void MoveAttackerToCentrePosition(CharacterEntityModel attacker, CoroutineData cData)
    {
        Debug.Log("CharacterEntityController.MoveAttackerToTargetNodeAttackPosition() called...");
        StartCoroutine(MoveAttackerToCentrePositionCoroutine(attacker, cData));
    }
    private IEnumerator MoveAttackerToCentrePositionCoroutine(CharacterEntityModel attacker, CoroutineData cData)
    {
        // Set up
        bool reachedDestination = false;
        Transform centrePos = LevelManager.Instance.CentrePos;
        Vector3 destination = new Vector3(centrePos.position.x, centrePos.position.y, 0);
        float moveSpeed = 10;

        // Face direction of centre pos
        if (attacker.allegiance == Allegiance.Player)
        {
            LevelManager.Instance.SetDirection(attacker.characterEntityView, FacingDirection.Right);
        }
        else if (attacker.allegiance == Allegiance.Enemy)
        {
            LevelManager.Instance.SetDirection(attacker.characterEntityView, FacingDirection.Left);
        }

        // Play movement animation
        PlayMoveAnimation(attacker.characterEntityView);

        while (reachedDestination == false)
        {
            attacker.characterEntityView.ucmMovementParent.transform.position = Vector2.MoveTowards(attacker.characterEntityView.WorldPosition, destination, moveSpeed * Time.deltaTime);

            if (attacker.characterEntityView.WorldPosition == destination)
            {
                reachedDestination = true;
            }
            yield return null;
        }

        // Resolve
        if (cData != null)
        {
            cData.MarkAsCompleted();
        }

    }
    public void MoveAllCharactersToOffScreenPosition()
    {
        foreach(CharacterEntityModel character in AllCharacters)
        {
            MoveEntityToOffScreenPosition(character);
        }
    }
    private void MoveEntityToOffScreenPosition(CharacterEntityModel entity)
    {
        if (entity.allegiance == Allegiance.Player)
        {
            entity.characterEntityView.ucmMovementParent.transform.position = LevelManager.Instance.DefenderOffScreenNode.transform.position;
        }
        else if (entity.allegiance == Allegiance.Enemy)
        {
            entity.characterEntityView.ucmMovementParent.transform.position = LevelManager.Instance.EnemyOffScreenNode.transform.position;
        }
    }
    public void MoveAllCharactersToStartingNodes(CoroutineData data)
    {
        Debug.Log("MoveAllCharactersToStartingNodes.MoveEntityToNodeCentre() called...");
        StartCoroutine(MoveAllCharactersToStartingNodesCoroutine(data));
    }
    private IEnumerator MoveAllCharactersToStartingNodesCoroutine(CoroutineData data)
    {
        foreach (CharacterEntityModel character in AllCharacters)
        {
            MoveEntityToNodeCentre(character.characterEntityView, character.levelNode, null);
        }

        yield return new WaitForSeconds(3f);

        if (data != null)
        {
            data.MarkAsCompleted();
        }
    }    
    public void MoveCharactersToOffScreenRight(List<CharacterEntityModel> characters, CoroutineData cData)
    {
        StartCoroutine(MoveCharactersToOffScreenRightCoroutine(characters, cData));
    }
    private IEnumerator MoveCharactersToOffScreenRightCoroutine(List<CharacterEntityModel> characters, CoroutineData cData)
    {
        foreach(CharacterEntityModel character in characters)
        {
            MoveEntityToNodeCentre(character.characterEntityView, LevelManager.Instance.EnemyOffScreenNode, null);
        }

        yield return new WaitForSeconds(3f);

        if(cData != null)
        {
            cData.MarkAsCompleted();
        }
    }
    #endregion

    // Determine a Character's Allies and Enemies Logic
    #region
    public bool IsTargetFriendly(CharacterEntityModel character, CharacterEntityModel target)
    {
        Debug.Log("CharacterEntityController.IsTargetFriendly() called, comparing " +
            character.myName + " to " + target.myName);

        return character.allegiance == target.allegiance;
    }
    public List<CharacterEntityModel> GetAllEnemiesOfCharacter(CharacterEntityModel character)
    {
        Debug.Log("CharacterEntityController.GetAllEnemiesOfCharacter() called...");

        List<CharacterEntityModel> listReturned = new List<CharacterEntityModel>();

        foreach (CharacterEntityModel entity in AllCharacters)
        {
            if (!IsTargetFriendly(character, entity))
            {
                listReturned.Add(entity);
            }
        }

        return listReturned;
    }
    public List<CharacterEntityModel> GetAllAlliesOfCharacter(CharacterEntityModel character, bool includeSelfInSearch = true)
    {
        Debug.Log("CharacterEntityController.GetAllEnemiesOfCharacter() called...");

        List<CharacterEntityModel> listReturned = new List<CharacterEntityModel>();

        foreach (CharacterEntityModel entity in AllCharacters)
        {
            if (IsTargetFriendly(character, entity))
            {
                listReturned.Add(entity);
            }
        }

        if (includeSelfInSearch == false &&
            listReturned.Contains(character))
        {
            listReturned.Remove(character);
        }

        return listReturned;
    }
    #endregion

    // Misc Events Handlers
    #region
    public void HandleTaunt(CharacterEntityModel taunter, CharacterEntityModel target)
    {
        Debug.Log("CharacterEntityController.HandleTaunt() called...");

        // does the enemy actually intent to attack?
        if (target.myNextAction.actionType == ActionType.AttackTarget)
        {
            // Set taunter as target of next enemy attack
            SetEnemyTarget(target, taunter);

            // Apply taunted passive
            PassiveController.Instance.ModifyTaunted(taunter, target.pManager, 1);

            // Update targeting gui/view
            UpdateEnemyIntentGUI(target);
        }
    }
    #endregion

}