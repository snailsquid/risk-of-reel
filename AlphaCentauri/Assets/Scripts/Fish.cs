using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Fish
{
    public string Name { get; private set; }
    public FishGenerator.FishType fishType { get; private set; }
    public FishRarity Rarity { get; private set; }
    public float Weight { get; private set; }
    public float Length { get; private set; }
    public float Strength { get; private set; }
    public float PricePerKg { get; private set; }
    public Sprite Image { get; private set; }
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
        fishType = fishTemplate.FishType;
        Rarity = fishTemplate.Rarity;
        Weight = GenerateStat(fishTemplate.MaxWeigth, fishTemplate.MinWeigth, time);
        Length = GenerateStat(fishTemplate.MaxLength, fishTemplate.MinLength, time);
        Strength = fishTemplate.Strength;
        Image = fishTemplate.Image;
        PricePerKg = fishTemplate.PricePerKg;
    }
    float GenerateStat(float max, float min, (float current, float max) time)
    {
        return (time.current * (max - min) * Random.Range(1, 4) / 3 / time.max) + min;
    }
}

public class FishTemplate
{
    public string Name { get; private set; }
    public FishGenerator.FishType FishType;
    public Fish.FishRarity Rarity { get; private set; }
    public float MaxWeigth { get; private set; }
    public float MinWeigth { get; private set; }
    public float MaxLength { get; private set; }
    public float MinLength { get; private set; }
    public float Strength { get; private set; }
    public float PricePerKg { get; private set; }
    public Sprite Image { get; private set; }
    public FishTemplate(FishGenerator.FishType fishType, string name, Fish.FishRarity rarity, float maxL, float minL, float maxW, float minW, float strength, float pricePerKg, Sprite image)
    {
        FishType = fishType;
        Name = name;
        Rarity = rarity;
        MaxWeigth = maxW;
        MinWeigth = minW;
        MaxLength = maxL;
        MinLength = minL;
        Strength = strength;
        PricePerKg = pricePerKg;
        Image = image;
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
        {FishType.Salmon, new FishTemplate(FishType.Salmon,"Salmon", Fish.FishRarity.Common, 45, 75, 2.2f, 5, 2, 1200, Resources.Load("Images/Fish/Salmon.png")as Sprite)},
        {FishType.Tuna, new FishTemplate(FishType.Tuna,"Tuna", Fish.FishRarity.Common, 120, 220, 12, 20, 4, 800, Resources.Load("Images/Fish/Tuna.png")as Sprite)},
        {FishType.Bass, new FishTemplate(FishType.Bass,"Bass", Fish.FishRarity.Common, 40, 75, 2, 3.5f, 2, 500, Resources.Load("Images/Fish/Bass.png")as Sprite)},
        {FishType.Trout, new FishTemplate(FishType.Trout,"Trout", Fish.FishRarity.Common, 61, 91, 2.8f, 8, 2, 600, Resources.Load("Images/Fish/Trout.png")as Sprite)},
        {FishType.Catfish, new FishTemplate(FishType.Catfish,"Catfish", Fish.FishRarity.Common, 130, 160, 6, 10, 3, 700, Resources.Load("Images/Fish/CatFish.png")as Sprite)},
        {FishType.Carp, new FishTemplate(FishType.Carp,"Carp", Fish.FishRarity.Common, 45, 95, 6, 14, 3, 6000, Resources.Load("Images/Fish/Carp.png")as Sprite)},
        {FishType.SecretFish, new FishTemplate(FishType.SecretFish,"Secret Fish", Fish.FishRarity.Dev, 0,0,0,0,0, 0, Resources.Load("Images/Fish/Secret Fish.png") as Sprite)}
    };
    public static GameObject GetFishModel(FishType fishType)
    {
        Debug.Log("Models/" + fishType.ToString());
        return Resources.Load("Models/" + fishType.ToString(), typeof(GameObject)) as GameObject;
    }
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