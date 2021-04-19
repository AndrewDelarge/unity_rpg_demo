using GameSystems;
using Managers;
using UI.Base;
using UI.MainMenu;
using UnityEngine;

namespace UI.Hud
{
    public class PauseMenu : UIWindow
    {

        public void OnResumeButtonClick()
        {
            UIManager.Instance().CloseWindow(UIManager.UIWindows.Pause);
        }
        
        public void OnExitButton()
        {
            GameManager.Instance().Quit();
        }

        public override void Open()
        {
            GameManager.Instance().Pause(true);

            base.Open();
        }

        public override void Close()
        {
            GameManager.Instance().Pause(false);

            base.Close();
        }
    }
}
