using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor.Common.CharacterScripts
{
	/// <summary>
	/// This script can change width and height of a character.
	/// </summary>
	public class CharacterBodySculptor : MonoBehaviour
	{
		public Slider WidthSlider;
		public Slider HeightSlider;

		public Transform Head;
		public Transform Torso;
		public Transform ArmL;
		public Transform[] ArmR;
		public Transform Pelvis;
		public Transform LegL;
		public Transform LegR;
		public Transform[] MeleeWeapon;
		public Transform Shield;
		public Transform[] Bow;
		public Transform Firearm;

		private Dictionary<Transform, Vector3> _positions = new Dictionary<Transform, Vector3>();

		public void Awake()
		{
			foreach (var part in new[] { Torso, Head, ArmL, ArmR[0], ArmR[1], Pelvis, LegL, LegR })
			{
				_positions.Add(part, part.transform.localPosition);
			}
		}

		/// <summary>
		/// Width slider handler.
		/// </summary>
		public void OnWidthChanged(float value)
		{
			var delta = value - 1;
			
			SetScale(Head, Mathf.Min(0, delta * 0.5f));
			SetScaleByX(Torso, delta);
			SetScale(ArmL, Mathf.Min(0, delta * 0.5f));
			SetScale(ArmR[0], Mathf.Min(0, delta * 0.5f));
			SetScale(ArmR[1], Mathf.Min(0, delta * 0.5f));
			SetScaleByX(Pelvis, delta);
			SetScaleByX(LegL, delta * 0.5f);
			SetScaleByX(LegR, delta * 0.5f);

			SetOffsetByX(ArmL, delta, 0.375f);
			SetOffsetByX(ArmR[0], delta,  -0.375f);
			SetOffsetByX(ArmR[1], delta, -0.375f);
			SetOffsetByX(LegL, delta, 0.25f);
			SetOffsetByX(LegR, delta, -0.25f);

			FixWeapons();
		}

		/// <summary>
		/// Height slider handler.
		/// </summary>
		public void OnHeightChanged(float value)
		{
			var delta = value - 1;

			SetScaleByY(Torso, delta * 0.5f);

			SetOffsetByY(Head, delta, 0.5f);
			SetOffsetByY(Torso, delta, 0.25f);
			SetOffsetByY(ArmL, delta, 0.375f);
			SetOffsetByY(ArmR[0], delta, 0.375f);
			SetOffsetByY(ArmR[1], delta, 0.375f);

			FixWeapons();
		}

		public void ResetWidth()
        {
            OnWidthChanged(1);

			if (WidthSlider != null)
            {
                WidthSlider.value = 1;
            }
		}

		public void ResetHeight()
		{
            OnHeightChanged(1);

			if (HeightSlider != null)
            {
                HeightSlider.value = 1;
            }
        }

		public void OnCharacterLoaded(Character character)
		{
			OnCharacterLoaded(character.BodyScale);
		}

		public void OnCharacterLoaded(Vector2 bodyScale)
		{
            if (WidthSlider != null && HeightSlider != null)
            {
                WidthSlider.value = bodyScale.x;
                HeightSlider.value = 1 + 2 * (bodyScale.y - 1);
			}
		}

		private static void SetScale(Transform target, float delta)
		{
			SetScaleByX(target, delta);
			SetScaleByY(target, delta);
		}

		private static void SetScaleByX(Transform target, float delta)
		{
			target.localScale = new Vector3(1 + delta, target.localScale.y, target.localScale.z);
		}

		private static void SetScaleByY(Transform target, float delta)
		{
			target.localScale = new Vector3(target.localScale.x, 1 + delta, target.localScale.z);
		}

		private void SetOffsetByX(Transform target, float delta, float vector)
		{
			target.localPosition = new Vector3(_positions[target].x + vector * delta, target.localPosition.y, target.localPosition.z);
		}

		private void SetOffsetByY(Transform target, float delta, float vector)
		{
			target.localPosition = new Vector3(target.localPosition.x, _positions[target].y + vector * delta, target.localPosition.z);
		}

		private void FixWeapons()
		{
			MeleeWeapon[0].localScale = new Vector3(1 / ArmL.transform.localScale.y, 1 / ArmL.transform.localScale.x, 1 / ArmL.transform.localScale.z);
			MeleeWeapon[1].localScale = new Vector3(1 / ArmR[0].transform.localScale.y, 1 / ArmR[0].transform.localScale.x, 1 / ArmR[0].transform.localScale.z);
			Shield.localScale = new Vector3(1 / ArmL.transform.localScale.x, 1 / ArmL.transform.localScale.y, 1 / ArmL.transform.localScale.z);
			Bow[0].localScale = new Vector3(1 / ArmL.transform.localScale.x, 1 / ArmL.transform.localScale.y, 1 / ArmL.transform.localScale.z);
			Bow[1].localScale = new Vector3(1 / ArmL.transform.localScale.x, 1 / ArmL.transform.localScale.y, 1 / ArmL.transform.localScale.z);
			Firearm.localScale = new Vector3(1 / ArmR[0].transform.localScale.x, 1 / ArmR[0].transform.localScale.y, 1 / ArmR[0].transform.localScale.z);
		}
	}
}