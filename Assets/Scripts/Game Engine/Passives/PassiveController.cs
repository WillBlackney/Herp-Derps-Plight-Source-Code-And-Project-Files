using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveController : Singleton<PassiveController>
{
    // Properties + Component References
    #region
    [Header("Passive Library Properties")]
    [SerializeField] private PassiveIconDataSO[] allIcons;

    // Getters
    public PassiveIconDataSO[] AllIcons
    {
        get { return allIcons; }
        private set { allIcons = value; }
    }
    #endregion

    // Library Logic
    #region
    public PassiveIconDataSO GetPassiveIconDataByName(string name)
    {
        PassiveIconDataSO iconReturned = null;

        foreach (PassiveIconDataSO icon in AllIcons)
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
        if (originalData.shieldWallStacks != 0)
        {
            ModifyShieldWall(newClone, originalData.shieldWallStacks, false);
        }
        if (originalData.fanOfKnivesStacks != 0)
        {
            ModifyFanOfKnives(newClone, originalData.fanOfKnivesStacks, false);
        }
        if (originalData.divineFavourStacks != 0)
        {
            ModifyDivineFavour(newClone, originalData.divineFavourStacks, false);
        }
        if (originalData.phoenixFormStacks != 0)
        {
            ModifyPhoenixForm(newClone, originalData.phoenixFormStacks, false);
        }
        if (originalData.poisonousStacks != 0)
        {
            ModifyPoisonous(newClone, originalData.poisonousStacks, false);
        }
        if (originalData.venomousStacks != 0)
        {
            ModifyVenomous(newClone, originalData.venomousStacks, false);
        }
        if (originalData.overloadStacks != 0)
        {
            ModifyOverload(newClone, originalData.overloadStacks, false);
        }
        if (originalData.fusionStacks != 0)
        {
            ModifyFusion(newClone, originalData.fusionStacks, false);
        }
        if (originalData.plantedFeetStacks != 0)
        {
            ModifyPlantedFeet(newClone, originalData.plantedFeetStacks, false);
        }
        if (originalData.consecrationStacks != 0)
        {
            ModifyConsecration(newClone, originalData.consecrationStacks, false);
        }
        #endregion

        // Special Defensive Passives
        #region
        if (originalData.runeStacks != 0)
        {
            ModifyRune(newClone, originalData.runeStacks, false);
        }
        if (originalData.barrierStacks != 0)
        {
            ModifyBarrier(newClone, originalData.barrierStacks, false);
        }
        #endregion

        // Aura Passives
        #region
        if (originalData.encouragingAuraStacks != 0)
        {
            ModifyEncouragingAura(newClone, originalData.encouragingAuraStacks, false);
        }
        if (originalData.shadowAuraStacks != 0)
        {
            ModifyShadowAura(newClone, originalData.shadowAuraStacks, false);
        }
        #endregion

        // Dot Passives
        #region
        if (originalData.poisonedStacks != 0)
        {
            ModifyPoisoned(null, newClone, originalData.poisonedStacks, false);
        }
        if (originalData.burningStacks != 0)
        {
            ModifyBurning(newClone, originalData.burningStacks, false);
        }
        #endregion

        // Core % Modifier passives
        #region
        if (originalData.weakenedStacks != 0)
        {
            ModifyWeakened(newClone, originalData.weakenedStacks, false);
        }
        if (originalData.wrathStacks != 0)
        {
            ModifyWrath(newClone, originalData.wrathStacks, false);
        }
        if (originalData.gritStacks != 0)
        {
            ModifyGrit(newClone, originalData.gritStacks, false);
        }
        if (originalData.vulnerableStacks != 0)
        {
            ModifyVulnerable(newClone, originalData.vulnerableStacks, false);
        }
        #endregion

        // Misc Passives
        #region
        if (originalData.fireBallBonusDamageStacks != 0)
        {
            ModifyFireBallBonusDamage(newClone, originalData.fireBallBonusDamageStacks, false);
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
        pManager.shieldWallStacks = original.shieldWallStacks;
        pManager.fanOfKnivesStacks = original.fanOfKnivesStacks;
        pManager.divineFavourStacks = original.divineFavourStacks;
        pManager.phoenixFormStacks = original.phoenixFormStacks;       
        pManager.poisonousStacks = original.poisonousStacks;
        pManager.venomousStacks = original.venomousStacks;
        pManager.overloadStacks = original.overloadStacks;
        pManager.fusionStacks = original.fusionStacks;
        pManager.plantedFeetStacks = original.plantedFeetStacks;
        pManager.consecrationStacks = original.consecrationStacks;

        pManager.runeStacks = original.runeStacks;
        pManager.barrierStacks = original.barrierStacks;

        pManager.encouragingAuraStacks = original.encouragingAuraStacks;
        pManager.shadowAuraStacks = original.shadowAuraStacks;

        pManager.wrathStacks = original.wrathStacks;
        pManager.gritStacks = original.gritStacks;
        pManager.weakenedStacks = original.weakenedStacks;
        pManager.vulnerableStacks = original.vulnerableStacks;

        pManager.poisonedStacks = original.poisonedStacks;
        pManager.burningStacks = original.burningStacks;

        pManager.fireBallBonusDamageStacks = original.fireBallBonusDamageStacks;

    }    
    public void BuildPlayerCharacterEntityPassivesFromCharacterData(CharacterEntityModel character, CharacterData data)
    {
        Debug.Log("PassiveController.BuildPlayerCharacterEntityPassivesFromCharacterData() called...");
        character.pManager = new PassiveManagerModel();
        character.pManager.myCharacter = character;

        BuildPassiveManagerFromOtherPassiveManager(data.passiveManager, character.pManager);
    }
    public void BuildEnemyCharacterEntityPassivesFromEnemyData(CharacterEntityModel character, EnemyDataSO data)
    {
        Debug.Log("PassiveController.BuildEnemyCharacterEntityPassivesFromEnemyData() called...");

        // Create an empty pManager that we deserialize the data into first
        PassiveManagerModel deserializedManager = new PassiveManagerModel();
        BuildPassiveManagerFromSerializedPassiveManager(deserializedManager, data.serializedPassiveManager);

        character.pManager = new PassiveManagerModel();
        character.pManager.myCharacter = character;

        // Copy data from desrialized pManager into the characters actual pManager
        BuildPassiveManagerFromOtherPassiveManager(deserializedManager, character.pManager);
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
        if (iconData.hiddenOnPassivePanel)
        {
            icon.gameObject.SetActive(false);
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
    private bool ShouldRuneBlockThisPassiveApplication(PassiveManagerModel pManager, PassiveIconDataSO iconData, int stacks)
    {
        if (pManager.runeStacks > 0 &&
            ((iconData.runeBlocksIncrease && stacks > 0) || (iconData.runeBlocksDecrease && stacks < 0)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    // Apply Passive To Character Entities
    #region
    public void ModifyPassiveOnCharacterEntity(PassiveManagerModel pManager, string originalData, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyPassiveOnCharacterEntity() called...");

        // Core stat bonuses
        #region
        if (originalData == "Power")
        {
            ModifyBonusPower(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Dexterity")
        {
            ModifyBonusDexterity(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Draw")
        {
            ModifyBonusDraw(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Initiative")
        {
            ModifyBonusInitiative(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Stamina")
        {
            ModifyBonusStamina(pManager, stacks, showVFX, vfxDelay);
        }
        #endregion

        // Temp Core stat bonuses
        #region
        else if (originalData == "Temporary Power")
        {
            ModifyTemporaryPower(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Temporary Dexterity")
        {
            ModifyTemporaryDexterity(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Temporary Draw")
        {
            ModifyTemporaryDraw(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Temporary Initiative")
        {
            ModifyTemporaryInitiative(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Temporary Stamina")
        {
            ModifyTemporaryStamina(pManager, stacks, showVFX, vfxDelay);
        }
        #endregion

        // Buff passives
        #region
        else if (originalData == "Enrage")
        {
            ModifyEnrage(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Shield Wall")
        {
            ModifyShieldWall(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Fan Of Knives")
        {
            ModifyFanOfKnives(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Divine Favour")
        {
            ModifyDivineFavour(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Phoenix Form")
        {
            ModifyPhoenixForm(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Poisonous")
        {
            ModifyPoisonous(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Venomous")
        {
            ModifyVenomous(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Overload")
        {
            ModifyOverload(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Fusion")
        {
            ModifyFusion(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Planted Feet")
        {
            ModifyPlantedFeet(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Consecration")
        {
            ModifyConsecration(pManager, stacks, showVFX, vfxDelay);
        }
        #endregion

        // Special Defensive Passives
        #region
        else if (originalData == "Barrier")
        {
            ModifyBarrier(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Rune")
        {
            ModifyRune(pManager, stacks, showVFX, vfxDelay);
        }
        #endregion

        // Aura Passives
        #region
        else if (originalData == "Encouraging Aura")
        {
            ModifyEncouragingAura(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Shadow Aura")
        {
            ModifyShadowAura(pManager, stacks, showVFX, vfxDelay);
        }
        #endregion

        // DoT passives
        #region
        else if (originalData == "Poisoned")
        {
            ModifyPoisoned(null, pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Burning")
        {
            ModifyBurning(pManager, stacks, showVFX, vfxDelay);
        }
        #endregion

        // Core % Modifier passives
        #region
        else if (originalData == "Wrath")
        {
            ModifyWrath(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Weakened")
        {
            ModifyWeakened(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Grit")
        {
            ModifyGrit(pManager, stacks, showVFX, vfxDelay);
        }
        else if (originalData == "Vulnerable")
        {
            ModifyVulnerable(pManager, stacks, showVFX, vfxDelay);
        }
        #endregion

        // Misc Stats
        #region
        else if (originalData == "Fire Ball Bonus Damage")
        {
            ModifyFireBallBonusDamage(pManager, stacks, showVFX, vfxDelay);
        }
        #endregion
    }
    #endregion

    // Modify Specific Passives
    #region

    // Bonus Core Stats
    #region
    public void ModifyBonusPower(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBonusStrength() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Power");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Power +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });
               
            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Power " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            // Update intent GUI, if enemy and attacking
            if (pManager.myCharacter.controller == Controller.AI &&
                pManager.myCharacter.myNextAction != null &&
                (pManager.myCharacter.myNextAction.actionType == ActionType.AttackTarget ||
                 pManager.myCharacter.myNextAction.actionType == ActionType.AttackAllEnemies))
            {
                CharacterEntityController.Instance.UpdateEnemyIntentGUI(pManager.myCharacter);
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }

        
    }
    public void ModifyBonusDexterity(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBonusDexterity() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Dexterity");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Dexterity +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Dexterity " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyBonusInitiative(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBonusInitiative() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Initiative");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Initiative + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Initiative " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyBonusStamina(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBonusStamina() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Stamina");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Stamina +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Stamina " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyBonusDraw(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBonusDraw() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Draw");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Draw + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Draw " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
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
    public void ModifyTemporaryPower(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTemporaryStrength() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Temporary Power");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Power +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Power Expired");
                   // VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                }, QueuePosition.Back, 0, 0.5f);
            }

            // Update intent GUI, if enemy and attacking
            if (pManager.myCharacter.controller == Controller.AI &&
                pManager.myCharacter.myNextAction != null &&
                 (pManager.myCharacter.myNextAction.actionType == ActionType.AttackTarget ||
                 pManager.myCharacter.myNextAction.actionType == ActionType.AttackAllEnemies))
            {
                CharacterEntityController.Instance.UpdateEnemyIntentGUI(pManager.myCharacter);
            }
            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyTemporaryDexterity(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTemporaryDexterity() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Temporary Dexterity");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Dexterity +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Dexterity Expired");
                    //VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }
            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyTemporaryInitiative(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTemporaryInitiative() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Temporary Initiative");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Initiative +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Initiative Expired");
                   // VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyTemporaryStamina(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTemporaryStamina() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Temporary Stamina");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Stamina + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Stamina Expired");
                   // VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyTemporaryDraw(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTemporaryDraw() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Temporary Draw");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Draw + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Draw Expired" + stacks.ToString());
                    //VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
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
    public void ModifyEnrage(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyEnrage() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Enrage");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Enrage +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Enrage " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyShieldWall(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyShieldWall() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Shield Wall");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.shieldWallStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Shield Wall +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Shield Wall " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyFanOfKnives(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyFanOfKnives() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Fan Of Knives");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.fanOfKnivesStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Fan Of Knives");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Fan Of Knives Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyDivineFavour(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyDivineFavour() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Divine Favour");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.divineFavourStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Divine Favour");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Divine Favour Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyPhoenixForm(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyPhoenixForm() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Phoenix Form");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.phoenixFormStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Phoenix Form");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Phoenix Form Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyPoisonous(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyPoisonous() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Poisonous");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.poisonousStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Poisonous");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Poisonous Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyConsecration(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyConsecration() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Consecration");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.consecrationStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Consecration");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Consecration Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyVenomous(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyVenomous() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Venomous");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.venomousStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Venomous");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Venomous Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyOverload(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyOverload() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Overload");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.overloadStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Overload +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGainOverloadEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Overload Removed");
                    //VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }

            // Handle extra draw from fusion passive, if it is a player character and it is their activation
            if (stacks > 0 &&
                pManager.fusionStacks > 0 &&
                pManager.myCharacter != null &&
                pManager.myCharacter.controller == Controller.Player &&
                ActivationManager.Instance.EntityActivated == pManager.myCharacter
                )
            {
                // Draw a card for each stack of fusion
                for (int i = 0; i < pManager.fusionStacks; i++)
                {
                    CardController.Instance.DrawACardFromDrawPile(pManager.myCharacter);
                }
            }
        }


    }
    public void ModifyFusion(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyFusion() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Fusion");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.fusionStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Fusion +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Fusion Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }          
        }


    }
    public void ModifyPlantedFeet(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyPlantedFeet() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Planted Feet");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.plantedFeetStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Planted Feet!" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Planted Feet Removed");
                    //VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            // Update cost of cards in hand
            if(pManager.myCharacter != null)
            {
                CardController.Instance.OnMeleeAttackReductionModified(pManager.myCharacter);
            }           

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    #endregion

    // Special Defensive Passives
    #region
    public void ModifyRune(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyRune() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Rune");
        CharacterEntityModel character = pManager.myCharacter;
        
        // Increment stacks
        pManager.runeStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Rune +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Rune " + stacks.ToString());
                    //VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyBarrier(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBarrier() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Barrier");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.barrierStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Barrier +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Barrier " + stacks.ToString());
                    //VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    #endregion

    // Aura Passives
    #region
    public void ModifyEncouragingAura(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyEncouragingAura() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Encouraging Aura");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.encouragingAuraStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Encouraging Aura");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Encouraging Aura Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    public void ModifyShadowAura(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyShadowAura() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Shadow Aura");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.shadowAuraStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Shadow Aura");
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Shadow Aura Removed");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }


    }
    #endregion

    // Core % Modifier Passives
    #region
    public void ModifyWrath(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyWrath() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Wrath");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.wrathStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Wrath +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Wrath " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            // Update intent GUI, if enemy and attacking
            if (pManager.myCharacter.controller == Controller.AI &&
                pManager.myCharacter.myNextAction != null &&
                 (pManager.myCharacter.myNextAction.actionType == ActionType.AttackTarget ||
                 pManager.myCharacter.myNextAction.actionType == ActionType.AttackAllEnemies))
            {
                CharacterEntityController.Instance.UpdateEnemyIntentGUI(pManager.myCharacter);
            }
            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyWeakened(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyWeakened() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Weakened");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.weakenedStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Weakened + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Weakened " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            // Update intent GUI, if enemy and attacking
            if (pManager.myCharacter.controller == Controller.AI &&
                pManager.myCharacter.myNextAction != null &&
                 (pManager.myCharacter.myNextAction.actionType == ActionType.AttackTarget ||
                 pManager.myCharacter.myNextAction.actionType == ActionType.AttackAllEnemies))
            {
                CharacterEntityController.Instance.UpdateEnemyIntentGUI(pManager.myCharacter);
            }
            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyVulnerable(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyVulnerable() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Vulnerable");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.vulnerableStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Vulnerable + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Vulnerable " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            // Update intent GUI of ai's targetting this character
            foreach(CharacterEntityModel entity in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(pManager.myCharacter))
            {
                if(entity.controller == Controller.AI &&
                   entity.myNextAction != null &&
                   entity.currentActionTarget == pManager.myCharacter &&
                    pManager.myCharacter.myNextAction.actionType == ActionType.AttackTarget)
                {
                    CharacterEntityController.Instance.UpdateEnemyIntentGUI(entity);
                }
            }
            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyGrit(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyGrit() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Grit");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.gritStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Grit +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Grit " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
            }

            // Update intent GUI of ai's targetting this character
            foreach (CharacterEntityModel entity in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(pManager.myCharacter))
            {
                if (entity.controller == Controller.AI &&
                   entity.myNextAction != null &&
                   entity.currentActionTarget == pManager.myCharacter &&
                   entity.myNextAction.actionType == ActionType.AttackTarget)
                {
                    CharacterEntityController.Instance.UpdateEnemyIntentGUI(entity);
                }
            }
            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    #endregion

    // DoT Passives
    #region
    public void ModifyPoisoned(CharacterEntityModel applier, PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyPoisoned() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Poisoned");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Add venomous bonus from applier
        if (applier != null &&
            applier.pManager.venomousStacks > 0)
        {
            stacks += applier.pManager.venomousStacks;
        }

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.poisonedStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Poisoned + " + stacks.ToString());
                    VisualEffectManager.Instance.CreateApplyPoisonedEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Poisoned " + stacks.ToString());
                });
            }
            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyBurning(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBurning() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Burning");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.burningStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Burning +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateApplyBurningEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.transform.position, "Burning " + stacks.ToString());
                });
            }
            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    #endregion

    // Misc Passives
    #region
    public void ModifyTaunted(CharacterEntityModel taunter, PassiveManagerModel targetPManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyTaunted() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Taunted");
        CharacterEntityModel character = targetPManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(targetPManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(targetPManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks and cache taunter
        targetPManager.tauntStacks += stacks;
        targetPManager.myTaunter = taunter;

        // Null taunt variable when removing taunt
        if(targetPManager.tauntStacks == 0)
        {
            targetPManager.myTaunter = null;
        }

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Taunted");
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Taunted Removed");
                });
            }

            if (showVFX)
            {
                VisualEventManager.Instance.InsertTimeDelayInQueue(vfxDelay);
            }
        }
    }
    public void ModifyFireBallBonusDamage(PassiveManagerModel pManager, int stacks, bool showVFX = true, float vfxDelay = 0f)
    {
        Debug.Log("PassiveController.ModifyBonusFireBallDamage() called...");

        // Setup + Cache refs
        PassiveIconDataSO iconData = GetPassiveIconDataByName("Fire Ball Bonus Damage");
        CharacterEntityModel character = pManager.myCharacter;

        // Check for rune
        if (ShouldRuneBlockThisPassiveApplication(pManager, iconData, stacks))
        {
            // Character is protected by rune: Cancel this status application, remove a rune, then return.
            ModifyRune(pManager, -1, showVFX, vfxDelay);
            return;
        }

        // Increment stacks
        pManager.fireBallBonusDamageStacks += stacks;

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
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Fire Ball Damage +" + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralBuffEffect(character.characterEntityView.WorldPosition);
                });

            }

            else if (stacks < 0 && showVFX)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateStatusEffect(character.characterEntityView.WorldPosition, "Fire Ball Damage " + stacks.ToString());
                    VisualEffectManager.Instance.CreateGeneralDebuffEffect(character.characterEntityView.WorldPosition);
                });
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
