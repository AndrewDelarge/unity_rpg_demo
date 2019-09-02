using Scriptable;
using UnityEngine;

namespace Player
{
    public class EquipmentManager : MonoBehaviour
    {
        #region Singleton
        public static EquipmentManager instance;

        private void Awake()
        {
            instance = this;
        }
        #endregion

        private Scriptable.Equipment[] currentEquipment;
        private Renderer[] currentMeshes;

        public Equipment[] defaultEquipments;
        public GameObject targetMesh;
        public delegate void OnItemUnequip(Scriptable.Equipment item);
        public delegate void OnItemEquip(Scriptable.Equipment item);

        public OnItemUnequip onItemUnequip;
        public OnItemEquip onItemEquip;
        
        private void Start()
        {
            int equipCount = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
            currentEquipment = new Scriptable.Equipment[equipCount];
            currentMeshes = new Renderer[equipCount];
            
            EquipDefault();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                UnequipAll();
            }
        }

        public void Equip(Scriptable.Equipment newItem)
        {
            int itemIndex = (int) newItem.equipmentSlot;

            Transform tmp;

            string equipPath = "";
            switch (newItem.equipmentSlot)
            {
                case EquipmentSlot.Weapon:
                    equipPath = "Armature/Root/Belly/Chest/UArm.R/LArm.R/HandHold.R/" + newItem.skinName;
                    break;
                case EquipmentSlot.Chest:
                    equipPath = "Armature/Root/Belly/Chest/" + newItem.skinName;
                    break;
                case EquipmentSlot.Head:
                    equipPath = "Armature/Root/Belly/Chest/Neck/Head/" + newItem.skinName;
                    break;
            }
            
            tmp = targetMesh.transform.Find(equipPath);

            if (tmp == null)
            {
                Debug.Log("You cant equip " + newItem.name + "; Not set skin name or skin with this name not exist!");
                return;
            }

            GameObject mesh = tmp.gameObject;
            
            Unequip(itemIndex);
            
            currentEquipment[itemIndex] = newItem;
            if (onItemEquip != null)
            {
                onItemEquip.Invoke(newItem);
            }

            Renderer newMesh = mesh.GetComponent<Renderer>();
            
            newMesh.enabled = true;
            currentMeshes[itemIndex] = newMesh;
        }

        public void Unequip(int slot)
        {
            if (IsSlotEquiped(slot))
            {
                if (currentMeshes[slot] != null)
                {

                    currentMeshes[slot].enabled = false;
                }
                
                Scriptable.Equipment oldItem = currentEquipment[slot];
                currentEquipment[slot] = null;
                if (onItemUnequip != null)
                {
                    onItemUnequip.Invoke(oldItem);
                }
            }
        }

        public void UnequipAll()
        {
            for (int i = 0; i < currentEquipment.Length; i++)
            {
                Unequip(i);
            }
        }

        void EquipDefault()
        {
            foreach (Equipment item in defaultEquipments)
            {
                Equip(item);
            }
        }
        
        public bool IsSlotEquiped(int slot)
        {
            if (currentEquipment[slot] != null)
            {
                return true;
            }

            return false;
        }
    }
   
    
}
