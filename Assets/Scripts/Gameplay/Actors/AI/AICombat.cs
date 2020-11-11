using Gameplay.Actors.Base;
using Gameplay.Actors.Base.StatsStuff;
using Gameplay.Projectile;
using GameSystems.Input;
using UnityEngine;

namespace Gameplay.Actors.AI
{
    public class AICombat : Base.Combat
    {
        public float rangeDamageMultiplier = 1f;
        public override void Init(Stats actorStats, BaseInput baseInput)
        {
            base.Init(actorStats, baseInput);

        }


        public override void RangeAttack(Vector3 point)
        {
            if (Time.time - lastRangeAttackTime < rangeAttackCooldown)
            {
                return;
            }
            EnterCombat();

            //TODO rework

            lastRangeAttackTime = Time.time;

            aimTime = Mathf.Min(aimTime, 1);
            Vector3 pos = transform.position;
            pos.y += .7f;
            GameObject gameObject = (GameObject) Instantiate(Resources.Load("Projectiles/ArrowE"), pos, Quaternion.identity);
            BaseProjectile projectile = gameObject.GetComponent<BaseProjectile>();
            Damage damage = stats.GetDamageValue(false, false, rangeDamageMultiplier);
            projectile.angleSpeed = 0;
            projectile.Launch(damage);
            gameObject.transform.LookAt(point);
            aimTime = 0;
            onAimEnd?.Invoke();
        }
    }
}