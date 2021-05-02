using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New CardDataSO", menuName = "CardDataSO", order = 52)]
public class CardDataSO : ScriptableObject
{
    [BoxGroup("General Info", true, true)]
    [GUIColor("Blue")]
    [LabelWidth(100)]
    public string cardName;

    [GUIColor("Blue")]
    [BoxGroup("General Info")]
    [LabelWidth(100)]
    public bool includeInGame = false;

    [GUIColor("Blue")]
    [BoxGroup("General Info")]
    [LabelWidth(100)]
    [TextArea]
    public string cardDescription;

 

    [HorizontalGroup("Core Data", 75)]
    [HideLabel] 
    [PreviewField(75)]
    [GUIColor("Blue")]
    public Sprite cardSprite;

    [VerticalGroup("Core Data/Stats")]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public bool xEnergyCost = false;
    [VerticalGroup("Core Data/Stats")]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    [ShowIf("ShowCardEnergyCost")]
    public int cardEnergyCost;
    [VerticalGroup("Core Data/Stats")]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public CardType cardType;
    [VerticalGroup("Core Data/Stats")]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public TargettingType targettingType;
    [VerticalGroup("Core Data/Stats")]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public TalentSchool talentSchool;
    [VerticalGroup("Core Data/Stats")]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public Rarity rarity;
  

    [BoxGroup("Upgrade Settings", true, true)]
    [LabelWidth(100)]
    [GUIColor("Yellow")]
    public bool upgradeable;

    [BoxGroup("Upgrade Settings")]
    [LabelWidth(100)]
    [GUIColor("Yellow")]
    public int upgradeLevel;

    [BoxGroup("Core Key Words", true, true)]
    [LabelWidth(100)]
    [GUIColor("Green")]
    public bool expend;
    [BoxGroup("Core Key Words")]
    [LabelWidth(100)]
    [GUIColor("Green")]
    public bool innate;
    [BoxGroup("Core Key Words")]
    [LabelWidth(100)]
    [GUIColor("Green")]
    public bool immutable;
    [BoxGroup("Core Key Words")]
    [LabelWidth(100)]
    [GUIColor("Green")]
    public bool fleeting;
    [BoxGroup("Core Key Words")]
    [LabelWidth(100)]
    [GUIColor("Green")]
    public bool unplayable;
    [BoxGroup("Core Key Words")]
    [LabelWidth(100)]
    [GUIColor("Green")]
    public bool lifeSteal;  

    [BoxGroup("Misc Key Words", true, true)]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public bool blessing;
    [BoxGroup("Misc Key Words")]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public bool sourceSpell;
    [BoxGroup("Misc Key Words")]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public bool affliction;
    [BoxGroup("Misc Key Words")]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public bool racialCard;
    [ShowIf("ShowOriginRace")]
    [BoxGroup("Misc Key Words")]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public CharacterRace originRace;

    [VerticalGroup("List Groups")]
    [LabelWidth(200)]
    public List<CustomString> customDescription;
    [VerticalGroup("List Groups")]
    [LabelWidth(200)]
    public List<CardEffect> cardEffects;
    [VerticalGroup("List Groups")]
    [LabelWidth(200)]
    public List<CardEventListener> cardEventListeners;
    [VerticalGroup("List Groups")]
    [LabelWidth(200)]
    public List<CardPassiveEffect> cardPassiveEffects;
    [VerticalGroup("List Groups")]
    [LabelWidth(200)]
    public List<KeyWordModel> keyWordModels;
   

    private Color Blue() { return Color.cyan; }
    private Color Green() { return Color.green; }
    private Color Yellow() { return Color.yellow; }

    public bool ShowCardEnergyCost()
    {
        if (xEnergyCost == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowOriginRace()
    {
        return racialCard;
    }
}





