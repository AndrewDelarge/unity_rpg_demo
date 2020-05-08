using System;
using Actors.Base.StatsStuff;
using UnityEngine;

namespace Actors.Base.Interface
{
    public interface IHealthable
    {
        bool IsDead();

        void TakeDamage(Damage damage);

        void Heal(Heal heal);

        int GetHealth();

        Transform GetTransform();

        int GetMaxHealth();

        bool IsHasLevel();
        
        int GetLevel();

        event EventHandler<HealthChangeEventArgs> OnHealthChange;
    }
    
    public class HealthChangeEventArgs : EventArgs
    {
        public int healthChange = 0;
        public Actor initiator;
        public BaseHealthModifier modifier;
    }
}