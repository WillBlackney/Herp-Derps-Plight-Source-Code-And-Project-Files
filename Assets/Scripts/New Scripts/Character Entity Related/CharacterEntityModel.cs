using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEntityModel 
{
    [Header("Core Stats Properties")]
    public int power;
    public int dexterity;
    public int energy;
    public int stamina;
    public int initiative;
    public int draw;

    [Header("Health + Block Properties")]
    public int health;
    public int maxHealth;
    public int block;

    [Header("Location Properties ")]
    public LevelNode levelNode;

    [Header("Misc Properties")]
    public CharacterEntityView characterEntityView;
    public Allegiance allegiance;
    public Controller controller;
    public string myName;

    [Header("Misc Combat Properties")]
    public int currentInitiativeRoll;
    public bool hasActivatedThisTurn;
}
public enum Allegiance
{
    Player,
    Enemy,
}
public enum Controller
{
    Player,
    AI
}
