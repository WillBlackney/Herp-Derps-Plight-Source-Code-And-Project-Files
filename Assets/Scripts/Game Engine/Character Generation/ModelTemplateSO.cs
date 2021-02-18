using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ModelTemplateSO", menuName = "ModelTemplateSO", order = 52)]
public class ModelTemplateSO : ScriptableObject
{
    [Header("Properties")]
    public CharacterRace race;
    public List<string> bodyParts = new List<string>();
}
