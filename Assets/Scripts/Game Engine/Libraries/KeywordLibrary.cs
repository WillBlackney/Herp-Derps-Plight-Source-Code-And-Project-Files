using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;
#endif

public class KeywordLibrary : Singleton<KeywordLibrary>
{
    [Header("All Key Word Data")]
    public KeyWordData[] allKeyWordData;

   
}
