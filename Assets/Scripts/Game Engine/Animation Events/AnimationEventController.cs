using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : Singleton<AnimationEventController>
{
    // Core Functions
    #region
    public void PlayAnimationEvent(AnimationEventData vEvent, CharacterEntityModel user = null, CharacterEntityModel target = null)
    {
        if(vEvent.eventType == AnimationEventType.CameraShake)
        {
            ResolveCameraShake(vEvent);
        }

        else if (vEvent.eventType == AnimationEventType.Delay)
        {
            ResolveDelay(vEvent);
        }

        else if (vEvent.eventType == AnimationEventType.CharacterAnimation)
        {
            ResolveCharacterAnimation(vEvent, user, target);
        }

        else if (vEvent.eventType == AnimationEventType.ParticleEffect)
        {
            ResolveParticleEffect(vEvent, user, target);
        }

        else if (vEvent.eventType == AnimationEventType.Movement)
        {
            ResolveMovement(vEvent, user, target);
        }
        else if (vEvent.eventType == AnimationEventType.SoundEffect)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => AudioManager.Instance.PlaySound(vEvent.soundEffect));
        }
    }
    #endregion

    // Handle specific events
    #region
    private void ResolveCameraShake(AnimationEventData vEvent)
    {
        VisualEventManager.Instance.CreateVisualEvent(() => CameraManager.Instance.CreateCameraShake(vEvent.cameraShake));
    }
    private void ResolveDelay(AnimationEventData vEvent)
    {
        VisualEventManager.Instance.InsertTimeDelayInQueue(vEvent.delayDuration);
    }
    private void ResolveCharacterAnimation(AnimationEventData vEvent, CharacterEntityModel user, CharacterEntityModel target)
    {
        // Melee Attack 
        if (vEvent.characterAnimation == CharacterAnimation.MeleeAttack)
        {
            CoroutineData cData = new CoroutineData();
            VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.TriggerMeleeAttackAnimation(user.characterEntityView, cData), cData);
        }
        // AoE Melee Attack 
        else if (vEvent.characterAnimation == CharacterAnimation.AoeMeleeAttack)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.TriggerAoeMeleeAttackAnimation(user.characterEntityView));
        }
        // Skill
        else if (vEvent.characterAnimation == CharacterAnimation.Skill)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.PlaySkillAnimation(user.characterEntityView));
        }
        // Shoot Bow 
        else if (vEvent.characterAnimation == CharacterAnimation.ShootBow)
        {
            // Character shoot bow animation
            CoroutineData cData = new CoroutineData();
            VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.PlayShootBowAnimation(user.characterEntityView, cData), cData);

            // Create and launch arrow projectile
            CoroutineData cData2 = new CoroutineData();
            VisualEventManager.Instance.CreateVisualEvent(() =>
            VisualEffectManager.Instance.ShootArrow(user.characterEntityView.WorldPosition, target.characterEntityView.WorldPosition, cData2), cData2);
        }
        // Shoot Projectile 
        else if (vEvent.characterAnimation == CharacterAnimation.ShootProjectile)
        {
            // Play character shoot anim
            VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.TriggerShootProjectileAnimation(user.characterEntityView));

            // Create projectile
            CoroutineData cData = new CoroutineData();
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.ShootProjectileAtLocation
            (vEvent.projectileFired, user.characterEntityView.WorldPosition, target.characterEntityView.WorldPosition, cData), cData);
        }
    }
    private void ResolveParticleEffect(AnimationEventData vEvent, CharacterEntityModel user, CharacterEntityModel target = null)
    {
        if(vEvent.onCharacter == CreateOnCharacter.Self)
        {
            VisualEventManager.Instance.CreateVisualEvent(() =>
            VisualEffectManager.Instance.CreateEffectAtLocation(vEvent.particleEffect, user.characterEntityView.WorldPosition));
        }
        else if (vEvent.onCharacter == CreateOnCharacter.Target)
        {
            VisualEventManager.Instance.CreateVisualEvent(() =>
            VisualEffectManager.Instance.CreateEffectAtLocation(vEvent.particleEffect, target.characterEntityView.WorldPosition));
        }
        else if (vEvent.onCharacter == CreateOnCharacter.AllAllies)
        {
            foreach(CharacterEntityModel ally in CharacterEntityController.Instance.GetAllAlliesOfCharacter(user))
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateEffectAtLocation(vEvent.particleEffect, ally.characterEntityView.WorldPosition));
            }           
        }
        else if (vEvent.onCharacter == CreateOnCharacter.AllEnemies)
        {
            foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(user))
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateEffectAtLocation(vEvent.particleEffect, enemy.characterEntityView.WorldPosition));
            }
        }
        else if (vEvent.onCharacter == CreateOnCharacter.All)
        {
            foreach (CharacterEntityModel character in CharacterEntityController.Instance.AllCharacters)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateEffectAtLocation(vEvent.particleEffect, character.characterEntityView.WorldPosition));
            }
        }
    }
    private void ResolveMovement(AnimationEventData vEvent, CharacterEntityModel user, CharacterEntityModel target)
    {
        if (vEvent.movementAnimation == MovementAnimEvent.MoveTowardsTarget &&
            target != null)
        {
            user.hasMovedOffStartingNode = true;
            LevelNode node = target.levelNode;
            CoroutineData cData = new CoroutineData();
            VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.MoveAttackerToTargetNodeAttackPosition(user, node, cData), cData);
        }
        else if (vEvent.movementAnimation == MovementAnimEvent.MoveToCentre)
        {
            user.hasMovedOffStartingNode = true;
            CoroutineData cData = new CoroutineData();
            VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.MoveAttackerToCentrePosition(user, cData), cData);
        }
        else if (vEvent.movementAnimation == MovementAnimEvent.MoveToMyNode)
        {
            user.hasMovedOffStartingNode = false;
            CoroutineData cData = new CoroutineData();
            LevelNode node = user.levelNode;
            VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.MoveEntityToNodeCentre(user, node, cData), cData, QueuePosition.Back, 0.3f, 0);
        }
    }

    #endregion
}
