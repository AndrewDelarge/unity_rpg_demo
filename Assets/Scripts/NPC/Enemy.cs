namespace NPC
{
    public class Enemy : NPCActor
    {
        public override void Interact()
        {
            base.Interact();
            if (interactInitedActor != null)
            {
                if (! characterStats.IsDead())
                {
                    interactInitedActor.Attack(this);
                }
                else
                {
                    Loot();
                }
            }
        }
    }
}
