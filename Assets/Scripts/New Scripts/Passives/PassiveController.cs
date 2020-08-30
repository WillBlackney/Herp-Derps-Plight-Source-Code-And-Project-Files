using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveController : Singleton<PassiveController>
{
    [Header("Passive Library Properties")]
    public List<PassiveIconDataSO> allIcons;

    // Library Logic
    #region
    public PassiveIconDataSO GetPassiveIconDataByName(string name)
    {
        PassiveIconDataSO iconReturned = null;

        foreach (PassiveIconDataSO icon in allIcons)
        {
            if (icon.passiveName == name)
            {
                iconReturned = icon;
                break;
            }
        }

        if (iconReturned == null)
        {
            Debug.Log("PassiveController.GetPassiveIconDataByName() could not find a passive icon SO with the name " +
                name + ", returning null...");
        }

        return iconReturned;
    }
    #endregion

    // Setup Logic
    #region
    private void BuildPassiveManagerFromOtherPassiveManager(PassiveManagerModel clone, PassiveManagerModel original)
    {
        Debug.Log("PassiveController.BuildPassiveManagerFromOtherPassiveManager() called...");

        if(original.bonusStrengthStacks != 0)
        {
            ModifyBonusStrength(clone, original.bonusStrengthStacks, false);
        }

    }
    public void BuildCharacterEntityPassivesFromCharacterData(CharacterEntityModel character, CharacterData data)
    {
        Debug.Log("PassiveController.BuildCharacterEntityPassivesFromCharacterData() called...");
        character.passiveManager = new PassiveManagerModel();
        character.passiveManager.myCharacterData = data;
        character.passiveManager.myCharacter = character;

        BuildPassiveManagerFromOtherPassiveManager(character.passiveManager, data.passiveManager);

    }
    public void BuildCharacterEntityPassivesFromEnemyData(CharacterEntityModel character, EnemyDataSO data)
    {
        Debug.Log("PassiveController.BuildCharacterEntityPassivesFromCharacterData() called...");
        character.passiveManager = new PassiveManagerModel();
        character.passiveManager.myCharacter = character;

        BuildPassiveManagerFromOtherPassiveManager(character.passiveManager, data.passiveManager);

    }
    #endregion

    // Update Passive Icons and Panel View
    #region
    private void BuildPassiveIconViewFromData(PassiveIconView icon, PassiveIconDataSO iconData)
    {
        Debug.Log("PassiveController.BuildPassiveIconViewFromData() called...");

        icon.myIconData = iconData;
        icon.passiveImage.sprite = iconData.passiveSprite;

        icon.statusName = iconData.passiveName;
        if (iconData.showStackCount)
        {
            icon.statusStacksText.gameObject.SetActive(true);
        }

        icon.statusStacksText.text = icon.statusStacks.ToString();

    }
    private void ModifyIconViewStacks(PassiveIconView icon, int stacksGainedOrLost)
    {
        Debug.Log("PassiveController.ModifyIconViewStacks() called...");

        icon.statusStacks += stacksGainedOrLost;
        icon.statusStacksText.text = icon.statusStacks.ToString();
        if (icon.statusStacks == 0)
        {
            icon.statusStacksText.gameObject.SetActive(false);
        }
    }
    private void StartAddPassiveToPanelProcess(CharacterEntityView view, PassiveIconDataSO iconData, int stacksGainedOrLost)
    {
        Debug.Log("PassiveController.StartAddPassiveToPanelProcess() called...");

        if (view.passiveIcons.Count > 0)
        {
            //StatusIconDataSO si = null;
            //int stacks = 0;
            bool matchFound = false;

            foreach (PassiveIconView icon in view.passiveIcons)
            {
                if (iconData.passiveName == icon.statusName)
                {
                    // Icon already exists in character's list
                    UpdatePassiveIconOnPanel(view, icon, stacksGainedOrLost);
                    matchFound = true;
                    break;
                }

                else
                {
                    //si = iconData;
                    //stacks = stacksGainedOrLost;
                }
            }

            if (matchFound == false)
            {
                //AddNewPassiveIconToPanel(view, si, stacks);
                AddNewPassiveIconToPanel(view, iconData, stacksGainedOrLost);
            }

        }
        else
        {
            AddNewPassiveIconToPanel(view, iconData, stacksGainedOrLost);
        }


    }
    private void AddNewPassiveIconToPanel(CharacterEntityView view, PassiveIconDataSO iconData, int stacksGained)
    {
        Debug.Log("PassiveController.AddNewPassiveIconToPanel() called...");

        // only create an icon if the the effects' stacks are at least 1 or -1
        if (stacksGained != 0)
        {
            GameObject newIconGO = Instantiate(PrefabHolder.Instance.PassiveIconViewPrefab, view.passiveIconsVisualParent.transform);
            PassiveIconView newStatus = newIconGO.GetComponent<PassiveIconView>();
            BuildPassiveIconViewFromData(newStatus, iconData);
            ModifyIconViewStacks(newStatus, stacksGained);
            view.passiveIcons.Add(newStatus);
        }

    }
    private void RemovePassiveIconFromPanel(CharacterEntityView view, PassiveIconView iconToRemove)
    {
        Debug.Log("PassiveController.RemovePassiveIconFromPanel() called...");
        view.passiveIcons.Remove(iconToRemove);
        Destroy(iconToRemove.gameObject);
    }
    private void UpdatePassiveIconOnPanel(CharacterEntityView view, PassiveIconView iconToUpdate, int stacksGainedOrLost)
    {
        Debug.Log("PassiveController.UpdatePassiveIconOnPanel() called...");

        ModifyIconViewStacks(iconToUpdate, stacksGainedOrLost);
        if (iconToUpdate.statusStacks == 0)
        {
            RemovePassiveIconFromPanel(view, iconToUpdate);
        }

    }
    #endregion
    
    // Modify Specific Passives
    #region

    // Bonus Core Stats
    private void ModifyBonusStrength(PassiveManagerModel pManager, int stacks, bool showVFX = true)
    {
        Debug.Log("PassiveController.ModifyBonusStrength() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Strength");
        CharacterEntityModel character = pManager.myCharacter;
        CharacterData data = pManager.myCharacterData;

        // Increment stacks
        pManager.bonusStrengthStacks += stacks;

        if(character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }          

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Strength + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateCoreStatBuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);
               
            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Strength - " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);
            }

            // Update intent GUI, if enemy and attacking
            if (pManager.myCharacter.controller == Controller.AI &&
                pManager.myCharacter.myNextAction != null &&
                pManager.myCharacter.myNextAction.actionType == ActionType.AttackTarget)
            {
                CharacterEntityController.Instance.UpdateEnemyIntentGUI(pManager.myCharacter);
            }            
        }

        
    }
    private void ModifyBonusDexterity(PassiveManagerModel pManager, int stacks, bool showVFX = true)
    {
        Debug.Log("PassiveController.ModifyBonusDexterity() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Dexterity");
        CharacterEntityModel character = pManager.myCharacter;
        CharacterData data = pManager.myCharacterData;

        // Increment stacks
        pManager.bonusDexterityStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Dexterity + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateCoreStatBuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Dexterity - " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);
            }
        }
    }
    private void ModifyBonusInitiative(PassiveManagerModel pManager, int stacks, bool showVFX = true)
    {
        Debug.Log("PassiveController.ModifyBonusInitiative() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Initiative");
        CharacterEntityModel character = pManager.myCharacter;
        CharacterData data = pManager.myCharacterData;

        // Increment stacks
        pManager.bonusDexterityStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Initiative + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateCoreStatBuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Initiative - " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);
            }
        }
    }
    private void ModifyBonusStamina(PassiveManagerModel pManager, int stacks, bool showVFX = true)
    {
        Debug.Log("PassiveController.ModifyBonusStamina() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Stamina");
        CharacterEntityModel character = pManager.myCharacter;
        CharacterData data = pManager.myCharacterData;

        // Increment stacks
        pManager.bonusDexterityStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Stamina + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateCoreStatBuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Stamina - " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);
            }
        }
    }
    private void ModifyBonusDraw(PassiveManagerModel pManager, int stacks, bool showVFX = true)
    {
        Debug.Log("PassiveController.ModifyBonusDraw() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Draw");
        CharacterEntityModel character = pManager.myCharacter;
        CharacterData data = pManager.myCharacterData;

        // Increment stacks
        pManager.bonusDrawStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Draw + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateCoreStatBuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Draw - " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);
            }
        }
    }

    // Temporary Bonus Core Stats
    private void ModifyTemporaryStrength(PassiveManagerModel pManager, int stacks, bool showVFX = true)
    {
        Debug.Log("PassiveController.ModifyTemporaryStrength() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Temporary Strength");
        CharacterEntityModel character = pManager.myCharacter;
        CharacterData data = pManager.myCharacterData;

        // Increment stacks
        pManager.temporaryBonusStrengthStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Strength + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateCoreStatBuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Strength - " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);
            }

            // Update intent GUI, if enemy and attacking
            if (pManager.myCharacter.controller == Controller.AI &&
                pManager.myCharacter.myNextAction != null &&
                pManager.myCharacter.myNextAction.actionType == ActionType.AttackTarget)
            {
                CharacterEntityController.Instance.UpdateEnemyIntentGUI(pManager.myCharacter);
            }
        }


    }
    private void ModifyTemporaryDexterity(PassiveManagerModel pManager, int stacks, bool showVFX = true)
    {
        Debug.Log("PassiveController.ModifyTemporaryDexterity() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Temporary Dexterity");
        CharacterEntityModel character = pManager.myCharacter;
        CharacterData data = pManager.myCharacterData;

        // Increment stacks
        pManager.temporaryBonusDexterityStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Dexterity + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateCoreStatBuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Dexterity - " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);
            }
        }
    }
    private void ModifyTemporaryInitiative(PassiveManagerModel pManager, int stacks, bool showVFX = true)
    {
        Debug.Log("PassiveController.ModifyTemporaryInitiative() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Temporary Initiative");
        CharacterEntityModel character = pManager.myCharacter;
        CharacterData data = pManager.myCharacterData;

        // Increment stacks
        pManager.temporaryBonusInitiativeStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Initiative + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateCoreStatBuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Initiative - " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);
            }
        }
    }
    private void ModifyTemporaryStamina(PassiveManagerModel pManager, int stacks, bool showVFX = true)
    {
        Debug.Log("PassiveController.ModifyTemporaryStamina() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Temporary Stamina");
        CharacterEntityModel character = pManager.myCharacter;
        CharacterData data = pManager.myCharacterData;

        // Increment stacks
        pManager.temporaryBonusStaminaStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Stamina + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateCoreStatBuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Stamina - " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);
            }
        }
    }
    private void ModifyTemporaryDraw(PassiveManagerModel pManager, int stacks, bool showVFX = true)
    {
        Debug.Log("PassiveController.ModifyTemporaryDraw() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Temporary Draw");
        CharacterEntityModel character = pManager.myCharacter;
        CharacterData data = pManager.myCharacterData;

        // Increment stacks
        pManager.temporaryBonusDrawStacks += stacks;

        if (character != null)
        {
            // Add icon view visual event
            if (showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks));
            }
            else
            {
                StartAddPassiveToPanelProcess(character.characterEntityView, iconData, stacks);
            }

            if (stacks > 0 && showVFX)
            {
                // VFX visual events
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Draw + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateCoreStatBuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Draw - " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);
            }
        }
    }
    #endregion
}
