using UnityEngine;
using System.Collections;

public class DamageEffectTest : MonoBehaviour {

    public GameObject DamagePrefab;
    public static DamageEffectTest Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            DamageEffect.CreateDamageEffect(transform.position, Random.Range(-7, 7));
    }
}
