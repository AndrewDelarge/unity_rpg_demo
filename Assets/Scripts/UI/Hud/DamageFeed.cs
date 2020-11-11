using System.Collections.Generic;
using GameSystems;
using Managers.Scenes;
using UnityEngine;

namespace UI.Hud
{
    public class DamageFeed : MonoBehaviour
    {
        private List<FloatingHealthChange> feedStack;
        private GameObject feedTemplate;


        public void Init(GameObject tempalte)
        {
            GameController.instance.sceneController.LevelController.OnLevelUnload += Clear;

            feedStack = new List<FloatingHealthChange>();
            feedTemplate = tempalte;
        }


        public void Add(Transform target, string value, bool isCrit)
        {
            GameObject damageText = Instantiate(feedTemplate, transform);

            FloatingHealthChange healthChange = damageText.GetComponent<FloatingHealthChange>();

            if (healthChange == null)
            {
                return;
            }
            healthChange.onHided += () => Remove(healthChange);
            healthChange.Init(target, value, isCrit);
            feedStack.Add(healthChange);
        }


        public void Clear()
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Transform damFeed = gameObject.transform.GetChild(i);
                Destroy(damFeed.gameObject);
            }
            feedStack.Clear();
        }

        public void Remove(FloatingHealthChange item)
        {
            feedStack.Remove(item);
            Destroy(item.gameObject);
        }
        
    }
}