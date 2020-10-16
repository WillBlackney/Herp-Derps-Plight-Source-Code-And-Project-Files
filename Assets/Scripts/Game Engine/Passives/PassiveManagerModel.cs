using UnityEngine;
public class PassiveManagerModel
{
    // Properties + References
    public CharacterEntityModel myCharacter;

    // Core stat bonuses
    public int bonusPowerStacks;
    public int bonusDexterityStacks;
    public int bonusStaminaStacks;
    public int bonusInitiativeStacks;
    public int bonusDrawStacks;

    // Temp Core stat bonuses
    public int temporaryBonusPowerStacks;
    public int temporaryBonusDexterityStacks;
    public int temporaryBonusStaminaStacks;
    public int temporaryBonusInitiativeStacks;
    public int temporaryBonusDrawStacks;

    // Special Defensive Passives
    public int runeStacks;
    public int barrierStacks;

    // Buff Passive bonuses
    public int enrageStacks;
    public int shieldWallStacks;
    public int fanOfKnivesStacks;
    public int divineFavourStacks;
    public int phoenixFormStacks;
    public int poisonousStacks;
    public int venomousStacks;
    public int overloadStacks;
    public int fusionStacks;
    public int plantedFeetStacks;
    public int takenAimStacks;
    public int consecrationStacks;
    public int growingStacks;
    public int cautiousStacks;
    public int infuriatedStacks;
    public int battleTranceStacks;
    public int balancedStanceStacks;
    public int flurryStacks;

    // Aura passives
    public int encouragingAuraStacks;
    public int shadowAuraStacks;

    // Core Damage % Modifier Passives
    public int wrathStacks;
    public int vulnerableStacks;
    public int weakenedStacks;
    public int gritStacks;

    // Disabling Debuffs
    public int disarmedStacks;
    public int sleepStacks;


    // Misc passives
    [HideInInspector] public int tauntStacks;
    [HideInInspector] public CharacterEntityModel myTaunter;
    public int fireBallBonusDamageStacks;

    // DoT Debuff Passives
    public int poisonedStacks;
    public int burningStacks;

}
