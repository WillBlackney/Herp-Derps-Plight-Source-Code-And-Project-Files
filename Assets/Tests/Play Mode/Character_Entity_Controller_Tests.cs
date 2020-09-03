using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class Character_Entity_Controller_Tests
    {
        // Scene ref
        private const string SCENE_NAME = "NewCombatSceneTest";

        // Mock data
        CharacterData characterData;
        List<CardDataSO> deckData;
        LevelNode node;

        [UnitySetUp]
        public IEnumerator Setup()
        {
            // Create Game Scene
            //GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Scenes/Game Scene.prefab"));

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
            node = LevelManager.Instance.GetNextAvailableDefenderNode();
        }

        [Test]
        public void Create_Player_Character_Function_Creates_Entity_View()
        {
            // Arange
            CharacterEntityModel model;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);

            // Assert
            Assert.IsNotNull(model.characterEntityView);
        }
        [Test]
        public void Create_Player_Character_Function_Builds_UCM()
        {
            // Arange
            CharacterEntityModel model;
            string expected;

            // Act
            expected = "Human_Chest";
            characterData.modelParts.Add(expected);
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);

            // Assert
            Assert.AreEqual(expected, model.characterEntityView.ucm.activeChest.name);
        }
        [Test]
        public void Create_Player_Character_Function_Adds_Character_To_Persistency()
        {
            // Arange
            CharacterEntityModel model;
            bool expected = false;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
            if (CharacterEntityController.Instance.AllCharacters.Contains(model) &&
                CharacterEntityController.Instance.AllDefenders.Contains(model))
            {
                expected = true;
            }

            // Assert
            Assert.IsTrue(expected);
        }
        [Test]
        public void Create_Player_Character_Function_Correctly_Sets_Main_Camera_On_UI_Canvas()
        {
            // Arange
            CharacterEntityModel model;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);

            // Assert
            Assert.AreEqual(model.characterEntityView.uiCanvas.worldCamera, CameraManager.Instance.MainCamera);
        }




        // TEST ENVIRONMENT SETUP CHECKING TESTS
        /*
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
        */
    }
}
