using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class Combat_Logic_Tests
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
            GameObject.FindObjectOfType<CombatTestSceneController>().runMockScene = false;

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


        // test to run
        // test pierce doesnt remove block and does reduce health
        // test handle death gets called if handle damage kills them
        // test game over defeat event is triggered if all allies die
        // test game over victory event is triggered if all enemies die
        // test if node position becomes available after death
        // test next character activates correctly if character is killed during their own turn
        // check that game over defeat is triggered if the last enemy and ally die at the same time

        // TESTS TO WRITE
        /*
         * If character dies during turn and is last defender, game over event does trigger
         * If character dies during turn and is last enemy, victory event does trigger 
         * 
         */

        [Test]
        public void Handle_Damage_Correctly_Sets_Death_State_When_Health_Reaches_0()
        {
            // Arange
            CharacterEntityModel enemyCharacter;

            // Act
            enemyCharacter = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, enemyNode);
            CombatLogic.Instance.HandleDamage(1000, null, enemyCharacter, DamageType.Physical, null);

            // Assert
            Assert.IsTrue(enemyCharacter.livingState == LivingState.Dead);
        }
        [Test]
        public void Last_Player_Character_Death_Does_Trigger_Defeat_Event()
        {
            // ARRANGE
            CharacterEntityModel playerOne;
            CharacterEntityModel enemyOne;
            CombatGameState expected = CombatGameState.DefeatTriggered;

            // Create characters
            playerOne = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, LevelManager.Instance.GetNextAvailableDefenderNode());
            enemyOne = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, LevelManager.Instance.GetNextAvailableEnemyNode());

            // Rig and fix activation order
            playerOne.initiative = 200;
            enemyOne.initiative = 100;

            // ACT
            ActivationManager.Instance.OnNewCombatEventStarted();
            CombatLogic.Instance.HandleDamage(1000, null, playerOne, DamageType.Physical);

            // ASSERT
            Assert.AreEqual(expected, CombatLogic.Instance.CurrentCombatState);
        }
        [Test]
        public void Last_Enemy_Character_Death_Does_Trigger_Victory_Event()
        {
            // ARRANGE
            CharacterEntityModel playerOne;
            CharacterEntityModel enemyOne;
            CombatGameState expected = CombatGameState.VictoryTriggered;

            // Create characters
            playerOne = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, LevelManager.Instance.GetNextAvailableDefenderNode());
            enemyOne = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, LevelManager.Instance.GetNextAvailableEnemyNode());

            // Rig and fix activation order
            playerOne.initiative = 200;
            enemyOne.initiative = 100;

            // ACT
            ActivationManager.Instance.OnNewCombatEventStarted();
            CombatLogic.Instance.HandleDamage(1000, null, enemyOne, DamageType.Physical);

            // ASSERT
            Assert.AreEqual(expected, CombatLogic.Instance.CurrentCombatState);

        }
    }
}
