using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ManaPoolVisual : MonoBehaviour {

    public int TestFullCrystals;
    public int TestTotalCrystalsThisTurn;

    public Image[] Crystals;
    public Text ProgressText;

    private int totalCrystals;
    public int TotalCrystals
    {
        get{ return totalCrystals; }

        set
        {
            //Debug.Log("Changed total mana to: " + value);

            if (value > Crystals.Length)
                totalCrystals = Crystals.Length;
            else if (value < 0)
                totalCrystals = 0;
            else
                totalCrystals = value;

            for (int i = 0; i < Crystals.Length; i++)
            {
                if (i < totalCrystals)
                {
                    if (Crystals[i].color == Color.clear)
                        Crystals[i].color = Color.gray;
                }
                else
                    Crystals[i].color = Color.clear;
            }

            // update the text
            ProgressText.text = string.Format("{0}/{1}", availableCrystals.ToString(), totalCrystals.ToString());
        }
    }

    private int availableCrystals;
    public int AvailableCrystals
    {
        get{ return availableCrystals; }

        set
        {
            //Debug.Log("Changed mana this turn to: " + value);

            if (value > totalCrystals)
                availableCrystals = totalCrystals;
            else if (value < 0)
                availableCrystals = 0;
            else
                availableCrystals = value;

            for (int i = 0; i < totalCrystals; i++)
            {
                if (i < availableCrystals)
                    Crystals[i].color = Color.white;
                else
                    Crystals[i].color = Color.gray;
            }

            // update the text
            ProgressText.text = string.Format("{0}/{1}", availableCrystals.ToString(), totalCrystals.ToString());

        }
    }

    void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            TotalCrystals = TestTotalCrystalsThisTurn;
            AvailableCrystals = TestFullCrystals;
        }
    }
	
}
