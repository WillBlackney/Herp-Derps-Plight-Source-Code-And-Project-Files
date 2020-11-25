using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class VisualEffectManager : Singleton<VisualEffectManager>
{
    // Prefabs, Components And Properties
    #region
    [Header("Screen Overlay Components + Properties")]
    public GameObject edgeOverlayParent;
    public CanvasGroup edgeOverlayCg;

    [Header("Card VFX Prefabs")]
    public GameObject ExpendEffectPrefab;
    public GameObject GreenGlowTrailEffectPrefab;
    public GameObject YellowGlowTrailEffectPrefab;

    [Header("VFX Prefabs")]
    public GameObject DamageEffectPrefab;
    public GameObject StatusEffectPrefab;
    public GameObject GainBlockEffectPrefab;
    public GameObject LoseBlockEffectPrefab;
    public GameObject HealEffectPrefab;
    public GameObject AoeMeleeAttackEffectPrefab;

    [Header("Projectile Prefabs")]
    public GameObject arrow;
    public GameObject fireBall;
    public GameObject poisonBall;
    public GameObject shadowBall;
    public GameObject frostBall;
    public GameObject lightningBall;
    public GameObject holyBall;
    public GameObject fireMeteor;

    [Header("Nova Prefabs")]
    public GameObject fireNova;
    public GameObject shadowNova;
    public GameObject poisonNova;
    public GameObject lightningNova;
    public GameObject frostNova;
    public GameObject holyNova;

    [Header("Ritual Circle Prefabs")]
    public GameObject ritualCircleYellow;
    public GameObject ritualCirclePurple;

    [Header("Debuff Prefabs")]
    public GameObject redPillarBuff;
    public GameObject yellowPillarBuff;
    public GameObject gainPoisoned;
    public GameObject gainBurning;

    [Header("Passive Buff Prefabs")]
    public GameObject gainOverload;

    [Header("Explosion Prefabs")]
    public GameObject smallFrostExplosion;
    public GameObject smallPoisonExplosion;
    public GameObject smallLightningExplosion;
    public GameObject smallFireExplosion;
    public GameObject smallShadowExplosion;
    public GameObject ghostExplosionPurple;
    public GameObject confettiExplosionRainbow;
    public GameObject bloodSplatterEffect;
    public GameObject goldCoinExplosion;

    [Header("Melee Impact Prefab References")]
    public GameObject smallMeleeImpact;

    #endregion

    // CORE FUNCTIONS
    #region
    public void CreateEffectAtLocation(ParticleEffect effect, Vector3 location)
    {
        if (effect == ParticleEffect.None)
        {
            return;
        }

        // General Buff FX
        if (effect == ParticleEffect.GeneralBuff)
        {
            CreateGeneralBuffEffect(location);
        }
        else if (effect == ParticleEffect.GeneralDebuff)
        {
            CreateGeneralDebuffEffect(location);
        }
        else if (effect == ParticleEffect.ApplyPoisoned)
        {
            CreateApplyPoisonedEffect(location);
        }
        else if (effect == ParticleEffect.ApplyBurning)
        {
            CreateApplyBurningEffect(location);
        }
        else if (effect == ParticleEffect.GainOverload)
        {
            CreateGainOverloadEffect(location);
        }
        else if (effect == ParticleEffect.HealEffect)
        {
            CreateHealEffect(location);
        }
        // Explosions
        else if (effect == ParticleEffect.LightningExplosion)
        {
            CreateLightningExplosion(location);
        }
        else if (effect == ParticleEffect.FireExplosion)
        {
            CreateFireExplosion(location);
        }
        else if (effect == ParticleEffect.FrostExplosion)
        {
            CreateFrostExplosion(location);
        }
        else if (effect == ParticleEffect.PoisonExplosion)
        {
            CreatePoisonExplosion(location);
        }
        else if (effect == ParticleEffect.ShadowExplosion)
        {
            CreateShadowExplosion(location);
        }
        else if (effect == ParticleEffect.BloodExplosion)
        {
            CreateBloodExplosion(location);
        }
        else if (effect == ParticleEffect.BloodExplosion)
        {
            CreateGoldCoinExplosion(location);
        }        
        else if (effect == ParticleEffect.GhostExplosionPurple)
        {
            CreateGhostExplosionPurple(location);
        }
        else if (effect == ParticleEffect.ConfettiExplosionRainbow)
        {
            CreateConfettiExplosionRainbow(location);
        }

        // Impacts
        else if (effect == ParticleEffect.SmallMeleeImpact)
        {
            CreateSmallMeleeImpact(location);
        }

        // Novas
        else if (effect == ParticleEffect.FireNova)
        {
            CreateFireNova(location);
        }
        else if (effect == ParticleEffect.FrostNova)
        {
            CreateFrostNova(location);
        }
        else if (effect == ParticleEffect.PoisonNova)
        {
            CreatePoisonNova(location);
        }
        else if (effect == ParticleEffect.ShadowNova)
        {
            CreateShadowNova(location);
        }
        else if (effect == ParticleEffect.HolyNova)
        {
            CreateHolyNova(location);
        }
        else if (effect == ParticleEffect.LightningNova)
        {
            CreateLightningNova(location);
        }

        // Ritual Circle
        else if (effect == ParticleEffect.RitualCirclePurple)
        {
            CreateRitualCirclePurple(location);
        }
        else if (effect == ParticleEffect.RitualCircleYellow)
        {
            CreateRitualCircleYellow(location);
        }

        // Misc
        else if (effect == ParticleEffect.AoeMeleeArc)
        {
            CreateAoEMeleeArc(location);
        }
    }
    public void ShootProjectileAtLocation(ProjectileFired projectileFired, Vector3 start, Vector3 end, CoroutineData cData)
    {
        if (projectileFired == ProjectileFired.None)
        {
            return;
        }

        // SHOOT PROJECTILE SEQUENCE
        if (projectileFired == ProjectileFired.FireBall1)
        {
            ShootFireball(start, end, cData);
        }
        else if (projectileFired == ProjectileFired.PoisonBall1)
        {
            ShootPoisonBall(start, end, cData);
        }
        else if (projectileFired == ProjectileFired.ShadowBall1)
        {
            ShootShadowBall(start, end, cData);
        }
        else if (projectileFired == ProjectileFired.LightningBall1)
        {
            ShootLightningBall(start, end, cData);
        }
        else if (projectileFired == ProjectileFired.HolyBall1)
        {
            ShootHolyBall(start, end, cData);
        }
        else if (projectileFired == ProjectileFired.FrostBall1)
        {
            ShootFrostBall(start, end, cData);
        }
        else if (projectileFired == ProjectileFired.FireMeteor)
        {
            ShootFireMeteor(start, end, cData);
        }
    }
    #endregion

    // SCREEN OVERLAY LOGIC
    #region
    public void DoScreenOverlayEffect(ScreenOverlayType type, ScreenOverlayColor color, float duration, float fadeInDuration, float fadeOutDuration)
    {
        StartCoroutine(DoScreenOverlayEffectCoroutine(type, color, duration, fadeInDuration, fadeInDuration));
    }
    private IEnumerator DoScreenOverlayEffectCoroutine(ScreenOverlayType type, ScreenOverlayColor color, float duration, float fadeInDuration, float fadeOutDuration)
    {
        CanvasGroup cg = null;
        GameObject parent = null;
        Image image = null;

        if(type == ScreenOverlayType.EdgeOverlay)
        {
            cg = edgeOverlayCg;
            parent = edgeOverlayParent;
            image = parent.GetComponent<Image>();
        }

        // Set color
        image.color = ColorLibrary.Instance.GetOverlayColor(color);

        // Set starting values
        parent.SetActive(true);
        cg.alpha = 0;

        // fade in
        cg.DOFade(1f, fadeInDuration);
        yield return new WaitForSeconds(fadeInDuration);

        // pause for duration
        yield return new WaitForSeconds(duration);

        // fade out
        cg.DOFade(0f, fadeOutDuration);
        yield return new WaitForSeconds(fadeOutDuration);

    }
    #endregion

    // CARD FX
    #region
    // Glow Trail
    public ToonEffect CreateYellowGlowTrailEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(YellowGlowTrailEffectPrefab, location, YellowGlowTrailEffectPrefab.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);

        return teScript;
    }
    public ToonEffect CreateGreenGlowTrailEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(GreenGlowTrailEffectPrefab, location, GreenGlowTrailEffectPrefab.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);

        return teScript;
    }

    // Expend
    public void CreateExpendEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f, bool playSFX = true)
    {
        GameObject hn = Instantiate(ExpendEffectPrefab, location, ExpendEffectPrefab.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        if (playSFX)
        {
            AudioManager.Instance.PlaySoundPooled(Sound.Explosion_Fire_1);
        }
    }
    #endregion

    // GENERAL FX
    #region

    // Damage Text Value Effect
    public void CreateDamageEffect(Vector3 location, int damageAmount, bool heal = false, bool healthLost = true)
    {
        GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
        damageEffect.GetComponent<DamageEffect>().InitializeSetup(damageAmount, heal, healthLost);
    }
    public void CreateBlockGainedTextEffect(Vector3 location, int blockGained)
    {
        GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
        damageEffect.GetComponent<DamageEffect>().InitializeSetup(blockGained);
    }

    // Status Text Effect
    public void CreateStatusEffect(Vector3 location, string statusEffectName)
    {
        Color thisColor = Color.white;
        GameObject damageEffect = Instantiate(StatusEffectPrefab, location, Quaternion.identity);
        damageEffect.GetComponent<StatusEffect>().InitializeSetup(statusEffectName, thisColor);
    }

    // General Debuff
    public void CreateGeneralDebuffEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(redPillarBuff, location, redPillarBuff.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        AudioManager.Instance.PlaySoundPooled(Sound.Passive_General_Debuff);
    }

    // General Buff
    public void CreateGeneralBuffEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(yellowPillarBuff, location, yellowPillarBuff.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        AudioManager.Instance.PlaySoundPooled(Sound.Passive_General_Buff);
    }
    #endregion

    // BLOCK FX
    #region

    // Gain Block
    public void CreateGainBlockEffect(Vector3 location, int blockGained)
    {
        GameObject newImpactVFX = Instantiate(GainBlockEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<GainArmorEffect>().InitializeSetup(location, blockGained);
        CreateBlockGainedTextEffect(location, blockGained);
        AudioManager.Instance.PlaySoundPooled(Sound.Ability_Gain_Block);
    }

    // Gain Block Text Effect


    // Lose Block Effect
    public void CreateLoseBlockEffect(Vector3 location, int blockLost)
    {
        GameObject newImpactVFX = Instantiate(LoseBlockEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<GainArmorEffect>().InitializeSetup(location, blockLost);
        CreateDamageEffect(location, blockLost, false, false);
        AudioManager.Instance.PlaySoundPooled(Sound.Ability_Damaged_Block_Lost);
    }
    #endregion

    // PROJECTILES
    #region

    // Shoot Arrow
    public void ShootArrow(Vector3 startPos, Vector3 endPos, CoroutineData cData, float speed = 15)
    {
        Debug.Log("VisualEffectManager.ShootArrow() called...");
        StartCoroutine(ShootArrowCoroutine(startPos, endPos, cData, speed));
    }
    private IEnumerator ShootArrowCoroutine(Vector3 startPos, Vector3 endPos, CoroutineData cData, float speed)
    {
        AudioManager.Instance.PlaySoundPooled(Sound.Projectile_Arrow_Fired);
        GameObject go = Instantiate(arrow, startPos, Quaternion.identity);
        Projectile projectileScript = go.GetComponent<Projectile>();
        projectileScript.InitializeSetup(startPos, endPos, speed);
        yield return new WaitUntil(() => projectileScript.DestinationReached == true);
        cData.MarkAsCompleted();
    }

    // Fire Ball
    public void ShootFireball(Vector3 startPos, Vector3 endPos, CoroutineData cData, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.7f)
    {
        Debug.Log("VisualEffectManager.ShootToonFireball() called...");
        StartCoroutine(ShootFireballCoroutine(startPos, endPos, cData, speed, sortingOrderBonus, scaleModifier));
    }
    private IEnumerator ShootFireballCoroutine(Vector3 startPosition, Vector3 endPosition, CoroutineData cData, float speed, int sortingOrderBonus, float scaleModifier)
    {
        AudioManager.Instance.PlaySoundPooled(Sound.Projectile_Fireball_Fired);
        GameObject go = Instantiate(fireBall, startPosition, fireBall.transform.rotation);
        ToonProjectile tsScript = go.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        bool destinationReached = false;

        // insta explode if created at destination
        if (go.transform.position.x == endPosition.x &&
            go.transform.position.y == endPosition.y &&
            destinationReached == false)
        {
            destinationReached = true;
            tsScript.OnDestinationReached();
            AudioManager.Instance.PlaySoundPooled(Sound.Explosion_Fire_1);

            // Resolve early
            if(cData != null)
            {
                cData.MarkAsCompleted();
            }          
        }

        while (go.transform.position != endPosition && 
               destinationReached == false)
        {
            go.transform.position = Vector2.MoveTowards(go.transform.position, endPosition, speed * Time.deltaTime);

            if (go.transform.position.x == endPosition.x &&
                go.transform.position.y == endPosition.y)
            {
                tsScript.OnDestinationReached();
                AudioManager.Instance.PlaySoundPooled(Sound.Explosion_Fire_1);
                destinationReached = true;
            }
            yield return null;
        }

        // Resolve
        if (cData != null)
        {
            cData.MarkAsCompleted();
        }

    }

    // Shadow Ball
    public void ShootShadowBall(Vector3 startPos, Vector3 endPos, CoroutineData cData, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.7f)
    {
        Debug.Log("VisualEffectManager.ShootShadowBall() called...");
        StartCoroutine(ShootShadowBallCoroutine(startPos, endPos, cData, speed, sortingOrderBonus, scaleModifier));
    }
    private IEnumerator ShootShadowBallCoroutine(Vector3 startPosition, Vector3 endPosition, CoroutineData cData, float speed, int sortingOrderBonus, float scaleModifier)
    {
        AudioManager.Instance.PlaySoundPooled(Sound.Projectile_Shadowball_Fired);
        GameObject go = Instantiate(shadowBall, startPosition, shadowBall.transform.rotation);
        ToonProjectile tsScript = go.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        bool destinationReached = false;

        // insta explode if created at destination
        if (go.transform.position.x == endPosition.x &&
            go.transform.position.y == endPosition.y &&
            destinationReached == false)
        {
            destinationReached = true;
            tsScript.OnDestinationReached();
            AudioManager.Instance.PlaySoundPooled(Sound.Explosion_Shadow_1);

            // Resolve early
            if (cData != null)
            {
                cData.MarkAsCompleted();
            }
        }

        while (go.transform.position != endPosition &&
               destinationReached == false)
        {
            go.transform.position = Vector2.MoveTowards(go.transform.position, endPosition, speed * Time.deltaTime);

            if (go.transform.position.x == endPosition.x &&
                go.transform.position.y == endPosition.y)
            {
                tsScript.OnDestinationReached();
                AudioManager.Instance.PlaySoundPooled(Sound.Explosion_Shadow_1);
                destinationReached = true;
            }
            yield return null;
        }

        // Resolve
        if (cData != null)
        {
            cData.MarkAsCompleted();
        }

    }

    // Poison Ball
    public void ShootPoisonBall(Vector3 startPos, Vector3 endPos, CoroutineData cData, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.7f)
    {
        Debug.Log("VisualEffectManager.ShootPoisonBallCoroutine() called...");
        StartCoroutine(ShootPoisonBallCoroutine(startPos, endPos, cData, speed, sortingOrderBonus, scaleModifier));
    }
    private IEnumerator ShootPoisonBallCoroutine(Vector3 startPosition, Vector3 endPosition, CoroutineData cData, float speed, int sortingOrderBonus, float scaleModifier)
    {
        AudioManager.Instance.PlaySoundPooled(Sound.Projectile_Poison_Fired);
        GameObject go = Instantiate(poisonBall, startPosition, poisonBall.transform.rotation);
        ToonProjectile tsScript = go.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        bool destinationReached = false;

        // insta explode if created at destination
        if (go.transform.position.x == endPosition.x &&
            go.transform.position.y == endPosition.y &&
            destinationReached == false)
        {
            destinationReached = true;

            tsScript.OnDestinationReached();
            AudioManager.Instance.PlaySoundPooled(Sound.Explosion_Poison_1);

            // Resolve early
            if (cData != null)
            {
                cData.MarkAsCompleted();
            }
        }

        while (go.transform.position != endPosition &&
               destinationReached == false)
        {
            go.transform.position = Vector2.MoveTowards(go.transform.position, endPosition, speed * Time.deltaTime);

            if (go.transform.position.x == endPosition.x &&
                go.transform.position.y == endPosition.y)
            {
                tsScript.OnDestinationReached();
                AudioManager.Instance.PlaySoundPooled(Sound.Explosion_Poison_1);
                destinationReached = true;
            }
            yield return null;
        }

        // Resolve
        if (cData != null)
        {
            cData.MarkAsCompleted();
        }

    }

    // Lightning Ball
    public void ShootLightningBall(Vector3 startPos, Vector3 endPos, CoroutineData cData, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.7f)
    {
        Debug.Log("VisualEffectManager.ShootLightningBallCoroutine() called...");
        StartCoroutine(ShootLightningBallCoroutine(startPos, endPos, cData, speed, sortingOrderBonus, scaleModifier));
    }
    private IEnumerator ShootLightningBallCoroutine(Vector3 startPosition, Vector3 endPosition, CoroutineData cData, float speed, int sortingOrderBonus, float scaleModifier)
    {
        AudioManager.Instance.PlaySoundPooled(Sound.Projectile_Lightning_Fired);
        GameObject go = Instantiate(lightningBall, startPosition, lightningBall.transform.rotation);
        ToonProjectile tsScript = go.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        bool destinationReached = false;

        // insta explode if created at destination
        if (go.transform.position.x == endPosition.x &&
            go.transform.position.y == endPosition.y &&
            destinationReached == false)
        {
            destinationReached = true;

            tsScript.OnDestinationReached();
            AudioManager.Instance.PlaySoundPooled(Sound.Explosion_Lightning_1);

            // Resolve early
            if (cData != null)
            {
                cData.MarkAsCompleted();
            }
        }

        while (go.transform.position != endPosition &&
               destinationReached == false)
        {
            go.transform.position = Vector2.MoveTowards(go.transform.position, endPosition, speed * Time.deltaTime);

            if (go.transform.position.x == endPosition.x &&
                go.transform.position.y == endPosition.y)
            {
                tsScript.OnDestinationReached();
                AudioManager.Instance.PlaySoundPooled(Sound.Explosion_Lightning_1);
                destinationReached = true;
            }
            yield return null;
        }

        // Resolve
        if (cData != null)
        {
            cData.MarkAsCompleted();
        }

    }

    // Holy Ball
    public void ShootHolyBall(Vector3 startPos, Vector3 endPos, CoroutineData cData, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.7f)
    {
        Debug.Log("VisualEffectManager.ShootHolyBallCoroutine() called...");
        StartCoroutine(ShootHolyBallCoroutine(startPos, endPos, cData, speed, sortingOrderBonus, scaleModifier));
    }
    private IEnumerator ShootHolyBallCoroutine(Vector3 startPosition, Vector3 endPosition, CoroutineData cData, float speed, int sortingOrderBonus, float scaleModifier)
    {
        GameObject go = Instantiate(holyBall, startPosition, holyBall.transform.rotation);
        ToonProjectile tsScript = go.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        bool destinationReached = false;

        // insta explode if created at destination
        if (go.transform.position.x == endPosition.x &&
            go.transform.position.y == endPosition.y &&
            destinationReached == false)
        {
            destinationReached = true;

            tsScript.OnDestinationReached();

            // Resolve early
            if (cData != null)
            {
                cData.MarkAsCompleted();
            }
        }

        while (go.transform.position != endPosition &&
               destinationReached == false)
        {
            go.transform.position = Vector2.MoveTowards(go.transform.position, endPosition, speed * Time.deltaTime);

            if (go.transform.position.x == endPosition.x &&
                go.transform.position.y == endPosition.y)
            {
                tsScript.OnDestinationReached();
                destinationReached = true;
            }
            yield return null;
        }

        // Resolve
        if (cData != null)
        {
            cData.MarkAsCompleted();
        }

    }

    // Holy Ball
    public void ShootFrostBall(Vector3 startPos, Vector3 endPos, CoroutineData cData, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.7f)
    {
        Debug.Log("VisualEffectManager.ShootFrostBallCoroutine() called...");
        StartCoroutine(ShootFrostBallCoroutine(startPos, endPos, cData, speed, sortingOrderBonus, scaleModifier));
    }
    private IEnumerator ShootFrostBallCoroutine(Vector3 startPosition, Vector3 endPosition, CoroutineData cData, float speed, int sortingOrderBonus, float scaleModifier)
    {
        GameObject go = Instantiate(frostBall, startPosition, frostBall.transform.rotation);
        ToonProjectile tsScript = go.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        bool destinationReached = false;

        // insta explode if created at destination
        if (go.transform.position.x == endPosition.x &&
            go.transform.position.y == endPosition.y &&
            destinationReached == false)
        {
            destinationReached = true;

            tsScript.OnDestinationReached();

            // Resolve early
            if (cData != null)
            {
                cData.MarkAsCompleted();
            }
        }

        while (go.transform.position != endPosition &&
               destinationReached == false)
        {
            go.transform.position = Vector2.MoveTowards(go.transform.position, endPosition, speed * Time.deltaTime);

            if (go.transform.position.x == endPosition.x &&
                go.transform.position.y == endPosition.y)
            {
                tsScript.OnDestinationReached();
                destinationReached = true;
            }
            yield return null;
        }

        // Resolve
        if (cData != null)
        {
            cData.MarkAsCompleted();
        }

    }
    
    // Fire Meteor
    public void ShootFireMeteor(Vector3 startPos, Vector3 endPos, CoroutineData cData, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 3f)
    {
        Debug.Log("VisualEffectManager.ShootFireMeteor() called...");
        StartCoroutine(ShootFireMeteorCoroutine(startPos, endPos, cData, speed, sortingOrderBonus, scaleModifier));
    }
    private IEnumerator ShootFireMeteorCoroutine(Vector3 startPosition, Vector3 endPosition, CoroutineData cData, float speed, int sortingOrderBonus, float scaleModifier)
    {
        AudioManager.Instance.PlaySoundPooled(Sound.Projectile_Fireball_Fired);
        GameObject go = Instantiate(fireMeteor, startPosition, fireMeteor.transform.rotation);
        ToonProjectile tsScript = go.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        bool destinationReached = false;

        // insta explode if created at destination
        if (go.transform.position.x == endPosition.x &&
            go.transform.position.y == endPosition.y &&
            destinationReached == false)
        {
            destinationReached = true;
            tsScript.OnDestinationReached();
            AudioManager.Instance.PlaySoundPooled(Sound.Explosion_Fire_1);

            // Resolve early
            if (cData != null)
            {
                cData.MarkAsCompleted();
            }
        }

        while (go.transform.position != endPosition &&
               destinationReached == false)
        {
            go.transform.position = Vector2.MoveTowards(go.transform.position, endPosition, speed * Time.deltaTime);

            if (go.transform.position.x == endPosition.x &&
                go.transform.position.y == endPosition.y)
            {
                tsScript.OnDestinationReached();
                AudioManager.Instance.PlaySoundPooled(Sound.Explosion_Fire_1);
                destinationReached = true;
            }
            yield return null;
        }

        // Resolve
        if (cData != null)
        {
            cData.MarkAsCompleted();
        }

    }

    #endregion

    // APPLY BUFF + DEBUFF FX
    #region

    // Heal Effect
    public void CreateHealEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(HealEffectPrefab, location, HealEffectPrefab.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }

    // Apply Poisoned Effect
    public void CreateApplyPoisonedEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(gainPoisoned, location, gainPoisoned.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);

    }

    // Apply Burning Effect
    public void CreateApplyBurningEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        AudioManager.Instance.PlaySoundPooled(Sound.Passive_Burning_Gained);
        GameObject hn = Instantiate(gainBurning, location, gainBurning.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);

    }

    // Apply Overload Effect    
    public void CreateGainOverloadEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        AudioManager.Instance.PlaySoundPooled(Sound.Passive_Overload_Gained);
        GameObject hn = Instantiate(gainOverload, location, gainOverload.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }
    #endregion

    // MELEE ATTACK VFX
    #region
    // Small Melee Impact
    public void CreateSmallMeleeImpact(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Debug.Log("VisualEffectManager.CreateSmallMeleeImpact() called...");
        GameObject hn = Instantiate(smallMeleeImpact, location, smallMeleeImpact.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }

    // AoE Melee Arc
    public void CreateAoEMeleeArc(Vector3 location, int sortingOrderBonus = 15)
    {
        Debug.Log("VisualEffectManager.CreateAoEMeleeArcEffect() called...");
        GameObject hn = Instantiate(AoeMeleeAttackEffectPrefab, location, AoeMeleeAttackEffectPrefab.transform.rotation);
        BuffEffect teScript = hn.GetComponent<BuffEffect>();
        teScript.InitializeSetup(location, sortingOrderBonus);
    }
    #endregion

    // EXPLOSIONS
    #region

    // Gold Coin Explosion
    public void CreateGoldCoinExplosion(Vector3 location, int sortingOrderBonus = 0, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(goldCoinExplosion, location, goldCoinExplosion.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }
    // Blood Explosion
    public void CreateBloodExplosion(Vector3 location, int sortingOrderBonus = 0, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(bloodSplatterEffect, location, bloodSplatterEffect.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }

    // Lightning Explosion
    public void CreateLightningExplosion(Vector3 location, int sortingOrderBonus = 0, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(smallLightningExplosion, location, smallLightningExplosion.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        AudioManager.Instance.PlaySoundPooled(Sound.Explosion_Lightning_1);
    }

    // Fire Explosion
    public void CreateFireExplosion(Vector3 location, int sortingOrderBonus = 0, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(smallFireExplosion, location, smallFireExplosion.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        AudioManager.Instance.PlaySoundPooled(Sound.Explosion_Fire_1);
    }

    // Poison Explosion
    public void CreatePoisonExplosion(Vector3 location, int sortingOrderBonus = 0, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(smallPoisonExplosion, location, smallPoisonExplosion.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        AudioManager.Instance.PlaySoundPooled(Sound.Explosion_Poison_1);
    }

    // Frost Explosion
    public void CreateFrostExplosion(Vector3 location, int sortingOrderBonus = 0, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(smallFrostExplosion, location, smallFrostExplosion.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }

    // Shadow Explosion
    public void CreateShadowExplosion(Vector3 location, int sortingOrderBonus = 0, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(smallShadowExplosion, location, smallShadowExplosion.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        AudioManager.Instance.PlaySoundPooled(Sound.Explosion_Shadow_1);
    }
    // Ghost Explosion Purple
    public void CreateGhostExplosionPurple(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(ghostExplosionPurple, location, ghostExplosionPurple.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        AudioManager.Instance.PlaySoundPooled(Sound.Passive_General_Debuff);
    }
    // Confetti Explosion Rainbow
    public void CreateConfettiExplosionRainbow(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(confettiExplosionRainbow, location, confettiExplosionRainbow.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        //AudioManager.Instance.PlaySound(Sound.Passive_General_Debuff);
    }
    #endregion

    // NOVAS
    #region
    // Fire Nova
    public void CreateFireNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(fireNova, location, fireNova.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }

    // Poison Nova
    public void CreatePoisonNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(poisonNova, location, poisonNova.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }

    // Frost Nova
    public void CreateFrostNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(frostNova, location, frostNova.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }

    // Lightning Nova
    public void CreateLightningNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(lightningNova, location, lightningNova.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }

    // Shadow Nova
    public void CreateShadowNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(shadowNova, location, shadowNova.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }

    // Holy Nova
    public void CreateHolyNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(holyNova, location, holyNova.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }
    #endregion

    // RITUAL CIRCLES
    #region
    public void CreateRitualCircleYellow(Vector3 location, int sortingOrderBonus = 5, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(ritualCircleYellow, location, ritualCircleYellow.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }
    public void CreateRitualCirclePurple(Vector3 location, int sortingOrderBonus = 5, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(ritualCirclePurple, location, ritualCirclePurple.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }
    #endregion


}
