using System.Collections.Generic;
using Actors.Base;
using GameSystems;
using Scriptable;
using UnityEngine;

namespace Managers.Player
{
    public class EquipmentManager : MonoBehaviour
    {
        private Equipment[] currentEquipment;
        private Renderer[] currentMeshes;
        private GameObject currentWeaponGameObject;
        private GameObject currentRangeWeaponGameObject;
        private MeleeWeapon meleeWeapon;
        private RangeWeapon rangeWeapon;
        private Weapon activeWeapon;


        public ModelEquipmentPath[] modelEquipmentPaths;
        private Dictionary<EquipmentSlot, ModelEquipmentPath> modelPaths;
        public Equipment[] defaultEquipments;
        public MeleeWeapon defaultWeapon;
        public RangeWeapon defaultRangeWeapon;
        public GameObject targetMesh;
        
        
        public delegate void OnItemUnequip(Equipment item);
        public delegate void OnItemEquip(Equipment item);
        public delegate void OnMeleeWeaponEquip(MeleeWeapon item);
        public delegate void OnMeleeWeaponUnequip(Weapon item);

        public OnItemUnequip onItemUnequip;
        public OnItemEquip onItemEquip;
        public OnMeleeWeaponEquip onMeleeWeaponEquip;
        public OnMeleeWeaponUnequip onMeleeWeaponUnequip;
        
        public void Init()
        {
            InitModelPaths();
            
            UnregisterEvents();
            
            int equipCount = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
            currentEquipment = new Equipment[equipCount];
            currentMeshes = new Renderer[equipCount];
        }
        private void InitModelPaths()
        {
            modelPaths = new Dictionary<EquipmentSlot, ModelEquipmentPath>();

            for (int i = 0; i < modelEquipmentPaths.Length; i++)
            {
                modelPaths.Add(modelEquipmentPaths[i].slot, modelEquipmentPaths[i]);
            }
        }
        
        private void UnregisterEvents()
        {
            onItemUnequip = null;
            onItemEquip = null;
            onMeleeWeaponEquip = null;
            onMeleeWeaponUnequip = null;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                UnequipAll();
            }
        }

        public void Equip(Equipment newItem)
        {
            int itemIndex = (int) newItem.equipmentSlot;
            
            Unequip(itemIndex);
            
            currentEquipment[itemIndex] = newItem;

            ShowEquipment(newItem);
            
            onItemEquip?.Invoke(newItem);
        }
        
        public void Equip(MeleeWeapon newItem)
        {
            int itemIndex = (int) newItem.equipmentSlot;
            
            Unequip(itemIndex);
            
            meleeWeapon = newItem;

            ShowEquipment(newItem);
            
            onItemEquip?.Invoke(newItem);
            onMeleeWeaponEquip?.Invoke(newItem);
        }
        
        public void Equip(RangeWeapon newItem)
        {
            int itemIndex = (int) newItem.equipmentSlot;
            Unequip(itemIndex);
            rangeWeapon = newItem;
            ShowEquipment(newItem);
            onItemEquip?.Invoke(newItem);
        }

        protected void ShowEquipment(Equipment equipment)
        {
            int itemIndex = (int) equipment.equipmentSlot;

            Transform itemTransform = GetEquipmentTransform(equipment);
            
            if (itemTransform == null)
            {
                Debug.Log("You cant equip " + equipment.name + "; Not set skin name or skin with this name not exist!");
                return;
            }
            
            GameObject mesh = itemTransform.gameObject;

            Renderer newMesh = mesh.GetComponent<Renderer>();
            newMesh.enabled = true;
            currentMeshes[itemIndex] = newMesh;
        }
        
        protected void ShowEquipment(Weapon equipment)
        {
            Actor player = GameController.instance.playerManager.GetPlayer();
            
            Transform itemTransform = player.animator.handholdBoneRepeater;
            if (!equipment.rightHand)
            {
                itemTransform = player.animator.handholdBoneLeftRepeater;
            }
            
            if (itemTransform == null)
            {
                Debug.Log("You cant equip " + equipment.name + "; Not set skin name or skin with this name not exist!");
                return;
            }
            
            GameObject curWeapon = Instantiate(equipment.weaponModel, itemTransform);
            
            curWeapon.transform.localPosition = equipment.localPosition;
            curWeapon.transform.localRotation = Quaternion.Euler(
                    equipment.localRotation.x, equipment.localRotation.y, equipment.localRotation.z);
            
            
            if (equipment.equipmentSlot == EquipmentSlot.RangeWeapon)
            {
                currentRangeWeaponGameObject = curWeapon;
                SetVisibleRange(false);
                return;
            }
            
            currentWeaponGameObject = curWeapon;
            activeWeapon = equipment;
        }

        public Weapon GetActiveWeapon()
        {
            return activeWeapon;
        }
        
        public void SetMainWeaponActive(bool active)
        {
            SetVisibleRange(! active);
            SetVisibleMelee(active);
            
            activeWeapon = meleeWeapon;
            if (! active)
            {
                activeWeapon = rangeWeapon;
            }
        }
        
        public void SetVisibleMelee(bool visible)
        {
            if (currentWeaponGameObject != null)
            {
                Renderer renderer = currentWeaponGameObject.GetComponentInChildren<Renderer>();
                renderer.enabled = visible;
            }
        }
        
        
        public void SetVisibleRange(bool visible)
        {
            if (currentRangeWeaponGameObject != null)
            {
                currentRangeWeaponGameObject.SetActive(visible);
            }
        }
        // Not for weapons!
        public Transform GetEquipmentTransform(Equipment equipment)
        {
            string equipPath = "";

            if (modelPaths.ContainsKey(equipment.equipmentSlot))
            {
                equipPath = modelPaths[equipment.equipmentSlot].path;
            }
            
            equipPath += equipment.skinName;
            
            return targetMesh.transform.Find(equipPath);
        }

        public Transform GetMeleeWeaponTransform()
        {
            if (currentWeaponGameObject != null)
            {
                return currentWeaponGameObject.transform;
            }

            return null;
        }
        
        
        public void Unequip(int slot)
        {
            Equipment oldItem = GetSlotEquipment(slot);

            if (oldItem == null)
            {
                return;
            }

            if (currentMeshes[slot] != null)
            {
                currentMeshes[slot].enabled = false;
            }

            switch (oldItem.equipmentSlot)
            {
                case EquipmentSlot.Weapon:
                    UneqipMelee();
                    break;
                case EquipmentSlot.RangeWeapon:
                    UneqipRange();
                    break;
                default:
                    currentEquipment[slot] = null;
                    onItemUnequip?.Invoke(oldItem);
                    break;
            }
            
        }

        private void UneqipMelee()
        {
            onMeleeWeaponUnequip?.Invoke(meleeWeapon);
            onItemUnequip?.Invoke(meleeWeapon);
            meleeWeapon = null;
            Destroy(currentWeaponGameObject);
            currentWeaponGameObject = null;
        }

        private void UneqipRange()
        {
            Debug.Log("ga");
            onItemUnequip?.Invoke(meleeWeapon);
            rangeWeapon = null;
            Destroy(currentRangeWeaponGameObject);
            currentRangeWeaponGameObject = null;
        }
        
        public void UnequipAll()
        {
            for (int i = 0; i < currentEquipment.Length; i++)
            {
                Unequip(i);
            }
        }

        public void Reequip()
        {
            for (int i = 0; i < currentEquipment.Length; i++)
            {
                if (currentEquipment[i] != null)
                {
                    onItemEquip?.Invoke(currentEquipment[i]);
                    ShowEquipment(currentEquipment[i]);
                }
            }

            if (meleeWeapon != null)
            {
                onItemEquip?.Invoke(meleeWeapon);
                onMeleeWeaponEquip?.Invoke(meleeWeapon);
                ShowEquipment(meleeWeapon);
            }
            
            
            if (rangeWeapon != null)
            {
                onItemEquip?.Invoke(rangeWeapon);
                ShowEquipment(rangeWeapon);
            }
        }
        
        public void EquipDefault()
        {
            foreach (Equipment item in defaultEquipments)
            {
                Equip(item);
            }

            if (defaultWeapon != null)
            {
                Equip(defaultWeapon);
            }
            
            if (defaultRangeWeapon != null)
            {
                Equip(defaultRangeWeapon);
            }
        }
        
        public Equipment GetSlotEquipment(int slot)
        {
            if (currentEquipment[slot] != null)
            {
                return currentEquipment[slot];
            }
                
            
            // TODO rework !
            if (slot == (int) EquipmentSlot.Weapon && meleeWeapon != null)
            {
                return meleeWeapon;
            }
            
            if (slot == (int) EquipmentSlot.RangeWeapon && rangeWeapon != null)
            {
                return rangeWeapon;
            }
            
            return null;
        }

        public MeleeWeapon GetMeleeWeapon()
        {
            return meleeWeapon;
        }
        
        public RangeWeapon GetRangeWeapon()
        {
            return rangeWeapon;
        }
    }


    [System.Serializable]
    public struct ModelEquipmentPath
    {
        public EquipmentSlot slot;
        public string path;
    }
}
