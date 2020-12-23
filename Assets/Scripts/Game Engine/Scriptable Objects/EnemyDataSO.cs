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

    // Health Stats
    [BoxGroup("Health Attributes", centerLabel: true)]
    [LabelWidth(150)]
    public bool overrideStartingHealth;
    [BoxGroup("Health Attributes")]
    [LabelWidth(150)]
    [ShowIf("ShowStartingHealth")]
    public int startingHealth;    
    [BoxGroup("Health Attributes")]
    [LabelWidth(150)]
    public bool enableFlexibleMaxHealth;
    [BoxGroup("Health Attributes")]
    [LabelWidth(150)]
    [ShowIf("ShowMaxHealthFlexAmount")]
    public int maxHealthFlexAmount;
    [BoxGroup("Health Attributes")]
    [LabelWidth(150)]
    public int maxHealth;
    [BoxGroup("Health Attributes")]
    [LabelWidth(150)]
    public int startingBlock;

    // Core Stats
    [BoxGroup("Core Attributes", centerLabel: true)]
    [LabelWidth(100)]
    public int strength = 10;
    [BoxGroup("Core Attributes")]
    [LabelWidth(100)]
    public int intelligence = 10;
    [BoxGroup("Core Attributes")]
    [LabelWidth(100)]
    public int dexterity = 10;
    [BoxGroup("Core Attributes")]
    [LabelWidth(100)]
    public int wits = 10;

    // Core Stats
    [BoxGroup("Secondary Attributes", centerLabel: true)]
    [LabelWidth(100)]
    public int initiative = 0;
    [BoxGroup("Secondary Attributes")]
    [LabelWidth(100)]
    public int power = 0;
    [BoxGroup("Secondary Attributes")]
    [LabelWidth(100)]
    public int baseCrit = 0;



    // Misc Stats
   

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
