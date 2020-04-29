using GameSystems;
using UnityEngine;

namespace UI.MainMenu
{
    [RequireComponent(typeof(Loading))]
    public class UI : MonoBehaviour
    {

        public int firstSceneIndex;
        public GameObject loading;

        private Loading _loading;
        
        private void Awake()
        {
            _loading = loading.GetComponent<Loading>();
            
            _loading.Hide();
        }

        public void OnButtonStart()
        {
            GameController.instance.StartScene(firstSceneIndex);
        }

        public void OnButtonExit()
        {
            Application.Quit();
        }
    }
}
