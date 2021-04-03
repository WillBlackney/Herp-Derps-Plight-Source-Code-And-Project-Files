using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class StoryChoiceButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Properties")]
    [HideInInspector] public StoryEventChoiceSO myData;
    [HideInInspector] public bool locked;

    [Header("Components")]
    public Image buttonBG;
    public TextMeshProUGUI activityDescriptionText;
    [SerializeField] GameObject lockParent;

    // Input Listeners
    #region
    public void OnPointerClick(PointerEventData eventData)
    {
        StoryEventController.Instance.OnChoiceButtonClicked(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StoryEventController.Instance.OnChoiceButtonMouseEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StoryEventController.Instance.OnChoiceButtonMouseExit(this);
    }


    #endregion

    // Misc
    #region
    public void ShowLock()
    {
        lockParent.SetActive(true);
        lockParent.transform.DOKill();
        lockParent.transform.localScale = Vector3.one;
        lockParent.transform.DOScale(1.25f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    public void HideLock()
    {
        lockParent.transform.DOKill();
        lockParent.transform.localScale = Vector3.one;
        lockParent.SetActive(false);
    }
    #endregion
}
