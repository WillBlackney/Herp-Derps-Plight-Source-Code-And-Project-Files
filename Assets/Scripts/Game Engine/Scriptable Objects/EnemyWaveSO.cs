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
    [BoxGroup("General Info")]
    [LabelWidth(100)]
    public CombatLevelRange levelRange;
    [BoxGroup("General Info")]
    [LabelWidth(100)]
    public EncounterSpriteType encounterSpriteType;
    [BoxGroup("Enemy Groupings", centerLabel: true)]
    [LabelWidth(100)]
    public List<EnemyGroup> enemyGroups;
}

[System.Serializable]
public class EnemyGroup
{
    public List<EnemyDataSO> possibleEnemies;
}


