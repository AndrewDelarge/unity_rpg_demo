using GameSystems.Languages;

namespace Actors.Base.Interface
{
    public interface ISpeakable
    {
        void Say(Text text, float time);

        void StopSay();
    }
    
    
    
}