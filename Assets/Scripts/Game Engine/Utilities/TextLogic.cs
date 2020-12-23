using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class TextLogic 
{
    // Properties + Color Codes
    #region
    [Header("RGBA Colour Codes")]
    public static string white = "<color=#FFFFFF>";
    public static string yellow = "<color=#FFF91C>";
    public static string blueNumber = "<color=#92E0FF>";
    public static string neutralYellow = "<color=#F8FF00>";

    public static string physical = "<color=#FF9500>";
    public static string magic = "<color=#C975FF>";
    public static string fire = "<color=#FF6637>";
    public static string frost = "<color=#3687FF>";
    public static string shadow = "<color=#CF01BC>";
    public static string air = "<color=#36EDFF>";
    public static string poison = "<color=#00EC4A>";

    public static string commonRarity = "<color=#FFFFFF>";
    public static string rareRarity = "<color=#6BFFE2>";
    public static string epicRarity = "<color=#D14EFF>";
    public static string legendaryRarity = "<color=#FFD440>";
    #endregion

    // Build stuff
    #region
    public static string ConvertCustomStringListToString(List<CustomString> csList)
    {
        string stringReturned = "";
        foreach(CustomString cs in csList)
        {
            stringReturned += ReturnColoredText(cs.phrase, GetColorCodeFromEnum(cs.color));
        }

        return stringReturned;
    }
    #endregion

    // Get strings, colours and text
    #region
    public static string ReturnColoredText(string text, string color)
    {
        // Just give it a string and a color reference,
        // and this function takes care of everything
        return (color + text + white);
    }
    private static string GetColorCodeFromEnum(TextColor color)
    {
        string colorCodeReturned = "";

        // standard colors
        if (color == TextColor.KeyWordYellow)
        {
            colorCodeReturned = neutralYellow;
        }
        else if(color == TextColor.BlueNumber)
        {
            colorCodeReturned = blueNumber;
        }
        else if (color == TextColor.White)
        {
            colorCodeReturned = white;
        }

        // Damage type colors
        else if (color == TextColor.PhysicalBrown)
        {
            colorCodeReturned = physical;
        }
        else if (color == TextColor.MagicPurple)
        {
            colorCodeReturned = magic;
        }
        else if(color == TextColor.FireRed)
        {
            colorCodeReturned = fire;
        }
        else if (color == TextColor.PoisonGreen)
        {
            colorCodeReturned = poison;
        }
        else if (color == TextColor.AirBlue)
        {
            colorCodeReturned = air;
        }
        else if (color == TextColor.ShadowPurple)
        {
            colorCodeReturned = shadow;
        }
        else if (color == TextColor.FireRed)
        {
            colorCodeReturned = fire;
        }

        else
        {
            colorCodeReturned = white;
        }

        return colorCodeReturned;
    }
    #endregion

    // General string functions
    #region
    public static string SplitByCapitals(string originalString)
    {
        // Function seperates a string by capitals, 
        // puts a space at the end of each string, then
        // recombines them into one string

        string stringReturned = originalString;

        StringBuilder builder = new StringBuilder();
        foreach (char c in stringReturned)
        {
            if (Char.IsUpper(c) && builder.Length > 0) builder.Append(' ');
            builder.Append(c);
        }

        stringReturned = builder.ToString();

        return stringReturned;
    }
    #endregion

    // Misc Stuff
    #region
    /*
    public static string GetTalentPairingTierOneDescriptionText(TalentSchool ts)
    {
        return "This character can add " + ReturnColoredText("Common", commonRarity) + ", " +
                ReturnColoredText("Rare", rareRarity) + " and " + ReturnColoredText("Epic", epicRarity) +
                " cards from the " + ReturnColoredText(ts.ToString(), neutralYellow) + " tree to it's deck.";
    }*/
    public static string GetTalentPairingTierOneDescriptionText(TalentSchool ts)
    {
        string sReturned =  "This character may add cards from the " + ReturnColoredText(ts.ToString(), neutralYellow) + " tree to it's deck. ";
        sReturned += "\n";

        if (ts == TalentSchool.Corruption)
        {
            sReturned += "Increase damage dealt against targets with " +
                ReturnColoredText("Poisoned", neutralYellow) + " by " + ReturnColoredText("5%", blueNumber) + ".";
        }
        else if (ts == TalentSchool.Warfare)
        {
            sReturned += ReturnColoredText("Melee Attack", neutralYellow) + " cards deal " +
                ReturnColoredText("10%", blueNumber) + " more damage.";
        }
        else if (ts == TalentSchool.Guardian)
        {
            sReturned += "Reduce all damage from enemy actions by " + ReturnColoredText("10%", blueNumber) +
                 ".";
        }
        else if (ts == TalentSchool.Scoundrel)
        {
            sReturned += "Increase the damage and " + ReturnColoredText("Block", neutralYellow) + " gained from " +
               ReturnColoredText("Criticals", neutralYellow) + " by " + ReturnColoredText("25%", blueNumber) + ".";
        }
        else if (ts == TalentSchool.Pyromania)
        {
            sReturned += "Increase damage dealt against targets with " +
                 ReturnColoredText("Burning", neutralYellow) + " by " + ReturnColoredText("5%", blueNumber)
                 + ". Bonus is doubled if you are also " + ReturnColoredText("Burning", neutralYellow) + ".";
        }
        else if (ts == TalentSchool.Naturalism)
        {
            sReturned += "While " + ReturnColoredText("Overloaded", neutralYellow) + ", increase all damage by " + ReturnColoredText("5%", blueNumber) + ".";
        }
        else if (ts == TalentSchool.Divinity)
        {
            sReturned += "On first activation start, add a random " +
                ReturnColoredText("Blessing", neutralYellow) + " card to your hand.";
        }
        else if (ts == TalentSchool.Shadowcraft)
        {
            sReturned += "Increase damage dealt against targets with " +
                 ReturnColoredText("Weakened", neutralYellow) + " by " + ReturnColoredText("5%", blueNumber) + ".";
        }
        else if (ts == TalentSchool.Manipulation)
        {
            sReturned += "On first activation start, add an " +
                 ReturnColoredText("Arcane Bolt", neutralYellow) + " card to your hand.";
        }
        else if (ts == TalentSchool.Ranger)
        {
            sReturned += ReturnColoredText("Ranged Attack", neutralYellow) + " cards deal " +
                 ReturnColoredText("10%", blueNumber) + " more damage.";
        }


        return sReturned;
    }
    public static string GetTalentPairingTierTwoDescriptionText(TalentSchool ts)
    {
        string sReturned = "";
        if(ts == TalentSchool.Corruption)
        {
            sReturned += "Increase damage dealt against targets with " +
                ReturnColoredText("Poisoned", neutralYellow) + " by " + ReturnColoredText("10%" , blueNumber) +".";
        }
        else if (ts == TalentSchool.Warfare)
        {
            sReturned += ReturnColoredText("Melee Attack", neutralYellow) + " cards deal "+
                ReturnColoredText("20%", blueNumber) + " more damage.";
        }
        else if (ts == TalentSchool.Guardian)
        {
            sReturned += "Reduce all damage from enemy actions by " + ReturnColoredText("20%", blueNumber) +
                 ".";
        }
        else if (ts == TalentSchool.Scoundrel)
        {
            sReturned = "Increase the damage and " + ReturnColoredText("Block", neutralYellow) + " gained from " +
               ReturnColoredText("Criticals", neutralYellow) + " by " + ReturnColoredText("50%", blueNumber) + ".";
        }
        else if (ts == TalentSchool.Pyromania)
        {
            sReturned += "Increase damage dealt against targets with " +
                 ReturnColoredText("Burning", neutralYellow) + " by " + ReturnColoredText("10%", blueNumber)
                 + ". Bonus is doubled if you are also " + ReturnColoredText("Burning", neutralYellow) + ".";
        }
        else if (ts == TalentSchool.Naturalism)
        {
            sReturned += "While " + ReturnColoredText("Overloaded", neutralYellow) +", increase all damage by " + ReturnColoredText("10%", blueNumber) +".";
        }
        else if (ts == TalentSchool.Divinity)
        {
            sReturned += "On first activation start, add 2 random " +
                ReturnColoredText("Blessing", neutralYellow) + " cards to your hand.";
        }
        else if (ts == TalentSchool.Shadowcraft)
        {
            sReturned += "Increase damage dealt against targets with " +
                 ReturnColoredText("Weakened", neutralYellow) + " by " + ReturnColoredText("10%", blueNumber) + ".";
        }
        else if (ts == TalentSchool.Manipulation)
        {
            sReturned += "On first activation start, add 2 " +
                 ReturnColoredText("Arcane Bolt", neutralYellow) + " cards to your hand.";
        }
        else if (ts == TalentSchool.Ranger)
        {
            sReturned += ReturnColoredText("Ranged Attack", neutralYellow) + " cards deal " +
                 ReturnColoredText("20%", blueNumber) + " more damage.";
        }

        return sReturned;
    }
    #endregion
}
