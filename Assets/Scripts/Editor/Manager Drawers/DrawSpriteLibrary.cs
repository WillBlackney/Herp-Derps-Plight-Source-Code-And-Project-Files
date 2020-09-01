using Sirenix.OdinInspector;

namespace CustomOdinGUI
{
    public class DrawSpriteLibrary : DrawSceneObject<SpriteLibrary>
    {
        // This class specifically draws a editor window
        // for the manager class 'SpriteLibrary'

        [ShowIf("@myObject != null")]
        [GUIColor(0.7f, 1f, 0.7f)]
        [ButtonGroup("Top Button")]
        private void SomeFunction()
        {

        }
        protected override string MyTitle()
        {
            return "Sprite Library";
        }

        protected override string MySubTitle()
        {
            return "A place to store and organise all sprites used in the project";
        }
    }
}
