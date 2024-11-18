using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float maxX = 250f;
    public float minX = -250f;
    public float moveSpeed = 250f;
    public float changeFrequency = 0.01f;
    public float targetPosition;
    bool Movingup = true;
    float mood;
    bool Neutral = true;
    bool Angry = false;
    bool Tired = false;
    float Ang;
    float Neu;
    float Tir;
    bool fishing = true;
    bool Stateloop;
    void Start()
    {
        targetPosition = Random.Range(minX,maxX);
        mood = 1f;
        Stateloop = true;
        //Neutral = true;
        
    }
    void Update()
    {
        //Move fish to targetPosition
        transform.localPosition = Vector3.MoveTowards(transform.localPosition,new Vector3(targetPosition,transform.localPosition.y,transform.localPosition.z),moveSpeed * mood * Time.deltaTime);
        //Checking the fish
        if (Mathf.Approximately(transform.localPosition.x,targetPosition))
        {
            //New Place
            targetPosition = Random.Range(minX,maxX);
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
    void FishMood()//State mood
    {
        Stateloop=false;
        if(Neutral)
        {
            StartCoroutine(WaitNeutral());
        }
        if(Angry)
        {
            StartCoroutine(WaitAngr());
        }
        if(Tired)
        {
            StartCoroutine(WaitTired());
        }
    }
    IEnumerator WaitNeutral()
    {
        Neu = Random.Range(3,8);
        yield return new WaitForSeconds(Neu);
        if(Neutral && !Angry && !Tired)
        {
            mood = 2f;
            Debug.Log("To Angry");
            Stateloop=true;
            ToAngry();
        }
    }
    IEnumerator WaitAngr()
    {
        Ang = Random.Range(2,5);
        yield return new WaitForSeconds(Ang);
        if (Angry && !Neutral && !Tired)
        {
            mood = 0.5f;
            Debug.Log("To Tired");
            Stateloop=true;
            ToTired();
        }
    }
    IEnumerator WaitTired()
    {
        Tir = Mathf.Abs(Neu-Ang);
        yield return new WaitForSeconds(Tir);
        if (Tired && !Neutral && !Angry)
        {
            mood = 1f;
            Debug.Log("To Neutral");
            Stateloop=true;
            ToNeutral();
        }
    }
    void ToNeutral()
    {
        Tired = false;
        Neutral = true;
        Angry=false;
    }
    void ToAngry()
    {
        Neutral = false;
        Angry = true;
        Tired = false;
    }
    void ToTired()
    {
        Angry = false;
        Tired = true;
        Neutral=false;
    }
}
