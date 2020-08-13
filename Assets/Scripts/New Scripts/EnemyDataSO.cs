using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New EnemyDataSO", menuName = "EnemyDataSO", order = 52)]

[Serializable]
public class EnemyDataSO : ScriptableObject
{
    // General Info
    [BoxGroup("General Info", centerLabel: true)]
    [LabelWidth(100)]
    public string enemyName;
    [BoxGroup("General Info")]
    [LabelWidth(100)]
    [TextArea]
    public string description;

    // Core Stats
    [BoxGroup("Core Stats", centerLabel: true)]
    [LabelWidth(100)]
    public int maxHealth;
    [BoxGroup("Core Stats")]
    [LabelWidth(100)]
    public int strength;
    [BoxGroup("Core Stats")]
    [LabelWidth(100)]
    public int wisdom;
    [BoxGroup("Core Stats")]
    [LabelWidth(100)]
    public int dexterity;
    [BoxGroup("Core Stats")]
    [LabelWidth(100)]
    public int initiative;


    // Misc Stats
    [BoxGroup("Misc Stats", centerLabel: true)]
    [LabelWidth(100)]
    public int startingHealth;
    [BoxGroup("Misc Stats")]
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
    [BoxGroup("Passive Traits", centerLabel: true)]
    [LabelWidth(100)]
    public List<StatusPairing> allPassives;

    // Actions and routines
    [BoxGroup("Actions + Combat AI", centerLabel: true)]
    [LabelWidth(100)]
    public List<EnemyAction> allEnemyActions;

    // Character Model Data 
    [BoxGroup("Character Model Data", centerLabel: true)]
    [LabelWidth(100)]
    public List<string> allBodyParts;
}
