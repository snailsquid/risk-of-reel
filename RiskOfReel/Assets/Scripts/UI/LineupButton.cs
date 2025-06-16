using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineupButton : MonoBehaviour
{
    [SerializeField] Transform gameManager;
    [SerializeField] Image image;
    [SerializeField] int index;
    ItemManager itemManager;
    ItemRegistry.BuyItemType buyItemType;

    public void SetButton(ItemRegistry.BuyItemType buyItemType)
    {
        this.buyItemType = buyItemType;
        image.sprite = ItemRegistry.BuyItems[buyItemType].Image;
    }
    void OnClick()
    {
        Debug.Log(ItemRegistry.BuyItems[buyItemType].Name);
        gameManager.GetComponent<ItemManager>().RemoveFromLineup(index);
    }
    void Awake()
    {
        itemManager = gameManager.GetComponent<ItemManager>();
    }
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
}
