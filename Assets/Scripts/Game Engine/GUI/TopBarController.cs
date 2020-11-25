using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TopBarController : Singleton<TopBarController>
{
    [SerializeField] private TextMeshProUGUI currentGoldText;
    [SerializeField] private GameObject goldTopBarImage;

    [SerializeField] private TextMeshProUGUI currentEncounterText;
    [SerializeField] private TextMeshProUGUI maxEncounterText;

    [SerializeField] private GameObject characterRosterButton;

    public TextMeshProUGUI CurrentGoldText
    {
        get { return currentGoldText; }
        set { currentGoldText = value; }
    }
    public GameObject GoldTopBarImage
    {
        get { return goldTopBarImage; }
        set { goldTopBarImage = value; }
    }

    public TextMeshProUGUI CurrentEncounterText
    {
        get { return currentEncounterText; }
        set { currentEncounterText = value; }
    }
    public TextMeshProUGUI MaxEncounterText
    {
        get { return maxEncounterText; }
        set { maxEncounterText = value; }
    }

    public GameObject CharacterRosterButton
    {
        get { return characterRosterButton; }
        set { characterRosterButton = value; }
    }
}
