using Sirenix.OdinInspector;

namespace CustomOdinGUI
{
    public class DrawPrefabHolder : DrawSceneObject<PrefabHolder>
    {
        // This class specifically draws a editor window
        // for the manager class 'CombatTestSceneController'

        [ShowIf("@myObject != null")]
        [GUIColor(0.7f, 1f, 0.7f)]
        [ButtonGroup("Top Button")]
        private void SomeFunction()
        {

        }

        protected override string MyTitle()
        {
            return "Prefab Holder";
        }
    }
}
