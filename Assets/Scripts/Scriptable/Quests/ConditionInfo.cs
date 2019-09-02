using UnityEngine;

namespace Scriptable.Quests
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quests/Condition")]
    public class ConditionInfo : ScriptableObject
    {
        public string title;
    }
}