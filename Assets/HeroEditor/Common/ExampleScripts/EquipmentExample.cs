using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.CommonScripts;
using HeroEditor.Common.Enums;
using UnityEngine;

namespace Assets.HeroEditor.Common.ExampleScripts
{
    /// <summary>
    /// An example of how to change character's equipment.
    /// </summary>
    public class EquipmentExample : MonoBehaviour
    {
        public Character Character;

        public void EquipRandomArmor()
        {
            var randomIndex = Random.Range(0, Character.SpriteCollection.Armor.Count);
            var randomItem = Character.SpriteCollection.Armor[randomIndex];

            Character.Equip(randomItem, EquipmentPart.Armor);
        }

        public void RemoveArmor()
        {
            Character.UnEquip(EquipmentPart.Armor);
        }

        public void EquipRandomHelmet()
        {
            Character.Equip(Character.SpriteCollection.Helmet.Random(), EquipmentPart.Helmet);
        }

        public void RemoveHelmet()
        {
            Character.UnEquip(EquipmentPart.Helmet);
        }

        public void EquipRandomShield()
        {
            Character.Equip(Character.SpriteCollection.Shield.Random(), EquipmentPart.Shield);
        }

        public void RemoveShield()
        {
            Character.UnEquip(EquipmentPart.Shield);
        }

        public void EquipRandomWeapon()
        {
            Character.Equip(Character.SpriteCollection.MeleeWeapon1H.Random(), EquipmentPart.MeleeWeapon1H);
        }

        public void RemoveWeapon()
        {
            Character.UnEquip(EquipmentPart.MeleeWeapon1H);
        }

        public void RandomAppearance()
        {
            var randomColor = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), 1);

            Character.SetBody(Character.SpriteCollection.Hair.Random(), BodyPart.Hair, randomColor);
            Character.SetBody(Character.SpriteCollection.Eyebrows.Random(), BodyPart.Eyebrows);
            Character.SetBody(Character.SpriteCollection.Eyes.Random(), BodyPart.Eyes);
            Character.SetBody(Character.SpriteCollection.Mouth.Random(), BodyPart.Mouth);
        }
    }
}