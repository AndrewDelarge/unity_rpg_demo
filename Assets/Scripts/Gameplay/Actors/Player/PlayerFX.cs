using System.Collections;
using Gameplay.Actors.Base;
using Gameplay.Actors.Base.Interface;
using GameSystems;
using GameSystems.FX;
using Managers;
using Managers.Player;
using Scriptable;
using UnityEngine;

namespace Gameplay.Actors.Player
{
    public class PlayerFX : ParticleSpawner
    {
        public GameObject healParticle;
        public GameObject hitParticle;
        public GameObject arrowPath;
        public float particleLifetime;
        
        
        protected ParticleSystem currentTrail;

        private Transform target;
        private Combat combat;
        private EquipmentManager equipmentManager;
        private Actor player;
        private IHealthable stats;
        private GameObject arrowPathObject;
        private LineRenderer arrowPathRenderer;
        private float trailTime;
        private bool inited = false;
        private bool updateAimPath = false;
        
        
        public void Init(Combat combat)
        {
            equipmentManager = PlayerManager.Instance().equipmentManager;
            player = PlayerManager.Instance().GetPlayer();
            stats = GetComponent<IHealthable>();
            this.combat = combat;
            
            RegisterEvents();
            
            target = transform;
            Transform targetRend = GetComponentInChildren<Transform>();
            if (targetRend != null)
            {
                target = targetRend.transform;
            }
            
            
            SpawnArrowPath();
            inited = true;
        }


        private void SpawnArrowPath()
        {
            Vector3 pos = Vector3.zero;
            pos.y = player.animator.lookPoint.localPosition.y;
            
            arrowPathObject = Instantiate(arrowPath, target);
            arrowPathObject.transform.localPosition = pos;
            arrowPathRenderer = arrowPathObject.GetComponent<LineRenderer>();
            
            SetVisibleArrowPath(false);
        }
        
        private void FixedUpdate()
        {
            if (! inited)
            {
                return;
            }

            if (updateAimPath)
            {
                UpdateAimPath();
            }
            
            
            arrowPathObject.transform.LookAt(player.animator.lookPoint);
            
        }

        private void UpdateAimPath()
        {
            RaycastHit hit;
            Vector3 pos = transform.position;
            Vector3 direction = transform.TransformDirection(player.animator.lookPoint.localPosition);
            pos.y = player.animator.lookPoint.position.y;
            direction.y = 0;
            if (Physics.Raycast(pos, direction, out hit,25f))
            {
                arrowPathRenderer.SetPosition(1, new Vector3(0, 0, Vector3.Distance(player.transform.position, hit.point)));
            }
            else
            {
                arrowPathRenderer.SetPosition(1, new Vector3(0, 0, 25));
            }
        }
        
        private void RegisterEvents()
        {
            equipmentManager.onMeleeWeaponEquip += SpawnTrail;
            equipmentManager.onMeleeWeaponUnequip += DestroyTrail;
            combat.OnAttack += () => StartCoroutine(ShowTrail());
            stats.OnHealthChange += ShowHealChange;
            combat.onAimStart += () => SetVisibleArrowPath(true);
            combat.onAimEnd += () => SetVisibleArrowPath(false);
            combat.onAimBreak += () => SetVisibleArrowPath(false);
        }

        void SetVisibleArrowPath(bool visible)
        {
            updateAimPath = visible;
            arrowPathObject.SetActive(visible);
        }
        
        
        void ShowHealChange(object healthable, HealthChangeEventArgs args)
        {
            if (args.healthChange > 0)
            {
                StartCoroutine(SpawnParticle(healParticle, transform, particleLifetime));
            }
            else if (args.healthChange < 0)
            {
                StartCoroutine(SpawnParticle(hitParticle, transform, particleLifetime));
            }
        }
        

        IEnumerator ShowTrail()
        {

            if (currentTrail != null)
            {
                currentTrail.Play();
                
                yield return new WaitForSeconds(combat.GetCurrentMeleeAttackDuration());
                
                currentTrail.Stop();
            }
        }


        void HideTrail()
        {
            if (currentTrail != null)
            {
                currentTrail.Stop();
            }
        }


        void SpawnTrail(MeleeWeapon weapon)
        {
            Transform weaponTransform = equipmentManager.GetMeleeWeaponTransform();
            GameObject trail = weapon.trail;
            
            if (trail == null)
            {
                return;
            }
            
            Vector3 pos = new Vector3(0.0025f, -0.01745f, -0.0013f);

            currentTrail = Instantiate(trail, weaponTransform, false).GetComponent<ParticleSystem>();
            if (currentTrail == null)
            {
                return;
            }
            currentTrail.Stop();
            currentTrail.transform.localPosition = weapon.trailSpawnLocalPos;
            currentTrail.transform.localScale = weapon.trailSpawnLocalScale;
            currentTrail.transform.localRotation = Quaternion.Euler(
                weapon.trailSpawnLocalRotation.x, weapon.trailSpawnLocalRotation.y, weapon.trailSpawnLocalRotation.z);
            
            trailTime = currentTrail.time;
        }

        void DestroyTrail(Weapon weapon)
        {
            if (currentTrail != null)
            {
                Destroy(currentTrail);
                currentTrail = null;
            }
        }
    }
}