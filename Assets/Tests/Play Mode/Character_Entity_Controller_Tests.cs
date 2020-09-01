using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Character_Entity_Controller_Tests
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

        [SetUp]
        public void Setup()
        {
            // Create Character Entity Controller
            characterEntityController = new GameObject().AddComponent<CharacterEntityController>();
            characterEntityController.RunAwake();

            // Create Card Controller
            cardController = new GameObject().AddComponent<CardController>();
            cardController.RunAwake();

            // Create Level Manager
            levelManager = new GameObject().AddComponent<LevelManager>();
            levelManager.RunAwake();

            // Create Camera Manager
            cameraManager = new GameObject().AddComponent<CameraManager>();
            cameraManager.RunAwake();

            // Create Camera Manager
            activationManager = new GameObject().AddComponent<ActivationManager>();
            activationManager.RunAwake();

            // Create Camera Manager
            passiveController = new GameObject().AddComponent<PassiveController>();
            passiveController.RunAwake();

            // Create Camera Manager
            positionLogic = new GameObject().AddComponent<PositionLogic>();
            positionLogic.RunAwake();

            // Create Visual Event Manager
            visualEventManager = new GameObject().AddComponent<VisualEventManager>();
            visualEventManager.RunAwake();
            visualEventManager.PauseQueue();

            // Create Prefab Holder
            prefabHolder = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/TESTING PREFABS/Prefab Holder.prefab").GetComponent<PrefabHolder>());
            prefabHolder.RunAwake();

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
        }

        [Test]
        public void Create_Player_Character_Function_Creates_Entity_View()
        {
            // Arange
            CharacterEntityModel model;

            // Act
            model = characterEntityController.CreatePlayerCharacter(characterData, null);

            // Assert
            Assert.IsNotNull(model.characterEntityView);
        }

        [Test]
        public void Character_Entity_Controller_Exists()
        {
            // Assert
            Assert.IsNotNull(characterEntityController);
        }        

        [Test]
        public void Card_Controller_Exists()
        {
            // Assert
            Assert.IsNotNull(cardController);
        }

        [Test]
        public void Level_Manager_Exists()
        {
            // Assert
            Assert.IsNotNull(levelManager);
        }

        [Test]
        public void Prefab_Holder_Exists()
        {
            // Assert
            Assert.IsNotNull(prefabHolder);
        }
    }
}
