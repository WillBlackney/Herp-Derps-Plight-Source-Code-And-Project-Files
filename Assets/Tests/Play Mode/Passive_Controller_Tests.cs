using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Passive_Controller_Tests
    {
        // Mock data
        CharacterData characterData;
        List<CardDataSO> deckData;
        LevelNode node;

        [OneTimeSetUp]
        public void Setup()
        {
            // Create Game Scene
            //gameScene = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Scenes/Game Scene.prefab"));
            GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Scenes/Game Scene.prefab"));

            // Create mock character data
            characterData = new CharacterData
            {
                myName = "Test Runner Name",
                health = 30,
                maxHealth = 30,
                stamina = 30,
                initiative = 3,
                draw = 5,
                dexterity = 0,
                power = 0,
            };

            // Create mock deck data
            deckData = new List<CardDataSO>();
            characterData.deck = deckData;
            deckData.Add(AssetDatabase.LoadAssetAtPath<CardDataSO>("Assets/SO Assets/Cards/Strike.asset"));

            // Create mock model data
            characterData.modelParts = new List<string>();

            // Create mock passive data
            characterData.passiveManager = new PassiveManagerModel();

            // Create mock level node
            node = LevelManager.Instance.GetNextAvailableDefenderNode();
        }
       
    }
}
