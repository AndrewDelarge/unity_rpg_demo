using Player;
using Scriptable;
using UnityEditor;
using UnityEngine;

namespace Quests.Conditions
{
    [AddComponentMenu("Quest/Condition/UseItem")]
    public class UseItem : ConditionBase
    {
        public Item item;
        public int times = 1;

        public int currentCount = 0;
        
        public override void Init()
        {
            Inventory.instance.onItemUse += Completing;
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
        
//        [MenuItem("GameObject/Quest/Conditions/UseItem", false, 10)]
//        static void Create(MenuCommand menuCommand)
//        {
//            GameObject go = new GameObject("UseItem");
//            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
//            go.AddComponent<UseItem>();
//            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
//            Selection.activeObject = go;
//        }
    }
}