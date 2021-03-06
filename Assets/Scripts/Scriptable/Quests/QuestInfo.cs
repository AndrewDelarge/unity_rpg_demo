﻿using UnityEngine;

namespace Scriptable.Quests
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quests/Quest")]
    public class QuestInfo : ScriptableObject
    {
        public string title;
        public string description;
    }
}
