using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataController : Singleton<CharacterDataController>
{
    [HideInInspector]public List<CharacterData> allPlayerCharacters = new List<CharacterData>();

    // Mock Data + Testing Stuff
    #region
    public void BuildAllCharactersFromMockCharacterData(CharacterData mockData)
    {
        for(int i = 0; i < 3; i++)
        {
            BuildCharacterDataFromMockData(mockData);
        }
    }
    public void BuildCharacterDataFromMockData(CharacterData mockData)
    {
        CharacterData newCharacter = new CharacterData();
        allPlayerCharacters.Add(newCharacter);

        newCharacter.myName = mockData.myName;
        newCharacter.health = mockData.health;
        newCharacter.maxHealth = mockData.maxHealth;

        newCharacter.stamina = mockData.stamina;
        newCharacter.initiative = mockData.initiative;
        newCharacter.dexterity = mockData.dexterity;
        newCharacter.draw = mockData.draw;
        newCharacter.power = mockData.power;

        newCharacter.deck = new List<CardDataSO>();
        newCharacter.deck.AddRange(mockData.deck);

        newCharacter.modelParts = new List<string>();
        newCharacter.modelParts.AddRange(mockData.modelParts);

        newCharacter.passiveManager = new PassiveManagerModel();
        PassiveController.Instance.BuildPassiveManagerFromSerializedPassiveManager(newCharacter.passiveManager, mockData.serializedPassiveManager);

    }
    #endregion
}
