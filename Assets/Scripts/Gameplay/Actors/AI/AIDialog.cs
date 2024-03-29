using System.Collections;
using Gameplay.Actors.Base.Interface;
using GameSystems.Languages;
using UI.Dialog;
using UnityEngine;

namespace Gameplay.Actors.AI
{
    public class AIDialog : Balloon, ISpeakable
    {
        private Text curText;
        private float curTime;


        public void Say(Text text, float time)
        {
            curText = text;
            curTime = time;
            StartCoroutine(Sayng(text, time));
        }

        private IEnumerator Sayng(Text text, float time)
        {
            SetText(text);
            Show();
            yield return new WaitForSeconds(time);
            Hide();
        }

        public void StopSay()
        {
            StopCoroutine(Sayng(curText, curTime));
            Hide();
        }
        
        
        
    }
}