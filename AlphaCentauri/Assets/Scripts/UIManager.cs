using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    [SerializedDictionary("UI Element", "UI Game Object")]
    public SerializedDictionary<string, UIBehaviour> uiElements;
    [SerializedDictionary("UI Element Group", "UI Element")]
    public SerializedDictionary<string, string[]> uiElementGroup;
    public void SetUI(string uiElement, bool active = true, UIAnimations.Type type = UIAnimations.Type.Move, float duration = 0.2f, Ease ease = Ease.InCirc)
    {
        if (!uiElements.ContainsKey(uiElement))
        {
            Debug.LogWarning("UI List does not contain \"" + uiElement + "\"");
            return;
        }
        UIBehaviour element = uiElements[uiElement];
        UIAnimations.Transition transition = active?UIAnimations.Transition.In:UIAnimations.Transition.Out;

        element.Animate(transition, type, duration, ease);
    }
    public void SetActiveGroup(string group, bool active)
    {
        if (uiElementGroup.ContainsKey(group))
        {
            foreach (string uiElement in uiElementGroup[group])
            {
                SetUI(uiElement, active);
            }
        }
    }
    public void NextUI(string currentUIElement, string nextUIElement)
    {
        SetUI(nextUIElement, true);
        SetUI(currentUIElement, false);
    }
    public void NextUIGroup(string currentUIGroup, string nextUIGroup)
    {
        SetActiveGroup(nextUIGroup, true);
        SetActiveGroup(currentUIGroup, false);
    }
    public void SetText(string uiElement, string text)
    {
        if (uiElements.ContainsKey(uiElement) && uiElements[uiElement] is TMP_Text)
        {
            uiElements[uiElement].GetComponent<TMP_Text>().text = text;
        }
    }
}
