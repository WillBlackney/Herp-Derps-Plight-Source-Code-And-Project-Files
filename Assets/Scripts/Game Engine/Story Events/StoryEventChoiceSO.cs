using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New StoryEventChoiceSO", menuName = "StoryEventChoice", order = 52)]
public class StoryEventChoiceSO : ScriptableObject
{
    [Header("Descriptions")]
    public List<CustomString> activityDescription;
    public List<CustomString> effectDescription;

    [Header("Effects")]
    public StoryChoiceRequirement[] requirements;
    public StoryChoiceEffect[] effects;
}


[System.Serializable]
public class StoryChoiceRequirement
{
    [Header("Core Data")]
    public StoryChoiceReqType requirementType;

    // Health 
    [ShowIf("ShowFlatHealth")]
    [Header("Health Data")]
    public int healthMinimum;
    [ShowIf("ShowPercentHealth")]
    [Header("Health Data")]
    [Range(1, 100)]
    public int healthPercentMinimum;

    // Gold
    [ShowIf("ShowGoldFields")]
    [Header("Gold Data")]
    public int goldMinimum;

    // Talent
    [ShowIf("ShowTalentFields")]
    [Header("Talent Data")]
    public TalentSchool talent;
    [ShowIf("ShowTalentFields")]
    [Range(1, 2)]
    public int talentLevel;

    // Attribute
    [ShowIf("ShowAttributeFields")]
    [Header("Attribute Data")]
    public CoreAttribute attribute;
    [ShowIf("ShowAttributeFields")]
    [Range(10, 30)]
    public int attributeLevel;

    // Racial
    [ShowIf("ShowRequiredRace")]
    [Header("Racial Data")]
    public CharacterRace requiredRace;

    // Odin Showifs
    #region
    public bool ShowRequiredRace()
    {
        return requirementType == StoryChoiceReqType.Race;
    }
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
    #endregion
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

    // Max Health Mod Fields
    [Header("Max Health Modification Settings")]
    [ShowIf("ShowMaxHealthGainedOrLost")]
    public int maxHealthGainedOrLost;

    // Damage Fields
    [Header("Heal Settings")]
    [ShowIf("ShowDamageAmount")]
    public int damageAmount;

    // Item Fields
    [Header("Item Settings")]
    [ShowIf("ShowItemRewardType")]
    public ItemRewardType itemRewardType;
    [ShowIf("ShowItemGained")]
    public ItemDataSO itemGained;
    [ShowIf("ShowItemRewardType")]
    public int totalItemsGained = 1;

    // Gold Fields
    [Header("Gold Settings")]
    [ShowIf("ShowLoseAllGold")]
    public bool loseAllGold;
    [ShowIf("ShowGoldGainedOrLost")]
    public int goldGainedOrLost;

    // Card Fields
    [Header("Card Settings")]
    [ShowIf("ShowCardGained")]
    public CardDataSO cardGained;
    [ShowIf("ShowCardGained")]
    public int copiesGained = 1;
    [ShowIf("ShowRandomCard")]
    public bool randomCard;
    [ShowIf("ShowRandomCard")]
    public int randomCardAmount;

    // Combat Fields
    [Header("Card Settings")]
    [ShowIf("ShowEnemyWave")]
    public EnemyWaveSO enemyWave;




    // Odin Show Ifs
    #region
    public bool ShowEnemyWave()
    {
        return effectType == StoryChoiceEffectType.StartCombat;
    }
    public bool ShowMaxHealthGainedOrLost()
    {
        return effectType == StoryChoiceEffectType.ModifyMaxHealth;
    }
    public bool ShowCardGained()
    {
        return effectType == StoryChoiceEffectType.GainCard && randomCard == false;
    }
    public bool ShowRandomCard()
    {
        return effectType == StoryChoiceEffectType.GainCard && randomCard == true;
    }
    public bool ShowLoseAllGold()
    {
        return effectType == StoryChoiceEffectType.ModifyGold;
    }
    public bool ShowGoldGainedOrLost()
    {
        if (effectType == StoryChoiceEffectType.ModifyGold && loseAllGold == false)
            return true;
        else
            return false;
    }
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
    public bool ShowDamageAmount()
    {
        return effectType == StoryChoiceEffectType.LoseHealth;
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
    Race = 6,
  

}
public enum StoryChoiceEffectType
{
    None = 0,
    LoadPage = 1,
    FinishEvent = 2,
    GainCard = 4,
    GainHealth = 7,
    GainItem = 13,
    LoseHealth = 18,
    LoseMaxHealth = 20,
    ModifyMaxHealth = 9,
    ModifyGold = 14,  
    RemoveCard = 24,
    StartCombat = 26,
    UpgradeCard = 25,
    
  
   
  
    
    


}
