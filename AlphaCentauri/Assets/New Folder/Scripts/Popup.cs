using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup : MonoBehaviour
{
    public GameObject popupPanel; // The panel containing the popup
    public Button closeButton; // Button to close the popup

    void Start()
    {
        popupPanel.SetActive(true);

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HidePopup);
        }
    }

    // Hide the popup
    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}
