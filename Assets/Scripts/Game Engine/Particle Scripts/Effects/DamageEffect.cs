using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageEffect : MonoBehaviour
{
    [Header("Component References")]
    public TextMeshProUGUI amountText;
    public GameObject myImageParent;
    public Image heartImage;
    public Image shieldImage;
    public Animator myAnim;

    // Initialization + Setup
    #region
    public void InitializeSetup(int damageAmount, bool heal = false, bool healthModified = true)
    {
        transform.position = new Vector2(transform.position.x - 0.2f, transform.position.y);

        if (healthModified)
        {
            heartImage.gameObject.SetActive(true);
        }
        else
        {
            shieldImage.gameObject.SetActive(true);
        }

        if (heal == false)
        {
            amountText.text = "-" + damageAmount.ToString();
        }

        else if (heal == true)
        {
            amountText.text = "+" + damageAmount.ToString();
            amountText.color = Color.green;
        }

        ChooseRandomDirection();

    }
    // Second overload used for gain block text effect
    public void InitializeSetup(int blockGained)
    {
        transform.position = new Vector2(transform.position.x - 0.2f, transform.position.y);

        heartImage.gameObject.SetActive(false);
        shieldImage.gameObject.SetActive(true);

        amountText.text = "+" + blockGained.ToString();

        ChooseRandomDirection();
    }
    #endregion

    // Logic
    #region
    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    public void ChooseRandomDirection()
    {
        int randomNumber = Random.Range(0, 100);
        if (randomNumber < 50)
        {
            myAnim.SetTrigger("Right");
        }
        else
        {
            myAnim.SetTrigger("Left");
        }
    }
    #endregion
}


