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
    [SerializeField] private GameObject nodesVisualParent;

    [Header("Off Screen Transform References")]
    [SerializeField] private LevelNode enemyOffScreenNode;
    [SerializeField] private LevelNode defenderOffScreenNode;

    [Header("Level References")]
    [SerializeField] private GameObject kbcViewParent;
    [SerializeField] private GameObject dungeonViewParent;
    [SerializeField] private GameObject[] allDungeonParents;
    [SerializeField] private GameObject campsiteViewParent;
    [SerializeField] private GameObject[] allCampSiteParents;
    [SerializeField] private GameObject shopViewParent;
    [SerializeField] private GameObject[] allShopParents;
    [SerializeField] private GameObject shrineViewParent;
    [SerializeField] private GameObject[] allShrineParents;

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
    public LevelNode EnemyOffScreenNode
    {
        get { return enemyOffScreenNode; }
        private set { enemyOffScreenNode = value; }
    }
    public LevelNode DefenderOffScreenNode
    {
        get { return defenderOffScreenNode; }
        private set { defenderOffScreenNode = value; }
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
    public List<LevelNode> GetAllAvailableEnemyNodes()
    {
        List<LevelNode> nodes = new List<LevelNode>();

        foreach (LevelNode node in AllLevelNodes)
        {
            if (node.allowedEntity == AllowedEntity.Enemy && node.occupied == false)
            {
                nodes.Add(node);
            }
        }

        return nodes;
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
    public void HideAllNodeViews()
    {
        nodesVisualParent.SetActive(false);
    }
    public void ShowAllNodeViews()
    {
        nodesVisualParent.SetActive(true);
    }
    public void SetMouseOverViewState(LevelNode node, bool onOrOff)
    {
        Debug.Log("LevelNode.SetMouseOverViewState() called, setting view state: " + onOrOff.ToString());

        if(node != null)
        {
            node.mouseOverParent.SetActive(onOrOff);
        }        
    }
    public void DisableAllActivationRings()
    {
        foreach(LevelNode n in allLevelNodes)
        {
            SetActivatedViewState(n, false);
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

    // Scenery Logic
    #region

    // Graveyard
    public void EnableGraveyardScenery()
    {
        kbcViewParent.SetActive(true);
    }
    public void DisableGraveyardScenery()
    {
        kbcViewParent.SetActive(false);
    }

    // Dungeon
    public void EnableDungeonScenery()
    {
        DisableAllDungeons();
        dungeonViewParent.SetActive(true);
        EnableRandomDungeon();
    }
    public void DisableDungeonScenery()
    {
        DisableAllDungeons();
        dungeonViewParent.SetActive(false);
    }
    private void DisableAllDungeons()
    {
        for(int i = 0; i < allDungeonParents.Length; i++)
        {
            allDungeonParents[i].SetActive(false);
        }
    }
    private void EnableRandomDungeon()
    {
        allDungeonParents[RandomGenerator.NumberBetween(0, allDungeonParents.Length -1)].SetActive(true);

    }

    // Camp Site
    public void EnableCampSiteScenery()
    {
        DisableAllCampSites();
        campsiteViewParent.SetActive(true);
        EnableRandomCampSite();
    }
    public void DisableCampSiteScenery()
    {
        DisableAllCampSites();
        campsiteViewParent.SetActive(false);
    }
    private void DisableAllCampSites()
    {
        for (int i = 0; i < allCampSiteParents.Length; i++)
        {
            allCampSiteParents[i].SetActive(false);
        }
    }
    private void EnableRandomCampSite()
    {
        allCampSiteParents[RandomGenerator.NumberBetween(0, allCampSiteParents.Length - 1)].SetActive(true);

    }

    // Shop Event
    public void EnableShopScenery()
    {
        DisableAllShops();
        shopViewParent.SetActive(true);
        EnableRandomShop();
    }
    public void DisableShopScenery()
    {
        DisableAllShops();
        shopViewParent.SetActive(false);
    }
    private void DisableAllShops()
    {
        for (int i = 0; i < allShopParents.Length; i++)
        {
            allShopParents[i].SetActive(false);
        }
    }
    private void EnableRandomShop()
    {
        allShopParents[RandomGenerator.NumberBetween(0, allShopParents.Length - 1)].SetActive(true);

    }

    // Shrine Event
    public void EnableShrineScenery()
    {
        DisableAllShrines();
        shrineViewParent.SetActive(true);
        EnableRandomShrine();
    }
    public void DisableShrineScenery()
    {
        DisableAllShrines();
        shrineViewParent.SetActive(false);
    }
    private void DisableAllShrines()
    {
        for (int i = 0; i < allShrineParents.Length; i++)
        {
            allShrineParents[i].SetActive(false);
        }
    }
    private void EnableRandomShrine()
    {
        allShrineParents[RandomGenerator.NumberBetween(0, allShrineParents.Length - 1)].SetActive(true);

    }

    #endregion
}
