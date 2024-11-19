using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HideButton : MonoBehaviour
{
    [SerializeField] Transform reference;
    Transform player;
    bool debounce = false;
    Button button;
    Hide hide;
    void Start()
    {
        player = reference.GetComponent<ReferenceScript>().player;
        hide = player.GetComponent<Hide>();
        button = transform.GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            print(hide.cooldownLeft);
            if (!hide.isHide && hide.cooldownLeft <= 0)
            {
                hide.StartHide();
            }
            else
            {
                hide.StopHide();
                debounce = false;
            }
        });

    }
    void Update()
    {
        if (!hide.isHide)
        {
            if (hide.cooldownLeft > 0)
            {

                button.GetComponentInChildren<TMP_Text>().text = "(" + Mathf.Round(hide.cooldownLeft) + ")";
            }
            else
            {

                button.GetComponentInChildren<TMP_Text>().text = "Hide";
            }
        }
        else
        {
            button.GetComponentInChildren<TMP_Text>().text = "Unhide (" + Mathf.Round(hide.forcedUnhideTime) + ")";
        }

    }
}
