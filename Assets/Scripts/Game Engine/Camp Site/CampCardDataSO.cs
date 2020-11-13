using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CampCardDataSO", menuName = "CampCardDataSO", order = 52)]
public class CampCardDataSO : ScriptableObject
{
    [BoxGroup("General Info", true, true)]
    [GUIColor("Blue")]
    [LabelWidth(100)]
    public string cardName;


    [HorizontalGroup("Core Data", 75)]
    [HideLabel]
    [PreviewField(75)]
    [GUIColor("Blue")]
    public Sprite cardSprite;

    [VerticalGroup("Core Data/Stats")]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public int cardEnergyCost;

    [VerticalGroup("Core Data/Stats")]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public CampTargettingType targettingType;

    [VerticalGroup("Core Data/Stats")]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public List<CampCardTargettingCondition> targetRequirements;



    [BoxGroup("Key Words", true, true)]
    [LabelWidth(100)]
    [GUIColor("Green")]
    public bool expend;
    [BoxGroup("Key Words")]
    [LabelWidth(100)]
    [GUIColor("Green")]
    public bool innate;



    [VerticalGroup("List Groups")]
    [LabelWidth(200)]
    public List<CustomString> customDescription;
    [VerticalGroup("List Groups")]

    [LabelWidth(200)]
    public List<CampCardEffect> cardEffects;

    [VerticalGroup("List Groups")]
    [LabelWidth(200)]
    public List<KeyWordModel> keyWordModels;


    private Color Blue() { return Color.cyan; }
    private Color Green() { return Color.green; }
    private Color Yellow() { return Color.yellow; }

}


[System.Serializable]
public class CampCardTargettingCondition
{
    [LabelWidth(200)]
    public TargettingConditionType targettingConditionType;
}
public enum TargettingConditionType
{
    None = 0,
    TargetIsAlive = 1,
    TargetIsDead = 2,
    TargetHasUpgradeableCards = 3,

}
