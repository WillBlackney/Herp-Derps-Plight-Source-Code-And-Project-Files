using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public int meleeAttackReductionStacks;
    [BoxGroup("Buff Passives")]
    [LabelWidth(200)]
    public int consecrationStacks;

    // Aura Passives
    [BoxGroup("Aura Passives", centerLabel: true)]
    [LabelWidth(200)]
    public int encouragingAuraStacks;
    [BoxGroup("Aura Passives")]
    [LabelWidth(200)]
    public int shadowAuraStacks;

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

    // DoT Debuff Passives
    [BoxGroup("DoT Debuff Passives", centerLabel: true)]
    [LabelWidth(200)]
    public int poisonedStacks;
    [BoxGroup("DoT Debuff Passives")]
    [LabelWidth(200)]
    public int burningStacks;

    // Misc Passives
    [BoxGroup("Misc Passives", centerLabel: true)]
    [LabelWidth(200)]
    public int fireBallBonusDamageStacks;


}
