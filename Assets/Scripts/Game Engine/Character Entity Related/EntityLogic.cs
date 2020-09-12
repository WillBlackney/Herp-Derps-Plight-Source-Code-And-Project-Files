using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class EntityLogic
{
    // NEW CALCULATORS!
    #region
    public static int GetTotalStamina(CharacterEntityModel entity)
    {
        Debug.Log("EntityLogic.GetTotalStamina() called for " + entity.myName + "...");

        // Base Stamina
        int staminaReturned = entity.stamina;
        Debug.Log(entity.myName + " base stamina: " + staminaReturned.ToString());

        // Bonus Stamina
        staminaReturned += entity.pManager.bonusStaminaStacks;
        Debug.Log("Value after bonus stamina added: " + staminaReturned.ToString());

        // Temporary Bonus Stamina
        staminaReturned += entity.pManager.temporaryBonusStaminaStacks;
        Debug.Log("Value after temporary bonus stamina added: " + staminaReturned.ToString());

        // Return final value
        Debug.Log("Final stamina value calculated: " + staminaReturned.ToString());
        return staminaReturned;
    }
    public static int GetTotalDraw(CharacterEntityModel entity)
    {
        Debug.Log("EntityLogic.GetTotalDraw() called for " + entity.myName + "...");

        // Base Draw
        int drawReturned = entity.draw;
        Debug.Log(entity.myName + " base draw: " + drawReturned.ToString());

        // Bonus Draw
        drawReturned += entity.pManager.bonusDrawStacks;
        Debug.Log("Value after bonus draw added: " + drawReturned.ToString());

        // Temporary Bonus Draw
        drawReturned += entity.pManager.temporaryBonusDrawStacks;
        Debug.Log("Value after temporary bonus draw added: " + drawReturned.ToString());

        // Return final value
        Debug.Log("Final draw value calculated: " + drawReturned.ToString());
        return drawReturned;
    }
    public static int GetTotalPower(CharacterEntityModel entity)
    {
        Debug.Log("EntityLogic.GetTotalStrength() called for " + entity.myName + "...");

        // Base Power
        int strengthReturned = entity.power;
        Debug.Log(entity.myName + " base strength: " + strengthReturned.ToString());

        // Bonus Power
        strengthReturned += entity.pManager.bonusPowerStacks;
        Debug.Log("Value after bonus strength added: " + strengthReturned.ToString());

        // Temporary Bonus Power
        strengthReturned += entity.pManager.temporaryBonusPowerStacks;
        Debug.Log("Value after temporary bonus strength added: " + strengthReturned.ToString());

        // Return final value
        Debug.Log("Final strength value calculated: " + strengthReturned.ToString());
        return strengthReturned;
    }
    public static int GetTotalInitiative(CharacterEntityModel entity)
    {
        Debug.Log("EntityLogic.GetTotalInitiative() called for " + entity.myName + "...");

        // Base Stamina
        int initiativeReturned = entity.initiative;
        Debug.Log(entity.myName + " base initiative: " + initiativeReturned.ToString());

        // Bonus Stamina
        initiativeReturned += entity.pManager.bonusInitiativeStacks;
        Debug.Log("Value after bonus initiative added: " + initiativeReturned.ToString());

        // Temporary Bonus Stamina
        initiativeReturned += entity.pManager.temporaryBonusInitiativeStacks;
        Debug.Log("Value after temporary bonus initiative added: " + initiativeReturned.ToString());

        // Return final value
        Debug.Log("Final initiative value calculated: " + initiativeReturned.ToString());
        return initiativeReturned;
    }
    public static int GetTotalDexterity(CharacterEntityModel entity)
    {
        Debug.Log("EntityLogic.GetTotalDexterity() called for " + entity.myName + "...");

        // Base Dexterity
        int dexterityReturned = entity.dexterity;
        Debug.Log(entity.myName + " base dexterity: " + dexterityReturned.ToString());

        // Bonus Dexterity
        dexterityReturned += entity.pManager.bonusDexterityStacks;
        Debug.Log("Value after bonus dexterity added: " + dexterityReturned.ToString());

        // Temporary Bonus Dexterity
        dexterityReturned += entity.pManager.temporaryBonusDexterityStacks;
        Debug.Log("Value after temporary bonus dexterity added: " + dexterityReturned.ToString());

        // Return final value
        Debug.Log("Final dexterity value calculated: " + dexterityReturned.ToString());
        return dexterityReturned;
    }
    #endregion
}