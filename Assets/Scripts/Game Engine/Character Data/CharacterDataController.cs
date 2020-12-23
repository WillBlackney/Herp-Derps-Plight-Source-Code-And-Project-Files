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
            CloneNewCharacterToPlayerRoster(ConvertCharacterTemplateToCharacterData(template));
        }
    }
    public CharacterData CloneNewCharacterToPlayerRoster(CharacterData character)
    {
        CharacterData newChar = CloneCharacterData(character);
        AllPlayerCharacters.Add(newChar);
        return newChar;
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
        ModifyCharacterTalentPoints(newCharacter, original.talentPoints);
        ModifyCharacterAttributePoints(newCharacter, original.attributePoints);

        SetCharacterMaxHealth(newCharacter, original.maxHealth);
        SetCharacterHealth(newCharacter, original.health);

        newCharacter.strength = original.strength;
        newCharacter.intelligence = original.intelligence;
        newCharacter.wits = original.wits;
        newCharacter.dexterity = original.dexterity;

        newCharacter.stamina = original.stamina;
        newCharacter.initiative = original.initiative;     
        newCharacter.draw = original.draw;
        newCharacter.power = original.power;
        newCharacter.baseCrit = original.baseCrit;
        newCharacter.critModifier = original.critModifier;

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
        ModifyCharacterTalentPoints(newCharacter, GlobalSettings.Instance.startingTalentPoints);
        ModifyCharacterAttributePoints(newCharacter, GlobalSettings.Instance.startingAttributePoints);
        HandleGainXP(newCharacter, GlobalSettings.Instance.startingXpBonus);

        SetCharacterMaxHealth(newCharacter, template.maxHealth);
        SetCharacterHealth(newCharacter, template.health);

        newCharacter.strength = template.strength;
        newCharacter.intelligence = template.intelligence;
        newCharacter.wits = template.wits;
        newCharacter.dexterity = template.dexterity;       

        newCharacter.stamina = template.stamina;
        newCharacter.initiative = template.initiative;
        newCharacter.baseCrit = template.baseCrit;
        newCharacter.critModifier = template.critModifier;
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
    public void ModifyStrength(CharacterData data, int gainedOrLost)
    {
        data.strength += gainedOrLost;
    }
    public void ModifyIntelligence(CharacterData data, int gainedOrLost)
    {
        data.intelligence += gainedOrLost;
    }
    public void ModifyWits(CharacterData data, int gainedOrLost)
    {
        data.wits += gainedOrLost;
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
    #region
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

            // Gain Talent point
            ModifyCharacterTalentPoints(data, 1);

            // Gain attribute points
            ModifyCharacterAttributePoints(data, GlobalSettings.Instance.attributePointsGainedOnLevelUp);

            // Reset current xp
            data.currentXP = 0;

            // Increase max xp on level up
            data.currentMaxXP += GlobalSettings.Instance.maxXpIncrementPerLevel;

            // Restart the xp gain procces with the spill over amount
            HandleGainXP(data, spillOver);
        }

        // Level up with no spill over
        else if(spillOver == 0)
        { 
            // Gain level
            SetCharacterLevel(data, data.currentLevel + 1);

            // Gain Talent point
            ModifyCharacterTalentPoints(data, 1);

            // Gain attribute points
            ModifyCharacterAttributePoints(data, GlobalSettings.Instance.attributePointsGainedOnLevelUp);

            // Reset current xp
            data.currentXP = 0;

            // Increase max xp on level up
            data.currentMaxXP += GlobalSettings.Instance.maxXpIncrementPerLevel;
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
    public void ModifyCharacterTalentPoints(CharacterData data, int gainedOrLost)
    {
        data.talentPoints += gainedOrLost;
    }
    public void ModifyCharacterAttributePoints(CharacterData data, int gainedOrLost)
    {
        data.attributePoints += gainedOrLost;
    }
    public TalentPairingModel HandlePlayerGainTalent(CharacterData character, TalentSchool talent, int pointsGained)
    {
        TalentPairingModel tpm = null;

        // Check if character already has unlocked first talent level
        foreach(TalentPairingModel tp in character.talentPairings)
        {
            if(tp.talentSchool == talent)
            {
                tpm = tp;
                break;
            }
        }

        // Did we find a pre-existing talent?
        if(tpm != null)
        {
            // We did, increase talent level
            tpm.talentLevel += pointsGained;

            // Prevent talent exceeding tier 2
            if(tpm.talentLevel > 2)
            {
                tpm.talentLevel = 2;
            }

            // Prevent going negative
            else if (tpm.talentLevel < 0)
            {
                tpm.talentLevel = 0;
            }
        }

        // Otherwise, create a new talent pairing
        else
        {
            tpm = new TalentPairingModel();
            tpm.talentLevel = pointsGained;
            tpm.talentSchool = talent;
            character.talentPairings.Add(tpm);
        }

        return tpm;
    }
    public bool DoesCharacterMeetTalentRequirement(CharacterData character, TalentSchool school, int minimumTier)
    {
        bool bReturned = false;

        foreach (TalentPairingModel tpm in character.talentPairings)
        {
            if (tpm.talentSchool == school && tpm.talentLevel >= minimumTier)
            {
                bReturned = true;
                break;
            }
        }

        return bReturned;
    }
    #endregion

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
    public void AutoAddCharactersRacialCard(CharacterData character)
    {
        // Get racial cards
        CardData[] racialCardData = CardController.Instance.QueryByRacial(CardController.Instance.AllCards).ToArray();

        // Add racial card to deck
        foreach (CardData card in racialCardData)
        {
            if (card.originRace == character.race && card.upgradeLevel == 0)
            {
                Debug.Log("BuildNewSaveFileOnNewGameStarted() found matching racial card, adding " + card.cardName +
                    " to " + character.myName + "'s deck");
                CardData newRacialCard = CardController.Instance.CloneCardDataFromCardData(card);
                AddCardToCharacterDeck(character, newRacialCard, 0);
                break;
            }
        }
    }
    #endregion

    

}
