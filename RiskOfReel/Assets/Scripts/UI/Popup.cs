using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PopUp : MonoBehaviour
{
    [SerializeField] public float popUpTime = 2f, hideTime = 5;
    [SerializeField] TMP_Text fishName, fishWeightAndLength;
    [SerializeField] Transform gameManager, fishPosition;
    [SerializeField] LinePointAttacher linePointAttacher;
    [SerializeField] ItemManager itemManager;
    [SerializeField] FishSpin fishSpin;
    public static string FishType;//nanti diganti
    Vector3 scaleTo = new Vector3(0.5f, 0.5f, 0.5f);

    RodManager rodManager;
    FishGenerator.FishType fishType;
    Dictionary<Fish.FishRarity, Color> fishColors = new Dictionary<Fish.FishRarity, Color>{
        {Fish.FishRarity.Common, new Color(90, 172, 84 )},
        {Fish.FishRarity.Legendary, new Color(90, 172, 84 )},
        {Fish.FishRarity.Special, new Color(90, 172, 84 )},
        {Fish.FishRarity.Ampas, new Color(1,1, 1 )},
        {Fish.FishRarity.Dev, new Color(1,1, 1 )},
    };
    void Start()
    {
        rodManager = gameManager.GetComponent<RodManager>();
    }
    public void SetText(Fish fish)
    {
        fishType = fish.fishType;
        fishName.text = fish.Name;
        if (fishColors[fish.Rarity] != null)
        {
            fishName.color = fishColors[fish.Rarity];
        }
        fishWeightAndLength.text = fish.Weight + "kg\n";
    }
    public void Show()
    {
        StartCoroutine(WaitCoroutine());
    }
    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(rodManager.equippedRod.RodMechanics.cast.bobberClone.GetComponent<Bobber>().duration);

        transform.DOScale(scaleTo, popUpTime).SetEase(Ease.InOutCirc);
        fishSpin.Show(fishType, popUpTime, hideTime);
        yield return new WaitForSeconds(hideTime);
        transform.DOScale(new Vector3(0, 0), popUpTime).SetEase(Ease.InOutCirc);
        rodManager.equippedRod.SetState(RodRegistry.RodState.PreCast);
        linePointAttacher.Equip(itemManager.shop.UpgradeItems[ItemRegistry.UpgradeItemType.Rod].CurrentLevel);
    }
}
