using System;
using Assets.HeroEditor.FantasyInventory.Scripts.Enums;
using Newtonsoft.Json;

namespace Assets.HeroEditor.FantasyInventory.Scripts.Data
{
    /// <summary>
    /// Represents item object for storing with game profile (please note, that item params are stored separately in params database).
    /// </summary>
    [Serializable]
    public class Item
    {
        public string Id; // Id is not unique. Use Hash to compare items!
        public Modifier Modifier;
        public int Count;

        public bool IsModified => Modifier != null && Modifier.Id != ItemModifier.None;

        /// <summary>
        /// This function may be overridden by the game. For example, the game may vary item params depending on Modificator.
        /// </summary>
        public static Func<Item, ItemParams> GetParams = item =>
        {
            if (ItemCollection.Instance == null) throw new ArgumentNullException(nameof(ItemCollection.Instance));

            if (ItemCollection.Instance.Dict.ContainsKey(item.Id))
            {
                return ItemCollection.Instance.Dict[item.Id];
            }

            throw new Exception($"Item params missed: {item.Id}");
        };

        public Item()
        {
        }

        public Item(string id, int count = 1)
        {
            Id = id;
            Count = count;
        }

        public Item(string id, Modifier modifier, int count = 1)
        {
            Id = id;
            Count = count;
            Modifier = modifier;
        }

        public Item Clone()
        {
            return (Item) MemberwiseClone();
        }

        [JsonIgnore] public ItemParams Params => GetParams(this);
        [JsonIgnore] public int Hash => $"{Id}.{Modifier?.Id}.{Modifier?.Level}".GetHashCode();
        [JsonIgnore] public bool IsEquipment => Params.Type == ItemType.Armor || Params.Type == ItemType.Helmet || Params.Type == ItemType.Weapon || Params.Type == ItemType.Shield;
        [JsonIgnore] public bool IsWeapon => Params.Type == ItemType.Weapon;
        [JsonIgnore] public bool IsShield => Params.Type == ItemType.Shield;
        [JsonIgnore] public bool IsDagger => Params.Class == ItemClass.Dagger;
        [JsonIgnore] public bool IsSword => Params.Class == ItemClass.Sword;
        [JsonIgnore] public bool IsAxe => Params.Class == ItemClass.Axe;
        [JsonIgnore] public bool IsWand => Params.Class == ItemClass.Wand;
        [JsonIgnore] public bool IsBlunt => Params.Class == ItemClass.Blunt;
        [JsonIgnore] public bool IsLance => Params.Class == ItemClass.Lance;
        [JsonIgnore] public bool IsMelee => Params.Type == ItemType.Weapon && Params.Class != ItemClass.Bow;
        [JsonIgnore] public bool IsBow => Params.Class == ItemClass.Bow;
        [JsonIgnore] public bool IsFirearm => Params.Class == ItemClass.Firearm;
        [JsonIgnore] public bool IsOneHanded => !IsTwoHanded;
        [JsonIgnore] public bool IsTwoHanded => Params.Class == ItemClass.Bow || Params.Tags.Contains(ItemTag.TwoHanded);
    }
}