using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNode : MonoBehaviour
{
    [Header("Properties")]
    [HideInInspector] public CharacterEntityModel myEntity;
    [HideInInspector] public bool occupied;
    public int nodePriority;
    public AllowedEntity allowedEntity;

    [Header("Parent References")]
    public GameObject mouseOverParent;
    public GameObject activatedParent;

    [Header("Target Path Components")]
    public LineRenderer myLr;
    public GameObject myLrVisualParent;
    public RectTransform nose;
    public RectTransform attackPos;

}

