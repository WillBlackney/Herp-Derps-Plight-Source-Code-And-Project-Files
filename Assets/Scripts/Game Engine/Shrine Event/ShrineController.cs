using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineController : Singleton<ShrineController>
{
    // Properties + Components
    #region
    private ShrineStateResult currentShrineStates;

    [Header("Character View References")]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] private CampSiteCharacterView[] allShrineCharacterViews;
    [SerializeField] private GameObject charactersViewParent;

    [Header("Nodes + Location References")]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] private CampSiteNode[] shrineNodes;
    [SerializeField] private Transform offScreenStartPosition;
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
    public ShrineStateResult GenerateNewShrineContentData()
    {
        ShrineStateResult scr = new ShrineStateResult();

        // Generate cards
        // TO DO: randomly pick 3 states to offer the player

        return scr;

    }
    #endregion

    // View Logic
    #region
    public void EnableCharacterViewParent()
    {
        charactersViewParent.SetActive(true);
    }
    public void DisableCharacterViewParent()
    {
        charactersViewParent.SetActive(false);
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