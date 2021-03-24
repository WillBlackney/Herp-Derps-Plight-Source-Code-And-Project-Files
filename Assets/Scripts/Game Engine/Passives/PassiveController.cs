using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveController : Singleton<PassiveController>
{
    // Properties + Component References
    #region
    [Header("Passive Library Properties")]
    [SerializeField] private PassiveIconDataSO[] allIconScriptableObjects;
    private PassiveIconData[] allIcons;

    // Getters
    public PassiveIconData[] AllIcons
    {
        get { return allIcons; }
        private set { allIcons = value; }
    }
    public PassiveIconDataSO[] AllIconScriptableObjects
    {
        get { return allIconScriptableObjects; }
    }
    #endregion

    // Library Logic
    #region
    private void Start()
    {
        BuildIconLibrary();
    }
    public void BuildIconLibrary()
    {
        List<PassiveIconData> tempList = new List<PassiveIconData>();

        foreach (PassiveIconDataSO dataSO in allIconScriptableObjects)
        {
            tempList.Add(BuildIconDataFromScriptableObjectData(dataSO));
        }

        AllIcons = tempList.ToArray();
    }
    public PassiveIconData BuildIconDataFromScriptableObjectData(PassiveIconDataSO data)
    {
        PassiveIconData p = new PassiveIconData();       
        p.passiveName = data.passiveName;
        p.passiveDescription = data.passiveDescription;
        p.passiveSprite = GetPassiveSpriteByName(data.passiveName);
        p.showStackCount = data.showStackCount;
        p.hiddenOnPassivePanel = data.hiddenOnPassivePanel;
        p.runeBlocksDecrease = data.runeBlocksDecrease;
        p.runeBlocksIncrease = data.runeBlocksIncrease;

        return p;
    }
    public PassiveIconData GetPassiveIconDataByName(string name)
    {
        PassiveIconData iconReturned = null;

        foreach (PassiveIconData icon in AllIcons)
        {
            if (icon.passiveName == name)
            {
                iconReturned = icon;
                break;
            }
        }

        if (iconReturned == null)
        {
            Debug.Log("PassiveController.GetPassiveIconDataByName() could not find a passive icon SO with the name " +
                name + ", returning null...");
        }

        return iconReturned;
    }
    public Sprite GetPassiveSpriteByName(string passiveName)
    {
        Sprite sprite = null;

        foreach (PassiveIconDataSO data in AllIconScriptableObjects)
        {
            if (data.passiveName == passiveName)
            {
                sprite = data.passiveSprite;
                break;
            }
        }

        return sprite;
    }
    #endregion

    // Setup Logic
    #region
    public void BuildPassiveManagerFromOtherPassiveManager(PassiveManagerModel originalData, PassiveManagerModel newClone)
    {
        Debug.Log("PassiveController.BuildPassiveManagerFromOtherPassiveManager() called...");

        // Core stat bonuses
        #region
        if (originalData.bonusPowerStacks != 0)
        {
            ModifyBonusPower(newClone, originalData.bonusPowerStacks, false);
        }
        if (originalData.bonusDexterityStacks != 0)
        {
            ModifyBonusDexterity(newClone, originalData.bonusDexterityStacks, false);
        }
        if (originalData.bonusDrawStacks != 0)
        {
            ModifyBonusDraw(newClone, originalData.bonusDrawStacks, false);
        }
        if (originalData.bonusStaminaStacks != 0)
        {
            ModifyBonusStamina(newClone, originalData.bonusStaminaStacks, false);
        }
        if (originalData.bonusInitiativeStacks != 0)
        {
            ModifyBonusInitiative(newClone, originalData.bonusInitiativeStacks, false);
        }
        #endregion

        // Temp Core stat bonuses
        #region
        if (originalData.temporaryBonusPowerStacks != 0)
        {
            ModifyTemporaryPower(newClone, originalData.temporaryBonusPowerStacks, false);
        }
        if (originalData.temporaryBonusDexterityStacks != 0)
        {
            ModifyTemporaryDexterity(newClone, originalData.temporaryBonusDexterityStacks, false);
        }
        if (originalData.temporaryBonusDrawStacks != 0)
        {
            ModifyTemporaryDraw(newClone, originalData.temporaryBonusDrawStacks, false);
        }
        if (originalData.temporaryBonusStaminaStacks != 0)
        {
            ModifyTemporaryStamina(newClone, originalData.temporaryBonusStaminaStacks, false);
        }
        if (originalData.temporaryBonusInitiativeStacks != 0)
        {
            ModifyTemporaryInitiative(newClone, originalData.temporaryBonusInitiativeStacks, false);
        }
        #endregion

        // Buff passives
        #region
        if (originalData.enrageStacks != 0)
        {
            ModifyEnrage(newClone, originalData.enrageStacks, false);
        }
        if (originalData.pierceStacks != 0)
        {
            ModifyPierce(newClone, originalData.pierceStacks, false);
        }
        if (originalData.unbreakableStacks != 0)
        {
            ModifyUnbreakable(newClone, originalData.unbreakableStacks, false);
        }
        if (originalData.enrageStacks != 0)
        {
            ModifyMalice(newClone, originalData.maliceStacks, false);
        }
        if (originalData.tranquilHateStacks != 0)
        {
            ModifyTranquilHate(newClone, originalData.tranquilHateStacks, false);
        }
        if (originalData.thornsStacks != 0)
        {
            ModifyThorns(newClone, originalData.thornsStacks, false);
        }
        if (originalData.shieldWallStacks != 0)
        {
            ModifyShieldWall(newClone, originalData.shieldWallStacks, false);
        }
        if (originalData.fanOfKnivesStacks != 0)
        {
            ModifyFanOfKnives(newClone, originalData.fanOfKnivesStacks, false);
        }
        if (originalData.divineFavourStacks != 0)
        {
            ModifyDivineFavour(newClone, originalData.divineFavourStacks, false);
        }
        if (originalData.phoenixFormStacks != 0)
        {
            ModifyPhoenixForm(newClone, originalData.phoenixFormStacks, false);
        }
        if (originalData.poisonousStacks != 0)
        {
            ModifyPoisonous(newClone, originalData.poisonousStacks, false);
        }
        if (originalData.inflamedStacks != 0)
        {
            ModifyInflamed(newClone, originalData.inflamedStacks, false);
        }
        if (originalData.venomousStacks != 0)
        {
            ModifyVenomous(newClone, originalData.venomousStacks, false);
        }
        if (originalData.overloadStacks != 0)
        {
            ModifyOverload(newClone, originalData.overloadStacks, false);
        }
        if (originalData.sourceStacks != 0)
        {
            ModifySource(newClone, originalData.sourceStacks, false);
        }
        if (originalData.fusionStacks != 0)
        {
            ModifyFusion(newClone, originalData.fusionStacks, false);
        }
        if (originalData.plantedFeetStacks != 0)
        {
            ModifyPlantedFeet(newClone, originalData.plantedFeetStacks, false);
        }
        if (originalData.takenAimStacks != 0)
        {
            ModifyTakenAim(newClone, originalData.takenAimStacks, false);
        }
        if (originalData.consecrationStacks != 0)
        {
            ModifyConsecration(newClone, originalData.consecrationStacks, false);
        }
        if (originalData.growingStacks != 0)
        {
            ModifyGrowing(newClone, originalData.growingStacks, false);
        }
        if (originalData.cautiousStacks != 0)
        {
            ModifyCautious(newClone, originalData.cautiousStacks, false);
        }
        if (originalData.infuriatedStacks != 0)
        {
            ModifyInfuriated(newClone, originalData.infuriatedStacks, false);
        }
        if (originalData.battleTranceStacks != 0)
        {
            ModifyBattleTrance(newClone, originalData.battleTranceStacks, false);
        }
        if (originalData.balancedStanceStacks != 0)
        {
            ModifyBalancedStance(newClone, originalData.balancedStanceStacks, false);
        }
        if (originalData.flurryStacks != 0)
        {
            ModifyFlurry(newClone, originalData.flurryStacks, false);
        }
        if (originalData.demonFormStacks != 0)
        {
            ModifyDemonForm(newClone, originalData.demonFormStacks, false);
        }
        if (originalData.lordOfStormsStacks != 0)
        {
            ModifyLordOfStorms(newClone, originalData.lordOfStormsStacks, false);
        }
        if (originalData.sentinelStacks != 0)
        {
            ModifySentinel(newClone, originalData.sentinelStacks, false);
        }
        if (originalData.ruthlessStacks != 0)
        {
            ModifyRuthless(newClone, originalData.ruthlessStacks, false);
        }
        if (originalData.evangelizeStacks != 0)
        {
            ModifyEvangelize(newClone, originalData.evangelizeStacks, false);
        }
        if (originalData.wellOfSoulsStacks != 0)
        {
            ModifyWellOfSouls(newClone, originalData.wellOfSoulsStacks, false);
        }
        if (originalData.corpseCollectorStacks != 0)
        {
            ModifyCorpseCollector(newClone, originalData.corpseCollectorStacks, false);
        }
        if (originalData.longDrawStacks != 0)
        {
            ModifyLongDraw(newClone, originalData.longDrawStacks, false);
        }
        if (originalData.sharpenBladeStacks != 0)
        {
            ModifySharpenBlade(newClone, originalData.sharpenBladeStacks, false);
        }
        if (originalData.pistoleroStacks != 0)
        {
            ModifyPistolero(newClone, originalData.pistoleroStacks, false);
        }
        if (originalData.fastLearnerStacks != 0)
        {
            ModifyFastLearner(newClone, originalData.fastLearnerStacks, false);
        }
        if (originalData.darkBargainStacks != 0)
        {
            ModifyDarkBargain(newClone, originalData.darkBargainStacks, false);
        }
        if (originalData.volatileStacks != 0)
        {
            ModifyVolatile(newClone, originalData.volatileStacks, false);
        }
        if (originalData.soulCollectorStacks != 0)
        {
            ModifySoulCollector(newClone, originalData.soulCollectorStacks, false);
        }
        if (originalData.magicMagnetStacks != 0)
        {
            ModifyMagicMagnet(newClone, originalData.magicMagnetStacks, false);
        }
        if (originalData.etherealStacks != 0)
        {
            ModifyEthereal(newClone, originalData.magicMagnetStacks, false);
        }
        if (originalData.shockingTouchStacks != 0)
        {
            ModifyShockingTouch(newClone, originalData.shockingTouchStacks, false);
        }
        if (originalData.stormShieldStacks != 0)
        {
            ModifyStormShield(newClone, originalData.stormShieldStacks, false);
        }
        if (originalData.regenerationStacks != 0)
        {
            ModifyRegeneration(newClone, originalData.regenerationStacks, false);
        }
        if (originalData.hurricaneStacks != 0)
        {
            ModifyHurricane(newClone, originalData.hurricaneStacks, false);
        }
        if (originalData.holierThanThouStacks != 0)
        {
            ModifyHolierThanThou(newClone, originalData.holierThanThouStacks, false);
        }
        #endregion

        // Special Defensive Passives
        #region
        if (originalData.runeStacks != 0)
        {
            ModifyRune(newClone, originalData.runeStacks, false);
        }
        if (originalData.barrierStacks != 0)
        {
            ModifyBarrier(newClone, originalData.barrierStacks, false);
        }
        if (originalData.incorporealStacks != 0)
        {
            ModifyIncorporeal(newClone, originalData.incorporealStacks, false);
        }
        #endregion

        // Aura Passives
        #region
        if (originalData.encouragingAuraStacks != 0)
        {
            ModifyEncouragingAura(newClone, originalData.encouragingAuraStacks, false);
        }
        if (originalData.shadowAuraStacks != 0)
        {
            ModifyShadowAura(newClone, originalData.shadowAuraStacks, false);
        }
        if (originalData.toxicAuraStacks != 0)
        {
            ModifyToxicAura(newClone, originalData.toxicAuraStacks, false);
        }
        if (originalData.guardianAuraStacks != 0)
        {
            ModifyGuardianAura(newClone, originalData.guardianAuraStacks, false);
        }
        if (originalData.hatefulAuraStacks != 0)
        {
            ModifyHatefulAura(newClone, originalData.hatefulAuraStacks, false);
        }
        if (originalData.intimidatingAuraStacks != 0)
        {
            ModifyIntimidatingAura(newClone, originalData.intimidatingAuraStacks, false);
        }
        #endregion

        // Dot Passives
        #region
        if (originalData.poisonedStacks != 0)
        {
            ModifyPoisoned(null, newClone, originalData.poisonedStacks, false);
        }
        if (originalData.burningStacks != 0)
        {
            ModifyBurning(newClone, originalData.burningStacks, false);
        }
        if (originalData.bleedingStacks != 0)
        {
            ModifyBleeding(newClone, originalData.bleedingStacks, false);
        }
        #endregion

        // Disabling Debuff Passives
        #region
        if (originalData.disarmedStacks != 0)
        {
            ModifyDisarmed(newClone, originalData.disarmedStacks, false);
        }
        if (originalData.sleepStacks != 0)
        {
            ModifySleep(newClone, originalData.sleepStacks, false);
        }
        if (originalData.silencedStacks != 0)
        {
            ModifySilenced(newClone, originalData.silencedStacks, false);
        }

        #endregion

        // Core % Modifier passives
        #region
        if (originalData.weakenedStacks != 0)
        {
            ModifyWeakened(newClone, originalData.weakenedStacks, null, false);
        }
        if (originalData.wrathStacks != 0)
        {
            ModifyWrath(newClone, originalData.wrathStacks, false);
        }
        if (originalData.gritStacks != 0)
        {
            ModifyGrit(newClone, originalData.gritStacks, false);
        }
        if (originalData.vulnerableStacks != 0)
        {
            ModifyVulnerable(newClone, originalData.vulnerableStacks, false);
        }
        #endregion

        // Misc Passives
        #region
        if (originalData.fireBallBonusDamageStacks != 0)
        {
            ModifyFireBallBonusDamage(newClone, originalData.fireBallBonusDamageStacks, false);
        }
        if (originalData.reflexShotBonusDamageStacks != 0)
        {
            ModifyReflexShotBonusDamage(newClone, originalData.reflexShotBonusDamageStacks, false);
        }
        #endregion

    }
    public void BuildPassiveManagerFromSerializedPassiveManager(PassiveManagerModel pManager, SerializedPassiveManagerModel original)
    {
        pManager.bonusPowerStacks = original.bonusPowerStacks;
        pManager.bonusDexterityStacks = original.bonusDexterityStacks;
        pManager.bonusStaminaStacks = original.bonusStaminaStacks;
        pManager.bonusDrawStacks = original.bonusDrawStacks;
        pManager.bonusInitiativeStacks = original.bonusInitiativeStacks;

        pManager.temporaryBonusPowerStacks = original.temporaryBonusPowerStacks;
        pManager.temporaryBonusDexterityStacks = original.temporaryBonusDexterityStacks;
        pManager.temporaryBonusStaminaStacks = original.temporaryBonusStaminaStacks;
        pManager.temporaryBonusDrawStacks = original.temporaryBonusDrawStacks;
        pManager.temporaryBonusInitiativeStacks = original.temporaryBonusInitiativeStacks;

        pManager.enrageStacks = original.enrageStacks;
        pManager.shieldWallStacks = original.shieldWallStacks;
        pManager.fanOfKnivesStacks = original.fanOfKnivesStacks;
        pManager.divineFavourStacks = original.divineFavourStacks;
        pManager.phoenixFormStacks = original.phoenixFormStacks;
        pManager.poisonousStacks = original.poisonousStacks;
        pManager.venomousStacks = original.venomousStacks;
        pManager.overloadStacks = original.overloadStacks;
        pManager.fusionStacks = original.fusionStacks;
        pManager.plantedFeetStacks = original.plantedFeetStacks;
        pManager.takenAimStacks = original.takenAimStacks;
        pManager.longDrawStacks = original.longDrawStacks;
        pManager.sharpenBladeStacks = original.sharpenBladeStacks;
        pManager.consecrationStacks = original.consecrationStacks;
        pManager.growingStacks = original.growingStacks;
        pManager.cautiousStacks = original.cautiousStacks;
        pManager.infuriatedStacks = original.infuriatedStacks;
        pManager.battleTranceStacks = original.battleTranceStacks;
        pManager.balancedStanceStacks = original.balancedStanceStacks;
        pManager.flurryStacks = original.flurryStacks;      
        pManager.lordOfStormsStacks = original.lordOfStormsStacks;        
        pManager.sentinelStacks = original.sentinelStacks;
        pManager.ruthlessStacks = original.ruthlessStacks;
        pManager.evangelizeStacks = original.evangelizeStacks;
        pManager.wellOfSoulsStacks = original.wellOfSoulsStacks;
        pManager.corpseCollectorStacks = original.corpseCollectorStacks;        
        pManager.pistoleroStacks = original.pistoleroStacks;
        pManager.fastLearnerStacks = original.fastLearnerStacks;
        pManager.demonFormStacks = original.demonFormStacks;
        pManager.darkBargainStacks = original.darkBargainStacks;
        pManager.volatileStacks = original.volatileStacks;
        pManager.soulCollectorStacks = original.soulCollectorStacks;
        pManager.magicMagnetStacks = original.magicMagnetStacks;
        pManager.etherealStacks = original.etherealStacks;
        pManager.thornsStacks = original.thornsStacks;
        pManager.tranquilHateStacks = original.tranquilHateStacks;
        pManager.inflamedStacks = original.inflamedStacks;
        pManager.stormShieldStacks = original.stormShieldStacks;
        pManager.shockingTouchStacks = original.shockingTouchStacks;
        pManager.maliceStacks = original.maliceStacks;
        pManager.unbreakableStacks = original.unbreakableStacks;
        pManager.pierceStacks = original.pierceStacks;
        pManager.regenerationStacks = original.regenerationStacks;
        pManager.hurricaneStacks = original.hurricaneStacks;
        pManager.holierThanThouStacks = original.holierThanThouStacks;

        pManager.runeStacks = original.runeStacks;
        pManager.barrierStacks = original.barrierStacks;
        pManager.incorporealStacks = original.incorporealStacks;

        pManager.encouragingAuraStacks = original.encouragingAuraStacks;
        pManager.shadowAuraStacks = original.shadowAuraStacks;
        pManager.toxicAuraStacks = original.toxicAuraStacks;
        pManager.guardianAuraStacks = original.guardianAuraStacks;
        pManager.hatefulAuraStacks = original.hatefulAuraStacks;
        pManager.intimidatingAuraStacks = original.intimidatingAuraStacks;

        pManager.wrathStacks = original.wrathStacks;
        pManager.gritStacks = original.gritStacks;
        pManager.weakenedStacks = original.weakenedStacks;
        pManager.vulnerableStacks = original.vulnerableStacks;

        pManager.disarmedStacks = original.disarmedStacks;
        pManager.sleepStacks = original.sleepStacks;
        pManager.silencedStacks = original.silencedStacks;

        pManager.poisonedStacks = original.poisonedStacks;
        pManager.burningStacks = original.burningStacks;
        pManager.bleedingStacks = original.bleedingStacks;

        pManager.fireBallBonusDamageStacks = original.fireBallBonusDamageStacks;
        pManager.reflexShotBonusDamageStacks = original.reflexShotBonusDamageStacks;

    }
    public void BuildPlayerCharacterEntityPassivesFromCharacterData(CharacterEntityModel character, CharacterData data)
    {
        Debug.Log("PassiveController.BuildPlayerCharacterEntityPassivesFromCharacterData() called...");
        character.pManager = new PassiveManagerModel();
        character.pManager.myCharacter = character;

        BuildPassiveManagerFromOtherPassiveManager(data.passiveManager, character.pManager);
    }
    public void BuildEnemyCharacterEntityPassivesFromEnemyData(CharacterEntityModel character, EnemyDataSO data)
    {
        Debug.Log("PassiveController.BuildEnemyCharacterEntityPassivesFromEnemyData() called...");

        // Create an empty pManager that we deserialize the data into first
        PassiveManagerModel deserializedManager = new PassiveManagerModel();
        BuildPassiveManagerFromSerializedPassiveManager(deserializedManager, data.serializedPassiveManager);

        character.pManager = new PassiveManagerModel();
        character.pManager.myCharacter = character;

        // Copy data from desrialized pManager into the characters actual pManager
        BuildPassiveManagerFromOtherPassiveManager(deserializedManager, character.pManager);
    }
    public void BuildCharacterEntityPassivesFromSummonedCharacterData(CharacterEntityModel character, SummonedCharacterDataSO data)
    {
        Debug.Log("PassiveController.BuildEnemyCharacterEntityPassivesFromEnemyData() called...");

        // Create an empty pManager that we deserialize the data into first
        PassiveManagerModel deserializedManager = new PassiveManagerModel();
        BuildPassiveManagerFromSerializedPassiveManager(deserializedManager, data.serializedPassiveManager);

        character.pManager = new PassiveManagerModel();
        character.pManager.myCharacter = character;

        // Copy data from desrialized pManager into the characters actual pManager
        BuildPassiveManagerFromOtherPassiveManager(deserializedManager, character.pManager);
    }

    #endregion

    // Apply Passive To Character Entities
    #region
    public void ModifyPassiveOnCharacterEntity(PassiveManagerModel pManager, string originalData, int stacks, bool showVFX = true, float vfxDelay = 0f, PassiveManagerModel applier = null)
    {
        Debug.Log("PassiveController.ModifyPassiveOnCharacterEntity() called...");

        // Core stat bonuses
        #region
        if (originalData == "Power")
        {
            ModifyBonusPower(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Dexterity")
        {
            ModifyBonusDexterity(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Draw")
        {
            ModifyBonusDraw(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Initiative")
        {
            ModifyBonusInitiative(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Stamina")
        {
            ModifyBonusStamina(pManager, stacks, showVFX, vfxDelay);
        }
        #endregion

        // Temp Core stat bonuses
        #region
        else if (originalData == "Temporary Power")
        {
            ModifyTemporaryPower(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Temporary Dexterity")
        {
            ModifyTemporaryDexterity(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Temporary Draw")
        {
            ModifyTemporaryDraw(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Temporary Initiative")
        {
            ModifyTemporaryInitiative(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Temporary Stamina")
        {
            ModifyTemporaryStamina(pManager, stacks, showVFX, vfxDelay);
        }
        #endregion

        // Buff passives
        #region
        else if (originalData == "Enrage")
        {
            ModifyEnrage(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Pierce")
        {
            ModifyPierce(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Unbreakable")
        {
            ModifyUnbreakable(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Tranquil Hate")
        {
            ModifyTranquilHate(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Thorns")
        {
            ModifyThorns(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Shield Wall")
        {
            ModifyShieldWall(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Fan Of Knives")
        {
            ModifyFanOfKnives(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Divine Favour")
        {
            ModifyDivineFavour(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Phoenix Form")
        {
            ModifyPhoenixForm(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Poisonous")
        {
            ModifyPoisonous(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Inflamed")
        {
            ModifyInflamed(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Venomous")
        {
            ModifyVenomous(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Overload")
        {
            ModifyOverload(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Source")
        {
            ModifySource(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Fusion")
        {
            ModifyFusion(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Planted Feet")
        {
            ModifyPlantedFeet(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Taken Aim")
        {
            ModifyTakenAim(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Long Draw")
        {
            ModifyLongDraw(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Sharpen Blade")
        {
            ModifySharpenBlade(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Consecration")
        {
            ModifyConsecration(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Growing")
        {
            ModifyGrowing(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Cautious")
        {
            ModifyCautious(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Infuriated")
        {
            ModifyInfuriated(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Battle Trance")
        {
            ModifyBattleTrance(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Balanced Stance")
        {
            ModifyBalancedStance(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Flurry")
        {
            ModifyFlurry(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Lord Of Storms")
        {
            ModifyLordOfStorms(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Sentinel")
        {
            ModifySentinel(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Ruthless")
        {
            ModifyRuthless(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Evangelize")
        {
            ModifyEvangelize(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Well Of Souls")
        {
            ModifyWellOfSouls(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Corpse Collector")
        {
            ModifyCorpseCollector(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Pistolero")
        {
            ModifyPistolero(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Fast Learner")
        {
            ModifyFastLearner(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Demon Form")
        {
            ModifyDemonForm(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Dark Bargain")
        {
            ModifyDarkBargain(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Volatile")
        {
            ModifyVolatile(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Soul Collector")
        {
            ModifySoulCollector(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Magic Magnet")
        {
            ModifyMagicMagnet(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Ethereal")
        {
            ModifyEthereal(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Shocking Touch")
        {
            ModifyShockingTouch(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Storm Shield")
        {
            ModifyStormShield(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Malice")
        {
            ModifyMalice(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Hurricane")
        {
            ModifyHurricane(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Holier Than Thou")
        {
            ModifyHolierThanThou(pManager, stacks, showVFX, vfxDelay);
        }
        #endregion

        // Special Defensive Passives
        #region
        else if (originalData == "Barrier")
        {
            ModifyBarrier(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Rune")
        {
            ModifyRune(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Incorporeal")
        {
            ModifyIncorporeal(pManager, stacks, showVFX, vfxDelay);
        }
        #endregion

        // Aura Passives
        #region
        else if (originalData == "Encouraging Aura")
        {
            ModifyEncouragingAura(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Shadow Aura")
        {
            ModifyShadowAura(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Toxic Aura")
        {
            ModifyToxicAura(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Guardian Aura")
        {
            ModifyGuardianAura(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Hateful Aura")
        {
            ModifyHatefulAura(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Intimidating Aura")
        {
            ModifyIntimidatingAura(pManager, stacks, showVFX, vfxDelay);
        }
        #endregion

        // DoT passives
        #region
        else if (originalData == "Poisoned")
        {
            ModifyPoisoned(applier, pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Burning")
        {
            ModifyBurning(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Bleeding")
        {
            ModifyBleeding(pManager, stacks, showVFX, vfxDelay);
        }
        #endregion

        // Core % Modifier passives
        #region
        else if (originalData == "Wrath")
        {
            ModifyWrath(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Weakened")
        {
            ModifyWeakened(pManager, stacks, pManager, showVFX, vfxDelay);
        }
        else if (originalData == "Grit")
        {
            ModifyGrit(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Vulnerable")
        {
            ModifyVulnerable(pManager, stacks, showVFX, vfxDelay);
        }
        #endregion

        // Disabling Debuff Passives
        #region
        else if (originalData == "Disarmed")
        {
            ModifyDisarmed(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Sleep")
        {
            ModifySleep(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Silenced")
        {
            ModifySilenced(pManager, stacks, showVFX, vfxDelay);
        }

        #endregion

        // Misc Stats
        #region
        else if (originalData == "Fire Ball Bonus Damage")
        {
            ModifyFireBallBonusDamage(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Reflex Shot Bonus Damage")
        {
            ModifyReflexShotBonusDamage(pManager, stacks, showVFX, vfxDelay);
        }
        #endregion
    }
    #endregion

    // Update Passive Icons and Panel View
    #region
    private void BuildPassiveIconViewFromData(PassiveIconView icon, PassiveIconData iconData)
    {
        Debug.Log("PassiveController.BuildPassiveIconViewFromData() called...");

        icon.myIconData = iconData;
        icon.passiveImage.sprite = GetPassiveSpriteByName(iconData.passiveName);

        icon.statusName = iconData.passiveName;
        if (iconData.showStackCount)
        {
            icon.statusStacksText.gameObject.SetActive(true);
        }
        if (iconData.hiddenOnPassivePanel)
        {
            icon.gameObject.SetActive(false);
        }

        icon.statusStacksText.text = icon.statusStacks.ToString();

    }
    private void ModifyIconViewStacks(PassiveIconView icon, int stacksGainedOrLost)
    {
        Debug.Log("PassiveController.ModifyIconViewStacks() called...");

        icon.statusStacks += stacksGainedOrLost;
        icon.statusStacksText.text = icon.statusStacks.ToString();
        if (icon.statusStacks == 0)
        {
            icon.statusStacksText.gameObject.SetActive(false);
        }
    }
    private void StartAddPassiveToPanelProcess(CharacterEntityView view, PassiveIconData iconData, int stacksGainedOrLost)
    {
        Debug.Log("PassiveController.StartAddPassiveToPanelProcess() called...");

        if (view.passiveIcons.Count > 0)
        {
            //StatusIconDataSO si = null;
            //int stacks = 0;
            bool matchFound = false;

            foreach (PassiveIconView icon in view.passiveIcons)
            {
                if (iconData.passiveName == icon.statusName)
                {
                    // Icon already exists in character's list
                    UpdatePassiveIconOnPanel(view, icon, stacksGainedOrLost);
                    matchFound = true;
                    break;
                }
            }

            if (matchFound == false)
            {
                AddNewPassiveIconToPanel(view, iconData, stacksGainedOrLost);
            }
        }
        else
        {
            AddNewPassiveIconToPanel(view, iconData, stacksGainedOrLost);
        }
    }
    private void AddNewPassiveIconToPanel(CharacterEntityView view, PassiveIconData iconData, int stacksGained)
    {
        Debug.Log("PassiveController.AddNewPassiveIconToPanel() called...");

        // only create an icon if the the effects' stacks are at least 1 or -1
        if (stacksGained != 0)
        {
            GameObject newIconGO = Instantiate(PrefabHolder.Instance.PassiveIconViewPrefab, view.passiveIconsVisualParent.transform);
            PassiveIconView newStatus = newIconGO.GetComponent<PassiveIconView>();
            BuildPassiveIconViewFromData(newStatus, iconData);
            ModifyIconViewStacks(newStatus, stacksGained);
            view.passiveIcons.Add(newStatus);
        }

    }
    private void RemovePassiveIconFromPanel(CharacterEntityView view, PassiveIconView iconToRemove)
    {
        Debug.Log("PassiveController.RemovePassiveIconFromPanel() called...");
        view.passiveIcons.Remove(iconToRemove);
        Destroy(iconToRemove.gameObject);
    }
    private void UpdatePassiveIconOnPanel(CharacterEntityView view, PassiveIconView iconToUpdate, int stacksGainedOrLost)
    {
        Debug.Log("PassiveController.UpdatePassiveIconOnPanel() called...");

        ModifyIconViewStacks(iconToUpdate, stacksGainedOrLost);
        if (iconToUpdate.statusStacks == 0)
        {
            RemovePassiveIconFromPanel(view, iconToUpdate);
        }

    }
    #endregion

    // Conditional Checks
    #region
    public bool IsEntityAffectedByPassive(PassiveManagerModel passiveManager, string passiveName, int stacksRequired = 1)
    {
        Debug.Log("PassiveController.IsEntityAffectedByPassive() called, checking for if entity has " + passiveName
            + " with at least " + stacksRequired.ToString() + " stacks");

        bool boolReturned = false;

        // Core stat bonuses
        #region
        if (passiveName == "Power" && passiveManager.bonusPowerStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        if (passiveName == "Dexterity" && passiveManager.bonusDexterityStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        if (passiveName == "Draw" && passiveManager.bonusDrawStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        if (passiveName == "Initiative" && passiveManager.bonusInitiativeStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        if (passiveName == "Stamina" && passiveManager.bonusStaminaStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        #endregion

        // Temp Core stat bonuses
        #region
        if (passiveName == "Temporary Power" && passiveManager.temporaryBonusPowerStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        if (passiveName == "Temporary Dexterity" && passiveManager.temporaryBonusDexterityStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        if (passiveName == "Temporary Draw" && passiveManager.temporaryBonusDrawStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        if (passiveName == "Temporary Initiative" && passiveManager.temporaryBonusInitiativeStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        if (passiveName == "Temporary Stamina" && passiveManager.temporaryBonusStaminaStacks >= stacksRequired)
        {
            boolReturned = true;
        }

        // Disabling Debuffs
        #region
        if (passiveName == "Sleep" && passiveManager.sleepStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        #endregion
        #endregion

        return boolReturned;
    }
    private bool ShouldRuneBlockThisPassiveApplication(PassiveManagerModel pManager, PassiveIconData iconData, int stacks)
    {
        if (pManager.runeStacks > 0 &&
            ((iconData.runeBlocksIncrease && stacks > 0) || (iconData.runeBlocksDecrease && stacks < 0)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
   
    // Modify Specific Passives
    #region

    // Bonus Core Stats
    #region
    public void ModifyBonusPower(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBonusStrength() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Power");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.bonusPowerStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Power +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Power " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            // Update intent GUI, if enemy and attacking
            if (pManager.myCharacter.controller == Controller.AI &&
                pManager.myCharacter.myNextAction != null &&
                (pManager.myCharacter.myNextAction.actionType == ActionType.AttackTarget ||
                 pManager.myCharacter.myNextAction.actionType == ActionType.AttackAllEnemies))
            {
                CharacterEntityController.Instance.UpdateEnemyIntentGUI(pManager.myCharacter);
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyBonusDexterity(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBonusDexterity() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Dexterity");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.bonusDexterityStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Dexterity +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Dexterity " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyBonusInitiative(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBonusInitiative() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Initiative");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.bonusInitiativeStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Initiative + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Initiative " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyBonusStamina(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBonusStamina() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Stamina");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.bonusStaminaStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Stamina +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Stamina " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyBonusDraw(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBonusDraw() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Draw");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.bonusDrawStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Draw + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Draw " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    #endregion

    // Temporary Bonus Core Stats
    #region
    public void ModifyTemporaryPower(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTemporaryStrength() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Temporary Power");
        CharacterEntityModel character = pManager.myCharacter;
        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        int originalStacks = pManager.temporaryBonusPowerStacks;
        pManager.temporaryBonusPowerStacks += stacks;
        int newStackCount = pManager.temporaryBonusPowerStacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                if (originalStacks < 0 && newStackCount == 0)
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Power Debuff Removed");
                    });
                }
                else
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Power +" + stacks.ToString());
                        VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                    });
                }

            }

            else if (stacks < 0 && showVFX)
            {
                if(originalStacks > 0 && newStackCount == 0)
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Power Buff Removed");
                    }, QueuePosition.Back, 0, 0.5f);
                }
                else
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Power " + stacks.ToString());
                    }, QueuePosition.Back, 0, 0.5f);
                }
            }

            // Update intent GUI, if enemy and attacking
            if (pManager.myCharacter.controller == Controller.AI &&
                pManager.myCharacter.myNextAction != null &&
                 (pManager.myCharacter.myNextAction.actionType == ActionType.AttackTarget ||
                 pManager.myCharacter.myNextAction.actionType == ActionType.AttackAllEnemies))
            {
                CharacterEntityController.Instance.UpdateEnemyIntentGUI(pManager.myCharacter);
            }
            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyTemporaryDexterity(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTemporaryDexterity() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Temporary Dexterity");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        int originalStacks = pManager.temporaryBonusDexterityStacks;
        pManager.temporaryBonusDexterityStacks += stacks;
        int newStackCount = pManager.temporaryBonusDexterityStacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                if (originalStacks < 0 && newStackCount == 0)
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Dexterity Debuff Removed");
                    });
                }
                else
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Dexterity +" + stacks.ToString());
                        VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                    });
                }

            }

            else if (stacks < 0 && showVFX)
            {
                if (originalStacks > 0 && newStackCount == 0)
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Dexterity Buff Removed");
                    }, QueuePosition.Back, 0, 0.5f);
                }
                else
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Dexterity " + stacks.ToString());
                    }, QueuePosition.Back, 0, 0.5f);
                }
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyTemporaryInitiative(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTemporaryInitiative() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Temporary Initiative");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        int originalStacks = pManager.temporaryBonusInitiativeStacks;
        pManager.temporaryBonusInitiativeStacks += stacks;
        int newStackCount = pManager.temporaryBonusInitiativeStacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                if (originalStacks < 0 && newStackCount == 0)
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Initiative Debuff Removed");
                    });
                }
                else
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Initiative +" + stacks.ToString());
                        VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                    });
                }

            }

            else if (stacks < 0 && showVFX)
            {
                if (originalStacks > 0 && newStackCount == 0)
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Initiative Buff Removed");
                    }, QueuePosition.Back, 0, 0.5f);
                }
                else
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Initiative " + stacks.ToString());
                    }, QueuePosition.Back, 0, 0.5f);
                }
            }


            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyTemporaryStamina(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTemporaryStamina() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Temporary Stamina");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        int originalStacks = pManager.temporaryBonusStaminaStacks;
        pManager.temporaryBonusStaminaStacks += stacks;
        int newStackCount = pManager.temporaryBonusStaminaStacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                if (originalStacks < 0 && newStackCount == 0)
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Stamina Debuff Removed");
                    });
                }
                else
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Stamina +" + stacks.ToString());
                        VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                    });
                }

            }

            else if (stacks < 0 && showVFX)
            {
                if (originalStacks > 0 && newStackCount == 0)
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Stamina Buff Removed");
                    }, QueuePosition.Back, 0, 0.5f);
                }
                else
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Stamina " + stacks.ToString());
                    }, QueuePosition.Back, 0, 0.5f);
                }
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyTemporaryDraw(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTemporaryDraw() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Temporary Draw");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        int originalStacks = pManager.temporaryBonusDrawStacks;
        pManager.temporaryBonusDrawStacks += stacks;
        int newStackCount = pManager.temporaryBonusDrawStacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                if (originalStacks < 0 && newStackCount == 0)
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Draw Debuff Removed");
                    });
                }
                else
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Draw +" + stacks.ToString());
                        VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                    });
                }

            }

            else if (stacks < 0 && showVFX)
            {
                if (originalStacks > 0 && newStackCount == 0)
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Draw Buff Removed");
                    }, QueuePosition.Back, 0, 0.5f);
                }
                else
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Draw " + stacks.ToString());
                    }, QueuePosition.Back, 0, 0.5f);
                }
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    #endregion

    // Buff Passives
    #region
    public void ModifyFastLearner(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyFastLearner() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Fast Learner");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.fastLearnerStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Fast Learner!");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Fast Learner Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyUnbreakable(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyUnbreakable() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Unbreakable");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.unbreakableStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Unbreakable!");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Unbreakable Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyPistolero(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyPistolero() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Pistolero");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.pistoleroStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Pistolero!");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Pistolero Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            // Update cost of cards in hand
            if (pManager.myCharacter != null)
            {
                CardController.Instance.OnPistoleroModified(pManager.myCharacter);
            }


            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyHurricane(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyHurricane() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Hurricane");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.hurricaneStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Hurricane!");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Hurricane Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyMalice(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyMalice() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Malice");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.maliceStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Malice!");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Malice Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyLongDraw(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyLongDraw() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Long Draw");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.longDrawStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Long Draw!");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Long Draw Removed");
                    //VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifySharpenBlade(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifySharpenBlade() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Sharpen Blade");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.sharpenBladeStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Sharpen Blade!");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Sharpen Blade Removed");
                   // VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyCorpseCollector(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyCorpseCollector() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Corpse Collector");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.corpseCollectorStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Corpse Collector +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Corpse Collector " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyWellOfSouls(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyWellOfSouls() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Well Of Souls");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.wellOfSoulsStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Well Of Souls +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Well Of Souls " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyDarkBargain(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyDarkBargain() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Dark Bargain");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.darkBargainStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Dark Bargain");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Dark Bargain Removed");
                });
            }

            // Update cost of cards in hand
            if (pManager.myCharacter != null)
            {
                CardController.Instance.OnDarkBargainModified(pManager.myCharacter);
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyEvangelize(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyEvangelize() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Evangelize");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.evangelizeStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Evangelize +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Evangelize " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyRuthless(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyRuthless() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Ruthless");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.ruthlessStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Ruthless +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Ruthless " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyEthereal(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyEthereal() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Ethereal");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.etherealStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Ethereal +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Ethereal " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifySentinel(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifySentinel() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Sentinel");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.sentinelStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Sentinel +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Sentinel " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyLordOfStorms(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyLordOfStorms() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Lord Of Storms");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.lordOfStormsStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Lord Of Storms +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Lord Of Storms " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyTranquilHate(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTranquilHate() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Tranquil Hate");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.tranquilHateStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Tranquil Hate +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Tranquil Hate " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyRegeneration(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyRegeneration() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Regeneration");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.regenerationStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Regeneration +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Regeneration " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyEnrage(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyEnrage() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Enrage");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.enrageStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Enrage +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Enrage " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyThorns(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyThorns() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Thorns");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.thornsStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Thorns +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Thorns " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyStormShield(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyStormShield() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Storm Shield");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.stormShieldStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Storm Shield +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Storm Shield " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyVolatile(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyVolatile() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Volatile");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.volatileStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Volatile");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Volatile Removed");
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyInfuriated(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyInfuriated() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Infuriated");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.infuriatedStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Infuriated!");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Infuriated Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyBattleTrance(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBattleTrance() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Battle Trance");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.battleTranceStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Battle Trance!");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Battle Trance Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyShieldWall(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyShieldWall() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Shield Wall");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.shieldWallStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Shield Wall +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Shield Wall " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyGrowing(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyGrowing() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Growing");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.growingStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Growing +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Growing Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyFanOfKnives(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyFanOfKnives() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Fan Of Knives");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.fanOfKnivesStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Fan Of Knives");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Fan Of Knives Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyDemonForm(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyDemonForm() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Demon Form");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.demonFormStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Demon Form");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Demon Form Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyDivineFavour(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyDivineFavour() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Divine Favour");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.divineFavourStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Divine Favour");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Divine Favour Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyMagicMagnet(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyMagicMagnet() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Magic Magnet");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.magicMagnetStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Magic Magnet");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Magic Magnet Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyPhoenixForm(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyPhoenixForm() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Phoenix Form");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.phoenixFormStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Phoenix Form");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Phoenix Form Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyHolierThanThou(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyHolierThanThou() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Holier Than Thou");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.holierThanThouStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Holier Than Thou!");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Holier Than Thou Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyInflamed(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyInflamed() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Inflamed");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.inflamedStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Inflamed");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Inflamed Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyPoisonous(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyPoisonous() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Poisonous");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.poisonousStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Poisonous");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Poisonous Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyShockingTouch(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyShockingTouch() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Shocking Touch");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.shockingTouchStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Shocking Touch");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Shocking Touch Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifySoulCollector(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifySoulCollector() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Soul Collector");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.soulCollectorStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Soul Collector");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Soul Collector Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyConsecration(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyConsecration() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Consecration");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.consecrationStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Consecration");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Consecration Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyBalancedStance(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBalancedStance() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Balanced Stance");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.balancedStanceStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Balanced Stance");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Balanced Stance Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyFlurry(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyFlurry() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Flurry");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.flurryStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Flurry!");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Flurry Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyVenomous(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyVenomous() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Venomous");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.venomousStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Venomous");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Venomous Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifySource(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifySource() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Source");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.sourceStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Source +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGainOverloadEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Source " + stacks.ToString());
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }

           
            // Update cost of source spell cards in hand
            if (pManager.myCharacter != null)
            {
                bool doBreath = false;
                if(stacks > 0)
                {
                    doBreath = true;
                }
                CardController.Instance.OnSourceModified(pManager.myCharacter, doBreath);
            }           
        }


    }
    public void ModifyOverload(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyOverload() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Overload");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.overloadStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Overload +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGainOverloadEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Overload Removed");
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }

            // Handle extra draw from fusion passive, if it is a player character and it is their activation
            if (stacks > 0 &&
                pManager.fusionStacks > 0 &&
                pManager.myCharacter != null &&
                pManager.myCharacter.controller == Controller.Player &&
                ActivationManager.Instance.EntityActivated == pManager.myCharacter
                )
            {
                // Draw a card for each stack of fusion
                for (int i = 0; i < pManager.fusionStacks; i++)
                {
                    CardController.Instance.DrawACardFromDrawPile(pManager.myCharacter);
                }
            }
        }


    }
    public void ModifyFusion(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyFusion() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Fusion");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.fusionStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Fusion +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Fusion Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyPlantedFeet(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyPlantedFeet() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Planted Feet");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.plantedFeetStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Planted Feet!");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Planted Feet Expired");
                });
            }

            // Update cost of cards in hand
            if (pManager.myCharacter != null)
            {
                CardController.Instance.OnMeleeAttackReductionModified(pManager.myCharacter);
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyTakenAim(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTakenAim() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Taken Aim");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.takenAimStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Taken Aim!");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Taken Aim Expired");
                });
            }

            // Update cost of cards in hand
            if (pManager.myCharacter != null)
            {
                CardController.Instance.OnRangedAttackReductionModified(pManager.myCharacter);
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    #endregion

    // Special Defensive Passives
    #region
    public void ModifyRune(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyRune() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Rune");
        CharacterEntityModel character = pManager.myCharacter;

        // Increment stacks
        pManager.runeStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Rune +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Debuff Blocked!" + stacks.ToString());
                    AudioManager.Instance.PlaySoundPooled(Sound.Passive_General_Buff);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyBarrier(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBarrier() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Barrier");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.barrierStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Barrier +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Barrier " + stacks.ToString());
                    //VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyIncorporeal(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyIncorporeal() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Incorporeal");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.incorporealStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Incorporeal!");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Incorporeal Removed");
                    //VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    #endregion

    // Aura Passives
    #region
    public void ModifyToxicAura(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyToxicAura() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Toxic Aura");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.toxicAuraStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Toxic Aura!");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Toxic Aura Removed!");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyGuardianAura(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyGuardianAura() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Guardian Aura");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.guardianAuraStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Guardian Aura");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Guardian Aura Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyPierce(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyPierce() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Pierce");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.pierceStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Pierce");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Pierce Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }

    public void ModifyIntimidatingAura(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyIntimidatingAura() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Intimidating Aura");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.intimidatingAuraStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Intimidating Aura");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Intimidating Aura Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyHatefulAura(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyHatefulAura() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Hateful Aura");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.hatefulAuraStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Hateful Aura");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Hateful Aura Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyEncouragingAura(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyEncouragingAura() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Encouraging Aura");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.encouragingAuraStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Encouraging Aura");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Encouraging Aura Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyShadowAura(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyShadowAura() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Shadow Aura");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.shadowAuraStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Shadow Aura");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Shadow Aura Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyCautious(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyCautious() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Cautious");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.cautiousStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Cautious!");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Cautious Removed");
                    // VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    #endregion

    // Core % Modifier Passives
    #region
    public void ModifyWrath(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyWrath() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Wrath");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        int originalStacks = pManager.wrathStacks;
        pManager.wrathStacks += stacks;
        int newStackCount = pManager.wrathStacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }


            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                if (originalStacks < 0 && newStackCount == 0)
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Wrath Expired");
                    });
                }
                else
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Wrath +" + stacks.ToString());
                        VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                    });
                }

            }

            else if (stacks < 0 && showVFX)
            {
                if (originalStacks > 0 && newStackCount == 0)
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Wrath Expired");
                    }, QueuePosition.Back, 0, 0.5f);
                }
                else
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Wrath " + stacks.ToString());
                    }, QueuePosition.Back, 0, 0.5f);
                }
            }

            // Update intent GUI, if enemy and attacking
            if (pManager.myCharacter.controller == Controller.AI &&
                pManager.myCharacter.myNextAction != null &&
                 (pManager.myCharacter.myNextAction.actionType == ActionType.AttackTarget ||
                 pManager.myCharacter.myNextAction.actionType == ActionType.AttackAllEnemies))
            {
                CharacterEntityController.Instance.UpdateEnemyIntentGUI(pManager.myCharacter);
            }

            // Handle gain block from tranquil hate passive
            if (stacks > 0 &&
                pManager.tranquilHateStacks > 0 &&
                pManager.myCharacter != null
                )
            {
                // Notication v event
                if (showVFX)
                {
                    VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
                    VisualEventManager.Instance.CreateVisualEvent(()=>
                    VisualEffectManager.Instance.CreateStatusEffect(pManager.myCharacter.characterEntityView.WorldPosition, "Tranquil Hate!"));
                }

                // Gain Block
                CharacterEntityController.Instance.GainBlock(pManager.myCharacter,
                    CombatLogic.Instance.CalculateBlockGainedByEffect(pManager.tranquilHateStacks, pManager.myCharacter, pManager.myCharacter), showVFX);
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyWeakened(PassiveManagerModel pManager, int stacks, PassiveManagerModel applier = null, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyWeakened() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Weakened");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        int originalStacks = pManager.weakenedStacks;
        pManager.weakenedStacks += stacks;
        int newStackCount = pManager.weakenedStacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Weakened + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                if (originalStacks > 0 && newStackCount == 0)
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Weakened Expired");
                    }, QueuePosition.Back, 0, 0.5f);
                }
                else
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Weakened " + stacks.ToString());
                    }, QueuePosition.Back, 0, 0.5f);
                }
            }

            // Update intent GUI, if enemy and attacking
            if (character.controller == Controller.AI &&
                character.myNextAction != null &&
                 (character.myNextAction.actionType == ActionType.AttackTarget ||
                 character.myNextAction.actionType == ActionType.AttackAllEnemies))
            {
                CharacterEntityController.Instance.UpdateEnemyIntentGUI(character);
            }
            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }

            // Check weakened applied listeners
            if (applier != null &&
                stacks > 0 &&
                applier.myCharacter == ActivationManager.Instance.EntityActivated)
            {
                CardController.Instance.HandleOnWeakenedAppliedCardListeners(applier.myCharacter);
            }

            // Check malice on applier
            if (applier != null &&
                stacks > 0 &&
                applier.maliceStacks > 0)
            {
                if (showVFX)
                {
                    VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
                }
                ModifyVulnerable(pManager, 1, showVFX, vfxDelay);
            }
        }
    }
    public void ModifyVulnerable(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyVulnerable() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Vulnerable");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        int originalStacks = pManager.vulnerableStacks;
        pManager.vulnerableStacks += stacks;
        int newStackCount = pManager.vulnerableStacks;


        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Vulnerable + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                if (originalStacks > 0 && newStackCount == 0)
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Vulnerable Expired");
                    }, QueuePosition.Back, 0, 0.5f);
                }
                else
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Vulnerable " + stacks.ToString());
                    }, QueuePosition.Back, 0, 0.5f);
                }
            }

            // Update intent GUI of ai's targetting this character
            foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(character))
            {                
                if (enemy.controller == Controller.AI &&
                   enemy.myNextAction != null &&
                   enemy.currentActionTarget != null &&
                   enemy.currentActionTarget == character &&
                   enemy.myNextAction.actionType == ActionType.AttackTarget)
                {
                    CharacterEntityController.Instance.UpdateEnemyIntentGUI(enemy);
                }
            }
            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyGrit(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyGrit() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Grit");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        int originalStacks = pManager.gritStacks;
        pManager.gritStacks += stacks;
        int newStackCount = pManager.gritStacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Grit +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                if (originalStacks > 0 && newStackCount == 0)
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Grit Expired");
                    }, QueuePosition.Back, 0, 0.5f);
                }
                else
                {
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Grit " + stacks.ToString());
                    }, QueuePosition.Back, 0, 0.5f);
                }
            }

            // Update intent GUI of ai's targetting this character
            foreach (CharacterEntityModel entity in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(pManager.myCharacter))
            {
                if (entity.controller == Controller.AI &&
                   entity.myNextAction != null &&
                   entity.currentActionTarget == pManager.myCharacter &&
                   entity.myNextAction.actionType == ActionType.AttackTarget)
                {
                    CharacterEntityController.Instance.UpdateEnemyIntentGUI(entity);
                }
            }
            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    #endregion

    // Disabling Debuff Passives
    #region
    public void ModifyDisarmed(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyDisarmed() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Disarmed");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.disarmedStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Disarmed!");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Disarmed Removed");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifySilenced(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifySilenced() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Silenced");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.silencedStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Silenced!");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Silenced Removed");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifySleep(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifySleep() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Sleep");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.sleepStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Sleeping!");
                });

            }

            else if (stacks < 0 && showVFX && pManager.sleepStacks != 0)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Sleeping " + stacks.ToString());
                });
            }

            else if (stacks < 0 && showVFX && pManager.sleepStacks == 0)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Woken Up!");
                });
            }

            // Handle enemy woken up, set new intent
            if (pManager.myCharacter.controller == Controller.AI)
            {
                CharacterEntityController.Instance.StartAutoSetEnemyIntentProcess(pManager.myCharacter);
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    #endregion

    // DoT Passives
    #region
    public void ModifyPoisoned(PassiveManagerModel applier, PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyPoisoned() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Poisoned");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Add venomous bonus from applier
        if (applier != null &&
            applier.venomousStacks > 0)
        {
            stacks += applier.venomousStacks;
        }

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.poisonedStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Poisoned + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateApplyPoisonedEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Poisoned " + stacks.ToString());
                });
            }
            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyBurning(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBurning() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Burning");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.burningStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Burning +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateApplyBurningEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Burning " + stacks.ToString());
                });
            }
            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyBleeding(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBleeding() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Bleeding");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.bleedingStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Bleeding +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateBloodExplosion(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Bleeding " + stacks.ToString());
                });
            }
            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    #endregion

    // Misc Passives
    #region
    public void ModifyTaunted(CharacterEntityModel taunter, PassiveManagerModel targetPManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTaunted() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Taunted");
        CharacterEntityModel character = targetPManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(targetPManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(targetPManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks and cache taunter
        targetPManager.tauntStacks += stacks;
        targetPManager.myTaunter = taunter;

        // Null taunt variable when removing taunt
        if (targetPManager.tauntStacks == 0)
        {
            targetPManager.myTaunter = null;
        }

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Taunted");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Taunted Removed");
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyFireBallBonusDamage(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBonusFireBallDamage() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Fire Ball Bonus Damage");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.fireBallBonusDamageStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Fire Ball Damage +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Fire Ball Damage " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyReflexShotBonusDamage(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyReflexShotBonusDamage() called...");

        // Setup + Cache refs
        PassiveIconData iconData = GetPassiveIconDataByName("Reflex Shot Bonus Damage");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.reflexShotBonusDamageStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Reflex Shot Damage +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Reflex Shot Damage " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    #endregion
    #endregion
}