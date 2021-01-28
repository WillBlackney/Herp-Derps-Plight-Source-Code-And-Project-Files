using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatData 
{
    public string encounterName;
    public CombatDifficulty combatDifficulty;
    public CombatLevelRange levelRange;
    public Sprite encounterSprite;
    public List<EnemyDataSO> enemies = new List<EnemyDataSO>();

    public int goldReward;
    // to do: other reward variables in future go here (trinket, building tokens, etc)
}
