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
    public int meleeAttackReductionStacks;
    public int consecrationStacks;

    // Aura passives
    public int encouragingAuraStacks;

    // Core Damage % Modifier Passives
    public int wrathStacks;
    public int vulnerableStacks;
    public int weakenedStacks;
    public int gritStacks;
    

    // Misc passives
    [HideInInspector] public int tauntStacks;
    [HideInInspector] public CharacterEntityModel myTaunter;
    public int fireBallBonusDamageStacks;

    // DoT Debuff Passives
    public int poisonedStacks;
    public int burningStacks;

}
