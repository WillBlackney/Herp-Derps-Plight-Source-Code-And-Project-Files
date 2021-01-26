using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spriter2UnityDX;
using DG.Tweening;

public class CharacterModelController: Singleton<CharacterModelController>
{
    // View Logic
    #region
    public void BuildModelFromStringReferences(UniversalCharacterModel model, List<string> partNames)
    {
        DisableAllActiveModelElementViews(model);
        ClearAllActiveModelElementsReferences(model);

        foreach(string part in partNames)
        {
            EnableAndSetElementOnModel(model, part);
        }
    }
    public void BuildModelMugShotFromStringReferences(UniversalCharacterModel model, List<string> partNames)
    {
        DisableAllActiveModelElementViews(model);
        ClearAllActiveModelElementsReferences(model);

        foreach (string part in partNames)
        {
            EnableAndSetMugShotElementOnModel(model, part);
        }
    }
    public void BuildModelFromModelClone(UniversalCharacterModel modelToBuild, UniversalCharacterModel modelClonedFrom)
    {
        if(modelClonedFrom == null)
        {
            Debug.Log("CharacterModelController.BuildModelFromModelClone() was given a null UCM to clone from, returning...");
            return;
        }

        if (modelToBuild == null)
        {
            Debug.Log("CharacterModelController.BuildModelFromModelClone() was given a null UCM to build into, returning...");
            return;
        }

        DisableAllActiveModelElementViews(modelToBuild);
        ClearAllActiveModelElementsReferences(modelToBuild);

        if(modelClonedFrom.allModelElements.Length > 0)
        {
            for (int index = 0; index < modelClonedFrom.allModelElements.Length - 1; index++)
            {
                if (modelClonedFrom.allModelElements[index].gameObject.activeSelf)
                {
                    EnableAndSetElementOnModel(modelToBuild, modelToBuild.allModelElements[index]);
                }
            }
        }      
    }
    public void ApplyItemManagerDataToCharacterModelView(ItemManagerModel iManager, UniversalCharacterModel model)
    {
        // Do weapons first

        // MH
        if(iManager.mainHandItem != null)
        {
            foreach (UniversalCharacterModelElement ucme in model.allMainHandWeapons)
            {
                foreach(ItemDataSO itemData in ucme.itemsWithMyView)
                {
                    if (itemData.itemName == iManager.mainHandItem.itemName)
                    {
                        EnableAndSetElementOnModel(model, ucme);
                        break;
                    }
                }
            }
        }

        // Off hand
        if (iManager.offHandItem != null)
        {
            foreach (UniversalCharacterModelElement ucme in model.allOffHandWeapons)
            {
                foreach (ItemDataSO itemData in ucme.itemsWithMyView)
                {
                    if (itemData.itemName == iManager.offHandItem.itemName)
                    {
                        EnableAndSetElementOnModel(model, ucme);
                        break;
                    }
                }
            }            
        }
    }  
    public void ClearAllActiveModelElementsReferences(UniversalCharacterModel model)
    {
        ClearAllClothingPartReferences(model);
        ClearAllActiveBodyPartReferences(model);
    }
    public void ClearAllActiveBodyPartReferences(UniversalCharacterModel model)
    {
        // Body Parts
        model.activeHead = null;
        model.activeFace = null;
        model.activeLeftLeg = null;
        model.activeRightLeg = null;
        model.activeRightHand = null;
        model.activeRightArm = null;
        model.activeLeftHand = null;
        model.activeLeftArm = null;
        model.activeChest = null;
    }
    public void ClearAllClothingPartReferences(UniversalCharacterModel model)
    {
        // Clothing 
        model.activeHeadWear = null;
        model.activeChestWear = null;
        model.activeRightLegWear = null;
        model.activeLeftLegWear = null;
        model.activeRightArmWear = null;
        model.activeRightHandWear = null;
        model.activeLeftArmWear = null;
        model.activeLeftHandWear = null;

        // Weapons
        model.activeMainHandWeapon = null;
        model.activeOffHandWeapon = null;
    }
    public void DisableAllActiveModelElementViews(UniversalCharacterModel model)
    {
        // Body Parts
        if (model.activeHead)
        {
            model.activeHead.gameObject.SetActive(false);
        }
        if (model.activeFace)
        {
            model.activeFace.gameObject.SetActive(false);
        }
        if (model.activeLeftLeg)
        {
            model.activeLeftLeg.gameObject.SetActive(false);
        }
        if (model.activeRightLeg)
        {
            model.activeRightLeg.gameObject.SetActive(false);
        }
        if (model.activeRightHand)
        {
            model.activeRightHand.gameObject.SetActive(false);
        }
        if (model.activeRightArm)
        {
            model.activeRightArm.gameObject.SetActive(false);
        }
        if (model.activeLeftHand)
        {
            model.activeLeftHand.gameObject.SetActive(false);
        }
        if (model.activeLeftArm)
        {
            model.activeLeftArm.gameObject.SetActive(false);
        }
        if (model.activeChest)
        {
            model.activeChest.gameObject.SetActive(false);
        }

        // Clothing 
        if (model.activeHeadWear)
        {
            model.activeHeadWear.gameObject.SetActive(false);
        }
        if (model.activeChestWear)
        {
            model.activeChestWear.gameObject.SetActive(false);
        }
        if (model.activeRightLegWear)
        {
            model.activeRightLegWear.gameObject.SetActive(false);
        }
        if (model.activeLeftLegWear)
        {
            model.activeLeftLegWear.gameObject.SetActive(false);
        }
        if (model.activeRightArmWear)
        {
            model.activeRightArmWear.gameObject.SetActive(false);
        }
        if (model.activeRightHandWear)
        {
            model.activeRightHandWear.gameObject.SetActive(false);
        }
        if (model.activeLeftArmWear)
        {
            model.activeLeftArmWear.gameObject.SetActive(false);
        }
        if (model.activeLeftHandWear)
        {
            model.activeLeftHandWear.gameObject.SetActive(false);
        }

        // Weapons
        if (model.activeMainHandWeapon)
        {
            model.activeMainHandWeapon.gameObject.SetActive(false);
        }
        if (model.activeOffHandWeapon)
        {
            model.activeOffHandWeapon.gameObject.SetActive(false);
        }
    }   
    public void AutoSetHeadMaskOrderInLayer(UniversalCharacterModel model)
    {
        int headSortOrder = model.myEntityRenderer.SortingOrder + 10;

        foreach(SpriteMask mask in model.allHeadWearSpriteMasks)
        {
            mask.frontSortingOrder = headSortOrder + 1;
            mask.backSortingOrder = headSortOrder - 1;
        }
    }
    #endregion

    // Set Specific Body Parts
    #region
    public void EnableAndSetElementOnModel(UniversalCharacterModel model, UniversalCharacterModelElement element)
    {
        // Set Active Body Part Reference
        if (element.bodyPartType == BodyPartType.Chest)
        {
            if (model.activeChest != null)
            {
                DisableAndClearElementOnModel(model, model.activeChest);
            }            
            model.activeChest = element;
        }
        else if (element.bodyPartType == BodyPartType.Head)
        {
            if (model.activeHead != null)
            {
                DisableAndClearElementOnModel(model, model.activeHead);
            }
            model.activeHead = element;
        }
        else if (element.bodyPartType == BodyPartType.Face)
        {
            if (model.activeFace != null)
            {
                DisableAndClearElementOnModel(model, model.activeFace);
            }
            model.activeFace = element;
        }
        else if (element.bodyPartType == BodyPartType.RightArm)
        {
            if (model.activeRightArm != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightArm);
            }
            model.activeRightArm = element;
        }
        else if (element.bodyPartType == BodyPartType.RightHand)
        {
            if (model.activeRightHand != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightHand);
            }
            model.activeRightHand = element;
        }
        else if (element.bodyPartType == BodyPartType.LeftArm)
        {
            if (model.activeLeftArm != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftArm);
            }
            model.activeLeftArm = element;
        }
        else if (element.bodyPartType == BodyPartType.LeftHand)
        {
            if (model.activeLeftHand != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftHand);
            }
            model.activeLeftHand = element;
        }
        else if (element.bodyPartType == BodyPartType.RightLeg)
        {
            if (model.activeRightLeg != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightLeg);
            }
            model.activeRightLeg = element;
        }
        else if (element.bodyPartType == BodyPartType.LeftLeg)
        {
            if (model.activeLeftLeg != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftLeg);
            }
            model.activeLeftLeg = element;
        }

        // Set Active Weapons + Clothing Reference
        else if (element.bodyPartType == BodyPartType.HeadWear)
        {
            if (model.activeHeadWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeHeadWear);
            }
            model.activeHeadWear = element;
        }
        else if (element.bodyPartType == BodyPartType.ChestWear)
        {
            if (model.activeChestWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeChestWear);
            }
            model.activeChestWear = element;
        }
        else if (element.bodyPartType == BodyPartType.LeftLegWear)
        {
            if (model.activeLeftLegWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftLegWear);
            }
            model.activeLeftLegWear = element;
        }
        else if (element.bodyPartType == BodyPartType.RightLegWear)
        {
            if (model.activeRightLegWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightLegWear);
            }
            model.activeRightLegWear = element;
        }
        else if (element.bodyPartType == BodyPartType.LeftArmWear)
        {
            if (model.activeLeftArmWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftArmWear);
            }
            model.activeLeftArmWear = element;
        }
        else if (element.bodyPartType == BodyPartType.RightArmWear)
        {
            if (model.activeRightArmWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightArmWear);
            }
            model.activeRightArmWear = element;
        }
        else if (element.bodyPartType == BodyPartType.LeftHandWear)
        {
            if (model.activeLeftHandWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftHandWear);
            }
            model.activeLeftHandWear = element;
        }
        else if (element.bodyPartType == BodyPartType.RightHandWear)
        {
            if (model.activeRightHandWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightHandWear);
            }
            model.activeRightHandWear = element;
        }
        else if (element.bodyPartType == BodyPartType.MainHandWeapon)
        {
            if (model.activeMainHandWeapon != null)
            {
                DisableAndClearElementOnModel(model, model.activeMainHandWeapon);
            }
            model.activeMainHandWeapon = element;
        }
        else if (element.bodyPartType == BodyPartType.OffHandWeapon)
        {
            if (model.activeOffHandWeapon != null)
            {
                DisableAndClearElementOnModel(model, model.activeOffHandWeapon);
            }
            model.activeOffHandWeapon = element;
        }

        // Enable GO
        element.gameObject.SetActive(true);

        // repeat for any connected elements (e.g. active arm/hand sprites that are connected to the chest piece
        foreach(UniversalCharacterModelElement connectedElement in element.connectedElements)
        {
            if(connectedElement == element)
            {
                Debug.Log("CharacterModelController.EnableAndSetElementOnModel() detected that the element " + element.gameObject.name +
                    " has a copy of itself in its connected elements lst, enabling this will cause an infinite loop, cancelling...");
            }
            else
            {
                EnableAndSetElementOnModel(model, connectedElement);
            }            
        }
    }
    public void EnableAndSetElementOnModel(UniversalCharacterModel model, string elementName)
    {
        UniversalCharacterModelElement element = null;

        // find element first
        foreach(UniversalCharacterModelElement modelElement in model.allModelElements)
        {
            if(modelElement.gameObject.name == elementName)
            {
                element = modelElement;
                break;
            }
        }

        if(element == null)
        {
            Debug.Log("CharacterModelController.EnableAndSetElementOnModel() could not find an model element with the name "
            + elementName + ", cancelling element enabling...");
            return;
        }

        // Set Active Body Part Reference
        if (element.bodyPartType == BodyPartType.Chest)
        {
            if (model.activeChest != null)
            {
                DisableAndClearElementOnModel(model, model.activeChest);
            }
            model.activeChest = element;
        }
        else if (element.bodyPartType == BodyPartType.Head)
        {
            if (model.activeHead != null)
            {
                DisableAndClearElementOnModel(model, model.activeHead);
            }
            model.activeHead = element;
        }
        else if (element.bodyPartType == BodyPartType.Face)
        {
            if (model.activeFace != null)
            {
                DisableAndClearElementOnModel(model, model.activeFace);
            }
            model.activeFace = element;
        }
        else if (element.bodyPartType == BodyPartType.RightArm)
        {
            if (model.activeRightArm != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightArm);
            }
            model.activeRightArm = element;
        }
        else if (element.bodyPartType == BodyPartType.RightHand)
        {
            if (model.activeRightHand != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightHand);
            }
            model.activeRightHand = element;
        }
        else if (element.bodyPartType == BodyPartType.LeftArm)
        {
            if (model.activeLeftArm != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftArm);
            }
            model.activeLeftArm = element;
        }
        else if (element.bodyPartType == BodyPartType.LeftHand)
        {
            if (model.activeLeftHand != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftHand);
            }
            model.activeLeftHand = element;
        }
        else if (element.bodyPartType == BodyPartType.RightLeg)
        {
            if (model.activeRightLeg != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightLeg);
            }
            model.activeRightLeg = element;
        }
        else if (element.bodyPartType == BodyPartType.LeftLeg)
        {
            if (model.activeLeftLeg != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftLeg);
            }
            model.activeLeftLeg = element;
        }

        // Set Active Weapons + Clothing Reference
        else if (element.bodyPartType == BodyPartType.HeadWear)
        {
            if (model.activeHeadWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeHeadWear);
            }
            model.activeHeadWear = element;
        }
        else if (element.bodyPartType == BodyPartType.ChestWear)
        {
            if (model.activeChestWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeChestWear);
            }
            model.activeChestWear = element;
        }
        else if (element.bodyPartType == BodyPartType.LeftLegWear)
        {
            if (model.activeLeftLegWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftLegWear);
            }
            model.activeLeftLegWear = element;
        }
        else if (element.bodyPartType == BodyPartType.RightLegWear)
        {
            if (model.activeRightLegWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightLegWear);
            }
            model.activeRightLegWear = element;
        }
        else if (element.bodyPartType == BodyPartType.LeftArmWear)
        {
            if (model.activeLeftArmWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftArmWear);
            }
            model.activeLeftArmWear = element;
        }
        else if (element.bodyPartType == BodyPartType.RightArmWear)
        {
            if (model.activeRightArmWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightArmWear);
            }
            model.activeRightArmWear = element;
        }
        else if (element.bodyPartType == BodyPartType.LeftHandWear)
        {
            if (model.activeLeftHandWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftHandWear);
            }
            model.activeLeftHandWear = element;
        }
        else if (element.bodyPartType == BodyPartType.RightHandWear)
        {
            if (model.activeRightHandWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightHandWear);
            }
            model.activeRightHandWear = element;
        }
        else if (element.bodyPartType == BodyPartType.MainHandWeapon)
        {
            if (model.activeMainHandWeapon != null)
            {
                DisableAndClearElementOnModel(model, model.activeMainHandWeapon);
            }
            model.activeMainHandWeapon = element;
        }
        else if (element.bodyPartType == BodyPartType.OffHandWeapon)
        {
            if (model.activeOffHandWeapon != null)
            {
                DisableAndClearElementOnModel(model, model.activeOffHandWeapon);
            }
            model.activeOffHandWeapon = element;
        }
        else if (element.bodyPartType == BodyPartType.BodyParticles)
        {
            if (model.activeChestParticles != null)
            {
                DisableAndClearElementOnModel(model, model.activeChestParticles);
            }
            model.activeChestParticles = element;
        }
        else if (element.bodyPartType == BodyPartType.BodyLighting)
        {
            if (model.activeChestLighting != null)
            {
                DisableAndClearElementOnModel(model, model.activeChestLighting);
            }
            model.activeChestLighting = element;
        }

        // Enable GO
        element.gameObject.SetActive(true);

        // repeat for any connected elements (e.g. active arm/hand sprites that are connected to the chest piece
        foreach (UniversalCharacterModelElement connectedElement in element.connectedElements)
        {
            if (connectedElement == element)
            {
                Debug.Log("CharacterModelController.EnableAndSetElementOnModel() detected that the element " + element.gameObject.name +
                    " has a copy of itself in its connected elements lst, enabling this will cause an infinite loop, cancelling...");
            }
            else
            {
                EnableAndSetElementOnModel(model, connectedElement);
            }            
        }
    }
    public void EnableAndSetMugShotElementOnModel(UniversalCharacterModel model, string elementName)
    {
        Debug.Log("EnableAndSetMugShotElementOnModel() called...");
        UniversalCharacterModelElement element = null;

        // find element first
        foreach (UniversalCharacterModelElement modelElement in model.allModelElements)
        {
            if (modelElement.gameObject.name == elementName)
            {
                element = modelElement;
                break;
            }
        }

        if (element == null)
        {
            Debug.Log("CharacterModelController.EnableAndSetElementOnModel() could not find an model element with the name "
            + elementName + ", cancelling element enabling...");
            return;
        }

        Debug.Log("Element type is: " + element.bodyPartType.ToString());

        if(element.bodyPartType != BodyPartType.Head &&
            element.bodyPartType != BodyPartType.HeadWear &&
            element.bodyPartType != BodyPartType.Face)
        {
            Debug.Log(elementName + " is not a head/headwear/face element, cancelling...");
            return;
        }

        // Set Active Body Part Reference
        if (element.bodyPartType == BodyPartType.Chest)
        {
            if (model.activeChest != null)
            {
                DisableAndClearElementOnModel(model, model.activeChest);
            }
            model.activeChest = element;
        }
        else if (element.bodyPartType == BodyPartType.Head)
        {
            if (model.activeHead != null)
            {
                DisableAndClearElementOnModel(model, model.activeHead);
            }
            model.activeHead = element;
        }
        else if (element.bodyPartType == BodyPartType.Face)
        {
            if (model.activeFace != null)
            {
                DisableAndClearElementOnModel(model, model.activeFace);
            }
            model.activeFace = element;
        }
        else if (element.bodyPartType == BodyPartType.RightArm)
        {
            if (model.activeRightArm != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightArm);
            }
            model.activeRightArm = element;
        }
        else if (element.bodyPartType == BodyPartType.RightHand)
        {
            if (model.activeRightHand != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightHand);
            }
            model.activeRightHand = element;
        }
        else if (element.bodyPartType == BodyPartType.LeftArm)
        {
            if (model.activeLeftArm != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftArm);
            }
            model.activeLeftArm = element;
        }
        else if (element.bodyPartType == BodyPartType.LeftHand)
        {
            if (model.activeLeftHand != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftHand);
            }
            model.activeLeftHand = element;
        }
        else if (element.bodyPartType == BodyPartType.RightLeg)
        {
            if (model.activeRightLeg != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightLeg);
            }
            model.activeRightLeg = element;
        }
        else if (element.bodyPartType == BodyPartType.LeftLeg)
        {
            if (model.activeLeftLeg != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftLeg);
            }
            model.activeLeftLeg = element;
        }

        // Set Active Weapons + Clothing Reference
        else if (element.bodyPartType == BodyPartType.HeadWear)
        {
            if (model.activeHeadWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeHeadWear);
            }
            model.activeHeadWear = element;
        }
        else if (element.bodyPartType == BodyPartType.ChestWear)
        {
            if (model.activeChestWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeChestWear);
            }
            model.activeChestWear = element;
        }
        else if (element.bodyPartType == BodyPartType.LeftLegWear)
        {
            if (model.activeLeftLegWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftLegWear);
            }
            model.activeLeftLegWear = element;
        }
        else if (element.bodyPartType == BodyPartType.RightLegWear)
        {
            if (model.activeRightLegWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightLegWear);
            }
            model.activeRightLegWear = element;
        }
        else if (element.bodyPartType == BodyPartType.LeftArmWear)
        {
            if (model.activeLeftArmWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftArmWear);
            }
            model.activeLeftArmWear = element;
        }
        else if (element.bodyPartType == BodyPartType.RightArmWear)
        {
            if (model.activeRightArmWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightArmWear);
            }
            model.activeRightArmWear = element;
        }
        else if (element.bodyPartType == BodyPartType.LeftHandWear)
        {
            if (model.activeLeftHandWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftHandWear);
            }
            model.activeLeftHandWear = element;
        }
        else if (element.bodyPartType == BodyPartType.RightHandWear)
        {
            if (model.activeRightHandWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightHandWear);
            }
            model.activeRightHandWear = element;
        }
        else if (element.bodyPartType == BodyPartType.MainHandWeapon)
        {
            if (model.activeMainHandWeapon != null)
            {
                DisableAndClearElementOnModel(model, model.activeMainHandWeapon);
            }
            model.activeMainHandWeapon = element;
        }
        else if (element.bodyPartType == BodyPartType.OffHandWeapon)
        {
            if (model.activeOffHandWeapon != null)
            {
                DisableAndClearElementOnModel(model, model.activeOffHandWeapon);
            }
            model.activeOffHandWeapon = element;
        }
        else if (element.bodyPartType == BodyPartType.BodyParticles)
        {
            if (model.activeChestParticles != null)
            {
                DisableAndClearElementOnModel(model, model.activeChestParticles);
            }
            model.activeChestParticles = element;
        }
        else if (element.bodyPartType == BodyPartType.BodyLighting)
        {
            if (model.activeChestLighting != null)
            {
                DisableAndClearElementOnModel(model, model.activeChestLighting);
            }
            model.activeChestLighting = element;
        }

        // Enable GO
        element.gameObject.SetActive(true);

        // repeat for any connected elements (e.g. active arm/hand sprites that are connected to the chest piece
        foreach (UniversalCharacterModelElement connectedElement in element.connectedElements)
        {
            if (connectedElement == element)
            {
                Debug.Log("CharacterModelController.EnableAndSetElementOnModel() detected that the element " + element.gameObject.name +
                    " has a copy of itself in its connected elements lst, enabling this will cause an infinite loop, cancelling...");
            }
            else
            {
                EnableAndSetElementOnModel(model, connectedElement);
            }
        }
    }
    public void DisableAndClearElementOnModel(UniversalCharacterModel model, UniversalCharacterModelElement element)
    {
        // disable view
        element.gameObject.SetActive(false);

        // Clear reference on model
        if (element.bodyPartType == BodyPartType.Chest)
        {
            model.activeChest = null;
        }
        else if (element.bodyPartType == BodyPartType.Head)
        {
            model.activeHead = null;
        }
        else if (element.bodyPartType == BodyPartType.Face)
        {
            model.activeFace = null;
        }
        else if (element.bodyPartType == BodyPartType.RightArm)
        {
            model.activeRightArm = null;
        }
        else if (element.bodyPartType == BodyPartType.RightHand)
        {
            model.activeRightHand = null;
        }
        else if (element.bodyPartType == BodyPartType.LeftArm)
        {
            model.activeLeftArm = null;
        }
        else if (element.bodyPartType == BodyPartType.LeftHand)
        {
            model.activeLeftHand = null;
        }
        else if (element.bodyPartType == BodyPartType.RightLeg)
        {
            model.activeRightLeg = null;
        }
        else if (element.bodyPartType == BodyPartType.LeftLeg)
        {
            model.activeLeftLeg = null;
        }

        // Set Active Weapons + Clothing Reference
        else if (element.bodyPartType == BodyPartType.HeadWear)
        {
            model.activeHeadWear = null;
        }
        else if (element.bodyPartType == BodyPartType.ChestWear)
        {
            model.activeChestWear = null;
        }
        else if (element.bodyPartType == BodyPartType.LeftLegWear)
        {
            model.activeLeftLegWear = null;
        }
        else if (element.bodyPartType == BodyPartType.RightLegWear)
        {
            model.activeRightLegWear = null;
        }
        else if (element.bodyPartType == BodyPartType.LeftArmWear)
        {
            model.activeLeftArmWear = null;
        }
        else if (element.bodyPartType == BodyPartType.RightArmWear)
        {
            model.activeRightArmWear = null;
        }
        else if (element.bodyPartType == BodyPartType.LeftHandWear)
        {
            model.activeLeftHandWear = null;
        }
        else if (element.bodyPartType == BodyPartType.RightHandWear)
        {
            model.activeRightHandWear = null;
        }
        else if (element.bodyPartType == BodyPartType.MainHandWeapon)
        {
            model.activeMainHandWeapon = null;
        }
        else if (element.bodyPartType == BodyPartType.OffHandWeapon)
        {
            model.activeOffHandWeapon = null;
        }

        // Particles
        else if (element.bodyPartType == BodyPartType.BodyParticles)
        {
            model.activeChestParticles = null;
        }

        // Lighting
        else if (element.bodyPartType == BodyPartType.BodyLighting)
        {
            model.activeChestLighting = null;
        }

        // repeat for any connected elements (e.g. active arm/hand sprites that are connected to the chest piece
        foreach (UniversalCharacterModelElement connectedElement in element.connectedElements)
        {
            DisableAndClearElementOnModel(model, connectedElement);
        }

    }
    #endregion

    // Get View Parts  
    #region
    public UniversalCharacterModelElement GetNextElementInList(List<UniversalCharacterModelElement> list)
    {
        // Set up
        UniversalCharacterModelElement elementReturned = null;
        int currentIndex = 0;
        int nextIndex = 0;

        // calculate list size
        int maxIndex = list.Count - 1;

        // prevent negative index
        if(maxIndex < 0)
        {
            maxIndex = 0;
        }
        
        // calculate current index
        foreach (UniversalCharacterModelElement ele in list)
        {
            if (ele.gameObject.activeSelf)
            {
                currentIndex = list.IndexOf(ele);
                Debug.Log("CharacterModelController.GetNextElementInList() calculated that " + ele.gameObject.name +
                    " is at list index " + currentIndex);
                break;
            }
        }

        // if at end of list, go back to index 0
        if(currentIndex + 1 > maxIndex)
        {
            nextIndex = 0;
        }
        else
        {
            nextIndex = currentIndex + 1;
        }
        
        elementReturned = list[nextIndex];

        Debug.Log("CharacterModelController.GetNextElementInList() returning " +
            elementReturned.gameObject.name + " as next indexed element");

        return elementReturned;
    }
    public UniversalCharacterModelElement GetPreviousElementInList(List<UniversalCharacterModelElement> list)
    {
        // Set up
        UniversalCharacterModelElement elementReturned = null;
        int currentIndex = 0;
        int nextIndex = 0;

        // calculate list size
        int maxIndex = list.Count - 1;

        // prevent negative index
        if (maxIndex < 0)
        {
            maxIndex = 0;
        }

        // calculate current index
        foreach (UniversalCharacterModelElement ele in list)
        {
            if (ele.gameObject.activeSelf)
            {
                currentIndex = list.IndexOf(ele);
                Debug.Log("CharacterModelController.GetPreviousElementInList() calculated that " + ele.gameObject.name +
                    " is at list index " + currentIndex);
                break;
            }
        }

        // if at start of list, go to the last index
        if (currentIndex - 1 < 0)
        {
            nextIndex = maxIndex;
        }
        else
        {
            nextIndex = currentIndex - 1;
        }

        elementReturned = list[nextIndex];

        Debug.Log("CharacterModelController.GetPreviousElementInList() returning " +
            elementReturned.gameObject.name + " as next indexed element");

        return elementReturned;
    }
    #endregion

    // Fading Logic
    #region
    public void FadeOutCharacterModel(UniversalCharacterModel model, float speed = 1f)
    {
        EntityRenderer view = model.myEntityRenderer;
        foreach (SpriteRenderer sr in view.renderers)
        {
            if (sr.gameObject.activeSelf)
                sr.DOFade(0, speed);
        }

        // Stop particles
        if(model.activeChestParticles != null)
        {
            ParticleSystem[] ps = model.activeChestParticles.GetComponentsInChildren<ParticleSystem>();
            foreach(ParticleSystem p in ps)
            {
                p.Stop();
            }
        }

    }   
    public void FadeInCharacterModel(UniversalCharacterModel model, float speed = 1f)
    {
        EntityRenderer view = model.myEntityRenderer;
        foreach (SpriteRenderer sr in view.renderers)
        {
            if (sr.gameObject.activeSelf)
                sr.DOKill();
            sr.DOFade(1, speed);
        }

        // Restart particles
        if (model.activeChestParticles != null)
        {
            ParticleSystem[] ps = model.activeChestParticles.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in ps)
            {
                p.Clear();
                p.Play();
            }
        }
    }
   
    public void FadeInCharacterShadow(CharacterEntityView view, float speed, System.Action onCompleteCallBack = null)
    {
        view.ucmShadowCg.DOFade(0f, 0f);
        Sequence s = DOTween.Sequence();
        s.Append(view.ucmShadowCg.DOFade(1f, speed));

        if (onCompleteCallBack != null)
        {
            s.OnComplete(() => onCompleteCallBack.Invoke());
        }
    }
    public void FadeOutCharacterShadow(CharacterEntityView view, float speed)
    {
        view.ucmShadowCg.DOFade(1f, 0f);
        view.ucmShadowCg.DOFade(0f, speed);
    }
    #endregion

}
