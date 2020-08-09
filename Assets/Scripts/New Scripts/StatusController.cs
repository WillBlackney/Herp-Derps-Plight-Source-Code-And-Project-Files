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


    public void ApplyStatusPairingToLivingEntity(LivingEntity entity, StatusPairing status)
    {
        // Setup Passives
        if (status.statusData.statusName == "Tenacious")
        {
            entity.myPassiveManager.ModifyTenacious(status.statusStacks);
        }
        else if (status.statusData.statusName == "Enrage")
        {
            entity.myPassiveManager.ModifyEnrage(status.statusStacks);
        }
        else if (status.statusData.statusName == "Masochist")
        {
            entity.myPassiveManager.ModifyMasochist(status.statusStacks);
        }
        else if (status.statusData.statusName == "Growing")
        {
            entity.myPassiveManager.ModifyGrowing(status.statusStacks);
        }
        else if (status.statusData.statusName == "Bonus Strength")
        {
            entity.myPassiveManager.ModifyBonusStrength(status.statusStacks);
        }
    }
}
