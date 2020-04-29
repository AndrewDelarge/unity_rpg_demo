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
        protected virtual void Start()
        {
            curElement.SetActive(false);
        }

        public void Show()
        {
            StartCoroutine(VisibleToggle(true));
        }

        public void Hide()
        {
            StartCoroutine(VisibleToggle(false));
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
                    renderers[i].SetAlpha(visible ? 1 - time : time);
                }

                yield return null;
            }
        }
    }
}