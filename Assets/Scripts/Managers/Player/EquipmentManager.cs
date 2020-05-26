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
        private MeleeWeapon currentWeapon;
        private GameObject currentWeaponGameObject;
        private RangeWeapon rangeWeapon;
        private GameObject currentRangeWeaponGameObject;
        private Renderer[] currentMeshes;


        public ModelEquipmentPath[] modelEquipmentPaths;
        private Dictionary<EquipmentSlot, ModelEquipmentPath> modelPaths;
        public Equipment[] defaultEquipments;
        public Weapon defaultWeapon;
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
            
            currentWeapon = newItem;

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
                return;
            }

            currentWeaponGameObject = curWeapon;
        }

        public void SetVisibleMelee(bool visible)
        {
            if (currentWeaponGameObject != null)
            {
                currentWeaponGameObject.SetActive(visible);
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
            onMeleeWeaponUnequip?.Invoke(currentWeapon);
            onItemUnequip?.Invoke(currentWeapon);
            currentWeapon = null;
            Destroy(currentWeaponGameObject);
            currentWeaponGameObject = null;
        }

        private void UneqipRange()
        {
            Debug.Log("ga");
            onItemUnequip?.Invoke(currentWeapon);
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

            if (currentWeapon != null)
            {
                onItemEquip?.Invoke(currentWeapon);
                onMeleeWeaponEquip?.Invoke(currentWeapon);
                ShowEquipment(currentWeapon);
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
        }
        
        public Equipment GetSlotEquipment(int slot)
        {
            if (currentEquipment[slot] != null)
            {
                return currentEquipment[slot];
            }
                
            
            // TODO rework !
            if (slot == (int) EquipmentSlot.Weapon && currentWeapon != null)
            {
                return currentWeapon;
            }
            
            if (slot == (int) EquipmentSlot.RangeWeapon && rangeWeapon != null)
            {
                return rangeWeapon;
            }
            
            return null;
        }

        public Weapon GetCurrentWeapon()
        {
            return currentWeapon;
        }
    }


    [System.Serializable]
    public struct ModelEquipmentPath
    {
        public EquipmentSlot slot;
        public string path;
    }
}
