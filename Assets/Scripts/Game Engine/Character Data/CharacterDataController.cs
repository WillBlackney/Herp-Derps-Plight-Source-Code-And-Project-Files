using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataController : Singleton<CharacterDataController>
{
    [HideInInspector]public List<CharacterData> allPlayerCharacters = new List<CharacterData>();

    // Mock Data + Testing Stuff
    #region
    public void BuildAllCharactersFromCharacterTemplateList(List<CharacterTemplateSO> characters)
    {
        foreach(CharacterTemplateSO template in characters)
        {
            BuildCharacterDataFromCharacterTemplate(template);
        }
    }
    public void BuildAllCharactersFromMockCharacterData(CharacterData mockData)
    {
        for(int i = 0; i < 3; i++)
        {
            BuildCharacterDataFromMockData(mockData);
        }
    }
    public void BuildCharacterDataFromMockData(CharacterData mockData)
    {
        Debug.Log("CharacterDataController.BuildCharacterDataFromMockData() called...");

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

        newCharacter.itemManager = new ItemManagerModel();
        ItemController.Instance.CopyItemManagerDataIntoOtherItemManager(mockData.itemManager, newCharacter.itemManager);
    }
    public void BuildCharacterDataFromCharacterTemplate(CharacterTemplateSO template)
    {
        Debug.Log("CharacterDataController.BuildCharacterDataFromMockData() called...");

        CharacterData newCharacter = new CharacterData();
        allPlayerCharacters.Add(newCharacter);

        newCharacter.myName = template.myName;
        newCharacter.health = template.health;
        newCharacter.maxHealth = template.maxHealth;

        newCharacter.stamina = template.stamina;
        newCharacter.initiative = template.initiative;
        newCharacter.dexterity = template.dexterity;
        newCharacter.draw = template.draw;
        newCharacter.power = template.power;

        newCharacter.deck = new List<CardDataSO>();
        newCharacter.deck.AddRange(template.deck);

        newCharacter.modelParts = new List<string>();
        newCharacter.modelParts.AddRange(template.modelParts);

        newCharacter.passiveManager = new PassiveManagerModel();
        PassiveController.Instance.BuildPassiveManagerFromSerializedPassiveManager(newCharacter.passiveManager, template.serializedPassiveManager);

        newCharacter.itemManager = new ItemManagerModel();
        ItemController.Instance.CopyItemManagerDataIntoOtherItemManager(template.itemManager, newCharacter.itemManager);
    }
    #endregion
}
