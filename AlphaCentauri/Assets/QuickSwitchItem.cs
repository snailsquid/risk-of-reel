using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSwitchItem : MonoBehaviour
{
    ItemRegistry.BuyItemType bait;
    ItemManager itemManager;
    [SerializeField] int index;
    void Start()
    {
        itemManager = GameObject.Find("GameManager").transform.GetComponent<ItemManager>();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    public void SetBait(ItemRegistry.BuyItemType bait, int index)
    {
        this.bait = bait;
        this.index = index;
        transform.GetChild(0).GetComponent<Image>().sprite = ItemRegistry.BuyItems[bait].Image;
    }
    void OnClick()
    {
        if (index == transform.parent.GetComponent<QuickSwitch>().baitIndex)
        {
            transform.parent.GetComponent<QuickSwitch>().ToggleContainer();
        }
        else
        {
            transform.parent.GetComponent<QuickSwitch>().SwitchBait(index);
            transform.parent.GetComponent<QuickSwitch>().ToggleContainer();
        }
    }
}
