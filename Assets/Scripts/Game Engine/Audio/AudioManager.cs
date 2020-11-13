using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Components + Pooling")]
    [SerializeField] private GameObject audioPlayerPrefab;
    [SerializeField] private Transform audioPlayerPoolParent;
    [SerializeField] private List<AudioPlayer> audioPlayerPool;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Properties")]
    private AudioModel[] allAudioModels;
    private AudioModel previousCombatTrack = null;

    [Header("Audio Profiles")]
    [SerializeField] private AudioProfileData[] allProfiles;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

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

    [Header("Environments SFX")]
    [SerializeField] private AudioModel[] allEnvironmentsSFX;

    [Header("Ambience SFX")]
    [SerializeField] private AudioModel[] allAmbienceSFX;

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
        allAudioModelsList.AddRange(allEnvironmentsSFX);
        allAudioModelsList.AddRange(allAmbienceSFX);

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
    }
    #endregion

    // Trigger Sounds
    #region
    public AudioModel GetAudioModel(Sound s)
    {
        AudioModel a = Array.Find(allAudioModels, sound => sound.soundType == s);
        return a;
    }
    public bool IsSoundPlaying(Sound s)
    {
        bool bReturned = false;

        AudioModel a = Array.Find(allAudioModels, sound => sound.soundType == s);

        if(a == null)
        {
            Debug.LogWarning("IsSoundPlaying() couln't find the sound...");
        }

        if(a != null && a.source.isPlaying)
        {
            bReturned = true;
        }

        return bReturned;
    }
    public void PlaySound(Sound s)
    {
        if(s == Sound.None)
        {
            return;
        }

        AudioModel a = Array.Find(allAudioModels, sound => sound.soundType == s);
        if (a != null)
        {
            a.fadingIn = false;
            a.fadingOut = false;

            // Randomize pitch if marked to do so
            if (a.randomizePitch)
            {
                a.source.pitch = RandomGenerator.NumberBetween(a.randomPitchLowerLimit, a.randomPitchUpperLimit);
            }

            // Randomize volume if marked to do so
            if (a.randomizeVolume)
            {
                a.source.volume = RandomGenerator.NumberBetween(a.randomVolumeLowerLimit, a.randomVolumeUpperLimit);
            }

            a.source.Play();
        }
    }
    public void PlaySound(AudioProfileType type, AudioSet set)
    {
        if (GlobalSettings.Instance.preventAudioProfiles)
        {
            return;
        }

        // Find the matching profile
        AudioProfileData apd = Array.Find(allProfiles, a => a.audioProfileType == type);

        if(apd == null)
        {
            Debug.LogWarning("PlaySound() could not find an AudioProfileData that matches the type '" + type.ToString() + "'.");
        }

        // Get the correct sounds
        List<AudioModel> validSounds = new List<AudioModel>();
        if(set == AudioSet.Die)
        {
            validSounds.AddRange(apd.dieSounds);
        }
        else if (set == AudioSet.Hurt)
        {
            validSounds.AddRange(apd.hurtSounds);
        }
        else if (set == AudioSet.MeleeAttack)
        {
            validSounds.AddRange(apd.meleeAttackSounds);
        }
        else if (set == AudioSet.Buff)
        {
            validSounds.AddRange(apd.buffSounds);
        }

        if(validSounds.Count == 0)
        {
            Debug.LogWarning("PlaySound() did not find any valid sounds within the set '" + set.ToString() + "', returning...");
            return;
        }

        // Randomly pick a sound
        AudioModel soundPlayed = validSounds[RandomGenerator.NumberBetween(0, validSounds.Count - 1)];

        // Set up audio player
        AudioPlayer player = GetNextAvailableAudioPlayer();
        BuildAudioPlayerFromAudioModelData(soundPlayed, player);

        // Play the sound!
        player.source.Play();        
    }   
    private void BuildAudioPlayerFromAudioModelData(AudioModel data, AudioPlayer player)
    {
        player.source.clip = data.audioClip;

        // Randomize pitch if marked to do so
        if (data.randomizePitch)
        {
            player.source.pitch = RandomGenerator.NumberBetween(data.randomPitchLowerLimit, data.randomPitchUpperLimit);
        }

        // Randomize volume if marked to do so
        if (data.randomizeVolume)
        {
            player.source.volume = RandomGenerator.NumberBetween(data.randomVolumeLowerLimit, data.randomVolumeUpperLimit);
        }
    }
    public void StopSound(Sound s)
    {
        AudioModel a = Array.Find(allAudioModels, sound => sound.soundType == s);
        if (a != null)
        {
            a.fadingIn = false;
            a.fadingOut = false;
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
    public void DisableAllAmbience()
    {
        StopSound(Sound.Ambience_Outdoor_Spooky);
        StopSound(Sound.Ambience_Crypt);
    }
    public void FadeOutAllAmbience(float fadeDuration)
    {
        if (IsSoundPlaying(Sound.Ambience_Outdoor_Spooky))
        {
            FadeOutSound(Sound.Ambience_Outdoor_Spooky, fadeDuration);
        }
        if (IsSoundPlaying(Sound.Ambience_Crypt))
        {
            FadeOutSound(Sound.Ambience_Crypt, fadeDuration);
        }
        if (IsSoundPlaying(Sound.Environment_Camp_Fire))
        {
            FadeOutSound(Sound.Environment_Camp_Fire, fadeDuration);
        }
    }
    public void FadeOutAllCombatMusic(float fadeDuration)
    {
        Debug.LogWarning("FadeOutAllCombatMusic()");
        if (IsSoundPlaying(Sound.Music_Basic_1))
        {
            FadeOutSound(Sound.Music_Basic_1, fadeDuration);
        }
        if (IsSoundPlaying(Sound.Music_Basic_2))
        {
            FadeOutSound(Sound.Music_Basic_2, fadeDuration);
        }
        if (IsSoundPlaying(Sound.Music_Basic_3))
        {
            FadeOutSound(Sound.Music_Basic_3, fadeDuration);
        }
        if (IsSoundPlaying(Sound.Music_Elite_1))
        {
            FadeOutSound(Sound.Music_Elite_1, fadeDuration);
        }
        if (IsSoundPlaying(Sound.Music_Elite_2))
        {
            FadeOutSound(Sound.Music_Elite_2, fadeDuration);
        }
        if (IsSoundPlaying(Sound.Music_Boss_1))
        {
            FadeOutSound(Sound.Music_Boss_1, fadeDuration);
        }
        if (IsSoundPlaying(Sound.Music_Boss_2))
        {
            FadeOutSound(Sound.Music_Boss_2, fadeDuration);
        }
    }
    public void ForceStopAllCombatMusic()
    {
        Debug.LogWarning("ForceStopAllCombatMusic()");
        StopSound(Sound.Music_Basic_1);
        StopSound(Sound.Music_Basic_2);
        StopSound(Sound.Music_Basic_3);
        StopSound(Sound.Music_Elite_1);
        StopSound(Sound.Music_Basic_2);
        StopSound(Sound.Music_Basic_3);
    }
    public void AutoPlayBasicCombatMusic(float fadeDuration)
    {
        Debug.LogWarning("AutoPlayBasicCombatMusic");

        // Find all basic combat music
        AudioModel[] basicCombatMusic = Array.FindAll(allAudioModels, sound => sound.combatCategory == CombatMusicCategory.Basic && sound != previousCombatTrack);

        // Choose one track randomly
        AudioModel musicSelected = basicCombatMusic[RandomGenerator.NumberBetween(0, basicCombatMusic.Length - 1)];

        // Start music fade in
        musicSelected.source.FadeIn(fadeDuration, musicSelected);

        // Cache track so it cant be played twice in a row
        previousCombatTrack = musicSelected;
    }
    public void AutoPlayEliteCombatMusic(float fadeDuration)
    {
        Debug.LogWarning("AutoPlayEliteCombatMusic");

        // Find all basic combat music
        AudioModel[] eliteCombatMusic = Array.FindAll(allAudioModels, sound => sound.combatCategory == CombatMusicCategory.Elite && sound != previousCombatTrack);

        // Choose one track randomly
        AudioModel musicSelected = eliteCombatMusic[RandomGenerator.NumberBetween(0, eliteCombatMusic.Length - 1)];

        // Start music fade in
        musicSelected.source.FadeIn(fadeDuration, musicSelected);

        // Cache track so it cant be played twice in a row
        previousCombatTrack = musicSelected;
    }
    #endregion

    // Audio Player + Pooling Logic
    #region
    private AudioPlayer GetNextAvailableAudioPlayer()
    {
        AudioPlayer availablePlayer = null;

        // Find an available player
        foreach (AudioPlayer ap in audioPlayerPool)
        {
            if (ap.source.isPlaying == false)
            {
                Debug.LogWarning("GetNextAvailableAudioPlayer() found an available player");
                availablePlayer = ap;
                break;
            }
        }

        // If there arent any available, create new one, add it to pool, then use it
        if (availablePlayer == null)
        {
            Debug.LogWarning("GetNextAvailableAudioPlayer() couldn't find an available player, creating a new one");
            availablePlayer = CreateAndAddAudioPlayerToPool();
        }

        return availablePlayer;
    }
    
    private AudioPlayer CreateAndAddAudioPlayerToPool()
    {
        Debug.LogWarning("CreateAndAddAudioPlayerToPool() called, creating a new audio player and adding it to the pool");
        AudioPlayer newAP = Instantiate(audioPlayerPrefab, audioPlayerPoolParent).GetComponent<AudioPlayer>();
        audioPlayerPool.Add(newAP);
        Debug.LogWarning("AudioManager total player pool count = " + audioPlayerPool.Count.ToString());
        return newAP;
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
    Ability_Metallic_Ting = 37,

    Ambience_Outdoor_Spooky = 39,
    Ambience_Crypt = 40,

    Card_Draw = 0,
    Card_Moused_Over = 1,
    Card_Dragging = 2,
    Card_Breathe = 3,
    Card_Discarded = 4,

    Character_Footsteps = 9,
    Character_Draw_Bow = 10,

    Environment_Gate_Open = 38,
    Environment_Camp_Fire = 49,

    Events_New_Game_Started = 35,
    Events_New_Turn_Notification = 41,

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

    Music_Basic_1 = 7,
    Music_Basic_2 = 42,
    Music_Basic_3 = 43,
    Music_Elite_1 = 44,
    Music_Elite_2 = 45,
    Music_Boss_1 = 47,
    Music_Boss_2 = 48,
    Music_Main_Menu_Theme_1 = 36,



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
