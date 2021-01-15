using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateData 
{
    public string stateName;
    private Sprite stateImage;
    public List<KeyWordModel> keyWordModels;
    public List<CustomString> customDescription;

    public Sprite StateImage
    {
        get 
        { 
            if (stateImage == null)
            {
                stateImage = GetMySprite();
                return stateImage;
            }
            else
                return stateImage;
        }
    }

    public Sprite GetMySprite()
    {
        return StateController.Instance.GetStateDataByName(stateName).stateImage;
    }

}
