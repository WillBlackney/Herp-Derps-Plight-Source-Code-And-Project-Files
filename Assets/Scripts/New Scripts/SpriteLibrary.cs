using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLibrary : MonoBehaviour
{
    // Singleton Pattern
    #region
    public static SpriteLibrary Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("Talent School Badges")]
    public Sprite duelistBadge;
    public Sprite brawlerBadge;
    public Sprite guardianBadge;
    public Sprite assassinationBadge;
    public Sprite rangerBadge;
    public Sprite pyromaniaBadge;
    public Sprite cyromancyBadge;
    public Sprite divinityBadge;
    public Sprite shadowCraftBadge;
    public Sprite corruptionBadge;
    public Sprite naturalismBadge;
    public Sprite manipulationBadge;

    public Sprite GetTalentSchoolSpriteFromEnumData(TalentSchool data)
    {
        Sprite spriteReturned = null;

        if(data == TalentSchool.Assassination)
        {
            spriteReturned = assassinationBadge;
        }
        else if (data == TalentSchool.Brawler)
        {
            spriteReturned = brawlerBadge;
        }
        else if (data == TalentSchool.Corruption)
        {
            spriteReturned = corruptionBadge;
        }
        else if (data == TalentSchool.Cyromancy)
        {
            spriteReturned = cyromancyBadge;
        }
        else if (data == TalentSchool.Divinity)
        {
            spriteReturned = divinityBadge;
        }
        else if (data == TalentSchool.Duelist)
        {
            spriteReturned = duelistBadge;
        }
        else if (data == TalentSchool.Guardian)
        {
            spriteReturned = guardianBadge;
        }
        else if (data == TalentSchool.Manipulation)
        {
            spriteReturned = manipulationBadge;
        }
        else if (data == TalentSchool.Naturalism)
        {
            spriteReturned = naturalismBadge;
        }
        else if (data == TalentSchool.Pyromania)
        {
            spriteReturned = pyromaniaBadge;
        }
        else if (data == TalentSchool.Shadowcraft)
        {
            spriteReturned = shadowCraftBadge;
        }

        return spriteReturned;
    }

}
