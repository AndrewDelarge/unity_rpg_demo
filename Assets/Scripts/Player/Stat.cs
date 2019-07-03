using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class Stat
    {
        
        [SerializeField]
        private int value = 0;

        
        private List<int> modifiers = new List<int>();
        public int GetValue()
        {
            int finalValue = value;
            modifiers.ForEach(x => finalValue += x);
            return finalValue;
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
