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

    [VerticalGroup("General Properties")]
    [LabelWidth(250)]
    public ParticleEffect effectOnSelfAtStart;

    [VerticalGroup("General Properties")]
    [LabelWidth(250)]
    public MovementAnimEvent startingMovementEvent;

    // NOTE: this bool should be true if the card used
    // is a melee attack that applies a buff to self
    // However, it should normally be false if the effect
    // applies a debuff to the enemy. This is purely to
    // to make visual event sequences look better.
    // It also prevents characters from running back and 
    // forth between the target and home node. For example, 
    // if a card read 'Deal damage to a random enemy 5 times',
    // and this was wrongly marked as true, the character would return 
    // back to its node at the end of each attack.
    [VerticalGroup("General Properties")]
    [LabelWidth(250)]
    [ShowIf("ShowReturnToMyNodeOnCardEffectResolved")]
    public bool returnToMyNodeOnCardEffectResolved;

    [VerticalGroup("General Properties")]
    [LabelWidth(250)]
    public CharacterAnimation characterAnimation;

    [VerticalGroup("General Properties")]
    [LabelWidth(250)]
    public ParticleEffect onCharacterAnimationFinish;

    [VerticalGroup("General Properties")]
    [LabelWidth(250)]
    [ShowIf("ShowProjectileFired")]
    public ProjectileFired projectileFired;

    [VerticalGroup("General Properties")]
    [LabelWidth(250)]
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
    public bool ShowReturnToMyNodeOnCardEffectResolved()
    {
        if (startingMovementEvent == MovementAnimEvent.MoveToCentre ||
            startingMovementEvent == MovementAnimEvent.MoveTowardsTarget)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

