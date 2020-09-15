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

        // Mock cards
        CardDataSO mockMeleeAttackCard;
        CardDataSO mockRangedAttackCard;
        CardDataSO mockSkillDamagingCard;

        // Pasive name refs;

        // Core Stats
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

        // Buff Stats
        private const string ENRAGE_NAME = "Enrage";
        private const string SHIELD_WALL_NAME = "Shield Wall";
        private const string FAN_OF_KNIVES_NAME = "Fan Of Knives";
        private const string PHOENIX_FORM_NAME = "Phoenix Form";
        private const string POISONOUS_NAME = "Poisonous";
        private const string VENOMOUS_NAME = "Venomous";
        private const string OVERLOAD_NAME = "Overload";

        // Core % Modifer Stats
        private const string WRATH_NAME = "Wrath";
        private const string WEAKENED_NAME = "Weakened";
        private const string GRIT_NAME = "Grit";
        private const string VULNERABLE_NAME = "Vulnerable";      

        // Misc passives
        private const string TAUNTED_NAME = "Taunted";
        private const string FIRE_BALL_BONUS_DAMAGE_NAME = "Fire Ball Bonus Damage";

        // DoTs 
        private const string POISONED_NAME = "Poisoned";
        private const string BURNING_NAME = "Burning";

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

            // Create mock cards
            mockMeleeAttackCard = AssetDatabase.LoadAssetAtPath<CardDataSO>("Assets/Tests/Mock Data Files/Mock Melee Attack Card.asset");
            mockRangedAttackCard = AssetDatabase.LoadAssetAtPath<CardDataSO>("Assets/Tests/Mock Data Files/Mock Ranged Attack Card.asset");
            mockSkillDamagingCard = AssetDatabase.LoadAssetAtPath<CardDataSO>("Assets/Tests/Mock Data Files/Mock Skill Damaging Card.asset");

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

        // Core Stat + Temp Core Stat Tests
        #region
        [Test]
        public void Build_Player_Character_Entity_Passives_From_Character_Data_Applies_ALL_Passives_Correctly()
        {
            // Arange
            CharacterEntityModel model;
            int stacks = 1;
            bool expected = false;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);

            // Apply every single passive in the game

            // Core stats
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, POWER_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, STAMINA_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, DRAW_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, INITIATIVE_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, DEXTERITY_NAME, stacks);

            // Temporary Core stats
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, TEMPORARY_POWER_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, TEMPORARY_STAMINA_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, TEMPORARY_DRAW_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, TEMPORARY_INITIATIVE_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, TEMPORARY_DEXTERITY_NAME, stacks);

            // Buff stats
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, ENRAGE_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, SHIELD_WALL_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, FAN_OF_KNIVES_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, PHOENIX_FORM_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, POISONOUS_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, VENOMOUS_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, OVERLOAD_NAME, stacks);

            // Core % Modifer Stats
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, WEAKENED_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, WRATH_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, VULNERABLE_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, GRIT_NAME, stacks);

            // DoTs
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, POISONED_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, BURNING_NAME, stacks);

            // Misc
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, FIRE_BALL_BONUS_DAMAGE_NAME, stacks);

            if (model.pManager.bonusDrawStacks == 1 &&
                model.pManager.bonusStaminaStacks == 1 &&
                model.pManager.bonusPowerStacks == 1 &&
                model.pManager.bonusDexterityStacks == 1 &&
                model.pManager.bonusInitiativeStacks == 1 &&

                model.pManager.temporaryBonusDrawStacks == 1 &&
                model.pManager.temporaryBonusStaminaStacks == 1 &&
                model.pManager.temporaryBonusPowerStacks == 1 &&
                model.pManager.temporaryBonusDexterityStacks == 1 &&
                model.pManager.temporaryBonusInitiativeStacks == 1 &&

                model.pManager.enrageStacks == 1 &&
                model.pManager.shieldWallStacks == 1 &&
                model.pManager.fanOfKnivesStacks == 1 &&
                model.pManager.phoenixFormStacks == 1 &&
                model.pManager.poisonousStacks == 1 &&
                model.pManager.venomousStacks == 1 &&
                model.pManager.overloadStacks == 1 &&

                model.pManager.weakenedStacks == 1 &&
                model.pManager.wrathStacks == 1 &&
                model.pManager.vulnerableStacks == 1 &&
                model.pManager.gritStacks == 1 &&

                model.pManager.poisonedStacks == 1 &&
                model.pManager.burningStacks == 1 &&

                model.pManager.fireBallBonusDamageStacks == 1 
                )
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            expectedTotal = stacks + EntityLogic.GetTotalPower(model);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);

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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            expectedTotal = stacks + EntityLogic.GetTotalPower(model);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);

            // Assert
            Assert.AreEqual(expectedTotal, EntityLogic.GetTotalPower(model));
        }
        [Test]
        public void Temporary_Bonus_Power_Does_Expire_On_Character_Activation_End()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(TEMPORARY_POWER_NAME).passiveName;
            int stacks = 1;
            int expected = 0;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();
            CharacterEntityController.Instance.CharacterOnActivationEnd(model);

            // Assert
            Assert.AreEqual(expected, EntityLogic.GetTotalPower(model));
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            expectedTotal = stacks + EntityLogic.GetTotalDexterity(model);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);

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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            expectedTotal = stacks + EntityLogic.GetTotalDexterity(model);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);

            // Assert
            Assert.AreEqual(expectedTotal, EntityLogic.GetTotalDexterity(model));
        }
        [Test]
        public void Temporary_Bonus_Dexterity_Does_Expire_On_Character_Activation_End()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(TEMPORARY_DEXTERITY_NAME).passiveName;
            int stacks = 1;
            int expected = 0;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();
            CharacterEntityController.Instance.CharacterOnActivationEnd(model);

            // Assert
            Assert.AreEqual(expected, EntityLogic.GetTotalDexterity(model));
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            expectedTotal = stacks + EntityLogic.GetTotalInitiative(model);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);

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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            expectedTotal = stacks + EntityLogic.GetTotalInitiative(model);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);

            // Assert
            Assert.AreEqual(expectedTotal, EntityLogic.GetTotalInitiative(model));
        }
        [Test]
        public void Temporary_Bonus_Initiative_Does_Expire_After_New_Turn_Initiative_Roll()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(TEMPORARY_INITIATIVE_NAME).passiveName;
            int stacks = 3;
            int expected = 0;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();

            // Assert
            Assert.AreEqual(expected, model.pManager.temporaryBonusInitiativeStacks);
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            expectedTotal = stacks + EntityLogic.GetTotalDraw(model);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);

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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            expectedTotal = stacks + EntityLogic.GetTotalDraw(model);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);

            // Assert
            Assert.AreEqual(expectedTotal, EntityLogic.GetTotalDraw(model));
        }
        [Test]
        public void Temporary_Bonus_Draw_Does_Expire_After_Draw_Cards_Event_On_Activation_Start()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(TEMPORARY_DRAW_NAME).passiveName;
            int stacks = 3;
            int expected = 0;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            expected = EntityLogic.GetTotalDraw(model);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();

            // Assert
            Assert.AreEqual(expected, EntityLogic.GetTotalDraw(model));
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            expectedTotal = stacks + EntityLogic.GetTotalStamina(model);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);

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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            expectedTotal = stacks + EntityLogic.GetTotalStamina(model);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);

            // Assert
            Assert.AreEqual(expectedTotal, EntityLogic.GetTotalStamina(model));
        }
        [Test]
        public void Temporary_Bonus_Stamina_Does_Expire_On_Activation_Start()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(TEMPORARY_STAMINA_NAME).passiveName;
            int stacks = 30;
            int expected = 0;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            expected = EntityLogic.GetTotalStamina(model);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();

            // Assert
            Assert.AreEqual(expected, EntityLogic.GetTotalStamina(model));
        }
        #endregion

        // Buff Passive Tests
        #region
        [Test]        
        public void Enrage_Triggers_Power_Gain_In_Handle_Damage_Method()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(ENRAGE_NAME).passiveName;
            int stacks = 2;
            int expectedTotal = 2;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            CombatLogic.Instance.HandleDamage(10, null, model, DamageType.Physical);

            // Assert
            Assert.AreEqual(expectedTotal, EntityLogic.GetTotalPower(model));
        }
        [Test]
        public void Shield_Wall_Does_Grant_Block_On_Activation_Start()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(SHIELD_WALL_NAME).passiveName;
            int stacks = 3;
            int expected = 3;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();

            // Assert
            Assert.AreEqual(expected, model.block);
        }
        [Test]
        public void Fan_Of_Knives_Does_Grant_Shank_Cards_On_Activation_Start()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(FAN_OF_KNIVES_NAME).passiveName;
            int stacks = 3;
            int expected = 3;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();

            // how many shanks in hand?
            int shanksInHand = 0;
            foreach(Card card in model.hand)
            {
                if(card.cardName == "Shank")
                {
                    shanksInHand++;
                }
            }

            // Assert
            Assert.AreEqual(expected, shanksInHand);
        }
        [Test]
        public void Phoenix_Form_Does_Grant_Fire_Ball_Cards_On_Activation_Start()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(PHOENIX_FORM_NAME).passiveName;
            int stacks = 3;
            int expected = 3;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();

            // how many fire balls in hand?
            int fireBallsInHand = 0;
            foreach (Card card in model.hand)
            {
                if (card.cardName == "Fire Ball")
                {
                    fireBallsInHand++;
                }
            }

            // Assert
            Assert.AreEqual(expected, fireBallsInHand);
        }



        // Poisonous Tests
        // Player Character with poisonous does apply poison with melee attack/ ranged attack
        // Player Character with poisonous does NOT apply poison with skill ability
        // Enemy character with poisonous does apply poisonous with attack
        [Test]
        public void Poisonous_Player_Character_Does_Apply_Poison_With_Melee_Attack()
        {
            // Arange
            CharacterEntityModel playerModel;
            CharacterEntityModel enemyModel;
            Card card;
           
            deckData = new List<CardDataSO>();
            characterData.deck = deckData;
            deckData.Add(mockMeleeAttackCard);

            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(POISONOUS_NAME).passiveName;
            int stacks = 2;
            int expected = 2;

            // Act
            playerModel = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(playerModel.pManager, passiveName, stacks);
            playerModel.initiative = 1000;
            card = playerModel.drawPile[0];

            enemyModel = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, defenderNode);            
            
            ActivationManager.Instance.OnNewCombatEventStarted();
            CardController.Instance.PlayCardFromHand(card, enemyModel);

            // Assert 
            Assert.AreEqual(expected, enemyModel.pManager.poisonedStacks);
        }
        [Test]
        public void Poisonous_Player_Character_Does_Apply_Poison_With_Ranged_Attack()
        {
            // Arange
            CharacterEntityModel playerModel;
            CharacterEntityModel enemyModel;
            Card card;

            deckData = new List<CardDataSO>();
            characterData.deck = deckData;
            deckData.Add(mockRangedAttackCard);

            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(POISONOUS_NAME).passiveName;
            int stacks = 2;
            int expected = 2;

            // Act
            playerModel = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(playerModel.pManager, passiveName, stacks);
            playerModel.initiative = 1000;
            card = playerModel.drawPile[0];

            enemyModel = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, defenderNode);

            ActivationManager.Instance.OnNewCombatEventStarted();
            CardController.Instance.PlayCardFromHand(card, enemyModel);

            // Assert 
            Assert.AreEqual(expected, enemyModel.pManager.poisonedStacks);
        }
        [Test]
        public void Poisonous_Player_Character_Does_NOT_Apply_Poison_With_Non_Attack_Card()
        {
            // Arange
            CharacterEntityModel playerModel;
            CharacterEntityModel enemyModel;
            Card card;

            deckData = new List<CardDataSO>();
            characterData.deck = deckData;
            deckData.Add(mockSkillDamagingCard);

            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(POISONOUS_NAME).passiveName;
            int stacks = 2;
            int expected = 0;

            // Act
            playerModel = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(playerModel.pManager, passiveName, stacks);
            playerModel.initiative = 1000;
            card = playerModel.drawPile[0];

            enemyModel = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, defenderNode);

            ActivationManager.Instance.OnNewCombatEventStarted();
            CardController.Instance.PlayCardFromHand(card, enemyModel);

            // Assert 
            Assert.AreEqual(expected, enemyModel.pManager.poisonedStacks);
        }
        [Test]
        public void Poisonous_Enemy_Character_Does_Apply_Poison_With_Attack_Enemy_Action_Effect()
        {
            // Arange
            CharacterEntityModel playerModel;
            CharacterEntityModel enemyModel;

            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(POISONOUS_NAME).passiveName;
            int stacks = 2;
            int expected = 2;

            // Act
            playerModel = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);            
            enemyModel = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, defenderNode);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(enemyModel.pManager, passiveName, stacks);
            enemyModel.initiative = 1000;

            ActivationManager.Instance.OnNewCombatEventStarted();

            // Assert 
            Assert.AreEqual(expected, playerModel.pManager.poisonedStacks);            
        }

        // Venomous Tests
        // Venomous does work with poisonous
        // Venomous does work normal card
        #endregion

        // Damage Percentage Modifier Passive Tests
        #region
        [Test]
        public void Wrath_Does_Increase_Damage_In_Handle_Damage_Calculations()
        {
            // Arange
            CharacterEntityModel attacker;
            CharacterEntityModel target;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(WRATH_NAME).passiveName;
            int expectedTotal = 13;          

            // Act
            attacker = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            target = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);

            Card card = new Card();
            card.owner = attacker;
            card.cardEffects.Add(new CardEffect { baseDamageValue = 10, cardEffectType = CardEffectType.DamageTarget });
            CardEffect cardEffect = card.cardEffects[0];

            PassiveController.Instance.ModifyPassiveOnCharacterEntity(attacker.pManager, passiveName, 1);

            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, target, DamageType.Physical, false, cardEffect.baseDamageValue, card, cardEffect);

            // Assert
            Assert.AreEqual(expectedTotal, finalDamageValue);
        }
        [Test]
        public void Wrath_Does_Expire_On_Character_Activation_End()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(WRATH_NAME).passiveName;
            int stacks = 1;
            int expected = 0;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();
            CharacterEntityController.Instance.CharacterOnActivationEnd(model);

            // Assert
            Assert.AreEqual(expected, model.pManager.wrathStacks);
        }
        [Test]
        public void Weakened_Does_Increase_Damage_In_Handle_Damage_Calculations()
        {
            // Arange
            CharacterEntityModel attacker;
            CharacterEntityModel target;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(WEAKENED_NAME).passiveName;
            int expectedTotal = 7;

            // Act
            attacker = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            target = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);

            Card card = new Card();
            card.owner = attacker;
            card.cardEffects.Add(new CardEffect { baseDamageValue = 10, cardEffectType = CardEffectType.DamageTarget });
            CardEffect cardEffect = card.cardEffects[0];

            PassiveController.Instance.ModifyPassiveOnCharacterEntity(attacker.pManager, passiveName, 1);

            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, target, DamageType.Physical, false, cardEffect.baseDamageValue, card, cardEffect);

            // Assert
            Assert.AreEqual(expectedTotal, finalDamageValue);
        }
        [Test]
        public void Weakened_Does_Expire_On_Character_Activation_End()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(WEAKENED_NAME).passiveName;
            int stacks = 1;
            int expected = 0;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();
            CharacterEntityController.Instance.CharacterOnActivationEnd(model);

            // Assert
            Assert.AreEqual(expected, model.pManager.weakenedStacks);
        }
        [Test]
        public void Grit_Does_Decrease_Damage_In_Handle_Damage_Calculations()
        {
            // Arange
            CharacterEntityModel attacker;
            CharacterEntityModel target;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(GRIT_NAME).passiveName;
            int expectedTotal = 7;

            // Act
            attacker = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            target = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, defenderNode);

            Card card = new Card();
            card.owner = attacker;
            card.cardEffects.Add(new CardEffect { baseDamageValue = 10, cardEffectType = CardEffectType.DamageTarget });
            CardEffect cardEffect = card.cardEffects[0];

            PassiveController.Instance.ModifyPassiveOnCharacterEntity(target.pManager, passiveName, 1);

            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, target, DamageType.Physical, false, cardEffect.baseDamageValue, card, cardEffect);

            // Assert
            Assert.AreEqual(expectedTotal, finalDamageValue);
        }
        [Test]
        public void Grit_Does_Expire_On_Character_Activation_End()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(GRIT_NAME).passiveName;
            int stacks = 1;
            int expected = 0;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();
            CharacterEntityController.Instance.CharacterOnActivationEnd(model);

            // Assert
            Assert.AreEqual(expected, model.pManager.gritStacks);
        }
        [Test]
        public void Vulnerable_Does_Increase_Damage_In_Handle_Damage_Calculations()
        {
            // Arange
            CharacterEntityModel attacker;
            CharacterEntityModel target;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(VULNERABLE_NAME).passiveName;
            int expectedTotal = 13;

            // Act
            attacker = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            target = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, defenderNode);

            Card card = new Card();
            card.owner = attacker;
            card.cardEffects.Add(new CardEffect { baseDamageValue = 10, cardEffectType = CardEffectType.DamageTarget });
            CardEffect cardEffect = card.cardEffects[0];

            PassiveController.Instance.ModifyPassiveOnCharacterEntity(target.pManager, passiveName, 1);

            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, target, DamageType.Physical, false, cardEffect.baseDamageValue, card, cardEffect);

            // Assert
            Assert.AreEqual(expectedTotal, finalDamageValue);
        }
        [Test]
        public void Vulnerable_Does_Expire_On_Character_Activation_End()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(VULNERABLE_NAME).passiveName;
            int stacks = 1;
            int expected = 0;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();
            CharacterEntityController.Instance.CharacterOnActivationEnd(model);

            // Assert
            Assert.AreEqual(expected, model.pManager.vulnerableStacks);
        }
        #endregion

        // Taunted Tests
        #region
        // tests to do
        // taunt does expire on activation end
        // taunt does change enemy's current target
        // taunt does nothing if target is not using a targetable attack action
        // taunt is removed if character's taunter is killed
        [Test]
        public void Taunt_Does_Expire_On_Character_Activation_End()
        {
            // Arange
            CharacterEntityModel playerModel;
            CharacterEntityModel enemyModel;
            bool expected = false;

            // Act 
            playerModel = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            enemyModel = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, enemyNode);
            
            ActivationManager.Instance.OnNewCombatEventStarted();
            CharacterEntityController.Instance.HandleTaunt(playerModel, enemyModel);
            CharacterEntityController.Instance.CharacterOnActivationEnd(enemyModel);

            if (enemyModel.pManager.tauntStacks == 0 && 
                enemyModel.pManager.myTaunter == null)
            {
                expected = true;
            }

            // Assert
            Assert.IsTrue(expected);
        }
        [Test]
        public void Taunt_Does_Change_Characters_Target()
        {
            // Arange
            CharacterEntityModel playerModelOne;
            CharacterEntityModel playerModelTwo;
            CharacterEntityModel enemyModel;
            CharacterEntityModel expectedTarget;

            // Act 
            playerModelOne = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            playerModelTwo = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, LevelManager.Instance.GetNextAvailableDefenderNode());            
            enemyModel = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, enemyNode);
            enemyModel.initiative = 0;
            expectedTarget = playerModelTwo;

            // setup enemy action
            EnemyAction enemyAction = new EnemyAction();
            enemyAction.actionType = ActionType.AttackTarget;
            enemyModel.myNextAction = enemyAction;
            enemyModel.currentActionTarget = playerModelOne;

            ActivationManager.Instance.OnNewCombatEventStarted();
            CharacterEntityController.Instance.HandleTaunt(playerModelOne, enemyModel);
            CharacterEntityController.Instance.HandleTaunt(playerModelTwo, enemyModel);

            // Assert
            Assert.AreEqual(expectedTarget, enemyModel.pManager.myTaunter);
        }
        [Test]
        public void Taunt_Is_Removed_When_Original_Taunter_Dies()
        {
            // Arange
            CharacterEntityModel playerModelOne;
            CharacterEntityModel playerModelTwo;
            CharacterEntityModel enemyModel;
            CharacterEntityModel expectedTarget = null;

            // Act 
            playerModelOne = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            playerModelTwo = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, LevelManager.Instance.GetNextAvailableDefenderNode());
            enemyModel = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, enemyNode);
            enemyModel.initiative = 0;

            // setup enemy action
            EnemyAction enemyAction = new EnemyAction();
            enemyAction.actionType = ActionType.AttackTarget;
            enemyModel.myNextAction = enemyAction;
            enemyModel.currentActionTarget = playerModelOne;

            ActivationManager.Instance.OnNewCombatEventStarted();
            CharacterEntityController.Instance.HandleTaunt(playerModelOne, enemyModel);
            CombatLogic.Instance.HandleDamage(1000, null, playerModelOne, DamageType.Physical, null);

            // Assert
            Assert.AreEqual(expectedTarget, enemyModel.pManager.myTaunter);
        }
        [Test]
        public void Taunt_Removal_Auto_Sets_New_Target_When_Original_Taunter_Dies()
        {
            // Arange
            CharacterEntityModel playerModelOne;
            CharacterEntityModel playerModelTwo;
            CharacterEntityModel enemyModel;
            CharacterEntityModel expectedTarget;

            // Act 
            playerModelOne = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            playerModelTwo = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, LevelManager.Instance.GetNextAvailableDefenderNode());
            enemyModel = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, enemyNode);
            enemyModel.initiative = 0;
            expectedTarget = playerModelTwo;

            // Setup enemy action
            EnemyAction enemyAction = new EnemyAction();
            enemyAction.actionType = ActionType.AttackTarget;
            enemyModel.myNextAction = enemyAction;
            enemyModel.currentActionTarget = playerModelOne;

            ActivationManager.Instance.OnNewCombatEventStarted();
            CharacterEntityController.Instance.HandleTaunt(playerModelOne, enemyModel);
            CombatLogic.Instance.HandleDamage(1000, null, playerModelOne, DamageType.Physical, null);

            // Assert
            Assert.AreEqual(expectedTarget, enemyModel.currentActionTarget);
        }


        #endregion

        // DoT Tests
        #region
        [Test]
        public void Poisoned_Does_Deal_Damage_On_Character_Activation_End()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(POISONED_NAME).passiveName;
            int stacks = 10;
            int expected;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            expected = model.health - stacks;
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();
            CharacterEntityController.Instance.CharacterOnActivationEnd(model);

            // Assert
            Assert.AreEqual(expected, model.health);
        }
        [Test]
        public void Poisoned_Does_Kill_Character_If_Lethal_On_Character_Activation_End()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(POISONED_NAME).passiveName;
            int stacks = 1000;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();
            CharacterEntityController.Instance.CharacterOnActivationEnd(model);

            // Assert
            Assert.IsTrue(model.livingState == LivingState.Dead);
        }
        [Test]
        public void Poisoned_Damage_Is_Affected_By_Poison_Resistance()
        {
            // TO DO: write this test when damage type resistance logic is written
        }
        [Test]
        public void Burning_Does_Deal_Damage_On_Character_Activation_End()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(BURNING_NAME).passiveName;
            int stacks = 10;
            int expected;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            expected = model.health - stacks;
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();
            CharacterEntityController.Instance.CharacterOnActivationEnd(model);

            // Assert
            Assert.AreEqual(expected, model.health);
        }
        [Test]
        public void Burning_Does_Kill_Character_If_Lethal_On_Character_Activation_End()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(BURNING_NAME).passiveName;
            int stacks = 1000;

            // Act 
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();
            CharacterEntityController.Instance.CharacterOnActivationEnd(model);

            // Assert
            Assert.IsTrue(model.livingState == LivingState.Dead);
        }
        [Test]
        public void Burning_Damage_Is_Affected_By_Fire_Resistance()
        {
            // TO DO: write this test when damage type resistance logic is written
        }
        [Test]
        public void Overload_Does_Deal_Damage_To_Random_Enemy_On_Character_Activation_End()
        {
            // Arange
            CharacterEntityModel player;
            CharacterEntityModel enemy;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(OVERLOAD_NAME).passiveName;
            int stacks = 10;
            int expected = 20;

            // Act 
            player = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, defenderNode);
            enemy = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, enemyNode);
            player.initiative = 100;
            enemy.health = 30;
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(player.pManager, passiveName, stacks);

            ActivationManager.Instance.OnNewCombatEventStarted();
            CharacterEntityController.Instance.CharacterOnActivationEnd(ActivationManager.Instance.EntityActivated);

            // Assert
            Assert.AreEqual(expected, enemy.health);
        }
        #endregion


    }


}
