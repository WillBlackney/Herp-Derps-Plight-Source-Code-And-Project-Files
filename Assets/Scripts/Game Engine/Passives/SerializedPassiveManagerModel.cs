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

    // Buff Passives
    [BoxGroup("Buff Passives", centerLabel: true)]
    [LabelWidth(200)]
    public int enrageStacks;

}
