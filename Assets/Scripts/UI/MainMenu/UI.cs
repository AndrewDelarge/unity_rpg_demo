using GameSystems;
using Managers;
using UnityEngine;

namespace UI.MainMenu
{
    
    // TODO: Перенести в главный UI ?
    [RequireComponent(typeof(Loading))]
    public class UI : MonoBehaviour
    {

        public GameObject loading;

        private Loading _loading;
        
        private void Awake()
        {
            _loading = loading.GetComponent<Loading>();
            
            _loading.Hide();
        }

        public void OnButtonStart()
        {
            //TODO: Переделать загрузку сцены
            GameController.instance.StartScene((int) SceneController.BaseScenes.Indexes.GAME);
        }

        public void OnButtonExit()
        {
            Application.Quit();
        }
    }
}
