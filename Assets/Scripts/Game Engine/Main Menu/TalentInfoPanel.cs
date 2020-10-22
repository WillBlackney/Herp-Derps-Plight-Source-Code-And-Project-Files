using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalentInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI talentNameText;
    [SerializeField] private Image talentImage;

    public void BuildFromTalentPairingModel(TalentPairingModel data)
    {
        gameObject.SetActive(true);
        talentNameText.text = data.talentSchool.ToString() + " +" + data.talentLevel.ToString();
        talentImage.sprite = SpriteLibrary.Instance.GetTalentSchoolSpriteFromEnumData(data.talentSchool);
    }
    
}
