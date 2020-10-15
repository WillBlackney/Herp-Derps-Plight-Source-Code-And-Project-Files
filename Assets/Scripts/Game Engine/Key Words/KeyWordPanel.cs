using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyWordPanel : MonoBehaviour
{
    [Header("Text Components")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    [Header("Image Icon Components")]
    public Image panelImage;
    public GameObject panelImageParent;

    [Header("Transform + Layout Components")]
    public RectTransform rootLayoutRect;

    public void RebuildLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rootLayoutRect);        
    }
}
