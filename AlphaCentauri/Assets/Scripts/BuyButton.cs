using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public GameObject GameManager;
    public GameObject Buyscreen;
    public GameObject Sellscreen;

    public void OpenBuyscreen()
    {
        if (Buyscreen != null)
        {
            Buyscreen.SetActive(true);
            Sellscreen.SetActive(false);
            GameManager.GetComponent<ItemManager>().UpdateUI();
        }
    }
}
