using System;
using System.Collections.Generic;
using System.Linq;
using Scriptable;
using UnityEngine;

namespace Player
{
    public class CharacterFraction
    {
        private Scriptable.Fraction Fraction;
        List<Fraction> _enemies;

        public CharacterFraction(Fraction fraction)
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
