using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    [Header("Properties")]
    public LivingEntity myLivingEntity;

    [Header("Component References")]
    public Animator animator;
    public GameObject visualParent;

    public void EnableView()
    {
        Debug.Log("TargetIndicator.EnableView() called...");

        visualParent.SetActive(true);
        PlayAnimation();
    }
    public void DisableView()
    {
        visualParent.SetActive(false);
    }

    private void PlayAnimation()
    {
        animator.SetTrigger("Breath");
    }

}
