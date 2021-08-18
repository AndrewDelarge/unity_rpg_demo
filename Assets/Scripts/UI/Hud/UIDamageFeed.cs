using System.Collections.Generic;
using Gameplay.Actors.Base;
using Gameplay.Actors.Base.Interface;
using GameSystems;
using Managers;
using Managers.Scenes;
using UnityEngine;

namespace UI.Hud
{
    public class UIDamageFeed : MonoBehaviour
    {
        private const int POOL_SIZE = 10;
        
        private List<FloatingHealthChange> feedStack = new List<FloatingHealthChange>();

        private List<FloatingHealthChange> floatingTextPool;
        
        [SerializeField]
        private FloatingHealthChange feedTemplate;
        
        public void Init()
        {
            floatingTextPool = new List<FloatingHealthChange>(POOL_SIZE);

            for (int i = 0; i < POOL_SIZE; i++)
            {
                var floatHealth = Instantiate(feedTemplate, transform);
                floatHealth.gameObject.SetActive(false);
                floatingTextPool.Add(floatHealth);
            }
            
            
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            var levelController = GameManager.Instance().sceneController.LevelController; 
            
//            levelController.OnLevelUnload += Clear;
            levelController.OnLevelLoaded += UpdateTrackingActors;
        }

        public void UpdateTrackingActors()
        {
            var actorsManager = GameManager.Instance().sceneController.GetActorsManager();

            for (int i = 0; i < actorsManager.AliveActors.Count; i++)
            {
                actorsManager.AliveActors[i].stats.OnHealthChange += ShowDamageText;
            }
        }
        
        void ShowDamageText(object healthable, HealthChangeEventArgs args)
        {
            Actor owner = args.modifier.GetOwner();
            
            if (owner == null || ! owner.IsPlayer())
                return;
            
            Add(args.actor.transform, args.modifier.GetValue().ToString(), args.modifier.IsCrit());
        }

        
        private void Add(Transform target, string value, bool isCrit)
        {
            for (int i = 0; i < POOL_SIZE; i++)
            {
                if (floatingTextPool[i].gameObject.activeSelf)
                {
                    continue;
                }

                var healthChange = floatingTextPool[i];
//                healthChange.onHided += () => Remove(healthChange);
                healthChange.Init(target, value, isCrit);
            
                feedStack.Add(healthChange);
                break;
            }
            
            
        }
//        private void Add(Transform target, string value, bool isCrit)
//        {
//            GameObject damageText = Instantiate(feedTemplate, transform);
//
//            FloatingHealthChange healthChange = damageText.GetComponent<FloatingHealthChange>();
//
//            if (healthChange == null)
//                return;
//            
//            healthChange.onHided += () => Remove(healthChange);
//            healthChange.Init(target, value, isCrit);
//            
//            feedStack.Add(healthChange);
//        }

        private void Clear()
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Transform damFeed = gameObject.transform.GetChild(i);
                Destroy(damFeed.gameObject);
            }
            
            feedStack.Clear();
        }

        private void Remove(FloatingHealthChange item)
        {
            feedStack.Remove(item);
            Destroy(item.gameObject);
        }
        
    }
}