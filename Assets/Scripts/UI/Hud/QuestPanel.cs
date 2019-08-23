using Quests;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
    public class QuestPanel : MonoBehaviour
    {
        static Color DONE_COLOR = Color.green;
        static Color NOT_DONE_COLOR = Color.white;
        static float COMPLETE_TEXT_SHOWTIME = 2f;
        
        public Text questTitle;
        
        public Text completed;

        public Text[] conditions;

        private float completeTextShown;
        
        private bool isShowed;


        private void Update()
        {
            if (completed.gameObject.activeSelf)
            {
                if ((Time.time - completeTextShown) > COMPLETE_TEXT_SHOWTIME)
                {
                    completed.gameObject.SetActive(false);
                    Show();
                }
            }
        }

        public void Show()
        {
            questTitle.gameObject.SetActive(true);
            foreach (Text condition in conditions)
            {
                condition.gameObject.SetActive(true);
            }
        }

        public void Hide()
        {
            questTitle.gameObject.SetActive(false);
            foreach (Text condition in conditions)
            {
                condition.gameObject.SetActive(false);
            }
//            gameObject.SetActive(false);
        }

        public void Toggle()
        {
            
            if (questTitle.gameObject.activeSelf)
            {
                Hide();
                return;
            }
            
            Show();
        }

        public void SetQuest(Quest quest)
        {
            ClearQuest();
            UpdateQuestInfo(quest);
        }

        public void CompleteQuest()
        {
            ClearQuest();
            Hide();
            completed.gameObject.SetActive(true);
            completeTextShown = Time.time;
        }
        
        public void ClearQuest()
        {
            questTitle.text = "";

            for (int i = 0; i < conditions.Length; i++)
            {
                conditions[i].text = "";
            }
        }

        public void UpdateQuestInfo(Quest quest)
        {
            questTitle.text = quest.info.title;

            for (int i = 0; i < quest.conditions.Length; i++)
            {
                if (i >= conditions.Length)
                {
                    break;
                }
                conditions[i].text = quest.conditions[i].GetTitle();
                conditions[i].color = (quest.conditions[i].IsCompleted()) ? DONE_COLOR : NOT_DONE_COLOR;
            }
        }
        
        
    }
}