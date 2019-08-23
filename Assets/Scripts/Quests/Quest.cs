using Player;
using Scriptable;
using Scriptable.Quests;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Quests
{
    public class Quest : MonoBehaviour
    {
        public QuestInfo info;
        public ConditionBase[] conditions;
        public Item[] rewards;

        public UnityEvent onStart;
        public UnityEvent onCompleted;

        private bool isCompleted;
        private bool wasStarted;
        
        public void StartQuest()
        {
            if (wasStarted)
            {
                return;
            }

            if (! PlayerManager.instance.StartQuest(this))
            {
                Debug.Log("You already have unfinished quest");
                return;
            }
            
            if (onStart != null)
            {
                onStart.Invoke();
            }

            foreach (ConditionBase condition in conditions)
            {
                condition.Init();
                condition.onComplete += OnConditionComplete;
            }

            ShowNewUI();
            wasStarted = true;
        }

        
        
        private void OnConditionComplete(ConditionBase conditionBase)
        {
            UpdateStatusUI();

            if (IsAllConditionsCompleted())
            {
                ShowCompletedUI();
                SendNudes();
                PlayerManager.instance.StopQuest(this);
                if (onCompleted != null)
                {
                    onCompleted.Invoke();
                }
            }
        }


        private void SendNudes()
        {
            Debug.Log("Now you've got reward");

            foreach (Item reward in rewards)
            {
                Inventory.instance.Add(reward);
            }
        }
        

        public bool IsAllConditionsCompleted()
        {
            foreach (ConditionBase condition in conditions)
            {
                if (!condition.IsCompleted())
                {
                    return false;
                }
            }

            return true;
        }


        private void ShowNewUI()
        {
            PlayerManager.instance.UI.questPanel.SetQuest(this);
            Debug.Log("Quest " + info.title + " started!");
            UpdateStatusUI();
        }

        private void ShowCompletedUI()
        {
            PlayerManager.instance.UI.questPanel.CompleteQuest();
            Debug.Log("Quest " + info.title + " completed!");
        }

        private void UpdateStatusUI()
        {
            PlayerManager.instance.UI.questPanel.UpdateQuestInfo(this);

            foreach (ConditionBase condition in conditions)
            {
                Debug.Log(" - condition " + condition.GetTitle() + " is completed: " + condition.IsCompleted());
            }
        }
        
        
//        [MenuItem("GameObject/Quest/NewQuest", false, 10)]
//        static void Create(MenuCommand menuCommand)
//        {
//            GameObject go = new GameObject("NewQuest");
//            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
//            go.AddComponent<Quest>();
//            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
//            Selection.activeObject = go;
//        }
        
        
    }
}