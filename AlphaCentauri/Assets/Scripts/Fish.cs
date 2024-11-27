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
        Legendary,
        Ampas,
        Special,
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
        return Mathf.Round(((time.current * (max - min) * Random.Range(1, 4) / 3 / time.max) + min) * 100) / 100f;
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
        Carp,
        KingCrab,
        Eel,
        Pufferfish,
        Sailfish,
        GreatWhiteShark,
        MantaRay,
        GiantSquid
    }

    public static readonly Dictionary<FishType, FishTemplate> FishTemplates = new Dictionary<FishType, FishTemplate>{
        {FishType.Salmon, new FishTemplate(FishType.Salmon,"Salmon", Fish.FishRarity.Common, 45, 75, 2.2f, 5, 2, 1200, Resources.Load("Images/Fish/Salmon.png")as Sprite)},
        {FishType.Tuna, new FishTemplate(FishType.Tuna,"Tuna", Fish.FishRarity.Common, 120, 220, 12, 20, 4, 800, Resources.Load("Images/Fish/Tuna.png")as Sprite)},
        {FishType.Bass, new FishTemplate(FishType.Bass,"Bass", Fish.FishRarity.Common, 40, 75, 2, 3.5f, 2, 500, Resources.Load("Images/Fish/Bass.png")as Sprite)},
        {FishType.Trout, new FishTemplate(FishType.Trout,"Trout", Fish.FishRarity.Common, 61, 91, 2.8f, 8, 2, 600, Resources.Load("Images/Fish/Trout.png")as Sprite)},
        {FishType.Catfish, new FishTemplate(FishType.Catfish,"Catfish", Fish.FishRarity.Common, 130, 160, 6, 10, 3, 700, Resources.Load("Images/Fish/CatFish.png")as Sprite)},
        {FishType.Carp, new FishTemplate(FishType.Carp,"Carp", Fish.FishRarity.Common, 45, 95, 6, 14, 3, 6000, Resources.Load("Images/Fish/Carp.png")as Sprite)},
        {FishType.KingCrab, new FishTemplate(FishType.KingCrab,"KingCrab", Fish.FishRarity.Special, 150, 200, 1.8f, 2, 60, 6000, Resources.Load("Images/Fish/KingCrab.png")as Sprite)},
        {FishType.Eel, new FishTemplate(FishType.Eel,"Eel", Fish.FishRarity.Special, 90, 150, 4.5f, 6.6f, 35, 2000, Resources.Load("Images/Fish/Eel.png")as Sprite)},
        {FishType.Pufferfish, new FishTemplate(FishType.Pufferfish,"Pufferfish", Fish.FishRarity.Special, 80, 120, 1.6f, 2.2f, 45, 9000, Resources.Load("Images/Fish/Pufferfish.png")as Sprite)},
        {FishType.Sailfish, new FishTemplate(FishType.Sailfish,"Sailfish", Fish.FishRarity.Special, 300, 400, 185, 225, 100, 500, Resources.Load("Images/Fish/Sailfish.png")as Sprite)},
        {FishType.GreatWhiteShark, new FishTemplate(FishType.GreatWhiteShark,"GreatWhiteShark", Fish.FishRarity.Legendary, 500, 600, 1800, 2000, 250, 750, Resources.Load("Images/Fish/GreatWhiteShark.png")as Sprite)},
        {FishType.MantaRay, new FishTemplate(FishType.MantaRay,"MantaRay", Fish.FishRarity.Legendary, 630, 700, 900, 1000, 180, 350, Resources.Load("Images/Fish/MantaRay.png")as Sprite)},
        {FishType.GiantSquid, new FishTemplate(FishType.GiantSquid,"GiantSquid", Fish.FishRarity.Legendary, 900, 1000, 200, 275, 150, 700, Resources.Load("Images/Fish/GiantSquid.png")as Sprite)},
        {FishType.SecretFish, new FishTemplate(FishType.SecretFish,"Secret Fish", Fish.FishRarity.Dev, 0,0,0,0,0, 0, Resources.Load("Images/Fish/Secret Fish.png") as Sprite)}
    };
    public static GameObject GetFishModel(FishType fishType)
    {
        Debug.Log("Models/" + fishType.ToString());
        return Resources.Load("Models/" + fishType.ToString(), typeof(GameObject)) as GameObject;
    }
    public static Fish GenerateFish(BaitRegistry.BaitType bait, (float current, float max) time)
    {
        BaitChance baitChance = BaitRegistry.Baits[bait].BaitChance;
        Dictionary<FishTemplate, float> fishChances = baitChance.FishChances;
        float totalchance = baitChance.TotalChance;
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