using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PassiveIconView : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Properties")]
    [HideInInspector] public PassiveIconDataSO myIconData;
    [HideInInspector] public string statusName;
    [HideInInspector] public int statusStacks;

    [Header("Component References")]
    public TextMeshProUGUI statusStacksText;
    public Image passiveImage;
    #endregion   

}
