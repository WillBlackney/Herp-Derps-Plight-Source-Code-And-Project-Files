using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTestSceneController : MonoBehaviour
{
    public static CombatTestSceneController Instance;
    public OldCharacterData sampleCharacterDataOne;
    public List<CardDataSO> sampleDeck;
    public EnemyWaveSO testingEnemyWave;
    public CharacterData characterDataSample;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RunCombatSceneStartup();
    }

    private void RunCombatSceneStartup()
    {
        ActivationManager.Instance.CreateSlotAndWindowHolders();
        CreateTestingPlayerCharacters();
        EnemySpawner.Instance.SpawnEnemyWave("Basic", testingEnemyWave);
        ActivationManager.Instance.OnNewCombatEventStarted();
    }
    private void CreateTestingPlayerCharacters()
    {
        for(int i =0; i < 3; i++)
        {
            // Instantiate Defender GO from prefab, get defender script ref
            /*
            GameObject defenderGO = Instantiate(PrefabHolder.Instance.defenderPrefab, transform.position, Quaternion.identity);
            Defender defender = defenderGO.GetComponent<Defender>();            

            defender.deckData.AddRange(sampleDeck);
            defender.myCharacterData = sampleCharacterDataOne;

            LevelNode startPos = LevelManager.Instance.GetNextAvailableDefenderNode();
            defender.InitializeSetup(startPos);
            */


            // NEW IMPLEMENTATION
            CharacterEntityController.Instance.CreatePlayerCharacter(characterDataSample, LevelManager.Instance.GetNextAvailableDefenderNode());
        }
        
    }
    
}
