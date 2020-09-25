using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Properties")]
    private AudioModel[] allAudioModels;

    [Header("Music")]
    [SerializeField] private AudioModel[] allMusic;

    [Header("Card SFX")]
    [SerializeField] private AudioModel[] allCardSFX;

    [Header("Ability SFX")]
    [SerializeField] private AudioModel[] allAbilitySFX;

    [Header("Character SFX")]
    [SerializeField] private AudioModel[] allCharacterSFX;

    [Header("Projectile SFX")]
    [SerializeField] private AudioModel[] allProjectileSFX;

    [Header("Explosion SFX")]
    [SerializeField] private AudioModel[] allExplosionSFX;

    [Header("Passive SFX")]
    [SerializeField] private AudioModel[] allPassiveSFX;

    [Header("GUI SFX")]
    [SerializeField] private AudioModel[] allGuiSFX;

    [Header("Events SFX")]
    [SerializeField] private AudioModel[] allEventsSFX;

    // Initialization 
    #region
    private void Start()
    {
        // Add all audio effects to persistency
        List<AudioModel> allAudioModelsList = new List<AudioModel>();

        // Add individual arrays
        allAudioModelsList.AddRange(allMusic);
        allAudioModelsList.AddRange(allCardSFX);
        allAudioModelsList.AddRange(allAbilitySFX);
        allAudioModelsList.AddRange(allCharacterSFX);
        allAudioModelsList.AddRange(allProjectileSFX);
        allAudioModelsList.AddRange(allExplosionSFX);
        allAudioModelsList.AddRange(allPassiveSFX);
        allAudioModelsList.AddRange(allGuiSFX);
        allAudioModelsList.AddRange(allEventsSFX);

        // Convert list to array
        allAudioModels = allAudioModelsList.ToArray();

        // Create an audio source for each audio model
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
    
    Ability_Gain_Block = 19,
    Ability_Damaged_Health_Lost = 12,
    Ability_Damaged_Block_Lost = 13,
    Ability_Bloody_Stab = 14,
    Ability_Oomph_Impact = 15,
    Ability_Sword_Ching = 16,
    Ability_Fire_Buff = 17,
    Ability_Holy_Buff = 18,
    Ability_Shadow_Buff = 20,

    Card_Draw = 0,
    Card_Moused_Over = 1,
    Card_Dragging = 2,
    Card_Breathe = 3,
    Card_Discarded = 4,

    Character_Footsteps = 9,
    Character_Draw_Bow = 10,

    Events_Turn_Change_Notification = 35,

    Explosion_Fire_1 = 21,
    Explosion_Shadow_1 = 22,
    Explosion_Lightning_1 = 22,
    Explosion_Poison_1 = 23,    

    GUI_Chime_1 = 31,
    GUI_Rolling_Bells = 32,
    GUI_Button_Mouse_Over = 33,
    GUI_Button_Clicked = 34,

    Passive_General_Buff = 5,
    Passive_General_Debuff = 6,
    Passive_Overload_Gained = 24,
    Passive_Burning_Gained = 25,

    Projectile_Arrow_Fired = 26,
    Projectile_Fireball_Fired = 27,
    Projectile_Lightning_Fired = 28,
    Projectile_Poison_Fired = 29,
    Projectile_Shadowball_Fired = 30,

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
