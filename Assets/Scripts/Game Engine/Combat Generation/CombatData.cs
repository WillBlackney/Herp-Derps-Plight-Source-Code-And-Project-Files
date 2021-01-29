using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatData 
{
    public string encounterName;
    public CombatDifficulty combatDifficulty;
    public CombatLevelRange levelRange;
    public EncounterSpriteType encounterSpriteType;
    public List<EnemyDataSO> enemies = new List<EnemyDataSO>();

    public int goldReward;
    // to do: other reward variables in future go here (trinket, building tokens, etc)

    public Sprite GetMySprite()
    {
        Sprite s = null;
        foreach(Sprite sprite in CombatGenerationController.Instance.EncounterSprites)
        {
            if (sprite.name == encounterSpriteType.ToString())
            {
                s = sprite;
                break;
            }              
        }

        return s;
    }
}
