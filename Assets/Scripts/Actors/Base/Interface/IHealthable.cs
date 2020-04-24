using System;
using Actors.Combat;
using UnityEngine;

namespace Actors.Base.Interface
{
    public interface IHealthable
    {
        bool IsDead();

        void TakeDamage(Damage damage);

        int GetHealth();

        Vector3 GetPosition();

        int GetMaxHealth();

        bool IsHasLevel();
        
        int GetLevel();

        event EventHandler<HealthChangeEventArgs> OnHealthChange;
    }
    
    public class HealthChangeEventArgs : EventArgs
    {
        public int healthChange = 0;
    }
}