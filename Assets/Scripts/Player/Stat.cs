using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class Stat
    {
        
        [SerializeField]
        private int value;

        
        private List<int> modifiers = new List<int>();
        public int GetValue()
        {
            return value;
        }


        public void AddModifier(int modifier)
        {
            if (modifier != 0)
            {
                modifiers.Add(modifier);
            }
        }

        public void RemoveModifier(int modifier)
        {
            if (modifier != 0)
            {
                modifiers.Remove(modifier);
            }
        }
        
    }
}
