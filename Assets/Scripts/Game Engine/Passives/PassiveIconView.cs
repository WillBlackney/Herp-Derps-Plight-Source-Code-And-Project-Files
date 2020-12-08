using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class PassiveIconView : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Properties")]
    [HideInInspector] public PassiveIconData myIconData;
    [HideInInspector] public string statusName;
    [HideInInspector] public int statusStacks;
    [SerializeField] private Image glowOutline;

    [Header("Component References")]
    public TextMeshProUGUI statusStacksText;
    public Image passiveImage;
    #endregion

    // Input 
    #region
    public void OnMouseEnter()
    {
        KeyWordLayoutController.Instance.BuildAllViewsFromPassiveString(myIconData.passiveName);
        AudioManager.Instance.PlaySoundPooled(Sound.GUI_Button_Mouse_Over);
       // glowOutline.gameObject.SetActive(true);
        //glowOutline.DOFade(1, 0f);

    }
    public void OnMouseExit()
    {
        KeyWordLayoutController.Instance.FadeOutMainView();
        //glowOutline.DOFade(0f, 0f);
       // glowOutline.gameObject.SetActive(false);
    }
    #endregion

}
