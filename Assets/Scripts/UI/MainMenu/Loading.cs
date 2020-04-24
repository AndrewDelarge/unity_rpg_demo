
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class Loading : MonoBehaviour
    {
        public RectTransform progressBar;
        private const float MAX_PROGRESS_SCALE = 1f;
        private Image progressBarImage;

        private void Awake()
        {
            progressBarImage = progressBar.GetComponent<Image>();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            SetProgress(0f);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }


        public void SetProgress(float progress)
        {
            if (progress <= MAX_PROGRESS_SCALE)
            {
                progressBarImage.fillAmount = progress;
            }
        }
        
    }
}
