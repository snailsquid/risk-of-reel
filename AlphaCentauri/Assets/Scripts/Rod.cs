using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rod
{
    enum RodType
    {
        FishingRod1,
        FishingRod2
    }
    enum RodRarity
    {
        Basic,
        Super,
        Ultimate
    }
    RodType rodType;
    RodRarity rodRarity;
}
