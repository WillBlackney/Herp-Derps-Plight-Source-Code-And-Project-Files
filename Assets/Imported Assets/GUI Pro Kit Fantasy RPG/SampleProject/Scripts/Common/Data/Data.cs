using System.Linq;
using UnityEngine;


namespace FantasyRPG
{



    public class Data
    {


        public const string path_sound = "_Sounds/";

        //씬
        public const string scene_title = "0_Title";
        public const string scene_home = "1_Home";
        public const string scene_play = "2_Play";



        //Todo 임시 데이터 처리
        public static bool IsClearMap
        {
            get { return PlayerPrefs2.GetBool("IsClearMap"); }
            set { PlayerPrefs2.SetBool("IsClearMap", value); }
        }


        public static bool IsBgm
        {
            get { return PlayerPrefs2.GetBool("IsBgm", true); }
            set { PlayerPrefs2.SetBool("IsBgm", value); }
        }

        public static bool IsHelp
        {
            get { return PlayerPrefs2.GetBool("IsHelp", true); }
            set { PlayerPrefs2.SetBool("IsHelp", value); }
        }

        public static bool IsLte
        {
            get { return PlayerPrefs2.GetBool("IsLte", false); }
            set { PlayerPrefs2.SetBool("IsLte", value); }
        }

        public static bool IsMan
        {
            get { return PlayerPrefs2.GetBool("IsMan", true); }
            set { PlayerPrefs2.SetBool("IsMan", value); }
        }
    }
}
