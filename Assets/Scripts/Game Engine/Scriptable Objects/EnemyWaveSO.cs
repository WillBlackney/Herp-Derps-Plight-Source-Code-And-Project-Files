using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New EnemyWaveSO", menuName = "EnemyWaveSO", order = 51)]
public class EnemyWaveSO : ScriptableObject
{
    [BoxGroup("General Info", centerLabel: true)]
    [LabelWidth(100)]
    public string encounterName;
    [BoxGroup("General Info")]
    [LabelWidth(100)]
    public CombatDifficulty combatDifficulty;

    [BoxGroup("Enemy Groupings", centerLabel: true)]
    [LabelWidth(100)]
    public List<EnemyGroup> enemyGroups;
    
    [Header("Custom Loot Settings")]
    [LabelWidth(100)]
    public ItemDataSO itemReward;
}

[System.Serializable]
public class EnemyGroup
{
    public List<EnemyDataSO> possibleEnemies;
}


