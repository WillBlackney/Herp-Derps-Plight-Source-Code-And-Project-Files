using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecruitCharacterTab : MonoBehaviour
{
    [Header("Properties")]
    [HideInInspector] public CharacterData characterDataRef;

    [Header("Components")]
    public UniversalCharacterModel ucm;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI raceAndClassText;
}
