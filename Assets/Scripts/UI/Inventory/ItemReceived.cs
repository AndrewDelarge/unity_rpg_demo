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
        public GameSystems.Languages.Text text;


        protected List<Item> itemsToShow;
        private Animator itemAnimator;

        public override void Init()
        {
            curElement = gameObject;
            itemAnimator = GetComponentInChildren<Animator>();
            textHolder.text = text.GetText();
            icon.enabled = false;
            inited = true;
        }

        public void ShowItem(Item item)
        {
            gameObject.SetActive(true);
            icon.sprite = item.icon;
            icon.enabled = true;
            if (itemAnimator != null)
            {
                itemAnimator.SetTrigger("Reset");
            }
            
            StartCoroutine(ShowWithAnimation());
        }

        IEnumerator ShowWithAnimation()
        {
            Show();
            yield return new WaitForSeconds(1f);

            if (itemAnimator != null)
            {
                itemAnimator.SetTrigger("Start");
            }
            
            yield return new WaitForSeconds(0.8f);
            Hide();
            icon.enabled = false;
            icon.sprite = null;
        }
    }
}