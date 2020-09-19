using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class AnimationEventData 
{
    // This class is used by CardDataSO and EnemyEffect/EnemyDataSO to
    // assign animations, particle VFX and other visual events to
    // cards and enemy abilities. This allows designers to assign
    // these types of events via the inspector, instead of programmatically
    // creating these effects on cards/enemy effects individually/

    public ParticleEffect effectOnSelfAtStart;
    public MovementAnimEvent startingMovementEvent;
    public CharacterAnimation characterAnimation;
    public ParticleEffect onCharacterAnimationFinish;
    [ShowIf("ShowProjectileFired")]
    public ProjectileFired projectileFired;
    public ParticleEffect onTargetHit;

    public bool ShowProjectileFired()
    {
        if (characterAnimation == CharacterAnimation.ShootProjectile)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

