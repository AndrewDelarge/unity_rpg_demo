namespace Gameplay.Actors.Base.StatsStuff
{
    public class Heal : BaseHealthModifier
    {
        private Actor owner;
        private int value;
        private bool isCrit;


        public Heal(int value, Actor owner = null, bool isCrit = false) : base(value, owner, isCrit)
        {
            this.owner = owner;
            this.value = value;
            this.isCrit = isCrit;
        }
    }
}