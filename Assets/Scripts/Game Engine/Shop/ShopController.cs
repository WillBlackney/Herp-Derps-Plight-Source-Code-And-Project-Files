using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spriter2UnityDX;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class ShopController : Singleton<ShopController>
{
    // Properties + Components
    #region
    [Header("Main View Components")]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] private GameObject mainVisualParent;
    [SerializeField] private CanvasGroup mainCg;

    [Header("Character View References")]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] private CampSiteCharacterView[] allShopCharacterViews;
    [SerializeField] private GameObject charactersViewParent;

    [Header("Nodes + Location References")]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] private CampSiteNode[] shopNodes;
    [SerializeField] private Transform offScreenStartPosition;

    [Header("Merchant Properties + Components")]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] private UniversalCharacterModel merchantUcm;
    [SerializeField] private List<string> merchantBodyParts;
    [SerializeField] private Image[] merchantAccesoryImages;
    [SerializeField] private Color merchantAccessoriesNormalColour;
    [SerializeField] private Color merchantNormalColour;
    [SerializeField] private Color merchantHightlightColour;    

    [Header("Speech Bubble Components")]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] private CanvasGroup bubbleCg;
    [SerializeField] private TextMeshProUGUI bubbleText;
    [SerializeField] private string[] bubbleGreetings;
    [SerializeField] private string[] bubbleFarewells;
    #endregion

    private void ShowMainPanelView()
    {
        mainCg.DOKill();
        mainCg.alpha = 0;
        mainVisualParent.SetActive(true);      
        mainCg.DOFade(1f, 0.25f);
    }

    private void HideMainPanelView()
    {
        mainCg.DOKill();
        mainVisualParent.SetActive(true);
        Sequence s = DOTween.Sequence();
        s.Append(mainCg.DOFade(0, 0.25f));
        s.OnComplete(() => mainVisualParent.SetActive(false));
    }
    public void SetMerchantColor(EntityRenderer view, Color newColor)
    {
        if (view != null)
        {
            view.Color = new Color(newColor.r, newColor.g, newColor.b, view.Color.a);
        }
    }
    public void SetMerchantAccessoriesColor(Color newColor)
    {
        foreach(Image i in merchantAccesoryImages)
        {
            i.DOKill();
            i.DOColor(newColor, 0.2f);
        }
    }
    public void OnMerchantCharacterClicked()
    {
        Debug.LogWarning("OnMerchantCharacterClicked()");
        if(mainVisualParent.activeSelf == false)
        {
            SetMerchantColor(merchantUcm.GetComponent<EntityRenderer>(), merchantNormalColour);
            SetMerchantAccessoriesColor(merchantNormalColour);
            AudioManager.Instance.PlaySound(Sound.GUI_Button_Clicked);
            ShowMainPanelView();
        }   
    }
    public void OnMerchantCharacterMouseEnter()
    {
        if(mainVisualParent.activeSelf == false)
        {
            AudioManager.Instance.PlaySound(Sound.GUI_Button_Mouse_Over);
            SetMerchantColor(merchantUcm.GetComponent<EntityRenderer>(), merchantHightlightColour);
            SetMerchantAccessoriesColor(merchantHightlightColour);
        }
    }
    public void OnMerchantCharacterMouseExit()
    {
        if (mainVisualParent.activeSelf == false)
        {
            SetMerchantColor(merchantUcm.GetComponent<EntityRenderer>(), merchantNormalColour);
            SetMerchantAccessoriesColor(merchantAccessoriesNormalColour);
        }
    }
    public void OnMainPanelBackButtonClicked()
    {
        HideMainPanelView();
    }

    public void BuildAllShopCharacterViews(List<CharacterData> characters)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            BuildShopCharacterView(allShopCharacterViews[i], characters[i]);
        }

        CharacterModelController.BuildModelFromStringReferences(merchantUcm, merchantBodyParts);
    }
    private void BuildShopCharacterView(CampSiteCharacterView view, CharacterData data)
    {
        Debug.LogWarning("CampSiteController.BuildShopCharacterView() called, character data: " + data.myName);

        // Connect data to view
        view.myCharacterData = data;

        // Enable shadow
        view.ucmShadowParent.SetActive(true);

        // Build UCM
        CharacterModelController.BuildModelFromStringReferences(view.characterEntityView.ucm, data.modelParts);
        CharacterModelController.ApplyItemManagerDataToCharacterModelView(data.itemManager, view.characterEntityView.ucm);
    }
    public void MoveAllCharactersToStartPosition()
    {
        for (int i = 0; i < allShopCharacterViews.Length; i++)
        {
            CampSiteCharacterView character = allShopCharacterViews[i];

            // Cancel if data ref is null (happens if player has less then 3 characters
            if (character.myCharacterData == null)
            {
                return;
            }

            character.characterEntityView.ucmMovementParent.transform.position = offScreenStartPosition.position;

            /*
            // Dead characters dont walk on screen, they start at their node
            if (character.myCharacterData.health <= 0)
            {

                //character.characterEntityView.ucmMovementParent.transform.position = campSiteNodes[i].transform.position;
               // CharacterEntityController.Instance.PlayLayDeadAnimation(character.characterEntityView);
            }

            // Alive characters start off screen, then walk to their node on event start
            else
            {
                character.characterEntityView.ucmMovementParent.transform.position = offScreenStartPosition.position;
            }
            */
        }
    }
    public void MoveAllCharactersToTheirNodes()
    {
        StartCoroutine(MoveAllCharactersToTheirNodesCoroutine());
    }
    private IEnumerator MoveAllCharactersToTheirNodesCoroutine()
    {
        for (int i = 0; i < allShopCharacterViews.Length; i++)
        {
            CampSiteCharacterView character = allShopCharacterViews[i];

            // Normal move to node
            if (character.myCharacterData != null && character.myCharacterData.health > 0)
            {
                // replace this with new move function in future, this function will make characters run through the camp fire
                CharacterEntityController.Instance.MoveEntityToNodeCentre(character.characterEntityView, shopNodes[i].LevelNode, null);
            }

            yield return new WaitForSeconds(0.25f);
        }

    }
    public void EnableCharacterViewParent()
    {
        charactersViewParent.SetActive(true);
    }

    // Greeting Visual Logic
    #region
    public void DoMerchantGreeting()
    {
        StartCoroutine(DoMerchantGreetingCoroutine());
    }
    private IEnumerator DoMerchantGreetingCoroutine()
    {
        bubbleCg.DOKill();
        bubbleText.text = bubbleGreetings[RandomGenerator.NumberBetween(0, bubbleGreetings.Length - 1)];
        bubbleCg.DOFade(1, 0.5f);
        yield return new WaitForSeconds(3.5f);
        bubbleCg.DOFade(0, 0.5f);
    }
    public void DoMerchantFarewell()
    {
        StartCoroutine(DoMerchantFarewellCoroutine());
    }
    private IEnumerator DoMerchantFarewellCoroutine()
    {
        bubbleCg.DOKill();
        bubbleText.text = bubbleFarewells[RandomGenerator.NumberBetween(0, bubbleFarewells.Length - 1)];
        bubbleCg.DOFade(1, 0.5f);
        yield return new WaitForSeconds(1.75f);
        bubbleCg.DOFade(0, 0.5f);
    }
    #endregion
}
