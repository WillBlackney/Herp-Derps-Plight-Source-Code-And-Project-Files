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

    // General Settings
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

    // XP Settings
    [Header("General XP + Level Settings")]
    [LabelWidth(200)]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [Range(1, 100)]
    public int startingLevel;

    [LabelWidth(200)]
    [Range(1, 100)]
    public int startingXpBonus;

    [LabelWidth(200)]
    [Range(1, 100)]
    public int startingMaxXp;

    [LabelWidth(200)]
    [Range(1, 100)]
    public int maxHpIncrementPerLevel;

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
    [Range(1, 100)]
    public int noDamageTakenXpReward;

    // Loot Settings
    [Header("Loot Settings")]
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

}
