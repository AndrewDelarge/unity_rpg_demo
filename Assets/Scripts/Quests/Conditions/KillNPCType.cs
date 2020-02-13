using NPC;
using Player;
using Scriptable;
using UnityEditor;
using UnityEngine;

namespace Quests.Conditions
{
    [AddComponentMenu("Quest/Condition/KillNPCType")]
    public class KillNPCType : ConditionBase
    {
        public GameActor enemyType;
        public int howMuch = 1;
        
        private int currentKills = 0;
        private NPCActor[] _npcActors;
        
        
        public override void Init()
        {
//            PlayerManager.instance.player.onTargetDied += Completing;



            UpdateEnemyList();
            
            SetCounterInTitle();
        }


        private void UpdateEnemyList()
        {
            _npcActors = FindObjectsOfType<NPCActor>();

            
            for (int i = 0; i < _npcActors.Length; i++)
            {
                NPCActor npcActor = _npcActors[i];
                _npcActors[i].characterStats.onDied += x => Completing(npcActor);
            }
        }
        
        void Completing(NPCActor actor)
        {
            if (actor.actorScript != enemyType)
            {
                return;
            }
            currentKills++;
            SetCounterInTitle();
            
            if (onComplete != null)
            {
                onComplete.Invoke(this);
            }
            
            if (currentKills >= howMuch)
            {
                Completed();
            }
        }

        void SetCounterInTitle()
        {
            this.title = info.title + " " + currentKills + "/" + howMuch;
        }
        
//        [MenuItem("GameObject/Quest/Conditions/KillNPCType", false, 10)]
//        static void Create(MenuCommand menuCommand)
//        {
//            GameObject go = new GameObject("KillNPCType");
//            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
//            go.AddComponent<KillNPCType>();
//            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
//            Selection.activeObject = go;
//        }
    }

}