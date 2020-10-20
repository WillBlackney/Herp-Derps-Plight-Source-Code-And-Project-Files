using UnityEngine;

public class PlayerPrefs2
{
    public static void SetBool(string key, bool state, bool first = false)
    {
        PlayerPrefs.SetInt(key, state ? 1 : 0);
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        if (PlayerPrefs.HasKey(key))
        {
            int value = PlayerPrefs.GetInt(key);
            if (value == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return defaultValue;
        }
    }
}