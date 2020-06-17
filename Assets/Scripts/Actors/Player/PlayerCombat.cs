using System.Collections;
using System.Collections.Generic;
using Actors.Base;
using Actors.Base.Interface;
using Actors.Base.StatsStuff;
using Gameplay.Projectile;
using Gameplay.Zones;
using GameSystems;
using GameSystems.Input;
using Managers.Player;
using Scriptable;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Actors.Player
{
    public class PlayerCombat : Base.Combat
    {
        public float minAimTime = 0.2f;
        [Header("Melee Weapons Combo")]
        public WeaponComboHitParams[] weaponsCombo;
        
        

        private Dictionary<WeaponType, WeaponComboHitParams> weaponComboHitParams;
        private WeaponComboHitParams currentWeaponComboHitParams;
        private EquipmentManager equipmentManager;


        public override void Init(Stats actorStats, BaseInput baseInput)
        {
            base.Init(actorStats, baseInput);

            equipmentManager = GameController.instance.playerManager.equipmentManager;
            
            
            weaponComboHitParams = new Dictionary<WeaponType, WeaponComboHitParams>();
            foreach (WeaponComboHitParams comboParams in weaponsCombo)
            {
                weaponComboHitParams.Add(comboParams.weaponType, comboParams);
            }

            currentWeaponComboHitParams = GetDefaultComboParams();
            equipmentManager.onMeleeWeaponEquip += SetComboHitByWeapon;
            equipmentManager.onMeleeWeaponUnequip += SetDefaultWeaponCombo;
        }

        public override void Aim()
        {
            if (equipmentManager.GetRangeWeapon() == null)
            {
                return;
            }
            if (aimTime == 0)
            {
                onAimStart?.Invoke();
            }
            aimTime += Time.deltaTime;
        }
        
        public override void RangeAttack(Vector3 point)
        {
            if (equipmentManager.GetRangeWeapon() == null)
            {
                return;
            }
            
            if (Time.time - lastRangeAttackTime < rangeAttackCooldown)
            {
                return;
            }

            if (aimTime < minAimTime)
            {
                onAimBreak?.Invoke();
                aimTime = 0;
                return;
            }
            lastRangeAttackTime = Time.time;
            aimTime = Mathf.Min(aimTime, 1);

            SpawnProjectile(equipmentManager.GetRangeWeapon().projectile, point);
            
            aimTime = 0;
            onAimEnd?.Invoke();
        }


        void SpawnProjectile(GameObject proj, Vector3 target)
        {
            Vector3 pos = transform.position;
            pos.y = target.y;
            GameObject gameObject = Instantiate(proj, pos, Quaternion.identity);
            gameObject.transform.LookAt(target);
            BaseProjectile projectile = gameObject.GetComponent<BaseProjectile>();
            projectile.angleSpeed = 1 - aimTime;
            projectile.ignorePlayer = true;
            projectile.Launch(stats.GetDamageValue());
        }
        
        
        public override void MeleeAttack(List<IHealthable> targetStats)
        {
            if (Time.time - lastAttackTime < curMAttackSpeed)
            {
                return;
            }
            curMAttackDelay = currentWeaponComboHitParams.GetDelay(successAttackInRow);
            curMAttackSpeed = currentWeaponComboHitParams.GetSpeed(successAttackInRow);
            curMAttackRadius = currentWeaponComboHitParams.GetRaduis(successAttackInRow);
            curMAttackDamageMultiplier = currentWeaponComboHitParams.GetFinalDamage(successAttackInRow);
            
            base.MeleeAttack(targetStats);

            if (IsLastCombatAttack())
            {
                StartCoroutine(SpawnPushBackWave(curMAttackDelay));
            }
            
        }

        IEnumerator SpawnPushBackWave(float waiting)
        {
            yield return new WaitForSeconds(waiting);
            
            PushBackZone.PushBackActors(transform.position, 2f, 1f);
        }

        private void SetComboHitByWeapon(Weapon weapon)
        {
            if (weaponComboHitParams.ContainsKey(weapon.type))
            {
                currentWeaponComboHitParams = weaponComboHitParams[weapon.type];

                successAttackInRow = 0;
                maxSuccessAttackInRow = currentWeaponComboHitParams.maxAttackInRow;
            }
        }

        void SetDefaultWeaponCombo(Weapon weapon)
        {
            currentWeaponComboHitParams = GetDefaultComboParams();
        }
        

        private WeaponComboHitParams GetDefaultComboParams()
        {
            WeaponComboHitParams param = new WeaponComboHitParams();
            param.maxAttackInRow = 1;
            param.delay = new float[1]{meleeAttackDelay};
            param.speed = new float[1]{meleeAttackSpeed};
            param.raduis = new float[1]{meleeAttackRaduis};
            param.finalDamageMultiplier = new float[1]{1f};
            
            return param;
        }

    }

    [System.Serializable]
    public struct WeaponComboHitParams
    {
        public WeaponType weaponType;
        public int maxAttackInRow;
        public float[] delay;
        public float[] speed;
        public float[] finalDamageMultiplier;
        public float[] raduis;

        public float GetDelay(int index)
        {
            return GetValue(delay, index);
        }
        
        public float GetSpeed(int index)
        {
            return GetValue(speed, index);
        }

        public float GetFinalDamage(int index)
        {
            return GetValue(finalDamageMultiplier, index);
        }
        
        public float GetRaduis(int index)
        {
            return GetValue(raduis, index);
        }

        private float GetValue(float[] array, int index)
        {
            if (index >= 0 && index < array.Length)
            {
                return array[index];
            }
            return array[array.Length - 1];
        }
    }
}