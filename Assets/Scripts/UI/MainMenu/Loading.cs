
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.MainMenu
{
    public class Loading : MonoBehaviour
    {
        private const float MAX_PROGRESS_SCALE = 1f;
        
        private AsyncOperation sceneLoadingOperation;
        
        [SerializeField]
        private RectTransform progressBar;


        private void Update()
        {
            if (gameObject.activeSelf)
            {
                if (sceneLoadingOperation.isDone)
                {
                    Hide();
                }
                
                SetProgress(sceneLoadingOperation.progress);
            }
        }

        public void ShowAndLoad(int scene)
        {
            gameObject.SetActive(true);
            sceneLoadingOperation = SceneManager.LoadSceneAsync(scene);
            SetProgress(0f);
//            sceneLoadingOperation.progress;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            sceneLoadingOperation = null;
        }


        void SetProgress(float progress)
        {
            if (progress <= MAX_PROGRESS_SCALE)
            {
                progressBar.transform.localScale = new Vector3(progress, 1, 1);
            }
        }
        
    }
}
