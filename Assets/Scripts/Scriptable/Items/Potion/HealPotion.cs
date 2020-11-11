using Gameplay.Actors.Base;
using Gameplay.Actors.Base.StatsStuff;
using UnityEngine;

namespace Scriptable.Items.Potion
{
    [CreateAssetMenu(fileName = "New Heal Potion", menuName = "Inventory/HealPotion")]
    public class HealPotion : Item
    {
        public int healPercents;
        
        
        public override void Use(Actor actor)
        {
            actor.stats.Heal(new Heal(healPercents, actor));
        }
    }
}