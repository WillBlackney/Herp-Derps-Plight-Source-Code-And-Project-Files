using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterData 
{
    [Header("Story Properties")]
    public string myName;
    public string myClassName;
    public CharacterRace race;
    public AudioProfileType audioProfile;

    [Header("Passive Properties")]
    public PassiveManagerModel passiveManager = new PassiveManagerModel();

    [Header("Health Properties")]
    public int health;
    public int maxHealth;

    [Header("Core Stat Properties")]
    public int stamina;
    public int initiative;
    public int draw;
    public int dexterity;
    public int power;

    [Header("Deck Properties")]
    [HideInInspector] public List<CardData> deck;

    [Header("Model Properties")]
    public List<string> modelParts;

    [Header("Item Properties")]
    public ItemManagerModel itemManager = new ItemManagerModel();

    [Header("Talent Properties")]
    public List<TalentPairingModel> talentPairings = new List<TalentPairingModel>();
}
