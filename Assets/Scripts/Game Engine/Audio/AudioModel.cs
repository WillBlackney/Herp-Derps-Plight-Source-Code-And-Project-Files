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
    public string name;

    [VerticalGroup("General Properties/Stats")]
    [LabelWidth(100)]
    [Range(0f, 1f)]
    public float volume;

    [VerticalGroup("General Properties/Stats")]
    [LabelWidth(100)]
    [Range(0.1f, 3f)]
    public float pitch;

    [VerticalGroup("General Properties/Stats")]
    [LabelWidth(100)]
    public bool loop;

    [HideInInspector]
    public AudioSource source;



}
