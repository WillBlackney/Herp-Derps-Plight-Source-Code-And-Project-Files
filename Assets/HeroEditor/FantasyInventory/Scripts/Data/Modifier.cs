using System;
using Assets.HeroEditor.FantasyInventory.Scripts.Enums;

namespace Assets.HeroEditor.FantasyInventory.Scripts.Data
{
    /// <summary>
    /// Can be used for item augmentation and other modifications.
    /// </summary>
    [Serializable]
    public class Modifier
    {
        public ItemModifier Id;
        public int Level;

        public Modifier()
        {
        }

        public Modifier(ItemModifier id, int level)
        {
            Id = id;
            Level = level;
        }
    }
}