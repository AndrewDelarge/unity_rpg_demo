using Actors.Base;
using UI.Hud.Joysticks;
using UnityEngine;

namespace UI.Hud
{
    public class ActionBar : MonoBehaviour
    {
        
        public Joystick joystick;
        public CooldownableStick aimControlStick;

        
        public delegate void OnActionKeyClick();
        public delegate void OnSecKeyClick();
        public OnActionKeyClick onActionKeyClick;
        public OnSecKeyClick onSecKeyClick;


        private Combat playerCombat;
        
        public void Init()
        {
            onActionKeyClick = null;
            onSecKeyClick = null;
        }

        private void FixedUpdate()
        {
            float cooldown = Mathf.Min(playerCombat.GetRangeCooldown(), playerCombat.rangeAttackCooldown);
            aimControlStick.SetCooldown(cooldown / playerCombat.rangeAttackCooldown);
            Debug.Log(cooldown / playerCombat.rangeAttackCooldown);
        }


        public void SetPlayer(GameObject player)
        {
            playerCombat = player.GetComponent<Combat>();
        }
        
        public void OnActionClick()
        {
            onActionKeyClick?.Invoke();
        }
        
        public void OnSecClick()
        {
            onSecKeyClick?.Invoke();
        }
    }
}
