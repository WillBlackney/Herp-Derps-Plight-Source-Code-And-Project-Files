using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New StateDataSO", menuName = "StateDataSO", order = 52)]
public class StateDataSO : ScriptableObject
{
    [HorizontalGroup("Info", 75)]
    [HideLabel]
    [PreviewField(75)]
    public Sprite stateImage;

    [VerticalGroup("Info/Stats")]
    [LabelWidth(100)]
    public StateName stateName;

    [VerticalGroup("Info/Stats")]
    [LabelWidth(100)]
    public Rarity rarity;

    [VerticalGroup("List Groups")]
    [LabelWidth(200)]
    public List<KeyWordModel> keyWordModels;

    [VerticalGroup("List Groups")]
    [LabelWidth(200)]
    public List<CustomString> customDescription;

    // to do: effects of the state
}
