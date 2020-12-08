using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UniversalCharacterModel : MonoBehaviour
{
    [Header("All Model Element References")]
    public UniversalCharacterModelElement[] allModelElements;
    public SpriteMask[] allHeadWearSpriteMasks;

    // NEW Properties + Component References
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

    [Header("Active Particle References")]
    [HideInInspector] public UniversalCharacterModelElement activeChestParticles;

    [Header("Head Wear References")]
    public List<UniversalCharacterModelElement> allHeadWear;

    [Header("Chest Wear References")]
    public List<UniversalCharacterModelElement> allChestWear;

    [Header("Leg Wear References")]
    public List<UniversalCharacterModelElement> allLeftLegWear;
    public List<UniversalCharacterModelElement> allRightLegWear;

    [Header("Arm Wear References")]
    public List<UniversalCharacterModelElement> allLeftArmWear;
    public List<UniversalCharacterModelElement> allRightArmWear;
    public List<UniversalCharacterModelElement> allLeftHandWear;
    public List<UniversalCharacterModelElement> allRightHandWear;

    [Header("Weapon References")]
    public List<UniversalCharacterModelElement> allMainHandWeapons;
    public List<UniversalCharacterModelElement> allOffHandWeapons;

    [Header("Human Model References")]
    public List<UniversalCharacterModelElement> humanHeads;
    public List<UniversalCharacterModelElement> humanFaces;
    public UniversalCharacterModelElement humanLeftLeg;
    public UniversalCharacterModelElement humanRightLeg;
    public UniversalCharacterModelElement humanRightHand;
    public UniversalCharacterModelElement humanRightArm;
    public UniversalCharacterModelElement humanLeftHand;
    public UniversalCharacterModelElement humanLeftArm;
    public UniversalCharacterModelElement humanChest;

    [Header("Orc Model References")]
    public List<UniversalCharacterModelElement> orcHeads;
    public List<UniversalCharacterModelElement> orcFaces;
    public UniversalCharacterModelElement orcLeftLeg;
    public UniversalCharacterModelElement orcRightLeg;
    public UniversalCharacterModelElement orcRightHand;
    public UniversalCharacterModelElement orcRightArm;
    public UniversalCharacterModelElement orcLeftHand;
    public UniversalCharacterModelElement orcLeftArm;
    public UniversalCharacterModelElement orcChest;

    [Header("Undead Model References")]
    public List<UniversalCharacterModelElement> undeadHeads;
    public List<UniversalCharacterModelElement> undeadFaces;
    public UniversalCharacterModelElement undeadLeftLeg;
    public UniversalCharacterModelElement undeadRightLeg;
    public UniversalCharacterModelElement undeadRightHand;
    public UniversalCharacterModelElement undeadRightArm;
    public UniversalCharacterModelElement undeadLeftHand;
    public UniversalCharacterModelElement undeadLeftArm;
    public UniversalCharacterModelElement undeadChest;

    [Header("Elf Model References")]
    public List<UniversalCharacterModelElement> elfHeads;
    public List<UniversalCharacterModelElement> elfFaces;
    public UniversalCharacterModelElement elfLeftLeg;
    public UniversalCharacterModelElement elfRightLeg;
    public UniversalCharacterModelElement elfRightHand;
    public UniversalCharacterModelElement elfRightArm;
    public UniversalCharacterModelElement elfLeftHand;
    public UniversalCharacterModelElement elfLeftArm;
    public UniversalCharacterModelElement elfChest;

    [Header("Satyr Model References")]
    public List<UniversalCharacterModelElement> satyrHeads;
    public List<UniversalCharacterModelElement> satyrFaces;
    public UniversalCharacterModelElement satyrLeftLeg;
    public UniversalCharacterModelElement satyrRightLeg;
    public UniversalCharacterModelElement satyrRightHand;
    public UniversalCharacterModelElement satyrRightArm;
    public UniversalCharacterModelElement satyrLeftHand;
    public UniversalCharacterModelElement satyrLeftArm;
    public UniversalCharacterModelElement satyrChest;

    [Header("Gnoll Model References")]
    public List<UniversalCharacterModelElement> gnollHeads;
    public List<UniversalCharacterModelElement> gnollFaces;
    public UniversalCharacterModelElement gnollLeftLeg;
    public UniversalCharacterModelElement gnollRightLeg;
    public UniversalCharacterModelElement gnollRightHand;
    public UniversalCharacterModelElement gnollRightArm;
    public UniversalCharacterModelElement gnollLeftHand;
    public UniversalCharacterModelElement gnollLeftArm;
    public UniversalCharacterModelElement gnollChest;

    // Properties + Component References
    #region
    [Header("Component References")]
    public Animator myAnimator;
    public Transform scalingParent;

    [Header("Properties")]
    public CharacterRace myModelRace;
    public bool initializedScaling;
    public float normalSizeScale;
    public float smallSizeScale;
    public float largeSizeScale;

    #endregion

    private void Start()
    {
        CharacterModelController.AutoSetHeadMaskOrderInLayer(this);
        SetScalingValues();
    }
    private void OnEnable()
    {
        SetScalingValues();
    }
    private void SetScalingValues()
    {
        if (!initializedScaling)
        {
            float currentScale = transform.localScale.x;
            normalSizeScale = currentScale;
            smallSizeScale = currentScale * 0.9f;
            largeSizeScale = currentScale * 1.1f;
            initializedScaling = true;
        }
    }


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
