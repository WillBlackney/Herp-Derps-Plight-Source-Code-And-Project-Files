using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RewardCharacterBox : MonoBehaviour
{
    public GameObject visualParent;
    public UniversalCharacterModel ucm;

    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI totalXpText;
    public TextMeshProUGUI combatTypeText;
    public TextMeshProUGUI combatTypeRewardText;

    public Slider xpBar;


}
public class PreviousXpState
{
    public CharacterData characterRef;
    public int previousXp;
    public int previousLevel;
    public int previousMaxXp;

    public PreviousXpState(CharacterData data)
    {
        characterRef = data;
        previousXp = data.currentXP;
        previousLevel = data.currentLevel;
        previousMaxXp = data.currentMaxXP;
    }
}
