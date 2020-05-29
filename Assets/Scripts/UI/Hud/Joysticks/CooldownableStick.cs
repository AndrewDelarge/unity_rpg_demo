using UnityEngine.UI;

namespace UI.Hud.Joysticks
{
    public class CooldownableStick : global::VariableJoystick
    {
        private Image backgroundImage;
        
        private void Awake()
        {
            backgroundImage = background.GetComponent<Image>();
        }

        public void SetCooldown(float fill)
        {
            if (fill < 1)
                enabled = false;
            else
                enabled = true;

            backgroundImage.fillAmount = fill;
        }
    }
}