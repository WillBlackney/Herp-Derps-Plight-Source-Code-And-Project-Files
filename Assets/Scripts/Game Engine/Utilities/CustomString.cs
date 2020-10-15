using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class CustomString 
{
    public TextColor color;
    public bool getPhraseFromCardValue = false;

    [ShowIf("ShowCardEffectType")]
    public CardEffectType cardEffectType;

    [ShowIf("ShowPhrase")]
    [TextArea]
    public string phrase;
    
    public bool ShowPhrase()
    {
        if(getPhraseFromCardValue == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ShowCardEffectType()
    {
        return getPhraseFromCardValue;
    }
}


