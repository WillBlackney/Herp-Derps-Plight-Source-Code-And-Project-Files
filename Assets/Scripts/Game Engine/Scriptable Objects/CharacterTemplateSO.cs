using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New CharacterTemplateSO", menuName = "CharacterTemplateSO", order = 52)]
public class CharacterTemplateSO : ScriptableObject
{
    // General Info
    [BoxGroup("General Info", centerLabel: true)]
    [LabelWidth(100)]
    [GUIColor("Yellow")]
    public string myName;
    [BoxGroup("General Info")]
    [LabelWidth(100)]
    [GUIColor("Yellow")]
    public string myClassName;
    [BoxGroup("General Info")]
    [LabelWidth(100)]
    [GUIColor("Yellow")]
    public CharacterRace race;
    [BoxGroup("General Info")]
    [LabelWidth(100)]
    [GUIColor("Yellow")]
    public AudioProfileType audioProfile;

    // Health Stats
    [BoxGroup("Health Attributes", centerLabel: true)]
    [LabelWidth(100)]
    [GUIColor("Red")]
    public int health;
    [BoxGroup("Health Attributes")]
    [LabelWidth(100)]
    [GUIColor("Red")]
    public int maxHealth;
    [BoxGroup("Health Attributes")]
    [LabelWidth(100)]
    [GUIColor("Red")]
    public int startingBlock;

    // Core Attributes
    [BoxGroup("Core Attributes", centerLabel: true)]
    [LabelWidth(100)]
    [Range(0,20)]
    [GUIColor("Green")]
    public int strength = 10;
    [BoxGroup("Core Attributes")]
    [LabelWidth(100)]
    [Range(0, 20)]
    [GUIColor("Green")]
    public int intelligence = 10;
    [BoxGroup("Core Attributes")]
    [LabelWidth(100)]
    [Range(0, 20)]
    [GUIColor("Green")]
    public int dexterity = 10;
    [BoxGroup("Core Attributes")]
    [LabelWidth(100)]
    [Range(0, 20)]
    [GUIColor("Green")]
    public int wits = 10;


    [BoxGroup("Secondary Attributes", centerLabel: true)]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public int power = 0;
    [BoxGroup("Secondary Attributes")]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public int initiative;
    [BoxGroup("Secondary Attributes")]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public int stamina = 3;
    [BoxGroup("Secondary Attributes")]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public int draw = 4;
    [BoxGroup("Secondary Attributes")]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public int baseCrit = 0;
    [BoxGroup("Secondary Attributes")]
    [LabelWidth(100)]
    [GUIColor("Blue")]
    public int critModifier = 30;

    // Passive Traits
    [BoxGroup("Passive Data", centerLabel: true)]
    [LabelWidth(100)]
    public SerializedPassiveManagerModel serializedPassiveManager;

    [Header("Deck Properties")]
    public List<CardDataSO> deck;

    [Header("Model Properties")]
    public List<string> modelParts;

    [Header("Item Properties")]
    public SerializedItemManagerModel serializedItemManager;

    [Header("Talent Properties")]
    public List<TalentPairingModel> talentPairings;

    // GUI Colours for Odin
    private Color Blue() { return Color.cyan; }
    private Color Green() { return Color.green; }
    private Color Yellow() { return Color.yellow; }
    private Color Red() { return Color.red; }
}
