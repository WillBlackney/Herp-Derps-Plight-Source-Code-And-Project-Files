using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeRollResult
{
    // Fields
    #region
    private int strengthRoll;
    private int intelligenceRoll;
    private int dexterityRoll;
    private int witsRoll;
    private int constitutionRoll;
    #endregion

    // Accessors + Getters
    #region
    public int StrengthRoll
    {
        get { return strengthRoll; }
    }
    public int IntelligenceRoll
    {
        get { return intelligenceRoll; }
    }
    public int DexterityRoll
    {
        get { return dexterityRoll; }
    }
    public int WitsRoll
    {
        get { return witsRoll; }
    }
    public int ConstitutionRoll
    {
        get { return constitutionRoll; }
    }
    #endregion

    public AttributeRollResult(CharacterData character)
    {
        strengthRoll = RandomGenerator.NumberBetween(1, 3);
        intelligenceRoll = RandomGenerator.NumberBetween(1, 3);
        dexterityRoll = RandomGenerator.NumberBetween(1, 3);
        witsRoll = RandomGenerator.NumberBetween(1, 3);
        constitutionRoll = RandomGenerator.NumberBetween(1, 3);
    }
    public AttributeRollResult(AttributeRollResult original)
    {
        strengthRoll = original.strengthRoll;
        intelligenceRoll = original.intelligenceRoll;
        dexterityRoll = original.dexterityRoll;
        witsRoll = original.witsRoll;
        constitutionRoll = original.constitutionRoll;
    }
}
