using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class Level_Manager_Tests
    {
        // Scene ref
        private const string SCENE_NAME = "NewCombatSceneTest";

        // Mock data
        CharacterData characterData;
        EnemyDataSO enemyData;
        List<CardDataSO> deckData;
        LevelNode defenderNode;
        LevelNode enemyNode;

        [UnitySetUp]
        public IEnumerator Setup()
        {
            // Load Scene, wait until completed
            AsyncOperation loading = SceneManager.LoadSceneAsync(SCENE_NAME);
            yield return new WaitUntil(() => loading.isDone);
            //GameObject.FindObjectOfType<CombatTestSceneController>().runMockScene = false;
            GameObject.FindObjectOfType<GlobalSettings>().gameMode = StartingSceneSetting.IntegrationTesting;

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
            defenderNode = LevelManager.Instance.GetNextAvailableDefenderNode();
            enemyNode = LevelManager.Instance.GetNextAvailableEnemyNode();

            // Create mock enemy data
            enemyData = AssetDatabase.LoadAssetAtPath<EnemyDataSO>("Assets/SO Assets/Enemies/Test Enemy.asset");
        }

        [Test]
        public void Level_Node_Does_Become_Unoccupied_When_Its_Character_Dies()
        {
            // Arange
            CharacterEntityModel enemyCharacter;
            bool expected = false;

            // Act
            enemyCharacter = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, enemyNode);
            CombatLogic.Instance.HandleDamage(1000, null, enemyCharacter, DamageType.Physical);

            // Assert
            Assert.AreEqual(expected, enemyNode.occupied);
        }
        [Test]
        public void Level_Node_Character_Reference_Does_Become_Null_When_Its_Character_Dies()
        {
            // Arange
            CharacterEntityModel enemyCharacter;
            CharacterEntityModel expected = null;

            // Act
            enemyCharacter = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, enemyNode);
            CombatLogic.Instance.HandleDamage(1000, null, enemyCharacter, DamageType.Physical);

            // Assert
            Assert.AreEqual(expected, enemyNode.myEntity);
        }
    }
}
