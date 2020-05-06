using Actors.Base.Interface;
using UI.Hud;
using UI.MainMenu;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public GameObject UIPrefab;

        private bool isSpawned = false;

        private UI.Hud.UI uiHud;
        
        public void Spawn(Transform parent)
        {
            if (isSpawned)
            {
                return;
            }
            
            GameObject ui = Instantiate(UIPrefab, parent);

            uiHud = ui.GetComponent<UI.Hud.UI>();
            isSpawned = true;
            HideHud();
        }

        public void SetPlayer(GameObject player)
        {
            uiHud.Init();
            uiHud.healthBar.SetHealthable(player.GetComponent<IHealthable>());
        }

        public ActionBar GetActionBar()
        {
            return uiHud.actionBar;
        }
        
        public QuestPanel GetQuestPanel()
        {
            return uiHud.questPanel;
        }


        public Loading GetLoadScreen()
        {
            return uiHud.loadingScreen;
        }

        public void HideHud()
        {
            uiHud.HideHud();
        }
        
        public void ShowHud()
        {
            uiHud.ShowHud();
        }
    }
}