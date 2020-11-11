
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;

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
    [LabelWidth(200)]
    public bool doThisOnFirstActivation;

    [BoxGroup("General Action Data")]
    [LabelWidth(200)]
    public bool canBeConsecutive;

    [BoxGroup("General Action Data")]
    [LabelWidth(200)]
    public bool prioritiseWhenRequirementsMet;

    [BoxGroup("General Action Data")]
    [LabelWidth(100)]
    public List<ActionRequirement> actionRequirements;

    [BoxGroup("General Action Data")]
    [LabelWidth(150)]
    public List<EnemyActionEffect> actionEffects;


  

}

[Serializable]
public class ActionRequirement
{
    public ActionRequirementType requirementType;
    [ShowIf("ShowReqValue")]
    public int requirementTypeValue;

    [ShowIf("ShowStatusRequired")]
    public PassiveIconDataSO passiveRequired;
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
    public AnimationEventData[] visualEventsOnStart;

    [VerticalGroup("General Properties")]
    [LabelWidth(150)]
    public AnimationEventData[] visualEventsOnFinish;

    [VerticalGroup("General Properties")]
    [LabelWidth(150)]
    public ActionType actionType;

    [VerticalGroup("General Properties")]
    [LabelWidth(150)]
    [ShowIf("ShowDamage")]
    public int effectLoops = 1;

    // damage target
    [VerticalGroup("Damage Properties")]
    [ShowIf("ShowDamage")]
    [LabelWidth(150)]
    public int baseDamage;

    [VerticalGroup("Damage Properties")]
    [ShowIf("ShowDamage")]
    [LabelWidth(150)]
    public DamageType damageType;

    // Summon Creature Properties
    [ShowIf("ShowCharacterSummoned")]
    [LabelWidth(150)]
    public EnemyDataSO characterSummoned;

    [ShowIf("ShowCharacterSummoned")]
    [LabelWidth(150)]
    public SummonAtLocation summonedCreatureStartPosition;

    [ShowIf("ShowCharacterSummoned")]
    [LabelWidth(150)]
    public float modelFadeInSpeed;

    [ShowIf("ShowCharacterSummoned")]
    [LabelWidth(150)]
    public float uiFadeInSpeed;

    [ShowIf("ShowCharacterSummoned")]
    [LabelWidth(150)]
    public AnimationEventData[] summonedCreatureVisualEvents;

   

    // Status properties
    [VerticalGroup("Status Properties")]
    [ShowIf("ShowStatus")]
    [LabelWidth(150)]
    public PassiveIconDataSO passiveApplied;

    [VerticalGroup("Status Properties")]
    [ShowIf("ShowStatus")]
    [LabelWidth(150)]
    public int passiveStacks;


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
    public bool ShowCharacterSummoned()
    {
        return actionType == ActionType.SummonCreature;
    }
    public bool ShowDamage()
    {
        if(actionType == ActionType.AttackAllEnemies ||
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
        if (actionType == ActionType.BuffAllAllies ||
            actionType == ActionType.BuffSelf ||
            actionType == ActionType.BuffTarget ||
            actionType == ActionType.DebuffAllEnemies ||
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
        if (actionType == ActionType.DefendAllAllies ||
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
        if (actionType == ActionType.AddCardToTargetCardCollection)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

public enum SummonAtLocation
{
    None = 0,
    StartNode = 1,
    OffScreen = 2,
}

