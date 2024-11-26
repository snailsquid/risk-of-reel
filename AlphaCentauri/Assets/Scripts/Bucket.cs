using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;

public class Bucket
{
    public Dictionary<Fish, int> Fishes { get; private set; }
    public float TotalWeight { get; private set; } = 0;
    public float MaxWeight { get; private set; } = 100;
    public bool AddFish(Fish fish)
    {
        if (TotalWeight + fish.Weight > MaxWeight) { return false; }
        return false;
        if (Fishes.ContainsKey(fish))
        {
            Fishes[fish] += 1;
            TotalWeight += fish.Weight;
        }
        else
        {
            Fishes.Add(fish, 1);
            TotalWeight += fish.Weight;
        }
        return true;
    }
    float CountMoney()
    {
        float moneySum = 0;
        Debug.Log(Fishes.Count);
        foreach (KeyValuePair<Fish, int> pair in Fishes)
        {
            Fish fish = pair.Key;
            Debug.Log(fish.Weight + "kg " + fish.PricePerKg + "/kg " + pair.Value);
            moneySum += fish.Weight * fish.PricePerKg * pair.Value;
        }
        Debug.Log("money sum " + moneySum);
        return moneySum;
    }
    void Reset()
    {
        TotalWeight = 0f;
        Fishes = new();
    }
    public Bucket(float maxWeight)
    {
        MaxWeight = maxWeight;
        Fishes = new();
    }
    public int EndRun()
    {
        int total = (int)CountMoney();
        Reset();
        Debug.Log("total " + total);
        return total;
    }
}
public static class BucketRegistry
{

}