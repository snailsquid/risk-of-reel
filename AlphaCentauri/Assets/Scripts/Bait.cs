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
            {FishTemplates[FishType.Ampas], 55},
            {FishTemplates[FishType.Tuna], 10},
            {FishTemplates[FishType.Carp], 10},
            {FishTemplates[FishType.Catfish], 10},
            {FishTemplates[FishType.Special], 5},
            {FishTemplates[FishType.Salmon],10},
        })},
        {BaitType.Pellet,new Bait("Pellet", new Dictionary<FishTemplate, float>(){
            {FishTemplates[FishType.Carp], 20},
            {FishTemplates[FishType.Trout], 35},
            {FishTemplates[FishType.Special], 5},
            {FishTemplates[FishType.Ampas], 5},
            {FishTemplates[FishType.Salmon],35},
        })},
        {BaitType.CacingTanah,new Bait("CacingTanah", new Dictionary<FishTemplate, float>(){
            {FishTemplates[FishType.Bass], 40},
            {FishTemplates[FishType.Catfish],30},
            {FishTemplates[FishType.Tuna],20},
            {FishTemplates[FishType.Special],5},
            {FishTemplates[FishType.Ampas],5},
        })},
        {BaitType.Jangkrik,new Bait("Jangkrik", new Dictionary<FishTemplate, float>(){
            {FishTemplates[FishType.KingCrab], 35},
            {FishTemplates[FishType.Eel], 35},
            {FishTemplates[FishType.Trout], 10},
            {FishTemplates[FishType.Carp], 10},
            {FishTemplates[FishType.Special],5},
            {FishTemplates[FishType.Ampas],5},
        })},
        {BaitType.DagingCincang,new Bait("DagingCincang", new Dictionary<FishTemplate, float>(){
            {FishTemplates[FishType.Pufferfish], 35},
            {FishTemplates[FishType.Sailfish],35},
            {FishTemplates[FishType.Salmon],10},
            {FishTemplates[FishType.Bass],10},
            {FishTemplates[FishType.Special],5},
            {FishTemplates[FishType.Ampas],5},
        })},
        {BaitType.BeefWellington,new Bait("BeefWellington", new Dictionary<FishTemplate, float>(){
            {FishTemplates[FishType.GreatWhiteShark], 5},
            {FishTemplates[FishType.Sailfish], 45},
            {FishTemplates[FishType.GiantSquid], 10},
            {FishTemplates[FishType.MarianaCrab], 10},
            {FishTemplates[FishType.Salmon], 10},
            {FishTemplates[FishType.Trout], 5},
            {FishTemplates[FishType.Special], 10},
            {FishTemplates[FishType.Ampas], 5},
        })},
        {BaitType.Mackarel,new Bait("Mackarel", new Dictionary<FishTemplate, float>(){
            {FishTemplates[FishType.GiantSquid], 50},
            {FishTemplates[FishType.MantaRay], 20},
            {FishTemplates[FishType.KingCrab], 10},
            {FishTemplates[FishType.Eel], 10},
            {FishTemplates[FishType.Special], 5},
            {FishTemplates[FishType.Ampas], 5},
        })},
        {BaitType.MarianaCrab,new Bait("MarianaCrab", new Dictionary<FishTemplate, float>(){
            {FishTemplates[FishType.MantaRay], 40},
            {FishTemplates[FishType.GiantSquid], 15},
            {FishTemplates[FishType.Sailfish], 15},
            {FishTemplates[FishType.Pufferfish], 10},
            {FishTemplates[FishType.Catfish], 10},
            {FishTemplates[FishType.Special], 5},
            {FishTemplates[FishType.Ampas], 5},
        })},
    };
    public BaitRegistry()
    {

    }
}