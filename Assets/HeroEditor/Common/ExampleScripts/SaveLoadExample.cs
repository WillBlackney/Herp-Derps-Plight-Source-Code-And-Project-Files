using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.CharacterScripts;
using HeroEditor.Common.Enums;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.HeroEditor.Common.ExampleScripts
{
	/// <summary>
	/// Class for storing character data
	/// </summary>
	[Serializable]
	public class CharacterData // TODO: Add more properties
	{
		public string Helmet;
		public string Armor;
		public List<string> Weapons;
		public WeaponType WeaponType;
	}

	/// <summary>
	/// This example will show you how to store character data. Don't mix it up with saving prefabs!
	/// </summary>
	public static class SaveLoadExample
	{
		public static void SaveToPlayerPrefs(Character character)
		{
			var characterData = new CharacterData
			{
				Helmet = character.Helmet.texture.name,
				Armor = character.Armor[0].texture.name,
				WeaponType = character.WeaponType
			};

			switch (character.WeaponType)
			{
				case WeaponType.Melee1H:
				case WeaponType.Melee2H:
					characterData.Weapons = new List<string> { character.PrimaryMeleeWeapon.texture.name };
					break;
				case WeaponType.MeleePaired:
					characterData.Weapons = new List<string> { character.PrimaryMeleeWeapon.texture.name, character.SecondaryMeleeWeapon.texture.name };
					break;
				case WeaponType.Bow:
					characterData.Weapons = new List<string> { character.Bow[0].texture.name };
					break;
				case WeaponType.Firearms1H:
				case WeaponType.Firearms2H:
					characterData.Weapons = new List<string> { character.Firearms[0].texture.name };
					break;
				default: throw new NotImplementedException();
			}

			var json = JsonUtility.ToJson(characterData);

			PlayerPrefs.SetString("Character", json);
			PlayerPrefs.Save();
		}

		public static CharacterData LoadFromPlayerPrefs()
		{
			var json = PlayerPrefs.GetString("Character");

			return JsonUtility.FromJson<CharacterData>(json);
		}

		public static Character CreateCharacter(GameObject prefab, CharacterData characterData)
		{
			var character = Object.Instantiate(prefab).GetComponent<Character>();
            var spriteCollection = character.SpriteCollection;

			character.Helmet = spriteCollection.Helmet.Single(i => i.Name == characterData.Helmet).Sprite;
			character.Armor = spriteCollection.Armor.Single(i => i.Name == characterData.Armor).Sprites;
			character.WeaponType = characterData.WeaponType;

			switch (character.WeaponType)
			{
				case WeaponType.Melee1H:
					character.PrimaryMeleeWeapon = spriteCollection.MeleeWeapon1H.Single(i => i.Name == characterData.Weapons[0]).Sprite;
					break;
				case WeaponType.Melee2H:
					character.PrimaryMeleeWeapon = spriteCollection.MeleeWeapon2H.Single(i => i.Name == characterData.Weapons[0]).Sprite;
					break;
				case WeaponType.MeleePaired:
					character.PrimaryMeleeWeapon = spriteCollection.MeleeWeapon1H.Single(i => i.Name == characterData.Weapons[0]).Sprite;
					character.SecondaryMeleeWeapon = spriteCollection.MeleeWeapon1H.Single(i => i.Name == characterData.Weapons[1]).Sprite;
					break;
				case WeaponType.Bow:
					character.Bow = spriteCollection.Bow.Single(i => i.Name == characterData.Weapons[0]).Sprites;
					break;
				case WeaponType.Firearms1H:
					character.Bow = spriteCollection.Firearms1H.Single(i => i.Name == characterData.Weapons[0]).Sprites;
					break;
				case WeaponType.Firearms2H:
					character.Bow = spriteCollection.Firearms2H.Single(i => i.Name == characterData.Weapons[0]).Sprites;
					break;
				default: throw new NotImplementedException();
			}

			return character;
		}
	}
}