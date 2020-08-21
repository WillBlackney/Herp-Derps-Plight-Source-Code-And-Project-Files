using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    // Any class that inherits from this will become a singleton

    public static T Instance;

    protected virtual void Awake()
    {
        if (!Instance)
        {
            Instance = GetComponent<T>();
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
