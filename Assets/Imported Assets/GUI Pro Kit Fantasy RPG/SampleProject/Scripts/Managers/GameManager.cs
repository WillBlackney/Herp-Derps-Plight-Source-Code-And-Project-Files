using UnityEngine;

namespace FantasyRPG
{

    public class GameManager : Singleton<GameManager>
    {
        private bool isSetFrameRate = false;
        private bool isSetScreenSize = false;



        public void SetInItializedSystem()
        {
            if (!isSetFrameRate) SetTargetFrameRate();
            if (isSetScreenSize) SetScreenSize();
        }

        public void SetTargetFrameRate(int framerate = 60)
        {
            isSetFrameRate = true;
            Application.targetFrameRate = framerate;
        }

        public void SetScreenSize(int width = 1920, int height = 1200)
        {
            isSetScreenSize = true;
            Screen.SetResolution(width, height, false);
        }

        public void SetCamera()
        {
            if (Camera.main != null)
            {
                Camera cam = Camera.main;
                cam.allowMSAA = false;
                cam.allowHDR = false;
            }
        }

    }
}
