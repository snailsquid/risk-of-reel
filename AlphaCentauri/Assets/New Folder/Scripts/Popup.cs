using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUp : MonoBehaviour
{
    public GameObject popupPanel;
    public Button closeButton;
    public static bool poupPanelActiveSelf;

    void Start()
    {
        popupPanel.SetActive(true);
        poupPanelActiveSelf = true;

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HidePopup);
        }
    }
    public void HidePopup()
    {
        popupPanel.SetActive(false);
        poupPanelActiveSelf = false;
    }
}
