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
        public Animator animator;
        public Text value;
        public float lifetime = 1f;
        public GameSystems.Languages.Text text;
        protected CameraController cam;
        public Transform target;
        private bool inited = false;
        
        public void Init(Transform targetTransform, string textOrCode, bool isCrit)
        {
            showSpeedTime = 4f;
            animator.speed = 0;
            curElement = gameObject;
            onHided += () => Destroy(this);
            Show();
            cam = GameController.instance.mainCamera;
            target = targetTransform;
            text.textCode = textOrCode;
            value.text = text.GetText();
            inited = true;
            if (isCrit)
            {
                value.color = Color.red * 2;
                value.text += " CRIT!";
            }
            
            StartCoroutine(Lifetime());
        }
        

        IEnumerator Lifetime()
        {
            animator.speed = UnityEngine.Random.Range(0.25f, 0.35f);
            yield return new WaitForSeconds(lifetime / 2);
            Hide();
            yield return new WaitForSeconds(showSpeedTime);
            //TODO Destroy does not work!
            Destroy(curElement);
        }
        
        void FixedUpdate()
        {
            if (! inited)
            {
                return;
            }
            
            curElement.transform.position = 
                new Vector3(target.position.x, (target.position.y + 1) * target.localScale.y, target.position.z);
            curElement.transform.forward = cam.GetCamera().transform.forward;
        }
    }
}