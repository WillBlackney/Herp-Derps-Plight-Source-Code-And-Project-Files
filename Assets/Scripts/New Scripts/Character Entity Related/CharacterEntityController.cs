using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEntityController: Singleton<CharacterEntityController>
{
    // Properties + Component References
    #region
    [Header("Character Entity Lists")]
    [HideInInspector] public List<CharacterEntityModel> allCharacters = new List<CharacterEntityModel>();
    [HideInInspector] public List<CharacterEntityModel> allDefenders = new List<CharacterEntityModel>();
    [HideInInspector] public List<CharacterEntityModel> allEnemies = new List<CharacterEntityModel>();

    [Header("UCM Colours ")]
    public Color normalColour;
    public Color highlightColour;
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
        view.uiCanvas.worldCamera = CameraManager.Instance.unityCamera.mainCamera;

        // Disable main UI canvas + card UI stuff
        view.uiCanvasParent.SetActive(false);
    }
    public CharacterEntityModel CreatePlayerCharacter(CharacterData data, LevelNode position, List<CardDataSO> deckData)
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

        // Connect model to data
        model.characterData = data;
        //data.myCharacterEntityModel = model;

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
        CardController.Instance.BuildDefenderDeckFromDeckData(model, deckData);

        // Add to persistency
        allCharacters.Add(model);
        allDefenders.Add(model);

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
        allCharacters.Add(model);
        allEnemies.Add(model);

        return model;
    }
    private void SetupCharacterFromCharacterData(CharacterEntityModel character, CharacterData data)
    {
        // Establish connection from defender script to character data
        //myCharacterData.myDefenderGO = this;

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

        // Build UCM
        // TO DO IN FUTURE: Build from actual character data, not sample test scene data
        CharacterModelController.BuildModelFromModelClone(character.characterEntityView.ucm, CombatTestSceneController.Instance.sampleUCM);

        // Build activation window
        ActivationManager.Instance.CreateActivationWindow(character);

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

        VisualEventManager.Instance.CreateVisualEvent(() => UpdateStaminaGUI(character, character.stamina), QueuePosition.Back, 0, 0);
    }
    private void UpdateEnergyGUI(CharacterEntityModel character, int newValue)
    {
        Debug.Log("CharacterEntityController.UpdateEnergyGUI() called for " + character.myName);
        character.characterEntityView.energyText.text = newValue.ToString();
    }
    private void UpdateStaminaGUI(CharacterEntityModel character, int newValue)
    {
        Debug.Log("CharacterEntityController.UpdateStaminaGUI() called for " + character.myName);
        character.characterEntityView.staminaText.text = newValue.ToString();
    }
    #endregion

    // Modify Block
    #region
    public void ModifyBlock(CharacterEntityModel character, int blockGainedOrLost)
    {
        Debug.Log("CharacterEntityController.ModifyBlock() called for " + character.myName);

        int finalBlockGainValue = blockGainedOrLost;

        // prevent block going negative
        if(finalBlockGainValue < 0)
        {
            finalBlockGainValue = 0;
        }

        // Apply block gain
        character.block += finalBlockGainValue;

        if (finalBlockGainValue > 0)
        {
            //StartCoroutine(VisualEffectManager.Instance.CreateGainBlockEffect(transform.position, blockGainedOrLost));
        }

        VisualEventManager.Instance.CreateVisualEvent(() => UpdateBlockGUI(character, finalBlockGainValue), QueuePosition.Back, 0, 0);
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
            EnemyController.Instance.StartEnemyActivation(character);
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
            EnemyController.Instance.StartAutoSetEnemyIntentProcess(entity);
        }

        // Disable level node activation ring view
        VisualEventManager.Instance.CreateVisualEvent(() => entity.levelNode.SetActivatedViewState(false));

        // activate the next character
        ActivationManager.Instance.ActivateNextEntity();
    }
    #endregion

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

        cData.MarkAsCompleted();
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
        cData.MarkAsCompleted();
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
        if(cData != null)
        {
            cData.MarkAsCompleted();
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

        if(cData != null)
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

            DefenderController.Instance.DisableAllDefenderTargetIndicators();

            if (enemy.currentActionTarget != null && enemy.currentActionTarget.allegiance == Allegiance.Player)
            {
                DefenderController.Instance.EnableDefenderTargetIndicator(enemy.currentActionTarget.characterEntityView);
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
            DefenderController.Instance.DisableAllDefenderTargetIndicators();

            if (enemy.livingState == LivingState.Alive && enemy.levelNode != null)
            {
                enemy.levelNode.SetLineViewState(false);
            }
        }

    }

    #endregion
}
