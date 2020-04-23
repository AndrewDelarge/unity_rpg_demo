using System.Collections;
using Managers.Player;
using Scriptable;
using UnityEngine;

namespace Actors.Player
{
    public class PlayerFX : MonoBehaviour
    {
        protected TrailRenderer currentTrail;
        private float trailTime;
        private Base.Combat combat;
        
        private EquipmentManager equipmentManager;
        public void Init(Base.Combat combat)
        {
            equipmentManager = GameController.instance.playerManager.equipmentManager;
            this.combat = combat;
            equipmentManager.onWeaponEquip += SpawnTrail;
            equipmentManager.onWeaponUnequip += DestroyTrail;
            combat.OnAttack += () => StartCoroutine(ShowTrail());
        }


        IEnumerator ShowTrail()
        {
            if (currentTrail != null)
            {
                currentTrail.enabled = true;
                
                yield return new WaitForSeconds(combat.GetCurrentMeleeAttackSpeed());
                
                currentTrail.enabled = false;
            }
        }


        void HideTrail()
        {
            if (currentTrail != null)
            {
                currentTrail.enabled = false;
            }
        }


        void SpawnTrail(Weapon weapon)
        {
            Transform weaponTransform = equipmentManager.GetEquipmentTransform(weapon);
            GameObject trail = weapon.trail;
            
            if (trail == null)
            {
                return;
            }
            
            Vector3 pos = new Vector3(0.002f, -0.015f, 0.0036f);

            currentTrail = Instantiate(trail, weaponTransform).GetComponent<TrailRenderer>();
//            currentTrail.transform.position = pos;
            currentTrail.enabled = false;
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