namespace Assets.HeroEditor.FantasyInventory.Scripts.Enums
{
    public enum ItemModifier
    {
        None = 0,
        Reinforced = 1, // Increase damage/resistance and STR req.
        Refined = 2, // Increase damage/resistance and DEX req.
        Sharpened = 3, // Critical damage up.
        Lightweight = 4, // Reduce weight.
        Poison = 5, // Add poison damage/resistance.
        Bleeding = 6, // Add bleeding damage/resistance.
        Spread = 7, // Reduced damage in a column.
        Onslaught = 8, // Reduced damage in a line;
        Shieldbreaker = 9, // Ignore shield.
        Fire = 10, // Add fire damage/resistance.
        Ice = 11, // Add ice damage/resistance.
        Lightning = 12, // Add lightning damage/resistance.
        Light = 13, // Add healing ability and halved holy damage/resistance.
        Darkness = 14, // Add darkness damage/resistance.
        Vampiric = 15 // Restore HP after each hit.
    }
}