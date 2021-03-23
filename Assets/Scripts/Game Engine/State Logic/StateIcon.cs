using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;

public class StateIcon : MonoBehaviour
{
    [Header("Properties")]
    [HideInInspector] public StateData myStateData;
    [SerializeField] private Color normalColour;
    [SerializeField] private Color highlightColour;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Core Components")]
    public GameObject visualParent;
    public Image stateImage;
    public Image frameImage;
    public GameObject greyOutParent;
    public CanvasGroup glowUnderlayCg;
    public GameObject stacksVisualParent;
    public TextMeshProUGUI stacksText;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Description Panel Components")]
    public GameObject infoPanelParent;
    public CanvasGroup infoPanelCg;
    public TextMeshProUGUI stateNameText;
    public TextMeshProUGUI infoPanelDescriptionText;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]


    // Input Listeners
    #region
    public void OnIconMouseEnter()
    {
        AudioManager.Instance.PlaySoundPooled(Sound.GUI_Button_Mouse_Over);

        if (myStateData != null)
        {
            KeyWordLayoutController.Instance.BuildAllViewsFromKeyWordModels(myStateData.keyWordModels);
        }

        // Visuals
        stateImage.DOKill();
        frameImage.DOKill();
        glowUnderlayCg.DOKill();

        infoPanelParent.SetActive(true);
        stateImage.DOColor(highlightColour, 0.2f);
        frameImage.DOColor(highlightColour, 0.2f);
        glowUnderlayCg.DOFade(1, 0.2f);

    }
    public void OnIconMouseExit()
    {
        infoPanelParent.SetActive(false);
        stateImage.DOColor(normalColour, 0.1f);
        frameImage.DOColor(normalColour, 0.1f);
        glowUnderlayCg.alpha = 0;
        KeyWordLayoutController.Instance.FadeOutMainView();
    }
    #endregion


}
