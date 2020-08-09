using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class EnemyAction
{
    [Header("General Action Data")]
    [LabelWidth(100)]
    public string actionName;
    [LabelWidth(100)]
    public ActionType actionType;
    [LabelWidth(100)]
    public bool forceIntentImage;
    [LabelWidth(100)]
    [ShowIf("ForceIntentImage")]
    public ActionType intentImage;
    [LabelWidth(100)]
    public int actionValue;
    [LabelWidth(100)]
    public int actionLoops;   
    [ShowIf("ShowDamageType")]
    [LabelWidth(100)]
    public AbilityDataSO.DamageType damageType;
    [ShowIf("ShowStatusData")]
    [LabelWidth(100)]
    public StatusPairing statusApplied;


    [LabelWidth(100)]
    public bool secondEffect;
    [Header("Second Effect Data")]
    [ShowIf("ShowSecondEffect")]
    [LabelWidth(200)]
    public ActionType secondActionType;
    [ShowIf("ShowSecondEffect")]
    [LabelWidth(200)]
    public int secondActionValue;
    [ShowIf("ShowSecondEffect")]
    [LabelWidth(200)]
    public int secondActionLoops;

    [ShowIf("ShowSecondStatusData")]
    [LabelWidth(100)]
    public StatusPairing secondStatusApplied;

    [Header("Routine Data")]
    [LabelWidth(200)]
    public bool canBeConsecutive;
    [LabelWidth(200)]
    public bool prioritiseWhenRequirementsMet;
    [LabelWidth(100)] 
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
    public bool ShowStatusData()
    {
        if (actionType == ActionType.BuffAll ||
            actionType == ActionType.BuffSelf ||
            actionType == ActionType.BuffTarget ||
            actionType == ActionType.DebuffAll ||
            actionType == ActionType.DebuffTarget)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowSecondStatusData()
    {
        if (secondEffect && 
            (secondActionType == ActionType.BuffAll ||
            secondActionType == ActionType.BuffSelf ||
            secondActionType == ActionType.BuffTarget ||
            secondActionType == ActionType.DebuffAll ||
            secondActionType == ActionType.DebuffTarget))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowSecondEffect()
    {
        if (secondEffect)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ForceIntentImage()
    {
        if (forceIntentImage)
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
    DefendAndBuffSelf,
    AttackTargetAndDefendSelf,
    AttackTargetAndBuffSelf,
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
    ActivatedXTimesOrLess,
}

[Serializable]
public class ActionRequirement
{
    public ActionRequirementType requirementType;
    public int requirementTypeValue;
}
