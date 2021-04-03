using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New StoryEventPageSO", menuName = "StoryEventPage", order = 52)]
public class StoryEventPageSO : ScriptableObject
{
    [TextArea]
    public string pageDescription;
    public StoryEventChoiceSO[] allChoices;

}
