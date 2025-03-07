using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UIAnimations
{
    public enum Transition
    {
        In,
        Out
    }
    public enum Type
    {
        Fade,
        Move,
        Scale,
        FadeMove,
        FadeScale,
        MoveScale,
    }
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Custom
    }

    public static void Animate(this UIBehaviour element, Transition transition, Type type, float duration, Ease ease)
    {
        element.gameObject.SetActive(transition == Transition.In ? true : transition == Transition.Out ? false : true);
        switch (type)
        {
            case Type.Fade:
                Fade(transition, element, duration);
                break;
            case Type.Move:
                Move(transition, element, duration);
                break;
            case Type.Scale:
                Scale(transition, element, duration);
                break;
            case Type.FadeMove:
                Fade(transition, element, duration);
                Move(transition, element, duration);
                break;
            case Type.FadeScale:
                Fade(transition, element, duration);
                Scale(transition, element, duration);
                break;
            case Type.MoveScale:
                Move(transition, element, duration);
                Scale(transition, element, duration);
                break;
        }
    }
    static void Fade(Transition transition, UIBehaviour element, float duration)
    {
        float initial = transition == Transition.In ? 0 : 1;
        float final = transition == Transition.In ? 1 : 0;
        CanvasGroup canvasGroup = element.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = element.gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = initial;
        canvasGroup.DOFade(final, duration);
    }
    static void Move(Transition transition, UIBehaviour element, float duration, Direction direction = Direction.Up, Vector2 initialAnchoredPos = new Vector2())
    {
        RectTransform rectTransform = element.GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("UIBehaviour does not have RectTransform component");
            return;
        }
        Vector2 initialAnchorMax = rectTransform.anchorMax;
        Vector2 initialAnchorMin = rectTransform.anchorMin;
        Vector3 finalPosition;
        if (direction == Direction.Custom)
        {
            Vector2 initialAnchoredPos_ = new Vector2(0, -5);
            if (initialAnchoredPos != new Vector2())
            {
                initialAnchoredPos_ = initialAnchoredPos;
            }
            finalPosition = initialAnchoredPos_;

            return;
        }
        switch (transition)
        {
            case Transition.In:
                finalPosition = rectTransform.anchoredPosition;
                switch (direction)
                {
                    case Direction.Up:
                        rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, 0);
                        rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, 0);
                        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -rectTransform.rect.height/2);
                        break;
                    case Direction.Down:
                        rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, 1);
                        rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, 1);
                        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.rect.height/2);
                        break;
                    case Direction.Left:
                        rectTransform.anchorMax = new Vector2(1, rectTransform.anchorMax.y);
                        rectTransform.anchorMin = new Vector2(1, rectTransform.anchorMin.y);
                        rectTransform.anchoredPosition = new Vector2(rectTransform.rect.width/2, rectTransform.anchoredPosition.y);
                        break;
                    case Direction.Right:
                        rectTransform.anchorMax = new Vector2(0, rectTransform.anchorMax.y);
                        rectTransform.anchorMin = new Vector2(0, rectTransform.anchorMin.y);
                        rectTransform.anchoredPosition = new Vector2(-rectTransform.rect.width/2, rectTransform.anchoredPosition.y);
                        break;
                }

                Vector2 localPosition = rectTransform.localPosition;
                rectTransform.anchorMax = initialAnchorMax;
                rectTransform.anchorMin = initialAnchorMin;
                rectTransform.localPosition = localPosition;
                rectTransform.DOAnchorPos(finalPosition, duration);
                break;
            case Transition.Out:
                Vector3 initialPosition = element.transform.position;
                switch (direction)
                {
                    case Direction.Up:
                        rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, 1);
                        rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, 1);
                        finalPosition = new Vector3(initialPosition.x, initialPosition.y + rectTransform.rect.height, initialPosition.z);
                        break;
                    case Direction.Down:
                        rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, 0);
                        rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, 0);
                        finalPosition = new Vector3(initialPosition.x, initialPosition.y - rectTransform.rect.height, initialPosition.z);
                        break;
                    case Direction.Left:
                        rectTransform.anchorMax = new Vector2(0, rectTransform.anchorMax.y);
                        rectTransform.anchorMin = new Vector2(0, rectTransform.anchorMin.y);
                        finalPosition = new Vector3(initialPosition.x - rectTransform.rect.width, initialPosition.y, initialPosition.z);
                        break;
                    case Direction.Right:
                        rectTransform.anchorMax = new Vector2(1, rectTransform.anchorMax.y);
                        rectTransform.anchorMin = new Vector2(1, rectTransform.anchorMin.y);
                        finalPosition = new Vector3(initialPosition.x + rectTransform.rect.width, initialPosition.y, initialPosition.z);
                        break;
                }
                break;
        }

    }
    static void Scale(Transition transition, UIBehaviour element, float duration)
    {
    }
}
