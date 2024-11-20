using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Bucket
{
    private Dictionary<Fish, int> Fishes;
    public float TotalWeight { get; private set; } = 0;
    public float MaxWeight { get; private set; } = 100;
    public bool AddFish(Fish fish)
    {
        if (TotalWeight > MaxWeight) { return false; }
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
        foreach (KeyValuePair<Fish, int> pair in Fishes)
        {
            Fish fish = pair.Key;
            moneySum += fish.Weight * fish.PricePerKg;
        }
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
    public float EndRun()
    {
        float total = CountMoney();
        Reset();
        return total;
    }
}
public static class BucketRegistry
{
    public enum Tier
    {
        Basic,
        Super,
        Ultimate
    }
    public static Dictionary<Tier, Bucket> Buckets = new(){
        {Tier.Basic,new Bucket(50)},
        {Tier.Super,new Bucket(280)},
        {Tier.Ultimate,new Bucket(5000)},
    };

}