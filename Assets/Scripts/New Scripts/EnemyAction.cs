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
    public int actionLoops = 1;

    [BoxGroup("General Action Data")]
    [LabelWidth(150)]
    public List<EnemyActionEffect> actionEffects;


    [BoxGroup("Routine Data", centerLabel: true)]
    [LabelWidth(200)]
    public bool canBeConsecutive;
    [BoxGroup("Routine Data")]
    [LabelWidth(200)]
    public bool prioritiseWhenRequirementsMet;
    [BoxGroup("Routine Data")]
    [LabelWidth(100)] 
    public List<ActionRequirement> actionRequirements;

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
    AddCard,
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
    HasPassiveTrait,
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
public enum CardCollection
{
    Hand,
    DrawPile,
    DiscardPile,
    PermanentDeck,
}

[Serializable]
public class ActionRequirement
{
    public ActionRequirementType requirementType;
    [ShowIf("ShowReqValue")]
    public int requirementTypeValue;

    [ShowIf("ShowStatusRequired")]
    public StatusIconDataSO statusRequired;
    [ShowIf("ShowStatusRequired")]
    public int statusStacksRequired;

    public bool ShowStatusRequired()
    {
        if(requirementType == ActionRequirementType.HasPassiveTrait)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowReqValue()
    {
        if (requirementType != ActionRequirementType.HasPassiveTrait)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
[Serializable]
public class EnemyActionEffect
{
    [VerticalGroup("General Properties")]
    [LabelWidth(150)]
    public ActionType actionType;


    // damage target
    [VerticalGroup("Damage Properties")]
    [ShowIf("ShowDamage")]
    [LabelWidth(150)]
    public int baseDamage;

    [VerticalGroup("Damage Properties")]
    [ShowIf("ShowDamage")]
    [LabelWidth(150)]
    public AbilityDataSO.DamageType damageType;

    [VerticalGroup("Damage Properties")]
    [ShowIf("ShowDamage")]
    [LabelWidth(150)]
    public int attackLoops = 1;



    // Status properties
    [VerticalGroup("Status Properties")]
    [ShowIf("ShowStatus")]
    [LabelWidth(150)]
    public StatusIconDataSO statusApplied;

    [VerticalGroup("Status Properties")]
    [ShowIf("ShowStatus")]
    [LabelWidth(150)]
    public int statusStacks;


    // Block properties
    [VerticalGroup("Block Properties")]
    [ShowIf("ShowBlock")]
    [LabelWidth(150)]
    public int blockGained;


    // Add card properties
    [VerticalGroup("Card Properties")]
    [ShowIf("ShowCard")]
    [LabelWidth(150)]
    public CardDataSO cardAdded;

    [VerticalGroup("Card Properties")]
    [ShowIf("ShowCard")]
    [LabelWidth(150)]
    public int copiesAdded;

    [VerticalGroup("Card Properties")]
    [ShowIf("ShowCard")]
    [LabelWidth(150)]
    public CardCollection collection;



    // Inspector bools
    public bool ShowDamage()
    {
        if(actionType == ActionType.AttackAll ||
            actionType == ActionType.AttackTarget)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ShowStatus()
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
    public bool ShowBlock()
    {
        if (actionType == ActionType.DefendAll ||
            actionType == ActionType.DefendSelf ||
            actionType == ActionType.DefendTarget)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowCard()
    {
        if (actionType == ActionType.AddCard)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
public class AddBoxToDrawer<T> : OdinValueDrawer<T>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        SirenixEditorGUI.BeginBox();
        CallNextDrawer(label);
        SirenixEditorGUI.EndBox();
    }
}
