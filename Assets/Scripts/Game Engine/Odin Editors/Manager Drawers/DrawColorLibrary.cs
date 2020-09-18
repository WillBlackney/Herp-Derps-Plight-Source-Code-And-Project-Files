using Sirenix.OdinInspector;

namespace CustomOdinGUI
{
    public class DrawColorLibrary : DrawSceneObject<ColorLibrary>
    {
        [ShowIf("@myObject != null")]
        [GUIColor(0.7f, 1f, 0.7f)]
        [ButtonGroup("Top Button")]
        private void SomeFunction()
        {

        }
        protected override string MyTitle()
        {
            return "Color Library";
        }

        protected override string MySubTitle()
        {
            return "A place to store and organise all colors used in the project";
        }
    }
}
