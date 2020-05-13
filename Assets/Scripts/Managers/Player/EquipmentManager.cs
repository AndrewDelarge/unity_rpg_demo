using System.Collections.Generic;
using Scriptable;
using UnityEngine;

namespace Managers.Player
{
    public class EquipmentManager : MonoBehaviour
    {
        private Equipment[] currentEquipment;
        private Weapon currentWeapon;
        private Renderer[] currentMeshes;


        public ModelEquipmentPath[] modelEquipmentPaths;
        private Dictionary<EquipmentSlot, ModelEquipmentPath> modelPaths;
        public Equipment[] defaultEquipments;
        public Weapon defaultWeapon;
        public GameObject targetMesh;
        public delegate void OnItemUnequip(Equipment item);
        public delegate void OnItemEquip(Equipment item);
        public delegate void OnWeaponEquip(Weapon item);
        public delegate void OnWeaponUnequip(Weapon item);

        public OnItemUnequip onItemUnequip;
        public OnItemEquip onItemEquip;
        public OnWeaponEquip onWeaponEquip;
        public OnWeaponUnequip onWeaponUnequip;
        
        public void Init()
        {
            modelPaths = new Dictionary<EquipmentSlot, ModelEquipmentPath>();

            for (int i = 0; i < modelEquipmentPaths.Length; i++)
            {
                modelPaths.Add(modelEquipmentPaths[i].slot, modelEquipmentPaths[i]);
            }
            
            
            onItemUnequip = null;
            onItemEquip = null;
            onWeaponEquip = null;
            onWeaponUnequip = null;
            
            int equipCount = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
            currentEquipment = new Equipment[equipCount];
            currentMeshes = new Renderer[equipCount];
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
        
        public void Equip(Weapon newItem)
        {
            int itemIndex = (int) newItem.equipmentSlot;
            
            Unequip(itemIndex);
            
            currentWeapon = newItem;

            ShowEquipment(newItem);
            
            onItemEquip?.Invoke(newItem);
            onWeaponEquip?.Invoke(newItem);
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
        
        protected void ShowEquipmentDump(Weapon equipment)
        {
            int itemIndex = (int) equipment.equipmentSlot;

            Transform itemTransform = targetMesh.transform.Find("Model/Rig/Hips/Spine/Chest/Shoulder.R/Upper Arm.R/Lower Arm.R/Hand.R/HandHold.R");
            
            if (itemTransform == null)
            {
                Debug.Log("You cant equip " + equipment.name + "; Not set skin name or skin with this name not exist!");
                return;
            }
            
            GameObject mesh = Instantiate(equipment.weaponModel, itemTransform);
            
            Renderer newMesh = mesh.GetComponent<Renderer>();
            newMesh.enabled = true;
            currentMeshes[itemIndex] = newMesh;
        }
        
        
        public Transform GetEquipmentTransform(Equipment equipment)
        {
            string equipPath = "";

            if (modelPaths.ContainsKey(equipment.equipmentSlot))
            {
                equipPath = modelPaths[equipment.equipmentSlot].path;
            }
            
            equipPath += equipment.skinName;
//            switch (equipment.equipmentSlot)
//            {
//                // Henry/Armature/Root/Belly/Chest/UArm.R/LArm.R/HandHold.R/
//                // TODO harcoded eqipment path
//                case EquipmentSlot.Weapon:
//                    equipPath = "Henry/Armature/Root/Belly/Chest/UArm.R/LArm.R/HandHold.R/" + equipment.skinName;
//                    break;
//                case EquipmentSlot.Chest:
//                    equipPath = "Henry/Armature/Root/Belly/Chest/" + equipment.skinName;
//                    break;
//                case EquipmentSlot.Head:
//                    equipPath = "Henry/Armature/Root/Belly/Chest/Neck/Head/" + equipment.skinName;
//                    break;
//            }
            
            return targetMesh.transform.Find(equipPath);
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
                    onWeaponUnequip?.Invoke(currentWeapon);
                    onItemUnequip?.Invoke(currentWeapon);
                    currentWeapon = null;
                    break;
                default:
                    currentEquipment[slot] = null;
                    onItemUnequip?.Invoke(oldItem);
                    break;
            }
            
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
                onWeaponEquip?.Invoke(currentWeapon);
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

            if (slot == (int) EquipmentSlot.Weapon && currentWeapon != null)
            {
                return currentWeapon;
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
