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

    public GameObject flawlessParent;
    public TextMeshProUGUI flawlessAmountText;

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
public class XpRewardData
{
    public CharacterData characterRef;
    public int totalXpGained;
    public int combatXpGained;
    public bool flawless = false;
    public CombatDifficulty encounterType;

    public XpRewardData(CharacterData data, int _totalXpGained, int _combatXpGained, bool _flawless, CombatDifficulty _encounterType)
    {
        characterRef = data;
        encounterType = _encounterType;
        totalXpGained = _totalXpGained;
        combatXpGained = _combatXpGained;
        flawless = _flawless;
    }
}
