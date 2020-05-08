using Actors.Base;
using Managers;
using Scriptable;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class InventoryStatsView : MonoBehaviour
    {
        public Text health;
        public Text armor;
        public Text damage;
        public Text coins;
        private Stats stats;
        
        
        public void Init(PlayerManager playerManager)
        {
            Actor player = playerManager.GetPlayer();
            stats = player.stats;
            playerManager.equipmentManager.onItemEquip += OnEqip;
            playerManager.equipmentManager.onItemUnequip += OnEqip;
            UpdateValues();
        }

        void OnEqip(Item item)
        {
            UpdateValues();
        }
        
        public void UpdateValues()
        {
            health.text = stats.GetMaxHealth().ToString();
            armor.text = $"{stats.GetArmorMultiplier()}%";
            damage.text = stats.GetDamageValue(false, false).GetValue().ToString();
        }
    }
}
