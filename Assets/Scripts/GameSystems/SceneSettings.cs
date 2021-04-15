using Gameplay;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystems
{
    public class SceneSettings : MonoBehaviour
    {
        
        public bool spawnPlayer = true;
        public int startLevel = 0;
        public LevelsSequence levels;
        public UnityEvent onStart;

        
        protected int currentScene;
        
        
        public void Apply()
        {
            onStart?.Invoke();
        }


        public GameObject GetLevel(int index)
        {
            return levels.levels[index];
        }
    }
}