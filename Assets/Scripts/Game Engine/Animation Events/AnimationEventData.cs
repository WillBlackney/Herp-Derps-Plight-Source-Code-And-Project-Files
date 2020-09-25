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
    public AnimationEventType eventType;


    [VerticalGroup("General Properties")]
    [LabelWidth(250)]
    [ShowIf("ShowParticleEffect")]
    public ParticleEffect particleEffect;


    [VerticalGroup("General Properties")]
    [LabelWidth(250)]
    [ShowIf("ShowCameraShake")]
    public CameraShakeType cameraShake;


    [VerticalGroup("General Properties")]
    [LabelWidth(250)]
    [ShowIf("ShowMovementAnimation")]
    public MovementAnimEvent movementAnimation;

    [VerticalGroup("General Properties")]
    [LabelWidth(250)]
    [ShowIf("ShowSoundEffect")]
    public Sound soundEffect;


    [VerticalGroup("General Properties")]
    [LabelWidth(250)]
    [ShowIf("ShowCharacterAnimation")]
    public CharacterAnimation characterAnimation;


    [VerticalGroup("General Properties")]
    [LabelWidth(250)]
    [ShowIf("ShowDelayDuration")]
    public float delayDuration;


    [VerticalGroup("General Properties")]
    [LabelWidth(250)]
    [ShowIf("ShowProjectileFired")]
    public ProjectileFired projectileFired;

    [VerticalGroup("General Properties")]
    [LabelWidth(250)]
    [ShowIf("ShowOnCharacter")]
    public CreateOnCharacter onCharacter;



    public bool ShowSoundEffect()
    {
        return eventType == AnimationEventType.SoundEffect;
    }
    public bool ShowParticleEffect()
    {
        return eventType == AnimationEventType.ParticleEffect;
    }
    public bool ShowCameraShake()
    {
        return eventType == AnimationEventType.CameraShake;
    }
    public bool ShowMovementAnimation()
    {
        return eventType == AnimationEventType.Movement;
    }
    public bool ShowCharacterAnimation()
    {
        return eventType == AnimationEventType.CharacterAnimation;
    }
    public bool ShowDelayDuration()
    {
        return eventType == AnimationEventType.Delay;
    }
    public bool ShowProjectileFired()
    {
        return characterAnimation == CharacterAnimation.ShootProjectile;
    }
    public bool ShowOnCharacter()
    {
        return eventType == AnimationEventType.ParticleEffect;
    }


}

