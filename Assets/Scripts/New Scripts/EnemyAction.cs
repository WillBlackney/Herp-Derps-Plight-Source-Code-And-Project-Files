using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class EnemyAction
{
    [Header("General Action Data")]
    public string actionName;
    public ActionType actionType;
    public int actionValue;
    public int actionLoops;

    [ShowIf("ShowDamageType")]
    public AbilityDataSO.DamageType damageType;

    [Header("Routine Data")]
    public bool canBeConsecutive;
    public bool prioritiseWhenRequirementsMet;
    public List<ActionRequirement> actionRequirements;

    // Odin Inspector Logic
    #region
    public bool ShowDamageType()
    {
        if (actionType == ActionType.AttackTarget ||
            actionType == ActionType.AttackAll)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

}
public enum ActionType
{
    AttackTarget,
    AttackAll,
    BuffSelf,
    BuffTarget,
    BuffAll,
    DebuffTarget,
    DebuffAll,
    DefendSelf,
    DefendTarget,
    DefendAll, 
    Sleep,
    PassTurn,
}
public enum ActionRequirementType
{
    None,
    IsTurn,
    IsMoreThanTurn,
    IsLessThanTurn,
    HealthIsLessThan,
    HealthIsMoreThan,
    AtLeastXAlliesAlive,
    HaventUsedActionInXTurns,
    ActivatedXTimesOrMore,
}

[Serializable]
public class ActionRequirement
{
    public ActionRequirementType requirementType;
    public int requirementTypeValue;
}
