using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.Data;
using Assets.HeroEditor.Common.CharacterScripts.Firearms.Enums;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using UnityEngine;

namespace Assets.HeroEditor.Common.CharacterScripts
{
    public partial class Character
    {
        /// <summary>
        /// Set character's body parts.
        /// </summary>
		public override void SetBody(SpriteGroupEntry item, BodyPart part, Color? color)
        {
            switch (part)
            {
                case BodyPart.Body:
                    Body = item?.Sprites;
					break;
                case BodyPart.Head:
                    Head = item?.Sprite;
					break;
                case BodyPart.Hair:
                    Hair = item?.Sprite;
                    HairRenderer.color = color ?? HairRenderer.color;
                    if (item != null && item.Tags.Contains("NoPaint")) HairRenderer.color = Color.white;
                    break;
                case BodyPart.Ears:
                    Ears = item?.Sprite;
                    break;
                case BodyPart.Eyebrows:
                    Expressions[0].Eyebrows = item?.Sprite;
                    break;
                case BodyPart.Eyes:
                    Expressions[0].Eyes = item?.Sprite;
                    EyesRenderer.color = color ?? EyesRenderer.color;
                    break;
                case BodyPart.Mouth:
                    Expressions[0].Mouth = item?.Sprite;
                    break;
                case BodyPart.Beard:
                    Beard = item?.Sprite;
                    break;
                default: throw new NotImplementedException($"Unsupported part: {part}.");
            }

            Initialize();
		}

        public override void SetBody(SpriteGroupEntry item, BodyPart part)
        {
            SetBody(item, part, null);
        }

        /// <summary>
        /// Set character's expression.
        /// </summary>
        public override void SetExpression(string expression)
        {
            if (Expressions.Count < 3) throw new Exception("Character must have at least 3 basic expressions: Default, Angry and Dead.");

            var e = Expressions.Single(i => i.Name == expression);

            Expression = expression;
            EyebrowsRenderer.sprite = e.Eyebrows;
            EyesRenderer.sprite = e.Eyes;
            MouthRenderer.sprite = e.Mouth;

            if (EyebrowsRenderer.sprite == null) EyebrowsRenderer.sprite = Expressions[0].Eyebrows;
            if (EyesRenderer.sprite == null) EyesRenderer.sprite = Expressions[0].Eyes;
            if (MouthRenderer.sprite == null) MouthRenderer.sprite = Expressions[0].Mouth;
        }

		/// <summary>
		/// Equip something from SpriteCollection.
		/// </summary>
		public override void Equip(SpriteGroupEntry item, EquipmentPart part, Color? color)
        {
            switch (part)
            {
                case EquipmentPart.MeleeWeapon1H:
                case EquipmentPart.MeleeWeapon2H:
                case EquipmentPart.MeleeWeaponPaired:
                case EquipmentPart.Bow:
                    Firearms = null;
                    FirearmsRenderers.ForEach(i => i.sprite = null);
                    break;
                case EquipmentPart.Firearm1H:
                case EquipmentPart.Firearm2H:
                    PrimaryMeleeWeapon = SecondaryMeleeWeapon = null;
                    PrimaryMeleeWeaponRenderer.sprite = SecondaryMeleeWeaponRenderer.sprite = null;
                    break;
            }

            switch (part)
            {
                case EquipmentPart.Helmet:
                    Helmet = item?.Sprite;
                    HelmetRenderer.color = color ?? HelmetRenderer.color;
                    break;
                case EquipmentPart.Armor:
                    Armor = item?.Sprites.ToList();
                    ArmorRenderers.ForEach(i => i.color = color ?? i.color);
                    break;
                case EquipmentPart.Pauldrons:
                case EquipmentPart.Vest:
                case EquipmentPart.Gloves:
                case EquipmentPart.Belt:
                case EquipmentPart.Boots:
                    foreach (var p in GetEquipmentSubPartNames(part))
                    {
                        SetArmorParts(p, item?.Sprites);
                    }
                    break;
                case EquipmentPart.MeleeWeapon1H:
                    PrimaryMeleeWeapon = item?.Sprite;
                    PrimaryMeleeWeaponRenderer.color = color ?? PrimaryMeleeWeaponRenderer.color;
                    if (WeaponType != WeaponType.MeleePaired) WeaponType = WeaponType.Melee1H;
					break;
                case EquipmentPart.MeleeWeapon2H:
                    PrimaryMeleeWeapon = item?.Sprite;
                    PrimaryMeleeWeaponRenderer.color = color ?? PrimaryMeleeWeaponRenderer.color;
                    WeaponType = WeaponType.Melee2H;
					break;
                case EquipmentPart.MeleeWeaponPaired:
                    if (WeaponType == WeaponType.Melee2H) PrimaryMeleeWeapon = null;
                    SecondaryMeleeWeapon = item?.Sprite;
                    WeaponType = WeaponType.MeleePaired;
					break;
                case EquipmentPart.Bow:
                    Bow = item?.Sprites.ToList();
                    WeaponType = WeaponType.Bow;
					break;
                case EquipmentPart.Firearm1H:
                    Firearms = item?.Sprites.ToList();
                    WeaponType = WeaponType.Firearms1H;
					break;
                case EquipmentPart.Firearm2H:
                    Firearms = item?.Sprites.ToList();
                    WeaponType = WeaponType.Firearms2H;
					break;
                case EquipmentPart.Shield:
                    Shield = item?.Sprite;
                    WeaponType = WeaponType.Melee1H;
					break;
                case EquipmentPart.Cape:
                    Cape = item?.Sprite;
                    break;
                case EquipmentPart.Back:
                    Back = item?.Sprite;
                    WeaponType = WeaponType.Melee1H;
					break;
                case EquipmentPart.Earrings:
                    Earrings = item?.Sprite;
                    break;
                case EquipmentPart.Glasses:
                    Glasses = item?.Sprite;
                    break;
                case EquipmentPart.Mask:
                    Mask = item?.Sprite;
                    break;
                default: throw new NotImplementedException($"Unsupported part: {part}.");
            }

            Initialize();
		}

        public override void Equip(SpriteGroupEntry item, EquipmentPart part)
        {
            Equip(item, part, null);
        }

        /// <summary>
        /// Remove equipment partially.
        /// </summary>
        public override void UnEquip(EquipmentPart part)
        {
			Equip(null, part);
        }

        /// <summary>
        /// Remove all equipment.
        /// </summary>
        public override void ResetEquipment()
        {
            Armor = null;
            Bow = null;
            Firearms = null;
            Helmet = Cape = Back = PrimaryMeleeWeapon = SecondaryMeleeWeapon = Shield = Mask = Glasses = Earrings = null;
            WeaponType = WeaponType.Melee1H;
            Initialize();
        }

		private void SetArmorParts(string part, List<Sprite> armor)
	    {
		    var sprite = armor?.SingleOrDefault(j => j.name == part);

            Armor?.RemoveAll(i => i == null || i.name == part);
            
            if (sprite != null)
            {
                if (Armor == null) Armor = new List<Sprite>();

                Armor.Add(sprite);
            }
	    }

		private void BuildFirearms(FirearmParams firearmParams)
        {
            if (firearmParams == null) return;

            Firearm.Params = firearmParams; // TODO:
		    Firearm.SlideTransform.localPosition = firearmParams.SlidePosition;
		    Firearm.MagazineTransform.localPosition = firearmParams.MagazinePosition;
		    Firearm.FireTransform.localPosition = firearmParams.FireMuzzlePosition;
		    Firearm.AmmoShooted = 0;

		    if (Firearm.Params.LoadType == FirearmLoadType.Lamp)
		    {
			    Firearm.Fire.SetLamp(firearmParams.GetColorFromMeta("LampReady"));
		    }
	    }

		private static void MapSprites(List<SpriteRenderer> spriteRenderers, List<Sprite> sprites)
        {
			foreach (var part in spriteRenderers)
            {
                part.sprite = sprites?.SingleOrDefault(i => i != null && i.name == part.name.Split('[')[0]);
            }
        }
    }
}