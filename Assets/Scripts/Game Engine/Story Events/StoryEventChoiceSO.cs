using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New StoryEventChoiceSO", menuName = "StoryEventChoice", order = 52)]
public class StoryEventChoiceSO : ScriptableObject
{
    [TextArea]
    public string activityDescription;

    [TextArea]
    public string effectDescription;

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
    // General Fields
    [Header("General Settings")]
    public StoryChoiceEffectType effectType; 
    [ShowIf("ShowTarget")]
    public ChoiceEffectTarget target;

    // Load page fields
    [ShowIf("ShowPageToLoad")]
    [Header("Page Settings")]
    public StoryEventPageSO pageToLoad;

    // Healing Fields
    [Header("Heal Settings")]
    [ShowIf("ShowHealType")]
    public HealType healType;
    [ShowIf("ShowHealPercentage")]
    [Range(1,100)]
    public float healPercentage;
    [ShowIf("ShowHealAmount")]
    public int healAmount;

    // Item Fields
    [Header("Item Settings")]
    [ShowIf("ShowItemRewardType")]
    public ItemRewardType itemRewardType;
    [ShowIf("ShowItemGained")]
    public ItemDataSO itemGained;
    [ShowIf("ShowItemRewardType")]
    public int totalItemsGained = 1;



    // Odin Show Ifs
    #region
    public bool ShowTarget()
    {
        if (effectType == StoryChoiceEffectType.LoadPage ||
            effectType == StoryChoiceEffectType.FinishEvent ||
            effectType == StoryChoiceEffectType.GainItem)
            return false;
        else
            return true;
    }
    public bool ShowHealType()
    {
        return effectType == StoryChoiceEffectType.GainHealth;
    }
    public bool ShowHealAmount()
    {
        return effectType == StoryChoiceEffectType.GainHealth && healType == HealType.HealFlatAmount;
    }
    public bool ShowHealPercentage()
    {
        return effectType == StoryChoiceEffectType.GainHealth && healType == HealType.HealPercentage;
    }
    public bool ShowPageToLoad()
    {
        return effectType == StoryChoiceEffectType.LoadPage;
    }
    public bool ShowItemRewardType()
    {
        return effectType == StoryChoiceEffectType.GainItem;
    }
    public bool ShowItemGained()
    {
        return effectType == StoryChoiceEffectType.GainItem && itemRewardType == ItemRewardType.SpecificItem;
    }
    #endregion

}

public enum ChoiceEffectTarget
{
    None = 0,
    SelectedCharacter = 1,
    AllCharacters = 2,
}
public enum HealType
{
    None = 0,
    HealFlatAmount = 1,
    HealPercentage = 2,
    HealMaximum = 3,
}
public enum ItemRewardType
{
    RandomItem = 0,
    SpecificItem = 1,
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
public enum StoryChoiceEffectType
{
    None = 0,
    LoadPage = 1,
    FinishEvent = 2,
    GainAfflictionChosen =3,
    GainAfflictionAll = 4,
    GainCardChosen = 5,
    GainCardAll = 6,
    GainHealth = 7,
    GainMaxHealthChosen = 9,
    GainMaxHealthAll = 10,
    GainRandomAfflictionChosen = 11,
    GainRandomAfflictionAll = 12,
    GainItem = 13,
    GainGold = 15,
    LoseGold = 16,
    LoseAllGold = 17,
    LoseHealthChosen = 18,
    LoseHealthAll = 19,
    LoseMaxHealthChosen = 20,
    LoseMaxHealthAll = 21,
    RemoveCard = 24,
    UpgradeCard = 25,
  
   
  
    
    


}
