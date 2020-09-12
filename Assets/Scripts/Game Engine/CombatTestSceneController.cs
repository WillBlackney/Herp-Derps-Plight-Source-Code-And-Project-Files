using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CombatTestSceneController : Singleton<CombatTestSceneController>
{
    public bool runMockScene;
    public EnemyWaveSO testingEnemyWave;
    public List<CharacterTemplateSO> characterTemplates;
    public CharacterData characterDataSample;    

    private void Start()
    {
        Debug.Log("CombatTestSceneController.Start() called...");
        StartCoroutine(RunCombatSceneStartup());
    }

    private IEnumerator RunCombatSceneStartup()
    {
        // NOTE: this function runs as a coroutine to support integration testing.
        // By yielding from a frame here, we give our integration tests a chance to
        // set 'runMockScene' as false, and therefore prevent running the game scene
        // normally. When playing the game normally though, 'runMockScene' should be
        // set to true via the inspector

        yield return null;

        if (runMockScene)
        {
            //CharacterDataController.Instance.BuildAllCharactersFromCharacterTemplateList(characterTemplates);
            CharacterDataController.Instance.BuildAllCharactersFromMockCharacterData(characterDataSample);
            CreateTestingPlayerCharacters();
            EnemySpawner.Instance.SpawnEnemyWave("Basic", testingEnemyWave);
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


}
