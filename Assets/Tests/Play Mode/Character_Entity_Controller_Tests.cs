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
        public void Create_Player_Character_Function_Creates_Entity_View()
        {
            // Arange
            CharacterEntityModel model;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);

            // Assert
            Assert.IsNotNull(model.characterEntityView);
        }
        [Test]
        public void Create_Player_Character_Function_Occupies_Node()
        {
            // Act
            CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);

            // Assert
            Assert.IsTrue(defenderNode.occupied);
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);

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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);

            // Assert
            Assert.AreEqual(model.characterEntityView.uiCanvas.worldCamera, CameraManager.Instance.MainCamera);
        }
        [Test]
        public void Create_Enemy_Character_Function_Creates_Entity_View()
        {
            // Arange
            CharacterEntityModel model;

            // Act
            model = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, enemyNode);

            // Assert
            Assert.IsNotNull(model.characterEntityView);
        }
        [Test]
        public void Create_Enemy_Character_Function_Occupies_Node()
        {
            // Act
            CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, enemyNode);

            // Assert
            Assert.IsTrue(enemyNode.occupied);
        }
        [Test]
        public void Create_Enemy_Character_Function_Builds_UCM()
        {
            // Arange
            CharacterEntityModel model;
            string expected;

            // Act
            expected = "Human_Chest";
            enemyData.allBodyParts.Add(expected);
            model = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, enemyNode);

            // Assert
            Assert.AreEqual(expected, model.characterEntityView.ucm.activeChest.name);
        }
        [Test]
        public void Create_Enemy_Character_Function_Adds_Character_To_Persistency()
        {
            // Arange
            CharacterEntityModel model;
            bool expected = false;

            // Act
            model = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, enemyNode);
            if (CharacterEntityController.Instance.AllCharacters.Contains(model) &&
                CharacterEntityController.Instance.AllEnemies.Contains(model))
            {
                expected = true;
            }

            // Assert
            Assert.IsTrue(expected);
        }
        [Test]
        public void Modify_Health_Cant_Set_Health_Above_Maximum()
        {
            // Arange
            CharacterEntityModel model;
            int expectedHealth = characterData.health;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            CharacterEntityController.Instance.ModifyHealth(model, 100);

            // Assert
            Assert.AreEqual(expectedHealth, model.health);
        }
        [Test]
        public void Modify_Health_Cant_Set_Health_Below_Zero()
        {
            // Arange
            CharacterEntityModel model;
            int expectedHealth = 0;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            CharacterEntityController.Instance.ModifyHealth(model, -100);

            // Assert
            Assert.AreEqual(expectedHealth, model.health);
        }
        [Test]
        public void Modify_Max_Health_Adjusts_Current_Health_Below_Maximum_If_Maximum_Is_Exceeded()
        {
            // Arange
            CharacterEntityModel model;
            int expectedHealth = 25;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            CharacterEntityController.Instance.ModifyMaxHealth(model, -5);

            // Assert
            Assert.AreEqual(expectedHealth, model.health);
        }
        [Test]
        public void Modify_Initiative_Cant_Set_Initiative_Below_Zero()
        {
            // Arange
            CharacterEntityModel model;
            int expectedInitiative = 0;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            CharacterEntityController.Instance.ModifyInitiative(model, 50);
            CharacterEntityController.Instance.ModifyInitiative(model, -100);

            // Assert
            Assert.AreEqual(expectedInitiative, model.dexterity);
        }
        [Test]
        public void Modify_Draw_Cant_Set_Draw_Below_Zero()
        {
            // Arange
            CharacterEntityModel model;
            int expectedDraw = 0;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            CharacterEntityController.Instance.ModifyDraw(model, 50);
            CharacterEntityController.Instance.ModifyDraw(model, -100);

            // Assert
            Assert.AreEqual(expectedDraw, model.draw);
        }
        [Test]
        public void Modify_Energy_Cant_Set_Energy_Below_Zero()
        {
            // Arange
            CharacterEntityModel model;
            int expectedEnergy = 0;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            CharacterEntityController.Instance.ModifyEnergy(model, 50);
            CharacterEntityController.Instance.ModifyEnergy(model, -1000);

            // Assert
            Assert.AreEqual(expectedEnergy, model.energy);
        }
        [Test]
        public void Modify_Stamina_Cant_Set_Stamina_Below_Zero()
        {
            // Arange
            CharacterEntityModel model;
            int expectedStamina = 0;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            CharacterEntityController.Instance.ModifyStamina(model, 50);
            CharacterEntityController.Instance.ModifyStamina(model, -1000);

            // Assert
            Assert.AreEqual(expectedStamina, model.stamina);
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
