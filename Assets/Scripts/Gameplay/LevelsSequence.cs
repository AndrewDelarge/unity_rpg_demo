using UnityEngine;

namespace Gameplay
{
    
    [CreateAssetMenu(fileName = "New LevelSequence", menuName = "Scene/LevelsSequence")]
    public class LevelsSequence : ScriptableObject
    {
        public GameObject[] levels;
    }
}