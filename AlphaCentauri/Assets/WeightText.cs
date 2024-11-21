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
        Debug.Log(GetComponent<TMP_Text>());
        rodManager = gameManager.GetComponent<RodManager>();
    }
    void Update()
    {
        if (gameManager.GetComponent<CentralStateManager>().playerState == CentralStateManager.PlayerState.Rod)
        {
            Debug.Log(rodManager.equippedBucket);
            GetComponent<TMP_Text>().text = rodManager.equippedBucket.TotalWeight + "kg/" + rodManager.equippedBucket.MaxWeight + "kg";
        }

    }
}
