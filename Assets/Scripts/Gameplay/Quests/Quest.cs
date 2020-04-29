using GameSystems;
using Managers.Player;
using Scriptable;
using Scriptable.Quests;
using UI.Hud;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Quests
{
    public class Quest : MonoBehaviour
    {
        public QuestInfo info;
        public ConditionBase[] conditions;
        public Item[] rewards;

        public UnityEvent onStart;
        public UnityEvent onCompleted;


        private QuestPanel questPanel;
        private InventoryManager inventory;
        private bool isCompleted;
        private bool wasStarted;
        
        public void StartQuest()
        {
            if (wasStarted)
            {
                return;
            }

            GameController gc = GameController.instance;
            questPanel = gc.uiManager.GetQuestPanel();
            inventory = gc.playerManager.inventoryManager;
            // TODO QUest manager
//            if (! PlayerManager.instance.StartQuest(this))
//            {
//                Debug.Log("You already have unfinished quest");
//                return;
//            }
            
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
                RewardPlayer();
//                PlayerManager.instance.StopQuest(this);
                if (onCompleted != null)
                {
                    onCompleted.Invoke();
                }
            }
        }
        
        private void RewardPlayer()
        {
            Debug.Log("Now you've got reward");

            foreach (Item reward in rewards)
            {
                inventory.Add(reward);
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
            questPanel.SetQuest(this);
            Debug.Log("Quest " + info.title + " started!");
            UpdateStatusUI();
        }

        private void ShowCompletedUI()
        {
            questPanel.CompleteQuest();
            Debug.Log("Quest " + info.title + " completed!");
        }

        private void UpdateStatusUI()
        {
            questPanel.UpdateQuestInfo(this);
            
            foreach (ConditionBase condition in conditions)
            {
                Debug.Log(" - condition " + condition.GetTitle() + " is completed: " + condition.IsCompleted());
            }
        }

    }
}