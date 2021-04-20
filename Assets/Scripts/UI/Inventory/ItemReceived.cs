using System.Collections;
using System.Collections.Generic;
using Scriptable;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class ItemReceived : HideableUi
    {
        
        public Image icon;
        public Text textHolder;
        
        [SerializeField] private Animator itemAnimator;
        
        public GameSystems.Languages.Text text;
        
        public override void Init()
        {
            curElement = gameObject;
            textHolder.text = text.GetText();
            
            icon.enabled = false;
            inited = true;
        }

        public void SetItem(Item item)
        {
            icon.sprite = item.icon;
        }
        
        public override void Show()
        {
            icon.enabled = true;
            
            if (itemAnimator != null)
                itemAnimator.SetTrigger("Reset");
            
            base.Show();
        }

        public override void Hide()
        {
            if (itemAnimator != null)
                itemAnimator.SetTrigger("Start");

            StartCoroutine(HideWithDelay());
        }
        IEnumerator HideWithDelay()
        {
            yield return new WaitForSeconds(0.8f);
            
            base.Hide();
            icon.enabled = false;
            icon.sprite = null;
        }

    }
}