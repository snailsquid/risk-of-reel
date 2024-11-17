using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rod
{
    public RodType RodType { get; private set; }
    public RodRarity RodRarity { get; private set; }
    public Bait Bait { get; set; }
    public Rod(RodType rodType, RodRarity rodRarity, Bait bait)
    {
        RodType = rodType;
        RodRarity = rodRarity;
        Bait = bait;
    }
}
public enum RodRarity
{
    Basic,
    Super,
    Ultimate
}

public enum RodType
{
    FishingRod1,
    FishingRod2
}