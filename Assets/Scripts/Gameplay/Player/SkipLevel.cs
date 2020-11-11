using GameSystems;
using UnityEngine;

namespace Gameplay.Player
{
    public class SkipLevel : MonoBehaviour
    {
        public void Skip()
        {
            GameController.instance.sceneController.NextLevel();
        }
    }
}