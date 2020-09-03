using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class CharacterEntityController: Singleton<CharacterEntityController>
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
    public CharacterEntityModel CreatePlayerCharacter(CharacterData data, LevelNode position)
    {
        // Create GO + View
        CharacterEntityView vm = CreateCharacterEntityView().GetComponent<CharacterEntityView>();

        // Face enemies
        PositionLogic.Instance.SetDirection(vm, "Right");

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
        SetupCharacterFromCharacterData(model, data);

        // Build deck
        CardController.Instance.BuildDefenderDeckFromDeckData(model, data.deck);

        // Add to persistency
        AddDefenderToPersistency(model);

        return model;
    }
    public CharacterEntityModel CreateEnemyCharacter(EnemyDataSO data, LevelNode position)
    {
        // Create GO + View
        CharacterEntityView vm = CreateCharacterEntityView().GetComponent<CharacterEntityView>();

        // Face player characters
        PositionLogic.Instance.SetDirection(vm, "Left");

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

        VisualEventManager.Instance.CreateVisualEvent(()=> UpdateHealthGUIElements(character, finalHealthValue, character.maxHealth),QueuePosition.Back, 0, 0);
    }
    public void ModifyMaxHealth(CharacterEntityModel character, int maxHealthGainedOrLost)
    {
        Debug.Log("CharacterEntityController.ModifyMaxHealth() called for " + character.myName);

        int currentHealth = character.health;
        character.maxHealth += maxHealthGainedOrLost;
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateHealthGUIElements(character, currentHealth, character.maxHealth), QueuePosition.Back, 0, 0);
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
        Debug.Log("CharacterEntityController.ModifyEnergy() called for " + character.myName);
        character.energy += energyGainedOrLost;

        if (character.energy < 0)
        {
            character.energy = 0;
        }

        VisualEventManager.Instance.CreateVisualEvent(() => UpdateEnergyGUI(character, character.energy), QueuePosition.Back, 0, 0);
    }
    public void ModifyStamina(CharacterEntityModel character, int staminaGainedOrLost)
    {
        Debug.Log("CharacterEntityController.ModifyStamina() called for " + character.myName);
        character.stamina += staminaGainedOrLost;

        if (character.stamina < 0)
        {
            character.stamina = 0;
        }

        VisualEventManager.Instance.CreateVisualEvent(() => UpdateStaminaGUI(character), QueuePosition.Back, 0, 0);
    }
    private void UpdateEnergyGUI(CharacterEntityModel character, int newValue)
    {
        Debug.Log("CharacterEntityController.UpdateEnergyGUI() called for " + character.myName);
        character.characterEntityView.energyText.text = newValue.ToString();
    }
    private void UpdateStaminaGUI(CharacterEntityModel character)
    {
        Debug.Log("CharacterEntityController.UpdateStaminaGUI() called for " + character.myName);
        character.characterEntityView.staminaText.text = EntityLogic.GetTotalStamina(character).ToString();
    }
    #endregion

    // Modify Block
    #region
    public void ModifyBlock(CharacterEntityModel character, int blockGainedOrLost)
    {
        Debug.Log("CharacterEntityController.ModifyBlock() called for " + character.myName);

        int finalBlockGainValue = blockGainedOrLost;
        int characterFinalBlockValue = 0;

        // prevent block going negative
        if(finalBlockGainValue < 0)
        {
            finalBlockGainValue = 0;
        }

        // Apply block gain
        character.block += finalBlockGainValue;

        if (finalBlockGainValue > 0)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateGainBlockEffect(character.characterEntityView.transform.position, finalBlockGainValue), QueuePosition.Back, 0, 0);
        }

        // Update GUI
        characterFinalBlockValue = character.block;
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateBlockGUI(character, characterFinalBlockValue), QueuePosition.Back, 0, 0);
    }
    public void ModifyBlockOnActivationStart(CharacterEntityModel character)
    {
        Debug.Log("CharacterEntityController.ModifyBlockOnActivationStart() called for " + character.myName);

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

        character.hasActivatedThisTurn = true;
        ModifyEnergy(character, EntityLogic.GetTotalStamina(character));
        ModifyBlockOnActivationStart(character);

        // enable activated view state
        VisualEventManager.Instance.CreateVisualEvent(() => character.levelNode.SetActivatedViewState(true), QueuePosition.Back);

        // is the character player controller?
        if (character.controller == Controller.Player)
        {
            // Activate main UI canvas view
            CoroutineData cData = new CoroutineData();
            VisualEventManager.Instance.CreateVisualEvent(()=> FadeInCharacterUICanvas(character.characterEntityView, cData), cData, QueuePosition.Back);

            // Draw cards on turn start
            CardController.Instance.DrawCardsOnActivationStart(character);
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
    public void CharacterOnActivationEnd(CharacterEntityModel entity)
    {
        Debug.Log("CharacterEntityController.CharacterOnActivationEnd() called for " + entity.myName);

        // Disable end turn button clickability
        UIManager.Instance.DisableEndTurnButtonInteractions();

        // Do player character exclusive logic
        if (entity.controller == Controller.Player)
        {
            // Lose unused energy, discard hand
            ModifyEnergy(entity, -entity.energy);

            // Discard Hand
            CardController.Instance.DiscardHandOnActivationEnd(entity);

            // Fade out view
            CoroutineData fadeOutEvent = new CoroutineData();
            VisualEventManager.Instance.CreateVisualEvent(() => FadeOutCharacterUICanvas(entity.characterEntityView, fadeOutEvent), fadeOutEvent);
        }

        // Do enemy character exclusive logic
        if (entity.controller == Controller.AI)
        {
            // Brief pause at the end of enemy action, so player can process whats happened
            VisualEventManager.Instance.InsertTimeDelayInQueue(1f);

            // Set next action + intent
            StartAutoSetEnemyIntentProcess(entity);
        }

        // Disable level node activation ring view
        VisualEventManager.Instance.CreateVisualEvent(() => entity.levelNode.SetActivatedViewState(false));

        // activate the next character
        ActivationManager.Instance.ActivateNextEntity();
    }
    #endregion

    // Defender Targetting View Logic
    #region
    private void EnableDefenderTargetIndicator(CharacterEntityView view)
    {
        Debug.Log("CharacterEntityController.EnableDefenderTargetIndicator() called...");
        view.myTargetIndicator.EnableView();
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
        foreach (LevelNode node in LevelManager.Instance.allLevelNodes)
        {
            node.DisableAllExtraViews();
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
    private void StartAutoSetEnemyIntentProcess(CharacterEntityModel enemy)
    {
        Debug.Log("CharacterEntityController.StartSetEnemyIntentProcess() called...");
        SetEnemyNextAction(enemy, DetermineNextEnemyAction(enemy));
        SetEnemyTarget(enemy, DetermineTargetOfNextEnemyAction(enemy, enemy.myNextAction));
        UpdateEnemyIntentGUI(enemy);
    }
    public void UpdateEnemyIntentGUI(CharacterEntityModel enemy)
    {
        Debug.Log("CharacterEntityController.UpdateEnemyIntentGUI() called...");

        // Setup for visual event
        Sprite intentSprite = SpriteLibrary.Instance.GetIntentSpriteFromIntentEnumData(enemy.myNextAction.intentImage);
        string attackDamageString = "";

        // if attacking, calculate + enable + set damage value text
        if (enemy.myNextAction.actionType == ActionType.AttackTarget)
        {
            // Use the first EnemyActionEffect in the list to base damage calcs off of.
            EnemyActionEffect effect = enemy.myNextAction.actionEffects[0];

            // Calculate damage to display
            string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(enemy, null, null, effect);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(enemy, enemy.currentActionTarget, damageType, false, effect.baseDamage, null, null, effect);

            if (effect.attackLoops > 1)
            {
                attackDamageString = finalDamageValue.ToString() + " x " + effect.attackLoops.ToString();
            }
            else
            {
                attackDamageString = finalDamageValue.ToString();
            }
        }

        // Create Visual event
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateEnemyIntentGUIVisualEvent(enemy.characterEntityView.myIntentViewModel, intentSprite, attackDamageString));

    }
    public void SetEnemyNextAction(CharacterEntityModel enemy, EnemyAction action)
    {
        Debug.Log("CharacterEntityController.SetEnemyNextAction() called, setting action '" + action.actionName +
            "' as next action for enemy '" + enemy.myName + "'.");

        enemy.myNextAction = action;
    }
    public void SetEnemyTarget(CharacterEntityModel enemy, CharacterEntityModel target)
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
        Debug.Log("Setting Entity Color....");
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
    public void TriggerMeleeAttackAnimation(CharacterEntityView view)
    {
        view.ucmAnimator.SetTrigger("Melee Attack");
    }
    public void PlayIdleAnimation(CharacterEntityView view)
    {
        view.ucmAnimator.SetTrigger("Idle");
    }
    public void PlayRangedAttackAnimation(CharacterEntityView view)
    {
        view.ucmAnimator.SetTrigger("Shoot Bow");
    }
    public void PlaySkillAnimation(CharacterEntityView view)
    {
        view.ucmAnimator.SetTrigger("Skill One");
    }    
    public void PlayMoveAnimation(CharacterEntityView view)
    {
        view.ucmAnimator.SetTrigger("Move");
    }
    public void PlayHurtAnimation(CharacterEntityView view)
    {
        view.ucmAnimator.SetTrigger("Hurt");
    }
    public void PlayDeathAnimation(CharacterEntityView view)
    {
        view.ucmAnimator.SetTrigger("Die");
    }
    #endregion

    // Mouse + Input Logic
    #region
    public void OnCharacterMouseEnter(CharacterEntityView view)
    {
        Debug.Log("CharacterEntityController.OnCharacterMouseOver() called...");

        // Cancel this if character is dead
        if(view.character == null ||
            view.character.livingState == LivingState.Dead)
        {
            // Prevents GUI bugs when mousing over an enemy that is dying
            DisableAllDefenderTargetIndicators();
            view.character.levelNode.SetMouseOverViewState(false);
            return;
        }

        // Enable activation window glow
        view.myActivationWindow.myGlowOutline.SetActive(true);
        view.character.levelNode.SetMouseOverViewState(true);

        // Set character highlight color
        SetCharacterColor(view, highlightColour);

        // AI + Enemy exclusive logic
        if (view.character.controller == Controller.AI)
        {
            CharacterEntityModel enemy = view.character;

            DisableAllDefenderTargetIndicators();

            if (enemy.currentActionTarget != null && enemy.currentActionTarget.allegiance == Allegiance.Player)
            {
                EnableDefenderTargetIndicator(enemy.currentActionTarget.characterEntityView);
                if (enemy.levelNode != null && enemy.livingState == LivingState.Alive)
                {
                    enemy.levelNode.ConnectTargetPathToTargetNode(enemy.currentActionTarget.levelNode);
                }

            }
        }

    }
    public void OnCharacterMouseExit(CharacterEntityView view)
    {
        Debug.Log("CharacterEntityController.OnCharacterMouseExit() called...");

        // Cancel this if character is dead
        if (view.character.livingState == LivingState.Dead)
        {
            // Prevents GUI bugs when mousing over an enemy that is dying
            DisableAllDefenderTargetIndicators();
            view.character.levelNode.SetMouseOverViewState(false);
            return;
        }
        // Enable activation window glow
        view.myActivationWindow.myGlowOutline.SetActive(false);

        // Do character vm stuff
        if (view.character != null)
        {
            // Set character highlight color
            SetCharacterColor(view, normalColour);

            // Set character's level node mouse over state
            if (view.character.levelNode != null)
            {
                view.character.levelNode.SetMouseOverViewState(false);
            }
        }


        // AI + Enemy exclusive logic
        if (view.character.controller == Controller.AI)
        {
            CharacterEntityModel enemy = view.character;
            DisableAllDefenderTargetIndicators();

            if (enemy.livingState == LivingState.Alive && enemy.levelNode != null)
            {
                enemy.levelNode.SetLineViewState(false);
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
                enemy.nextActivationCount != ar.requirementTypeValue)
            {
                Debug.Log(enemyAction.actionName + " failed 'IsTurn' requirement");
                checkResults.Add(false);
            }

            // Check enough allies alive
            if (ar.requirementType == ActionRequirementType.AtLeastXAlliesAlive &&
                EnemyManager.Instance.allEnemies.Count < ar.requirementTypeValue)
            {
                Debug.Log(enemyAction.actionName + " failed 'AtLeastXAlliesAlive' requirement");
                checkResults.Add(false);
            }

            // Check havent used abilty for X turns
            if (ar.requirementType == ActionRequirementType.HaventUsedActionInXTurns)
            {
                if (enemy.myPreviousActionLog.Count == 0)
                {
                    Debug.Log(enemyAction.actionName + " passed 'HaventUsedActionInXTurns' requirement");
                    checkResults.Add(true);
                }
                else
                {
                    int loops = 0;
                    for (int index = enemy.myPreviousActionLog.Count - 1; loops < ar.requirementTypeValue; index--)
                    {
                        if (index >= 0 &&
                           index < enemy.myPreviousActionLog.Count &&
                           enemy.myPreviousActionLog[index] == enemyAction)
                        {
                            Debug.Log(enemyAction.actionName + " failed 'HaventUsedActionInXTurns' requirement");
                            checkResults.Add(false);
                        }

                        loops++;
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
                PassiveController.Instance.IsEntityAffectedByPassive(enemy.passiveManager, ar.passiveRequired.passiveName) == false)
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
        if(enemy.enemyData.allEnemyActions.Count == 1)
        {
            actionReturned = enemy.enemyData.allEnemyActions[0];
            Debug.Log("EnemyController.DetermineNextEnemyAction() returning " + actionReturned.actionName);
            return actionReturned;
        }

        // Check if an action is forced on activation one
        foreach(EnemyAction ea in enemy.enemyData.allEnemyActions)
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
            if(viableNextMoves.Count == 1)
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

        if (action.actionType == ActionType.AttackTarget || action.actionType == ActionType.DebuffTarget)
        {
            targetReturned = CharacterEntityController.Instance.AllDefenders[Random.Range(0, CharacterEntityController.Instance.AllDefenders.Count)];
        }
        else if (action.actionType == ActionType.DefendTarget)
        {
            // Get a valid target
            List<CharacterEntityModel> validTargets = new List<CharacterEntityModel>();

            // add all enemies
            validTargets.AddRange(CharacterEntityController.Instance.AllEnemies);

            // remove self from consideration
            validTargets.Remove(enemy);

            // randomly chose enemy from remaining valid choices
            if (validTargets.Count > 0)
            {
                targetReturned = validTargets[Random.Range(0, validTargets.Count)];
            }
            else
            {
                // set self as target, if no valid allies
                targetReturned = enemy;
            }

        }

        return targetReturned;
    }
    private void ExecuteEnemyNextAction(CharacterEntityModel enemy)
    {
        Debug.Log("CharacterEntityController.ExecuteEnemyNextActionCoroutine() called...");

        // Setup
        EnemyAction nextAction = enemy.myNextAction;
        bool hasMovedOffStartingNode = false;

        // Reaquire target (if the target was killed since setting the intent
        // TO DO IN FUTURE: this target reaquisition process should occur when an entity dies,
        // not right before an enemy performs its action
        // Was the target killed after the intent was decided?
        if (enemy.currentActionTarget == null)
        {
            // it was, find a new target
            enemy.currentActionTarget = DetermineTargetOfNextEnemyAction(enemy, nextAction);
        }
        // Trigger and resolve all effects of the action        
        for (int i = 0; i < nextAction.actionLoops; i++)
        {
            if (enemy != null && enemy.livingState == LivingState.Alive)
            {
                foreach (EnemyActionEffect effect in nextAction.actionEffects)
                {
                    // if this action moves enemy off its level node,
                    // remember this for later and move back to start pos
                    if (nextAction.actionType == ActionType.AttackTarget)
                    {
                        hasMovedOffStartingNode = true;
                    }

                    TriggerEnemyActionEffect(enemy, effect);
                }
            }
        }

        // POST ACTION STUFF
        // Record action
        AddActionToEnemyPastActionsLog(enemy, nextAction);

        // Move back to starting node pos, if we moved off 
        if (hasMovedOffStartingNode && enemy.livingState == LivingState.Alive)
        {
            CoroutineData cData = new CoroutineData();
            VisualEventManager.Instance.CreateVisualEvent(() => MoveEntityToNodeCentre(enemy, enemy.levelNode, cData), cData, QueuePosition.Back, 0.3f, 0);
        }
    }
    private void TriggerEnemyActionEffect(CharacterEntityModel enemy, EnemyActionEffect effect)
    {
        Debug.Log("CharacterEntityController.TriggerEnemyActionEffect() called on enemy " + enemy.myName);

        // Cache refs for visual events
        CharacterEntityModel target = enemy.currentActionTarget;

        // If no target, set self as target
        if (target == null)
        {
            target = enemy;
        }

        // Execute effect based on effect type

        // Attack Target
        if (effect.actionType == ActionType.AttackTarget)
        {
            for (int i = 0; i < effect.attackLoops; i++)
            {
                if (target != null &&
                   target.livingState == LivingState.Alive)
                {
                    // Move towards target visual event
                    CoroutineData cData = new CoroutineData();
                    VisualEventManager.Instance.CreateVisualEvent(() => MoveAttackerToTargetNodeAttackPosition(enemy, target, cData), cData);

                    // Play melee attack anim
                    VisualEventManager.Instance.CreateVisualEvent(() => TriggerMeleeAttackAnimation(enemy.characterEntityView));

                    // Calculate damage
                    string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(enemy, null, null, effect);
                    int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(enemy, target, damageType, false, effect.baseDamage, null, null, effect);

                    // Start damage sequence
                    CombatLogic.Instance.HandleDamage(finalDamageValue, enemy, target, damageType);
                }
            }
        }

        // Defend self + Defend target
        else if (effect.actionType == ActionType.DefendSelf || effect.actionType == ActionType.DefendTarget)
        {
            ModifyBlock(target, CombatLogic.Instance.CalculateBlockGainedByEffect(effect.blockGained, enemy, target));
        }

        // Defend All
        else if (effect.actionType == ActionType.DefendAll)
        {
            foreach (CharacterEntityModel ally in GetAllAlliesOfCharacter(enemy))
            {
                ModifyBlock(ally, CombatLogic.Instance.CalculateBlockGainedByEffect(effect.blockGained, enemy, ally));
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

            PassiveController.Instance.ApplyPassiveToCharacterEntity(target.passiveManager, effect.passiveApplied.passiveName, effect.passiveStacks);
        }

        // Buff All
        else if (effect.actionType == ActionType.BuffAll)
        {
            foreach (CharacterEntityModel ally in GetAllAlliesOfCharacter(enemy))
            {
                PassiveController.Instance.ApplyPassiveToCharacterEntity(ally.passiveManager, effect.passiveApplied.passiveName, effect.passiveStacks);
            }

        }

        // Debuff Target
        else if (effect.actionType == ActionType.DebuffTarget)
        {
            PassiveController.Instance.ApplyPassiveToCharacterEntity(target.passiveManager, effect.passiveApplied.passiveName, effect.passiveStacks);
        }

        // Debuff All
        else if (effect.actionType == ActionType.DebuffAll)
        {
            foreach (CharacterEntityModel enemyy in GetAllEnemiesOfCharacter(enemy))
            {
                PassiveController.Instance.ApplyPassiveToCharacterEntity(enemyy.passiveManager, effect.passiveApplied.passiveName, effect.passiveStacks);
            }

        }

        // Add Card
        else if (effect.actionType == ActionType.AddCard)
        {
            for (int i = 0; i < effect.copiesAdded; i++)
            {
                if (effect.collection == CardCollection.DiscardPile)
                {
                    // TO DO: Make a new method in CardController for this and future similar effects, like CreateCardAndAddToDiscardPile

                    //Card card = CardController.Instance.BuildCardFromCardData(effect.cardAdded, enemy.currentActionTarget.defender);
                    //CardController.Instance.AddCardToDiscardPile(enemy.currentActionTarget.defender, card);
                }
            }

        }


        // TO DO: This pause occurs even if the target or enemy is dead, how 
        // to remove this pause when this occurs?

        // Brief pause at end of each effect
        //yield return new WaitForSeconds(0.5f);
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
    public void MoveAttackerToTargetNodeAttackPosition(CharacterEntityModel attacker, CharacterEntityModel target, CoroutineData cData)
    {
        Debug.Log("CharacterEntityController.MoveAttackerToTargetNodeAttackPosition() called...");
        StartCoroutine(MoveAttackerToTargetNodeAttackPositionCoroutine(attacker, target, cData));
    }
    private IEnumerator MoveAttackerToTargetNodeAttackPositionCoroutine(CharacterEntityModel attacker, CharacterEntityModel target, CoroutineData cData)
    {
        // Set up
        bool reachedDestination = false;
        Vector3 destination = new Vector3(target.levelNode.nose.position.x, target.levelNode.nose.position.y, 0);
        float moveSpeed = 10;

        // Face direction of destination
        PositionLogic.Instance.TurnFacingTowardsLocation(attacker.characterEntityView, target.characterEntityView.transform.position);

        // Play movement animation
        PlayMoveAnimation(attacker.characterEntityView);

        while (reachedDestination == false)
        {
            attacker.characterEntityView.ucmMovementParent.transform.position = Vector2.MoveTowards(attacker.characterEntityView.ucmMovementParent.transform.position, destination, moveSpeed * Time.deltaTime);

            if (attacker.characterEntityView.ucmMovementParent.transform.position == destination)
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

        // Face direction of destination node
        PositionLogic.Instance.TurnFacingTowardsLocation(entity.characterEntityView, node.transform.position);

        // Play movement animation
        PlayMoveAnimation(entity.characterEntityView);

        // Move
        while (reachedDestination == false)
        {
            entity.characterEntityView.ucmMovementParent.transform.position = Vector2.MoveTowards(entity.characterEntityView.ucmMovementParent.transform.position, destination, moveSpeed * Time.deltaTime);

            if (entity.characterEntityView.ucmMovementParent.transform.position == destination)
            {
                Debug.Log("CharacterEntityController.MoveEntityToNodeCentreCoroutine() detected destination was reached...");
                reachedDestination = true;
            }
            yield return null;
        }

        // Reset facing, depending on living entity type
        if (entity.allegiance == Allegiance.Player)
        {
            PositionLogic.Instance.SetDirection(entity.characterEntityView, "Right");
        }
        else if (entity.allegiance == Allegiance.Enemy)
        {
            PositionLogic.Instance.SetDirection(entity.characterEntityView, "Left");
        }

        // Idle anim
        PlayIdleAnimation(entity.characterEntityView);

        // Resolve event
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

        foreach(CharacterEntityModel entity in AllCharacters)
        {
            if(!IsTargetFriendly(character, entity))
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

        if(includeSelfInSearch == false &&
            listReturned.Contains(character))
        {
            listReturned.Remove(character);
        }

        return listReturned;
    }
    #endregion

}
