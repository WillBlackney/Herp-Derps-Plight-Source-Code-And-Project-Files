using System.Collections.Generic;
using Assets.HeroEditor.FantasyInventory.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor.FantasyInventory.Scripts.Interface.Elements
{
    public class ScrollInventoryFilter : MonoBehaviour
    {
        public ScrollInventory ScrollInventory;
        public Toggle Weapon;
        public Toggle Armor;
        public Toggle Helmet;
        public Toggle Shield;

        public void OnSelect(bool value)
        {
            var types = new List<ItemType>();

            if (Weapon.isOn) types.Add(ItemType.Weapon);
            if (Armor.isOn) types.Add(ItemType.Armor);
            if (Helmet.isOn) types.Add(ItemType.Helmet);
            if (Shield.isOn) types.Add(ItemType.Shield);
            
            ScrollInventory.SetTypeFilter(types);
        }
    }
}