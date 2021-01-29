using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatGenerationController : Singleton<CombatGenerationController>
{
    // Properties + Component References
    #region
    [Header("Properties")]
    [SerializeField] private EnemyWaveSO[] allEnemyWaves;
    [SerializeField] private Sprite[] encounterSprites;
    #endregion

    // Getters + Accessors
    #region
    public EnemyWaveSO[] AllEnemyWaves
    {
        get { return allEnemyWaves; }
    }
    public Sprite[] EncounterSprites
    {
        get { return encounterSprites; }
    }
    #endregion

    public CombatChoicesResult GenerateWeeklyCombatChoices()
    {
        Debug.LogWarning("CombatGenerationController.GenerateWeeklyCombatChoices() called...");
        CombatChoicesResult ccr = new CombatChoicesResult();

        // Generate 2 basic and 1 elite each week for now
        List<EnemyWaveSO> allBasicLevelOnes = new List<EnemyWaveSO>();
        List<EnemyWaveSO> allEliteLevelOnes = new List<EnemyWaveSO>();
        List<EnemyWaveSO> chosenWaves = new List<EnemyWaveSO>();

        foreach(EnemyWaveSO wave in AllEnemyWaves)
        {
            if (wave.combatDifficulty == CombatDifficulty.Basic)
                allBasicLevelOnes.Add(wave);
            else if (wave.combatDifficulty == CombatDifficulty.Elite)
                allEliteLevelOnes.Add(wave);
        }

        // Choose 2 different basic enemy waves
        for(int i = 0; i < 2; i++)
        {
            allBasicLevelOnes.Shuffle();
            chosenWaves.Add(allBasicLevelOnes[0]);
            allBasicLevelOnes.Remove(allBasicLevelOnes[0]);
        }

        // Choose an elite wave
        allEliteLevelOnes.Shuffle();
        chosenWaves.Add(allEliteLevelOnes[0]);

        // Build combat data set for each enemy wave
        foreach(EnemyWaveSO w in chosenWaves)        
            ccr.encounters.Add(GenerateCombatDataFromDataSO(w));       


        return ccr;
    }
    private CombatData GenerateCombatDataFromDataSO(EnemyWaveSO data)
    {
        Debug.LogWarning("CombatGenerationController.GenerateCombatDataFromDataSO() called...");

        CombatData ewd = new CombatData();

        ewd.encounterName = data.encounterName;
        ewd.combatDifficulty = data.combatDifficulty;
        ewd.levelRange = data.levelRange;
        ewd.encounterSpriteType = data.encounterSpriteType;

        // Choose enemies randomly
        foreach (EnemyGroup enemyGroup in data.enemyGroups)
        {
            // Choose random enemy from grouping
            ewd.enemies.Add(enemyGroup.possibleEnemies[RandomGenerator.NumberBetween(0, enemyGroup.possibleEnemies.Count - 1)]);
        }

        // Calculate + randomize base gold reward and multiplier
        float rewardMultiplier = 1;
        int baseGoldReward = RandomGenerator.NumberBetween(100, 120);

        if (data.combatDifficulty == CombatDifficulty.Elite)
            rewardMultiplier = 1.5f;
        else if (data.combatDifficulty == CombatDifficulty.Boss)
            rewardMultiplier = 2;

        ewd.goldReward = (int) (baseGoldReward * rewardMultiplier);

        // to do: calculate trinket + building token rewards
        //



        return ewd;
    }
}
public class CombatChoicesResult
{
    public List<CombatData> encounters = new List<CombatData>();
}

public enum EncounterSpriteType
{
    None = 0,
    Human1 = 1,
    Undead1 = 2,
    Orc1 = 3,
    Goblin1 = 4,
    Ent1 = 5,
}