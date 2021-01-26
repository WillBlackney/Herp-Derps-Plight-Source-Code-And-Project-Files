using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterPanelView : MonoBehaviour
{
    [Header("Core Components")]
    public GameObject visualParent;
    public UniversalCharacterModel ucm;
    public TextMeshProUGUI nameText;
   

    [Header("Health Bar Components")]
    public TextMeshProUGUI currentHealthText;
    public TextMeshProUGUI maxHealthText;
    public Slider healthBar;

    [Header("XP Bar Components")]
    public TextMeshProUGUI levelText;
    public Slider xpBar;

}
