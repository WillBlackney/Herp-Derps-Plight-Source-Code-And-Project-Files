using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTestSceneController : Singleton<CombatTestSceneController>
{
    public EnemyWaveSO testingEnemyWave;
    public CharacterData characterDataSample;
    public UniversalCharacterModel sampleUCM;    

    private void Start()
    {
        Debug.Log("CombatTestSceneController.Start() called...");
        RunCombatSceneStartup();
    }

    private void RunCombatSceneStartup()
    {
        //ActivationManager.Instance.CreateSlotAndWindowHolders();
        CreateTestingPlayerCharacters();
        EnemySpawner.Instance.SpawnEnemyWave("Basic", testingEnemyWave);
        ActivationManager.Instance.OnNewCombatEventStarted();
    }
    private void CreateTestingPlayerCharacters()
    {
        for(int i =0; i < 3; i++)
        {
            // NEW IMPLEMENTATION
            CharacterEntityController.Instance.CreatePlayerCharacter(characterDataSample, LevelManager.Instance.GetNextAvailableDefenderNode());
        }
        
    }
    
}
