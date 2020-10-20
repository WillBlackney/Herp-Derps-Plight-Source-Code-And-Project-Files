using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace FantasyRPG
{

    public class PlayManager : Singleton<PlayManager>
    {
        public bool isLoadScene = false;
        private PanelLoading loading;


        private CtrBase _currentCtr;
        public CtrBase CurrentCtr
        {
            get { return _currentCtr; }
            set { _currentCtr = value; }
        }
        
        #region - SceneLoad

        /// 씬 불러오기
        public void LoadScene(string sceneName)
        {
            if (isLoadScene) return;
            isLoadScene = true;
            IsTouch = false;



            StartCoroutine(LoadSceneCo(sceneName));
        }

        IEnumerator LoadSceneCo(string sceneName)
        {
            if (!loading)
            {
                GameObject ob = (GameObject) Instantiate(Resources.Load("Panel_Loading"));
                ob.transform.SetParent(transform);
                loading = ob.GetComponent<PanelLoading>();
            }
            
            loading.gameObject.SetActive(true);
            loading.Show();

            yield return new WaitForSeconds(1.5f);

            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (!async.isDone)
            {
                if (async.progress < 0.9f)
                {
                    //로딩중
                    async.allowSceneActivation = false;
                }
                else
                {
                    //로딩완료
                    async.allowSceneActivation = true;
                }

                yield return null;
            }

            //씬 로딩 완료------------------------------------------------------
            loading.Hide();
            isLoadScene = false;
            IsTouch = true;
        }

        #endregion
        
        
        public bool IsTouch
        {
            set { GameObject.Find("EventSystem").SetActive(value); }
        }
    }
}
