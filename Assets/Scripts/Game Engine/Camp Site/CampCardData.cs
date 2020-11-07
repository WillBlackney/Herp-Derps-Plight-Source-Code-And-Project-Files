using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampCardData
{
    public string cardName;
    public int cardEnergyCost;
    public CampTargettingType targettingType;
    public List<CampCardTargettingCondition> targetRequirements;

    public bool expend;
    public bool innate;

    public List<CampCardEffect> cardEffects = new List<CampCardEffect>();
    public List<CustomString> customDescription = new List<CustomString>();
    public List<KeyWordModel> keyWordModels = new List<KeyWordModel>();
}
