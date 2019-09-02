
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class Loading : MonoBehaviour
    {
        [SerializeField]
        private RectTransform progressBar;
        private const float MAX_PROGRESS_SCALE = 1f;
        private AsyncOperation sceneLoadingOperation;
        private Image progressBarImage;

        private void Awake()
        {
            progressBarImage = progressBar.GetComponent<Image>();
        }

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
                progressBarImage.fillAmount = progress;
            }
        }
        
    }
}
