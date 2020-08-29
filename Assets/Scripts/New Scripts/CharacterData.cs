using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharacterData 
{
    [Header("Story Properties")]
    public string myName;

    [Header("Health Properties")]
    public int health;
    public int maxHealth;

    [Header("Core Stat Properties")]
    public int stamina;
    public int initiative;
    public int draw;
    public int dexterity;
    public int power;


}
