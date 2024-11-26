using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class QuickSwitch : MonoBehaviour
{
    [SerializeField] Transform baitItemPrefab, baitItemContainer;
    [SerializeField] float transitionTime = 0.5f;
    public List<ItemRegistry.BuyItemType> baits { get; private set; } = new List<ItemRegistry.BuyItemType>();
    [SerializeField] public int baitIndex { get; private set; } = 0;
    List<Transform> baitItem = new List<Transform>();
    ItemManager itemManager;

    void SetBaits(List<ItemRegistry.BuyItemType> baits)
    {
        Debug.Log("Setting bait");
        if (baits.Count == 3)
        {
            this.baits = baits;
        }
    }
    public void SwitchBait(int index)
    {
        baitIndex = index;
        ResetUI();
    }
    void Start()
    {
        itemManager = GameObject.Find("GameManager").transform.GetComponent<ItemManager>();
    }

    public void ResetUI()
    {
        Debug.Log("Resetting UI");
        SetBaits(itemManager.BaitLineup);
        foreach (Transform item in baitItemContainer)
        {
            Destroy(item.gameObject);
        }
        int index = 0;

        Transform indexItem = null;
        foreach (ItemRegistry.BuyItemType item in baits)
        {
            Debug.Log(item);
            Transform clone = Instantiate(baitItemPrefab, baitItemContainer);
            clone.GetComponent<QuickSwitchItem>().SetBait(item, index);
            if (index == baitIndex)
            {
                indexItem = clone;
            }
            index++;
        }
        if (indexItem == null) return;
        indexItem.SetAsFirstSibling();
    }
    public void ToggleContainer()
    {
        RectTransform rt = baitItemContainer.GetComponent<RectTransform>();
        float y = rt.sizeDelta.y;
        float x = rt.sizeDelta.x;
        float calc = x == y ? x * 3 : x;
        Debug.Log(x + " " + y);
        Debug.Log(calc + " " + y);
        rt.DOSizeDelta(new Vector2(x, calc), transitionTime).SetEase(Ease.InOutCirc);
    }

}
