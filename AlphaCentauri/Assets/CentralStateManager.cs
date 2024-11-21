using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CentralStateManager : MonoBehaviour
{
    [SerializeField] Transform hideButton;
    TimeManager timeManager;
    RodManager rodManager;
    ItemManager itemManager;
    public enum PlayerState
    {
        StartMenu,
        Rod,
        Shop,
    }
    public PlayerState playerState { get; private set; }
    void Awake()
    {
        timeManager = GetComponent<TimeManager>();
        rodManager = GetComponent<RodManager>();
        itemManager = GetComponent<ItemManager>();
    }
    void Start()
    {
        SetState(PlayerState.Shop);
    }

    void SetState(PlayerState state)
    {
        Debug.Log("Changing to state " + state);
        timeManager.UI(state == PlayerState.Rod);
        itemManager.UI(state == PlayerState.Shop);
        hideButton.gameObject.SetActive(state == PlayerState.Rod);
        playerState = state;
    }
}
