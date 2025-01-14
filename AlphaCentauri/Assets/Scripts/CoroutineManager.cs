using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    bool Pulling;
    bool IsBite;
    float FishTimer = 0f;

    public void StartFishStruggle()
    {
        StartCoroutine(IStartFishStruggle());
    }

    IEnumerator FishingCorountine()
    {
        int wait_time = Random.Range(5, 10);//Wait random time to fish to bite the bait
        yield return new WaitForSeconds(wait_time);
        Debug.Log("Biting");
        StartFishStruggle();
    }
    void StartFishing()
    {
        StartCoroutine(FishingCorountine());
    }
    IEnumerator IStartFishStruggle()
    {
        IsBite = true;
        //wait until pull
        while (!Pulling)
        {
            yield return FishTimer += Time.deltaTime;
            if (FishTimer > 10)
            {
                IsBite = false;
                Debug.Log("The fish go away");
                StartFishing();
                FishTimer = 0f;
                break;
            }
        }
        if (IsBite)
        {
            Debug.Log("Start fish battle");//Start fish battle
            IsBite = false;
            FishTimer = 0f;
        }
    }
}
