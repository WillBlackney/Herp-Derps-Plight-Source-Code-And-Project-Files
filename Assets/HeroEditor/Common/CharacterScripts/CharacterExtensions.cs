using Assets.HeroEditor.Common.CommonScripts;
using Assets.HeroEditor.Common.CharacterScripts.Firearms;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using UnityEngine;

namespace Assets.HeroEditor.Common.CharacterScripts
{
    /// <summary>
    /// You can extend 'CharacterBase' class here. Alternatively, you can just use derived class 'Character' for adding new features.
    /// </summary>
    public static class CharacterExtensions
    {
        public static void Randomize(this CharacterBase character)
        {
            character.ResetEquipment();

            var randomColor = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), 1);

            character.SetBody(character.SpriteCollection.Hair.Random(), BodyPart.Hair, randomColor);
            character.SetBody(character.SpriteCollection.Eyebrows.Random(), BodyPart.Eyebrows);
            character.SetBody(character.SpriteCollection.Eyes.Random(), BodyPart.Eyes);
            character.SetBody(character.SpriteCollection.Mouth.Random(), BodyPart.Mouth);

            character.Equip(character.SpriteCollection.Helmet.Random(), EquipmentPart.Helmet);
            character.Equip(character.SpriteCollection.Armor.Random(), EquipmentPart.Armor);
            character.Equip(character.SpriteCollection.MeleeWeapon1H.Random(), EquipmentPart.MeleeWeapon1H);
            character.Equip(character.SpriteCollection.Shield.Random(), EquipmentPart.Shield);
        }

        public static Firearm GetFirearm(this CharacterBase character)
        {
            return ((Character) character).Firearm;
        }
    }
}