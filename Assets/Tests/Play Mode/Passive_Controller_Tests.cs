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
        EnemyDataSO enemyData;
        List<CardDataSO> deckData;
        LevelNode node;

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

        // Core % Modifer Stats
        private const string WRATH_NAME = "Wrath";
        private const string WEAKENED_NAME = "Weakened";
        private const string GRIT_NAME = "Grit";
        private const string VULNERABLE_NAME = "Vulnerable";

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

            // Create mock enemy data
            enemyData = AssetDatabase.LoadAssetAtPath<EnemyDataSO>("Assets/SO Assets/Enemies/Test Enemy.asset");
        }

        // Core Stat + Temp Core Stat Tests
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

            // Core % Modifer Stats
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, WEAKENED_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, WRATH_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, VULNERABLE_NAME, stacks);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, GRIT_NAME, stacks);

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

                model.pManager.weakenedStacks == 1 &&
                model.pManager.wrathStacks == 1 &&
                model.pManager.vulnerableStacks == 1 &&
                model.pManager.gritStacks == 1 )
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
            expected = EntityLogic.GetTotalStamina(model);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();

            // Assert
            Assert.AreEqual(expected, EntityLogic.GetTotalStamina(model));
        }

        // Buff Passive Tests
        [Test]        
        public void Enrage_Triggers_Power_Gain_In_Handle_Damage_Method()
        {
            // Arange
            CharacterEntityModel model;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(ENRAGE_NAME).passiveName;
            int stacks = 2;
            int expectedTotal = 2;

            // Act
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();

            // Assert
            Assert.AreEqual(expected, model.block);
        }

        // Damage Perctange Modifier Passive Tests
        [Test]
        public void Wrath_Does_Increase_Damage_In_Handle_Damage_Calculations()
        {
            // Arange
            CharacterEntityModel attacker;
            CharacterEntityModel target;
            string passiveName = PassiveController.Instance.GetPassiveIconDataByName(WRATH_NAME).passiveName;
            int expectedTotal = 13;          

            // Act
            attacker = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
            target = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);

            Card card = new Card();
            card.owner = attacker;
            card.cardEffects.Add(new CardEffect { baseDamageValue = 10, cardEffectType = CardEffectType.DealDamage });
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
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
            attacker = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
            target = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);

            Card card = new Card();
            card.owner = attacker;
            card.cardEffects.Add(new CardEffect { baseDamageValue = 10, cardEffectType = CardEffectType.DealDamage });
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
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
            attacker = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
            target = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, node);

            Card card = new Card();
            card.owner = attacker;
            card.cardEffects.Add(new CardEffect { baseDamageValue = 10, cardEffectType = CardEffectType.DealDamage });
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
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
            attacker = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
            target = CharacterEntityController.Instance.CreateEnemyCharacter(enemyData, node);

            Card card = new Card();
            card.owner = attacker;
            card.cardEffects.Add(new CardEffect { baseDamageValue = 10, cardEffectType = CardEffectType.DealDamage });
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
            model = CharacterEntityController.Instance.CreatePlayerCharacter(characterData, node);
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(model.pManager, passiveName, stacks);
            ActivationManager.Instance.OnNewCombatEventStarted();
            CharacterEntityController.Instance.CharacterOnActivationEnd(model);

            // Assert
            Assert.AreEqual(expected, model.pManager.vulnerableStacks);
        }


        // for each passive % 
        // does expire on activation end
        // doesnt expire on activation end if still 1 or more stacks
        // does effect combat logic HandleDamage damage value
    }

   
}
