using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
    
}

public enum DamageType
{
    None,
    Physical,
    Poison, 
    Frost, 
    Fire, 
    Shadow, 
    Air
}
public enum CardType
{
    None,
    Skill,
    MeleeAttack,
    RangedAttack,
    Power
};

public enum TargettingType
{
    NoTarget,
    Ally,
    AllyOrSelf,
    Enemy,
    AllCharacters
};

public enum TalentSchool
{
    None, Neutral, Brawler, Duelist, Assassination, Guardian, Pyromania, Cyromancy, Ranger, Manipulation,
    Divinity, Shadowcraft, Corruption, Naturalism,
}
public enum Rarity
{
    None, 
    Common,
    Rare,
    Epic,
    Legendary,
}

public enum CombatGameState
{
    None,
    VictoryTriggered,
    DefeatTriggered,
    CombatActive,
}
public enum ItemType
{
    None,
    OneHandMelee,
    TwoHandMelee,
    TwoHandRanged,
    Shield,
}
