using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PassiveDataSO", menuName = "PassiveDataSO", order = 52)]
public class PassiveDataSO : ScriptableObject
{
    [Header("Core Properties")]
    public string passiveName;
    public Sprite passiveSprite;
    public string passiveDescription;

    [Header("Secondary Properties")]
    public bool showStackCount;
}
