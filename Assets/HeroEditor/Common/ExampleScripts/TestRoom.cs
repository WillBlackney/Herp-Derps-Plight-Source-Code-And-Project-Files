using System;
using System.Linq;
using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.EditorScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.HeroEditor.Common.ExampleScripts
{
    public class TestRoom : MonoBehaviour
    {
        public Character Character;
        public string ReturnSceneName;

        public void Awake()
        {
            Physics.gravity = new Vector3(0, -12.5f, 0);
            Physics.defaultSolverIterations = 8;
            Physics.defaultSolverVelocityIterations = 2;
        }

        public void Start()
        {
            if (CharacterEditor.CharacterJson != null)
            {
                Character.FromJson(CharacterEditor.CharacterJson);
            }
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                #if UNITY_EDITOR

                if (UnityEditor.EditorBuildSettings.scenes.All(i => !i.path.Contains(ReturnSceneName)))
                {
                    throw new Exception("Please add the following scene to Build Settings: " + ReturnSceneName);
                }

                #endif

                SceneManager.LoadScene(ReturnSceneName);
            }
        }
    }
}