using NPC;
using UnityEditor;
using UnityEngine;

namespace Quests.Conditions
{
    [AddComponentMenu("Quest/Condition/KillNPC")]
    public class KillNPC : ConditionBase
    {
        public NPCActor[] targets;

        private int currentKills = 0;
        
        public override void Init()
        {
            foreach (NPCActor target in targets)
            {
                target.characterStats.onDied += Completing;
            }

            SetCounterInTitle();
        }

        void Completing(GameObject gameObject)
        {
            currentKills++;
            SetCounterInTitle();
            
            if (onComplete != null)
            {
                onComplete.Invoke(this);
            }
            if (currentKills >= targets.Length)
            {
                Completed();
                return;
            }
        }


        void SetCounterInTitle()
        {
            this.title = info.title + " " + currentKills + "/" + targets.Length;
        }
        
//        [MenuItem("GameObject/Quest/Conditions/KillNPC", false, 10)]
//        static void Create(MenuCommand menuCommand)
//        {
//            GameObject go = new GameObject("KillNPC");
//            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
//            go.AddComponent<KillNPC>();
//            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
//            Selection.activeObject = go;
//        }
    }
}