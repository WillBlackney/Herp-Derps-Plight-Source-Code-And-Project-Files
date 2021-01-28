using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CharacterPanelViewController : Singleton<CharacterPanelViewController>
{
    // Properties + Components
    #region
    [Header("Core Component References")]
    [SerializeField] GameObject visualParent;

    [Header("Panel References")]
    [SerializeField] CharacterPanelView[] allCharacterPanels;

    [Header("Text References")]
    [SerializeField] TextMeshProUGUI currentCharacterCountText;
    [SerializeField] TextMeshProUGUI maxCharacterCountText;
    #endregion

    // Getters + Accessors
    #region
    public CharacterPanelView[] AllCharacterPanels
    {
        get { return allCharacterPanels; }
    }
    #endregion

    // Show + Hide Views Logic
    #region
    public void ShowCharacterRosterPanel()
    {
        visualParent.SetActive(true);
    }
    public void HideCharacterRosterPanel()
    {
        visualParent.SetActive(false);
    }
    #endregion

    // Build Roster Logic
    #region
    public void RebuildAllViews()
    {
        BuildAllCharacterPanelsFromCharacterDataSet(CharacterDataController.Instance.AllPlayerCharacters);
        UpdateCurrentCharacterCountText();
        UpdateMaxRosterSizeText();
    }
    private void HideAllCharacterPanels()
    {
        foreach (CharacterPanelView cpv in AllCharacterPanels)
            cpv.visualParent.SetActive(false);
    }
    public void BuildAllCharacterPanelsFromCharacterDataSet(List<CharacterData> characters)
    {
        HideAllCharacterPanels();

        for(int i = 0; i < characters.Count; i++)
        {
            BuildCharacterPanelFromCharacterData(AllCharacterPanels[i], characters[i]);
        }
    }
    private void BuildCharacterPanelFromCharacterData(CharacterPanelView panel, CharacterData data)
    {
        panel.characterDataRef = data; 
        panel.visualParent.SetActive(true);
        panel.nameText.text = data.myName;
        panel.levelText.text = data.currentLevel.ToString();
        CharacterModelController.Instance.BuildModelMugShotFromStringReferences(panel.ucm, data.modelParts);

        // Health bar slider logic
        float currentHealthFloat = data.health;
        float currentMaxHealthFloat = data.MaxHealthTotal;
        float healthBarFloat = currentHealthFloat / currentMaxHealthFloat;
        panel.healthBar.value = healthBarFloat;
        panel.currentHealthText.text = data.health.ToString();
        panel.maxHealthText.text = data.MaxHealthTotal.ToString();

        // Xp bar slider logic
        float currentXP = data.currentXP;
        float currentMaxXpFloat = data.currentMaxXP;
        float xpBarFloat = currentXP / currentMaxXpFloat;
        panel.xpBar.value = xpBarFloat;
    }
    private void UpdateMaxRosterSizeText()
    {
        maxCharacterCountText.text = CharacterDataController.Instance.CurrentMaxRosterSize.ToString();
    }
    private void UpdateCurrentCharacterCountText()
    {
        currentCharacterCountText.text = CharacterDataController.Instance.AllPlayerCharacters.Count.ToString();
    }
    public void OnCharacterPanelViewClicked(CharacterPanelView panel)
    {
        CharacterSheetViewController.Instance.OnCharacterPanelViewClicked(panel);
    }
    #endregion
}
