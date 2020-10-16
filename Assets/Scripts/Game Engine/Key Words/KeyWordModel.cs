using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class KeyWordModel 
{
    // Properties
    #region
    [Header("Properties")]
    public KeyWordType kewWordType;

    [ShowIf("ShowWeaponRequirement")]
    public CardWeaponRequirement weaponRequirement;

    [ShowIf("ShowPassiveType")]
    public Passive passiveType;
    #endregion

    // Odin Show if's
    #region
    public bool ShowWeaponRequirement()
    {
        return kewWordType == KeyWordType.WeaponRequirement;
    }
    public bool ShowPassiveType()
    {
        return kewWordType == KeyWordType.Passive;
    }
    #endregion
}

