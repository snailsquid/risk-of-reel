using System.Collections;
using System.Collections.Generic;
using static FishMovement.FishEmotion;
using UnityEngine;
using Unity.VisualScripting;
public class FishMovement : MonoBehaviour
{
    float initialWidth;
    float maxX = 250f;
    float minX = -250f;
    public float moveSpeed = 250f;
    public float changeFrequency = 0.01f;
    public float targetPosition;
    float strength = 1f;
    bool Movingup = true;
    float moodModifier;
    [SerializeField] Transform gameManager;
    Rod rod;
    public enum FishEmotion { Neutral, Angry, Tired }
    FishEmotion fishEmotion;
    Dictionary<FishEmotion, float> fishTime = new Dictionary<FishEmotion, float>(){
        {Neutral, 1f},
        {Angry, 2f},
        {Tired, 0.5f}
    };
    bool fishing = true;
    bool Stateloop;
    ItemManager itemManager;
    void Start()
    {
        itemManager = gameManager.GetComponent<ItemManager>();
        rod = gameManager.GetComponent<RodManager>().equippedRod;
        initialWidth = GetComponent<RectTransform>().sizeDelta.x;
        RectTransform RectParent = transform.parent.GetComponent<RectTransform>();
        RectTransform Rect = transform.GetComponent<RectTransform>();
        maxX = RectParent.rect.width / 2 - Rect.rect.width / 2;
        minX = -maxX;
        targetPosition = Random.Range(minX, maxX);
        moodModifier = 1f;
        Stateloop = true;
        strength = rod.fishAttached.Strength;
        //Neutral = true;

    }
    void Update()
    {
        if (rod.IsFishBite)
        {

            RectTransform rt = GetComponent<RectTransform>();
            float width = initialWidth * ItemRegistry.UpgradeItems[ItemRegistry.UpgradeItemType.Rod].Values[itemManager.shop.UpgradeItems[ItemRegistry.UpgradeItemType.Rod].CurrentLevel];
            rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
            RectTransform RectParent = transform.parent.GetComponent<RectTransform>();
            RectTransform Rect = transform.GetComponent<RectTransform>();
            maxX = RectParent.rect.width / 2 - Rect.rect.width / 2;
            minX = -maxX;
            //Move fish to targetPosition
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(targetPosition, transform.localPosition.y, transform.localPosition.z), moveSpeed * moodModifier * Time.deltaTime);
            //Checking the fish
            if (Mathf.Approximately(transform.localPosition.x, targetPosition))
            {
                //New Place
                targetPosition = Random.Range(minX, maxX);
            }
            //Change direction
            if (Random.value < changeFrequency)
            {
                Movingup = !Movingup;
                targetPosition = Movingup ? maxX : minX;
            }
            if (Stateloop)
            {
                FishMood();
            }
        }
    }
    void FishMood()//State mood
    {
        Stateloop = false;
        if (fishEmotion == Neutral)
        {
            StartCoroutine(WaitNeutral());
        }
        if (fishEmotion == Angry)
        {
            StartCoroutine(WaitAngr());
        }
        if (fishEmotion == Tired)
        {
            StartCoroutine(WaitTired());
        }
    }
    IEnumerator WaitNeutral()
    {
        fishTime[Neutral] = Random.Range(3, 8) / strength;
        yield return new WaitForSeconds(fishTime[Neutral]);
        if (fishEmotion == Neutral)
        {
            moodModifier = 2f;
            Debug.Log("To Angry");
            Stateloop = true;
            fishEmotion = Angry;
        }
    }
    IEnumerator WaitAngr()
    {
        fishTime[Angry] = Random.Range(2, 5) / strength;
        yield return new WaitForSeconds(fishTime[Angry]);
        if (fishEmotion == Angry)
        {
            moodModifier = 0.5f;
            Debug.Log("To Tired");
            Stateloop = true;
            fishEmotion = Tired;
        }
    }
    IEnumerator WaitTired()
    {
        fishTime[Tired] = Mathf.Abs(fishTime[Neutral] - fishTime[Angry]);
        yield return new WaitForSeconds(fishTime[Tired]);
        if (fishEmotion == Tired)
        {
            moodModifier = 1f;
            Debug.Log("To Neutral");
            Stateloop = true;
            fishEmotion = Neutral;
        }
    }
}
