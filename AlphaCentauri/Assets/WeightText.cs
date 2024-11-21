using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeightText : MonoBehaviour
{
    [SerializeField] Transform gameManager;
    RodManager rodManager;
    void Start()
    {
        rodManager = gameManager.GetComponent<RodManager>();
    }
    void Update()
    {
        if (gameManager.GetComponent<CentralStateManager>().playerState == CentralStateManager.PlayerState.Rod)
        {
            GetComponent<TMP_Text>().text = rodManager.equippedBucket.TotalWeight + "kg/" + rodManager.equippedBucket.MaxWeight + "kg";
        }

    }
}
