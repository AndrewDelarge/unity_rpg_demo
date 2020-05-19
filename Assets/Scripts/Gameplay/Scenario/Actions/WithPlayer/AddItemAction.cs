using Managers.Player;
using Scriptable;

namespace Gameplay.Scenario.Actions.WithPlayer
{
    public class AddItemAction : ScenarioAction
    {

        public Item item;
        
        
        public override void Do()
        {
            base.Do();
            InventoryManager.instance.Add(item, true);
            doing = true;
        }

        public override void CheckDoing()
        {
            return;
        }

        public override void Stop()
        {
            return;
        }


    }
}