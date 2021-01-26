using System;
using System.Linq;
using Assets.HeroEditor.Common.CommonScripts.Springs;
using Assets.HeroEditor.Common.ExampleScripts;
using Assets.HeroEditor.Common.CharacterScripts.Firearms;
using Assets.HeroEditor.Common.CommonScripts;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using UnityEngine;

namespace Assets.HeroEditor.Common.CharacterScripts
{
    /// <summary>
    /// Character presentation in editor. Contains sprites, renderers, animation and so on.
    /// </summary>
    public partial class Character : CharacterBase
    {
        [Header("Weapons")]
        public MeleeWeapon MeleeWeapon;
        public Firearm Firearm;

		[Header("Service")]
		public LayerManager LayerManager;

        public Vector2 BodyScale
	    {
		    get { return BodyRenderers.Single(i => i.name == "Torso").transform.localScale; }
		    set => GetComponent<CharacterBodySculptor>().OnCharacterLoaded(value);
        }

	    /// <summary>
		/// Called automatically when something was changed.
		/// </summary>
		public void OnValidate()
        {
            if (Head == null) return;

            Initialize();
        }

        public void Start()
        {
            // We can use [StateHandler] attached to animation states to handle state transitions.
            foreach (var handler in Animator.GetBehaviours<StateHandler>().Where(i => i.Name.Contains("Death")))
            {
                handler.StateEnter.RemoveAllListeners();
                handler.StateEnter.AddListener(() => SetExpression("Dead"));

                handler.StateExit.RemoveAllListeners();
                handler.StateExit.AddListener(() => SetExpression("Default"));
            }
        }
        
        /// <summary>
        /// Called automatically when object was enabled.
        /// </summary>
        public void OnEnable()
        {
            HairMask.isCustomRangeActive = true;
            HairMask.frontSortingOrder = HelmetRenderer.sortingOrder;
            HairMask.backSortingOrder = HairRenderer.sortingOrder;
			UpdateAnimation();
        }

	    public void OnDisable()
	    {
		    _animationState = -1;
	    }

	    private int _animationState = -1;

		/// <summary>
        /// Initializes character renderers with selected sprites.
        /// </summary>
        public override void Initialize()
        {
            try // Disable try/catch for debugging.
            {
                TryInitialize();
            }
            catch (Exception e)
            {
                Debug.LogWarningFormat("Unable to initialize character {0}: {1}", name, e.Message);
            }
        }

		/// <summary>
		/// Initializes character renderers with selected sprites.
		/// </summary>
		private void TryInitialize()
		{
            if (Expressions.All(i => i.Name != "Default") || Expressions.All(i => i.Name != "Angry") || Expressions.All(i => i.Name != "Dead"))
            {
                throw new Exception("Character must have at least 3 basic expressions: Default, Angry and Dead.");
            }

			HeadRenderer.sprite = Head;
			HairRenderer.sprite = Hair;
			HairRenderer.maskInteraction = Helmet == null || Helmet.name.Contains("[FullHair]") ? SpriteMaskInteraction.None : SpriteMaskInteraction.VisibleInsideMask;
			EarsRenderer.sprite = Ears;
			SetExpression(Expression);
			BeardRenderer.sprite = Beard;
			MapSprites(BodyRenderers, Body);
			HelmetRenderer.sprite = Helmet;
			GlassesRenderer.sprite = Glasses;
			MaskRenderer.sprite = Mask;
			EarringsRenderer.sprite = Earrings;
			MapSprites(ArmorRenderers, Armor);
			CapeRenderer.sprite = Cape;
			BackRenderer.sprite = Back;
			PrimaryMeleeWeaponRenderer.sprite = PrimaryMeleeWeapon;
			SecondaryMeleeWeaponRenderer.sprite = SecondaryMeleeWeapon;
			MapSprites(BowRenderers, Bow);
			MapSprites(FirearmsRenderers, Firearms);
			ShieldRenderer.sprite = Shield;

			PrimaryMeleeWeaponRenderer.enabled = WeaponType != WeaponType.Bow;
			SecondaryMeleeWeaponRenderer.enabled = WeaponType == WeaponType.MeleePaired;
			BowRenderers.ForEach(i => i.enabled = WeaponType == WeaponType.Bow);
			ShieldRenderer.enabled = WeaponType == WeaponType.Melee1H;

			if (Hair != null && Hair.name.Contains("[HideEars]") && HairRenderer.maskInteraction == SpriteMaskInteraction.None)
			{
				EarsRenderer.sprite = null;
			}

			switch (WeaponType)
			{
                case WeaponType.Firearms1H:
                case WeaponType.Firearms2H:
                {
                    Firearm.AmmoShooted = 0;
                    BuildFirearms(Firearm.Params);
                    break;
                }
            }

			UpdateAnimation();
		}

        /// <summary>
        /// Refer to Animator window for animation params, states and transitions!
        /// </summary>
        public override void UpdateAnimation()
        {
            if (!Animator.isInitialized) return;

            var state = 100 * (int) WeaponType;

            Animator.SetInteger("WeaponType", (int) WeaponType);

            if ((WeaponType == WeaponType.Firearms1H || WeaponType == WeaponType.Firearms2H || WeaponType == WeaponType.FirearmsPaired) && Firearm.Params != null)
            {
				Animator.SetInteger("MagazineType", (int) Firearm.Params.MagazineType);
                Animator.SetInteger("HoldType", (int) Firearm.Params.HoldType);
                state += (int) Firearm.Params.HoldType;
            }

            if (state == _animationState) return; // No need to change animation.

            _animationState = state;
			
            if (WeaponType == WeaponType.Firearms1H || WeaponType == WeaponType.Firearms2H)
            {
                Animator.Play("IdleFirearm", 0); // Upper body
            }
            else
            {
                Animator.Play("IdleMelee", 0); // Upper body
            }

            Relax();
            SetState(CharacterState.Idle);
		}

		/// <summary>
		/// Alternative way to Hit character (with a script).
		/// </summary>
		public void Spring()
        {
            ScaleSpring.Begin(this, 1f, 1.1f, 40, 2);
        }
	}
}