using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CentralStateManager : MonoBehaviour
{
    [SerializeField] Transform hideButton, weightText, postRunPopupObject;
    PostRunPopup postRunPopup;
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
        postRunPopup = postRunPopupObject.GetComponent<PostRunPopup>();
        timeManager = GetComponent<TimeManager>();
        rodManager = GetComponent<RodManager>();
        itemManager = GetComponent<ItemManager>();
    }
    void Start()
    {
        SetState(PlayerState.Shop);
    }

    public void SetState(PlayerState state)
    {
        Debug.Log("Changing to state " + state);
        timeManager.UI(state == PlayerState.Rod);
        itemManager.UI(state == PlayerState.Shop);
        weightText.gameObject.SetActive(state == PlayerState.Rod);
        hideButton.gameObject.SetActive(state == PlayerState.Rod);
        playerState = state;
    }
    public void FinishRun(bool canContinue) // can be continued
    {
        Debug.Log(rodManager.equippedBucket.Fishes.Count);
        timeManager.PauseTime();
        postRunPopup.SetFishes(BucketToList(rodManager.equippedBucket));
        postRunPopup.Show(canContinue);
        Debug.Log(rodManager.equippedBucket.Fishes.Count);
    }
    List<Fish> BucketToList(Bucket bucket)
    {
        List<Fish> fishes = new List<Fish>();
        foreach (KeyValuePair<Fish, int> pair in bucket.Fishes)
        {

            fishes.AddRange(Enumerable.Repeat(pair.Key, pair.Value));
        }
        foreach (Fish fish in fishes)
        {
            Debug.Log(fish.Name);
        }
        return fishes;
    }
    public void ContinueRun()
    {
        rodManager.equippedRod.Restart();
        timeManager.StartTime();
    }
    public void EndRun() // actual end of run
    {
        //Animation first here
        Debug.Log(rodManager.equippedBucket.Fishes.Count);
        timeManager.RestartTime();
        itemManager.shop.AddBalance(rodManager.equippedBucket.EndRun());
        itemManager.UpdateBalanceUI();
        SetState(PlayerState.Shop);
        rodManager.equippedRod.RodState = RodRegistry.RodState.PreCast;
    }
}
