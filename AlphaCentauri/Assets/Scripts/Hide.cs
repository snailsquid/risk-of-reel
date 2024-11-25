using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Hide : MonoBehaviour
{
    public bool isHide
    {
        get;
        private set;
    } = false;
    [SerializeField] float maxHideTime = 8, cooldownTime = 10, timeHideAnim = 0.8f;
    [SerializeField] Transform playerDefaultLocation, bush;
    float timeHiding = 0, timeUnhiding = 0;
    bool canHide = true;
    Vector3 velocity;
    CentralStateManager centralStateManager;
    public float cooldownLeft
    {
        get;
        private set;
    }
    public float forcedUnhideTime { get; private set; }
    void Start()
    {
        centralStateManager = GameObject.Find("GameManager").GetComponent<CentralStateManager>();
    }
    void Update()
    {
    }

    public void StartHide()
    {
        print("cant hide rn" + cooldownLeft);
        if (canHide)
        {
            print("I can hide");
            GoHide();
        }
    }
    public void GoHide()
    {
        transform.DOMove(bush.position, timeHideAnim);
        transform.DORotate(bush.rotation.eulerAngles, timeHideAnim);
    }
    public void GoUnhide()
    {
        transform.DOMove(playerDefaultLocation.position, timeHideAnim);
        transform.DORotate(bush.rotation.eulerAngles, timeHideAnim);
    }
    public void StopHide()
    {
        GoUnhide();
    }

}
