using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Properties")]
    private AudioModel[] allAudioModels;

    [Header("Music")]
    public AudioModel battleTheme1;

    [Header("Card SFX")]
    [SerializeField] private AudioModel[] allCardSFX;
    public AudioModel cardDraw;
    public AudioModel cardMousedOver;
    public AudioModel cardDiscarded;
    public AudioModel cardDragging;
    public AudioModel cardBreathe;

    // Initialization 
    #region
    private void Start()
    {
        // Add all audio effects to persistency
        List<AudioModel> allAudioModelsList = new List<AudioModel>();
        // Music
        allAudioModelsList.Add(battleTheme1);

        // Card SFX
        allAudioModelsList.Add(cardDraw);
        allAudioModelsList.Add(cardMousedOver);
        allAudioModelsList.Add(cardDiscarded);
        allAudioModelsList.Add(cardDragging);
        allAudioModelsList.Add(cardBreathe);

        // Convert list to array
        allAudioModels = allAudioModelsList.ToArray();

        // create an audio source for each audio model
        foreach (AudioModel a in allAudioModels)
        {
            a.source = gameObject.AddComponent<AudioSource>();
            a.source.clip = a.audioClip;
            a.source.volume = a.volume;
            a.source.pitch = a.pitch;
            a.source.loop = a.loop;
        }

        PlaySound(Sound.Music_Battle_Theme_1);
    }
    #endregion

    // Trigger Sounds
    #region
    public void PlaySound(Sound s)
    {
        AudioModel a = Array.Find(allAudioModels, sound => sound.soundType == s);
        if (a != null)
        {
            a.source.Play();
        }
        else
        {
            Debug.LogWarning("AudioManager.PlaySound() did not find an audio model with the name " + name);
        }
    }
    public void StopSound(Sound s)
    {
        AudioModel a = Array.Find(allAudioModels, sound => sound.soundType == s);
        if (a != null)
        {
            a.source.Stop();
        }
        else
        {
            Debug.LogWarning("AudioManager.StopSound() did not find an audio model with the name " + name);
        }
    }
    public void FadeOutSound(Sound s, float duration)
    {
        AudioModel a = Array.Find(allAudioModels, sound => sound.soundType == s);
        if (a != null)
        {
            a.source.FadeOut(duration, a);
        }
        else
        {
            Debug.LogWarning("AudioManager.FadeOutSound() did not find an audio model with the name " + name);
        }
    }
    public void FadeInSound(Sound s, float duration)
    {
        AudioModel a = Array.Find(allAudioModels, sound => sound.soundType == s);
        if (a != null)
        {
            a.source.FadeIn(duration, a);
        }
        else
        {
            Debug.LogWarning("AudioManager.FadeInSound() did not find an audio model with the name " + name);
        }
    }
    #endregion

}

public enum Sound
{
    None = 8,
    Card_draw = 0,
    Card_moused_over = 1,
    Card_dragging = 2,
    Card_breathe = 3,
    Card_discarded = 4,

    Ability_general_buff = 5,
    Ability_general_debuff = 6,

    Music_Battle_Theme_1 = 7,
}

// AUDIO SOURCE EXTENSIONS!!
namespace UnityEngine
{
    public static class AudioSourceExtensions
    {
        public static void FadeOut(this AudioSource a, float duration, AudioModel data)
        {
            a.GetComponent<MonoBehaviour>().StartCoroutine(FadeOutCore(a, duration, data));
        }

        private static IEnumerator FadeOutCore(AudioSource a, float duration, AudioModel data)
        {
            data.fadingIn = false;
            data.fadingOut = true;
            float startVolume = data.volume;

            while (a.volume > 0 && data.fadingOut)
            {
                a.volume -= startVolume * Time.deltaTime / duration;
                if(a.volume <= 0)
                {
                    a.Stop();
                    a.volume = startVolume;
                    data.fadingOut = false;
                }
                yield return new WaitForEndOfFrame();
            }

            
        }

        public static void FadeIn(this AudioSource a, float duration, AudioModel data)
        {
            a.GetComponent<MonoBehaviour>().StartCoroutine(FadeInCore(a, duration, data));
        }

        private static IEnumerator FadeInCore(AudioSource a, float duration, AudioModel data)
        {
            data.fadingOut = false;
            data.fadingIn = true;

            float endVolume = data.volume;
            a.volume = 0f;
            a.Play();

            while (a.volume < endVolume && data.fadingIn)
            {
                a.volume += endVolume * Time.deltaTime / duration;

                if (a.volume >= endVolume)
                {
                    a.volume = endVolume;
                    data.fadingIn = false;
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }
}
