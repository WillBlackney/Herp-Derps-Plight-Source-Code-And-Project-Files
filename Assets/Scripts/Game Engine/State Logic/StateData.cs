using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateData 
{
    public StateName stateName;
    private Sprite stateSprite;
    public Rarity rarity;
    public List<KeyWordModel> keyWordModels;
    public List<CustomString> customDescription;
    public bool lootable = true;

    public bool hasStacks;
    public int currentStacks;
    

    public Sprite StateSprite
    {
        get 
        { 
            if (stateSprite == null)
            {
                stateSprite = GetMySprite();
                return stateSprite;
            }
            else
                return stateSprite;
        }
    }

    private Sprite GetMySprite()
    {
        return StateController.Instance.GetStateDataByName(stateName).stateImage;
    }

}
