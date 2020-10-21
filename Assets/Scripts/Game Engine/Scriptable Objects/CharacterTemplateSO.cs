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
    public string myName;
    [BoxGroup("General Info")]
    [LabelWidth(100)]
    public string myClassName;

    // Core Stats
    [BoxGroup("Core Stats", centerLabel: true)]
    [LabelWidth(100)]
    public int health;
    [BoxGroup("Core Stats")]
    [LabelWidth(100)]
    public int maxHealth;
    [BoxGroup("Core Stats")]
    [LabelWidth(100)]
    public int power;
    [BoxGroup("Core Stats")]
    [LabelWidth(100)]
    public int dexterity;
    [BoxGroup("Core Stats")]
    [LabelWidth(100)]
    public int stamina;
    [BoxGroup("Core Stats")]
    [LabelWidth(100)]
    public int draw;
    [BoxGroup("Core Stats")]
    [LabelWidth(100)]
    public int initiative;


    // Misc Stats
    [BoxGroup("Misc Stats", centerLabel: true)]
    [LabelWidth(100)]
    public int startingBlock;

    // Resistances
    [BoxGroup("Resistances", centerLabel: true)]
    [LabelWidth(100)]
    [Range(-100, 100)]
    [GUIColor(0.8f, 0.4f, 0.4f)]
    public int physicalResistance;
    [BoxGroup("Resistances")]
    [LabelWidth(100)]
    [Range(-100, 100)]
    [GUIColor(1f, 0f, 0f)]
    public int fireResistance;
    [BoxGroup("Resistances")]
    [LabelWidth(100)]
    [Range(-100, 100)]
    [GUIColor(0f, 1f, 1f)]
    public int frostResistance;
    [BoxGroup("Resistances")]
    [LabelWidth(100)]
    [Range(-100, 100)]
    [GUIColor(1f, 0f, 1f)]
    public int shadowResistance;
    [BoxGroup("Resistances")]
    [LabelWidth(100)]
    [Range(-100, 100)]
    [GUIColor(0.5f, 1f, 0.5f)]
    public int poisonResistance;
    [BoxGroup("Resistances")]
    [LabelWidth(100)]
    [Range(-100, 100)]
    [GUIColor(0.5f, 0.5f, 0.5f)]
    public int airResistance;

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
}
