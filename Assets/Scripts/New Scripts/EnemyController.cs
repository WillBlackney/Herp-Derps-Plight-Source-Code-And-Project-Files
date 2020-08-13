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
        enemy.ModifyCurrentStrength(data.strength);
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
            StatusController.Instance.ApplyStatusPairingToLivingEntity(enemy, sp);
        }
    }
    public void ApplyStatusPairingToEnemy(Enemy enemy, StatusPairing status)
    {
        // Setup Passives
        if (status.statusData.statusName == "Tenacious")
        {
            enemy.myPassiveManager.ModifyTenacious(status.statusStacks);
        }
        else if (status.statusData.statusName == "Enrage")
        {
            enemy.myPassiveManager.ModifyEnrage(status.statusStacks);
        }
        else if (status.statusData.statusName == "Masochist")
        {
            enemy.myPassiveManager.ModifyMasochist(status.statusStacks);
        }


        /*
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
        */
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
            // Calculate damage to display
            string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(enemy, null, null, enemy.myNextAction);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(enemy, enemy.currentActionTarget, damageType, false, enemy.myNextAction.actionValue, null, null, enemy.myNextAction);

            enemy.myIntentViewModel.valueText.gameObject.SetActive(true);
            enemy.myIntentViewModel.valueText.text = finalDamageValue.ToString();
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
                Debug.Log(enemyAction.actionName + " passed 'IsMoreThanTurn' requirement");
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
        if(action.actionType == ActionType.AttackTarget)
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
    public Action ExecuteEnemyNextAction(Enemy enemy)
    {
        Debug.Log("EnemyController.ExecuteEnemyNextAction() called...");

        Action action = new Action(true);
        StartCoroutine(ExecuteEnemyNextActionCoroutine(enemy, action));
        return action;
    }
    private IEnumerator ExecuteEnemyNextActionCoroutine(Enemy enemy, Action action)
    {
        Debug.Log("EnemyController.ExecuteEnemyNextActionCoroutine() called...");

        EnemyAction nextAction = enemy.myNextAction;
        bool hasMovedOffStartingNode = false;

        // First action efffect
        if (nextAction.actionType == ActionType.AttackTarget)
        {
            hasMovedOffStartingNode = true;

            // Move towards target
            Action moveAction = MovementLogic.Instance.MoveAttackerToTargetNodeAttackPosition(enemy, enemy.currentActionTarget);
            yield return new WaitUntil(() => moveAction.ActionResolved());

            // Play melee attack anim
            enemy.TriggerMeleeAttackAnimation();

            // Calculate damage
            string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(enemy, null, null, nextAction);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(enemy, enemy.currentActionTarget, damageType, false, nextAction.actionValue, null, null, nextAction);

            // Start deal damage event
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, enemy, enemy.currentActionTarget, damageType);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }
        else if (nextAction.actionType == ActionType.DefendSelf)
        {
            enemy.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(nextAction.actionValue, enemy));
        }
        else if(nextAction.actionType == ActionType.DefendTarget)
        {
            // Was the target killed after the intent was decided?
            if(enemy.currentActionTarget == null)
            {
                // it was, find a new target
                enemy.currentActionTarget = DetermineTargetOfNextEnemyAction(enemy, nextAction);
            }
            enemy.currentActionTarget.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(nextAction.actionValue, enemy.currentActionTarget));
        }
        else if (nextAction.actionType == ActionType.BuffSelf ||
                 nextAction.actionType == ActionType.BuffTarget)
        {
            // Set self as target if 'BuffSelf' type
            if(nextAction.actionType == ActionType.BuffSelf)
            {
                enemy.currentActionTarget = enemy;
            }

            StatusController.Instance.ApplyStatusPairingToLivingEntity(enemy.currentActionTarget, nextAction.statusApplied);
        }

        // Second action effect
        if (nextAction.secondEffect)
        {
            if (nextAction.secondActionType == ActionType.DefendSelf)
            {
                enemy.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(nextAction.secondActionValue, enemy));
            }
            else if (nextAction.secondActionType == ActionType.BuffSelf)
            {
                StatusController.Instance.ApplyStatusPairingToLivingEntity(enemy, nextAction.secondStatusApplied);
            }
        }

        // POST ACTION STUFF

        // Record action
        AddActionToEnemyPastActionsLog(enemy, nextAction);

        // Move back to starting node pos, if we moved off 
        if (hasMovedOffStartingNode && enemy.inDeathProcess == false)
        {
            Action moveBackEvent = MovementLogic.Instance.MoveEntityToNodeCentre(enemy, enemy.levelNode);
            yield return new WaitUntil(() => moveBackEvent.ActionResolved() == true);
        }

        // Resolve
        action.actionResolved = true;
    }
    public void StartEnemyActivation(Enemy enemy)
    {
        StartCoroutine(StartEnemyActivationCoroutine(enemy));
    }
    private IEnumerator StartEnemyActivationCoroutine(Enemy enemy)
    {
        Action actionEvent = ExecuteEnemyNextAction(enemy);
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
