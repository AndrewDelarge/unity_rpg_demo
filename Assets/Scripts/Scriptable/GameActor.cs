using UnityEngine;


namespace Scriptable
{
    [CreateAssetMenu(fileName = "New Actor", menuName = "GameActors/Actor")]
    public class GameActor : ScriptableObject
    {
        public string title = "New Actor";
        public Fraction fraction;
    }
}
