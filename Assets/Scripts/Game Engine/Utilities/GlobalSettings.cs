using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GlobalSettings : Singleton<GlobalSettings>
{
    // Properties
    #region
    [Header("Game Mode Settings")]
    [LabelWidth(200)]
    public StartingSceneSetting gameMode;

    [Header("General Test Settings Settings")]
    [LabelWidth(200)]
    [ShowIf("ShowTestCharacterTemplates")]
    public CharacterTemplateSO[] testingCharacterTemplates;

    [LabelWidth(200)]
    [ShowIf("ShowTestCharacterTemplates")]
    public StateDataSO[] testingStates;

    // Testing Settings
    [Header("Combat Test Scene Settings")]
    [LabelWidth(200)]
    [ShowIf("ShowEnemyWave")]
    public EnemyWaveSO testingEnemyWave; 

    [LabelWidth(200)]
    [ShowIf("ShowTestingCampDeck")]
    public CampCardDataSO[] testingCampDeck;

    [LabelWidth(200)]
    [ShowIf("ShowTestingCampDeck")]
    public int testingCampPoints;

    [LabelWidth(200)]
    [ShowIf("ShowTestingCampDeck")]
    public int testingCampDraw;

    // Input/Device Settings Settings
    [Header("Input/Device Settings")]
    [LabelWidth(200)]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    public DeviceMode deviceMode;

    [LabelWidth(200)]
    [ShowIf("deviceMode", DeviceMode.Mobile)]
    public float mouseDragSensitivity;

    [LabelWidth(200)]
    [ShowIf("deviceMode", DeviceMode.Mobile)]
    public Vector3 hoverMovePosition;

    [LabelWidth(200)]
    [ShowIf("deviceMode", DeviceMode.Mobile)]
    public float hoverScaleAmount;

    // General Settings
    [Header("General Settings")]
    [LabelWidth(200)]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    public int startingGold;

    // Audio Settings
    [Header("Audio Settings")]
    [LabelWidth(200)]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    public bool preventAudioProfiles = true;

    // Card Settings
    [Header("Card Settings")]
    [LabelWidth(200)]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    public InnateSettings innateSetting;

    [LabelWidth(200)]
    public OnPowerCardPlayedSettings onPowerCardPlayedSetting;

    // Combat Settings
    [Header("Combat Settings")]
    [LabelWidth(200)]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    public InitiativeSettings initiativeSetting;

    [LabelWidth(200)]
    public bool enableMaximumBlock;

    [ShowIf("ShowMaximumBlockAmount")]
    [LabelWidth(200)]
    public int maximumBlockAmount;

    [LabelWidth(200)]
    public bool blockExpiresOnActivationStart = false;

    public bool ShowMaximumBlockAmount()
    {
        return enableMaximumBlock;
    }


    // XP Settings
    [Header("General XP + Level Settings")]
    [LabelWidth(200)]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [Range(1, 10)]
    public int startingLevel;

    [LabelWidth(200)]
    [Range(0, 100)]
    public int startingXpBonus;

    [LabelWidth(200)]
    [Range(1, 100)]
    public int startingMaxXp;

    [LabelWidth(200)]
    [Range(0, 100)]
    public int maxXpIncrementPerLevel;

    [LabelWidth(200)]
    [Range(0, 100)]
    public int startingTalentPoints = 0;

    [LabelWidth(200)]
    [Range(0, 100)]
    public int startingAttributePoints = 0;

    [LabelWidth(200)]
    [Range(0, 100)]
    public int attributePointsGainedOnLevelUp = 2;

    [Header("Combat XP Related Settings")]
    [LabelWidth(200)]
    [Range(1, 100)]
    public int basicCombatXpReward;

    [LabelWidth(200)]
    [Range(1, 100)]
    public int eliteCombatXpReward;

    [LabelWidth(200)]
    [Range(1, 100)]
    public int bossCombatXpReward;

    [LabelWidth(200)]
    [Range(0, 100)]
    public int noDamageTakenXpReward;

    // Loot Settings
    [Header("Trinket Loot Settings")]
    [LabelWidth(200)]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [Range(0, 100)]
    public int basicTrinketProbability;

    [LabelWidth(200)]
    [Range(0, 100)]
    public int eliteTrinketProbability;

    [Header("Card Loot Settings")]
    [LabelWidth(200)]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [Range(1, 100)]
    public int rareCardLowerLimitProbability;

    [LabelWidth(200)]
    [Range(1, 100)]
    public int rareCardUpperLimitProbability;

    [LabelWidth(200)]
    [Range(1, 100)]
    public int epicCardLowerLimitProbability;

    [LabelWidth(200)]
    [Range(1, 100)]
    public int epicCardUpperLimitProbability;

    [LabelWidth(200)]
    [Range(1, 100)]
    public int rarePityTimer;

    [LabelWidth(200)]
    [Range(1, 100)]
    public int epicPityTimer;

    [LabelWidth(200)]
    [Range(1, 100)]
    public int upgradePityTimer;

    [Header("Gold Loot Settings")]    
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [LabelWidth(300)]
    [Range(1, 100)]
    public int basicEnemyGoldRewardLowerLimit;

    [LabelWidth(300)]
    [Range(1, 100)]
    public int basicEnemyGoldRewardUpperLimit;

    [LabelWidth(300)]
    [Range(1, 100)]
    public int eliteEnemyGoldRewardLowerLimit;

    [LabelWidth(300)]
    [Range(1, 100)]
    public int eliteEnemyGoldRewardUpperLimit;

    [LabelWidth(300)]
    [Range(1, 100)]
    public int bossEnemyGoldRewardLowerLimit;

    [LabelWidth(300)]
    [Range(1, 200)]
    public int bossEnemyGoldRewardUpperLimit;

    // Shop Settings
    [Header("Shop Settings")]    
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [LabelWidth(200)]
    [Range(1, 200)]
    public int commonCardCostLowerLimit;

    [LabelWidth(200)]
    [Range(1, 200)]
    public int commonCardCostUpperLimit;

    [LabelWidth(200)]
    [Range(1, 200)]
    public int rareCardCostLowerLimit;

    [LabelWidth(200)]
    [Range(1, 200)]
    public int rareCardCostUpperLimit;

    [LabelWidth(200)]
    [Range(1, 200)]
    public int epicCardCostLowerLimit;

    [LabelWidth(200)]
    [Range(1, 200)]
    public int epicCardCostUpperLimit;

    // Odin bools
    public bool ShowTestSceneProperties()
    {
        return gameMode == StartingSceneSetting.CombatSceneSingle;
    }
    public bool ShowTestCharacterTemplates()
    {
        if(gameMode == StartingSceneSetting.KingsBlessingEvent || gameMode == StartingSceneSetting.Standard)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool ShowEnemyWave()
    {
        return gameMode == StartingSceneSetting.CombatSceneSingle;
    }
    public bool ShowTestingCampDeck()
    {
        return gameMode == StartingSceneSetting.CampSiteEvent;
    }
    #endregion       
}


public enum InnateSettings
{
    DrawInnateCardsExtra = 0,
    PrioritiseInnate = 1,
}
public enum DeviceMode
{
    Desktop = 0,
    Mobile = 1,
}
public enum InitiativeSettings
{
    RollInitiativeOnceOnCombatStart = 0,
    RerollInitiativeEveryTurn = 1,
}
public enum OnPowerCardPlayedSettings
{
    RemoveFromPlay = 0,
    MoveToDiscardPile = 1,
    Expend = 2,
}
public enum StartingSceneSetting
{
    Standard = 0,
    CombatSceneSingle = 1,
    CombatEndLootEvent = 3,
    RecruitCharacterEvent = 4,
    IntegrationTesting = 2,
    KingsBlessingEvent = 5,
    CampSiteEvent = 6,
    ShopEventTest = 7,
    ShrineEventTest = 8

}
