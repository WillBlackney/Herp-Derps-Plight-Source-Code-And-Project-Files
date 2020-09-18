﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;

public class ColorLibrary : Singleton<ColorLibrary>
{
    // Talent Colors
    #region
    [ColorFoldoutGroup("Talent Colors", 0f, 1f, 0f)]
    public Color neutralColor;

    [ColorFoldoutGroup("Talent Colors")]
    public Color brawlerColor;

    [ColorFoldoutGroup("Talent Colors")]
    public Color armsColor;

    [ColorFoldoutGroup("Talent Colors")]
    public Color guardianColor;

    [ColorFoldoutGroup("Talent Colors")]
    public Color pyromaniaColor;

    [ColorFoldoutGroup("Talent Colors")]
    public Color manipulationColor;

    [ColorFoldoutGroup("Talent Colors")]
    public Color rangerColor;

    [ColorFoldoutGroup("Talent Colors")]
    public Color shadowcraftColor;

    [ColorFoldoutGroup("Talent Colors")]
    public Color divinityColor;

    [ColorFoldoutGroup("Talent Colors")]
    public Color naturalismColor;

    [ColorFoldoutGroup("Talent Colors")]
    public Color corruptionColor;

    [ColorFoldoutGroup("Talent Colors")]
    public Color assassinationColor;

    [ColorFoldoutGroup("Talent Colors")]
    public Color duelistColor;
    #endregion

    // Rarity Colors
    #region
    [ColorFoldoutGroup("Rarity Colors")]
    public Color commonColor;

    [ColorFoldoutGroup("Rarity Colors")]
    public Color rareColor;

    [ColorFoldoutGroup("Rarity Colors")]
    public Color epicColor;

    [ColorFoldoutGroup("Rarity Colors")]
    public Color legendaryColor;
    #endregion

    // Logic
    #region
    public Color GetTalentColor(TalentSchool talent)
    {
        Color colorReturned = neutralColor;

        if(talent == TalentSchool.Arms)
        {
            colorReturned = armsColor;
        }
        else if (talent == TalentSchool.Brawler)
        {
            colorReturned = brawlerColor;
        }
        else if (talent == TalentSchool.Guardian)
        {
            colorReturned = guardianColor;
        }
        else if (talent == TalentSchool.Assassination)
        {
            colorReturned = assassinationColor;
        }
        else if (talent == TalentSchool.Duelist)
        {
            colorReturned = duelistColor;
        }
        else if (talent == TalentSchool.Ranger)
        {
            colorReturned = rangerColor;
        }
        else if (talent == TalentSchool.Pyromania)
        {
            colorReturned = pyromaniaColor;
        }
        else if (talent == TalentSchool.Naturalism)
        {
            colorReturned = naturalismColor;
        }
        else if (talent == TalentSchool.Shadowcraft)
        {
            colorReturned = shadowcraftColor;
        }
        else if (talent == TalentSchool.Corruption)
        {
            colorReturned = corruptionColor;
        }
        else if (talent == TalentSchool.Divinity)
        {
            colorReturned = divinityColor;
        }
        else if (talent == TalentSchool.Manipulation)
        {
            colorReturned = manipulationColor;
        }

        colorReturned = new Color(colorReturned.r, colorReturned.g, colorReturned.b, 0.66f);

        return colorReturned;
    }
    public Color GetRarityColor(Rarity rarity)
    {
        Color colorReturned = commonColor;

        if (rarity == Rarity.Common)
        {
            colorReturned = commonColor;
        }
        else if (rarity == Rarity.Rare)
        {
            colorReturned = rareColor;
        }
        else if (rarity == Rarity.Epic)
        {
            colorReturned = epicColor;
        }
        else if (rarity == Rarity.Legendary)
        {
            colorReturned = legendaryColor;
        }



        return colorReturned;
    }
    #endregion

}
