using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GUIWidgetController : Singleton<GUIWidgetController>
{
    public void HandleWidgetEvents(GUIWidget widget, GUIWidgetEventData[] wEvents)
    {
        for(int i = 0; i < wEvents.Length; i++)
        {
            StartCoroutine(HandleWidgetEvent(widget, wEvents[i]));
        }
    }
    private IEnumerator HandleWidgetEvent(GUIWidget widget, GUIWidgetEventData wEvent)
    {
        // Wait for start delay
        if (wEvent.enableStartDelay)
        {
            yield return new WaitForSeconds(wEvent.startDelay);

            // Cancel if the pointer needs to be held over the
            // object, and the user has moved their mouse off the widget
            if(wEvent.onlyIfMouseIsStillOverMe &&
                (widget.pointerIsOverMe == false || ((Time.realtimeSinceStartup - widget.timeSinceLastPointerEnter) < wEvent.startDelay) ))
            {
                yield break;
            }
        }        

        if(wEvent.widgetEvent == WidgetEvent.EnableGameObject)
        {
            wEvent.objectEnabled.SetActive(true);
        }
        else if (wEvent.widgetEvent == WidgetEvent.DisableGameObject)
        {
            wEvent.objectDisabled.SetActive(false);
        }
        else if (wEvent.widgetEvent == WidgetEvent.InvokeFunction)
        {
            wEvent.functionInvoked.Invoke();
        }
        else if (wEvent.widgetEvent == WidgetEvent.PlaySound)
        {
            AudioManager.Instance.PlaySound(wEvent.soundPlayed);
        }
        else if (wEvent.widgetEvent == WidgetEvent.FadeInCanvasGroup)
        {
            wEvent.canvasGroup.DOKill();
            wEvent.canvasGroup.alpha = 0;
            wEvent.canvasGroup.DOFade(1, wEvent.fadeSpeed);
        }
        else if (wEvent.widgetEvent == WidgetEvent.FadeOutCanvasGroup)
        {
            wEvent.canvasGroup.DOKill();
            wEvent.canvasGroup.alpha = 1;
            wEvent.canvasGroup.DOFade(0, wEvent.fadeSpeed);
        }
        else if (wEvent.widgetEvent == WidgetEvent.FadeInImage)
        {
            wEvent.image.DOKill();
            wEvent.image.DOFade(0, 0);
            wEvent.image.DOFade(1, wEvent.fadeSpeed);
        }
        else if (wEvent.widgetEvent == WidgetEvent.FadeOutImage)
        {
            wEvent.image.DOKill();
            wEvent.image.DOFade(1, 0);
            wEvent.image.DOFade(0, wEvent.fadeSpeed);
        }

        else if (wEvent.widgetEvent == WidgetEvent.TransisitionImageColour)
        {
            wEvent.image.DOKill();
            wEvent.image.DOColor(wEvent.endColour, wEvent.fadeSpeed);
        }

        else if (wEvent.widgetEvent == WidgetEvent.TransistionTextColour)
        {
            wEvent.text.DOKill();
            wEvent.text.DOColor(wEvent.endColour, wEvent.fadeSpeed);
        }
    }
}

[Serializable]
public class GUIWidgetEventData
{
    [Header("Core Event Properties")]
    public WidgetEvent widgetEvent;

    [ShowIf("ShowDelayProperties")]
    public bool enableStartDelay;

    [ShowIf("enableStartDelay", true)]
    public float startDelay;

    [ShowIf("enableStartDelay", true)]
    public bool onlyIfMouseIsStillOverMe;

    [ShowIf("widgetEvent", WidgetEvent.DisableGameObject)]
    public GameObject objectDisabled;

    [ShowIf("widgetEvent", WidgetEvent.EnableGameObject)]
    public GameObject objectEnabled;

    [ShowIf("widgetEvent", WidgetEvent.InvokeFunction)]
    public UnityEvent functionInvoked;

    [ShowIf("widgetEvent", WidgetEvent.PlaySound)]
    public Sound soundPlayed;

    [ShowIf("ShowCanvasField")]
    public CanvasGroup canvasGroup;

    [ShowIf("ShowImageField")]
    public Image image;

    [ShowIf("widgetEvent", WidgetEvent.TransistionTextColour)]
    public TextMeshProUGUI text;

    [ShowIf("ShowEndColour")]
    public Color endColour;

    [ShowIf("ShowFadeSpeed")]
    public float fadeSpeed;

    public bool ShowDelayProperties()
    {
        return widgetEvent == WidgetEvent.EnableGameObject || 
            widgetEvent == WidgetEvent.FadeInCanvasGroup || 
            widgetEvent == WidgetEvent.FadeInImage;
    }
    public bool ShowEndColour()
    {
        return widgetEvent == WidgetEvent.TransistionTextColour || widgetEvent == WidgetEvent.TransisitionImageColour;
    }
    public bool ShowCanvasField()
    {
        return widgetEvent == WidgetEvent.FadeInCanvasGroup || widgetEvent == WidgetEvent.FadeOutCanvasGroup;
    }
    public bool ShowImageField()
    {
        return widgetEvent == WidgetEvent.FadeInImage || 
            widgetEvent == WidgetEvent.FadeOutImage ||
            widgetEvent == WidgetEvent.TransisitionImageColour; 
    }
    public bool ShowFadeSpeed()
    {
        return widgetEvent == WidgetEvent.FadeInCanvasGroup || 
               widgetEvent == WidgetEvent.FadeOutCanvasGroup ||
               widgetEvent == WidgetEvent.FadeInImage || 
               widgetEvent == WidgetEvent.FadeOutImage ||
               widgetEvent == WidgetEvent.TransisitionImageColour ||
               widgetEvent == WidgetEvent.TransistionTextColour;
    }
}
public enum WidgetEvent
{
    None = 0,
    EnableGameObject = 1,
    DisableGameObject = 2,
    InvokeFunction = 3,
    PlaySound = 4,
    FadeInCanvasGroup = 5,
    FadeOutCanvasGroup = 6,
    FadeInImage = 7,
    FadeOutImage = 8,
    TransisitionImageColour = 9,
    TransistionTextColour = 10,
}

public enum WidgetInputType
{
    IPointer = 0,
    Collider = 1,
}
