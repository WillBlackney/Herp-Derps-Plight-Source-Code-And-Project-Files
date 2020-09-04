using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class LevelManager : Singleton<LevelManager>
{
    // Properties + Components
    #region
    [Header("Component References")]
    public List<LevelNode> allLevelNodes;
    #endregion  

    // Level Node Logic   
    #region
    public LevelNode GetNextAvailableDefenderNode()
    {
        LevelNode nodeReturned = null;

        List<LevelNode> orderNodes = allLevelNodes.OrderBy(o => o.nodePriority).ToList();

        foreach(LevelNode node in orderNodes)
        {
            if(node.allowedEntity == AllowedEntity.Defender && node.occupied == false)
            {
                nodeReturned = node;
                break;
            }
        }

        return nodeReturned;
    }
    public LevelNode GetNextAvailableEnemyNode()
    {
        LevelNode nodeReturned = null;

        List<LevelNode> orderNodes = allLevelNodes.OrderBy(o => o.nodePriority).ToList();

        foreach (LevelNode node in orderNodes)
        {
            if (node.allowedEntity == AllowedEntity.Enemy && node.occupied == false)
            {
                nodeReturned = node;
                break;
            }
        }

        return nodeReturned;
    }
    public void PlaceEntityAtNode(CharacterEntityModel entity, LevelNode node)
    {
        Debug.Log("LevelManager.PlaceEntityAtNode() called...");

        node.myEntity = entity;
        node.occupied = true;
        entity.levelNode = node;
        entity.characterEntityView.transform.position = node.transform.position;
    }
    public void DisconnectEntityFromNode(CharacterEntityModel entity)
    {
        entity.levelNode.DisableAllExtraViews();
        entity.levelNode.occupied = false;
        entity.levelNode.myEntity = null;
        entity.levelNode = null;
    }
    #endregion
}
