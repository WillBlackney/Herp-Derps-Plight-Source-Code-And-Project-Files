using UnityEngine;
using System.Collections;

public class SpellEffect
{
    public virtual void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        Debug.Log("No Spell effect with this name found! Check for typos in CardAssets");
    }
        
}
