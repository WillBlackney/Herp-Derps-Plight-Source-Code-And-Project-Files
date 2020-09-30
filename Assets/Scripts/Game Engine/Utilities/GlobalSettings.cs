using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GlobalSettings : Singleton<GlobalSettings>
{
    // General Info
    [BoxGroup("Card Settings", centerLabel: true)]
    [LabelWidth(200)]
    public InnateSettings innateSetting;

    [BoxGroup("Card Settings")]
    [LabelWidth(200)]
    public OnPowerCardPlayedSettings onPowerCardPlayedSetting;

    [BoxGroup("Combat Settings", centerLabel: true)]
    [LabelWidth(200)]
    public InitiativeSettings initiativeSetting;
}

public enum InnateSettings
{
    DrawInnateCardsExtra = 0,
    PrioritiseInnate = 1,
}
public enum InitiativeSettings
{
    RollInitiativeOnceOnCombatStart = 0,
    RerollInitiativeEveryTurn = 1,
}
public enum OnPowerCardPlayedSettings
{
    RemoveFromPlay = 0,
    MoveToDiscardPile = 1,
    Expend = 2,
}
