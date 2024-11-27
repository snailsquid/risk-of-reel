using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FadeAnim : MonoBehaviour
{
    [SerializeField] float duration = 1f, delay = 0f;
    float distance = 1;
    [SerializeField] Ease ease = Ease.OutBack;
    bool showing = false;
    Vector3 original;
    IEnumerator ShowCoroutine()
    {
        yield return new WaitForSeconds(delay);
        GetComponent<CanvasGroup>().DOFade(1, .2f);
        transform.DOMove(original, duration).SetEase(ease);
    }
    IEnumerator HideCoroutine()
    {
        yield return new WaitForSeconds(delay);
        GetComponent<CanvasGroup>().DOFade(0, 1f);
        Vector3 end = new Vector3(transform.position.x, transform.position.y - distance, transform.position.z);
        transform.DOMove(end, duration).SetEase(ease).onComplete += () => gameObject.SetActive(false);
    }
    void Update()
    {
        if (gameObject.activeSelf && !showing)
        {
            // StartCoroutine(ShowCoroutine());
            showing = true;
        }
        // else if (!gameObject.activeSelf && showing) { StartCoroutine(Hide()); showing = false; }
    }
    public void Show()
    {
        if (gameObject.activeSelf) return;
        if (GetComponent<CanvasGroup>() == null) { Debug.Log(gameObject + " does not have canvas group"); return; }
        gameObject.SetActive(true);
        original = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y - distance, transform.position.z);
        Debug.Log(GetComponent<CanvasGroup>());
        GetComponent<CanvasGroup>().alpha = 0;
        StartCoroutine(ShowCoroutine());
    }
    public void Hide()
    {
        if (gameObject.activeSelf)
            StartCoroutine(HideCoroutine());
    }
    public void SetUI(bool active)
    {
        if (active) Show();
        else Hide();
    }
}
