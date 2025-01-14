using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Transform gameManager;
    public string description;
    public void Start()
    {
        gameManager = GameObject.Find("GameManager").transform;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameManager.GetComponent<Tooltip>().SetDesc(description);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        gameManager.GetComponent<Tooltip>().Hide();
    }
}
