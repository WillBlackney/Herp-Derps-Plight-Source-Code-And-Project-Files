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

    // Core Damage % Modifier Passives
    public int wrathStacks;
    public int vulnerableStacks;
    public int weakenedStacks;
    public int gritStacks;
    public int poisonousStacks;
    public int venomousStacks;

    // Misc passives
    [HideInInspector] public int tauntStacks;
    [HideInInspector] public CharacterEntityModel myTaunter;

    // DoT Debuff Passives
    public int poisonedStacks;

}
