using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MapSystem;

public class ShrineController : Singleton<ShrineController>
{
    // Properties + Components
    #region
    private ShrineStateResult currentShrineStates;
    private bool shrineIsInteractable = false;

    [Header("Shrine Properties")]
    [SerializeField] private Color normalColor, mouseOverColor;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Shrine Components")]
    [SerializeField] private Image shrineImage;
    [SerializeField] private GameObject continueButtonParent;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Character View References")]   
    [SerializeField] private CampSiteCharacterView[] allShrineCharacterViews;
    [SerializeField] private GameObject allShrineViewsParent;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Nodes + Location References")]
    [SerializeField] private CampSiteNode[] shrineNodes;
    [SerializeField] private Transform offScreenStartPosition;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    #endregion

    // Accessors + Getters
    #region
    public ShrineStateResult CurrentShrineStates
    {
        get { return currentShrineStates; }
        private set { currentShrineStates = value; }
    }
    #endregion

    // Save + Load Logic
    #region
    public void BuildMyDataFromSaveFile(SaveGameData save)
    {
        save.currentShrineStates = CurrentShrineStates;
    }
    public void SaveMyDataToSaveFile(SaveGameData save)
    {
        CurrentShrineStates = save.currentShrineStates;
    }
    #endregion

    // Generate Shrine Content Logic
    #region
    public void SetAndCacheNewShrineContentDataSet()
    {
        CurrentShrineStates = GenerateNewShrineContentData();
    }
    private ShrineStateResult GenerateNewShrineContentData()
    {
        Debug.Log("ShrineController.GenerateNewShrineContentData() called...");

        ShrineStateResult scr = new ShrineStateResult();

        // Generate 3 states
        List<StateData> possibleStates = StateController.Instance.GetAllAvailableStates();

        for(int i = 0; i < 3; i++)
        {
            possibleStates.Shuffle();
            int index = RandomGenerator.NumberBetween(0, possibleStates.Count -1);
            scr.states.Add(possibleStates[index]);
            possibleStates.RemoveAt(index);
        }

        return scr;

    }
    #endregion

    // View Logic
    #region
    public void EnableAllViews()
    {
        allShrineViewsParent.SetActive(true);
    }
    public void DisableAllViews()
    {
        allShrineViewsParent.SetActive(false);
    }
    public void BuildAllShrineCharacterViews(List<CharacterData> characters)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            BuildShrineCharacterView(allShrineCharacterViews[i], characters[i]);
        }
    }
    private void BuildShrineCharacterView(CampSiteCharacterView view, CharacterData data)
    {
        Debug.LogWarning("ShrineController.BuildShopCharacterView() called, character data: " + data.myName);

        // Connect data to view
        view.myCharacterData = data;

        // Enable shadow
        view.ucmShadowParent.SetActive(true);

        // Build UCM
        CharacterModelController.Instance.BuildModelFromStringReferences(view.characterEntityView.ucm, data.modelParts);
        CharacterModelController.Instance.ApplyItemManagerDataToCharacterModelView(data.itemManager, view.characterEntityView.ucm);
    }
    public void OnContinueButtonClicked()
    {
        MapPlayerTracker.Instance.UnlockMap();
        MapView.Instance.OnWorldMapButtonClicked();
    }
    public void ShowContinueButton()
    {
        continueButtonParent.SetActive(true);
    }
    public void HideContinueButton()
    {
        continueButtonParent.SetActive(false);
    }
    #endregion

    // Shrine Object Logic
    #region
    public void SetShrineInteractivityState(bool onOrOff)
    {
        shrineIsInteractable = onOrOff;
    }
    public void OnShrineMouseEnter()
    {
        Debug.Log("mouse enter shrine");
        if (!shrineIsInteractable)
            return;

        AudioManager.Instance.PlaySound(Sound.GUI_Button_Mouse_Over);
        shrineImage.DOKill();      
        shrineImage.DOColor(mouseOverColor, 0.25f);
    }
    public void OnShrineMouseExit()
    {
        if (!shrineIsInteractable)
            return;

        shrineImage.DOKill();
        shrineImage.DOColor(normalColor, 0.25f);
    }
    public void OnShrineMouseClick()
    {
        if (!shrineIsInteractable)
            return;

        // TO DO: fancy on click shrine animation stuff

        ChooseStateWindowController.Instance.BuildAndShowWindowInShrineEvent(CurrentShrineStates.states);
        SetShrineInteractivityState(false);
    }
    #endregion

    // Model Movement Logic
    #region
    public void MoveAllCharactersToStartPosition()
    {
        for (int i = 0; i < allShrineCharacterViews.Length; i++)
        {
            CampSiteCharacterView character = allShrineCharacterViews[i];

            // Cancel if data ref is null (happens if player has less then 3 characters
            if (character.myCharacterData == null)
            {
                return;
            }

            character.characterEntityView.ucmMovementParent.transform.position = offScreenStartPosition.position;
        }
    }
    public void MoveAllCharactersToTheirNodes()
    {
        StartCoroutine(MoveAllCharactersToTheirNodesCoroutine());
    }
    private IEnumerator MoveAllCharactersToTheirNodesCoroutine()
    {
        for (int i = 0; i < allShrineCharacterViews.Length; i++)
        {
            CampSiteCharacterView character = allShrineCharacterViews[i];

            // Normal move to node
            if (character.myCharacterData != null && character.myCharacterData.health > 0)
            {
                // replace this with new move function in future, this function will make characters run through the camp fire
                CharacterEntityController.Instance.MoveEntityToNodeCentre(character.characterEntityView, shrineNodes[i].LevelNode, null);
            }

            yield return new WaitForSeconds(0.25f);
        }

    }
    public void MoveCharactersToOffScreenRight()
    {
        StartCoroutine(MoveCharactersToOffScreenRightCoroutine());
    }
    private IEnumerator MoveCharactersToOffScreenRightCoroutine()
    {
        foreach (CampSiteCharacterView character in allShrineCharacterViews)
        {
            // Only move existing living characters
            if (character.myCharacterData != null &&
                character.myCharacterData.health > 0)
            {
                CharacterEntityController.Instance.MoveEntityToNodeCentre(character.characterEntityView, LevelManager.Instance.EnemyOffScreenNode, null);
            }
        }

        yield return new WaitForSeconds(3f);
    }
    #endregion


}
public class ShrineStateResult
{
    public List<StateData> states = new List<StateData>();

}