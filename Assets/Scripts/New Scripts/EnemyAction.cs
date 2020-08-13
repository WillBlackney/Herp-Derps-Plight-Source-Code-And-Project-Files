using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;

[Serializable]
public class EnemyAction
{
    [BoxGroup("General Action Data", centerLabel: true)]
    [LabelWidth(150)]
    public string actionName;

    [BoxGroup("General Action Data")]
    [LabelWidth(150)]
    public ActionType actionType;

    [BoxGroup("General Action Data")]
    [LabelWidth(150)]
    public IntentImage intentImage;

    [BoxGroup("General Action Data")]
    [LabelWidth(150)]
    public int actionValue;

    [BoxGroup("General Action Data")]
    [LabelWidth(150)]
    public int actionLoops;

    [BoxGroup("General Action Data")]
    [ShowIf("ShowDamageType")]
    [LabelWidth(150)]
    public AbilityDataSO.DamageType damageType;

    [BoxGroup("General Action Data")]
    [ShowIf("ShowStatusData")]
    [LabelWidth(150)]
    public StatusPairing statusApplied;

    [BoxGroup("General Action Data")]
    [LabelWidth(150)]
    public bool secondEffect;

    [BoxGroup("General Action Data")]
    [Header("Second Effect Data")]
    [ShowIf("ShowSecondEffect")]
    [LabelWidth(200)]
    public ActionType secondActionType;
    [BoxGroup("General Action Data")]
    [ShowIf("ShowSecondEffect")]
    [LabelWidth(200)]
    public int secondActionValue;
    [BoxGroup("General Action Data")]
    [ShowIf("ShowSecondEffect")]
    [LabelWidth(200)]
    public int secondActionLoops;

    [BoxGroup("General Action Data")]
    [ShowIf("ShowSecondStatusData")]
    [LabelWidth(100)]
    public StatusPairing secondStatusApplied;

    [BoxGroup("Routine Data", centerLabel: true)]
    [LabelWidth(200)]
    public bool canBeConsecutive;
    [BoxGroup("Routine Data")]
    [LabelWidth(200)]
    public bool prioritiseWhenRequirementsMet;
    [BoxGroup("Routine Data")]
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
    #endregion

}
public class AddBoxToDrawer<T>: OdinValueDrawer<T> 
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        SirenixEditorGUI.BeginBox();
        CallNextDrawer(label);
        SirenixEditorGUI.EndBox();
    }
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

public enum IntentImage
{
    Attack,
    AttackBuff,
    AttackDebuff,
    AttackDefend,
    Buff,
    DefendBuff,
    Defend,
    GreenDebuff,
    PurpleDebuff,
    Unknown,
    Flee,

}
