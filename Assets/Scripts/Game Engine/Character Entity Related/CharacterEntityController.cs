using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;


public class CharacterEntityController : Singleton<CharacterEntityController>
{
    // Properties + Component References
    #region
    [Header("Character Entity List Variables")]
    private List<CharacterEntityModel> allCharacters = new List<CharacterEntityModel>();
    private List<CharacterEntityModel> allDefenders = new List<CharacterEntityModel>();
    private List<CharacterEntityModel> allEnemies = new List<CharacterEntityModel>();

    [Header("UCM Colours")]
    public Color normalColour;
    public Color highlightColour;
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

        // Disable block icon
        view.blockIcon.SetActive(false);

        // Get camera references
        view.uiCanvas.worldCamera = CameraManager.Instance.MainCamera;

        // Disable main UI canvas + card UI stuff
        view.uiCanvasParent.SetActive(false);
    }
    public void CreateAllPlayerCombatCharacters()
    {
        foreach (CharacterData data in CharacterDataController.Instance.allPlayerCharacters)
        {
            // Dont spawn dead characters
            if (data.health > 0)
            {
                CreatePlayerCharacter(data, LevelManager.Instance.GetNextAvailableDefenderNode());
            }          
        }
    }
    public void DestroyAllCombatCharacters()
    {
        foreach (CharacterEntityModel character in AllCharacters)
        {
            Destroy(character.characterEntityView.gameObject);
        }

        AllCharacters.Clear();
        AllDefenders.Clear();
        AllEnemies.Clear();
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

        // Set up view
        SetCharacterViewStartingState(model);

        // Copy data from character data into new model
        SetupCharacterFromEnemyData(model, data);

        // Add to persistency
        AddEnemyToPersistency(model);

        return model;
    }
    private void SetupCharacterFromCharacterData(CharacterEntityModel character, CharacterData data)
    {
        // Set general info
        character.myName = data.myName;

        // Setup Core Stats
        ModifyStamina(character, data.stamina);
        ModifyInitiative(character, data.initiative);
        ModifyDraw(character, data.draw);
        ModifyDexterity(character, data.dexterity);
        ModifyPower(character, data.power);

        // Set up health
        ModifyMaxHealth(character, data.maxHealth);
        ModifyHealth(character, data.health);

        // TO DO IN FUTURE: We need a better way to track character data's body 
        // parts: strings references are not scaleable
        // Build UCM
        CharacterModelController.BuildModelFromStringReferences(character.characterEntityView.ucm, data.modelParts);

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

        // Setup Core Stats
        ModifyInitiative(character, data.initiative);
        ModifyDexterity(character, data.dexterity);
        ModifyPower(character, data.power);

        // Set up health + Block
        ModifyMaxHealth(character, data.maxHealth);
        ModifyHealth(character, data.startingHealth);
        ModifyBlock(character, data.startingBlock, false);

        // Build UCM
        CharacterModelController.BuildModelFromStringReferences(character.characterEntityView.ucm, data.allBodyParts);

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
    public void RemoveDefenderFromPersistency(CharacterEntityModel character)
    {
        Debug.Log("CharacterEntityController.RemoveDefenderFromPersistency() called, removing: " + character.myName);
        AllCharacters.Remove(character);
        AllDefenders.Remove(character);
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
        if (finalHealthValue > character.maxHealth)
        {
            finalHealthValue = character.maxHealth;
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

        VisualEventManager.Instance.CreateVisualEvent(() => UpdateHealthGUIElements(character, finalHealthValue, character.maxHealth), QueuePosition.Back, 0, 0);
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
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateHealthGUIElements(character, currentHealth, character.maxHealth), QueuePosition.Back, 0, 0);

        // Update health if it now excedes max health
        if (character.health > character.maxHealth)
        {
            ModifyHealth(character, (character.maxHealth - character.health));
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
    public void ModifyEnergy(CharacterEntityModel character, int energyGainedOrLost, bool showVFX = false)
    {
        Debug.Log("CharacterEntityController.ModifyEnergy() called for " + character.myName);
        character.energy += energyGainedOrLost;
        CharacterEntityView view = character.characterEntityView;

        if (character.energy < 0)
        {
            character.energy = 0;
        }

        if (showVFX && view != null)
        {
            // Status notification
            VisualEventManager.Instance.CreateVisualEvent(() =>
            VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Energy +" + energyGainedOrLost.ToString()));

            // Buff sparkle VFX
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateGeneralBuffEffect(view.WorldPosition));
        }

        VisualEventManager.Instance.CreateVisualEvent(() => UpdateEnergyGUI(view, character.energy), QueuePosition.Back, 0, 0);
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

        VisualEventManager.Instance.CreateVisualEvent(() => UpdateStaminaGUI(view, EntityLogic.GetTotalStamina(character)), QueuePosition.Back, 0, 0);
    }
    private void UpdateEnergyGUI(CharacterEntityView view, int newValue)
    {
        view.energyText.text = newValue.ToString();
    }
    private void UpdateStaminaGUI(CharacterEntityView view, int newValue)
    {
        view.staminaText.text = newValue.ToString();
    }
    #endregion

    // Modify Block
    #region
    public void ModifyBlock(CharacterEntityModel character, int blockGainedOrLost, bool showVFX = true)
    {
        Debug.Log("CharacterEntityController.ModifyBlock() called for " + character.myName);

        int finalBlockGainValue = blockGainedOrLost;
        int characterFinalBlockValue = 0;
        CharacterEntityView view = character.characterEntityView;

        // prevent block going negative
        if (finalBlockGainValue < 0)
        {
            finalBlockGainValue = 0;
        }

        // Apply block gain
        character.block += finalBlockGainValue;
        characterFinalBlockValue = character.block;

        if (finalBlockGainValue > 0 && showVFX)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateGainBlockEffect(view.WorldPosition, finalBlockGainValue), QueuePosition.Back, 0, 0);
        }

        if (showVFX)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => UpdateBlockGUI(character, characterFinalBlockValue), QueuePosition.Back, 0, 0);
        }
        else
        {
            UpdateBlockGUI(character, characterFinalBlockValue);
        }

        // Resolve Sentinel passive effect
        if(character.pManager != null &&
           character.pManager.sentinelStacks > 0)
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
                int sentinelDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(character, targetHit, DamageType.Physical, character.pManager.sentinelStacks, null, null);
                CombatLogic.Instance.HandleDamage(sentinelDamageValue, character, targetHit, DamageType.Physical, true);
            }
        }
    }
    public void SetBlock(CharacterEntityModel character, int newBlockValue)
    {
        Debug.Log("CharacterEntityController.SetBlock() called for " + character.myName);

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
    #endregion

    // Activation Related
    #region    
    public void CharacterOnNewTurnCycleStarted(CharacterEntityModel character)
    {
        Debug.Log("CharacterEntityController.CharacterOnNewTurnCycleStartedCoroutine() called for " + character.myName);

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
        Debug.Log("CharacterEntityController.CharacterOnActivationStart() called for " + character.myName);

        // Enable activated view state
        LevelNode charNode = character.levelNode;
        VisualEventManager.Instance.CreateVisualEvent(() => LevelManager.Instance.SetActivatedViewState(charNode, true), QueuePosition.Back);

        // Gain Energy
        ModifyEnergy(character, EntityLogic.GetTotalStamina(character));

        // Modify relevant passives
        if (character.pManager.temporaryBonusStaminaStacks != 0)
        {
            PassiveController.Instance.ModifyTemporaryStamina(character.pManager, -character.pManager.temporaryBonusStaminaStacks, true, 0.5f);
        }

        // is the character player controller?
        if (character.controller == Controller.Player)
        {
            // Activate main UI canvas view
            CoroutineData cData = new CoroutineData();
            VisualEventManager.Instance.CreateVisualEvent(() => FadeInCharacterUICanvas(character.characterEntityView, cData), cData, QueuePosition.Back);

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
                viableCards.AddRange(CardController.Instance.GetCardsQuery(CardController.Instance.AllCards, TalentSchool.None, Rarity.Common));

                for (int i = 0; i < character.pManager.fastLearnerStacks; i++)
                {
                    // Create and add new card to hand
                    CardData newCard = viableCards[RandomGenerator.NumberBetween(0, viableCards.Count - 1)];
                    Card card = CardController.Instance.CreateAndAddNewCardToCharacterHand(character, newCard);

                    // Reduce its energy cost by 1
                    CardController.Instance.ReduceCardEnergyCostThisCombat(card, 1);
                }
            }

            // Lord Of Storms
            if (character.pManager.lordOfStormsStacks > 0)
            {
                PassiveController.Instance.ModifyOverload(character.pManager, character.pManager.lordOfStormsStacks, true, 0.5f);
            }

            // Draw cards on turn start
            CardController.Instance.DrawCardsOnActivationStart(character);

            // Remove temp draw
            if (character.pManager.temporaryBonusDrawStacks != 0)
            {
                PassiveController.Instance.ModifyTemporaryDraw(character.pManager, -character.pManager.temporaryBonusDrawStacks, true, 0.5f);
            }
        }

        // is the character an enemy?
        if (character.controller == Controller.AI &&
            character.allegiance == Allegiance.Enemy)
        {
            // Brief pause at the start of enemy action, so player can anticipate visual events
            VisualEventManager.Instance.InsertTimeDelayInQueue(1f);

            // Star enemy activation process
            StartEnemyActivation(character);
        }
    }
    public void CharacterOnActivationEnd(CharacterEntityModel entity)
    {
        Debug.Log("CharacterEntityController.CharacterOnActivationEnd() called for " + entity.myName);

        // Cache refs for visual events
        LevelNode veNode = entity.levelNode;
        CharacterEntityView view = entity.characterEntityView;

        // Disable end turn button clickability
        UIManager.Instance.DisableEndTurnButtonInteractions();

        // reset misc properties
        entity.meleeAttacksPlayedThisActivation = 0;

        // Disable level node activation ring view        
        VisualEventManager.Instance.CreateVisualEvent(() => LevelManager.Instance.SetActivatedViewState(veNode, false));

        // Stop if combat has ended
        if (CombatLogic.Instance.CurrentCombatState != CombatGameState.CombatActive)
        {
            Debug.Log("CharacterEntityController.CharacterOnActivationEnd() detected combat state is not active, cancelling early... ");
            return;
        }

        // Do player character exclusive logic
        if (entity.controller == Controller.Player)
        {
            // Lose unused energy, discard hand
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
            PassiveController.Instance.ModifyTemporaryPower(entity.pManager, -1, true, 0.5f);
        }
        if (entity.pManager.temporaryBonusDexterityStacks != 0)
        {
            PassiveController.Instance.ModifyTemporaryDexterity(entity.pManager, -1, true, 0.5f);
        }

        // Percentage modifiers
        if (entity.pManager.wrathStacks > 0)
        {
            PassiveController.Instance.ModifyWrath(entity.pManager, -1, true, 0.5f);
        }
        if (entity.pManager.weakenedStacks > 0)
        {
            PassiveController.Instance.ModifyWeakened(entity.pManager, -1, true, 0.5f);
        }
        if (entity.pManager.gritStacks > 0)
        {
            PassiveController.Instance.ModifyGrit(entity.pManager, -1, true, 0.5f);
        }
        if (entity.pManager.vulnerableStacks > 0)
        {
            PassiveController.Instance.ModifyVulnerable(entity.pManager, -1, true, 0.5f);
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

        // Buff Passive Triggers

        // Shield Wall
        if (entity.pManager.shieldWallStacks > 0)
        {
            // Notication vfx
            VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateStatusEffect(entity.characterEntityView.transform.position, "Shield Wall"));

            // Apply block gain
            ModifyBlock(entity, CombatLogic.Instance.CalculateBlockGainedByEffect(entity.pManager.shieldWallStacks, entity, entity));
        }       

        // Growing
        if (entity.pManager.growingStacks > 0)
        {
            // Notication vfx
            VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateStatusEffect(entity.characterEntityView.transform.position, "Growing!"), QueuePosition.Back, 0, 0.5f);

            // Gain power
            PassiveController.Instance.ModifyBonusPower(entity.pManager, entity.pManager.growingStacks, true, 0.5f);
        }

        // Encouraging Aura
        if (entity.pManager.encouragingAuraStacks > 0)
        {
            CharacterEntityModel[] allAllies = GetAllAlliesOfCharacter(entity, false).ToArray();
            CharacterEntityModel chosenAlly = allAllies[RandomGenerator.NumberBetween(0, allAllies.Length - 1)];

            if (chosenAlly != null)
            {
                // Notification event
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Encouraging Aura!"), QueuePosition.Back, 0, 0.5f);

                // Random ally gains energy
                ModifyEnergy(chosenAlly, entity.pManager.encouragingAuraStacks, true);
            }
        }

        // Shadow Aura
        if (entity.pManager.shadowAuraStacks > 0)
        {
            CharacterEntityModel[] allEnemies = GetAllEnemiesOfCharacter(entity).ToArray();
            CharacterEntityModel chosenEnemy = allEnemies[RandomGenerator.NumberBetween(0, allEnemies.Length - 1)];

            if (chosenEnemy != null)
            {
                // Notification event
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Shadow Aura!"), QueuePosition.Back, 0, 0.5f);

                // Random ally gains energy
                PassiveController.Instance.ModifyWeakened(chosenEnemy.pManager, entity.pManager.shadowAuraStacks, true);
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
                PassiveController.Instance.ModifyPoisoned(entity, enemy.pManager, entity.pManager.toxicAuraStacks, true);
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
                ModifyBlock(ally, CombatLogic.Instance.CalculateBlockGainedByEffect(entity.pManager.guardianAuraStacks, entity, ally));
            }
        }

        // DoTs
        if (entity.pManager.poisonedStacks > 0)
        {
            // Notification event
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Poisoned!"), QueuePosition.Back, 0, 0.5f);

            // Calculate and deal Poison damage
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(null, entity, DamageType.Poison, entity.pManager.poisonedStacks, null, null);
            VisualEventManager.Instance.CreateVisualEvent(() => CameraManager.Instance.CreateCameraShake(CameraShakeType.Small));
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateEffectAtLocation(ParticleEffect.PoisonExplosion, view.WorldPosition));
            CombatLogic.Instance.HandleDamage(finalDamageValue, null, entity, DamageType.Poison, true);           
        }
        if (entity.pManager.burningStacks > 0)
        {
            // Notification event
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Burning!"), QueuePosition.Back, 0, 0.5f);

            // Calculate and deal Poison damage
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(null, entity, DamageType.Fire, entity.pManager.burningStacks, null, null);
            VisualEventManager.Instance.CreateVisualEvent(() => CameraManager.Instance.CreateCameraShake(CameraShakeType.Small));
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateEffectAtLocation(ParticleEffect.FireExplosion, view.WorldPosition));
            CombatLogic.Instance.HandleDamage(finalDamageValue, null, entity, DamageType.Fire, true);           
        }

        // Overload
        if (entity.pManager.overloadStacks > 0)
        {
            // Notification event
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Overload!"), QueuePosition.Back, 0, 0.5f);

            // Get random enemy
            CharacterEntityModel[] enemies = GetAllEnemiesOfCharacter(entity).ToArray();
            CharacterEntityModel randomEnemy = enemies[Random.Range(0, enemies.Length)];
            CharacterEntityView randomEnemyView = randomEnemy.characterEntityView;

            // Create lightning ball missle
            CoroutineData cData = new CoroutineData();
            VisualEventManager.Instance.CreateVisualEvent(() =>
            VisualEffectManager.Instance.ShootProjectileAtLocation(ProjectileFired.LightningBall1, view.WorldPosition, randomEnemyView.WorldPosition, cData), cData);
            VisualEventManager.Instance.CreateVisualEvent(() => CameraManager.Instance.CreateCameraShake(CameraShakeType.Small));

            // Deal air damage
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(entity, randomEnemy, DamageType.Air, entity.pManager.overloadStacks, null, null);
            CombatLogic.Instance.HandleDamage(finalDamageValue, entity, randomEnemy, DamageType.Air, true);

            // Brief pause here
            VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
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

        // Disable level node activation ring view        
        //VisualEventManager.Instance.CreateVisualEvent(() => LevelManager.Instance.SetActivatedViewState(veNode, false));

        // Activate next character
        if (entity != null &&
           entity.livingState == LivingState.Alive)
        {
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

            if (enemy.myNextAction.actionLoops > 1)
            {
                attackDamageString = finalDamageValue.ToString() + " x " + enemy.myNextAction.actionLoops.ToString();
            }
            else
            {
                attackDamageString = finalDamageValue.ToString();
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
    public void FadeOutCharacterWorldCanvas(CharacterEntityView view, CoroutineData cData)
    {
        StartCoroutine(FadeOutCharacterWorldCanvasCoroutine(view, cData));
    }
    private IEnumerator FadeOutCharacterWorldCanvasCoroutine(CharacterEntityView view, CoroutineData cData)
    {
        view.worldSpaceCanvasParent.gameObject.SetActive(true);
        view.worldSpaceCG.alpha = 1;
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

    }
    private void UpdateEnemyIntentGUIVisualEvent(IntentViewModel intentView, Sprite intentSprite, string attackDamageString)
    {        
        // Disable text view
        intentView.valueText.gameObject.SetActive(false);

        // Start fade in effect        
        intentView.FadeInView();

        // Set intent image
        intentView.SetIntentSprite(intentSprite);

        if (attackDamageString != "")
        {
            // Enable attack damage value text, if we have value to show
            intentView.valueText.gameObject.SetActive(true);
            intentView.valueText.text = attackDamageString;
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
    public void FadeOutCharacterModel(CharacterEntityView view, CoroutineData cData)
    {
        Debug.Log("CharacterEntityController.FadeOutCharacterModel() called...");
        StartCoroutine(FadeOutCharacterModelCoroutine(view, cData));
    }
    private IEnumerator FadeOutCharacterModelCoroutine(CharacterEntityView view, CoroutineData cData)
    {
        float currentAlpha = view.entityRenderer.Color.a;
        float fadeSpeed = 5f;

        while (currentAlpha > 0)
        {
            view.entityRenderer.Color = new Color(view.entityRenderer.Color.r, view.entityRenderer.Color.g, view.entityRenderer.Color.b, currentAlpha - (fadeSpeed * Time.deltaTime));
            currentAlpha = view.entityRenderer.Color.a;
            yield return null;
        }

        // Resolve
        if (cData != null)
        {
            cData.MarkAsCompleted();
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
        AudioManager.Instance.PlaySound(Sound.Character_Draw_Bow);
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

        // prevent clicking through an active UI screen
        if (CardController.Instance.DiscoveryScreenIsActive || 
            CardController.Instance.ChooseCardScreenIsActive ||
            MainMenuController.Instance.AnyMenuScreenIsActive())
        {
            return;
        }

        // Mouse over SFX
        AudioManager.Instance.PlaySound(Sound.GUI_Button_Mouse_Over);

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
        view.myActivationWindow.myGlowOutline.SetActive(true);
        LevelManager.Instance.SetMouseOverViewState(view.character.levelNode, true);

        // Set character highlight color
        SetCharacterColor(view, highlightColour);

        // Update card being dragged text
        if (Draggable.DraggingThis != null &&
            Draggable.DraggingThis.Da is DragSpellOnTarget && 
            view.character != null)
        {
            CardController.Instance.AutoUpdateCardDescriptionText(Draggable.DraggingThis.Da.CardVM().card, view.character);
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

        // prevent clicking through an active UI screen
        if (CardController.Instance.DiscoveryScreenIsActive || 
            CardController.Instance.ChooseCardScreenIsActive ||
             MainMenuController.Instance.AnyMenuScreenIsActive())
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

        // Enable activation window glow
        view.myActivationWindow.myGlowOutline.SetActive(false);

        // Update card being dragged text, reset to targetless calculation
        if (Draggable.DraggingThis != null &&
            Draggable.DraggingThis.Da is DragSpellOnTarget)
        {
            CardController.Instance.AutoUpdateCardDescriptionText(Draggable.DraggingThis.Da.CardVM().card, null);
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
                ActivationManager.Instance.CurrentTurn == ar.requirementTypeValue)
            {
                Debug.Log(enemyAction.actionName + " failed 'IsTurn' requirement");
                checkResults.Add(false);
            }

            // Check enough allies alive
            if (ar.requirementType == ActionRequirementType.AtLeastXAlliesAlive &&
                GetAllAlliesOfCharacter(enemy).Count < ar.requirementTypeValue)
            {
                Debug.Log(enemyAction.actionName + " failed 'AtLeastXAlliesAlive' requirement");
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
               ar.requirementTypeValue < ActivationManager.Instance.CurrentTurn)
            {
                Debug.Log(enemyAction.actionName + " failed 'IsMoreThanTurn' requirement");
                checkResults.Add(false);
            }

            // Check ActivatedXTimesOrMore
            if (ar.requirementType == ActionRequirementType.ActivatedXTimesOrMore &&
               enemy.myPreviousActionLog.Count < ar.requirementTypeValue)
            {
                Debug.Log(enemyAction.actionName + " failed 'ActivatedXTimesOrMore' requirement");
                checkResults.Add(false);
            }

            // Check ActivatedXTimesOrMore
            if (ar.requirementType == ActionRequirementType.ActivatedXTimesOrLess &&
               enemy.myPreviousActionLog.Count > ar.requirementTypeValue)
            {
                Debug.Log(enemyAction.actionName + " failed 'ActivatedXTimesOrMore' requirement");
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
                actionReturned = viableNextMoves[Random.Range(0, viableNextMoves.Count)];
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
                targetReturned = enemies[Random.Range(0, enemies.Length)];
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
                targetReturned = allies[Random.Range(0, allies.Length)];
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
            VisualEventManager.Instance.CreateVisualEvent(() => MoveEntityToNodeCentre(enemy, node, cData), cData, QueuePosition.Back, 0.3f, 0);
        }

        // Brief pause at the of all effects
        VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
    }
    private void TriggerEnemyActionEffect(CharacterEntityModel enemy, EnemyActionEffect effect)
    {
        Debug.Log("CharacterEntityController.TriggerEnemyActionEffect() called on enemy " + enemy.myName);

        // Cache refs for visual events
        CharacterEntityModel target = enemy.currentActionTarget;

        // if invalid targetting issues occured before triggering event, return
        // TO DO: we should probably perform this validation process before calling 'TriggerEnemyActionEffect'
        if ((target != null && target.livingState == LivingState.Dead) ||
            ((effect.actionType == ActionType.AttackTarget ||
            effect.actionType == ActionType.DebuffTarget ||
            effect.actionType == ActionType.AddCardToTargetCardCollection) && target == enemy))
        {
            return;
        }

        // TO DO: we should probably remove this and find a better way to prevent enemies targetting themselves
        // with harmful effects
        if ((effect.actionType == ActionType.AttackTarget ||
            effect.actionType == ActionType.DebuffTarget ||
            effect.actionType == ActionType.AddCardToTargetCardCollection) &&
            target == null)
        {
            return;
        }

        // If no target, set self as target
        if ((effect.actionType == ActionType.DefendTarget ||
            effect.actionType == ActionType.BuffTarget) &&
            target == null)
        {
            target = enemy;
        }

        // trigger starting visual events
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

        // Defend target
        else if (effect.actionType == ActionType.DefendTarget)
        {
            if(target == null)
            {
                target = enemy;
            }

            ModifyBlock(target, CombatLogic.Instance.CalculateBlockGainedByEffect(effect.blockGained, enemy, target, effect, null));
        }

        // Defend self
        else if (effect.actionType == ActionType.DefendSelf)
        {
            ModifyBlock(enemy, CombatLogic.Instance.CalculateBlockGainedByEffect(effect.blockGained, enemy, enemy, effect, null));
        }

        // Defend All
        else if (effect.actionType == ActionType.DefendAllAllies)
        {
            foreach (CharacterEntityModel ally in GetAllAlliesOfCharacter(enemy))
            {
                ModifyBlock(ally, CombatLogic.Instance.CalculateBlockGainedByEffect(effect.blockGained, enemy, ally, effect, null));
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

            PassiveController.Instance.ModifyPassiveOnCharacterEntity(target.pManager, effect.passiveApplied.passiveName, effect.passiveStacks);
        }

        // Buff All
        else if (effect.actionType == ActionType.BuffAllAllies)
        {
            foreach (CharacterEntityModel ally in GetAllAlliesOfCharacter(enemy))
            {
                PassiveController.Instance.ModifyPassiveOnCharacterEntity(ally.pManager, effect.passiveApplied.passiveName, effect.passiveStacks);
            }

        }

        // Debuff Target
        else if (effect.actionType == ActionType.DebuffTarget)
        {
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(target.pManager, effect.passiveApplied.passiveName, effect.passiveStacks);
        }

        // Debuff All
        else if (effect.actionType == ActionType.DebuffAllEnemies)
        {
            foreach (CharacterEntityModel enemyy in GetAllEnemiesOfCharacter(enemy))
            {
                PassiveController.Instance.ModifyPassiveOnCharacterEntity(enemyy.pManager, effect.passiveApplied.passiveName, effect.passiveStacks);
            }

        }

        // Add Card
        else if (effect.actionType == ActionType.AddCardToTargetCardCollection)
        {
            List<Card> cardsAdded = new List<Card>();
            for (int i = 0; i < effect.copiesAdded; i++)
            {
                if (effect.collection == CardCollection.DiscardPile)
                {
                    // TO DO: Make a new method in CardController for this and future similar effects, like CreateCardAndAddToDiscardPile

                    Card card = CardController.Instance.CreateAndAddNewCardToCharacterDiscardPile(enemy.currentActionTarget, effect.cardAdded);
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
    private void StartEnemyActivation(CharacterEntityModel enemy)
    {
        Debug.Log("CharacterEntityController.StartEnemyActivation() called ");
        ExecuteEnemyNextAction(enemy);
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
    public void MoveEntityToNodeCentre(CharacterEntityModel entity, LevelNode node, CoroutineData data)
    {
        Debug.Log("CharacterEntityController.MoveEntityToNodeCentre() called...");
        StartCoroutine(MoveEntityToNodeCentreCoroutine(entity, node, data));
    }
    private IEnumerator MoveEntityToNodeCentreCoroutine(CharacterEntityModel entity, LevelNode node, CoroutineData cData)
    {
        // Set up
        bool reachedDestination = false;
        Vector3 destination = new Vector3(node.transform.position.x, node.transform.position.y, 0);
        float moveSpeed = 10;

        // Brief yield here (incase melee attack anim played and character hasn't returned to attack pos )
        yield return new WaitForSeconds(0.3f);

        // Face direction of destination node
        LevelManager.Instance.TurnFacingTowardsLocation(entity.characterEntityView, node.transform.position);

        // Play movement animation
        PlayMoveAnimation(entity.characterEntityView);

        // Move
        while (reachedDestination == false)
        {
            entity.characterEntityView.ucmMovementParent.transform.position = Vector2.MoveTowards(entity.characterEntityView.WorldPosition, destination, moveSpeed * Time.deltaTime);

            if (entity.characterEntityView.WorldPosition == destination)
            {
                Debug.Log("CharacterEntityController.MoveEntityToNodeCentreCoroutine() detected destination was reached...");
                reachedDestination = true;
            }
            yield return null;
        }

        // Reset facing, depending on living entity type
        if (entity.allegiance == Allegiance.Player)
        {
            LevelManager.Instance.SetDirection(entity.characterEntityView, FacingDirection.Right);
        }
        else if (entity.allegiance == Allegiance.Enemy)
        {
            LevelManager.Instance.SetDirection(entity.characterEntityView, FacingDirection.Left);
        }

        // Idle anim
        PlayIdleAnimation(entity.characterEntityView);

        // Resolve event
        if (cData != null)
        {
            cData.MarkAsCompleted();
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