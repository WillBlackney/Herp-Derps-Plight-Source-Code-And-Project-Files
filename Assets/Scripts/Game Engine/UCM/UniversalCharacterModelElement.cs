using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalCharacterModelElement : MonoBehaviour
{
    public int sortingOrderBonus;
    public BodyPartType bodyPartType;
    public List<ItemDataSO> itemsWithMyView;
    public List<UniversalCharacterModelElement> connectedElements;
}
