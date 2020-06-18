using GameSystems;
using UnityEngine;

namespace Helpers
{
    public class PositionHelper : MonoBehaviour
    {
        public static void ShowPosition(Vector3 position)
        {
            Instantiate(Resources.Load("System/Point") as GameObject, position, Quaternion.identity);
        }
    }
}