using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "New Sequence", menuName = "Scene/LevelSequence")]
    public class LevelsSequence : ScriptableObject
    {
        public GameObject[] levels;
    }
}