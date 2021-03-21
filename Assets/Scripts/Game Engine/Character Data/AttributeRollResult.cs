using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeRollResult
{
    // Fields
    #region
    public int strengthRoll = 1;
    public int intelligenceRoll = 1;
    public int dexterityRoll = 1;
    public int witsRoll = 1;
    public int constitutionRoll = 1;
    public bool alreadyGenerated = false;
    #endregion

    public void GenerateMyRolls()
    {
        if (alreadyGenerated)
        {
            Debug.Log("AttributeRollResult() already generated rolls, cancelling...");
            return;
        }

        alreadyGenerated = true;
        strengthRoll = RandomGenerator.NumberBetween(1, 3);
        intelligenceRoll = RandomGenerator.NumberBetween(1, 3);
        dexterityRoll = RandomGenerator.NumberBetween(1, 3);
        witsRoll = RandomGenerator.NumberBetween(1, 3);
        constitutionRoll = RandomGenerator.NumberBetween(1, 3);

        Debug.Log("AttributeRollResult.GenerateMyRolls() generated result: Strength = " + strengthRoll.ToString() +
            ", Intelligence = " + intelligenceRoll.ToString() + ", Dexterity = " + dexterityRoll.ToString() +
            ", Wits = " + witsRoll.ToString() + ", Constitution = " + constitutionRoll.ToString());
    }
    public AttributeRollResult Clone(AttributeRollResult original)
    {
        AttributeRollResult clone = new AttributeRollResult();

        clone.alreadyGenerated = original.alreadyGenerated;
        clone.strengthRoll = original.strengthRoll;
        clone.intelligenceRoll = original.intelligenceRoll;
        clone.dexterityRoll = original.dexterityRoll;
        clone.witsRoll = original.witsRoll;
        clone.constitutionRoll = original.constitutionRoll;

        return clone;
    }

   
}
