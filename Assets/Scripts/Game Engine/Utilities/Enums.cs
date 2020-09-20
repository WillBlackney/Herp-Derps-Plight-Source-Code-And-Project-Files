using System;

public class Enums
{
    
}

// Card Related Enums
#region
public enum CardType
{
    None = 0,
    Skill = 1,
    MeleeAttack = 2,
    RangedAttack = 3,
    Power = 4,
};
public enum TargettingType
{
    NoTarget = 0,
    Ally = 1,
    AllyOrSelf = 2,
    Enemy = 3,
    AllCharacters = 4,
};
public enum CardWeaponRequirement
{
    None = 0,
    Shielded = 1,
    TwoHanded = 2,
    DW = 3,
    Ranged = 4,
}
public enum CardCollection
{
    Hand = 0,
    DrawPile = 1,
    DiscardPile = 2,
    PermanentDeck = 3,
}
[Serializable]
public enum CardEffectType
{
    None = 0,
    AddCardsToHand = 1,
    ApplyPassiveToSelf = 2,
    ApplyPassiveToTarget = 3,
    ApplyPassiveToAllEnemies = 4,
    ApplyPassiveToAllAllies = 5,
    DamageTarget = 6,
    DamageSelf = 7,
    DamageAllEnemies = 8,
    DrawCards = 9,
    GainBlockSelf = 10,
    GainBlockTarget = 19,
    GainBlockAllAllies = 12,
    GainEnergy = 13,
    LoseHP = 14,
    RemoveAllPoisonedFromSelf = 15,
    RemoveAllPoisonedFromTarget = 16,
    RemoveAllOverloadFromSelf = 20,
    TauntTarget = 17,
    TauntAllEnemies = 18,



}
[Serializable]
public enum ExtraDrawEffect
{
    None,
    ReduceEnergyCostThisCombat,
    SetEnergyCostToZeroThisCombat,
}
[Serializable]
public enum CardEventListenerType
{
    None,
    OnLoseHealth,
    OnDraw,
    OnActivationEnd,
}

[Serializable]
public enum CardEventListenerFunction
{
    None,
    ReduceCardEnergyCost,
    ApplyPassiveToSelf
}
#endregion

// Commonly Shared Enums
#region
public enum DamageType
{
    None = 0,
    Physical = 1,
    Poison = 2,
    Frost = 3,
    Fire = 4,
    Shadow = 5,
    Air = 6,
}
public enum TalentSchool
{
    None = 0,
    Arms = 14,
    Neutral = 1,
    Brawler = 2,
    Duelist = 3,
    Assassination = 4,
    Guardian = 5,
    Pyromania = 6,
    Cyromancy = 7,
    Ranger = 8,
    Manipulation = 9,
    Divinity = 10,
    Shadowcraft = 11,
    Corruption = 12,
    Naturalism = 13,
}
public enum Rarity
{
    None = 0,
    Common = 1,
    Rare = 2,
    Epic = 3,
    Legendary = 4,
}
[Serializable]
public enum CharacterRace 
{ 
    None = 0,
    Human = 1,
    Orc = 2, 
    Undead = 3, 
    Elf = 4, 
    Goblin = 5, 
    Satyr = 6,
    Gnoll = 7,
};
#endregion

// Item Enums
#region
public enum ItemType
{
    None = 0,
    OneHandMelee = 1,
    TwoHandMelee = 2,
    TwoHandRanged = 3,
    Shield = 4,
}
#endregion

// Level Related Enums
#region
public enum FacingDirection
{
    Right = 0,
    Left = 1
}
public enum AllowedEntity
{
    None = 0,
    All = 1,
    Defender = 2,
    Enemy =3, 
}

#endregion

// Character Entity Enums
#region
public enum Allegiance
{
    Player = 0,
    Enemy = 1,
}
public enum Controller
{
    Player = 0,
    AI = 1,
}
public enum LivingState
{
    Alive = 0,
    Dead = 1,
}
#endregion

// Enemy Action Enums
#region
public enum ActionType
{
    AttackTarget = 0,
    AttackAllEnemies = 1,
    BuffSelf = 2,
    BuffTarget = 3,
    BuffAllAllies = 4,
    DebuffTarget = 5,
    DebuffAllEnemies = 6,
    DefendSelf = 7,
    DefendTarget = 8,
    DefendAllAllies = 9,
    Sleep = 10,
    PassTurn = 11,
    AddCard = 12,
}
public enum ActionRequirementType
{
    None = 0,
    IsTurn = 1,
    IsMoreThanTurn = 2,
    IsLessThanTurn = 3,
    HealthIsLessThan = 4,
    HealthIsMoreThan = 5,
    AtLeastXAlliesAlive = 6,
    HaventUsedActionInXTurns = 7,
    ActivatedXTimesOrMore = 8,
    ActivatedXTimesOrLess = 9,
    HasPassiveTrait = 10,
}
public enum IntentImage
{
    Attack = 0,
    AttackAll = 11,
    AttackBuff = 1,
    AttackDebuff = 2,
    AttackDefend = 3,
    Buff = 4,
    DefendBuff = 5,
    Defend = 6,
    GreenDebuff = 7,
    PurpleDebuff = 8,
    Unknown = 9,
    Flee = 10,
    

}
#endregion

// Misc Enums
#region
public enum CombatGameState
{
    None = 0,
    VictoryTriggered = 1,
    DefeatTriggered = 2,
    CombatActive = 3,
}

#endregion

// Visual Event Enums
#region
public enum EventDetail
{
    None =0,
    CardDraw = 1,
    GameOverVictory = 2,
    GameOverDefeat = 3,
}
public enum QueuePosition
{
    Front = 0,
    Back = 1,
}
#endregion

// Enemy Wave Enums
#region
public enum CombatDifficulty
{
    None = 0,
    Basic = 1,
    Elite = 2,
    Boss = 3,
}
#endregion

// UCM Enums
#region
public enum BodyPartType
{
    None = 0, 
    Head = 1, 
    Face = 2, 
    Chest = 3,
    RightLeg = 4, 
    LeftLeg = 5,
    RightArm = 6,
    RightHand = 7,
    LeftArm = 8, 
    LeftHand = 9,
    HeadWear = 10, 
    ChestWear = 11, 
    RightLegWear = 12,
    LeftLegWear = 13, 
    RightArmWear = 14, 
    RightHandWear = 15, 
    LeftArmWear = 16, 
    LeftHandWear = 17, 
    MainHandWeapon = 18, 
    OffHandWeapon = 19,
};
#endregion

// Animation Event Data Enums
#region

public enum MovementAnimEvent
{
    None = 0,
    MoveTowardsTarget = 1,
    MoveToCentre = 2,
}
public enum CharacterAnimation
{
    None = 0,
    MeleeAttack = 1,
    ShootBow = 2,
    ShootProjectile = 3,
    Skill = 4,
}
public enum ParticleEffect
{
    None = 0,
    GeneralBuff = 14,
    GeneralDebuff = 15,
    ApplyBurning = 16,
    ApplyPoisoned = 17,
    GainOverload = 18,
    LightningExplosion1 = 1,
    FireExplosion1 = 4,
    PoisonExplosion1 = 5,
    ShadowExplosion1 = 6,
    FrostExplosion1 = 7,
    BloodExplosion1 = 2,
    AoeMeleeArc = 3,
    FireNova = 8,
    PoisonNova = 9,
    ShadowNova = 10,
    LightningNova = 11,
    FrostNova = 12,
    HolyNova = 13,

}
public enum ProjectileFired
{
    None = 4,
    Arrow = 0,
    FireBall1 = 1,
    ShadowBall1 = 2,
    PoisonBall1 = 3,
    FrostBall1 = 6,
    LightningBall1 = 7,
    HolyBall1 = 8,

}
#endregion


