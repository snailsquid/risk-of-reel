using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    Bucket currentBucket;
    void Start()
    {
        currentBucket = BucketRegistry.Buckets[BucketRegistry.Rarity.Basic];
    }
}
