using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// TO DO WAAAAY DOWN THE TRACK
// We dont need to serialize this class in the future
// it is only serialized now so that we can create mock
// character data in the inspector for testing.
// Because this is serialized, we cannot serialize pmanagerModel class
// or we will get an endless serialization loop error when the two 
// scripts try to serialize each other. When we unserialize this,
// we can remove the script SerializedPassiveManagerModel from this class.

[Serializable]
public class CharacterData 
{
    [Header("Story Properties")]
    public string myName;

    [Header("Passive Properties")]
    public PassiveManagerModel passiveManager;

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
    public List<CardDataSO> deck;

    [Header("Model Properties")]
    public List<string> modelParts;

    [Header("Item Properties")]
    public ItemManagerModel itemManager = new ItemManagerModel();

}
