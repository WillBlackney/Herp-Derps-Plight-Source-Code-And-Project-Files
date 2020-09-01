using Sirenix.OdinInspector;

namespace CustomOdinGUI
{
    public class DrawTestSceneManager : DrawSceneObject<CombatTestSceneController>
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
            return "Combat Test Scene Controller";
        }
    }
}
