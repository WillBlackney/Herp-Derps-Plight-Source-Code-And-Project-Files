using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class Activation_Controller_Tests
    {        

        // Declarations + Setup
        #region
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
                deck = new List<CardData>(),
            };
                        
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
        #endregion


        // TESTS TO WRITE
        /* If character dies during turn, next entity does activate
         * If character dies during CharacterOnActivationEnd phase (e.g. from poisoned damage), next entity does activate
         * If character dies during turn and is last defender, game over event does trigger
         * If character dies during turn and is last enemy, victory event does trigger 
         * 
         */
        [Test]
        public void Player_Character_Death_During_Turn_Does_Activate_Next_Entity()
        {
            // ARRANGE
            CharacterEntityModel playerOne;
            CharacterEntityModel playerTwo;
            CharacterEntityModel enemyOne;
            CharacterEntityModel expected;

            // Create characters
            playerOne = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, LevelManager.Instance.GetNextAvailableDefenderNode());
            playerTwo = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, LevelManager.Instance.GetNextAvailableDefenderNode());
            enemyOne = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, LevelManager.Instance.GetNextAvailableEnemyNode());
            expected = playerTwo;

            // Rig and fix activation order
            playerOne.initiative = 100;
            playerTwo.initiative = 200;
            enemyOne.initiative = 300;

            // ACT
            ActivationManager.Instance.OnNewCombatEventStarted();
            CombatLogic.Instance.HandleDamage(1000, null, playerOne, DamageType.Physical);

            // ASSERT
            Assert.AreEqual(expected, ActivationManager.Instance.EntityActivated);

        }
        [Test]
        public void Player_Character_Death_During_On_Activation_End_Routine_Does_Activate_Next_Entity()
        {
            // ARRANGE
            CharacterEntityModel playerOne;
            CharacterEntityModel playerTwo;
            CharacterEntityModel enemyOne;
            CharacterEntityModel enemyTwo;
            CharacterEntityModel expected;

            // Create characters
            playerOne = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, LevelManager.Instance.GetNextAvailableDefenderNode());
            playerTwo = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, LevelManager.Instance.GetNextAvailableDefenderNode());
            enemyOne = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, LevelManager.Instance.GetNextAvailableEnemyNode());
            enemyTwo = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, LevelManager.Instance.GetNextAvailableEnemyNode());
            PassiveController.Instance.ModifyPoisoned(null, playerOne.pManager, 1000);
            expected = playerTwo;

            // Rig and fix activation order
            playerOne.initiative = 300;
            playerTwo.initiative = 200;
            enemyOne.initiative = 100;
            enemyTwo.initiative = 50;

            // ACT
            ActivationManager.Instance.OnNewCombatEventStarted();
            CharacterEntityController.Instance.CharacterOnActivationEnd(ActivationManager.Instance.EntityActivated);

            // ASSERT
            Assert.AreEqual(expected, ActivationManager.Instance.EntityActivated);
        }
        [Test]
        public void Enemy_Character_Death_During_On_Activation_End_Routine_Does_Activate_Next_Entity()
        {
            // ARRANGE
            CharacterEntityModel playerOne;
            CharacterEntityModel playerTwo;
            CharacterEntityModel enemyOne;
            CharacterEntityModel enemyTwo;
            CharacterEntityModel expected;

            // Create characters
            playerOne = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, LevelManager.Instance.GetNextAvailableDefenderNode());
            playerTwo = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, LevelManager.Instance.GetNextAvailableDefenderNode());
            enemyOne = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, LevelManager.Instance.GetNextAvailableEnemyNode());
            enemyTwo = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, LevelManager.Instance.GetNextAvailableEnemyNode());
            PassiveController.Instance.ModifyPoisoned(null, enemyOne.pManager, 1000);
            expected = playerTwo;

            // Rig and fix activation order
            playerOne.initiative = 100;
            playerTwo.initiative = 200;
            enemyOne.initiative = 300;
            enemyTwo.initiative = 50;

            // ACT
            ActivationManager.Instance.OnNewCombatEventStarted();

            // ASSERT
            Assert.AreEqual(expected, ActivationManager.Instance.EntityActivated);
        }
    }
}
