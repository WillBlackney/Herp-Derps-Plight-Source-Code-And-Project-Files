using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTestSceneController : Singleton<CombatTestSceneController>
{
    public EnemyWaveSO testingEnemyWave;
    public CharacterData characterDataSample;  

    private void Start()
    {
        Debug.Log("CombatTestSceneController.Start() called...");
        RunCombatSceneStartup();
    }

    private void RunCombatSceneStartup()
    {
        CharacterDataController.Instance.BuildAllCharactersFromMockCharacterData(characterDataSample);
        CreateTestingPlayerCharacters();
        EnemySpawner.Instance.SpawnEnemyWave("Basic", testingEnemyWave);
        ActivationManager.Instance.OnNewCombatEventStarted();
    }
    private void CreateTestingPlayerCharacters()
    {
        foreach (CharacterData data in CharacterDataController.Instance.allPlayerCharacters)
        {
            CharacterEntityController.Instance.CreatePlayerCharacter(data, LevelManager.Instance.GetNextAvailableDefenderNode());
        }

    }
    
}
