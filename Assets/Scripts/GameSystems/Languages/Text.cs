using System;
using UnityEngine;

namespace GameSystems.Languages
{
    [Serializable]
    public struct Text : ITranslatable
    {
        public string textCode;
        [SerializeField] private TextType type;
        
        public string GetText()
        {
            // TODO text translating
            return textCode;
        }
    }
}