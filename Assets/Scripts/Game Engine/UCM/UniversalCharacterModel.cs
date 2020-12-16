using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Spriter2UnityDX;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UniversalCharacterModel : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Core Components")]
    public Animator myAnimator;
    public EntityRenderer myEntityRenderer;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("All Model Element References")]
    public UniversalCharacterModelElement[] allModelElements;
    public SpriteMask[] allHeadWearSpriteMasks;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Active Particle References")]
    [HideInInspector] public UniversalCharacterModelElement activeChestParticles;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Weapon References")]
    public List<UniversalCharacterModelElement> allMainHandWeapons;
    public List<UniversalCharacterModelElement> allOffHandWeapons;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Active Body Part References")]
    [HideInInspector] public UniversalCharacterModelElement activeHead;
    [HideInInspector] public UniversalCharacterModelElement activeFace;
    [HideInInspector] public UniversalCharacterModelElement activeLeftLeg;
    [HideInInspector] public UniversalCharacterModelElement activeRightLeg;
    [HideInInspector] public UniversalCharacterModelElement activeRightHand;
    [HideInInspector] public UniversalCharacterModelElement activeRightArm;
    [HideInInspector] public UniversalCharacterModelElement activeLeftHand;
    [HideInInspector] public UniversalCharacterModelElement activeLeftArm;
    [HideInInspector] public UniversalCharacterModelElement activeChest;

    [Header("Active Clothing Part References")]
    [HideInInspector] public UniversalCharacterModelElement activeHeadWear;
    [HideInInspector] public UniversalCharacterModelElement activeChestWear;
    [HideInInspector] public UniversalCharacterModelElement activeLeftLegWear;
    [HideInInspector] public UniversalCharacterModelElement activeRightLegWear;
    [HideInInspector] public UniversalCharacterModelElement activeLeftArmWear;
    [HideInInspector] public UniversalCharacterModelElement activeRightArmWear;
    [HideInInspector] public UniversalCharacterModelElement activeLeftHandWear;
    [HideInInspector] public UniversalCharacterModelElement activeRightHandWear;
    [HideInInspector] public UniversalCharacterModelElement activeMainHandWeapon;
    [HideInInspector] public UniversalCharacterModelElement activeOffHandWeapon;
    #endregion

    // Initialization
    #region
    private void Start()
    {
        CharacterModelController.Instance.AutoSetHeadMaskOrderInLayer(this);
    }
    #endregion

    // Animation Logic
    #region 
    public void SetBaseAnim()
    {
        myAnimator.SetTrigger("Base");
    }
    public void SetIdleAnim()
    {
        myAnimator.SetTrigger("Idle");
    }

    #endregion

}
