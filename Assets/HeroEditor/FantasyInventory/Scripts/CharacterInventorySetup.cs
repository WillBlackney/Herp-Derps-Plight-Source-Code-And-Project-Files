using System;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.CommonScripts;
using Assets.HeroEditor.FantasyInventory.Scripts.Data;
using Assets.HeroEditor.FantasyInventory.Scripts.Enums;
using HeroEditor.Common.Enums;
using UnityEngine;

namespace Assets.HeroEditor.FantasyInventory.Scripts
{
    public class CharacterInventorySetup
    {
        public static void Setup(Character character, List<Item> equipped)
        {
            character.ResetEquipment();

            foreach (var item in equipped)
            {
                try
                {
                    switch (item.Params.Type)
                    {
                        case ItemType.Helmet:
                            character.Helmet = character.SpriteCollection.Helmet.FindSprite(item.Params.Path);
                            break;
                        case ItemType.Armor:
                            character.Armor = character.SpriteCollection.Armor.FindSprites(item.Params.Path);
                            break;
                        case ItemType.Shield:
                            character.Shield = character.SpriteCollection.Shield.FindSprite(item.Params.Path);
                            character.WeaponType = WeaponType.Melee1H;
                            break;
                        case ItemType.Weapon:

                            switch (item.Params.Class)
                            {
                                case ItemClass.Bow:
                                    character.WeaponType = WeaponType.Bow;
                                    character.Bow = character.SpriteCollection.Bow.FindSprites(item.Params.Path);
                                    break;
                                default:
                                    if (item.IsFirearm)
                                    {
                                        throw new NotImplementedException("Firearm equipping is not implemented. Implement if needed.");
                                    }
                                    else
                                    {
                                        character.WeaponType = item.Params.Tags.Contains(ItemTag.TwoHanded) ? WeaponType.Melee2H : WeaponType.Melee1H;
                                        character.PrimaryMeleeWeapon = (character.WeaponType == WeaponType.Melee1H ? character.SpriteCollection.MeleeWeapon1H : character.SpriteCollection.MeleeWeapon2H).FindSprite(item.Params.Path);
                                    }
                                    break;
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogErrorFormat("Unable to equip {0} ({1})", item.Params.Path, e.Message);
                }
            }

            character.Initialize();
        }
    }
}