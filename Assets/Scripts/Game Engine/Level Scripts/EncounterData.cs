using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class EncounterData 
{
    [LabelWidth(200)]
    public EncounterType encounterType;

    [LabelWidth(200)]
    [ShowIf("ShowRandomEncounterBool")]
    public bool randomEncounter = false;

    [LabelWidth(200)]
    [ShowIf("ShowEnemyEncounterData")]
    public EnemyWaveSO enemyEncounterData;

    public bool ShowRandomEncounterBool()
    {
        if(encounterType == EncounterType.BasicEnemy ||
            encounterType == EncounterType.EliteEnemy ||
            encounterType == EncounterType.BossEnemy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowEnemyEncounterData()
    {
        if ((encounterType == EncounterType.BasicEnemy ||
            encounterType == EncounterType.EliteEnemy ||
            encounterType == EncounterType.BossEnemy) && randomEncounter == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}


