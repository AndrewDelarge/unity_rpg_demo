using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Dialog
{
    public class Balloon : WorldspaceUi
    {
        private Text text;
        
        public override void Init()
        {
            type = WorldUiType.Dialog;

            base.Init();

            text = curElement.GetComponentInChildren<Text>();
            Hide();
            
            curElement.SetActive(true);
        }

        public void SetText(GameSystems.Languages.Text textStr)
        {
            text.text = textStr.GetText();
        }
    }
}