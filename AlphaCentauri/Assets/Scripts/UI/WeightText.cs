using System.Collections;
using System.Collections.Generic;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeightText : MonoBehaviour
{
    [SerializeField] Transform gameManager;
    [SerializeField] Image image;
    [SerializeField] ItemManager itemManager;
    RodManager rodManager;
    void Start()
    {
        rodManager = gameManager.GetComponent<RodManager>();
    }
    void Update()
    {
        if (gameManager.GetComponent<CentralStateManager>().CurrentGameState == CentralStateManager.GameState.Rod)
        {
            GetComponent<TMP_Text>().text = rodManager.equippedBucket.TotalWeight + "kg/" + rodManager.equippedBucket.MaxWeight + "kg";
            UpgradeItem upgradeItem = itemManager.shop.UpgradeItems[ItemRegistry.UpgradeItemType.Bucket];
            image.sprite = upgradeItem.Image[upgradeItem.CurrentLevel];
        }

    }
}
