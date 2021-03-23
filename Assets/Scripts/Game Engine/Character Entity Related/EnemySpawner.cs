using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    // Properties + Component References
    #region


    #endregion   

    // Enemy Spawning + Related
    #region
    public void SpawnEnemyWave(string enemyType = "Basic", EnemyWaveSO enemyWave = null)
    {
        Debug.Log("SpawnEnemyWave() Called....");
        EnemyWaveSO enemyWaveSO = enemyWave;

        // Create all enemies in wave
        foreach (EnemyGroup enemyGroup in enemyWaveSO.enemyGroups)
        {
            // Random choose enemy data
            int randomIndex = Random.Range(0, enemyGroup.possibleEnemies.Count);
            EnemyDataSO data = enemyGroup.possibleEnemies[randomIndex];

            CharacterEntityController.Instance.CreateEnemyCharacter(data, LevelManager.Instance.GetNextAvailableEnemyNode());

        }

    }
    public void PopulateWaveList(List <EnemyWaveSO> waveListToPopulate, IEnumerable<EnemyWaveSO> wavesCopiedIn)
    {
        waveListToPopulate.AddRange(wavesCopiedIn);
    }    
   
    #endregion



}
