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
    DualWield = 3,
    Ranged = 4,
}
public enum CardCollection
{
    Hand = 0,
    DrawPile = 1,
    DiscardPile = 2,
    PermanentDeck = 3,
    ExpendPile = 4,
    CardLibrary = 5,
}
[Serializable]
public enum CardEffectType
{
    None = 0,
    AddCardsToHand = 1,
    AddRandomBlessingsToHand = 21,
    ModifyAllCardsInHand = 24,
    ApplyPassiveToSelf = 2,
    ApplyPassiveToTarget = 3,
    ApplyPassiveToAllEnemies = 4,
    ApplyPassiveToAllAllies = 5,
    ChooseCardInHand = 23,
    DamageTarget = 6,
    DamageSelf = 7,
    DamageAllEnemies = 8,
    DrawCards = 9,
    DiscoverCards = 22,
    DoubleTargetsPoisoned = 29,
    GainBlockSelf = 10,
    GainBlockTarget = 19,
    GainBlockAllAllies = 12,
    GainEnergy = 13,
    LoseHP = 14,
    HealSelf = 25,
    HealTarget = 26,
    HealSelfAndAllies = 27,
    ModifyMyPermanentDeckCard = 28,
    RemoveAllBlock = 30, 
    RemoveAllPoisonedFromSelf = 15,
    RemoveAllPoisonedFromTarget = 16,
    RemoveAllOverloadFromSelf = 20,
    RemoveAllBurningFromSelf = 31,
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
    None = 0,
    OnLoseHealth = 1,
    OnDraw = 2,
    OnActivationEnd = 3,
    OnMeleeAttackCardPlayed = 4,
    OnFireBallCardPlayed = 6,
    OnTargetKilled = 5,
}

[Serializable]
public enum CardEventListenerFunction
{
    None = 0,
    ReduceCardEnergyCost = 1,
    ReduceCardEnergyCostThisActivation = 3,
    ApplyPassiveToSelf = 2,
    DrawThis = 4,
    ModifyMaxHealth = 5,
}
#endregion

// Commonly Shared Enums
#region
public enum DamageType
{
    None = 0,
    Physical = 1,
    Magic = 7,
    Poison = 2,
    Frost = 3,
    Fire = 4,
    Shadow = 5,
    Air = 6,
}
public enum TalentSchool
{
    None = 0,
    Neutral = 1,
    Warfare = 2,
    Scoundrel = 3,
    Guardian = 5,
    Pyromania = 6,
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
public enum Passive
{
    None = 0,
    Barrier = 1,
    BattleTrance = 40,
    BalancedStance = 41,
    Burning = 2,
    Cautious = 3,
    Consecration = 4,
    CorpseCollector = 49,
    DarkBargain = 55,
    DemonForm = 54,
    Dexterity = 5,
    Disarmed = 6,
    DivineFavour = 7,
    Draw = 8,
    EncouragingAura = 9,
    Enrage = 10,
    Evangelize = 47,
    FanOfKnives = 11,
    FastLearner = 53,
    FireBallBonusDamage = 12,
    Flurry = 42,
    Fusion = 13,
    Grit = 14,
    Growing = 15,
    GuardianAura = 44,
    Infuriated = 16,
    Initiative = 17,
    LongDraw = 51,
    LordOfStorms = 43,
    Overload = 18,
    Pistolero = 52,
    PhoenixForm = 19,
    PlantedFeet = 20,
    Poisoned = 21,
    Poisonous = 22,
    Power = 23,
    Rune = 24,
    Ruthless = 46,
    Sentinel = 45,
    ShadowAura = 25,
    SharpenBlade = 54,
    ShieldWall = 26,
    Sleep = 27,
    Stamina = 28,
    Silenced = 43,
    TakenAim = 39,
    Taunted = 29,
    TemporaryDexterity = 30,
    TemporaryDraw = 31,
    TemporaryInitiative = 32,
    TemporaryPower = 33,
    TemporaryStamina = 34,
    ToxicAura = 50, 
    Venomous = 35,
    Vulnerable = 36,
    Weakened = 37,
    WellOfSouls = 48,
    Wrath = 38,

};
public enum KeyWordType
{
    None = 0,
    Expend = 1,
    Innate = 2,
    Fleeting = 3,
    Unplayable = 4,
    LifeSteal = 12,
    Blessing = 5,
    Shank = 6,
    Burn = 11,
    WeaponRequirement = 7,
    Passive = 8,
    Block = 9,
    Energy = 10,
}
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
    AddCardToTargetCardCollection = 12,
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
public enum TextColor
{
    None = 0,
    White = 1,
    BlueNumber = 2,
    KeyWordYellow = 3,
    PhysicalBrown = 4,
    MagicPurple = 9,
    FireRed = 5,
    ShadowPurple = 6,
    AirBlue = 7,
    PoisonGreen = 8
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
    BatchedEvent = 3,
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
public enum AnimationEventType
{
    None = 0,
    Movement = 1,
    CharacterAnimation = 2,
    ParticleEffect = 3,
    CameraShake = 4,
    SoundEffect = 5,
    ScreenOverlay = 7,
    Delay = 6,
}
public enum ScreenOverlayType
{
    None = 0,
    EdgeOverlay = 1,
}
public enum ScreenOverlayColor
{
    White = 0,
    Fire = 1,
    Purple = 2,
}
public enum MovementAnimEvent
{
    None = 0,
    MoveTowardsTarget = 1,
    MoveToCentre = 2,
    MoveToMyNode = 3,
}
public enum CharacterAnimation
{
    None = 0,
    MeleeAttack = 1,
    AoeMeleeAttack = 5,
    ShootBow = 2,
    ShootProjectile = 3,
    Skill = 4,
}
public enum ParticleEffect
{
    None = 0,
    GeneralBuff = 14,
    GeneralDebuff = 15,
    HealEffect = 24,
    ApplyBurning = 16,
    ApplyPoisoned = 17,
    GainOverload = 18,
    LightningExplosion = 1,
    FireExplosion = 4,
    PoisonExplosion = 5,
    ShadowExplosion = 6,
    FrostExplosion = 7,
    BloodExplosion = 2,
    GhostExplosionPurple = 22,
    ConfettiExplosionRainbow = 23,
    SmallMeleeImpact = 21,
    AoeMeleeArc = 3,
    FireNova = 8,
    PoisonNova = 9,
    ShadowNova = 10,
    LightningNova = 11,
    FrostNova = 12,
    HolyNova = 13,
    RitualCircleYellow = 19,
    RitualCirclePurple = 20,

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
    FireMeteor = 9,

}
public enum ProjectileStartPosition
{
    Shooter = 0,
    SkyCentreOffScreen = 1,
    AboveTargetOffScreen
}
public enum CameraShakeType
{
    None = 0,
    Small = 1,
    Medium = 2,
    Large = 3,
}
public enum CreateOnCharacter
{
    None,
    Self,
    Target,
    All,
    AllAllies,
    AllEnemies,
}
#endregion

// Journey + Encounter Enums
#region
public enum EncounterType
{
    None = 0,
    BasicEnemy = 1,
    EliteEnemy = 2,
    BossEnemy = 3,
}
#endregion


