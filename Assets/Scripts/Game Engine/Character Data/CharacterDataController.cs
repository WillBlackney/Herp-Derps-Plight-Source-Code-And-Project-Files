using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataController : Singleton<CharacterDataController>
{
    // Properties
    #region
    [Header("Properties")]
    private List<CharacterData> allPlayerCharacters = new List<CharacterData>();

    [SerializeField] private CharacterTemplateSO[] allCharacterTemplatesSOs;
    [SerializeField] private CharacterData[] allCharacterTemplates;


    public List<CharacterData> AllPlayerCharacters
    {
        get { return allPlayerCharacters; }
        private set { allPlayerCharacters = value; }

    }
    public CharacterData[] AllCharacterTemplates
    {
        get { return allCharacterTemplates; }
        private set { allCharacterTemplates = value; }
    }
    #endregion

    // Initialization
    private void Start()
    {
        BuildTemplateLibrary();
    }
    private void BuildTemplateLibrary()
    {
        Debug.LogWarning("CharacterDataController.BuildTemplateLibrary() called...");

        List<CharacterData> tempList = new List<CharacterData>();

        foreach (CharacterTemplateSO dataSO in allCharacterTemplatesSOs)
        {
            tempList.Add(ConvertCharacterTemplateToCharacterData(dataSO));
        }

        AllCharacterTemplates = tempList.ToArray();
    }

    // Build, Add and Delete Characters 
    #region
    public void BuildCharacterRosterFromCharacterTemplateList(IEnumerable<CharacterTemplateSO> characters)
    {
        foreach(CharacterTemplateSO template in characters)
        {
            AddNewCharacterToPlayerRoster(ConvertCharacterTemplateToCharacterData(template));
        }
    }
    public void AddNewCharacterToPlayerRoster(CharacterData character)
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

    // Data Conversion + Cloning
    #region
    public CharacterData CloneCharacterData(CharacterData original)
    {
        Debug.Log("CharacterDataController.ConverCharacterTemplateToCharacterData() called...");

        CharacterData newCharacter = new CharacterData();

        newCharacter.myName = original.myName;
        newCharacter.myClassName = original.myClassName;
        newCharacter.race = original.race;
        newCharacter.audioProfile = original.audioProfile;

        newCharacter.currentMaxXP = original.currentMaxXP;
        newCharacter.currentXP = original.currentXP;
        SetCharacterLevel(newCharacter, original.currentLevel);

        SetCharacterMaxHealth(newCharacter, original.maxHealth);
        SetCharacterHealth(newCharacter, original.health);

        newCharacter.stamina = original.stamina;
        newCharacter.initiative = original.initiative;
        newCharacter.dexterity = original.dexterity;
        newCharacter.draw = original.draw;
        newCharacter.power = original.power;

        newCharacter.deck = new List<CardData>();
        foreach (CardData cso in original.deck)
        {
            AddCardToCharacterDeck(newCharacter, CardController.Instance.CloneCardDataFromCardData(cso));
        }

        // UCM Data
        newCharacter.modelParts = new List<string>();
        newCharacter.modelParts.AddRange(original.modelParts);

        // Passive Data
        newCharacter.passiveManager = new PassiveManagerModel();
        PassiveController.Instance.BuildPassiveManagerFromOtherPassiveManager(original.passiveManager, newCharacter.passiveManager);

        // Item Data
        newCharacter.itemManager = new ItemManagerModel();
        ItemController.Instance.CopyItemManagerDataIntoOtherItemManager(original.itemManager, newCharacter.itemManager);

        // Talent Data
        newCharacter.talentPairings = new List<TalentPairingModel>();
        foreach (TalentPairingModel tpm in original.talentPairings)
        {
            newCharacter.talentPairings.Add(CloneTalentPairingModel(tpm));
        }

        return newCharacter;


    }
    public CharacterData ConvertCharacterTemplateToCharacterData(CharacterTemplateSO template)
    {
        Debug.Log("CharacterDataController.ConverCharacterTemplateToCharacterData() called...");

        CharacterData newCharacter = new CharacterData();

        newCharacter.myName = template.myName;
        newCharacter.myClassName = template.myClassName;
        newCharacter.race = template.race;
        newCharacter.audioProfile = template.audioProfile;
        
        newCharacter.currentLevel = GlobalSettings.Instance.startingLevel;
        newCharacter.currentMaxXP = GlobalSettings.Instance.startingMaxXp;
        HandleGainXP(newCharacter, GlobalSettings.Instance.startingXpBonus);

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

    // Modify Character Stats & Core Attributes
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
    public void ModifyPower(CharacterData data, int gainedOrLost)
    {
        data.power += gainedOrLost;
    }
    public void ModifyInitiative(CharacterData data, int gainedOrLost)
    {
        data.initiative += gainedOrLost;
    }
    public void ModifyDexterity(CharacterData data, int gainedOrLost)
    {
        data.dexterity += gainedOrLost;
    }
    public void ModifyStamina(CharacterData data, int gainedOrLost)
    {
        data.stamina += gainedOrLost;
    }
    public void ModifyDraw(CharacterData data, int gainedOrLost)
    {
        data.draw += gainedOrLost;
    }
    #endregion

    // Modify XP + Level Logic
    public void SetCharacterLevel(CharacterData data, int newLevelValue)
    {
        data.currentLevel = newLevelValue;
    }
    public void HandleGainXP(CharacterData data, int xpGained)
    {
        // check spill over + level up first
        int spillOver = (data.currentXP + xpGained) - data.currentMaxXP;

        // Level up occured with spill over XP
        if(spillOver > 0)
        {
            // Gain level
            SetCharacterLevel(data, data.currentLevel + 1);

            // Reset current xp
            data.currentXP = 0;

            // Increase max xp on level up
            data.currentMaxXP += GlobalSettings.Instance.maxHpIncrementPerLevel;

            // Restart the xp gain procces with the spill over amount
            HandleGainXP(data, spillOver);
        }

        // Level up with no spill over
        else if(spillOver == 0)
        { 
            // Gain level
            SetCharacterLevel(data, data.currentLevel + 1);

            // Reset current xp
            data.currentXP = 0;

            // Increase max xp on level up
            data.currentMaxXP += GlobalSettings.Instance.maxHpIncrementPerLevel;
        }

        // Gain xp without leveling up
        else
        {
            data.currentXP += xpGained;
        }
    }
    public void HandleXpRewardPostCombat(EncounterType encounter)
    {
        // Apply flat combat type xp reward
        if(encounter == EncounterType.BasicEnemy)
        {
            foreach(CharacterData character in AllPlayerCharacters)
            {
                HandleGainXP(character, GlobalSettings.Instance.basicCombatXpReward);
            }         
        }
        else if (encounter == EncounterType.EliteEnemy)
        {
            foreach (CharacterData character in AllPlayerCharacters)
            {
                HandleGainXP(character, GlobalSettings.Instance.eliteCombatXpReward);
            }
        }
        else if (encounter == EncounterType.BossEnemy)
        {
            foreach (CharacterData character in AllPlayerCharacters)
            {
                HandleGainXP(character, GlobalSettings.Instance.bossCombatXpReward);
            }
        }

        // Check and apply flawless bonus
        foreach (CharacterData character in AllPlayerCharacters)
        {
            foreach(CharacterEntityModel entity in CharacterEntityController.Instance.AllDefenders)
            {
                if(entity.characterData == character && entity.hasLostHealthThisCombat == false)
                {
                    HandleGainXP(character, GlobalSettings.Instance.noDamageTakenXpReward);
                }
            }
            
        }
    }

    // Modify Character Deck
    #region
    public void AddCardToCharacterDeck(CharacterData character, CardData card)
    {
        Debug.Log("CharacterDataController.AddCardToCharacterDeck() called, adding " +
            card.cardName + " to " + character.myName);
        character.deck.Add(card);
    }
    public void AddCardToCharacterDeck(CharacterData character, CardData card, int indexInDeck)
    {
        Debug.Log("CharacterDataController.AddCardToCharacterDeck() called, adding " +
          card.cardName + " to " + character.myName);
        character.deck.Insert(indexInDeck, card);
    }
    public void RemoveCardFromCharacterDeck(CharacterData character, CardData card)
    {
        Debug.Log("CharacterDataController.RemoveCardFromCharacterDeck() called, removing " +
            card.cardName + " from " + character.myName);
        character.deck.Remove(card);
    }
    #endregion

    public bool DoesCharacterMeetTalentRequirement(CharacterData character, TalentSchool school, int minimumTier)
    {
        bool bReturned = false;

        foreach(TalentPairingModel tpm in character.talentPairings)
        {
            if(tpm.talentSchool == school && tpm.talentLevel >= minimumTier)
            {
                bReturned = true;
                break;
            }
        }

        return bReturned;
    }

}
