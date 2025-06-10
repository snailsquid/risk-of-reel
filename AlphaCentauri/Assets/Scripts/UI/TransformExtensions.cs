using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public static class TransformExtensions
    {
        public static void Toggle(this Transform transform, bool value)
        {
            const UITransition.TransitionType defaultTransitionTypeIn = UITransition.TransitionType.FadeIn;
            const UITransition.TransitionType defaultTransitionTypeOut = UITransition.TransitionType.FadeOut;
            
            Toggle(transform, value, value?  defaultTransitionTypeIn: defaultTransitionTypeOut);
        }
        public static void Toggle(this Transform transform, bool value, UITransition.TransitionType transitionType)
        {
            switch (transitionType)
            {
                case UITransition.TransitionType.FadeIn:
                    transform.gameObject.SetActive(true); break;
                case UITransition.TransitionType.FadeOut:
                    transform.gameObject.SetActive(false); break;
                default:
                    transform.gameObject.SetActive(false); break;
            }
        }
    }
}