using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance { get; set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
    }
    #endregion
}
