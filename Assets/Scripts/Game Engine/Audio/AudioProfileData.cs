using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class AudioProfileData 
{
    [Header("General Properties")]
    public AudioProfileType audioProfileType;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("SFX Buckets")]
    public AudioModel[] meleeAttackSounds;
    public AudioModel[] hurtSounds;
    public AudioModel[] dieSounds;
    public AudioModel[] buffSounds;

}

public enum AudioProfileType
{
    None = 0,
    HumanMale = 1,
    HumanFemale = 2,
    Orc = 3,
    Undead = 4,
    Goblin = 5,
    Gnoll = 6,
    Satyr = 7,
}

public enum AudioSet
{
    None = 0,
    MeleeAttack = 1,
    Hurt = 2,
    Die = 3,
    Buff = 4,
}