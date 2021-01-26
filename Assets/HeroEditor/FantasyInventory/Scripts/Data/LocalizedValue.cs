using System;

namespace Assets.HeroEditor.FantasyInventory.Scripts.Data
{
    [Serializable]
    public class LocalizedValue
    {
        public string Language;
        public string Value;

        public LocalizedValue(string language, string value)
        {
            Language = language;
            Value = value;
        }
    }
}