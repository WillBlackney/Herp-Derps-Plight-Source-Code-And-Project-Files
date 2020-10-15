using System;
using Sirenix.OdinInspector;

[Serializable]
public class KeyWordModel 
{
    public KeyWordType kewWordType;

    [ShowIf("ShowWeaponRequirement")]
    public CardWeaponRequirement weaponRequirement;

    [ShowIf("ShowPassiveType")]
    public Passive passiveType;

    public bool ShowWeaponRequirement()
    {
        return kewWordType == KeyWordType.WeaponRequirement;
    }
    public bool ShowPassiveType()
    {
        return kewWordType == KeyWordType.Passive;
    }
}

public enum KeyWordType
{
    None = 0,
    Expend = 1,
    Innate = 2,
    Fleeting = 3,
    Unplayable = 4,
    Blessing = 5,
    Shank = 6,
    Burn = 11,
    WeaponRequirement = 7,
    Passive = 8,
    Block = 9,
    Energy = 10,


}