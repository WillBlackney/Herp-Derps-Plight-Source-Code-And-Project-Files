using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.ExampleScripts;
using Assets.HeroEditor.Common.CharacterScripts.Firearms.Enums;
using UnityEngine;

namespace Assets.HeroEditor.Common.Data
{
    /// <summary>
    /// Represents firearm parameters.
    /// </summary>
    [Serializable]
    public class FirearmParams
    {
        public string Name;
	    public Texture FirearmTexture;
        public FirearmType Type;
        public HoldType HoldType;
        public MagazineType MagazineType;
	    public FirearmLoadType LoadType;
		public bool AutomaticFire;
        public bool AutomaticLoad;
        public int FireRateInMinute;
        public int MagazineCapacity;

        /// <summary>
        /// Arm recoil (offset in local space)
        /// </summary>
        [Range(0, 0.25f)] public float Recoil = 0.05f;

        /// <summary>
        /// 0 = max spreading angle (45 degree), 1 = 100% accuracy (zero spreading).
        /// </summary>
        [Range(0, 1)] public float Accuracy = 0.95f;

        /// <summary>
        /// Muzzle velocity in m/s.
        /// </summary>
        [Range(0, 5000)]
        public float MuzzleVelocity = 1500f;

        [Header("Positions")]
        public Vector2 SlidePosition;
        public Vector2 MagazinePosition;
        public Vector2 FireMuzzlePosition;

        [Header("Components")]
        public ParticleSystem FireMuzzlePrefab;
        public Projectile ProjectilePrefab;

        [Header("Sounds")]
        public AudioClip SoundFire;
        public AudioClip SoundClipIn;
        public AudioClip SoundClipOut;
        public AudioClip SoundLoad;
        public AudioClip SoundPump;
        
        [Header("Animation")]
        public AnimationClip HoldAnimation;
        public AnimationClip LoadAnimation;
        public AnimationClip ReloadAnimation;

	    /// <summary>
	    /// Store specific weapon params here.
	    /// </summary>
	    [Header("Meta")]
	    public List<string> Meta;

		/// <summary>
		/// Parse meta to dictionary
		/// </summary>
		public Dictionary<string, string> MetaAsDictionary
		{
			get { return Meta.Select(i => i.Split('=')).ToDictionary(i => i[0], i => i[1]); }
		}

		/// <summary>
		/// Parse color from meta by key
		/// </summary>
	    public Color GetColorFromMeta(string key)
	    {
		    Color color;

		    ColorUtility.TryParseHtmlString(Meta.First(i => i.Contains(key)).Split('=')[1], out color);

		    return color;
	    }
    }
}