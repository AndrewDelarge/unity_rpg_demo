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
        public Text value;
        public float lifetime = 1f;
        public GameSystems.Languages.Text text;
        public Transform target;
        protected Camera cam;
        protected bool inited = false;
        
        public void Init(Transform targetTransform, string textOrCode, bool isCrit)
        {
            cam = GameController.instance.cameraController.GetCamera();
            target = targetTransform;
            curElement = gameObject;

            showSpeedTime = 4f;
            onHided += () => Destroy(gameObject);

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
        }
        
        void FixedUpdate()
        {
            if (! inited)
            {
                return;
            }

            Vector3 pos = cam.WorldToScreenPoint(target.position);
            pos.y *= target.localScale.y;
            curElement.transform.position = pos;
        }
    }
}