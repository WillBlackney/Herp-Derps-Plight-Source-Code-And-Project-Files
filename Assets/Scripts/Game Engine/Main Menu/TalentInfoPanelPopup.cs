using UnityEngine;
using TMPro;

public class TalentInfoPanelPopup : MonoBehaviour
{
    [SerializeField] private GameObject visualParent;
    [SerializeField] private TextMeshProUGUI infoPopupText;

    public void BuildMe(string descriptionText)
    {
        visualParent.SetActive(true);
        infoPopupText.text = descriptionText;
    }
    public void HideMe()
    {
        visualParent.SetActive(false);
    }
}
