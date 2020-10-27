using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataController : Singleton<CharacterDataController>
{
    // Properties
    #region
    [Header("Properties")]
    private List<CharacterData> allPlayerCharacters = new List<CharacterData>();
    public List<CharacterData> AllPlayerCharacters
    {
        get { return allPlayerCharacters; }
        private set { allPlayerCharacters = value; }

    }
    #endregion

    // Build, Add and Delete Characters 
    #region
    public void BuildCharacterRosterFromCharacterTemplateList(IEnumerable<CharacterTemplateSO> characters)
    {
        foreach(CharacterTemplateSO template in characters)
        {
            AddNewCharacterToRoster(ConverCharacterTemplateToCharacterData(template));
        }
    }
    public void AddNewCharacterToRoster(CharacterData character)
    {
        AllPlayerCharacters.Add(character);
    }

    #endregion

    // Save + Load Logic
    #region
    public void BuildMyDataFromSaveFile(SaveGameData saveFile)
    {
        AllPlayerCharacters.Clear();

        foreach (CharacterData characterData in saveFile.characters)
        {
            AllPlayerCharacters.Add(characterData);
        }
    }
    public void SaveMyDataToSaveFile(SaveGameData saveFile)
    {
        foreach (CharacterData character in AllPlayerCharacters)
        {
            saveFile.characters.Add(character);
        }
    }
    #endregion

    // Data Conversion
    #region
    public CharacterData ConverCharacterTemplateToCharacterData(CharacterTemplateSO template)
    {
        Debug.Log("CharacterDataController.ConverCharacterTemplateToCharacterData() called...");

        CharacterData newCharacter = new CharacterData();

        newCharacter.myName = template.myName;
        newCharacter.myClassName = template.myClassName;
        newCharacter.race = template.race;

        SetCharacterMaxHealth(newCharacter, template.maxHealth);
        SetCharacterHealth(newCharacter, template.health);

        newCharacter.stamina = template.stamina;
        newCharacter.initiative = template.initiative;
        newCharacter.dexterity = template.dexterity;
        newCharacter.draw = template.draw;
        newCharacter.power = template.power;

        newCharacter.deck = new List<CardData>();
        foreach(CardDataSO cso in template.deck)
        {
            AddCardToCharacterDeck(newCharacter, CardController.Instance.BuildCardDataFromScriptableObjectData(cso));
            //newCharacter.deck.Add(CardController.Instance.BuildCardDataFromScriptableObjectData(cso, newCharacter));
        }

        // UCM Data
        newCharacter.modelParts = new List<string>();
        newCharacter.modelParts.AddRange(template.modelParts);

        // Passive Data
        newCharacter.passiveManager = new PassiveManagerModel();
        PassiveController.Instance.BuildPassiveManagerFromSerializedPassiveManager(newCharacter.passiveManager, template.serializedPassiveManager);

        // Item Data
        newCharacter.itemManager = new ItemManagerModel();      
        ItemController.Instance.CopySerializedItemManagerIntoStandardItemManager(template.serializedItemManager, newCharacter.itemManager);

        // Talent Data
        newCharacter.talentPairings = new List<TalentPairingModel>();
        foreach(TalentPairingModel tpm in template.talentPairings)
        {
            newCharacter.talentPairings.Add(CloneTalentPairingModel(tpm));
        }

        return newCharacter;
    }
    private TalentPairingModel CloneTalentPairingModel(TalentPairingModel original)
    {
        TalentPairingModel tpm = new TalentPairingModel();
        tpm.talentLevel = original.talentLevel;
        tpm.talentSchool = original.talentSchool;

        return tpm;
    }

    #endregion

    // Modify Character Stats
    #region
    public void SetCharacterHealth(CharacterData data, int newValue)
    {
        Debug.Log("CharacterDataController.SetCharacterHealth() called for '" +
            data.myName + "', new health value = " + newValue.ToString());

        data.health = newValue;
    }
    public void SetCharacterMaxHealth(CharacterData data, int newValue)
    {
        Debug.Log("CharacterDataController.SetCharacterMaxHealth() called for '" +
            data.myName + "', new max health value = " + newValue.ToString());

        data.maxHealth = newValue;
    }
    #endregion

    // Modify Character Deck
    public void AddCardToCharacterDeck(CharacterData character, CardData card)
    {
        character.deck.Add(card);
    }


}
