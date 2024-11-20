using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Build;
using UnityEngine;

public class Fish
{
    public string Name { get; private set; }
    public FishRarity Rarity { get; private set; }
    public float Weight { get; private set; }
    public float Length { get; private set; }
    public float Strength { get; private set; }
    public float PricePerKg { get; private set; }
    public enum FishRarity
    {
        Common,
        Rare,
        Legendary,
        Misc,
        Dev
    }
    public Fish(FishTemplate fishTemplate, (float current, float max) time)
    {
        Name = fishTemplate.Name;
        Rarity = fishTemplate.Rarity;
        Weight = GenerateStat(fishTemplate.MaxWeigth, fishTemplate.MinWeigth, time);
        Length = GenerateStat(fishTemplate.MaxLength, fishTemplate.MinLength, time);
        Strength = fishTemplate.Strength;
    }
    float GenerateStat(float max, float min, (float current, float max) time)
    {
        return (time.current * (max - min) * Random.Range(1, 4) / 3 / time.max) + min;
    }
}
public enum FishTypes
{

}

public class FishTemplate
{
    public string Name { get; private set; }
    public Fish.FishRarity Rarity { get; private set; }
    public float MaxWeigth { get; private set; }
    public float MinWeigth { get; private set; }
    public float MaxLength { get; private set; }
    public float MinLength { get; private set; }
    public float Strength { get; private set; }
    public float PricePerKg { get; private set; }
    public FishTemplate(string name, Fish.FishRarity rarity, float maxL, float minL, float maxW, float minW, float strength, float pricePerKg)
    {
        Name = name;
        Rarity = rarity;
        MaxWeigth = maxW;
        MinWeigth = minW;
        MaxLength = maxL;
        MinLength = minL;
        Strength = strength;
        PricePerKg = pricePerKg;
    }
}

public class FishGenerator
{
    public enum FishType
    {
        SecretFish,
        Salmon,
        Tuna,
        Bass,
        Trout,
        Catfish,
        Carp
    }

    public static readonly Dictionary<FishType, FishTemplate> FishTemplates = new Dictionary<FishType, FishTemplate>{
        {FishType.Salmon, new FishTemplate("Salmon", Fish.FishRarity.Common, 45, 75, 2.2f, 5, 2, 1200)},
        {FishType.Tuna, new FishTemplate("Tuna", Fish.FishRarity.Common, 120, 220, 12, 20, 4, 800)},
        {FishType.Bass, new FishTemplate("Bass", Fish.FishRarity.Common, 40, 75, 2, 3.5f, 2, 500)},
        {FishType.Trout, new FishTemplate("Trout", Fish.FishRarity.Common, 61, 91, 2.8f, 8, 2, 600)},
        {FishType.Catfish, new FishTemplate("Catfish", Fish.FishRarity.Common, 130, 160, 6, 10, 3, 700)},
        {FishType.Carp, new FishTemplate("Carp", Fish.FishRarity.Common, 45, 95, 6, 14, 3, 6000)},
        {FishType.SecretFish, new FishTemplate("Secret Fish", Fish.FishRarity.Dev, 0,0,0,0,0, 0)}
    };
    public static Fish GenerateFish(Bait bait, (float current, float max) time)
    {
        Dictionary<FishTemplate, float> fishChances = bait.BaitChance.FishChances;
        float totalchance = bait.BaitChance.TotalChance;
        float rng = Random.Range(0, totalchance);
        float sum = 0;
        foreach (KeyValuePair<FishTemplate, float> key in fishChances)
        {
            sum += key.Value;
            if (rng <= sum)
            {
                return new Fish(key.Key, time);
            }
        }
        Debug.Log("Nah couldnt find yer fish mate my bad gng");
        return new Fish(FishTemplates[FishType.SecretFish], time);
    }
}