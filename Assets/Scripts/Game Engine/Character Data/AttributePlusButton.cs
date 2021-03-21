using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AttributePlusButton : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public Image buttonImage;

    [SerializeField] private CoreAttribute myAttribute;    
    [HideInInspector] public bool isPressed = false;
    public Color normalColor;
    public Color pressedColor;

    public CoreAttribute MyAttribute
    {
        get { return myAttribute; }
    }
    public void OnClick()
    {
        CharacterRosterViewController.Instance.OnAttributePlusButtonClicked(this);
    }



}
