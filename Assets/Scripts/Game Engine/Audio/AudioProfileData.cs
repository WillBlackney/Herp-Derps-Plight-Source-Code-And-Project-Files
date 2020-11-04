using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AudioProfileData 
{
    public AudioProfileType audioProfileType;

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