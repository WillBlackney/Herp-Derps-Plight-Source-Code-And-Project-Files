using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    // Singleton Pattern
    #region
    public static EnemyController Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    // Build Enemies
    #region
    public void BuildEnemyFromEnemyData(Enemy enemy, EnemyDataSO data)
    {
        // Build view model
        CharacterModelController.BuildModelFromStringReferences(enemy.myModel, data.allBodyParts);

        // Set general info
        enemy.myName = data.enemyName;

        // Set up health
        enemy.currentMaxHealth += data.maxHealth;
        enemy.currentHealth += data.startingHealth;

        // Set up core stats
        enemy.ModifyCurrentStrength(data.power);
        enemy.ModifyCurrentWisdom(data.wisdom);
        enemy.ModifyCurrentDexterity(data.dexterity);
        enemy.ModifyCurrentInitiative(data.initiative);

        // Set up Resistances
        enemy.ModifyPhysicalResistance(data.physicalResistance);
        enemy.ModifyPoisonResistance(data.poisonResistance);
        enemy.ModifyFireResistance(data.fireResistance);
        enemy.ModifyFrostResistance(data.frostResistance);
        enemy.ModifyShadowResistance(data.shadowResistance);
        enemy.ModifyAirResistance(data.airResistance);

        // Set up misc stats
        enemy.ModifyCurrentBlock(data.startingBlock);

        // Set up passive trais
        foreach(StatusPairing sp in data.allPassives)
        {
            StatusController.Instance.ApplyStatusToLivingEntity(enemy, sp.statusData, sp.statusStacks);
        }
    }
    #endregion

    // Intent Logic
    #region
    public void SetAllEnemyIntents()
    {
        Debug.Log("EnemyController.SetAllEnemyIntents() called...");
        foreach(CharacterEntityModel enemy in CharacterEntityController.Instance.AllEnemies)
        {
            StartAutoSetEnemyIntentProcess(enemy);
        }
    }
    public void StartAutoSetEnemyIntentProcess(CharacterEntityModel enemy)
    {
        Debug.Log("EnemyController.StartSetEnemyIntentProcess() called...");
        SetEnemyNextAction(enemy, DetermineNextEnemyAction(enemy));
        SetEnemyTarget(enemy, DetermineTargetOfNextEnemyAction(enemy, enemy.myNextAction));
        UpdateEnemyIntentGUI(enemy);
    }
    private void UpdateEnemyIntentGUI(CharacterEntityModel enemy)
    {
        Debug.Log("EnemyController.UpdateEnemyIntentGUI() called...");

        // Setup for visual event
        Sprite intentSprite = SpriteLibrary.Instance.GetIntentSpriteFromIntentEnumData(enemy.myNextAction.intentImage);
        string attackDamageString = "";

        // if attacking, calculate + enable + set damage value text
        if(enemy.myNextAction.actionType == ActionType.AttackTarget)
        {
            // Use the first EnemyActionEffect in the list to base damage calcs off of.
            EnemyActionEffect effect = enemy.myNextAction.actionEffects[0];

            // Calculate damage to display
            string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(enemy, null, null, effect);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(enemy, enemy.currentActionTarget, damageType, false, effect.baseDamage, null, null, effect);

            if(effect.attackLoops > 1)
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
        Debug.Log("EnemyController.SetEnemyNextAction() called, setting action '" + action.actionName + 
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

        Debug.Log("EnemyController.SetEnemyTarget() called, setting '" + targetName +
            "' as target for '" + enemy.myName + "'.");

        enemy.currentActionTarget = target;
    }

    #endregion

    // Determine and Execute Actions
    #region
    public void AddActionToEnemyPastActionsLog(CharacterEntityModel enemy, EnemyAction action)
    {
        enemy.myPreviousActionLog.Add(action);
    }
    public bool DoesEnemyActionMeetItsRequirements(EnemyAction enemyAction, CharacterEntityModel enemy)
    {
        Debug.Log("EnemyController.DoesEnemyActionMeetItsRequirements() called, checking action '" + enemyAction.actionName +
            "' by enemy " + enemy.myName);

        List<bool> checkResults = new List<bool>();
        bool boolReturned = false;

        foreach(ActionRequirement ar in enemyAction.actionRequirements)
        {
            Debug.Log("Checking requirement of type: " + ar.requirementType.ToString());

            // Check is turn requirement
            if(ar.requirementType == ActionRequirementType.IsTurn &&
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
                if(enemy.myPreviousActionLog.Count == 0)
                {
                    Debug.Log(enemyAction.actionName + " passed 'HaventUsedActionInXTurns' requirement");
                    checkResults.Add(true);
                }
                else
                {
                    int loops = 0;
                    for(int index = enemy.myPreviousActionLog.Count -1; loops < ar.requirementTypeValue; index--)
                    {
                        if(index >= 0 && 
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
            if(ar.requirementType == ActionRequirementType.IsMoreThanTurn &&
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

            /*
             *  TO DO: unncomment and update when we fix passive system
            // Check HasPassive
            if (ar.requirementType == ActionRequirementType.HasPassiveTrait &&
                StatusController.Instance.IsEntityEffectedByStatus(enemy, ar.statusRequired, ar.statusStacksRequired) == false)
            {
                Debug.Log(enemyAction.actionName + " failed 'HasPassive' requirement");
                checkResults.Add(false);
            }
            */
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
    public EnemyAction DetermineNextEnemyAction(CharacterEntityModel enemy)
    {
        Debug.Log("EnemyController.DetermineNextEnemyAction() called for enemy: " + enemy.myName);

        List<EnemyAction> viableNextMoves = new List<EnemyAction>();
        EnemyAction actionReturned = null;
        bool foundForcedAction = false;

        // Determine which actions are viable
        foreach(EnemyAction enemyAction in enemy.enemyData.allEnemyActions)
        {
            List<bool> checkResults = new List<bool>();

            // Check consecutive action use condition
            if(enemyAction.canBeConsecutive == false && 
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
            if(enemyAction.actionRequirements.Count > 0)
            {
                if (DoesEnemyActionMeetItsRequirements(enemyAction, enemy) == false)
                {
                    Debug.Log(enemyAction.actionName + " failed its RequirementType checks");
                    checkResults.Add(false);
                }
            }
            else if(enemyAction.actionRequirements.Count == 0)
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
        foreach(EnemyAction enemyAction in enemy.enemyData.allEnemyActions)
        {
            if(DoesEnemyActionMeetItsRequirements(enemyAction, enemy) &&
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
        if(foundForcedAction == false)
        {
            actionReturned = viableNextMoves[Random.Range(0, viableNextMoves.Count)];
        }

        Debug.Log("EnemyController.DetermineNextEnemyAction() returning " + actionReturned.actionName);
        return actionReturned;
    }
    public CharacterEntityModel DetermineTargetOfNextEnemyAction(CharacterEntityModel enemy, EnemyAction action)
    {
        Debug.Log("EnemyController.DetermineTargetOfNextEnemyAction() called for enemy: " + enemy.myName);

        CharacterEntityModel targetReturned = null;

        if(action.actionType == ActionType.AttackTarget || action.actionType == ActionType.DebuffTarget)
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
            if(validTargets.Count > 0)
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
        Debug.Log("EnemyController.ExecuteEnemyNextActionCoroutine() called...");

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

        // Ability name notification
        // TO DO: Uncomment and update when we have updated VFX manager script
        //OldCoroutineData notification = VisualEffectManager.Instance.CreateStatusEffect(enemy.transform.position, enemy.myNextAction.actionName);
        //yield return new WaitForSeconds(0.5f);

        // Trigger and resolve all effects of the action        
        for(int i = 0; i < nextAction.actionLoops; i++)
        {
            if(enemy != null && enemy.livingState == LivingState.Alive)
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
            VisualEventManager.Instance.CreateVisualEvent(() => MovementLogic.Instance.MoveEntityToNodeCentre(enemy, enemy.levelNode, cData), cData, QueuePosition.Back, 0.3f, 0);
        }
    }
    private void TriggerEnemyActionEffect(CharacterEntityModel enemy, EnemyActionEffect effect)
    {
        // Cache refs for visual events
        CharacterEntityModel target = enemy.currentActionTarget;

        // If no target, set self as target
        if(target == null)
        {
            target = enemy;
        }

        // Execute effect based on effect type
        if (effect.actionType == ActionType.AttackTarget)
        {
            for(int i = 0; i < effect.attackLoops; i++)
            {
                if(target != null &&
                   target.livingState == LivingState.Alive)
                {
                    // Move towards target visual event
                    CoroutineData cData = new CoroutineData();
                    VisualEventManager.Instance.CreateVisualEvent(() => MovementLogic.Instance.MoveAttackerToTargetNodeAttackPosition(enemy, target, cData), cData);

                    // Play melee attack anim
                    VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.TriggerMeleeAttackAnimation(enemy.characterEntityView));

                    // Calculate damage
                    string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(enemy, null, null, effect);
                    int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(enemy, target, damageType, false, effect.baseDamage, null, null, effect);

                    // Start damage sequence
                    CombatLogic.Instance.HandleDamage(finalDamageValue, enemy, target, damageType);
                }                   
            }            
        }

        
        else if (effect.actionType == ActionType.DefendSelf || effect.actionType == ActionType.DefendTarget)
        {
            CharacterEntityController.Instance.ModifyBlock(target, CombatLogic.Instance.CalculateBlockGainedByEffect(effect.blockGained, enemy, target));
        }

        else if (effect.actionType == ActionType.BuffSelf ||
                 effect.actionType == ActionType.BuffTarget)
        {
            // Set self as target if 'BuffSelf' type
            if (effect.actionType == ActionType.BuffSelf)
            {
                target = enemy;
            }

            //StatusController.Instance.ApplyStatusToLivingEntity(target, effect.statusApplied, effect.statusStacks);
        }
        else if (effect.actionType == ActionType.DebuffTarget)
        {
            //StatusController.Instance.ApplyStatusToLivingEntity(target, effect.statusApplied, effect.statusStacks);
        }
        else if (effect.actionType == ActionType.DebuffAll)
        {
            foreach(Defender defender in DefenderManager.Instance.allDefenders)
            {
                //StatusController.Instance.ApplyStatusToLivingEntity(defender, effect.statusApplied, effect.statusStacks);
            }
          
        }
        else if (effect.actionType == ActionType.AddCard)
        {
            for(int i = 0; i < effect.copiesAdded; i++)
            {
                if (effect.collection == CardCollection.DiscardPile)
                {
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
    public void StartEnemyActivation(CharacterEntityModel enemy)
    {
        ExecuteEnemyNextAction(enemy);
        CharacterEntityController.Instance.CharacterOnActivationEnd(enemy);
    }
    #endregion

    // Mouse + Input Logic
    #region
    public void OnEnemyMouseEnter(Enemy enemy)
    {
    }
    public void OnEnemyMouseExit(Enemy enemy)
    {
        Debug.Log("EnemyController.OnEnemyMouseOver() called for enemy: " + enemy.myName);

        CharacterEntityController.Instance.DisableAllDefenderTargetIndicators();

        if (enemy.inDeathProcess == false && enemy.levelNode != null)
        {
            enemy.levelNode.SetLineViewState(false);
        }
      
    }
    #endregion

    // Visual Events
    #region
    public void UpdateEnemyIntentGUIVisualEvent(IntentViewModel intentView, Sprite intentSprite, string attackDamageString)
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
}
