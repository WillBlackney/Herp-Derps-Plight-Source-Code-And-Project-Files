using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System.Linq;

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
        CardDataSO mockMeleeAttackCard;
        CardDataSO mockRangedAttackCard;
        CardDataSO mockInnateCard;

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
            mockMeleeAttackCard = AssetDatabase.LoadAssetAtPath<CardDataSO>("Assets/Tests/Mock Data Files/Mock Melee Attack Card.asset");
            mockRangedAttackCard = AssetDatabase.LoadAssetAtPath<CardDataSO>("Assets/Tests/Mock Data Files/Mock Ranged Attack Card.asset");
            mockInnateCard = AssetDatabase.LoadAssetAtPath<CardDataSO>("Assets/Tests/Mock Data Files/Mock Innate Card.asset");

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


        // Expend Tests
        #region
        [Test]
        public void Expend_Card_Is_Removed_From_All_Character_Collections_When_Played()
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
        public void Expend_Card_Is_Moved_To_Expend_Pile_When_Played()
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

            if (model.expendPile.Contains(card) == false)
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
        #endregion

        // Power Card Tests
        #region
        [Test]
        public void Power_Card_Is_Removed_From_All_Character_Collections_When_Played()
        {
            // Arange
            CharacterEntityModel model;
            deckData.Add(mockPowerCard);
            bool expected;
            GlobalSettings.Instance.onPowerCardPlayedSetting = OnPowerCardPlayedSettings.RemoveFromPlay;

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
        [Test]
        public void Power_Card_Is_Expended_When_Played()
        {
            // Arange
            CharacterEntityModel model;
            deckData.Add(mockPowerCard);
            bool expected;
            GlobalSettings.Instance.onPowerCardPlayedSetting = OnPowerCardPlayedSettings.Expend;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            Card card = model.drawPile[0];
            ActivationManager.Instance.OnNewCombatEventStarted();
            CardController.Instance.PlayCardFromHand(card);

            if (model.hand.Contains(card) ||
               model.drawPile.Contains(card) ||
               model.discardPile.Contains(card) || 
               model.expendPile.Contains(card) == false)
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
        public void Power_Card_Is_Shuffled_To_Discard_Pile_When_Played_From_Hand()
        {
            // Arange
            CharacterEntityModel model;
            GlobalSettings.Instance.onPowerCardPlayedSetting = OnPowerCardPlayedSettings.MoveToDiscardPile;
            deckData.Clear();
            deckData.Add(mockPowerCard);

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            Card theCard = model.drawPile[0];
            model.draw = 1;

           // Card card = new Card();
           // card.cardType = CardType.Power;
           // model.drawPile.Add(card);

            ActivationManager.Instance.OnNewCombatEventStarted();
            CardController.Instance.PlayCardFromHand(model.hand[0]);

            // Assert
            Assert.IsTrue(model.discardPile.Contains(theCard));
        }
        #endregion

        // Innate Tests
        #region
        [Test]
        public void Innate_Card_Is_Drawn_In_Opening_Hand()
        {
            // Arange
            CharacterEntityModel model;
            deckData.Clear();

            for(int i = 0; i < 10; i++)
            {
                deckData.Add(mockPowerCard);
            }

            GlobalSettings.Instance.innateSetting = InnateSettings.PrioritiseInnate;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            Card expectedCard = new Card();
            expectedCard.innate = true;
            model.drawPile.Add(expectedCard);
            model.draw = 1;

            ActivationManager.Instance.OnNewCombatEventStarted();

            // Assert
            Assert.AreEqual(expectedCard, model.hand[0]);
        }

        [Test]
        public void Innate_Cards_Are_Drawn_Extra_In_Opening_Hand()
        {
            // Arange
            CharacterEntityModel model;
            for (int i = 0; i < 10; i++)
            {
                deckData.Add(mockPowerCard);
            }
            
            int expectedHandCount = 8;
            GlobalSettings.Instance.innateSetting = InnateSettings.DrawInnateCardsExtra;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            for(int i = 0; i < 3; i++)
            {
                Card newCard = new Card();
                newCard.innate = true;
                model.drawPile.Add(newCard);
            }

            model.draw = 5;

            ActivationManager.Instance.OnNewCombatEventStarted();

            // Assert
            Assert.AreEqual(expectedHandCount, model.hand.Count);
        }
        #endregion
    }
}
