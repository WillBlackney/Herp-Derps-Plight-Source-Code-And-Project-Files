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

    public static string physical = "<color=#BA8400>";
    public static string fire = "<color=#FF6637>";
    public static string frost = "<color=#3687FF>";
    public static string shadow = "<color=#CF01BC>";
    public static string air = "<color=#36EDFF>";
    public static string poison = "<color=#00EC4A>";
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
    private static string ReturnColoredText(string text, string color)
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
}
