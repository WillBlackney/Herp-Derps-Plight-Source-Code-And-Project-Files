using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class PassiveManagerModel
{
    [Header("Core Properties")]
    [HideInInspector] public CharacterEntityModel myCharacter;
    [HideInInspector] public CharacterData myCharacterData;

    // Core stat bonuses
    [BoxGroup("Core Stat Bonus Passives", centerLabel: true)]
    [LabelWidth(200)]
    public int bonusStrengthStacks;
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
    public int temporaryBonusStrengthStacks;
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

}
