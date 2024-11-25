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
    [SerializeField] Image image;
    void Start()
    {

        transform.localScale = new Vector2(0, 0);
    }
    public void SetFish(Fish fish)
    {
        transform.DOScale(new Vector2(1, 1), 0.5f);
        fishName.text = fish.Name;
        weightAndLength.text = Mathf.Round(fish.Weight * 100) / 100 + "kg   " + Mathf.Round(fish.Length * 100) / 100 + "m";
        if (fish.Image != null)
        {
            image.GetComponent<Image>().sprite = fish.Image;
        }
    }
}
