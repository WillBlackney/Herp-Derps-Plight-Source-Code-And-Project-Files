using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CharacterPanelView : MonoBehaviour, IPointerClickHandler
{
    [Header("Properties")]
    [HideInInspector] public CharacterData characterDataRef;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        CharacterPanelViewController.Instance.OnCharacterPanelViewClicked(this);
    }
}
