using System.Collections.Generic;
using Mechanic.Items;
using UnityEngine;

namespace Mechanic.Bucket
{
    public class Bucket : MonoBehaviour
    {
        [SerializeField] private BucketData bucketData;
        
        [Header("Information")]
        [SerializeField] private int currentWeight;
        [SerializeField] private int currentSize;

        [SerializeField] private List<Fish.Fish> fishes;

        public bool AddToBucket(Fish.Fish fish)
        {
            if (fish.fishData.weight + currentWeight >= bucketData.weightCapacity)
                return false;
            if (fish.fishData.size + currentSize >= bucketData.sizeCapacity)
                return false;
            
            fishes.Add(fish);
            currentWeight += fish.fishData.weight;
            currentSize += fish.fishData.size;
            return true;
        }
    }
}