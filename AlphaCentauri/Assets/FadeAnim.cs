using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FadeAnim : MonoBehaviour
{
    [SerializeField] float duration = 1f, distance = 50, delay = 0f;
    [SerializeField] Ease ease = Ease.OutBack;
    bool showing = false;
    IEnumerator ShowCoroutine()
    {
        yield return new WaitForSeconds(delay);
        GetComponent<CanvasGroup>().DOFade(1, 1f);
        Vector3 original = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y - distance, transform.position.z);
        transform.DOMove(original, duration).SetEase(ease);
    }
    IEnumerator Hide()
    {
        yield return new WaitForSeconds(delay);
        GetComponent<CanvasGroup>().DOFade(0, 1f);
        Vector3 end = new Vector3(transform.position.x, transform.position.y - distance, transform.position.z);
        transform.DOMove(end, duration).SetEase(ease);
    }
    void Update()
    {
        if (gameObject.activeSelf && !showing)
        {
            StartCoroutine(ShowCoroutine());
            showing = true;
        }
        else if (!gameObject.activeSelf && showing) { StartCoroutine(Hide()); showing = false; }
    }
}
