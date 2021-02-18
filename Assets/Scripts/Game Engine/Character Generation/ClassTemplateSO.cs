using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ClassTemplateSO", menuName = "ClassTemplate", order = 52)]
public class ClassTemplateSO : ScriptableObject
{
    [Header("Core Data")]
    public string templateName;

    [Header("Templates")]
    public List<DeckTemplateSO> possibleDecks = new List<DeckTemplateSO>();
    public List<OutfitTemplateSO> possibleOutfits = new List<OutfitTemplateSO>();
    public List<SerializedItemManagerModel> possibleWeapons = new List<SerializedItemManagerModel>();

    [Header("Core Attributes")]
    public int strength;
    public int intelligence;
    public int dexterity;
    public int wits;
    public int constitution;

    [Header("Secondary Attributes")]
    public int maxHealth;

    [Header("Talent Data")]
    public List<TalentPairingModel> talentPairings = new List<TalentPairingModel>();

}
