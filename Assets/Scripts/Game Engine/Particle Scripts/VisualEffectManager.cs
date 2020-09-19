using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualEffectManager : Singleton<VisualEffectManager>
{
    [Header("Properties")]
    public int campsiteVfxSortingLayer;

    [Header("VFX Prefab References")]
    public GameObject DamageEffectPrefab;
    public GameObject StatusEffectPrefab;
    public GameObject ImpactEffectPrefab;
    public GameObject MeleeAttackEffectPrefab;
    public GameObject GainBlockEffectPrefab;
    public GameObject LoseBlockEffectPrefab;
    public GameObject BuffEffectPrefab;
    public GameObject DebuffEffectPrefab;
    public GameObject PoisonAppliedEffectPrefab;
    public GameObject DamagedByPoisonEffect;
    public GameObject TeleportEffectPrefab;
    public GameObject HealEffectPrefab;
    public GameObject AoeMeleeAttackEffectPrefab;

    [Header("Projectile Prefab References")]
    public GameObject ArrowPrefab;
    public GameObject FireBallPrefab;
    public GameObject ShadowBallPrefab;
    public GameObject FrostBoltPrefab;
    public GameObject HolyFirePrefab;

    [Header("TOON Projectile Prefab References")]
    public GameObject toonFireBall;
    public GameObject toonPoisonBall;
    public GameObject toonShadowBall;
    public GameObject toonFrostBall;
    public GameObject toonLightningBall;
    public GameObject toonHolyBall;

    [Header("TOON Nova Prefab References")]
    public GameObject toonFireNova;
    public GameObject toonShadowNova;
    public GameObject toonPoisonNova;
    public GameObject toonLightningNova;
    public GameObject toonFrostNova;
    public GameObject toonHolyNova;
    public GameObject toonBlueNova;

    [Header("TOON Buff Prefab References")]
    public GameObject toonGeneralBuffPrefab;
    public GameObject toonShadowBuffPrefab;
    public GameObject toonHolyBuffPrefab;
    public GameObject toonGainEnergyPrefab;
    public GameObject toonGainCoreStatPrefab;
    public GameObject toonGainCamoflagePrefab;

    [Header("TOON Debuff Prefab References")]
    public GameObject redPillarBuff;
    public GameObject yellowPillarBuff;
    public GameObject toonApplyStunned;
    public GameObject toonApplyPoisoned;
    public GameObject toonApplyBurning;
    public GameObject toonApplyShocked;
    public GameObject toonApplyChilled;
    public GameObject toonApplyVulnerable;
    public GameObject toonApplyWeakened;

    [Header("TOON Explosion Prefab References")]
    public GameObject smallFrostExplosion;
    public GameObject smallPoisonExplosion;
    public GameObject smallLightningExplosion;
    public GameObject smallFireExplosion;
    public GameObject smallShadowExplosion;

    [Header("TOON Melee Impact Prefab References")]
    public GameObject smallMeleeImpact;
    public GameObject bigMeleeImpact;

    [Header("TOON Misc Prefab References")]
    public GameObject hardLandingEffect;
    public GameObject bloodSplatterEffect; 
    
    // Create VFX
    #region
   
    public IEnumerator CreateBuffEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(BuffEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }
    public IEnumerator CreateDebuffEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(DebuffEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }
    public IEnumerator CreatePoisonAppliedEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(PoisonAppliedEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }
    public IEnumerator CreateDamagedByPoisonEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(DamagedByPoisonEffect, location, Quaternion.identity);
        newImpactVFX.GetComponent<GainArmorEffect>().InitializeSetup(location, 0);
        yield return null;
    }
    public IEnumerator CreateTeleportEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(TeleportEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }
    public IEnumerator CreateHealEffect(Vector3 location, int healAmount)
    {
        GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
        damageEffect.GetComponent<DamEffect.DamageEffect>().InitializeSetup(healAmount, true, false);        
        GameObject newHealVFX = Instantiate(HealEffectPrefab, location, Quaternion.identity);
        newHealVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }
    public IEnumerator CreateAoeMeleeAttackEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(AoeMeleeAttackEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }


    #endregion

    // Camp Site VFX
    #region
        /*
    public void CreateTriageEffect(Vector3 location, int sortingLayer) 
    {
        StartCoroutine(CreateTriageEffectCoroutine(location, sortingLayer));
    }
    private IEnumerator CreateTriageEffectCoroutine(Vector3 location, int sortingLayer)
    {
        Debug.Log("VisualEffectManager.CreateTriageEffectCoroutine() started...");

        float yOffSet = 0.25f;
        Vector3 screenPos = new Vector3(location.x, location.y + yOffSet, location.z);        

        GameObject newHealVFX = Instantiate(HealEffectPrefab, screenPos, Quaternion.identity);
        SetEffectScale(newHealVFX, 10);;

        newHealVFX.GetComponent<BuffEffect>().InitializeSetup(screenPos, sortingLayer);

        CreateStatusEffectOnCampSiteCharacter(location, "Triage!", sortingLayer);
        yield return null;
    }
    public void CreateTrainEffect(Vector3 location, int sortingLayer)
    {
        StartCoroutine(CreateTrainEffectCoroutine(location, sortingLayer));
    }
    private IEnumerator CreateTrainEffectCoroutine(Vector3 location, int sortingLayer)
    {
        Debug.Log("VisualEffectManager.CreateTrainEffectCoroutine() started...");

        float yOffSet = 0.25f;
        Vector3 screenPos = new Vector3(location.x, location.y + yOffSet, location.z);

        GameObject newHealVFX = Instantiate(BuffEffectPrefab, screenPos, Quaternion.identity);
        SetEffectScale(newHealVFX, 10);

        newHealVFX.GetComponent<BuffEffect>().InitializeSetup(screenPos, sortingLayer);

        CreateStatusEffectOnCampSiteCharacter(location, "Train!", sortingLayer);

        yield return null;
    }
    public void CreateReadEffect(Vector3 location, int sortingLayer)
    {
        StartCoroutine(CreateReadEffectCoroutine(location, sortingLayer));
    }
    private IEnumerator CreateReadEffectCoroutine(Vector3 location, int sortingLayer)
    {
        Debug.Log("VisualEffectManager.CreateReadEffectCoroutine() started...");

        float yOffSet = 0.25f;
        Vector3 screenPos = new Vector3(location.x, location.y + yOffSet, location.z);

        GameObject newHealVFX = Instantiate(BuffEffectPrefab, screenPos, Quaternion.identity);
        SetEffectScale(newHealVFX, 10);

        newHealVFX.GetComponent<BuffEffect>().InitializeSetup(screenPos, sortingLayer);

        CreateStatusEffectOnCampSiteCharacter(location, "Read!", sortingLayer);

        yield return null;
    }
    public void CreateStatusEffectOnCampSiteCharacter(Vector3 location, string statusText, int sortingLayer)
    {
        StartCoroutine(CreateStatusEffectOnCampSiteCharacterCorotuine(location, statusText, sortingLayer));
    }
    private IEnumerator CreateStatusEffectOnCampSiteCharacterCorotuine(Vector3 location, string statusText, int sortingLayer)
    {
        float yOffSet = 0.25f;
        Vector3 screenPos = new Vector3(location.x, location.y + yOffSet, location.z);

        GameObject statusVFX = Instantiate(StatusEffectPrefab, screenPos, Quaternion.identity);
        SetEffectScale(statusVFX, 2);

        statusVFX.GetComponent<StatusEffect>().InitializeSetup(statusText, Color.white, sortingLayer);
        yield return null;
    }
    */
    #endregion

    // Projectiles
    #region
        /*
    public OldCoroutineData ShootFireball(Vector3 startPos, Vector3 endPos)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(ShootFireballCoroutine(startPos, endPos, action));
        return action;
    }
    public IEnumerator ShootFireballCoroutine(Vector3 startPosition, Vector3 endPosition, OldCoroutineData action, float speed = 20)
    {        
        GameObject fireBall = Instantiate(FireBallPrefab, startPosition, FireBallPrefab.transform.rotation);
        ExplodeOnHit myExplodeOnHit = fireBall.gameObject.GetComponent<ExplodeOnHit>();

        // make fireball explode instantly if instantiated on the destination 
        if (fireBall.transform.position == endPosition)
        {
            myExplodeOnHit.Explode();
            action.coroutineCompleted = true;
        }

        // else, travel towards destination, explode on arrival
        while (fireBall.transform.position != endPosition)
        {
            fireBall.transform.position = Vector2.MoveTowards(fireBall.transform.position, endPosition, speed * Time.deltaTime);
            if (fireBall.transform.position == endPosition)
            {
                myExplodeOnHit.Explode();
                action.coroutineCompleted = true;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public OldCoroutineData ShootArrow(Vector3 startPos, Vector3 endPos, float speed = 15)
    {
        Debug.Log("VisualEffectManager.ShootArrow() called...");
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(ShootArrowCoroutine(startPos, endPos, action, speed));
        return action;
    }
    public IEnumerator ShootArrowCoroutine(Vector3 startPos, Vector3 endPos, OldCoroutineData action, float speed)
    {
        GameObject arrow = Instantiate(ArrowPrefab,startPos,Quaternion.identity);
        Projectile projectileScript = arrow.GetComponent<Projectile>();
        projectileScript.InitializeSetup(startPos, endPos, speed);
        yield return new WaitUntil(() => projectileScript.destinationReached == true);
        action.coroutineCompleted = true;
    }
    public OldCoroutineData ShootShadowBall(Vector3 startPos, Vector3 endPos)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(ShootShadowBallCoroutine(startPos, endPos, action));
        return action;
    }
    public IEnumerator ShootShadowBallCoroutine(Vector3 startPosition, Vector3 endPosition, OldCoroutineData action, float speed = 4)
    {
        GameObject shadowBall = Instantiate(ShadowBallPrefab, startPosition, ShadowBallPrefab.transform.rotation);
        ExplodeOnHit myExplodeOnHit = shadowBall.gameObject.GetComponent<ExplodeOnHit>();

        while (shadowBall.transform.position != endPosition)
        {
            shadowBall.transform.position = Vector2.MoveTowards(shadowBall.transform.position, endPosition, speed * Time.deltaTime);            
            yield return new WaitForEndOfFrame();
        }

        if (shadowBall.transform.position == endPosition)
        {
            myExplodeOnHit.Explode();
            action.coroutineCompleted = true;
        }
    }
    public OldCoroutineData ShootHolyFire(Vector3 endPos)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(ShootHolyFireCoroutine(endPos, action));
        return action;
    }
    public IEnumerator ShootHolyFireCoroutine(Vector3 endPosition, OldCoroutineData action)
    {
        GameObject holyFire = Instantiate(HolyFirePrefab, endPosition, HolyFirePrefab.transform.rotation);
        Destroy(holyFire, 3);
        action.coroutineCompleted = true;
        yield return null;
        
    }
    public OldCoroutineData ShootFrostBolt(Vector3 startPos, Vector3 endPos)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(ShootFrostBoltCoroutine(startPos, endPos, action));
        return action;
    }
    public IEnumerator ShootFrostBoltCoroutine(Vector3 startPosition, Vector3 endPosition, OldCoroutineData action, float speed = 5)
    {
        GameObject frostBolt = Instantiate(FrostBoltPrefab, startPosition, FrostBoltPrefab.transform.rotation);
        FaceDestination(frostBolt, endPosition);
        ExplodeOnHit myExplodeOnHit = frostBolt.gameObject.GetComponent<ExplodeOnHit>();

        while (frostBolt.transform.position != endPosition)
        {
            frostBolt.transform.position = Vector2.MoveTowards(frostBolt.transform.position, endPosition, speed * Time.deltaTime);
            if (frostBolt.transform.position == endPosition)
            {
                myExplodeOnHit.Explode();
                action.coroutineCompleted = true;
            }
            yield return new WaitForEndOfFrame();
        }
    } 
    */
    #endregion

    // Logic
    #region
        /*
    public void FaceDestination(GameObject projectile, Vector3 destination)
    {
        Vector2 direction = destination - projectile.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        projectile.transform.rotation = Quaternion.Slerp(projectile.transform.rotation, rotation, 10000f);
    }
    */
    public void SetEffectScale(GameObject scaleParent, int scale)
    {
        Debug.Log("VisualEffectManager.SetEffectScale() called, scaling " +
            scaleParent.name + " to " + scale.ToString());

        RectTransform rectTransform = scaleParent.GetComponent<RectTransform>();
        Transform normalTransform = scaleParent.GetComponent<Transform>();
        Vector3 newScale = new Vector3(scale, scale, scale);

        if (rectTransform)
        {
            rectTransform.localScale = newScale;
        }
        else if (normalTransform)
        {
            normalTransform.localScale = newScale;
        }
    }
    #endregion

    // Toon Vfx Projectiles
    #region
    // Fire Ball
    /*
    public OldCoroutineData ShootToonFireball(Vector3 startPos, Vector3 endPos, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.7f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(ShootToonFireballCoroutine(startPos, endPos, action, speed, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator ShootToonFireballCoroutine(Vector3 startPosition, Vector3 endPosition, OldCoroutineData action, float speed, int sortingOrderBonus, float scaleModifier)
    {
        bool destinationReached = false;

        GameObject fireBall = Instantiate(toonFireBall, startPosition, toonFireBall.transform.rotation);
        ToonProjectile tsScript = fireBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);

        // insta explode of created on destination
        if (fireBall.transform.position == endPosition)
        {
            destinationReached = true;
            tsScript.OnDestinationReached();
            action.coroutineCompleted = true;
        }

        while (fireBall.transform.position != endPosition)
        {
            fireBall.transform.position = Vector2.MoveTowards(fireBall.transform.position, endPosition, speed * Time.deltaTime);
            if (fireBall.transform.position == endPosition && !destinationReached)
            {
                destinationReached = true;
                tsScript.OnDestinationReached();
                action.coroutineCompleted = true;
            }
            yield return new WaitForEndOfFrame();
        }

    }

    // Poison Ball
    public OldCoroutineData ShootToonPoisonBall(Vector3 startPos, Vector3 endPos, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.5f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(ShootToonPoisonBallCoroutine(startPos, endPos, action, speed, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator ShootToonPoisonBallCoroutine(Vector3 startPosition, Vector3 endPosition, OldCoroutineData action, float speed, int sortingOrderBonus, float scaleModifier)
    {
        bool destinationReached = false;

        GameObject poisonBall = Instantiate(toonPoisonBall, startPosition, toonPoisonBall.transform.rotation);
        ToonProjectile tsScript = poisonBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);

        // insta explode of created on destination
        if (poisonBall.transform.position == endPosition)
        {
            destinationReached = true;
            tsScript.OnDestinationReached();
            action.coroutineCompleted = true;
        }

        while (poisonBall.transform.position != endPosition)
        {
            poisonBall.transform.position = Vector2.MoveTowards(poisonBall.transform.position, endPosition, speed * Time.deltaTime);
            if (poisonBall.transform.position == endPosition && !destinationReached)
            {
                destinationReached = true;
                tsScript.OnDestinationReached();
                action.coroutineCompleted = true;
            }
            yield return new WaitForEndOfFrame();
        }

    }

    // Shadow Ball
    public OldCoroutineData ShootToonShadowBall(Vector3 startPos, Vector3 endPos, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.5f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(ShootToonShadowBallCoroutine(startPos, endPos, action, speed, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator ShootToonShadowBallCoroutine(Vector3 startPosition, Vector3 endPosition, OldCoroutineData action, float speed, int sortingOrderBonus, float scaleModifier)
    {
        bool destinationReached = false;

        GameObject shadowBall = Instantiate(toonShadowBall, startPosition, toonShadowBall.transform.rotation);
        ToonProjectile tsScript = shadowBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);

        // insta explode of created on destination
        if (shadowBall.transform.position == endPosition)
        {
            destinationReached = true;
            tsScript.OnDestinationReached();
            action.coroutineCompleted = true;
        }

        while (shadowBall.transform.position != endPosition)
        {
            shadowBall.transform.position = Vector2.MoveTowards(shadowBall.transform.position, endPosition, speed * Time.deltaTime);
            if (shadowBall.transform.position == endPosition && !destinationReached)
            {
                destinationReached = true;
                tsScript.OnDestinationReached();
                action.coroutineCompleted = true;
            }
            yield return new WaitForEndOfFrame();
        }

    }

    // Frost Ball
    public OldCoroutineData ShootToonFrostBall(Vector3 startPos, Vector3 endPos, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.5f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(ShootToonFrostBallCoroutine(startPos, endPos, action, speed, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator ShootToonFrostBallCoroutine(Vector3 startPosition, Vector3 endPosition, OldCoroutineData action, float speed, int sortingOrderBonus, float scaleModifier)
    {
        bool destinationReached = false;

        GameObject frostBall = Instantiate(toonFrostBall, startPosition, toonFrostBall.transform.rotation);
        ToonProjectile tsScript = frostBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);

        // insta explode of created on destination
        if (frostBall.transform.position == endPosition)
        {
            destinationReached = true;
            tsScript.OnDestinationReached();
            action.coroutineCompleted = true;
        }

        while (frostBall.transform.position != endPosition)
        {
            frostBall.transform.position = Vector2.MoveTowards(frostBall.transform.position, endPosition, speed * Time.deltaTime);
            if (frostBall.transform.position == endPosition && !destinationReached)
            {
                destinationReached = true;
                tsScript.OnDestinationReached();
                action.coroutineCompleted = true;
            }
            yield return new WaitForEndOfFrame();
        }

    }

    // Lightning Ball
    public OldCoroutineData ShootToonLightningBall(Vector3 startPos, Vector3 endPos, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.5f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(ShootToonLightningBallCoroutine(startPos, endPos, action, speed, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator ShootToonLightningBallCoroutine(Vector3 startPosition, Vector3 endPosition, OldCoroutineData action, float speed, int sortingOrderBonus, float scaleModifier)
    {
        bool destinationReached = false;

        GameObject lightningBall = Instantiate(toonLightningBall, startPosition, toonLightningBall.transform.rotation);
        ToonProjectile tsScript = lightningBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);

        // insta explode of created on destination
        if (lightningBall.transform.position == endPosition)
        {
            destinationReached = true;
            tsScript.OnDestinationReached();
            action.coroutineCompleted = true;
        }

        while (lightningBall.transform.position != endPosition)
        {
            lightningBall.transform.position = Vector2.MoveTowards(lightningBall.transform.position, endPosition, speed * Time.deltaTime);
            if (lightningBall.transform.position == endPosition && !destinationReached)
            {
                destinationReached = true;
                tsScript.OnDestinationReached();
                action.coroutineCompleted = true;
            }
            yield return new WaitForEndOfFrame();
        }

    }

    // Holy Ball
    public OldCoroutineData ShootToonHolyBall(Vector3 startPos, Vector3 endPos, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.5f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(ShootToonHolyBallCoroutine(startPos, endPos, action, speed, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator ShootToonHolyBallCoroutine(Vector3 startPosition, Vector3 endPosition, OldCoroutineData action, float speed, int sortingOrderBonus, float scaleModifier)
    {
        bool destinationReached = false;

        GameObject hb = Instantiate(toonHolyBall, startPosition, toonHolyBall.transform.rotation);
        ToonProjectile tsScript = hb.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);

        yield return null;

        // insta explode of created on destination
        if (hb.transform.position == endPosition)
        {
            destinationReached = true;
            tsScript.OnDestinationReached();
            action.coroutineCompleted = true;
        }

        while (hb.transform.position != endPosition)
        {
            hb.transform.position = Vector2.MoveTowards(hb.transform.position, endPosition, speed * Time.deltaTime);
            if (hb.transform.position == endPosition && !destinationReached)
            {
                destinationReached = true;
                tsScript.OnDestinationReached();
                action.coroutineCompleted = true;
            }
            yield return new WaitForEndOfFrame();
        }

    }
    */
    #endregion

    // Toon Auras + Novas
    #region
    /*
    // Fire Nova
    public OldCoroutineData CreateFireNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(CreateFireNovaCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateFireNovaCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject fireNova = Instantiate(toonFireNova, location, toonFireNova.transform.rotation);
        ToonEffect teScript = fireNova.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.coroutineCompleted = true;
        yield return null;

    }

    // Poison Nova
    public OldCoroutineData CreatePoisonNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(CreatePoisonNovaCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreatePoisonNovaCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject poisonNova = Instantiate(toonPoisonNova, location, toonPoisonNova.transform.rotation);
        ToonEffect teScript = poisonNova.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.coroutineCompleted = true;
        yield return null;

    }

    // Shadow Nova
    public OldCoroutineData CreateShadowNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(CreateShadowNovaCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateShadowNovaCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject shadowNova = Instantiate(toonShadowNova, location, toonShadowNova.transform.rotation);
        ToonEffect teScript = shadowNova.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.coroutineCompleted = true;
        yield return null;

    }

    // Frost Nova
    public OldCoroutineData CreateFrostNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(CreateFrostNovaCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateFrostNovaCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject frostNova = Instantiate(toonFrostNova, location, toonFrostNova.transform.rotation);
        ToonEffect teScript = frostNova.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.coroutineCompleted = true;
        yield return null;

    }

    // Lightning Nova
    public OldCoroutineData CreateLightningNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(CreateLightningNovaCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateLightningNovaCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject lightningNova = Instantiate(toonLightningNova, location, toonLightningNova.transform.rotation);
        ToonEffect teScript = lightningNova.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.coroutineCompleted = true;
        yield return null;

    }

    // Holy Nova
    public OldCoroutineData CreateHolyNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(CreateHolyNovaCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateHolyNovaCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonHolyNova, location, toonHolyNova.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.coroutineCompleted = true;
        yield return null;

    }

    // Blue Nova
    public OldCoroutineData CreateBlueNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(CreateBlueNovaCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateBlueNovaCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonBlueNova, location, toonBlueNova.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.coroutineCompleted = true;
        yield return null;

    }
    */
    #endregion

    // Toon Debuffs
    #region
    /*
    // Apply General Debuff
    public OldCoroutineData CreateGeneralDebuffEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(CreateGeneralDebuffEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateGeneralDebuffEffectCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonGeneralDebuff, location, toonGeneralDebuff.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.coroutineCompleted = true;
        yield return null;

    }

    // Apply Stunned Effect
    public OldCoroutineData CreateStunnedEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(CreateStunnedEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateStunnedEffectCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
    {
        Vector3 offsetLocation = new Vector3(location.x, location.y + 0.2f, location.z);
        GameObject hn = Instantiate(toonApplyStunned, offsetLocation, toonApplyStunned.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.coroutineCompleted = true;
        yield return null;

    }

    // Apply Burning Effect
    public OldCoroutineData CreateApplyBurningEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(CreateApplyBurningEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateApplyBurningEffectCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonApplyBurning, location, toonApplyBurning.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.coroutineCompleted = true;
        yield return null;

    }

    // Apply Burning Effect
    public OldCoroutineData CreateApplyPoisonedEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(CreateApplyPoisonedEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateApplyPoisonedEffectCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonApplyPoisoned, location, toonApplyPoisoned.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.coroutineCompleted = true;
        yield return null;

    }

    // Apply Shocked Effect
    public OldCoroutineData CreateApplyShockedEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(CreateApplyShockedEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateApplyShockedEffectCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonApplyShocked, location, toonApplyShocked.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.coroutineCompleted = true;
        yield return null;
    }

    // Apply Chilled Effect
    public OldCoroutineData CreateApplyChilledEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(CreateApplyChilledEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateApplyChilledEffectCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonApplyChilled, location, toonApplyChilled.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.coroutineCompleted = true;
        yield return null;

    }

    // Apply Weakened Effect
    public OldCoroutineData CreateApplyWeakenedEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(CreateApplyWeakenedEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateApplyWeakenedEffectCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonApplyWeakened, location, toonApplyWeakened.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.coroutineCompleted = true;
        yield return null;

    }

    // Apply Vulnerable Effect
    public OldCoroutineData CreateApplyVulnerableEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(CreateApplyVulnerableEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateApplyVulnerableEffectCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonApplyVulnerable, location, toonApplyVulnerable.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.coroutineCompleted = true;
        yield return null;

    }
    */
    #endregion

    // Toon Explosions
    #region
    /*
    // Small Frost Explosion
    public OldCoroutineData CreateSmallFrostExplosion(Vector3 location, int sortingOrder = 15, float scaleModifier = 1f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(CreateSmallFrostExplosionCoroutine(location, action, sortingOrder, scaleModifier));
        return action;
    }
    private IEnumerator CreateSmallFrostExplosionCoroutine(Vector3 location, OldCoroutineData action, int sortingOrder, float scaleModifier)
    {
        GameObject hn = Instantiate(smallFrostExplosion, location, smallFrostExplosion.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrder, scaleModifier);
        action.coroutineCompleted = true;
        yield return null;

    }

    // Small Poison Explosion
    public OldCoroutineData CreateSmallPoisonExplosion(Vector3 location, int sortingOrder = 15, float scaleModifier = 1f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(CreateSmallPoisonExplosionCoroutine(location, action, sortingOrder, scaleModifier));
        return action;
    }
    private IEnumerator CreateSmallPoisonExplosionCoroutine(Vector3 location, OldCoroutineData action, int sortingOrder, float scaleModifier)
    {
        GameObject hn = Instantiate(smallPoisonExplosion, location, smallPoisonExplosion.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrder, scaleModifier);
        action.coroutineCompleted = true;
        yield return null;

    }

    // Small Lightning Explosion
    public OldCoroutineData CreateSmallLightningExplosion(Vector3 location, int sortingOrder = 15, float scaleModifier = 1f)
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(CreateSmallLightningExplosionCoroutine(location, action, sortingOrder, scaleModifier));
        return action;
    }
    private IEnumerator CreateSmallLightningExplosionCoroutine(Vector3 location, OldCoroutineData action, int sortingOrder, float scaleModifier)
    {
        GameObject hn = Instantiate(smallLightningExplosion, location, smallLightningExplosion.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrder, scaleModifier);
        action.coroutineCompleted = true;
        yield return null;

    }
    */
    #endregion

    // Toon Buffs
    #region
    /*
 // General Buff
 public OldCoroutineData CreateGeneralBuffEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
 {
     OldCoroutineData action = new OldCoroutineData();
     StartCoroutine(CreateGeneralBuffEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
     return action;
 }
 private IEnumerator CreateGeneralBuffEffectCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
 {
     GameObject hn = Instantiate(toonGeneralBuffPrefab, location, toonGeneralBuffPrefab.transform.rotation);
     ToonEffect teScript = hn.GetComponent<ToonEffect>();
     teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
     action.coroutineCompleted = true;
     yield return null;

 }

 // Shadow Buff
 public OldCoroutineData CreateShadowBuffEffect(Vector3 location, int sortingOrderBonus = 0, float scaleModifier = 1f)
 {
     OldCoroutineData action = new OldCoroutineData();
     StartCoroutine(CreateShadowBuffEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
     return action;
 }
 private IEnumerator CreateShadowBuffEffectCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
 {
     GameObject hn = Instantiate(toonShadowBuffPrefab, location, toonShadowBuffPrefab.transform.rotation);
     ToonEffect teScript = hn.GetComponent<ToonEffect>();
     teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
     action.coroutineCompleted = true;
     yield return null;

 }

 // Holy Buff
 public OldCoroutineData CreateHolyBuffEffect(Vector3 location, int sortingOrderBonus = 0, float scaleModifier = 1f)
 {
     OldCoroutineData action = new OldCoroutineData();
     StartCoroutine(CreateHolyBuffEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
     return action;
 }
 private IEnumerator CreateHolyBuffEffectCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
 {
     GameObject hn = Instantiate(toonHolyBuffPrefab, location, toonHolyBuffPrefab.transform.rotation);
     ToonEffect teScript = hn.GetComponent<ToonEffect>();
     teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
     action.coroutineCompleted = true;
     yield return null;

 }

 // Core Stat Buff
 public OldCoroutineData CreateCoreStatBuffEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
 {
     OldCoroutineData action = new OldCoroutineData();
     StartCoroutine(CreateCoreStatBuffEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
     return action;
 }
 private IEnumerator CreateCoreStatBuffEffectCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
 {
     GameObject hn = Instantiate(toonGainCoreStatPrefab, location, toonGainCoreStatPrefab.transform.rotation);
     ToonEffect teScript = hn.GetComponent<ToonEffect>();
     teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
     action.coroutineCompleted = true;
     yield return null;

 }

 // Gain Energy Buff
 public OldCoroutineData OldCreateGainEnergyBuffEffect2(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
 {
     OldCoroutineData action = new OldCoroutineData();
     StartCoroutine(CreateGainEnergyBuffEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
     return action;
 }
 private IEnumerator CreateGainEnergyBuffEffectCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
 {
     GameObject hn = Instantiate(toonGainEnergyPrefab, location, toonGainEnergyPrefab.transform.rotation);
     ToonEffect teScript = hn.GetComponent<ToonEffect>();
     teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
     action.coroutineCompleted = true;
     yield return null;

 }


 // Camoflage Buff
 public OldCoroutineData CreateCamoflageBuffEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
 {
     OldCoroutineData action = new OldCoroutineData();
     StartCoroutine(CreateCamoflageBuffEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
     return action;
 }
 private IEnumerator CreateCamoflageBuffEffectCoroutine(Vector3 location, OldCoroutineData action, int sortingOrder, float scaleModifier)
 {
     GameObject hn = Instantiate(toonGainCamoflagePrefab, location, toonGainCamoflagePrefab.transform.rotation);
     ToonEffect teScript = hn.GetComponent<ToonEffect>();
     teScript.InitializeSetup(sortingOrder, scaleModifier);
     action.coroutineCompleted = true;
     yield return null;

 }
 */
    #endregion

    // Melee Impacts
    #region

    /*
public OldCoroutineData OldCreateSmallMeleeImpact(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
{
    OldCoroutineData action = new OldCoroutineData();
    StartCoroutine(CreateSmallMeleeImpactCoroutine(location, action, sortingOrderBonus, scaleModifier));
    return action;
}
private IEnumerator CreateSmallMeleeImpactCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
{
    GameObject hn = Instantiate(smallMeleeImpact, location, smallMeleeImpact.transform.rotation);
    ToonEffect teScript = hn.GetComponent<ToonEffect>();
    teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    action.coroutineCompleted = true;
    yield return null;

}

// Big Melee Impact
public OldCoroutineData CreateBigMeleeImpact(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
{
    OldCoroutineData action = new OldCoroutineData();
    StartCoroutine(CreateBigMeleeImpactCoroutine(location, action, sortingOrderBonus, scaleModifier));
    return action;
}
private IEnumerator CreateBigMeleeImpactCoroutine(Vector3 location, OldCoroutineData action, int sortingOrderBonus, float scaleModifier)
{
    GameObject hn = Instantiate(bigMeleeImpact, location, bigMeleeImpact.transform.rotation);
    ToonEffect teScript = hn.GetComponent<ToonEffect>();
    teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    action.coroutineCompleted = true;
    yield return null;

}
*/

    #endregion


    // CORE FUNCTIONS
    #region
    public void CreatEffectAtLocation(ParticleEffect effect, Vector3 location)
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

        // Explosions
        else if (effect == ParticleEffect.LightningExplosion1)
        {
            CreateLightningExplosion(location);
        }
        else if (effect == ParticleEffect.FireExplosion1)
        {
            CreateFireExplosion(location);
        }
        else if (effect == ParticleEffect.FrostExplosion1)
        {
            CreateFrostExplosion(location);
        }
        else if (effect == ParticleEffect.PoisonExplosion1)
        {
            CreatePoisonExplosion(location);
        }
        else if (effect == ParticleEffect.ShadowExplosion1)
        {
            CreateShadowExplosion(location);
        }
        else if (effect == ParticleEffect.BloodExplosion1)
        {
            CreateBloodExplosion(location);
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
    }
    #endregion

    // GENERAL FX
    #region

    // Damage Text Value Effect
    public void CreateDamageEffect(Vector3 location, int damageAmount, bool heal = false, bool healthLost = true)
    {
        GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
        damageEffect.GetComponent<DamEffect.DamageEffect>().InitializeSetup(damageAmount, heal, healthLost);
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
    }

    // General Buff
    public void CreateGeneralBuffEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(yellowPillarBuff, location, yellowPillarBuff.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }
    #endregion

    // BLOCK FX
    #region

    // Gain Block
    public void CreateGainBlockEffect(Vector3 location, int blockGained)
    {
        GameObject newImpactVFX = Instantiate(GainBlockEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<GainArmorEffect>().InitializeSetup(location, blockGained);
    }

    // Lose Block Effect
    public void CreateLoseBlockEffect(Vector3 location, int blockLost)
    {
        GameObject newImpactVFX = Instantiate(LoseBlockEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<GainArmorEffect>().InitializeSetup(location, blockLost);
        CreateDamageEffect(location, blockLost, false, false);
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
        GameObject arrow = Instantiate(ArrowPrefab, startPos, Quaternion.identity);
        Projectile projectileScript = arrow.GetComponent<Projectile>();
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
        GameObject fireBall = Instantiate(toonFireBall, startPosition, toonFireBall.transform.rotation);
        ToonProjectile tsScript = fireBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        bool destinationReached = false;

        // insta explode if created at destination
        if (fireBall.transform.position.x == endPosition.x &&
            fireBall.transform.position.y == endPosition.y &&
            destinationReached == false)
        {
            destinationReached = true;

            tsScript.OnDestinationReached();

            // Resolve early
            if(cData != null)
            {
                cData.MarkAsCompleted();
            }          
        }

        while (fireBall.transform.position != endPosition && 
               destinationReached == false)
        {
            fireBall.transform.position = Vector2.MoveTowards(fireBall.transform.position, endPosition, speed * Time.deltaTime);

            if (fireBall.transform.position.x == endPosition.x &&
                fireBall.transform.position.y == endPosition.y)
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

    // Shadow Ball
    public void ShootShadowBall(Vector3 startPos, Vector3 endPos, CoroutineData cData, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.7f)
    {
        Debug.Log("VisualEffectManager.ShootShadowBall() called...");
        StartCoroutine(ShootShadowBallCoroutine(startPos, endPos, cData, speed, sortingOrderBonus, scaleModifier));
    }
    private IEnumerator ShootShadowBallCoroutine(Vector3 startPosition, Vector3 endPosition, CoroutineData cData, float speed, int sortingOrderBonus, float scaleModifier)
    {
        GameObject shadowBall = Instantiate(toonShadowBall, startPosition, toonShadowBall.transform.rotation);
        ToonProjectile tsScript = shadowBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        bool destinationReached = false;

        // insta explode if created at destination
        if (shadowBall.transform.position.x == endPosition.x &&
            shadowBall.transform.position.y == endPosition.y &&
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

        while (shadowBall.transform.position != endPosition &&
               destinationReached == false)
        {
            shadowBall.transform.position = Vector2.MoveTowards(shadowBall.transform.position, endPosition, speed * Time.deltaTime);

            if (shadowBall.transform.position.x == endPosition.x &&
                shadowBall.transform.position.y == endPosition.y)
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

    // Poison Ball
    public void ShootPoisonBall(Vector3 startPos, Vector3 endPos, CoroutineData cData, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.7f)
    {
        Debug.Log("VisualEffectManager.ShootPoisonBallCoroutine() called...");
        StartCoroutine(ShootPoisonBallCoroutine(startPos, endPos, cData, speed, sortingOrderBonus, scaleModifier));
    }
    private IEnumerator ShootPoisonBallCoroutine(Vector3 startPosition, Vector3 endPosition, CoroutineData cData, float speed, int sortingOrderBonus, float scaleModifier)
    {
        GameObject poisonBall = Instantiate(toonPoisonBall, startPosition, toonPoisonBall.transform.rotation);
        ToonProjectile tsScript = poisonBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        bool destinationReached = false;

        // insta explode if created at destination
        if (poisonBall.transform.position.x == endPosition.x &&
            poisonBall.transform.position.y == endPosition.y &&
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

        while (poisonBall.transform.position != endPosition &&
               destinationReached == false)
        {
            poisonBall.transform.position = Vector2.MoveTowards(poisonBall.transform.position, endPosition, speed * Time.deltaTime);

            if (poisonBall.transform.position.x == endPosition.x &&
                poisonBall.transform.position.y == endPosition.y)
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

    // Lightning Ball
    public void ShootLightningBall(Vector3 startPos, Vector3 endPos, CoroutineData cData, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.7f)
    {
        Debug.Log("VisualEffectManager.ShootLightningBallCoroutine() called...");
        StartCoroutine(ShootLightningBallCoroutine(startPos, endPos, cData, speed, sortingOrderBonus, scaleModifier));
    }
    private IEnumerator ShootLightningBallCoroutine(Vector3 startPosition, Vector3 endPosition, CoroutineData cData, float speed, int sortingOrderBonus, float scaleModifier)
    {
        GameObject lightningBall = Instantiate(toonLightningBall, startPosition, toonLightningBall.transform.rotation);
        ToonProjectile tsScript = lightningBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        bool destinationReached = false;

        // insta explode if created at destination
        if (lightningBall.transform.position.x == endPosition.x &&
            lightningBall.transform.position.y == endPosition.y &&
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

        while (lightningBall.transform.position != endPosition &&
               destinationReached == false)
        {
            lightningBall.transform.position = Vector2.MoveTowards(lightningBall.transform.position, endPosition, speed * Time.deltaTime);

            if (lightningBall.transform.position.x == endPosition.x &&
                lightningBall.transform.position.y == endPosition.y)
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
    public void ShootHolyBall(Vector3 startPos, Vector3 endPos, CoroutineData cData, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.7f)
    {
        Debug.Log("VisualEffectManager.ShootHolyBallCoroutine() called...");
        StartCoroutine(ShootHolyBallCoroutine(startPos, endPos, cData, speed, sortingOrderBonus, scaleModifier));
    }
    private IEnumerator ShootHolyBallCoroutine(Vector3 startPosition, Vector3 endPosition, CoroutineData cData, float speed, int sortingOrderBonus, float scaleModifier)
    {
        GameObject holyBall = Instantiate(toonHolyBall, startPosition, toonHolyBall.transform.rotation);
        ToonProjectile tsScript = holyBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        bool destinationReached = false;

        // insta explode if created at destination
        if (holyBall.transform.position.x == endPosition.x &&
            holyBall.transform.position.y == endPosition.y &&
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

        while (holyBall.transform.position != endPosition &&
               destinationReached == false)
        {
            holyBall.transform.position = Vector2.MoveTowards(holyBall.transform.position, endPosition, speed * Time.deltaTime);

            if (holyBall.transform.position.x == endPosition.x &&
                holyBall.transform.position.y == endPosition.y)
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
        GameObject frostBall = Instantiate(toonFrostBall, startPosition, toonFrostBall.transform.rotation);
        ToonProjectile tsScript = frostBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        bool destinationReached = false;

        // insta explode if created at destination
        if (frostBall.transform.position.x == endPosition.x &&
            frostBall.transform.position.y == endPosition.y &&
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

        while (frostBall.transform.position != endPosition &&
               destinationReached == false)
        {
            frostBall.transform.position = Vector2.MoveTowards(frostBall.transform.position, endPosition, speed * Time.deltaTime);

            if (frostBall.transform.position.x == endPosition.x &&
                frostBall.transform.position.y == endPosition.y)
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
    #endregion

    // APPLY BUFF + DEBUFF FX
    #region

    // Apply Poisoned Effect
    public void CreateApplyPoisonedEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(toonApplyPoisoned, location, toonApplyPoisoned.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);

    }

    // Apply Burning Effect
    public void CreateApplyBurningEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(toonApplyBurning, location, toonApplyBurning.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);

    }

    // Apply Overload Effect    
    public void CreateGainOverloadEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(toonApplyShocked, location, toonApplyShocked.transform.rotation);
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
    }

    // Fire Explosion
    public void CreateFireExplosion(Vector3 location, int sortingOrderBonus = 0, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(smallFireExplosion, location, smallFireExplosion.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }

    // Poison Explosion
    public void CreatePoisonExplosion(Vector3 location, int sortingOrderBonus = 0, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(smallPoisonExplosion, location, smallPoisonExplosion.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
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
    }
    #endregion

    // NOVAS
    #region
    // Fire Nova
    public void CreateFireNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(toonFireNova, location, toonFireNova.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }

    // Poison Nova
    public void CreatePoisonNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(toonPoisonNova, location, toonPoisonNova.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }

    // Frost Nova
    public void CreateFrostNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(toonFrostNova, location, toonFrostNova.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }

    // Lightning Nova
    public void CreateLightningNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(toonLightningNova, location, toonLightningNova.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }

    // Shadow Nova
    public void CreateShadowNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(toonShadowNova, location, toonShadowNova.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }

    // Holy Nova
    public void CreateHolyNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        GameObject hn = Instantiate(toonHolyNova, location, toonHolyNova.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
    }
    #endregion


}
