using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GlobalSettings : Singleton<GlobalSettings>
{
    // Void Combat Test Scene Logic
    #region
    private void Start()
    {
        Debug.Log("CombatTestSceneController.Start() called...");
        StartCoroutine(RunCombatSceneStartup());
    }
    private IEnumerator RunCombatSceneStartup()
    {
        yield return null;

        if (gameMode == StartingSceneSetting.CombatSceneTesting)
        {
            // Build character data
            CharacterDataController.Instance.BuildAllCharactersFromCharacterTemplateList(characterTemplates);

            // Create player characters in scene
            CreateTestingPlayerCharacters();

            // Spawn enemies
            EnemySpawner.Instance.SpawnEnemyWave("Basic", testingEnemyWave);

            // Start a new combat event
            ActivationManager.Instance.OnNewCombatEventStarted();
        }

    }
    private void CreateTestingPlayerCharacters()
    {
        foreach (CharacterData data in CharacterDataController.Instance.allPlayerCharacters)
        {
            CharacterEntityController.Instance.CreatePlayerCharacter(data, LevelManager.Instance.GetNextAvailableDefenderNode());
        }

    }
    #endregion
    [Header("Game Mode Settings")]
    public StartingSceneSetting gameMode;

    [Header("Combat Test Scene Settings")]
    [LabelWidth(200)]
    [ShowIf("ShowTestSceneProperties")]
    public EnemyWaveSO testingEnemyWave;

    [LabelWidth(200)]
    [ShowIf("ShowTestSceneProperties")]
    public CharacterTemplateSO[] characterTemplates;

    // General Info
    [Header("Input/Device Settings")]
    [LabelWidth(200)]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    public DeviceMode deviceMode;

    [LabelWidth(200)]
    [ShowIf("deviceMode", DeviceMode.Mobile)]
    public float mouseDragSensitivity;


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

    // Odin bools
    public bool ShowTestSceneProperties()
    {
        return gameMode == StartingSceneSetting.CombatSceneTesting;
    }
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
    CombatSceneTesting = 1,
    IntegrationTesting = 2,

}
