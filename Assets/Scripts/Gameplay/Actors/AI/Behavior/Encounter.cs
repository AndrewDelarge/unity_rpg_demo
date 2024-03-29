﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

namespace Gameplay.Actors.AI.Behavior
{
    /**
     *  TODO Need rework
     * 
     */
    public class Encounter : MonoBehaviour
    {
        public Wave[] waves;
        public GameObject[] spawnPoints;

        public bool nextWaveInTime;
        public int timeBetweenWaves = 10;
        public UnityEvent onEncounterEnds;

        protected List<AIActor> currentWaveMobs = new List<AIActor>();
        protected int currentWave;
        protected float lastWaveSpawn;
        protected bool active;

        public void Enable()
        {
            enabled = true;
        }
        
        public void Launch()
        {
            if (active || ! enabled)
            {
                return;
            }
            
            this.currentWave = 0;
            active = true;
            SpawnWave();
        }

        void SpawnWave()
        {
            if (currentWave >= waves.Length)
            {
                Debug.Log("too much waves!");
                return;
            }
            
            Debug.Log(currentWave + " wave going to spawn!");

            Random random = new Random();
            Wave wave = waves[currentWave];
            
            foreach (GameObject mob in wave.mobs)
            {
                int spawnPointIndex = random.Next(0, spawnPoints.Length);
                
                AIActor npc = Instantiate(mob, spawnPoints[spawnPointIndex].transform.position, new Quaternion()).GetComponent<AIActor>();
                npc.stats.onDied += RemoveFromCurrentWave;
//                npc.SetTarget(PlayerManager.instance.player);
                currentWaveMobs.Add(npc);
            }

            lastWaveSpawn = Time.time;
        }

        void SpawnNextWave()
        {
            currentWave++;
            SpawnWave();
        }

        private void FixedUpdate()
        {
            if (! active)
            {
                return;
            }
            
            if (currentWave > waves.Length)
            {
                active = false;
                enabled = false;
                EndEncounter();
                return;
            }

            if (nextWaveInTime)
            {
                if (Time.time >= timeBetweenWaves + lastWaveSpawn)
                {
                    SpawnNextWave();
                    return;
                }
            }
            
            if (currentWaveMobs.Count == 0)
            {
                SpawnNextWave();
            }
        }

        void RemoveFromCurrentWave(GameObject mob)
        {
            AIActor npc = mob.GetComponent<AIActor>();
            if (npc != null)
            {
                currentWaveMobs.Remove(npc);
            }
        }

        void EndEncounter()
        {
            if (onEncounterEnds != null)
            {
                onEncounterEnds.Invoke();
            }
        }
    }

    [Serializable]
    public struct Wave
    {
        public GameObject[] mobs;
    }
}
