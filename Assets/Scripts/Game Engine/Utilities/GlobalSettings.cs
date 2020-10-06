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

    [LabelWidth(200)]
    [ShowIf("deviceMode", DeviceMode.Mobile)]
    public Vector3 hoverMovePosition;

    [LabelWidth(200)]
    [ShowIf("deviceMode", DeviceMode.Mobile)]
    public float hoverScaleAmount;

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
        return gameMode == StartingSceneSetting.CombatSceneSingle;
    }
    #endregion

    // Game Start + Application Entry Points
    #region
    private void Start()
    {
        RunApplication();
    }
    private void RunApplication()
    {
        Debug.Log("CombatTestSceneController.Start() called...");

        // Establish settings

        // this prevents mobiles from sleeping due to inactivity
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // Start application type
        if (gameMode == StartingSceneSetting.CombatSceneSingle)
        {
            StartCoroutine(RunCombatSceneStartup());
        }
        else if (gameMode == StartingSceneSetting.Standard)
        {
            StartCoroutine(RunStandardGameModeSetup());
        }


    }
    #endregion

    // Standard game logic
    #region
    private IEnumerator RunStandardGameModeSetup()
    {
        yield return null;

        MainMenuController.Instance.ShowFrontScreen();
    }
    #endregion

    // Combat testing scene logic
    #region
    private IEnumerator RunCombatSceneStartup()
    {
        yield return null;

        // Play battle theme music
        AudioManager.Instance.PlaySound(Sound.Music_Battle_Theme_1);

        // Build character data
        CharacterDataController.Instance.BuildAllCharactersFromCharacterTemplateList(characterTemplates);

        // Create player characters in scene
        CharacterEntityController.Instance.CreateAllPlayerCombatCharacters();
        //CreateTestingPlayerCharacters();

        // Spawn enemies
        EnemySpawner.Instance.SpawnEnemyWave("Basic", testingEnemyWave);

        // Start a new combat event
        ActivationManager.Instance.OnNewCombatEventStarted();

    }
    private void CreateTestingPlayerCharacters()
    {
        foreach (CharacterData data in CharacterDataController.Instance.allPlayerCharacters)
        {
            CharacterEntityController.Instance.CreatePlayerCharacter(data, LevelManager.Instance.GetNextAvailableDefenderNode());
        }
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
    CombatSceneWithProgression,
    IntegrationTesting = 2,

}
