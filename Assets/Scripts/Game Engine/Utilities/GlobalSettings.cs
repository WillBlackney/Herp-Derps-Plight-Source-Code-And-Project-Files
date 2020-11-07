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

    [Header("Combat Test Scene Settings")]
    [LabelWidth(200)]
    [ShowIf("ShowEnemyWave")]
    public EnemyWaveSO testingEnemyWave;

    [LabelWidth(200)]
    [ShowIf("ShowTestCharacterTemplates")]
    public CharacterTemplateSO[] testingCharacterTemplates;

    [LabelWidth(200)]
    [ShowIf("ShowTestingCampDeck")]
    public CampCardDataSO[] testingCampDeck;

    [LabelWidth(200)]
    [ShowIf("ShowTestingCampDeck")]
    public int testingCampPoints;

    [LabelWidth(200)]
    [ShowIf("ShowTestingCampDeck")]
    public int testingCampDraw;

    // General Info
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

    // Audio Info
    [Header("Audio Settings")]
    [LabelWidth(200)]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    public bool preventAudioProfiles = true;

    // General Info
    [Header("Card Settings")]
    [LabelWidth(200)]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    public InnateSettings innateSetting;

    [LabelWidth(200)]
    public OnPowerCardPlayedSettings onPowerCardPlayedSetting;

    [Header("Combat Settings")]
    [LabelWidth(200)]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    public InitiativeSettings initiativeSetting;

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

    // Odin bools
    public bool ShowTestSceneProperties()
    {
        return gameMode == StartingSceneSetting.CombatSceneSingle;
    }
    public bool ShowTestCharacterTemplates()
    {
        return gameMode != StartingSceneSetting.KingsBlessingEvent || gameMode != StartingSceneSetting.Standard;
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
