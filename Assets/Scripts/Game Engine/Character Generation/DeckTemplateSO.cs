using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DeckTemplateSO", menuName = "DeckTemplateSO", order = 52)]
public class DeckTemplateSO : ScriptableObject
{
    public List<CardDataSO> cards = new List<CardDataSO>();
   
}
