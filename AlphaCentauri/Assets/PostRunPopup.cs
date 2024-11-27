using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PostRunPopup : MonoBehaviour
{
    [SerializeField] float popUpTime = 2f, hideTime = 8;
    [SerializeField] int rokokPrice = 50000;
    [SerializeField] Transform gameManager, fishItemPrefab, fishesContainer, acceptButton, denyButton;
    [SerializeField] TMP_Text totalWeight;
    [SerializeField] RodManager rodManager;
    [SerializeField] FishingProgress fishingProgress;
    [SerializeField] LinePointAttacher linePointAttacher;
    [SerializeField] Guard guard;
    ItemManager itemManager;
    CentralStateManager centralStateManager;
    void Start()
    {
        itemManager = gameManager.GetComponent<ItemManager>();
        centralStateManager = gameManager.GetComponent<CentralStateManager>();
    }
    public void SetFishes(List<Fish> fishes)
    {
        float weightSum = 0;
        foreach (Transform child in fishesContainer)
        {
            Destroy(child.gameObject);
            Debug.Log("destroyin");
        }
        foreach (Fish fish in fishes)
        {
            Debug.Log("generating");
            Transform clone = Instantiate(fishItemPrefab, fishesContainer);
            clone.GetComponent<FishCollected>().SetFish(fish);
            weightSum += fish.Weight;
        }
        totalWeight.text = Mathf.Round(weightSum).ToString();

        rodManager.equippedRod.Hide();
        fishingProgress.Reset();
        rodManager.equippedRod.CanFish = false;
        if (rodManager.equippedRod.RodMechanics.cast.bobberClone != null)
        {
            rodManager.equippedRod.RodMechanics.cast.bobberClone.GetComponent<Bobber>().Finish();
        }
        rodManager.equippedRod.RodMechanics.fishWait.SetTempFishBite(false);
        linePointAttacher.Unequip();
    }
    bool canContinue;
    public void Show(bool canContinue)
    {
        this.canContinue = canContinue;
        denyButton.gameObject.SetActive(true);
        if (canContinue)
        {
            acceptButton.gameObject.SetActive(true);
        }
        else
        {
            acceptButton.gameObject.SetActive(false);
        }
    }
    public void Accept()
    {
        rodManager.equippedRod.CanFish = true;
        itemManager.DeductBalance(rokokPrice);
        transform.DOScale(new Vector3(0, 0, 0), popUpTime);
        guard.Reset();
        acceptButton.gameObject.SetActive(false);
        denyButton.gameObject.SetActive(false);
        centralStateManager.ContinueRun();
    }
    public void Deny()
    {
        transform.DOScale(new Vector3(0, 0, 0), popUpTime);
        centralStateManager.EndRun();
        acceptButton.gameObject.SetActive(false);
        denyButton.gameObject.SetActive(false);
        guard.Reset();
    }
}
