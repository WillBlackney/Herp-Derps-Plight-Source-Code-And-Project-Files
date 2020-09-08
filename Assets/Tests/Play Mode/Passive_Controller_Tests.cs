using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class Passive_Controller_Tests
    {
        // Scene ref
        private const string SCENE_NAME = "NewCombatSceneTest";

        // Mock data
        CharacterData characterData;
        List<CardDataSO> deckData;
        LevelNode node;

        // Pasive name refs;
        private const string POWER_NAME = "Power";
        private const string TEMPORARY_POWER_NAME = "Temporary Power";
        private const string DRAW_NAME = "Draw";
        private const string TEMPORARY_DRAW_NAME = "Temporary Draw";
        private const string INITIATIVE_NAME = "Initiative";
        private const string TEMPORARY_INITIATIVE_NAME = "Temporary Initiative";
        private const string DEXTERITY_NAME = "Dexterity";
        private const string TEMPORARY_DEXTERITY_NAME = "Temporary Dexterity";
        private const string STAMINA_NAME = "Stamina";
        private const string TEMPORARY_STAMINA_NAME = "Temporary Stamina";

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
            node = LevelManager.Instance.GetNextAvailableDefenderNode();
        }

        [Test]
        public void Build_Player_Character_Entity_Passives_From_Character_Data_Applies_ALL_Passives_Correctly()
        {
            // Arange
            CharacterEntityModel model;
            int stacks = 1;
            bool expected = false;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);

            // Apply every single passive in the game
            // Core stats
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, POWER_NAME, stacks);
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, STAMINA_NAME, stacks);
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, DRAW_NAME, stacks);
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, INITIATIVE_NAME, stacks);
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, DEXTERITY_NAME, stacks);

            // Temporary Core stats
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, TEMPORARY_POWER_NAME, stacks);
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, TEMPORARY_STAMINA_NAME, stacks);
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, TEMPORARY_DRAW_NAME, stacks);
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, TEMPORARY_INITIATIVE_NAME, stacks);
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, TEMPORARY_DEXTERITY_NAME, stacks);

            if (model.passiveManager.bonusDrawStacks == 1 &&
                model.passiveManager.bonusStaminaStacks == 1 &&
                model.passiveManager.bonusPowerStacks == 1 &&
                model.passiveManager.bonusDexterityStacks == 1 &&
                model.passiveManager.bonusInitiativeStacks == 1 &&

                model.passiveManager.temporaryBonusDrawStacks == 1 &&
                model.passiveManager.temporaryBonusStaminaStacks == 1 &&
                model.passiveManager.temporaryBonusPowerStacks == 1 &&
                model.passiveManager.temporaryBonusDexterityStacks == 1 &&
                model.passiveManager.temporaryBonusInitiativeStacks == 1 )
            {
                expected = true;
            }

            // Assert
            Assert.IsTrue(expected);
        }
        [Test]
        public void Modify_Bonus_Power_Effects_Total_Power()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(POWER_NAME).passiveName;
            int expectedTotal;
            int stacks = 2;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
            expectedTotal = stacks + EntityLogic.GetTotalPower(model);
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, passiveName, stacks);

            // Assert
            Assert.AreEqual(expectedTotal, EntityLogic.GetTotalPower(model));
        }
        [Test]
        public void Modify_Temporary_Bonus_Power_Effects_Total_Power()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(TEMPORARY_POWER_NAME).passiveName;
            int expectedTotal;
            int stacks = 2;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
            expectedTotal = stacks + EntityLogic.GetTotalPower(model);
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, passiveName, stacks);

            // Assert
            Assert.AreEqual(expectedTotal, EntityLogic.GetTotalPower(model));
        }
        [Test]
        public void Modify_Bonus_Dexterity_Effects_Total_Dexterity()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(DEXTERITY_NAME).passiveName;
            int expectedTotal;
            int stacks = 2;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
            expectedTotal = stacks + EntityLogic.GetTotalDexterity(model);
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, passiveName, stacks);

            // Assert
            Assert.AreEqual(expectedTotal, EntityLogic.GetTotalDexterity(model));
        }
        [Test]
        public void Modify_Temporary_Bonus_Dexterity_Effects_Total_Dexterity()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(TEMPORARY_DEXTERITY_NAME).passiveName;
            int expectedTotal;
            int stacks = 2;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
            expectedTotal = stacks + EntityLogic.GetTotalDexterity(model);
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, passiveName, stacks);

            // Assert
            Assert.AreEqual(expectedTotal, EntityLogic.GetTotalDexterity(model));
        }
        [Test]
        public void Modify_Bonus_Initiative_Effects_Total_Initiative()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(INITIATIVE_NAME).passiveName;
            int expectedTotal;
            int stacks = 2;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
            expectedTotal = stacks + EntityLogic.GetTotalInitiative(model);
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, passiveName, stacks);

            // Assert
            Assert.AreEqual(expectedTotal, EntityLogic.GetTotalInitiative(model));
        }
        [Test]
        public void Modify_Temporary_Bonus_Initiative_Effects_Total_Initiative()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(TEMPORARY_INITIATIVE_NAME).passiveName;
            int expectedTotal;
            int stacks = 2;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
            expectedTotal = stacks + EntityLogic.GetTotalInitiative(model);
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, passiveName, stacks);

            // Assert
            Assert.AreEqual(expectedTotal, EntityLogic.GetTotalInitiative(model));
        }
        [Test]
        public void Modify_Bonus_Draw_Effects_Total_Draw()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(DRAW_NAME).passiveName;
            int expectedTotal;
            int stacks = 2;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
            expectedTotal = stacks + EntityLogic.GetTotalDraw(model);
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, passiveName, stacks);

            // Assert
            Assert.AreEqual(expectedTotal, EntityLogic.GetTotalDraw(model));
        }
        [Test]
        public void Modify_Temporary_Draw_Effects_Total_Draw()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(TEMPORARY_DRAW_NAME).passiveName;
            int expectedTotal;
            int stacks = 2;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
            expectedTotal = stacks + EntityLogic.GetTotalDraw(model);
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, passiveName, stacks);

            // Assert
            Assert.AreEqual(expectedTotal, EntityLogic.GetTotalDraw(model));
        }
        [Test]
        public void Modify_Bonus_Stamina_Effects_Total_Stamina()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(STAMINA_NAME).passiveName;
            int expectedTotal;
            int stacks = 2;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
            expectedTotal = stacks + EntityLogic.GetTotalStamina(model);
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, passiveName, stacks);

            // Assert
            Assert.AreEqual(expectedTotal, EntityLogic.GetTotalStamina(model));
        }
        [Test]
        public void Modify_Temporary_Stamina_Effects_Total_Stamina()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(TEMPORARY_STAMINA_NAME).passiveName;
            int expectedTotal;
            int stacks = 2;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
            expectedTotal = stacks + EntityLogic.GetTotalStamina(model);
            PassiveController.Instance.ApplyPassiveToCharacterEntity(model.passiveManager, passiveName, stacks);

            // Assert
            Assert.AreEqual(expectedTotal, EntityLogic.GetTotalStamina(model));
        }


    }

   
}
