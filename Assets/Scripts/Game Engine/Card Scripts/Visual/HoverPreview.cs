using UnityEngine;
using System.Collections;
using DG.Tweening;

public class HoverPreview: MonoBehaviour
{
    // Properties + Componet References
    #region
    // PUBLIC FIELDS
    public GameObject TurnThisOffWhenPreviewing;  // if this is null, will not turn off anything 
    public Vector3 TargetPosition;
    public float TargetScale;
    public GameObject previewGameObject;
    public bool ActivateInAwake = false;
    [SerializeField] private CardViewModel mainCardVM;
    [HideInInspector] public bool inChooseScreenTransistion;

    // PRIVATE FIELDS
    private static HoverPreview currentlyViewing = null;

    // PROPERTIES WITH UNDERLYING PRIVATE FIELDS
    private static bool _PreviewsAllowed = true;
    public static bool PreviewsAllowed
    {
        get { return _PreviewsAllowed;}

        set 
        { 
            Debug.Log("Hover Previews Allowed is now: " + value);
            _PreviewsAllowed= value;
            if (!_PreviewsAllowed)
                StopAllPreviews();
        }
    }

    private bool _thisPreviewEnabled = false;
    public bool ThisPreviewEnabled
    {
        get { return _thisPreviewEnabled;}

        set 
        { 
            _thisPreviewEnabled = value;
            if (!_thisPreviewEnabled )
                StopThisPreview();
        }
    }

    public bool OverCollider { get; set;}

    private bool touchFingerIsOverMe = false;
    #endregion

    // Life cycle
    #region
    void Awake()
    {
        ThisPreviewEnabled = ActivateInAwake;
    }
    #endregion

    // Input Hooks
    #region
    private void OnMouseOver()
    {
        // is mobile user touching screen
        if(GlobalSettings.Instance.deviceMode == DeviceMode.Mobile &&
            Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // did player take finger off the screen?
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // they did cancel the preview
                OverCollider = false;
                if (!PreviewingSomeCard())
                {
                    touchFingerIsOverMe = false;
                    StopAllPreviews();

                    // Close key word pop ups
                    KeyWordLayoutController.Instance.FadeOutMainView();
                }
            }

            // prevent clicking through an active UI screen
            else if (touchFingerIsOverMe == false &&
                PreviewsAllowed &&
                !MainMenuController.Instance.AnyMenuScreenIsActive() &&
                !CardController.Instance.DiscoveryScreenIsActive &&
                CardController.Instance.CurrentChooseCardScreenSelection != mainCardVM.card &&
                !inChooseScreenTransistion)
            {
                touchFingerIsOverMe = true;
                PreviewThisObject();
            }
        }
    }

    void OnMouseEnter()
    {
        Debug.Log("HoverPreview.OnMousEnter() called...");

        if (GlobalSettings.Instance.deviceMode == DeviceMode.Desktop)
        {
            OverCollider = true;

            // prevent clicking through an active UI screen
            if (PreviewsAllowed &&
                !MainMenuController.Instance.AnyMenuScreenIsActive() &&
                !CardController.Instance.DiscoveryScreenIsActive &&
                CardController.Instance.CurrentChooseCardScreenSelection != mainCardVM.card &&
                !inChooseScreenTransistion)
            {
                PreviewThisObject();
            }              
        }        

    }
        
    void OnMouseExit()
    {
        Debug.Log("HoverPreview.OnMouseExit() called...");

        if (GlobalSettings.Instance.deviceMode == DeviceMode.Desktop)
        {
            OverCollider = false;
            if (!PreviewingSomeCard())
            {
                StopAllPreviews();

                // Close key word pop ups
                KeyWordLayoutController.Instance.FadeOutMainView();
            }
        }

        else if (GlobalSettings.Instance.deviceMode == DeviceMode.Mobile)
        {
            OverCollider = false;
            if (!PreviewingSomeCard())
            {
                touchFingerIsOverMe = false;
                StopAllPreviews();
                // Close key word pop ups
                KeyWordLayoutController.Instance.FadeOutMainView();
            }
               
        }


    }
    #endregion

    // Misc Logic
    #region
    public void SetChooseCardScreenTransistionState(bool newState)
    {
        inChooseScreenTransistion = newState;
    }
    void PreviewThisObject()
    {
        Debug.Log("HoverPreview.PreviewThisObject() called...");
        // 1) clone this card 
        // first disable the previous preview if there is one already
        StopAllPreviews();
        // 2) save this HoverPreview as curent
        currentlyViewing = this;
        // 3) enable Preview game object
        previewGameObject.SetActive(true);
        // 4) disable if we have what to disable
        if (TurnThisOffWhenPreviewing!=null)
            TurnThisOffWhenPreviewing.SetActive(false);
        // 5) play sfx
        AudioManager.Instance.PlaySound(Sound.Card_Moused_Over);
        // 6) tween to target position
        previewGameObject.transform.localPosition = Vector3.zero;
        previewGameObject.transform.localScale = Vector3.one;

        Vector3 movePos = TargetPosition;
        float scaleAmount = TargetScale;

        if(GlobalSettings.Instance.deviceMode == DeviceMode.Mobile)
        {
            movePos = GlobalSettings.Instance.hoverMovePosition;
            scaleAmount = GlobalSettings.Instance.hoverScaleAmount;
        }

        previewGameObject.transform.DOLocalMove(movePos, 0.5f).SetEase(Ease.OutQuint);
        previewGameObject.transform.DOScale(scaleAmount, 0.5f).SetEase(Ease.OutQuint);

        // Key word pop logic should go here:
        if(mainCardVM != null &&
            mainCardVM.card != null)
        {
            CardController.Instance.AutoUpdateCardDescriptionText(mainCardVM.card);
            KeyWordLayoutController.Instance.BuildAllViewsFromKeyWordModels(mainCardVM.card.keyWordModels);
        }
       
    }
    void StopThisPreview()
    {
        Debug.Log("HoverPreview.StopThisPreview() called...");

        // Close preview
        previewGameObject.SetActive(false);
        previewGameObject.transform.localScale = Vector3.one;
        previewGameObject.transform.localPosition = Vector3.zero;
        if (TurnThisOffWhenPreviewing!=null)
            TurnThisOffWhenPreviewing.SetActive(true); 
    }

    // STATIC METHODS
    private static void StopAllPreviews()
    {
        if (currentlyViewing != null)
        {
            currentlyViewing.previewGameObject.SetActive(false);
            currentlyViewing.previewGameObject.transform.localScale = Vector3.one;
            currentlyViewing.previewGameObject.transform.localPosition = Vector3.zero;
            if (currentlyViewing.TurnThisOffWhenPreviewing!=null)
                currentlyViewing.TurnThisOffWhenPreviewing.SetActive(true); 
        }
         
    }
    private static bool PreviewingSomeCard()
    {
        if (!PreviewsAllowed)
            return false;

        HoverPreview[] allHoverBlowups = FindObjectsOfType<HoverPreview>();

        foreach (HoverPreview hb in allHoverBlowups)
        {
            if (hb.OverCollider && hb.ThisPreviewEnabled)
                return true;
        }

        return false;
    }
    #endregion

}
