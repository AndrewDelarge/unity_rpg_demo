using Gameplay.Quests;
using Gameplay.Quests.Conditions;
using UnityEditor;
using UnityEngine;

namespace Editor.Quests
{
    [InitializeOnLoad]
    public static class Quests
    {
        [MenuItem("GameObject/Quest/NewQuest", false, 10)]
        static void CreateQuest(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("NewQuest");
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            go.AddComponent<Quest>();
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
        
        [MenuItem("GameObject/Quest/Conditions/KillNPC", false, 10)]
        static void CreateCondKillNpc(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("KillNPC");
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            go.AddComponent<KillNPC>();
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
        
        [MenuItem("GameObject/Quest/Conditions/KillNPCType", false, 10)]
        static void CreateCondKillNpcType(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("KillNPCType");
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            go.AddComponent<KillNPCType>();
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
        
        
        [MenuItem("GameObject/Quest/Conditions/StepInTrigger", false, 10)]
        static void CreateCondStepInTrig(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("StepInTrigger");
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            go.AddComponent<StepInTrigger>();
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
        
        
        [MenuItem("GameObject/Quest/Conditions/UseItem", false, 10)]
        static void CreateCondUseItem(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("UseItem");
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            go.AddComponent<UseItem>();
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }
}