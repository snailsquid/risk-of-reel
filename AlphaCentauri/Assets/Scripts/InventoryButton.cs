using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    public GameObject Buyscreen;
    public GameObject Inventory;

    public void OpenInventory()
    {
        if(Inventory != null)
        {
           Inventory.SetActive(true);
           Buyscreen.SetActive(false);
        }
    }
}

