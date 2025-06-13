using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static UI.UIUtils;
using static UI.UIAnimation;

namespace UI
{
    public static class UITweener
    {
        public static void Animate(this RectTransform ui, UITransition uiTransition)
        {
            if (uiTransition.TransitionAnimation.HasFlag(TransitionAnimation.Fade) && ui.GetComponent<CanvasGroup>() == null)
            {
                Debug.LogWarning("UI Transition UI: UI Canvas Group is missing");
                return;
            }
            
            var originalPosition = ui.anchoredPosition;
            var endPosition = originalPosition;
            var startPosition = CalculateEndPosition(endPosition, uiTransition.Distance, uiTransition.TransitionDirection);
            float startAlpha = 0;
            float endAlpha = 1;

            if (uiTransition.TransitionType == TransitionType.Out)
            {
                (startPosition, endPosition) = (endPosition, startPosition);
                (startAlpha, endAlpha) = (endAlpha, startAlpha);
            }

            if (uiTransition.TransitionAnimation.HasFlag(TransitionAnimation.Fade))
            {
                Fade(ui.GetComponent<CanvasGroup>(), uiTransition.Duration, uiTransition.Ease, startAlpha, endAlpha);    
            }

            if (uiTransition.TransitionAnimation.HasFlag(TransitionAnimation.Slide))
            {
                Slide(ui, startPosition, endPosition, uiTransition.Duration, uiTransition.Ease, originalPosition);    
            }

        }


    }
        public class UITransition
        {
            public readonly float Duration = 1f;
            public readonly float Distance = 100f;
            public readonly TransitionType TransitionType;
            public readonly TransitionAnimation TransitionAnimation = TransitionAnimation.Slide | TransitionAnimation.Fade;
            public readonly TransitionDirection TransitionDirection = TransitionDirection.Bottom;
            public readonly Ease Ease = Ease.OutBack;

            public UITransition(TransitionType transitionType)
            {
                TransitionType = transitionType;
            }
            public UITransition(TransitionType transitionType, float duration, float distance, TransitionDirection transitionDirection, TransitionAnimation transitionAnimation, Ease ease)
            {
                TransitionType = transitionType;
                Duration = duration;
                Distance = distance;
                TransitionDirection = transitionDirection;
                TransitionAnimation = transitionAnimation;
                Ease = ease;
            }
        }

    public static class UIAnimation
    {
        public static void Slide(RectTransform rt, Vector2 startPosition, Vector2 endPosition, float duration, Ease ease, Vector2 originalPosition )
        { 
            Debug.Log(startPosition + " - " + endPosition);
            rt.anchoredPosition = startPosition;
            rt.DOAnchorPos(endPosition, duration).SetEase(ease).onComplete = () => {rt.anchoredPosition = originalPosition;};
        }

        public static void Fade(CanvasGroup canvasGroup, float duration, Ease ease, float startAlpha = 0, float endAlpha = 1)
        {
            canvasGroup.alpha = startAlpha;
            canvasGroup.DOFade(endAlpha, duration).SetEase(ease);
        }
    }
    public static class UIUtils
    {
        [Flags]
        public enum TransitionAnimation
        {
            None = 0,
            Slide = 1 << 1,
            Fade = 1 << 2,
        }

        public enum TransitionDirection
        {
            None,
            Bottom,
            Top,
            Right,
            Left
        }

        public enum TransitionType
        {
            In, Out
        }


        public static Vector2 CalculateEndPosition(Vector2 startPosition, float distance,
             TransitionDirection transitionDirection)
        {
            return transitionDirection switch
            {
                TransitionDirection.Bottom => startPosition - new Vector2(0f, distance),
                TransitionDirection.Top => startPosition + new Vector2(0f, distance),
                TransitionDirection.Right => startPosition + new Vector2(distance, 0f),
                TransitionDirection.Left => startPosition - new Vector2(distance, 0f),
                _ => startPosition // Fallback
            };
        }
    }
}