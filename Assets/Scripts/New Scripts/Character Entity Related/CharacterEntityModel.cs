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

    [Header("Health Properties")]
    public int health;
    public int maxHealth;

    [Header("Location Properties ")]
    public LevelNode levelNode;

    [Header("Misc Properties")]
    public CharacterEntityView characterEntityView;
    public Allegiance allegiance;
    public string myName;


}
public enum Allegiance
{
    Player,
    Enemy,
}
