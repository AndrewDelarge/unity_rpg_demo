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
        private SkinnedMeshRenderer[] currentMeshes;

        public Equipment[] defaultEquipments;
        public SkinnedMeshRenderer targetMesh;
        
        public delegate void OnItemUnequip(Scriptable.Equipment item);
        public delegate void OnItemEquip(Scriptable.Equipment item);

        public OnItemUnequip onItemUnequip;
        public OnItemEquip onItemEquip;
        
        private void Start()
        {
            int equipCount = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
            currentEquipment = new Scriptable.Equipment[equipCount];
            currentMeshes = new SkinnedMeshRenderer[equipCount];
            
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
            
            GameObject mesh = targetMesh.transform.Find(newItem.skinName).gameObject;

            if (mesh == null)
            {
                Debug.Log("You cant equip " + newItem.name + "; Not set skin name or skin with this name not exist!");
                return;
            }
            
            Unequip(itemIndex);
            
            currentEquipment[itemIndex] = newItem;
            SetEquipmentBlendShape(newItem, 100);
            if (onItemEquip != null)
            {
                onItemEquip.Invoke(newItem);
            }

            SkinnedMeshRenderer newMesh = mesh.GetComponent<SkinnedMeshRenderer>();
            
            newMesh.enabled = true;
//            SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);
//            newMesh.transform.parent = targetMesh.transform;
//
//            newMesh.bones = targetMesh.bones;
//            newMesh.rootBone = targetMesh.rootBone;
            currentMeshes[itemIndex] = newMesh;
        }


        public void Unequip(int slot)
        {
            if (IsSlotEquiped(slot))
            {
                if (currentMeshes[slot] != null)
                {
                    Destroy(currentMeshes[slot].gameObject);
                }
                
                Scriptable.Equipment oldItem = currentEquipment[slot];
                currentEquipment[slot] = null;
                SetEquipmentBlendShape(oldItem, 0);
                if (onItemUnequip != null)
                {
                    onItemUnequip.Invoke(oldItem);
                }
            }
        }

        void SetEquipmentBlendShape(Equipment item, int weight)
        {
            foreach (EquipmentMeshRegion meshRegion in item.coveredMeshRegion)
            {
                targetMesh.SetBlendShapeWeight((int) meshRegion, weight);
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
