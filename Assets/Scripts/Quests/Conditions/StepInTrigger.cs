using Player;
using UnityEditor;
using UnityEngine;

namespace Quests.Conditions
{
    [AddComponentMenu("Quest/Condition/StepInTriggerClass")]
    public class StepInTrigger : ConditionBase
    {
        public Trigger targetTrigger;

        public override void Init()
        {
            targetTrigger.OnEnter.AddListener(Completed);
        }
        
//        [MenuItem("GameObject/Quest/Conditions/StepInTrigger", false, 10)]
//        static void Create(MenuCommand menuCommand)
//        {
//            GameObject go = new GameObject("StepInTrigger");
//            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
//            go.AddComponent<StepInTrigger>();
//            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
//            Selection.activeObject = go;
//        }
    }
}