using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerationController : Singleton<CharacterGenerationController>
{
    // Properties + Components
    #region
    [Header("Templates")]
    public List<ClassTemplateSO> allTemplateSOs;
    public List<ModelTemplateSO> allModelTemplateSOs;

    private List<CharacterData> characterDeck = new List<CharacterData>();
    private List<CharacterRace> validCharacterRaces = new List<CharacterRace> 
    { CharacterRace.Demon, CharacterRace.Elf, CharacterRace.Ent, CharacterRace.Gnoll, CharacterRace.Goblin, CharacterRace.Human,
      CharacterRace.Orc, CharacterRace.Satyr, CharacterRace.Undead};

    #endregion

    // Accessors + Getters
    #region
    public List<CharacterData> CharacterDeck
    {
        get { return characterDeck; }
        private set { characterDeck = value; }
    }
    #endregion

    public CharacterData GenerateCharacter(ClassTemplateSO ct, CharacterRace race)
    {
        CharacterData character = new CharacterData();

        Debug.Log("CharacterDataController.ConverCharacterTemplateToCharacterData() called...");

        CharacterData newCharacter = new CharacterData();

        newCharacter.myName = "random name";
        newCharacter.myClassName = ct.templateName;
        newCharacter.race = race;
        // TO DO: change in future when we re-implement audio profiles
        newCharacter.audioProfile = AudioProfileType.HumanMale;

        newCharacter.currentLevel = GlobalSettings.Instance.startingLevel;
        newCharacter.currentMaxXP = GlobalSettings.Instance.startingMaxXp;
        CharacterDataController.Instance.ModifyCharacterTalentPoints(newCharacter, GlobalSettings.Instance.startingTalentPoints);
        CharacterDataController.Instance.ModifyCharacterAttributePoints(newCharacter, GlobalSettings.Instance.startingAttributePoints);
        CharacterDataController.Instance.HandleGainXP(newCharacter, GlobalSettings.Instance.startingXpBonus);

        newCharacter.strength = ct.strength;
        newCharacter.intelligence = ct.intelligence;
        newCharacter.wits = ct.wits;
        newCharacter.dexterity = ct.dexterity;
        newCharacter.constitution = ct.constitution;

        CharacterDataController.Instance.SetCharacterMaxHealth(newCharacter, ct.maxHealth);
        CharacterDataController.Instance.SetCharacterHealth(newCharacter, newCharacter.MaxHealthTotal);

        newCharacter.stamina = 2;
        newCharacter.initiative = 10;
        newCharacter.baseCrit = 0;
        newCharacter.critModifier = 30;
        newCharacter.baseFirstActivationDrawBonus = 2;
        newCharacter.draw = 2;
        newCharacter.power = 0;

        newCharacter.attributePoints = 0;
        newCharacter.talentPoints = 0;

        // Randomize deck
        DeckTemplateSO randomDeckData = ct.possibleDecks[RandomGenerator.NumberBetween(0, ct.possibleDecks.Count - 1)];
        newCharacter.deck = new List<CardData>();
        foreach (CardDataSO cso in randomDeckData.cards)
        {
            CharacterDataController.Instance.AddCardToCharacterDeck(newCharacter, CardController.Instance.BuildCardDataFromScriptableObjectData(cso));
        }

        // Randomize appearance + outfit
        newCharacter.modelParts = new List<string>();
        OutfitTemplateSO randomOutfit = ct.possibleOutfits[RandomGenerator.NumberBetween(0, ct.possibleOutfits.Count - 1)];
        ModelTemplateSO randomRaceModel = GetRandomModelTemplate(race);
        newCharacter.modelParts.AddRange(randomOutfit.outfitParts);
        newCharacter.modelParts.AddRange(randomRaceModel.bodyParts);

        // Randomize items
        newCharacter.itemManager = new ItemManagerModel();
        SerializedItemManagerModel randomItemSet = ct.possibleWeapons[RandomGenerator.NumberBetween(0, ct.possibleWeapons.Count - 1)];        
        ItemController.Instance.CopySerializedItemManagerIntoStandardItemManager(randomItemSet, newCharacter.itemManager);

        // Talents
        newCharacter.talentPairings = new List<TalentPairingModel>();
        foreach (TalentPairingModel tpm in ct.talentPairings)        
            newCharacter.talentPairings.Add(CharacterDataController.Instance.CloneTalentPairingModel(tpm));

        return character;
    }
    private ModelTemplateSO GetRandomModelTemplate(CharacterRace race)
    {
        List<ModelTemplateSO> validTemplates = new List<ModelTemplateSO>();

        foreach(ModelTemplateSO mt in allModelTemplateSOs)
        {
            if (mt.race == race)
                validTemplates.Add(mt);
        }

        return validTemplates[RandomGenerator.NumberBetween(0, validTemplates.Count - 1)];

    }
    private CharacterRace GetRandomRace()
    {
        return validCharacterRaces[RandomGenerator.NumberBetween(0, validCharacterRaces.Count - 1)];
    }
    public List<CharacterData> GenerateCharacterDeck()
    {
        List<CharacterData> newCharacterDeck = new List<CharacterData>();

        foreach(ClassTemplateSO ct in allTemplateSOs)
        {
            for(int i = 0; i < 2; i++)
            {
                newCharacterDeck.Add(GenerateCharacter(ct, GetRandomRace()));
            }
        }

        return newCharacterDeck;
    }
   
}
