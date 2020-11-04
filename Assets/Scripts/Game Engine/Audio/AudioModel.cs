using UnityEngine.Audio;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class AudioModel 
{

    [HorizontalGroup("General Properties", 75)]
    [HideLabel]
    [PreviewField(75)]
    public AudioClip audioClip;

    [VerticalGroup("General Properties/Stats")]
    [LabelWidth(100)]
    public Sound soundType;

    [VerticalGroup("General Properties/Stats")]
    [LabelWidth(100)]
    public CombatMusicCategory combatCategory;

    [VerticalGroup("General Properties/Stats")]
    [LabelWidth(100)]
    public bool randomizeVolume;

    [VerticalGroup("General Properties/Stats")]
    [LabelWidth(100)]
    [Range(0f, 1f)]
    [ShowIf("ShowRandomVolumeSettings")]
    public float randomVolumeLowerLimit = 0.3f;

    [VerticalGroup("General Properties/Stats")]
    [LabelWidth(100)]
    [Range(0f, 1f)]
    [ShowIf("ShowRandomVolumeSettings")]
    public float randomVolumeUpperLimit = 0.5f;

    [VerticalGroup("General Properties/Stats")]
    [LabelWidth(100)]
    [Range(0f, 1f)]
    [ShowIf("ShowVolume")]
    public float volume = 0.5f;

    [VerticalGroup("General Properties/Stats")]
    [LabelWidth(100)]
    public bool randomizePitch;

    [VerticalGroup("General Properties/Stats")]
    [LabelWidth(100)]
    [Range(0.1f, 3f)]
    [ShowIf("ShowRandomPitchSettings")]
    public float randomPitchLowerLimit = 0.8f;

    [VerticalGroup("General Properties/Stats")]
    [LabelWidth(100)]
    [Range(0.1f, 3f)]
    [ShowIf("ShowRandomPitchSettings")]
    public float randomPitchUpperLimit = 1.2f;

    [VerticalGroup("General Properties/Stats")]
    [LabelWidth(100)]
    [Range(0.1f, 3f)]
    [ShowIf("ShowPitch")]
    public float pitch = 1f;

    [VerticalGroup("General Properties/Stats")]
    [LabelWidth(100)]
    public bool loop;

    [HideInInspector]
    public AudioSource source;

    // Misc
    [HideInInspector] public bool fadingIn;
    [HideInInspector] public bool fadingOut;


    public bool ShowVolume()
    {
        return randomizeVolume == false;
    }
    public bool ShowRandomVolumeSettings()
    {
        return randomizeVolume == true;
    }

    public bool ShowPitch()
    {
        return randomizePitch == false;
    }
    public bool ShowRandomPitchSettings()
    {
        return randomizePitch == true;
    }



}
public enum CombatMusicCategory
{
    None = 0,
    Basic =1,
    Elite =2,
    Boss = 3
}
