using Scriptable;
using UI.Base;

namespace UI.Windows.ItemReceived
{
    public class ItemReceivedWindowConfig : WindowConfig
    {
        private Item item;

        public Item Item => item;

        public ItemReceivedWindowConfig(Item itemToShow)
        {
            item = itemToShow;
        }
    }
}