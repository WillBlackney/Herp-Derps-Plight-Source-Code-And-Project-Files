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
    [SerializeField] private Transform centrePos;
    [SerializeField] private LevelNode[] allLevelNodes;

    public LevelNode[] AllLevelNodes
    {
        get { return allLevelNodes; }
        private set { allLevelNodes = value; }
    }
    public Transform CentrePos
    {
        get { return centrePos; }
        private set { centrePos = value; }
    }
    #endregion  

    // Level Node Logic   
    #region
    public LevelNode GetNextAvailableDefenderNode()
    {
        LevelNode nodeReturned = null;

        LevelNode[] orderNodes = AllLevelNodes.OrderBy(o => o.nodePriority).ToArray();

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

        LevelNode[] orderNodes = AllLevelNodes.OrderBy(o => o.nodePriority).ToArray();

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
    public void DisconnectEntityFromNode(CharacterEntityModel entity, LevelNode node)
    {
        node.occupied = false;
        node.myEntity = null;
        entity.levelNode = null;
    }
    public void ClearAndResetAllNodes()
    {
        foreach(LevelNode node in AllLevelNodes)
        {
            node.occupied = false;
            node.myEntity = null;
            SetActivatedViewState(node, false);
            SetMouseOverViewState(node, false);
            SetLineViewState(node, false);
        }
    }
    #endregion

    // Level Node View Logic
    #region
    public void SetMouseOverViewState(LevelNode node, bool onOrOff)
    {
        Debug.Log("LevelNode.SetMouseOverViewState() called, setting view state: " + onOrOff.ToString());

        if(node != null)
        {
            node.mouseOverParent.SetActive(onOrOff);
        }        
    }
    public void SetActivatedViewState(LevelNode node, bool onOrOff)
    {
        Debug.Log("LevelNode.SetActivatedViewState() called, setting: " + onOrOff.ToString());
        node.activatedParent.SetActive(onOrOff);
    }
    public void SetLineViewState(LevelNode node, bool onOrOff)
    {
        DottedLine.Instance.DestroyAllPaths();
        node.myLrVisualParent.SetActive(onOrOff);
    }
    public void DisableAllExtraViews(LevelNode node)
    {
        SetLineViewState(node, false);
    }
    public void ConnectTargetPathToTargetNode(LevelNode startNode, LevelNode targetNode)
    {
        // Dont show path if player is dragging a card over the enemy
        if (Draggable.DraggingThis == null)
        {
            // Activate view
            //SetLineViewState(startNode, true);
            DottedLine.Instance.DestroyAllPaths();
             DottedLine.Instance.DrawDottedLine(startNode.nose.position, targetNode.nose.position);
            //DottedLine.Instance.DrawDottedLine(startNode.nose.position, targetNode.attackPos.position);
            //DottedLine.Instance.DrawDottedLine(targetNode.attackPos.position, targetNode.nose.position);
           // DottedLine.Instance.DrawDottedLine(startNode.attackPos.position, targetNode.attackPos.position);
        }

        // Clear previous path
        startNode.myLr.positionCount = 0;

        // Set new line renderer vertex points
        // startNode.myLr.positionCount = 4;
        /*
        startNode.myLr.positionCount = 3;
        startNode.myLr.SetPosition(0, new Vector3(startNode.nose.position.x, startNode.nose.position.y, 1));
        startNode.myLr.SetPosition(1, new Vector3(startNode.attackPos.position.x, startNode.attackPos.position.y, 1));
        startNode.myLr.SetPosition(2, new Vector3(targetNode.nose.position.x, targetNode.nose.position.y, 1));
        */
        // startNode.myLr.SetPosition(0, new Vector3(startNode.nose.position.x, startNode.nose.position.y, 1));
        // startNode.myLr.SetPosition(1, new Vector3(startNode.attackPos.position.x, startNode.attackPos.position.y, 1));
        // startNode.myLr.SetPosition(2, new Vector3(targetNode.attackPos.position.x, targetNode.attackPos.position.y, 1));
        // startNode.myLr.SetPosition(3, new Vector3(targetNode.nose.position.x, targetNode.nose.position.y, 1));


    }
    #endregion

    // Position Logic
    #region
    public void FlipCharacterSprite(CharacterEntityView character, FacingDirection direction)
    {
        Debug.Log("PositionLogic.FlipCharacterSprite() called...");
        float scale = Mathf.Abs(character.ucmVisualParent.transform.localScale.x);

        if (direction == FacingDirection.Right)
        {
            if (character.ucmVisualParent != null)
            {
                character.ucmVisualParent.transform.localScale = new Vector3(scale, Mathf.Abs(scale));
            }
        }

        else
        {
            if (character.ucmVisualParent != null)
            {
                character.ucmVisualParent.transform.localScale = new Vector3(-scale, Mathf.Abs(scale));
            }
        }

    }
    public void SetDirection(CharacterEntityView character, FacingDirection direction)
    {
        Debug.Log("PositionLogic. SetDirection() called, setting direction of " + direction.ToString());
        if (direction == FacingDirection.Left)
        {
            FlipCharacterSprite(character, FacingDirection.Left);
        }
        else if (direction == FacingDirection.Right)
        {
            FlipCharacterSprite(character, FacingDirection.Right);
        }
    }
    public void TurnFacingTowardsLocation(CharacterEntityView entity, Vector3 location)
    {
        if (entity.WorldPosition.x < location.x)
        {
            SetDirection(entity, FacingDirection.Right);
        }
        else if (entity.WorldPosition.x > location.x)
        {
            SetDirection(entity, FacingDirection.Left);
        }
    }
    #endregion
}
