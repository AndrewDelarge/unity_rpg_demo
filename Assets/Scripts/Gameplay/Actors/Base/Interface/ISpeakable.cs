using GameSystems.Languages;

namespace Gameplay.Actors.Base.Interface
{
    public interface ISpeakable
    {
        void Say(Text text, float time);

        void StopSay();
    }
    
    
    
}