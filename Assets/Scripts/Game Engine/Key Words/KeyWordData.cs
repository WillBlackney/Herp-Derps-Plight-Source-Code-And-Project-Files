using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class KeyWordData
{
    // Properties
    #region
    [Header("Properties")]
    public KeyWordType kewWordType;

    [ShowIf("ShowWeaponRequirement")]
    public CardWeaponRequirement weaponRequirement;

    [ShowIf("ShowPassiveType")]
    public Passive passiveType;

    [ShowIf("ShowDescription")]
    [TextArea]
    public string keyWordDescription;
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
    public bool ShowDescription()
    {
        if(kewWordType == KeyWordType.Passive)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion
}
