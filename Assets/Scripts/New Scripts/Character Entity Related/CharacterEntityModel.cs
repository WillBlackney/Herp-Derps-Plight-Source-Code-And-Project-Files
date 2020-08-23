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

    [Header("Card Properties")]
    [HideInInspector] public List<Card> drawPile = new List<Card>();
    [HideInInspector] public List<Card> discardPile = new List<Card>();
    [HideInInspector] public List<Card> hand = new List<Card>();

    [Header("Misc Combat Properties")]
    [HideInInspector] public int currentInitiativeRoll;
    [HideInInspector] public bool hasActivatedThisTurn;
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
