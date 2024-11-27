using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnHoverBrighten : MonoBehaviour, IPointerEnterHandler
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color = new Color(1, 1, 1, 0.8f);
    }
    public void OnPointerEnter(PointerEventData data)
    {
        GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }
}
