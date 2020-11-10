using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New EnemyDataSO", menuName = "EnemyDataSO", order = 52)]
public class EnemyDataSO : ScriptableObject
{
    // General Info
    [BoxGroup("General Info", centerLabel: true)]
    [LabelWidth(100)]
    public string enemyName;
    [BoxGroup("General Info")]
    [LabelWidth(100)]
    public CharacterModelSize enemyModelSize;
    [BoxGroup("General Info")]
    [LabelWidth(100)]
    public AudioProfileType audioProfile;

    // Core Stats
    [BoxGroup("Core Stats", centerLabel: true)]
    [LabelWidth(100)]
    public bool enableFlexibleMaxHealth;
    [BoxGroup("Core Stats")]
    [LabelWidth(100)]
    [ShowIf("ShowMaxHealthFlexAmount")]
    public int maxHealthFlexAmount;
    [BoxGroup("Core Stats")]
    [LabelWidth(100)]
    public int maxHealth;
    [BoxGroup("Core Stats")]
    [LabelWidth(100)]
    public int initiative;
    [BoxGroup("Core Stats")]
    [LabelWidth(100)]
    public int power;
    [BoxGroup("Core Stats")]
    [LabelWidth(100)]
    public int dexterity;   

    

    // Misc Stats
    [BoxGroup("Misc Stats", centerLabel: true)]
    [LabelWidth(100)]
    public bool overrideStartingHealth;
    [BoxGroup("Misc Stats")]
    [LabelWidth(100)]
    [ShowIf("ShowStartingHealth")]
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
    [GUIColor(1f, 0f, 1f)]
    public int magicResistance;

    // Passive Traits
    [BoxGroup("Passive Data", centerLabel: true)]
    [LabelWidth(100)]
    public SerializedPassiveManagerModel serializedPassiveManager;

    // Actions and routines
    [BoxGroup("Actions + Combat AI", centerLabel: true)]
    [LabelWidth(100)]
    public List<EnemyAction> allEnemyActions;

    // Character Model Data 
    [BoxGroup("Character Model Data", centerLabel: true)]
    [LabelWidth(100)]
    public List<string> allBodyParts;



    public bool ShowStartingHealth()
    {
        return overrideStartingHealth;
    }
    public bool ShowMaxHealthFlexAmount()
    {
        return enableFlexibleMaxHealth;
    }
}
