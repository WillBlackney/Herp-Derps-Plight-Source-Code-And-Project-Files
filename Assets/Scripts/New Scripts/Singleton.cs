using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    // Any class that inherits from this will become a singleton

    // Properties
    #region
    public static T Instance;

    [Header("Singleton Properties")]
    [SerializeField] bool dontDestroyOnLoad;
    #endregion

    // Singleton Creation Logic
    #region
    protected virtual void Awake()
    {
        BuildSingleton();
    }
    protected void BuildSingleton()
    {
        if (!Instance)
        {
            Instance = GetComponent<T>();
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(Instance);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // FOR TESTING ONLY!! NEVER CALL THIS FUNCTION OUTSIDE OF TESTS!
    // Reason: awake is not called on monobehaviours unit tests.
    // It unwise to make the normal Awake() function public, so
    // the awake function is wrapped in the public RunAwake()
    // function instead.
    public void RunAwake()
    {
        if (!Instance)
        {
            Awake();
        }
    }
    #endregion


}
