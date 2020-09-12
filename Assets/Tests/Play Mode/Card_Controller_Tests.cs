using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class Card_Controller_Tests
    {
        // Scene ref
        private const string SCENE_NAME = "NewCombatSceneTest";

        // Mock data
        CharacterData characterData;
        EnemyDataSO enemyData;
        List<CardDataSO> deckData;
        LevelNode defenderNode;
        LevelNode enemyNode;

        // Mock card data SO's
        CardDataSO mockExpendCard;
        CardDataSO mockPowerCard;

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

            // Create mock cards
            mockExpendCard = AssetDatabase.LoadAssetAtPath<CardDataSO>("Assets/Tests/Mock Data Files/Mock Exhaust Card.asset");
            mockPowerCard = AssetDatabase.LoadAssetAtPath<CardDataSO>("Assets/Tests/Mock Data Files/Mock Power Card.asset");

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
        public void Exhaust_Card_Is_Removed_From_All_Character_Collections_When_Played_From_Hand()
        {
            // Arange
            CharacterEntityModel model;
            deckData.Add(mockExpendCard);
            bool expected;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            Card card = model.drawPile[0];
            ActivationManager.Instance.OnNewCombatEventStarted();
            CardController.Instance.PlayCardFromHand(card);

            if(model.hand.Contains(card) ||
               model.drawPile.Contains(card) ||
               model.discardPile.Contains(card))
            {
                expected = false;
            }
            else
            {
                expected = true;
            }

            // Assert
            Assert.IsTrue(expected);
        }
        [Test]
        public void Power_Card_Is_Removed_From_All_Character_Collections_When_Played_From_Hand()
        {
            // Arange
            CharacterEntityModel model;
            deckData.Add(mockPowerCard);
            bool expected;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            Card card = model.drawPile[0];
            ActivationManager.Instance.OnNewCombatEventStarted();
            CardController.Instance.PlayCardFromHand(card);

            if (model.hand.Contains(card) ||
               model.drawPile.Contains(card) ||
               model.discardPile.Contains(card))
            {
                expected = false;
            }
            else
            {
                expected = true;
            }

            // Assert
            Assert.IsTrue(expected);
        }
    }
}
