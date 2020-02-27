using Actors.Base;

namespace Actors.Combat
{
    public class Damage
    {
        private Actor owner;
        private int value;


        public Damage(int value, Actor owner = null)
        {
            this.owner = owner;
            this.value = value;
        }

        public int GetValue()
        {
            return value;
        }

        public Actor GetOwner()
        {
            return owner;
        }
        
    }
}