using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FishCollected : MonoBehaviour
{
    [SerializeField] TMP_Text fishName, weightAndLength;
    [SerializeField] FishSpin fishSpin;
    Fish fish;
    void Start()
    {
        Debug.Log(transform.Find("FishContainer"));
        fishSpin = GameObject.Find("FishContainer").GetComponent<FishSpin>();
    }
    public void SetFish(Fish fish)
    {
        this.fish = fish;
        fishName.text = fish.Name;
        weightAndLength.text = Mathf.Round(fish.Weight * 100) / 100 + "kg   " + Mathf.Round(fish.Length * 100) / 100 + "m";
    }
    public void OnClick()
    {
        fishSpin.Show(fish.fishType, 0.5f, 1f);
    }
}
