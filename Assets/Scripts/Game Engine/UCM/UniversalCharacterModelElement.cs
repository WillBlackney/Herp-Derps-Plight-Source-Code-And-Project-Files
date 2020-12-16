using System.Collections.Generic;
using UnityEngine;

public class UniversalCharacterModelElement : MonoBehaviour
{
    [Header("Core Properties + Components")]
    public int sortingOrderBonus;
    public BodyPartType bodyPartType;
    public List<ItemDataSO> itemsWithMyView;
    public List<UniversalCharacterModelElement> connectedElements;
    


}
