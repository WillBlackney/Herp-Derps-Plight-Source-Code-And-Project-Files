using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    // Singleton Pattern
    #region
    public static StatusController Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public void ApplyStatusToLivingEntity(LivingEntity entity, StatusIconDataSO status, int stacks)
    {
        Debug.Log("StatusController.ApplyStatusToLivingEntity() called, applying " + status.statusName + "(" +
            stacks.ToString() + ") to " + entity.myName);

        if(entity == null || entity.inDeathProcess)
        {
            Debug.Log("StatusController.ApplyStatusToLivingEntity() detected entity is null or dying, cancelling status application process...");
            return;
        }

        // Setup Passives
        if (status.statusName == "Tenacious")
        {
            entity.myPassiveManager.ModifyTenacious(stacks);
        }
        else if (status.statusName == "Enrage")
        {
            entity.myPassiveManager.ModifyEnrage(stacks);
        }
        else if (status.statusName == "Masochist")
        {
            entity.myPassiveManager.ModifyMasochist(stacks);
        }
        else if (status.statusName == "Growing")
        {
            entity.myPassiveManager.ModifyGrowing(stacks);
        }
        else if (status.statusName == "Bonus Strength")
        {
            entity.myPassiveManager.ModifyBonusStrength(stacks);
        }
        else if (status.statusName == "Bonus Dexterity")
        {
            entity.myPassiveManager.ModifyBonusDexterity(stacks);
        }
        else if (status.statusName == "Cautious")
        {
            entity.myPassiveManager.ModifyCautious(stacks);
        }
        else if (status.statusName == "Weakened")
        {
            entity.myPassiveManager.ModifyWeakened(stacks);
        }
        else if (status.statusName == "Vulnerable")
        {
            entity.myPassiveManager.ModifyVulnerable(stacks);
        }
        else if (status.statusName == "Poisoned")
        {
            entity.myPassiveManager.ModifyPoisoned(stacks);
        }
        else if (status.statusName == "Unstable")
        {
            entity.myPassiveManager.ModifyUnstable(stacks);
        }
        else if (status.statusName == "Disarmed")
        {
            entity.myPassiveManager.ModifyDisarmed(stacks);
        }
        else if (status.statusName == "Blind")
        {
            entity.myPassiveManager.ModifyBlind(stacks);
        }
        else if (status.statusName == "Infuriated")
        {
            entity.myPassiveManager.ModifyInfuriated(stacks);
        }
        else if (status.statusName == "Sleep")
        {
            entity.myPassiveManager.ModifySleep(stacks);
        }
    }
    public bool IsEntityEffectedByStatus(LivingEntity entity, StatusIconDataSO status, int stacks)
    {
        bool hasStatus = false;

        foreach(StatusIcon icon in entity.myStatusManager.myStatusIcons)
        {
            // look for matching names
            if(icon.statusName == status.statusName &&
                icon.statusStacks >= stacks)
            {
                // match found, return true
                hasStatus = true;
                break;
            }
        }

        return hasStatus;
    }
}
