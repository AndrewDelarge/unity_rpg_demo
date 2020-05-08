namespace Actors.Base.StatsStuff
{
    public abstract class BaseHealthModifier
    {
        private Actor owner;
        private int value;
        private bool isCrit;

        public BaseHealthModifier(int value, Actor owner = null, bool isCrit = false)
        {
            this.owner = owner;
            this.value = value;
            this.isCrit = isCrit;
        }

        public int GetValue()
        {
            return value;
        }

        public Actor GetOwner()
        {
            return owner;
        }

        public bool IsCrit()
        {
            return isCrit;
        }
    }
}