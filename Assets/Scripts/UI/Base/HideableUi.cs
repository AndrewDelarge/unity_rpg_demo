using System;
using System.Collections;
using Exceptions.Game.UI;
using GameSystems;
using UnityEngine;

namespace UI.Base
{
    public class HideableUi : MonoBehaviour
    {
        public float showSpeedTime = 1f;
        protected GameObject curElement;
        protected bool inited = false;

        public Action onHided;
        
        public virtual void Init()
        {
            curElement.SetActive(false);
            inited = true;
        }

        public void Show()
        {
            if (! inited)
                return;
            curElement.SetActive(true);
            StartCoroutine(VisibleToggle(true));
        }

        public void Hide()
        {
            if (! inited)
                return;
            curElement.SetActive(true);
            StartCoroutine(VisibleToggle(false));
        }
        
        public void Hide(bool fast)
        {
            if (! inited)
                return;
            curElement.SetActive(false);
        }

        protected IEnumerator VisibleToggle(bool visible)
        {
            CanvasRenderer[] renderers = curElement.GetComponentsInChildren<CanvasRenderer>();
            float time = 1;

            while (time > 0)
            {
                time -= Time.deltaTime * showSpeedTime;
                for (int i = 0; i < renderers.Length; i++)
                {
                    if (renderers[i] != null)
                    {
                        renderers[i].SetAlpha(visible ? 1 - time : time);
                    }
                }

                yield return null;
            }


            if (! visible)
            {
                onHided?.Invoke();
            }
        }

    }
}