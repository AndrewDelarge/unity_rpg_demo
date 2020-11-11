using System;
using System.Collections.Generic;
using System.Linq;
using Scriptable;

namespace Gameplay.Actors.Base
{
    public class ActorFraction
    {
        private Scriptable.Fraction Fraction;
        private List<Fraction> _enemies;

        public ActorFraction(Fraction fraction)
        {
            Fraction = fraction;
            _enemies = fraction.enemies.ToList();
        }
        
        public bool FractionInEnemies(Fraction fraction)
        {
            if (fraction.id == -1)
            {
                throw new Exception("ID fraction " + fraction.title + " not set!");
            }

            if (fraction.unfrendly)
            {
                return true;
            }

            bool isEnemy = false;
            _enemies.ForEach(x =>
            {
                if (x.id == fraction.id)
                {
                    isEnemy = true;
                }
            });
            return isEnemy;
        }

        public void AddEnemy(Fraction fraction)
        {
            if (! FractionInEnemies(fraction))
            {
                _enemies.Add(fraction);
            }
        }
        
    }
}
