﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNode : MonoBehaviour
{
    [HideInInspector] public CharacterEntityModel myEntity;
    [HideInInspector] public bool occupied;
    public int nodePriority;
    public AllowedEntity allowedEntity;   
    public GameObject mouseOverParent;
    public GameObject activatedParent;

    [Header("Target Path Components")]
    public LineRenderer myLr;
    public GameObject myLrVisualParent;
    public RectTransform nose;
    public RectTransform attackPos;

    public void SetMouseOverViewState(bool onOrOff)
    {
        Debug.Log("LevelNode.SetMouseOverViewState() called, setting view state: " + onOrOff.ToString());

        mouseOverParent.SetActive(onOrOff);
    }
    public void SetActivatedViewState(bool onOrOff)
    {
        Debug.Log("LevelNode.SetActivatedViewState() called, setting: " + onOrOff.ToString());
        activatedParent.SetActive(onOrOff);
    }
    public void SetLineViewState(bool onOrOff)
    {
        myLrVisualParent.SetActive(onOrOff);
    }
    public void DisableAllExtraViews()
    {
        SetLineViewState(false);
    }
    public void ConnectTargetPathToTargetNode(LevelNode targetNode)
    {
        // Dont show path if player is dragging a card over the enemy
        if(Draggable.DraggingThis == null)
        {
            // Activate view
            SetLineViewState(true);
        }       

        // Clear previous path
        myLr.positionCount = 0;

        // Set new line renderer vertex points
        myLr.positionCount = 4;

        myLr.SetPosition(0, new Vector3(nose.position.x, nose.position.y, 1));
        myLr.SetPosition(1, new Vector3(attackPos.position.x, attackPos.position.y, 1));
        myLr.SetPosition(2, new Vector3(targetNode.attackPos.position.x, targetNode.attackPos.position.y, 1));
        myLr.SetPosition(3, new Vector3(targetNode.nose.position.x, targetNode.nose.position.y, 1));


    }
}
public enum AllowedEntity
{
    None, 
    All,
    Defender,
    Enemy
}