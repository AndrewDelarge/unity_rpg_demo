using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Dialog
{
    public class Balloon : WorldspaceUi
    {
        private Text text;
        
        protected override void Start()
        {
            type = WorldUiType.Dialog;

            base.Start();

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