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
        foreach(Enemy enemy in EnemyManager.Instance.allEnemies)
        {
            StartAutoSetEnemyIntentProcess(enemy);
        }
    }
    public void StartAutoSetEnemyIntentProcess(Enemy enemy)
    {
        Debug.Log("EnemyController.StartSetEnemyIntentProcess() called...");
        enemy.myNextAction = DetermineNextEnemyAction(enemy);
        enemy.currentActionTarget = DetermineTargetOfNextEnemyAction(enemy, enemy.myNextAction);
        UpdateEnemyIntentGUI(enemy);
    }
    public void UpdateEnemyIntentGUI(Enemy enemy)
    {
        Debug.Log("EnemyController.UpdateEnemyIntentGUI() called...");

        // Disable all views
        enemy.myIntentViewModel.valueText.gameObject.SetActive(false);

        // Start fade in effect
        enemy.myIntentViewModel.FadeInView();

        // Set intent image
        enemy.myIntentViewModel.SetIntentSprite
            (SpriteLibrary.Instance.GetIntentSpriteFromIntentEnumData(enemy.myNextAction.intentImage));

        // if attacking, calculate + enable + set damage value text
        if(enemy.myNextAction.actionType == ActionType.AttackTarget)
        {
            // Use the first EnemyActionEffect in the list to base damage calcs off of.
            EnemyActionEffect effect = enemy.myNextAction.actionEffects[0];
            // Calculate damage to display
            string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(enemy, null, null, effect);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(enemy, enemy.currentActionTarget, damageType, false, effect.baseDamage, null, null, effect);

            enemy.myIntentViewModel.valueText.gameObject.SetActive(true);

            if(effect.attackLoops > 1)
            {
                enemy.myIntentViewModel.valueText.text = finalDamageValue.ToString() + " x " + effect.attackLoops.ToString();
            }
            else
            {
                enemy.myIntentViewModel.valueText.text = finalDamageValue.ToString();
            }
          
        }            
        
    }
    #endregion

    // Determine and Execute Actions
    #region
    public void AddActionToEnemyPastActionsLog(Enemy enemy, EnemyAction action)
    {
        enemy.myPreviousActionLog.Add(action);
    }
    public bool DoesEnemyActionMeetItsRequirements(EnemyAction enemyAction, Enemy enemy)
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
               ar.requirementTypeValue < TurnChangeNotifier.Instance.currentTurnCount)
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
                StatusController.Instance.IsEntityEffectedByStatus(enemy, ar.statusRequired, ar.statusStacksRequired) == false)
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
    public EnemyAction DetermineNextEnemyAction(Enemy enemy)
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
    public LivingEntity DetermineTargetOfNextEnemyAction(Enemy enemy, EnemyAction action)
    {
        Debug.Log("EnemyController.DetermineTargetOfNextEnemyAction() called...");

        LivingEntity targetReturned = null;
        if(action.actionType == ActionType.AttackTarget || action.actionType == ActionType.DebuffTarget)
        {
            targetReturned = DefenderManager.Instance.allDefenders[Random.Range(0, DefenderManager.Instance.allDefenders.Count)];
        }
        else if (action.actionType == ActionType.DefendTarget)
        {
            // Get a valid target
            List<Enemy> validTargets = new List<Enemy>();

            // add all enemies
            validTargets.AddRange(EnemyManager.Instance.allEnemies);

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
    public OldCoroutineData ExecuteEnemyNextAction(Enemy enemy)
    {
        Debug.Log("EnemyController.ExecuteEnemyNextAction() called...");

        OldCoroutineData action = new OldCoroutineData(true);
        StartCoroutine(ExecuteEnemyNextActionCoroutine(enemy, action));
        return action;
    }
    private IEnumerator ExecuteEnemyNextActionCoroutine(Enemy enemy, OldCoroutineData action)
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
        OldCoroutineData notification = VisualEffectManager.Instance.CreateStatusEffect(enemy.transform.position, enemy.myNextAction.actionName);
        yield return new WaitForSeconds(0.5f);

        // Trigger and resolve all effects of the action        
        for(int i = 0; i < nextAction.actionLoops; i++)
        {
            if(enemy != null && enemy.inDeathProcess == false)
            {
                foreach (EnemyActionEffect effect in nextAction.actionEffects)
                {
                    if (nextAction.actionType == ActionType.AttackTarget)
                    {
                        hasMovedOffStartingNode = true;
                    }

                    OldCoroutineData effectEvent = TriggerEnemyActionEffect(enemy, effect);
                    yield return new WaitUntil(() => effectEvent.ActionResolved());
                }
            }            
        }        

        // POST ACTION STUFF

        // Record action
        AddActionToEnemyPastActionsLog(enemy, nextAction);

        // Move back to starting node pos, if we moved off 
        if (hasMovedOffStartingNode && enemy.inDeathProcess == false)
        {
            //OldCoroutineData moveBackEvent = MovementLogic.Instance.MoveEntityToNodeCentre(enemy, enemy.levelNode);
            //yield return new WaitUntil(() => moveBackEvent.ActionResolved() == true);
        }

        // Resolve
        action.coroutineCompleted = true;
    }
    public OldCoroutineData TriggerEnemyActionEffect(Enemy enemy,EnemyActionEffect effect)
    {
        OldCoroutineData action = new OldCoroutineData(true);
        StartCoroutine(TriggerEnemyActionEffectCoroutine(enemy, effect, action));
        return action; 
    }
    private IEnumerator TriggerEnemyActionEffectCoroutine(Enemy enemy, EnemyActionEffect effect, OldCoroutineData action)
    {
        // Execute effect based on effect type
        if (effect.actionType == ActionType.AttackTarget)
        {
            for(int i = 0; i < effect.attackLoops; i++)
            {
                if(enemy.currentActionTarget != null && enemy.currentActionTarget.inDeathProcess == false &&
                    enemy != null && enemy.inDeathProcess == false)
                {
                    // Move towards target
                   // OldCoroutineData moveAction = MovementLogic.Instance.MoveAttackerToTargetNodeAttackPosition(enemy, enemy.currentActionTarget);
                    //yield return new WaitUntil(() => moveAction.ActionResolved());

                    // Play melee attack anim
                    enemy.TriggerMeleeAttackAnimation();

                    // Calculate damage
                    string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(enemy, null, null, effect);
                    int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(enemy, enemy.currentActionTarget, damageType, false, effect.baseDamage, null, null, effect);

                    // Start deal damage event
                    OldCoroutineData abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, enemy, enemy.currentActionTarget, damageType);
                    yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
                }
                   
            }            
        }
        else if (effect.actionType == ActionType.DefendSelf)
        {
            enemy.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(effect.blockGained, enemy));
        }
        else if (effect.actionType == ActionType.DefendTarget)
        {
            enemy.currentActionTarget.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(effect.blockGained, enemy.currentActionTarget));
        }
        else if (effect.actionType == ActionType.BuffSelf ||
                 effect.actionType == ActionType.BuffTarget)
        {
            // Set self as target if 'BuffSelf' type
            if (effect.actionType == ActionType.BuffSelf)
            {
                enemy.currentActionTarget = enemy;
            }

            StatusController.Instance.ApplyStatusToLivingEntity(enemy.currentActionTarget, effect.statusApplied, effect.statusStacks);
        }
        else if (effect.actionType == ActionType.DebuffTarget)
        {
            StatusController.Instance.ApplyStatusToLivingEntity(enemy.currentActionTarget, effect.statusApplied, effect.statusStacks);
        }
        else if (effect.actionType == ActionType.DebuffAll)
        {
            foreach(Defender defender in DefenderManager.Instance.allDefenders)
            {
                StatusController.Instance.ApplyStatusToLivingEntity(defender, effect.statusApplied, effect.statusStacks);
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
        yield return new WaitForSeconds(0.5f);

        // Resolve
        action.coroutineCompleted = true;
    }
    public void StartEnemyActivation(Enemy enemy)
    {
        StartCoroutine(StartEnemyActivationCoroutine(enemy));
    }
    private IEnumerator StartEnemyActivationCoroutine(Enemy enemy)
    {
        OldCoroutineData actionEvent = ExecuteEnemyNextAction(enemy);
        yield return new WaitUntil(() => actionEvent.ActionResolved() == true);
        LivingEntityManager.Instance.EndEntityActivation(enemy);
    }
    #endregion

    // Mouse + Input Logic
    #region
    public void OnEnemyMouseEnter(Enemy enemy)
    {
        Debug.Log("EnemyController.OnEnemyMouseOver() called for enemy: " + enemy.myName);

        DefenderController.Instance.DisableAllDefenderTargetIndicators();

        if(enemy.currentActionTarget != null && enemy.currentActionTarget is Defender)
        {
            DefenderController.Instance.EnableDefenderTargetIndicator(enemy.currentActionTarget);
            if(enemy.levelNode != null && enemy.inDeathProcess == false)
            {
                enemy.levelNode.ConnectTargetPathToTargetNode(enemy.currentActionTarget.levelNode);
            }
            
        }
    }
    public void OnEnemyMouseExit(Enemy enemy)
    {
        Debug.Log("EnemyController.OnEnemyMouseOver() called for enemy: " + enemy.myName);

        DefenderController.Instance.DisableAllDefenderTargetIndicators();

        if (enemy.inDeathProcess == false && enemy.levelNode != null)
        {
            enemy.levelNode.SetLineViewState(false);
        }
      
    }
    #endregion


}
