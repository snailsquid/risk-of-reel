using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] Transform tooltipObject;
    CentralStateManager centralStateManager;
    // Start is called before the first frame update
    void Start()
    {
        centralStateManager = GetComponent<CentralStateManager>();
        tooltipObject.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (centralStateManager.playerState == CentralStateManager.PlayerState.Shop)
        {
            tooltipObject.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
        }
    }
    public void SetDesc(string desc)
    {
        tooltipObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = desc;
        tooltipObject.gameObject.SetActive(true);
    }
    public void Hide()
    {
        tooltipObject.gameObject.SetActive(false);
    }
}
