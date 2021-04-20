using Managers;
using Scriptable;
using UI.Base;
using UI.Inventory;
using UI.Windows.ItemReceived;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class UIItemReceivedWindow : UIWindow
    {
        [SerializeField] private Inventory.ItemReceived itemReceived;
        
        [SerializeField] private Button substrate;
        
        private Item currentItem;

        public void Init()
        {
            itemReceived.Init();
            
            substrate.onClick.AddListener(() => UIManager.Instance().CloseWindow(UIManager.UIWindows.ItemReceived));
        }
        
        public override void Open()
        {
            itemReceived.SetItem(currentItem);
            
            base.Open();
            
            itemReceived.Show();
        }

        public override void Close()
        {
            currentItem = null;
            itemReceived.onHided += base.Close;

            itemReceived.Hide();
        }

        public override void SetConfig(WindowConfig config)
        {
            base.SetConfig(config);

            var windowConfig = (ItemReceivedWindowConfig) config;

            currentItem = windowConfig.Item;
        }
    }
}