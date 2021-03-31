using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataController : Singleton<CharacterDataController>
{
    // Properties
    #region
    [Header("Properties")]
    private List<CharacterData> allPlayerCharacters = new List<CharacterData>();
    private List<CharacterData> characterDeck = new List<CharacterData>();
    private List<CharacterRace> validCharacterRaces = new List<CharacterRace>
    { CharacterRace.Demon, CharacterRace.Elf, CharacterRace.Ent, CharacterRace.Gnoll, CharacterRace.Goblin, CharacterRace.Human,
      CharacterRace.Orc, CharacterRace.Satyr, CharacterRace.Undead};

    [Header("Templates Buckets")]
    [SerializeField] private CharacterTemplateSO[] allCharacterTemplatesSOs;
    [SerializeField] private CharacterData[] allCharacterTemplates;
    public List<ClassTemplateSO> allClassTemplateSOs;
    public List<ModelTemplateSO> allModelTemplateSOs;

    [Header("Character Name Buckets")]
    [SerializeField] string[] humanNames;
    [SerializeField] string[] elfNames;
    [SerializeField] string[] orcNames;
    [SerializeField] string[] satyrNames;
    [SerializeField] string[] goblinNames;
    [SerializeField] string[] entNames;
    [SerializeField] string[] demonNames;
    [SerializeField] string[] gnollNames;
    [SerializeField] string[] undeadNames;

    #endregion

    // Accessors + Getters
    #region
    public List<CharacterData> AllPlayerCharacters
    {
        get { return allPlayerCharacters; }
        private set { allPlayerCharacters = value; }

    }
    public CharacterTemplateSO[] AllCharacterTemplatesSOs
    {
        get { return allCharacterTemplatesSOs; }
        private set { allCharacterTemplatesSOs = value; }
    }
    public CharacterData[] AllCharacterTemplates
    {
        get { return allCharacterTemplates; }
        private set { allCharacterTemplates = value; }
    }
    public List<CharacterData> CharacterDeck
    {
        get { return characterDeck; }
        private set { characterDeck = value; }
    }
    #endregion

    // Initialization
    #region
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
    #endregion

    // Build, Add and Delete Characters 
    #region
    public void BuildCharacterRosterFromCharacterTemplateList(IEnumerable<CharacterTemplateSO> characters)
    {
        foreach (CharacterTemplateSO template in characters)
        {
            CloneNewCharacterToPlayerRoster(ConvertCharacterTemplateToCharacterData(template));
        }
    }
    public CharacterData CloneNewCharacterToPlayerRoster(CharacterData character)
    {
        CharacterData newChar = CloneCharacterData(character);
        AddCharacterToRoster(newChar);
        return newChar;
    }
    public void AddCharacterToRoster(CharacterData character)
    {
        AllPlayerCharacters.Add(character);
    }
    public void ClearCharacterRoster()
    {
        AllPlayerCharacters.Clear();
    }
    public void ClearCharacterDeck()
    {
        CharacterDeck.Clear();
    }

    #endregion

    // Save + Load Logic
    #region
    public void BuildMyDataFromSaveFile(SaveGameData saveFile)
    {
        // Build character roster
        AllPlayerCharacters.Clear();
        foreach (CharacterData characterData in saveFile.characters)
        {
            AddCharacterToRoster(characterData);
        }
        
        // Build character deck
        CharacterDeck.Clear();
        foreach (CharacterData cd in saveFile.characterDeck)
            CharacterDeck.Add(cd);
    }
    public void SaveMyDataToSaveFile(SaveGameData saveFile)
    {
        foreach (CharacterData character in AllPlayerCharacters)        
            saveFile.characters.Add(character);        
        
        foreach (CharacterData character in CharacterDeck)        
            saveFile.characterDeck.Add(character);
        
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

        newCharacter.strength = original.strength;
        newCharacter.intelligence = original.intelligence;
        newCharacter.wits = original.wits;
        newCharacter.dexterity = original.dexterity;
        newCharacter.constitution = 0;
        ModifyConstitution(newCharacter, original.constitution);
        newCharacter.constitution = original.constitution;

        SetCharacterMaxHealth(newCharacter, original.maxHealth);
        SetCharacterHealth(newCharacter, original.health);

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

        // Attribute rolls        
        newCharacter.attributeRollResults = new List<AttributeRollResult>();
        foreach (AttributeRollResult arr in original.attributeRollResults)
        {
            newCharacter.attributeRollResults.Add(arr.Clone(arr));
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

        newCharacter.strength = template.strength;
        newCharacter.intelligence = template.intelligence;
        newCharacter.wits = template.wits;
        newCharacter.dexterity = template.dexterity;
        // ModifyConstitution(newCharacter, template.constitution);
        newCharacter.constitution = template.constitution;

        SetCharacterMaxHealth(newCharacter, template.maxHealth);
        SetCharacterHealth(newCharacter, newCharacter.MaxHealthTotal);

        newCharacter.stamina = template.stamina;
        newCharacter.initiative = template.initiative;
        newCharacter.baseCrit = template.baseCrit;
        newCharacter.critModifier = template.critModifier;
        newCharacter.draw = template.draw;
        newCharacter.power = template.power;

        newCharacter.attributePoints = template.startingAttributePoints;
        newCharacter.talentPoints = template.startingTalentPoints;

        newCharacter.deck = new List<CardData>();
        foreach (CardDataSO cso in template.deck)
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
        foreach (TalentPairingModel tpm in template.talentPairings)
        {
            newCharacter.talentPairings.Add(CloneTalentPairingModel(tpm));
        }


        return newCharacter;
    }
    public TalentPairingModel CloneTalentPairingModel(TalentPairingModel original)
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

        if (data.health > data.MaxHealthTotal)
            data.health = data.MaxHealthTotal;
    }
    public void SetCharacterMaxHealth(CharacterData data, int newValue)
    {
        Debug.Log("CharacterDataController.SetCharacterMaxHealth() called for '" +
            data.myName + "', new max health value = " + newValue.ToString());

        data.maxHealth = newValue;

        if (data.health > data.MaxHealthTotal)
            data.health = data.MaxHealthTotal;
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
    public void ModifyConstitution(CharacterData data, int gainedOrLost, bool updateCurrentHealth = true)
    {

        int previousMaxHealth = data.MaxHealthTotal;
        data.constitution += gainedOrLost;

        if (updateCurrentHealth)
        {
            int difference = data.MaxHealthTotal - previousMaxHealth;
            SetCharacterHealth(data, data.health + difference);

            //Debug.LogWarning("Difference = " + difference.ToString());
        }
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
        if (spillOver > 0)
        {
            // Glow top bar button
            TopBarController.Instance.ShowCharacterRosterButtonGlow();

            // Gain level
            SetCharacterLevel(data, data.currentLevel + 1);

            // Gain Talent point
            ModifyCharacterTalentPoints(data, 1);

            // Do attribute level up roll result logic
            GenerateAndCacheAttributeRollOnLevelUp(data);

            // Reset current xp
            data.currentXP = 0;

            // Increase max xp on level up
            data.currentMaxXP += GlobalSettings.Instance.maxXpIncrementPerLevel;

            // Restart the xp gain procces with the spill over amount
            HandleGainXP(data, spillOver);


        }

        // Level up with no spill over
        else if (spillOver == 0)
        {
            // Glow top bar button
            TopBarController.Instance.ShowCharacterRosterButtonGlow();

            // Gain level
            SetCharacterLevel(data, data.currentLevel + 1);

            // Gain Talent point
            ModifyCharacterTalentPoints(data, 1);

            // Do attribute level up roll result logic
            GenerateAndCacheAttributeRollOnLevelUp(data);

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
    public void HandleXpRewardPostCombat(EncounterType encounter, List<CharacterData> characters)
    {
        // Apply flat combat type xp reward
        if (encounter == EncounterType.BasicEnemy)
        {
            foreach (CharacterData character in characters)
            {
                HandleGainXP(character, GlobalSettings.Instance.basicCombatXpReward);
            }
        }
        else if (encounter == EncounterType.EliteEnemy)
        {
            foreach (CharacterData character in characters)
            {
                HandleGainXP(character, GlobalSettings.Instance.eliteCombatXpReward);
            }
        }
        else if (encounter == EncounterType.BossEnemy)
        {
            foreach (CharacterData character in characters)
            {
                HandleGainXP(character, GlobalSettings.Instance.bossCombatXpReward);
            }
        }

        // Check and apply flawless bonus
        foreach (CharacterData character in characters)
        {
            foreach (CharacterEntityModel entity in CharacterEntityController.Instance.AllDefenders)
            {
                if (entity.characterData == character && entity.hasLostHealthThisCombat == false)
                {
                    HandleGainXP(character, GlobalSettings.Instance.noDamageTakenXpReward);
                }
            }

        }
    }
    public void ModifyCharacterTalentPoints(CharacterData data, int gainedOrLost)
    {
        Debug.Log("CharacterDataController.ModifyCharacterTalentPoints() called, modifying talents by " + gainedOrLost.ToString() +
            " for character " + data.myName);
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
        foreach (TalentPairingModel tp in character.talentPairings)
        {
            if (tp.talentSchool == talent)
            {
                tpm = tp;
                break;
            }
        }

        // Did we find a pre-existing talent?
        if (tpm != null)
        {
            // We did, increase talent level
            tpm.talentLevel += pointsGained;

            // Prevent talent exceeding tier 2
            if (tpm.talentLevel > 2)
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
    private void GenerateAndCacheAttributeRollOnLevelUp(CharacterData character)
    {
        Debug.Log("CharacterDataController.GenerateAndCacheAttributeRollOnLevelUp() called for character: " + character.myName);
        AttributeRollResult arr = new AttributeRollResult();
        arr.GenerateMyRolls();
        character.attributeRollResults.Add(arr);
        Debug.Log("new character attribute rolls count = " + character.attributeRollResults.Count.ToString());
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
                Debug.Log("AutoAddCharactersRacialCard() found matching racial card, adding " + card.cardName +
                    " to " + character.myName + "'s deck");
                CardData newRacialCard = CardController.Instance.CloneCardDataFromCardData(card);
                AddCardToCharacterDeck(character, newRacialCard, 0);
                break;
            }
        }
    }
    #endregion


    // Misc Logic + Calculators
    #region
    public int GetTalentLevel(CharacterData character, TalentSchool school)
    {
        int level = 0;

        foreach (TalentPairingModel tp in character.talentPairings)
        {
            if (tp.talentSchool == school)
            {
                level = tp.talentLevel;
                break;
            }
        }

        return level;
    }
    #endregion

    // Character Generation Logic
    #region
    private CharacterData GenerateCharacter(ClassTemplateSO ct, CharacterRace race)
    {
        Debug.Log("CharacterDataController.GenerateCharacter() called...");

        CharacterData newCharacter = new CharacterData();

        newCharacter.myName = GetRandomCharacterName(race);
        newCharacter.myClassName = ct.templateName;
        newCharacter.race = race;
        newCharacter.audioProfile = GetAudioProfileForRace(race);

        newCharacter.currentLevel = GlobalSettings.Instance.startingLevel;
        newCharacter.currentMaxXP = GlobalSettings.Instance.startingMaxXp;
        ModifyCharacterTalentPoints(newCharacter, GlobalSettings.Instance.startingTalentPoints);
        ModifyCharacterAttributePoints(newCharacter, GlobalSettings.Instance.startingAttributePoints);
        HandleGainXP(newCharacter, GlobalSettings.Instance.startingXpBonus);

        //Set base attribute stat levels
        newCharacter.strength = 20;
        newCharacter.intelligence = 20;
        newCharacter.wits = 20;
        newCharacter.dexterity = 20;
        newCharacter.constitution = 20;

        // Randomize base stats
        if (RandomGenerator.NumberBetween(1,100) > 50)
            newCharacter.strength = RandomGenerator.NumberBetween(18, 22);
        if (RandomGenerator.NumberBetween(1, 100) > 50)
            newCharacter.intelligence = RandomGenerator.NumberBetween(18, 22);
        if (RandomGenerator.NumberBetween(1, 100) > 50)
            newCharacter.wits = RandomGenerator.NumberBetween(18, 22);
        if (RandomGenerator.NumberBetween(1, 100) > 50)
            newCharacter.dexterity = RandomGenerator.NumberBetween(18, 22);
        if (RandomGenerator.NumberBetween(1, 100) > 50)
            newCharacter.constitution = RandomGenerator.NumberBetween(18, 22);

        // Apply stat modifier from template
        newCharacter.strength += ct.strengthMod;
        newCharacter.intelligence += ct.intelligenceMod;
        newCharacter.wits += ct.witsMod;
        newCharacter.dexterity += ct.dexterityMod;
        newCharacter.constitution += ct.constitutionMod;

        // Randomize health
        SetCharacterMaxHealth(newCharacter, RandomGenerator.NumberBetween(85, 95));
        SetCharacterHealth(newCharacter, newCharacter.MaxHealthTotal);

        newCharacter.stamina = 2;
        newCharacter.initiative = 10;
        newCharacter.baseCrit = 0;
        newCharacter.critModifier = 30;
        newCharacter.baseFirstActivationDrawBonus = 0;
        newCharacter.draw = 4;
        newCharacter.power = 0;

        // Randomize deck
        DeckTemplateSO randomDeckData = ct.possibleDecks[RandomGenerator.NumberBetween(0, ct.possibleDecks.Count - 1)];
        newCharacter.deck = new List<CardData>();
        foreach (CardDataSO cso in randomDeckData.cards)
        {
            AddCardToCharacterDeck(newCharacter, CardController.Instance.BuildCardDataFromScriptableObjectData(cso));
        }

        // Add racial cards


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
            newCharacter.talentPairings.Add(CloneTalentPairingModel(tpm));

        return newCharacter;
    }
    public string GetRandomCharacterName(CharacterRace race)
    {
        string nameReturned = "";
        if (race == CharacterRace.Demon)
            nameReturned = demonNames[RandomGenerator.NumberBetween(0, demonNames.Length - 1)];

        if (race == CharacterRace.Elf)
            nameReturned = elfNames[RandomGenerator.NumberBetween(0, elfNames.Length - 1)];

        if (race == CharacterRace.Ent)
            nameReturned = entNames[RandomGenerator.NumberBetween(0, entNames.Length - 1)];

        if (race == CharacterRace.Gnoll)
            nameReturned = gnollNames[RandomGenerator.NumberBetween(0, gnollNames.Length - 1)];

        if (race == CharacterRace.Goblin)
            nameReturned = goblinNames[RandomGenerator.NumberBetween(0, goblinNames.Length - 1)];

        if (race == CharacterRace.Human)
            nameReturned = humanNames[RandomGenerator.NumberBetween(0, humanNames.Length - 1)];

        if (race == CharacterRace.Orc)
            nameReturned = orcNames[RandomGenerator.NumberBetween(0, orcNames.Length - 1)];

        if (race == CharacterRace.Satyr)
            nameReturned = satyrNames[RandomGenerator.NumberBetween(0, satyrNames.Length - 1)];

        if (race == CharacterRace.Undead)
            nameReturned = undeadNames[RandomGenerator.NumberBetween(0, undeadNames.Length - 1)];


        return nameReturned;
    }
    private ModelTemplateSO GetRandomModelTemplate(CharacterRace race)
    {
        List<ModelTemplateSO> validTemplates = new List<ModelTemplateSO>();

        foreach (ModelTemplateSO mt in allModelTemplateSOs)
        {
            if (mt.race == race)
                validTemplates.Add(mt);
        }

        return validTemplates[RandomGenerator.NumberBetween(0, validTemplates.Count - 1)];

    }
    private AudioProfileType GetAudioProfileForRace(CharacterRace race)
    {
        if (race == CharacterRace.Human)
            return AudioProfileType.HumanMale;

        else if (race == CharacterRace.Elf)
            return AudioProfileType.HumanFemale;

        else if (race == CharacterRace.Undead ||
            race == CharacterRace.Demon ||
            race == CharacterRace.Ent)
            return AudioProfileType.Undead;

        else if (race == CharacterRace.Satyr)
            return AudioProfileType.Satyr;

        else if (race == CharacterRace.Orc)
            return AudioProfileType.Orc;

        else if (race == CharacterRace.Goblin)
            return AudioProfileType.Goblin;

        else if (race == CharacterRace.Gnoll)
            return AudioProfileType.Gnoll;

        else
            return AudioProfileType.None;
    }
    private CharacterRace GetRandomRace()
    {
        return validCharacterRaces[RandomGenerator.NumberBetween(0, validCharacterRaces.Count - 1)];
    }
    private CharacterRace GetRandomRace(List<CharacterRace> validRaces)
    {
        return validRaces[RandomGenerator.NumberBetween(0, validRaces.Count - 1)];
    }
    private List<CharacterData> GenerateCharacterDeck()
    {
        Debug.Log("CharacterDataController.GenerateCharacterDeck() called...");
        List<CharacterData> newCharacterDeck = new List<CharacterData>();

        foreach (ClassTemplateSO ct in allClassTemplateSOs)
        {
            newCharacterDeck.Add(GenerateCharacter(ct, GetRandomRace(ct.possibleRaces)));
        }

        return newCharacterDeck;
    }
    public void AutoGenerateAndCacheNewCharacterDeck()
    {
        Debug.Log("AutoGenerateAndCacheNewCharacterDeck() called, generating new character deck");
        CharacterDeck.Clear();
        CharacterDeck = GenerateCharacterDeck();
        CharacterDeck.Shuffle();
    }
    #endregion


}