using System.Collections;
using UnityEngine;
using static UI.UIUtils;

namespace UI
{
    public static class RectTransformExtensions
    {
        public static void Toggle(this RectTransform transform, bool value)
        {
            Debug.Log("Toggling " + transform + ": " + value);
            UITransition uiTransition = new UITransition(value?TransitionType.In:TransitionType.Out);
            transform.Toggle(uiTransition);
        }
        public static void Toggle(this RectTransform transform, UITransition uiTransition)
        {
            var value = uiTransition.TransitionType == TransitionType.In;
            transform.gameObject.SetActive(value);
            transform.Animate(uiTransition);
            // SetActiveWait(transform, uiTransition.Duration, value);
        }
        private static IEnumerator SetActiveWait(RectTransform transform, float duration, bool value)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.gameObject.SetActive(value);
        }
    }
}