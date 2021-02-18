using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemTemplateSO", menuName = "ItemTemplateSO", order = 52)]
public class ItemTemplateSO : ScriptableObject
{
    public ItemDataSO mainHand;
    public ItemDataSO offHand;
}
