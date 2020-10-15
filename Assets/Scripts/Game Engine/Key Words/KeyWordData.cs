using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class KeyWordData
{
    public KeyWordType kewWordType;

    [ShowIf("ShowWeaponRequirement")]
    public CardWeaponRequirement weaponRequirement;

    [ShowIf("ShowPassiveType")]
    public Passive passiveType;

    [ShowIf("ShowDescription")]
    [TextArea]
    public string keyWordDescription;

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
}
