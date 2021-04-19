using System.Collections;
using GameSystems;
using GameSystems.Languages;
using UI.Base;
using UnityEngine;
using Random = System.Random;
using Text = UnityEngine.UI.Text;

namespace UI
{
    public class FloatingHealthChange : HideableUi
    {
        [SerializeField]
        private Text value;
        [SerializeField]
        private float lifetime = 1f;
        
        private GameSystems.Languages.Text text;
        private Transform target;
        private Camera cam;
        
        public void Init(Transform targetTransform, string textOrCode, bool isCrit)
        {
            cam = CameraManager.Instance().GetCamera();
            target = targetTransform;
            curElement = gameObject;
            
            showSpeedTime = 4f;

            text.textCode = textOrCode;
            value.text = text.GetText();
            
            if (isCrit)
            {
                value.color = Color.red * 2;
                value.text += "!";
            }
            
            inited = true;
            Show();
            StartCoroutine(Lifetime());
        }
        

        IEnumerator Lifetime()
        {
            yield return new WaitForSeconds(lifetime / 2);
            Hide();
            inited = false;
        }
        
        void FixedUpdate()
        {
            if (! inited)
                return;
            
            Vector3 pos = cam.WorldToScreenPoint(target.position);
            
            pos.y *= target.localScale.y;
            curElement.transform.position = pos;
        }

    }
}