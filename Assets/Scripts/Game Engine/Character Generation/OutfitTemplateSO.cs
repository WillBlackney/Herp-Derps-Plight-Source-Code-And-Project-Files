using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New OutfitTemplateSO", menuName = "OutfitTemplateSO", order = 52)]
public class OutfitTemplateSO : ScriptableObject
{
    public string outfitName;
    public List<string> outfitParts = new List<string>();
}
