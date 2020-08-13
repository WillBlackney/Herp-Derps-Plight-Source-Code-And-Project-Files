using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;

namespace Engine
{
    public class TheGameManager : OdinMenuEditorWindow
    {
        [OnValueChanged("StateChange")]
        [LabelText("Manager View")]
        [LabelWidth(100f)]
        [EnumToggleButtons]
        [ShowInInspector]
        private ManagerState managerState;
        private int enumIndex = 0;
        private bool treeRebuild = false;

        // Create field for each type of scriptable object in project to be drawn
        private DrawSelected<EnemyDataSO> drawEnemies = new DrawSelected<EnemyDataSO>();
        private DrawSelected<CardDataSO> drawCards = new DrawSelected<CardDataSO>();
        private DrawSelected<EnemyWaveSO> drawEncounters = new DrawSelected<EnemyWaveSO>();

        // Create field for each type of manager object in project to be drawn
        private DrawTestSceneManager drawTestSceneManager = new DrawTestSceneManager();
        private DrawSpriteLibrary drawSpriteLibrary = new DrawSpriteLibrary();

        // Hard coded file directory paths to specific SO's
        private string enemyPath = "Assets/SO Assets/Enemies";
        private string cardPath = "Assets/SO Assets/Cards";
        private string encountersPath = "Assets/SO Assets/Enemy Encounters";

        [MenuItem("Tools/The Game Manager")]
        public static void OpenWindow()
        {
            GetWindow<TheGameManager>().Show();
        }
        private void StateChange()
        {
            // Listens to changes to the variable 'managerState'
            // via the event listener attribute 'OnPropertyChanged'.
            // clicking on an enum toggle button triggers this function, which
            // signals that the menu tree for that page needs to be rebuilt
            treeRebuild = true;
        }
        protected override void Initialize()
        {
            // Set SO directory folder paths
            drawEnemies.SetPath(enemyPath);
            drawCards.SetPath(cardPath);
            drawEncounters.SetPath(encountersPath);

            // Find manager objects
            drawTestSceneManager.FindMyObject();
            drawSpriteLibrary.FindMyObject();
        }
        protected override void OnGUI()
        {
            // Did we toggle to a new page? 
            // Should we rebuild the menu tree?
            if (treeRebuild && Event.current.type == EventType.Layout)
            {
                ForceMenuTreeRebuild();
                treeRebuild = false;
            }

            SirenixEditorGUI.Title("The Game Manager", "Heroes Of Herp Derp", TextAlignment.Center, true);
            EditorGUILayout.Space();

            switch (managerState)
            {
                case ManagerState.enemies:
                case ManagerState.combatEncounters:
                case ManagerState.items:
                case ManagerState.cards:
                    DrawEditor(enumIndex);
                    break;
                default:
                    break;
            }

            EditorGUILayout.Space();
            base.OnGUI();
        }
        protected override void DrawEditors()
        {
            // Which target should the window draw?
            // in cases where SO's need to be drawn, do SetSelected();
            // this takes the selected value from the menu tree, then
            // then draws it in the main window for editing

            switch (managerState)
            {
                case ManagerState.testTab:
                    DrawEditor(enumIndex);
                    break;

                case ManagerState.enemies:
                    drawEnemies.SetSelected(MenuTree.Selection.SelectedValue);
                    break;
              
                case ManagerState.items:
                    break;

                case ManagerState.cards:
                    drawCards.SetSelected(MenuTree.Selection.SelectedValue);
                    break;

                case ManagerState.combatEncounters:
                    drawEncounters.SetSelected(MenuTree.Selection.SelectedValue);
                    break;

                case ManagerState.spriteLibrary:
                    DrawEditor(enumIndex);
                    break;

            }

            // Which editor window should be drawn?
            // just cast the enum value as int to be used as the index
            DrawEditor((int)managerState);
        }
        protected override IEnumerable<object> GetTargets()
        {
            List<object> targets = new List<object>();

            // Targets must be added and drawn in the order
            // that the enum values are in!!
            // allows us to take advantage of the 
            // numerical value behind the enum values

            // Only draw for layouts that need to display scriptable objects
            // Otherwise, just add a null for managers
            targets.Add(drawTestSceneManager);
            targets.Add(drawEnemies);
            targets.Add(null);
            targets.Add(drawCards);
            targets.Add(drawEncounters);
            targets.Add(drawSpriteLibrary);
            targets.Add(base.GetTarget());            

            enumIndex = targets.Count - 1;

            return targets;
        }
        protected override void DrawMenu()
        {
            switch (managerState)
            {
                case ManagerState.enemies:
                case ManagerState.items:
                case ManagerState.cards:
                case ManagerState.combatEncounters:
                    base.DrawMenu();
                    break;
                default:
                    break;
            }
        }
        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree();

            switch (managerState)
            {
                case ManagerState.cards:
                    tree.AddAllAssetsAtPath("Card Data", cardPath, typeof(CardDataSO));
                    break;
                case ManagerState.enemies:
                    tree.AddAllAssetsAtPath("Enemy Data", enemyPath, typeof(EnemyDataSO));
                    break;
                case ManagerState.combatEncounters:
                    tree.AddAllAssetsAtPath("Combat Encounters", encountersPath, typeof(EnemyWaveSO));
                    break;
            }
            return tree;
        }
        public enum ManagerState
        {
            testTab,
            enemies,
            items,
            cards,
            combatEncounters,
            spriteLibrary,
        }


    }
}




