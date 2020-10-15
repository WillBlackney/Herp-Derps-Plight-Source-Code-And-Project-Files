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
            DottedLine.Instance.DrawDottedLine(startNode.nose.position, targetNode.nose.position);
        }

        // if dragging is occuring, update card description with target info
        /*
        else
        {
            Debug.LogWarning("Mouse over node detected");

            CharacterEntityModel target = startNode.myEntity;

            if(Draggable.DraggingThis.Da is DragSpellOnTarget && target != null)
            {
                Debug.LogWarning("Ready to go, updating card description text");
                CardController.Instance.AutoUpdateCardDescriptionText(Draggable.DraggingThis.Da.CardVM().card, target);
            }
        }
        */
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
