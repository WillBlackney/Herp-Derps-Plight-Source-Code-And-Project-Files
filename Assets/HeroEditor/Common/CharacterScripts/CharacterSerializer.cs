using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Assets.HeroEditor.Common.Data;
using Assets.HeroEditor.Common.CharacterScripts.Firearms;
using HeroEditor.Common;
using HeroEditor.Common.Data;
using HeroEditor.Common.Enums;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.HeroEditor.Common.CharacterScripts
{
	public partial class Character
	{
		public override string ToJson()
		{
			if (SpriteCollection == null) throw new Exception("SpriteCollection is not set!");

            var description = new Dictionary<string, string>
			{
				{ "Head", SpriteToString(SpriteCollection.Head, HeadRenderer) },
				{ "Body", SpriteToString(SpriteCollection.Body, BodyRenderers[0]) },
				{ "Ears", SpriteToString(SpriteCollection.Ears, EarsRenderer) },
				{ "Hair", SpriteToString(SpriteCollection.Hair, HairRenderer) },
				{ "Beard", SpriteToString(SpriteCollection.Beard, BeardRenderer) },
				{ "Helmet", SpriteToString(SpriteCollection.Helmet, HelmetRenderer) },
				{ "Glasses", SpriteToString(SpriteCollection.Glasses, GlassesRenderer) },
				{ "Mask", SpriteToString(SpriteCollection.Mask, MaskRenderer) },
				{ "Cape", SpriteToString(SpriteCollection.Cape, CapeRenderer) },
				{ "Back", SpriteToString(SpriteCollection.Back, BackRenderer) },
				{ "Shield", SpriteToString(SpriteCollection.Shield, ShieldRenderer) },
				{ "WeaponType", WeaponType.ToString() },
				{ "Expression", Expression },
				{ "BodyScaleX", BodyScale.x.ToString(CultureInfo.InvariantCulture) },
                { "BodyScaleY", BodyScale.y.ToString(CultureInfo.InvariantCulture) }
			};

            switch (WeaponType)
            {
                case WeaponType.Melee1H:
                case WeaponType.Melee2H:
                case WeaponType.MeleePaired:
					description.Add("PrimaryMeleeWeapon", SpriteToString(GetWeaponCollection(WeaponType), PrimaryMeleeWeaponRenderer));
                    description.Add("SecondaryMeleeWeapon", SpriteToString(GetWeaponCollection(WeaponType), SecondaryMeleeWeaponRenderer));
					break;
				case WeaponType.Bow:
                    description.Add("Bow", SpriteToString(SpriteCollection.Bow, BowRenderers[0]));
					break;
                case WeaponType.Firearms1H:
                case WeaponType.Firearms2H:
					description.Add("Firearms", SpriteToString(GetWeaponCollection(WeaponType), FirearmsRenderers[0]));
                    description.Add("FirearmParams", Firearm.Params.Name);
					break;
			}

			for (var i = 0; i < ArmorRenderers.Count; i++)
            {
				description.Add($"Armor[{i}]", SpriteToString(SpriteCollection.Armor, ArmorRenderers[i]));
			}

			foreach (var expression in Expressions)
			{
				description.Add($"Expression.{expression.Name}.Eyebrows", SpriteToString(SpriteCollection.Eyebrows, expression.Eyebrows, EyebrowsRenderer.color));
				description.Add($"Expression.{expression.Name}.Eyes", SpriteToString(SpriteCollection.Eyes, expression.Eyes, EyesRenderer.color));
				description.Add($"Expression.{expression.Name}.Mouth", SpriteToString(SpriteCollection.Mouth, expression.Mouth, MouthRenderer.color));
			}

			return JsonConvert.SerializeObject(description);
		}

		public override void FromJson(string serialized)
		{
            if (SpriteCollection == null) throw new Exception("SpriteCollection is not set!");

			var description = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialized);

			RestoreFromString(ref Head, HeadRenderer, SpriteCollection.Head, description["Head"]);
			RestoreFromString(ref Body, BodyRenderers, SpriteCollection.Body, description["Body"]);
			RestoreFromString(ref Ears, EarsRenderer, SpriteCollection.Ears, description["Ears"]);
			RestoreFromString(ref Hair, HairRenderer, SpriteCollection.Hair, description["Hair"]);
			RestoreFromString(ref Beard, BeardRenderer, SpriteCollection.Beard, description["Beard"]);
			RestoreFromString(ref Helmet, HelmetRenderer, SpriteCollection.Helmet, description["Helmet"]);
			RestoreFromString(ref Glasses, GlassesRenderer, SpriteCollection.Glasses, description["Glasses"]);
			RestoreFromString(ref Mask, MaskRenderer, SpriteCollection.Mask, description["Mask"]);

            Armor.Clear();

			for (var i = 0; i < ArmorRenderers.Count; i++)
            {
                Sprite sprite = null;
				RestoreFromString(ref sprite, ArmorRenderers[i], SpriteCollection.Armor, description[$"Armor[{i}]"], byRendererName: true);

				if (sprite != null && !Armor.Contains(sprite)) Armor.Add(sprite);
			}
			
			WeaponType = (WeaponType) Enum.Parse(typeof(WeaponType), description["WeaponType"]);

            if (description.ContainsKey("PrimaryMeleeWeapon"))
            {
                RestoreFromString(ref PrimaryMeleeWeapon, PrimaryMeleeWeaponRenderer, GetWeaponCollection(WeaponType), description["PrimaryMeleeWeapon"]);
			}

            if (description.ContainsKey("SecondaryMeleeWeapon"))
            {
                RestoreFromString(ref SecondaryMeleeWeapon, SecondaryMeleeWeaponRenderer, GetWeaponCollection(WeaponType), description["SecondaryMeleeWeapon"]);
			}

            if (description.ContainsKey("Bow"))
            {
				RestoreFromString(ref Bow, BowRenderers, SpriteCollection.Bow, description["Bow"]);
			}

            if (description.ContainsKey("Firearms"))
            {
				RestoreFromString(ref Firearms, FirearmsRenderers, GetWeaponCollection(WeaponType), description["Firearms"]);

                if (!FirearmCollection.Instances.ContainsKey(SpriteCollection.Id)) throw new Exception($"FirearmCollection={SpriteCollection.Id} is missed!");

                var firearmParams = FirearmCollection.Instances[SpriteCollection.Id].Firearms.Single(i => i.Name == description["FirearmParams"]);

                if (firearmParams == null) throw new Exception($"FirearmCollection doesn't contain a definition for {description["FirearmParams"]}!");

                Firearm.Params = firearmParams;
				PrimaryMeleeWeapon = SecondaryMeleeWeapon = null;
                Bow = null;
            }
            else
            {
                Firearm.Params = new FirearmParams();
                Firearms = null;
            }

			RestoreFromString(ref Cape, CapeRenderer, SpriteCollection.Cape, description["Cape"]);
			RestoreFromString(ref Back, BackRenderer, SpriteCollection.Back, description["Back"]);
			RestoreFromString(ref Shield, ShieldRenderer, SpriteCollection.Shield, description["Shield"]);
			Expression = description["Expression"];
			Expressions = new List<Expression>();

			foreach (var key in description.Keys)
			{
				if (key.Contains("Expression."))
				{
					var parts = key.Split('.');
					var expressionName = parts[1];
					var expressionPart = parts[2];
					var expression = Expressions.SingleOrDefault(i => i.Name == expressionName);

					if (expression == null)
					{
						expression = new Expression { Name = expressionName };
						Expressions.Add(expression);
					}

					switch (expressionPart)
					{
						case "Eyebrows":
							RestoreFromString(ref expression.Eyebrows, EyebrowsRenderer, SpriteCollection.Eyebrows, description[key]);
							break;
						case "Eyes":
							RestoreFromString(ref expression.Eyes, EyesRenderer, SpriteCollection.Eyes, description[key]);
							break;
						case "Mouth":
							RestoreFromString(ref expression.Mouth, MouthRenderer, SpriteCollection.Mouth, description[key]);
							break;
						default:
							throw new NotSupportedException(expressionPart);
					}
				}
			}

			BodyScale = new Vector2(float.Parse(description["BodyScaleX"], CultureInfo.InvariantCulture), float.Parse(description["BodyScaleY"], CultureInfo.InvariantCulture));
			Initialize();
			UpdateAnimation();
		}

		private IEnumerable<SpriteGroupEntry> GetWeaponCollection(WeaponType weaponType)
		{
			switch (weaponType)
			{
				case WeaponType.Melee1H: return SpriteCollection.MeleeWeapon1H;
				case WeaponType.MeleePaired: return SpriteCollection.MeleeWeapon1H;
				case WeaponType.Melee2H: return SpriteCollection.MeleeWeapon2H;
				case WeaponType.Bow: return SpriteCollection.Bow;
				case WeaponType.Firearms1H: return SpriteCollection.Firearms1H;
				case WeaponType.FirearmsPaired: return SpriteCollection.Firearms1H;
				case WeaponType.Firearms2H: return SpriteCollection.Firearms2H;
				case WeaponType.Supplies: return SpriteCollection.Supplies;
				default:
					throw new NotSupportedException(weaponType.ToString());
			}
		}

		private static string SpriteToString(IEnumerable<SpriteGroupEntry> collection, SpriteRenderer renderer)
		{
			return SpriteToString(collection, renderer.sprite, renderer.color);
		}

		private static string SpriteToString(IEnumerable<SpriteGroupEntry> collection, Sprite sprite, Color color)
		{
			if (sprite == null) return null;

			var entry = collection.SingleOrDefault(i => i.Sprites.Contains(sprite));

		    if (entry == null)
		    {
		        throw new Exception($"Can't find {sprite.name} in SpriteCollection.");
		    }

			var result = color == Color.white ? entry.FullName : entry.FullName + "#" + ColorUtility.ToHtmlStringRGBA(color);

			return result;
		}

		private static void RestoreFromString(ref Sprite sprite, SpriteRenderer renderer, IEnumerable<SpriteGroupEntry> collection, string serialized, bool byRendererName = false)
		{
			if (string.IsNullOrEmpty(serialized))
			{
				sprite = renderer.sprite = null;
				renderer.color = Color.white;
				return;
			}

			var parts = serialized.Split('#');
			var id = parts[0];
			var color = Color.white;

			if (parts.Length > 1)
			{
				ColorUtility.TryParseHtmlString("#" + parts[1], out color);
			}

			var entries = collection.Where(i => i.FullName == id).ToList();

			switch (entries.Count)
			{
				case 1:
                    sprite = byRendererName ? entries[0].Sprites.SingleOrDefault(i => i.name == renderer.name.Split('[')[0]) : entries[0].Sprite;
					renderer.color = color;
					break;
				case 0:
					throw new Exception($"Entry with id {id} not found in SpriteCollection.");
				default:
					throw new Exception($"Multiple entries with id {id} found in SpriteCollection.");
			}
		}

		private static void RestoreFromString(ref List<Sprite> sprites, List<SpriteRenderer> renderers, IEnumerable<SpriteGroupEntry> collection, string serialized)
		{
			if (string.IsNullOrEmpty(serialized))
			{
				sprites = new List<Sprite>();

				foreach (var renderer in renderers)
				{
					renderer.sprite = null;
					renderer.color = Color.white;
				}
				
				return;
			}

			var parts = serialized.Split('#');
			var id = parts[0];
			var color = Color.white;

			if (parts.Length > 1)
			{
				ColorUtility.TryParseHtmlString("#" + parts[1], out color);
			}

			var entries = collection.Where(i => i.FullName == id).ToList();

			switch (entries.Count)
			{
				case 1:
					sprites = entries[0].Sprites.ToList();
					renderers.ForEach(i => i.color = color);
					break;
				case 0:
					throw new Exception($"Entry with id {id} not found in SpriteCollection.");
				default:
					throw new Exception($"Multiple entries with id {id} found in SpriteCollection.");
			}
		}
	}
}