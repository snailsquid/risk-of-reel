using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public static class CanvasExtensions
    {
        public static void Toggle(this Canvas canvas, bool value)
        {
            const UITransition.TransitionType defaultTransitionTypeIn = UITransition.TransitionType.FadeIn;
            const UITransition.TransitionType defaultTransitionTypeOut = UITransition.TransitionType.FadeOut;
            
            Toggle(canvas, value, value?  defaultTransitionTypeIn: defaultTransitionTypeOut);
        }
        public static void Toggle(this Canvas canvas, bool value, UITransition.TransitionType transitionType)
        {
            switch (transitionType)
            {
                case UITransition.TransitionType.FadeIn:
                    canvas.enabled = true; break;
                case UITransition.TransitionType.FadeOut:
                    canvas.enabled = false; break;
                default:
                    canvas.enabled = false; break;
            }
        }
    }
}