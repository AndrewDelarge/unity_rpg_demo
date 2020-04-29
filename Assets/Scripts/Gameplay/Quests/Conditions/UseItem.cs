using GameSystems;
using Scriptable;
using UnityEngine;

namespace Gameplay.Quests.Conditions
{
    [AddComponentMenu("Quest/Condition/UseItem")]
    public class UseItem : ConditionBase
    {
        public Item item;
        public int times = 1;
        public int currentCount = 0;
        
        public override void Init()
        {
            GameController.instance.playerManager.inventoryManager.onItemUse += Completing;
        }

        void Completing(Item usedItem)
        {
            if (usedItem != item)
            {
                return;
            }

            currentCount++;
            SetCounterInTitle();
            
            if (currentCount >= times)
            {
                Completed();
            }
        }
        
        void SetCounterInTitle()
        {
            this.title = info.title + " " + currentCount + "/" + times;
        }
        

    }
}