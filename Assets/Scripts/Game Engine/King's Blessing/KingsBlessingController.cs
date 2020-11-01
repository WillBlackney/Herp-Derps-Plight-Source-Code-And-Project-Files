using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingsBlessingController : Singleton<KingsBlessingController>
{
    [Header("UCM References")]
    [SerializeField] private UniversalCharacterModel playerModel;
    [SerializeField] private UniversalCharacterModel kingModel;

    [Header("King Model References")]
    [SerializeField] private List<string> kingBodyParts = new List<string>();

    public void BuildAllViews(CharacterData startingCharacter)
    {
        Debug.LogWarning("KingsBlessingController.BuildAllViews()");

        // Build player model
        CharacterModelController.BuildModelFromStringReferences(playerModel, startingCharacter.modelParts);
        CharacterModelController.ApplyItemManagerDataToCharacterModelView(startingCharacter.itemManager, playerModel);

        // Build skeleton king model
        CharacterModelController.BuildModelFromStringReferences(kingModel, kingBodyParts);
    }
   
}
