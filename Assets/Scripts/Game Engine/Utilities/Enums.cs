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
public enum CampTargettingType
{
    NoTarget = 0,
    Target = 1,
};
public enum CardWeaponRequirement
{
    None = 0,
    Shielded = 1,
    TwoHanded = 2,
    DualWield = 3,
    Ranged = 4,
}
public enum TargetRequirement
{
    None = 0,
    IsWeakened = 1,
    IsVulnerable = 2,
}
public enum UpgradeFilter
{
    Any = 0,
    OnlyNonUpgraded = 1,
    OnlyUpgraded = 2,
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
    AddCardsToDrawPile = 36,
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
    GainEnergySelf = 13,
    GainEnergyTarget = 32,
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
    RemoveAllBurningFromTarget = 35,
    RemoveWeakenedFromSelfAndAllies = 33,
    RemoveVulnerableFromSelfAndAllies = 34,
    SummonCharacter = 37,
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
    OnActivationEnd = 3,
    OnArcaneBoltCardPlayed = 8,
    OnBlessingCardPlayed = 11,
    OnCardDraw = 2,
    OnFireBallCardPlayed = 6,
    OnLoseHealth = 1,   
    OnMeleeAttackCardPlayed = 4,    
    OnTargetKilled = 5,
    OnThisCardDrawn = 7,
    OnThisCardPlayed = 9,   
    OnWeakenedApplied = 12,
    WhileHoldingCertainCard = 10,
}

[Serializable]
public enum CardEventListenerFunction
{
    None = 0,
    ReduceCardEnergyCost = 1,
    ReduceCardEnergyCostThisActivation = 3,
    ApplyPassiveToSelf = 2,
    DrawThis = 4,
    LoseHealth = 7,
    ModifyMaxHealth = 5,
    ModifyEnergy = 6,
}
#endregion

// Commonly Shared Enums
#region
public enum CharacterModelSize
{
    Small = 1,
    Normal = 0,
    Large = 2,
    Massive = 3,
}
public enum DamageType
{
    None = 0,
    Physical = 1,
    Magic = 2,
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
    Polymorph = 14,
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
    Ent = 8,
    Demon = 9,
};
public enum Passive
{
    None = 0,
    Barrier = 1,
    BattleTrance = 40,
    BalancedStance = 41,
    Bleeding = 58,
    Bully = 84,
    Burning = 2,
    Cautious = 3,
    Consecration = 4,
    CorpseCollector = 49,
    DarkBargain = 55,
    DemonForm = 66,
    Dexterity = 5,
    Disarmed = 6,
    DivineFavour = 7,
    Draw = 8,
    EncouragingAura = 9,
    Enrage = 10,
    EnragingAura = 86,
    Ethereal = 61,
    Evangelize = 47,
    FanOfKnives = 11,
    FastLearner = 53,
    FireBallBonusDamage = 12,
    Flurry = 42,
    Fusion = 13,
    Grit = 14,
    Growing = 15,
    GuardianAura = 44,
    HatefulAura = 62,
    HolierThanThou = 75,
    Hurricane = 74,
    Incorporeal = 77,
    Inflamed = 65,
    Infuriated = 16,
    Initiative = 17,
    IntimidatingAura = 76,
    LongDraw = 51,
    LordOfStorms = 57,
    Overload = 18,
    MagicMagnet = 60,
    MagicDamageBonus = 81,
    Malice = 69,
    Patience = 79,
    Pierce = 70,
    Pistolero = 52,
    PhoenixForm = 19,
    PhysicalDamageBonus = 80,
    PlantedFeet = 20,
    Poisoned = 21,
    Poisonous = 22,
    Power = 23,
    ProvocativeAura = 85,
    ReflexShotBonusDamage = 72,
    Regeneration = 73,
    Rune = 24,
    Ruthless = 46,
    Sadistic = 83,
    Sentinel = 45,
    ShadowAura = 25,
    SharpenBlade = 54,
    ShieldWall = 26,
    ShockingTouch = 67,
    Sleep = 27,
    Stamina = 28,
    StormShield = 68,
    Silenced = 43,
    SoulCollector = 56,
    Source = 59,
    TakenAim = 39,
    Taunted = 29,
    TemporaryDexterity = 30,
    TemporaryDraw = 31,
    TemporaryInitiative = 32,
    TemporaryPower = 33,
    TemporaryStamina = 34,
    Tenacious = 78,
    Thorns = 63,
    Torturer = 82,
    ToxicAura = 50, 
    TranquilHate = 64,
    Unbreakable = 71,
    Venomous = 35,
    Vulnerable = 36,
    Weakened = 37,
    WellOfSouls = 48,
    Wrath = 38,  

};
public enum CoreAttribute
{
    None = 0,
    Power = 1,
    Dexterity = 2,
    Stamina = 3,
    Draw = 4,
    Wits = 5,
    Strength = 6,
    Intelligence = 7,
    Constitution = 8,

}
public enum KeyWordType
{
    None = 0,
    Blessing = 5,
    Block = 9,
    Burn = 11,
    Energy = 10,
    Expend = 1,   
    Fleeting = 3,
    Innate = 2,
    Lethal = 14,
    LifeSteal = 12,
    Passive = 8,
    Unplayable = 4,
    SourceSpell = 13,
    Shank = 6,  
    WeaponRequirement = 7,       
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
    Trinket = 5,
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
public enum ActivationPhase
{
    NotActivated = 0,
    StartPhase = 1,
    ActivationPhase = 2,
    EndPhase =3,
}
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
    HealAllAllies = 14,
    Sleep = 10,
    PassTurn = 11,
    AddCardToTargetCardCollection = 12,
    SummonCreature = 13,
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
    AtLeastXAvailableNodes = 11,
    AtLeastOneAllyWounded = 12,
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
    SummonHelp = 13,
    Sleep = 12,

    

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
    PoisonGreen = 8,
    LightRed = 10,
    LightGreen = 12,
    RareTextBlue = 11,
   
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
    BodyParticles = 20,
    BodyLighting = 21,
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
    Resurrect = 6,
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
    Javelin = 10,

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
    RecruitCharacter = 4,
    KingsBlessingEvent = 5,
    CampSite = 6,
    Shop = 7,
    Mystery = 8,
    Treasure = 9,
    Shrine = 10,
}
public enum MapOrientation
{
    BottomToTop,
    TopToBottom,
    RightToLeft,
    LeftToRight
}
public enum NodeType
{
    MinorEnemy = 0,
    EliteEnemy = 1,
    CampSite = 2,
    Treasure = 3,
    Shop = 4,
    Boss = 5,
    Mystery = 6,
    Recruit = 7,
}
public enum NodeStates
{
    Locked,
    Visited,
    Attainable
}
#endregion

// State Related
#region
public enum StateName
{
    None = 0, 
    Adaptation = 1,
    Aggression = 2,
    Ambition = 23,
    ArmsMastery = 26,
    Benevolence = 3,
    ContractKillers = 4,
    Eagerness = 5,
    Endurance = 6,
    Godlike = 7,    
    GodsChosen = 9,
    HappyCampers = 10,
    Intimidation = 11,
    WrathOfTheKing = 22,
    PolishedArmour = 12,
    PowerOverwhelming = 21,
    Radiance = 24,
    Recycling = 13,
    Resourcefulness = 25,
    PumpedUp = 14,
    Sadism = 15,
    SavvyInvestors = 16,
    Heresy = 8,
    Survivalist = 17,
    ToughNutz = 18,
    Torturer = 27,
    Vindicated = 19,   
    WellLaidPlans = 20,
}
#endregion

