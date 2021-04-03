using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New StoryEventDataSO", menuName = "StoryEventData", order = 52)]
public class StoryEventDataSO : ScriptableObject
{
    [BoxGroup("General Data", true, true)]
    [LabelWidth(100)]
    public string storyEventName;

    [BoxGroup("General Data")]
    [LabelWidth(100)]
    public StoryEventPageSO firstPage;

    [BoxGroup("Requirements", true, true)]
    [LabelWidth(100)]
    public bool stageOne;
    [BoxGroup("Requirements")]
    [LabelWidth(100)]
    public bool stageTwo;
    [BoxGroup("Requirements")]
    [LabelWidth(100)]
    public bool stageThree;

}
