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
    [Range(0f, 1f)]
    public float volume = 0.5f;

    [VerticalGroup("General Properties/Stats")]
    [LabelWidth(100)]
    [Range(0.1f, 3f)]
    public float pitch = 1f;

    [VerticalGroup("General Properties/Stats")]
    [LabelWidth(100)]
    public bool loop;

    [HideInInspector]
    public AudioSource source;

    // Misc
    [HideInInspector] public bool fadingIn;
    [HideInInspector] public bool fadingOut;



}
public enum CombatMusicCategory
{
    None = 0,
    Basic =1,
    Elite =2,
    Boss = 3
}
