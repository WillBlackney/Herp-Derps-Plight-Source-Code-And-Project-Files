using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{   
    [Header("Component References")]
    public GameObject enemiesParent;

    [Header("Properties")]
    public List<Enemy> allEnemies = new List<Enemy>();    
    public Enemy selectedEnemy;

    public static EnemyManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Logic
    #region
    public void SelectEnemy(Enemy enemy)
    {
        Debug.Log("EnemyManager.SelectEnemy() called");
        selectedEnemy = enemy;

        Defender selectedDefender = DefenderManager.Instance.selectedDefender;

        // check consumables first
        if (ConsumableManager.Instance.awaitingLovePotionTarget ||
            ConsumableManager.Instance.awaitingHandCannonTarget)
        {
            ConsumableManager.Instance.ApplyConsumableToTarget(selectedEnemy);
        }

        else if (ConsumableManager.Instance.awaitingFireBombTarget ||
            ConsumableManager.Instance.awaitingDynamiteTarget ||
            ConsumableManager.Instance.awaitingPoisonGrenadeTarget ||
            ConsumableManager.Instance.awaitingBottledFrostTarget)
        {
            ConsumableManager.Instance.ApplyConsumableToTarget(selectedEnemy.tile);
        }

        else if (ConsumableManager.Instance.awaitingBlinkPotionCharacterTarget)
        {
            ConsumableManager.Instance.StartBlinkPotionLocationSettingProcess(selectedEnemy);
        }
        
    }
    #endregion



}
