using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterData 
{
    [Header("Story Properties")]
    public string myName;
    public string myClassName;
    public CharacterRace race;

    [Header("Passive Properties")]
    public PassiveManagerModel passiveManager = new PassiveManagerModel();

    // TO DO WAAAAY DOWN THE TRACK
    // Remove this: we only need this for starting the game
    // when not in the menu scene. This property is used to
    // create mock character data in the editor, then build
    // a character from that. in the future, characters will
    // be built in the menu scene, and this wont be needed
    public SerializedPassiveManagerModel serializedPassiveManager;

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
