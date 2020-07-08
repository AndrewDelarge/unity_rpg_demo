using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud.Joysticks
{
    public class CooldownableStick : VariableJoystick
    {
        private Image backgroundImage;
        private Image handleImage;

        private Color defaultHandleColor;
        private void Awake()
        {
            backgroundImage = background.GetComponent<Image>();
            handleImage = handle.GetComponent<Image>();
            defaultHandleColor = handleImage.color;
        }

        public void SetCooldown(float fill)
        {
            if (fill < 1)
                handleImage.color = new Color(1, 0, 0, .4f);
            else
                handleImage.color = defaultHandleColor;


            backgroundImage.fillAmount = fill;
        }
    }
}