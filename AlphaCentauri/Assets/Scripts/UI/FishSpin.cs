using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FishSpin : MonoBehaviour
{
    [SerializeField] float spinTime = 3;
    void Start()
    {
        StartCoroutine(SpinCoroutine());
    }
    IEnumerator SpinCoroutine()
    {
        while (true)
        {
            transform.DORotate(new Vector3(0, 450, 0), spinTime, RotateMode.FastBeyond360).SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(spinTime);
        }
    }
    public void Show(FishGenerator.FishType fishType, float popUpTime, float hideTime)
    {
        StartCoroutine(ShowCoroutine(fishType, popUpTime, hideTime));

    }
    IEnumerator ShowCoroutine(FishGenerator.FishType fishType, float popUpTime, float hideTime)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        GameObject clone = Instantiate(FishGenerator.GetFishModel(fishType), transform);
        clone.transform.localPosition = new Vector3(0, 0, 0);
        clone.layer = 3;

        transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(new Vector3(37, 37, 37), popUpTime).SetEase(Ease.InOutCirc);
        yield return new WaitForSeconds(hideTime);
        transform.DOScale(new Vector3(0, 0, 0), popUpTime).SetEase(Ease.InOutCirc);
    }
}
