using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveController : Singleton<PassiveController>
{
    // Properties + Component References
    #region
    [Header("Passive Library Properties")]
    public List<PassiveIconDataSO> allIcons;
    #endregion

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
    private void BuildPassiveManagerFromOtherPassiveManager(PassiveManagerModel originalData, PassiveManagerModel newClone)
    {
        Debug.Log("PassiveController.BuildPassiveManagerFromOtherPassiveManager() called...");

        // Core stat bonuses
        #region
        if (originalData.bonusPowerStacks != 0)
        {
            ModifyBonusPower(newClone, originalData.bonusPowerStacks, false);
        }
        if (originalData.bonusDexterityStacks != 0)
        {
            ModifyBonusDexterity(newClone, originalData.bonusDexterityStacks, false);
        }
        if (originalData.bonusDrawStacks != 0)
        {
            ModifyBonusDraw(newClone, originalData.bonusDrawStacks, false);
        }
        if (originalData.bonusStaminaStacks != 0)
        {
            ModifyBonusStamina(newClone, originalData.bonusStaminaStacks, false);
        }
        if (originalData.bonusInitiativeStacks != 0)
        {
            ModifyBonusInitiative(newClone, originalData.bonusInitiativeStacks, false);
        }
        #endregion

        // Temp Core stat bonuses
        #region
        if (originalData.temporaryBonusPowerStacks != 0)
        {
            ModifyTemporaryPower(newClone, originalData.temporaryBonusPowerStacks, false);
        }
        if (originalData.temporaryBonusDexterityStacks != 0)
        {
            ModifyTemporaryDexterity(newClone, originalData.temporaryBonusDexterityStacks, false);
        }
        if (originalData.temporaryBonusDrawStacks != 0)
        {
            ModifyTemporaryDraw(newClone, originalData.temporaryBonusDrawStacks, false);
        }
        if (originalData.temporaryBonusStaminaStacks != 0)
        {
            ModifyTemporaryStamina(newClone, originalData.temporaryBonusStaminaStacks, false);
        }
        if (originalData.temporaryBonusInitiativeStacks != 0)
        {
            ModifyTemporaryInitiative(newClone, originalData.temporaryBonusInitiativeStacks, false);
        }
        #endregion

        // Buff passives
        #region
        if (originalData.enrageStacks != 0)
        {
            ModifyEnrage(newClone, originalData.enrageStacks, false);
        }
        #endregion

    }
    public void BuildPassiveManagerFromSerializedPassiveManager(PassiveManagerModel pManager, SerializedPassiveManagerModel original)
    {
        pManager.bonusPowerStacks = original.bonusPowerStacks;
        pManager.bonusDexterityStacks = original.bonusDexterityStacks;
        pManager.bonusStaminaStacks = original.bonusStaminaStacks;
        pManager.bonusDrawStacks = original.bonusDrawStacks;
        pManager.bonusInitiativeStacks = original.bonusInitiativeStacks;

        pManager.temporaryBonusPowerStacks = original.temporaryBonusPowerStacks;
        pManager.temporaryBonusDexterityStacks = original.temporaryBonusDexterityStacks;
        pManager.temporaryBonusStaminaStacks = original.temporaryBonusStaminaStacks;
        pManager.temporaryBonusDrawStacks = original.temporaryBonusDrawStacks;
        pManager.temporaryBonusInitiativeStacks = original.temporaryBonusInitiativeStacks;

        pManager.enrageStacks = original.enrageStacks;
    }    
    public void BuildPlayerCharacterEntityPassivesFromCharacterData(CharacterEntityModel character, CharacterData data)
    {
        Debug.Log("PassiveController.BuildPlayerCharacterEntityPassivesFromCharacterData() called...");
        character.passiveManager = new PassiveManagerModel();
        character.passiveManager.myCharacter = character;

        BuildPassiveManagerFromOtherPassiveManager(data.passiveManager, character.passiveManager);
    }
    public void BuildEnemyCharacterEntityPassivesFromEnemyData(CharacterEntityModel character, EnemyDataSO data)
    {
        Debug.Log("PassiveController.BuildEnemyCharacterEntityPassivesFromEnemyData() called...");

        // Create an empty pManager that we deserialize the data into first
        PassiveManagerModel deserializedManager = new PassiveManagerModel();
        BuildPassiveManagerFromSerializedPassiveManager(deserializedManager, data.serializedPassiveManager);

        character.passiveManager = new PassiveManagerModel();
        character.passiveManager.myCharacter = character;

        // Copy data from desrialized pManager into the characters actual pManager
        BuildPassiveManagerFromOtherPassiveManager(deserializedManager, character.passiveManager);
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

    // Conditional Checks
    #region
    public bool IsEntityAffectedByPassive(PassiveManagerModel passiveManager, string passiveName, int stacksRequired = 1)
    {
        Debug.Log("PassiveController.IsEntityAffectedByPassive() called, checking for if entity has " + passiveName
            + " with at least " + stacksRequired.ToString() + " stacks");

        bool boolReturned = false;

        // Core stat bonuses
        #region
        if (passiveName == "Power" && passiveManager.bonusPowerStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        if (passiveName == "Dexterity" && passiveManager.bonusDexterityStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        if (passiveName == "Draw" && passiveManager.bonusDrawStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        if (passiveName == "Initiative" && passiveManager.bonusInitiativeStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        if (passiveName == "Stamina" && passiveManager.bonusStaminaStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        #endregion

        // Temp Core stat bonuses
        #region
        if (passiveName == "Temporary Power" && passiveManager.temporaryBonusPowerStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        if (passiveName == "Temporary Dexterity" && passiveManager.temporaryBonusDexterityStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        if (passiveName == "Temporary Draw" && passiveManager.temporaryBonusDrawStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        if (passiveName == "Temporary Initiative" && passiveManager.temporaryBonusInitiativeStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        if (passiveName == "Temporary Stamina" && passiveManager.temporaryBonusStaminaStacks >= stacksRequired)
        {
            boolReturned = true;
        }
        #endregion

        return boolReturned;
    }
    #endregion

    // Apply Passive To Character Entities
    #region
    public void ApplyPassiveToCharacterEntity(PassiveManagerModel newClone, string originalData, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ApplyPassiveToCharacterEntity() called...");

        // Core stat bonuses
        #region
        if (originalData == "Power")
        {
            ModifyBonusPower(newClone, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Dexterity")
        {
            ModifyBonusDexterity(newClone, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Draw")
        {
            ModifyBonusDraw(newClone, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Initiative")
        {
            ModifyBonusInitiative(newClone, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Stamina")
        {
            ModifyBonusStamina(newClone, stacks, showVFX, vfxDelay);
        }
        #endregion

        // Temp Core stat bonuses
        #region
        else if (originalData == "Temporary Power")
        {
            ModifyTemporaryPower(newClone, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Temporary Dexterity")
        {
            ModifyTemporaryDexterity(newClone, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Temporary Draw")
        {
            ModifyTemporaryDraw(newClone, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Temporary Initiative")
        {
            ModifyTemporaryInitiative(newClone, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Temporary Stamina")
        {
            ModifyTemporaryStamina(newClone, stacks, showVFX, vfxDelay);
        }
        #endregion

        // Buff passives
        #region
        else if (originalData == "Enrage")
        {
            ModifyEnrage(newClone, stacks, showVFX, vfxDelay);
        }
        #endregion
    }
    #endregion

    // Modify Specific Passives
    #region

    // Bonus Core Stats
    #region
    private void ModifyBonusPower(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBonusStrength() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Power");
        CharacterEntityModel character = pManager.myCharacter;

        // Increment stacks
        pManager.bonusPowerStacks += stacks;

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

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }

        
    }
    private void ModifyBonusDexterity(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBonusDexterity() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Dexterity");
        CharacterEntityModel character = pManager.myCharacter;
        //CharacterData data = pManager.myCharacterData;

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

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    private void ModifyBonusInitiative(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBonusInitiative() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Initiative");
        CharacterEntityModel character = pManager.myCharacter;
        //CharacterData data = pManager.myCharacterData;

        // Increment stacks
        pManager.bonusInitiativeStacks += stacks;

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

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    private void ModifyBonusStamina(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBonusStamina() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Stamina");
        CharacterEntityModel character = pManager.myCharacter;
        //CharacterData data = pManager.myCharacterData;

        // Increment stacks
        pManager.bonusStaminaStacks += stacks;

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

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    private void ModifyBonusDraw(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBonusDraw() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Draw");
        CharacterEntityModel character = pManager.myCharacter;
        //CharacterData data = pManager.myCharacterData;

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

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    #endregion

    // Temporary Bonus Core Stats
    #region
    private void ModifyTemporaryPower(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTemporaryStrength() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Temporary Power");
        CharacterEntityModel character = pManager.myCharacter;
        //CharacterData data = pManager.myCharacterData;

        // Increment stacks
        pManager.temporaryBonusPowerStacks += stacks;

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
            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    private void ModifyTemporaryDexterity(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTemporaryDexterity() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Temporary Dexterity");
        CharacterEntityModel character = pManager.myCharacter;
        //CharacterData data = pManager.myCharacterData;

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
            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    private void ModifyTemporaryInitiative(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTemporaryInitiative() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Temporary Initiative");
        CharacterEntityModel character = pManager.myCharacter;
        //CharacterData data = pManager.myCharacterData;

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

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    private void ModifyTemporaryStamina(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTemporaryStamina() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Temporary Stamina");
        CharacterEntityModel character = pManager.myCharacter;
        //CharacterData data = pManager.myCharacterData;

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

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    private void ModifyTemporaryDraw(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTemporaryDraw() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Temporary Draw");
        CharacterEntityModel character = pManager.myCharacter;
        //CharacterData data = pManager.myCharacterData;

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

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    #endregion

    // Buff Passives
    #region
    private void ModifyEnrage(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyEnrage() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Enrage");
        CharacterEntityModel character = pManager.myCharacter;

        // Increment stacks
        pManager.enrageStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Enrage + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateCoreStatBuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Enrage - " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.transform.position);
                }, QueuePosition.Back, 0, 0.5f);
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    #endregion
    #endregion
}
