namespace Assets.HeroEditor.FantasyInventory.Scripts.Enums
{
    /// <summary>
    /// Item tags can be used for implementing custom logic (special cases).
    /// Use constant integer values for enums to avoid data distortion when adding/removing new values.
    /// </summary>
    public enum ItemTag
    {
        Undefined   = 0,
        NotForSale  = 1,
        Quest       = 2,
        TwoHanded	= 3,
        Light       = 4,
        Heavy       = 5,
        Short       = 6,
        Long        = 7,
        Christmas   = 8,
        Farm        = 9
    }
}