using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Table : MonoBehaviour 
{
    public List<CreatureLogic> CreaturesOnTable = new List<CreatureLogic>();

    public void PlaceCreatureAt(int index, CreatureLogic creature)
    {
        CreaturesOnTable.Insert(index, creature);
    }
        
}
