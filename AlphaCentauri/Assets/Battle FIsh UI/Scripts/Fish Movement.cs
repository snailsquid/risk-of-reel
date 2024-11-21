using System.Collections;
using System.Collections.Generic;
using static FishMovement.FishEmotion;
using UnityEngine;
using Unity.VisualScripting;
public class FishMovement : MonoBehaviour
{

    public float maxX = 250f;
    public float minX = -250f;
    public float moveSpeed = 250f;
    public float changeFrequency = 0.01f;
    public float targetPosition;
    float strength = 1f;
    bool Movingup = true;
    float mood;
    [SerializeField] Transform gameManager;
    Rod rod;
    public enum FishEmotion { Neutral, Angry, Tired }
    FishEmotion fishEmotion;
    Dictionary<FishEmotion, float> fishSpeed = new Dictionary<FishEmotion, float>(){
        {Neutral, 1f},
        {Angry, 2f},
        {Tired, 0.5f}
    };
    bool fishing = true;
    bool Stateloop;
    void Start()
    {
        rod = gameManager.GetComponent<RodManager>().equippedRod;
        RectTransform RectParent = transform.parent.GetComponent<RectTransform>();
        RectTransform Rect = transform.GetComponent<RectTransform>();
        maxX = RectParent.rect.width / 2 - Rect.rect.width / 2;
        minX = -maxX;
        print(maxX);
        targetPosition = Random.Range(minX, maxX);
        mood = 1f;
        Stateloop = true;
        //Neutral = true;

    }
    void Update()
    {
        if (rod.IsFishBite)
        {

            //Move fish to targetPosition
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(targetPosition, transform.localPosition.y, transform.localPosition.z), moveSpeed * mood * Time.deltaTime);
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
        fishSpeed[Neutral] = Random.Range(3, 8) / strength;
        yield return new WaitForSeconds(fishSpeed[Neutral]);
        if (fishEmotion == Neutral)
        {
            mood = 2f;
            Debug.Log("To Angry");
            Stateloop = true;
            fishEmotion = Angry;
        }
    }
    IEnumerator WaitAngr()
    {
        fishSpeed[Angry] = Random.Range(2, 5) / strength;
        yield return new WaitForSeconds(fishSpeed[Angry]);
        if (fishEmotion == Angry)
        {
            mood = 0.5f;
            Debug.Log("To Tired");
            Stateloop = true;
            fishEmotion = Tired;
        }
    }
    IEnumerator WaitTired()
    {
        fishSpeed[Tired] = Mathf.Abs(fishSpeed[Neutral] - fishSpeed[Angry]);
        yield return new WaitForSeconds(fishSpeed[Tired]);
        if (fishEmotion == Tired)
        {
            mood = 1f;
            Debug.Log("To Neutral");
            Stateloop = true;
            fishEmotion = Neutral;
        }
    }
}
