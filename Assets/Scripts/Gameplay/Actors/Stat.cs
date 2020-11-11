using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Actors
{
    [System.Serializable]
    public class Stat
    {
        [SerializeField]
        private int value = 0;
        private List<int> modifiers = new List<int>();
        public Action onChange;
        
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
            
            onChange?.Invoke();
        }

        public void RemoveModifier(int modifier)
        {
            if (modifier != 0)
            {
                modifiers.Remove(modifier);
            }
            
            onChange?.Invoke();
        }
        
    }
}
