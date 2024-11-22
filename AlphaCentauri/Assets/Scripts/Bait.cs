using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FishGenerator;

public class Bait
{
    public string Name { get; private set; }
    public BaitChance BaitChance { get; private set; }
    public Bait(string name, Dictionary<FishTemplate, float> fishChances)
    {
        Name = name;
        BaitChance = new BaitChance(fishChances);
    }
}

public class BaitChance
{
    readonly List<FishTemplate> fishTemplates = new();
    public float TotalChance { get; private set; }
    public Dictionary<FishTemplate, float> FishChances { get; private set; } = new();
    public BaitChance(Dictionary<FishTemplate, float> fishChances, Dictionary<Fish.FishRarity, float> fishRarityChances) : this(RarityToBaitChance(fishChances, fishRarityChances))
    {
    }
    static Dictionary<FishTemplate, float> RarityToBaitChance(Dictionary<FishTemplate, float> fishChances, Dictionary<Fish.FishRarity, float> fishRarityChances)
    {
        Dictionary<FishTemplate, float> clonedFishChances = fishChances;
        foreach (var (key, fishTemplate) in FishTemplates)
        {
            if (fishRarityChances.ContainsKey(fishTemplate.Rarity))
            {
                clonedFishChances[fishTemplate] = fishRarityChances[fishTemplate.Rarity];
            }
        }
        return clonedFishChances;
    }
    public BaitChance(Dictionary<FishTemplate, float> fishChances)
    {
        foreach (var (key, fishTemplate) in FishTemplates)
        {

            fishTemplates.Add(fishTemplate);
        }
        foreach (FishTemplate fishTemplate in fishTemplates)
        {
            if (fishChances.ContainsKey(fishTemplate))
            {
                this.FishChances.Add(fishTemplate, fishChances[fishTemplate]);
                TotalChance += fishChances[fishTemplate];
            }
            else
            {
                this.FishChances.Add(fishTemplate, 0);
            }
        }
    }
}

public class BaitRegistry
{
    public enum BaitType
    {
        None,
        Pellet,
        CacingTanah,
        Jangkrik,
        DagingCincang,
        BeefWellington,
        Mackarel,
        Crab,
    }
    public static Dictionary<BaitType, Bait> Baits { get; private set; } = new Dictionary<BaitType, Bait>(){
        {BaitType.None,new Bait("None", new Dictionary<FishTemplate, float>(){
            {FishTemplates[FishType.Bass], 0},
            {FishTemplates[FishType.Salmon],20}
        })},
        {BaitType.CacingTanah,new Bait("wormy", new Dictionary<FishTemplate, float>(){
            {FishTemplates[FishType.Bass], 0},
            {FishTemplates[FishType.Salmon],20}
        })},
        {BaitType.Mackarel,new Bait("MACKAREL OMG", new Dictionary<FishTemplate, float>(){
            {FishTemplates[FishType.Bass], 0},
            {FishTemplates[FishType.Salmon],20}
        })},
    };
    public BaitRegistry()
    {

    }
}