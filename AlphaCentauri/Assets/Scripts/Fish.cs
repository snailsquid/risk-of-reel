using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Fish
{
    public string Name { get; private set; }
    public FishRarity Rarity { get; private set; }
    public float Weight { get; private set; }
    public float Length { get; private set; }
    public float Strength { get; private set; }
    public enum FishRarity
    {
        Common,
        Rare,
        Legendary,
        Misc,
        Dev
    }
    public Fish(FishTemplate fishTemplate)
    {
        Name = fishTemplate.Name;
        Rarity = fishTemplate.Rarity;
        Weight = Random.Range(fishTemplate.MinWeigth, fishTemplate.MaxWeigth);
        Length = Random.Range(fishTemplate.MinLength, fishTemplate.MaxLength);
        Strength = fishTemplate.Strength;
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
    public FishTemplate(string name, Fish.FishRarity rarity, float maxL, float minL, float maxW, float minW, float strength)
    {
        Name = name;
        Rarity = rarity;
        MaxWeigth = maxW;
        MinWeigth = minW;
        MaxLength = maxL;
        MinLength = minL;
        Strength = strength;
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
        {FishType.Salmon, new FishTemplate("Salmon", Fish.FishRarity.Common, 45, 75, 2.2f, 5, 2)},
        {FishType.Tuna, new FishTemplate("Tuna", Fish.FishRarity.Common, 120, 220, 12, 20, 4)},
        {FishType.Bass, new FishTemplate("Bass", Fish.FishRarity.Common, 40, 75, 2, 3.5f, 2)},
        {FishType.Trout, new FishTemplate("Trout", Fish.FishRarity.Common, 61, 91, 2.8f, 8, 2)},
        {FishType.Catfish, new FishTemplate("Catfish", Fish.FishRarity.Common, 130, 160, 6, 10, 3)},
        {FishType.Carp, new FishTemplate("Carp", Fish.FishRarity.Common, 45, 95, 6, 14, 3)},
        {FishType.SecretFish, new FishTemplate("Secret Fish", Fish.FishRarity.Dev, 0,0,0,0,0)}
    };
    public static Fish GenerateFish(Bait bait)
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
                return new Fish(key.Key);
            }
        }
        Debug.Log("Nah couldnt find yer fish mate my bad gng");
        return new Fish(FishTemplates[FishType.SecretFish]);
    }
}