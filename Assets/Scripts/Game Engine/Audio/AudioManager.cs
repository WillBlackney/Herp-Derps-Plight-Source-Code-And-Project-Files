using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : Singleton<AudioManager>
{
    public AudioModel[] allAudioModels;

    private void Start()
    {

        foreach(AudioModel a in allAudioModels)
        {
            a.source = gameObject.AddComponent<AudioSource>();
            a.source.clip = a.audioClip;
            a.source.volume = a.volume;
            a.source.pitch = a.pitch;
            a.source.loop = a.loop;
        }

        PlaySound("Battle Theme Music");
    }

    public void PlaySound(string name)
    {
        AudioModel a = Array.Find(allAudioModels, sound => sound.name == name);
        if(a != null)
        {
            a.source.Play();
        }
        else
        {
            Debug.LogWarning("AudioManager.PlaySound() did not find an audio model with the name " + name);
        }
       
    }
}
