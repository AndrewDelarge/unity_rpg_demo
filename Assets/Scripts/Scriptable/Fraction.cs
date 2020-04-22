using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "New Fraction", menuName = "GameActors/Fraction")]
    public class Fraction : ScriptableObject
    {
        public int id = -1;
        public string title;
        public bool unfrendly;
        public List<Fraction> enemies = new List<Fraction>();

        public bool FractionInEnemies(Fraction fraction)
        {
            if (fraction.id == -1)
            {
                throw new Exception("ID fraction " + fraction.title + " not set!");
            }

            if (unfrendly)
            {
                return true;
            }

            bool isEnemy = false;
            enemies.ForEach(x =>
            {
                if (x.id == fraction.id)
                {
                    isEnemy = true;
                }
            });
            return isEnemy;
        }
        
        public void Awake()
        {
        }
        public void AddEnemy(Fraction fraction)
        {
            if (! FractionInEnemies(fraction))
            {
                enemies.Add(fraction);
            }
        }

    }
}
