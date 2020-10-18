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
        List<CardDataSO> deckData;
        LevelNode defenderNode;

        // Mock card data SO's
        CardDataSO mockExpendCard;
        CardDataSO mockPowerCard;

        [UnitySetUp]
        public IEnumerator Setup()
        {
            // Load Scene, wait until completed
            AsyncOperation loading = SceneManager.LoadSceneAsync(SCENE_NAME);
            yield return new WaitUntil(() => loading.isDone);
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

            // Create mock deck data
            deckData = new List<CardDataSO>();

            // Create mock cards
            mockExpendCard = AssetDatabase.LoadAssetAtPath<CardDataSO>("Assets/Tests/Mock Data Files/Mock Exhaust Card.asset");
            mockPowerCard = AssetDatabase.LoadAssetAtPath<CardDataSO>("Assets/Tests/Mock Data Files/Mock Power Card.asset");

            // Create mock model data
            characterData.modelParts = new List<string>();

            // Create mock passive data
            characterData.passiveManager = new PassiveManagerModel();

            // Create mock level node
            defenderNode = LevelManager.Instance.GetNextAvailableDefenderNode();
        }


        // Expend Tests
        #region
        [Test]
        public void Expend_Card_Is_Removed_From_All_Character_Collections_When_Played()
        {
            // Arange
            CharacterEntityModel model;
            bool expected;

            // Act 
            CardData newCard = CardController.Instance.BuildCardDataFromScriptableObjectData(mockExpendCard, characterData);
            characterData.deck.Add(newCard);

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
        public void Expend_Card_Is_Moved_To_Expend_Pile_When_Played()
        {
            // Arange
            CharacterEntityModel model;
            bool expected;

            // Act 
            CardData newCard = CardController.Instance.BuildCardDataFromScriptableObjectData(mockExpendCard, characterData);
            characterData.deck.Add(newCard);

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
            bool expected;
            GlobalSettings.Instance.onPowerCardPlayedSetting = OnPowerCardPlayedSettings.RemoveFromPlay;

            // Act 
            CardData newCard = CardController.Instance.BuildCardDataFromScriptableObjectData(mockPowerCard, characterData);
            characterData.deck.Add(newCard);

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
            bool expected;
            GlobalSettings.Instance.onPowerCardPlayedSetting = OnPowerCardPlayedSettings.Expend;

            // Act 
            CardData newCard = CardController.Instance.BuildCardDataFromScriptableObjectData(mockPowerCard, characterData);
            characterData.deck.Add(newCard);

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

            // Act 
            CardData newCard = CardController.Instance.BuildCardDataFromScriptableObjectData(mockPowerCard, characterData);
            characterData.deck.Add(newCard);

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

            for (int i = 0; i < 10; i++)
            {
                CardData newCard = CardController.Instance.BuildCardDataFromScriptableObjectData(mockPowerCard, characterData);
                characterData.deck.Add(newCard);
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
                CardData newCard = CardController.Instance.BuildCardDataFromScriptableObjectData(mockPowerCard, characterData);
                characterData.deck.Add(newCard);
            }

            int expectedHandCount = 8;
            GlobalSettings.Instance.innateSetting = InnateSettings.DrawInnateCardsExtra;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            for (int i = 0; i < 3; i++)
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
