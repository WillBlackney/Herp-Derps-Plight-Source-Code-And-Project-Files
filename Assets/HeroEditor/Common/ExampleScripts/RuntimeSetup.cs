using System.Linq;
using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.EditorScripts;
using Assets.HeroEditor.Common.CharacterScripts.Firearms;
using HeroEditor.Common.Enums;
using UnityEngine;

namespace Assets.HeroEditor.Common.ExampleScripts
{
	/// <summary>
	/// Changing equipment at runtime examples.
	/// </summary>
	public class RuntimeSetup : MonoBehaviour // TODO: Extend this class to fit your needs!
	{
		public Character Character;

		/// <summary>
		/// Example call: SetBody("HeadScar", "Basic", "Human", "Basic");
		/// </summary>
		public void SetBody(string headName, string headCollection, string bodyName, string bodyCollection)
		{
			var head = Character.SpriteCollection.Head.SingleOrDefault(i => i.Name == headName && i.Collection == headCollection);
			var body = Character.SpriteCollection.Body.SingleOrDefault(i => i.Name == bodyName && i.Collection == bodyCollection);

            Character.SetBody(head, BodyPart.Head);
            Character.SetBody(body, BodyPart.Body);
		}

		public void EquipMeleeWeapon1H(string spriteName, string collectionName)
		{
			var entry = Character.SpriteCollection.MeleeWeapon1H.SingleOrDefault(i => i.Name == spriteName && i.Collection == collectionName);
			
			Character.Equip(entry, EquipmentPart.MeleeWeapon1H);
		}

		public void EquipMeleeWeapon2H(string spriteName, string collectionName)
		{
			var entry = Character.SpriteCollection.MeleeWeapon2H.SingleOrDefault(i => i.Name == spriteName && i.Collection == collectionName);

			Character.Equip(entry, EquipmentPart.MeleeWeapon2H);
		}

		public void EquipBow(string spriteName, string collectionName)
		{
			var entry = Character.SpriteCollection.Bow.SingleOrDefault(i => i.Name == spriteName && i.Collection == collectionName);

			Character.Equip(entry, EquipmentPart.Bow);
		}

		public void EquipFirearm1H(string spriteName, string collectionName)
		{
			var entry = Character.SpriteCollection.Firearms1H.SingleOrDefault(i => i.Name == spriteName && i.Collection == collectionName);
			
			Character.Equip(entry, EquipmentPart.Firearm1H);
            Character.Firearm.Params = FirearmCollection.Instances[Character.SpriteCollection.Id].Firearms.SingleOrDefault(i => i.Name == spriteName);
		}

		public void EquipShield(string spriteName, string collectionName)
		{
			var entry = Character.SpriteCollection.Shield.SingleOrDefault(i => i.Name == spriteName && i.Collection == collectionName);

			Character.Equip(entry, EquipmentPart.Shield);
		}

		public void EquipArmor(string spriteName, string collectionName)
		{
			var entry = Character.SpriteCollection.Armor.SingleOrDefault(i => i.Name == spriteName && i.Collection == collectionName);

			Character.Equip(entry, EquipmentPart.Armor);
		}

		public void EquipHelmet(string spriteName, string collectionName)
		{
			var entry = Character.SpriteCollection.Helmet.SingleOrDefault(i => i.Name == spriteName && i.Collection == collectionName);

            Character.Equip(entry, EquipmentPart.Helmet);
		}

		public void RemoveHelmet()
		{
            Character.UnEquip(EquipmentPart.Helmet); // Simply put null to remove equipment.
		}

        public void RemoveHair()
        {
            Character.Hair = null; // Alternatively, you can work with properties directly (or write your own wrappers).
            Character.Initialize();
		}
	}
}