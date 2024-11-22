using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FishCollected : MonoBehaviour
{
    [SerializeField] TMP_Text fishName, weightAndLength;
    [SerializeField] Image image;
    public void SetFish(Fish fish)
    {
        fishName.text = fish.Name;
        weightAndLength.text = Mathf.Round(fish.Weight * 100) / 100 + "kg   " + Mathf.Round(fish.Length * 100) / 100 + "m";
        if (fish.Image != null)
        {
            image.GetComponent<Image>().sprite = fish.Image;
        }
    }
}
