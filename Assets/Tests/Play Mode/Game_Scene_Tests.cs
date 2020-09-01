using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Game_Scene_Tests
    {

        // Singletons
        CharacterEntityController characterEntityController;
        CardController cardController;
        LevelManager levelManager;
        PrefabHolder prefabHolder;
        CameraManager cameraManager;
        VisualEventManager visualEventManager;
        ActivationManager activationManager;
        PassiveController passiveController;
        PositionLogic positionLogic;

        // Mock data
        CharacterData characterData;
        List<CardDataSO> deckData;

        // Scenes
        GameObject gameScene;

        [OneTimeSetUp]
        public void Setup()
        {
            // Create Game Scene
            gameScene = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Scenes/Game Scene.prefab"));
            
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
        }

        [Test]
        public void Create_Player_Character_Function_Creates_Entity_View()
        {
            // Arange
            CharacterEntityModel model;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, null);

            // Assert
            Assert.IsNotNull(model.characterEntityView);
        }

        [Test]
        public void Game_Scene_Exists()
        {
            // Assert
            Assert.IsNotNull(gameScene);
        }

        [Test]
        public void Character_Entity_Controller_Exists()
        {
            // Assert
            Assert.IsNotNull(CharacterEntityController.Instance);
        }

        [Test]
        public void Card_Controller_Exists()
        {
            // Assert
            Assert.IsNotNull(CardController.Instance);
        }

        [Test]
        public void Level_Manager_Exists()
        {
            // Assert
            Assert.IsNotNull(LevelManager.Instance);
        }

        [Test]
        public void Prefab_Holder_Exists()
        {
            // Assert
            Assert.IsNotNull(PrefabHolder.Instance);
        }
        [Test]
        public void Card_Asset_Does_Load()
        {
            int expected = 1;

            // Assert
            Assert.AreEqual(expected, deckData.Count);
        }
        [Test]
        public void Card_Asset_Strike_Does_Load()
        {
            string expected = "Strike";

            // Assert
            Assert.AreEqual(expected, deckData[0].cardName);
        }

        [Test]
        public void Character_Data_Deck_Exists()
        {
            // Assert
            Assert.IsNotNull(characterData.deck);
        }

    }
}
