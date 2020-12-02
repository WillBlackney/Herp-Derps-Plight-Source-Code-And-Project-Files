using System;
using Sirenix.OdinInspector;

[Serializable]
public class SerializedPassiveManagerModel
{
    // Core stat bonuses
    [BoxGroup("Core Stat Bonus Passives", centerLabel: true)]
    [LabelWidth(200)]
    public int bonusPowerStacks;
    [BoxGroup("Core Stat Bonus Passives")]
    [LabelWidth(200)]
    public int bonusDexterityStacks;

    [BoxGroup("Core Stat Bonus Passives", centerLabel: true)]
    [LabelWidth(200)]
    public int bonusStaminaStacks;

    [BoxGroup("Core Stat Bonus Passives")]
    [LabelWidth(200)]
    public int bonusInitiativeStacks;

    [BoxGroup("Core Stat Bonus Passives")]
    [LabelWidth(200)]
    public int bonusDrawStacks;

    // Temp Core stat bonuses
    [BoxGroup("Temp Core Stat Bonus Passives", centerLabel: true)]
    [LabelWidth(200)]
    public int temporaryBonusPowerStacks;
    [BoxGroup("Temp Core Stat Bonus Passives")]
    [LabelWidth(200)]
    public int temporaryBonusDexterityStacks;

    [BoxGroup("Temp Core Stat Bonus Passives", centerLabel: true)]
    [LabelWidth(200)]
    public int temporaryBonusStaminaStacks;

    [BoxGroup("Temp Core Stat Bonus Passives")]
    [LabelWidth(200)]
    public int temporaryBonusInitiativeStacks;

    [BoxGroup("Temp Core Stat Bonus Passives")]
    [LabelWidth(200)]
    public int temporaryBonusDrawStacks;


    [BoxGroup("Special Defensive Passives", centerLabel: true)]
    [LabelWidth(200)]
    public int runeStacks;
    [BoxGroup("Special Defensive Passives")]
    [LabelWidth(200)]
    public int barrierStacks;

    // Buff Passives
    [BoxGroup("Buff Passives", centerLabel: true)]
    [LabelWidth(200)]
    public int enrageStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int shieldWallStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int fanOfKnivesStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int divineFavourStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int phoenixFormStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int poisonousStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int venomousStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int overloadStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int fusionStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int plantedFeetStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int takenAimStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int longDrawStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int sharpenBladeStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int consecrationStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int growingStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int cautiousStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int infuriatedStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int battleTranceStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int balancedStanceStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int flurryStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int lordOfStormsStacks;    
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int sentinelStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int ruthlessStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int evangelizeStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int wellOfSoulsStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int corpseCollectorStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int pistoleroStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int fastLearnerStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int demonFormStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int darkBargainStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int volatileStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int soulCollectorStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int magicMagnetStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int etherealStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int thornsStacks;

    // Aura Passives
    [BoxGroup("Aura Passives", centerLabel: true)]
    [LabelWidth(200)]
    public int encouragingAuraStacks;
    [BoxGroup("Aura Passives")]
    [LabelWidth(200)]
    public int shadowAuraStacks;
    [BoxGroup("Aura Passives")]
    [LabelWidth(200)]
    public int guardianAuraStacks;
    [BoxGroup("Aura Passives")]
    [LabelWidth(200)]
    public int toxicAuraStacks;
    [BoxGroup("Aura Passives")]
    [LabelWidth(200)]
    public int hatefulAuraStacks;

    // Buff Passives
    [BoxGroup("Core Damage % Modifier Passives", centerLabel: true)]
    [LabelWidth(200)]
    public int wrathStacks;

    [BoxGroup("Core Damage % Modifier Passives")]
    [LabelWidth(200)]
    public int weakenedStacks;

    [BoxGroup("Core Damage % Modifier Passives")]
    [LabelWidth(200)]
    public int vulnerableStacks;

    [BoxGroup("Core Damage % Modifier Passives")]
    [LabelWidth(200)]
    public int gritStacks;

    // Disabling Debuff Passives
    [BoxGroup("Disabling Debuff Passives", centerLabel: true)]
    [LabelWidth(200)]
    public int disarmedStacks;

    [BoxGroup("Disabling Debuff Passives")]
    [LabelWidth(200)]
    public int silencedStacks;

    [BoxGroup("Disabling Debuff Passives")]
    [LabelWidth(200)]
    public int sleepStacks;

    // DoT Debuff Passives
    [BoxGroup("DoT Debuff Passives", centerLabel: true)]
    [LabelWidth(200)]
    public int poisonedStacks;
    [BoxGroup("DoT Debuff Passives")]
    [LabelWidth(200)]
    public int burningStacks;
    [BoxGroup("DoT Debuff Passives")]
    [LabelWidth(200)]
    public int bleedingStacks;

    // Misc Passives
    [BoxGroup("Misc Passives", centerLabel: true)]
    [LabelWidth(200)]
    public int fireBallBonusDamageStacks;

    [BoxGroup("Misc Passives")]
    [LabelWidth(200)]
    public int sourceStacks;


}
