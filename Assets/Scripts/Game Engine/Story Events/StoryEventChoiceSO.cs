using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New StoryEventChoiceSO", menuName = "StoryEventChoice", order = 52)]
public class StoryEventChoiceSO : ScriptableObject
{
    [TextArea]
    public string activityDescription;

    public StoryChoiceRequirement[] requirements;
    public StoryChoiceEffect[] effects;
}

[System.Serializable]
public class StoryChoiceRequirement
{
    [Header("Core Data")]
    public StoryChoiceReqType requirementType;

    [ShowIf("ShowFlatHealth")]
    [Header("Health Data")]
    public int healthMinimum;
    [ShowIf("ShowPercentHealth")]
    [Header("Health Data")]
    [Range(1, 100)]
    public int healthPercentMinimum;

    [ShowIf("ShowGoldFields")]
    [Header("Gold Data")]
    public int goldMinimum;

    [ShowIf("ShowTalentFields")]
    [Header("Talent Data")]
    public TalentSchool talent;
    [ShowIf("ShowTalentFields")]
    [Range(1, 2)]
    public int talentLevel;

    [ShowIf("ShowAttributeFields")]
    [Header("Attribute Data")]
    public CoreAttribute attribute;
    [ShowIf("ShowAttributeFields")]
    [Range(10, 30)]
    public int attributeLevel;

    public bool ShowTalentFields()
    {
        return requirementType == StoryChoiceReqType.TalentLevel;
    }
    public bool ShowAttributeFields()
    {
        return requirementType == StoryChoiceReqType.AttributeLevel;
    }
    public bool ShowFlatHealth()
    {
        return requirementType == StoryChoiceReqType.AtleastXHealthFlat;
    }
    public bool ShowPercentHealth()
    {
        return requirementType == StoryChoiceReqType.AtleastXHealthPercent;
    }
    public bool ShowGoldFields()
    {
        return requirementType == StoryChoiceReqType.GoldAmount;
    }

}
[System.Serializable]
public class StoryChoiceEffect
{

}
public enum StoryChoiceReqType
{
    None = 0,
    AtleastXHealthFlat = 1,
    AtleastXHealthPercent = 2,
    AttributeLevel = 3,
    GoldAmount = 4,
    TalentLevel = 5,
  

}
