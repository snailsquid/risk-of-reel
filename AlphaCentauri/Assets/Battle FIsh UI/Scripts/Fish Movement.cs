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
    public bool Movingup = true;
    public float mood;
    public bool Neutral = true;
    public bool Angry = false;
    public bool Tired = false;
    float Ang;
    float Neu;
    float Tir;
    bool fishing = true;
    void Start()
    {
        targetPosition = Random.Range(minX,maxX);
        mood = 1f;
        //Neutral = true;
        FishMood();
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
    }
    void FishMood()
    {
        while (fishing)
        {
            if(Neutral && !Angry && !Tired)
            {
                mood = 1f;
                StartCoroutine(WaitNeutral());
            }
            if (Angry && !Neutral && !Tired)
            {
                mood = 2f;
                StartCoroutine(WaitAngr());
            }
            if (Tired && !Neutral && !Angry)
            {
                mood = 0.5f;
                StartCoroutine(WaitTired());
            }
        }
    }
    IEnumerator WaitNeutral()
    {
        int Neu = Random.Range(3,8);
        yield return new WaitForSeconds(Neu);
        if(Neutral && !Angry && !Tired)
        {
            Debug.Log("Neutral");
            ToAngry();
        }
    }
    IEnumerator WaitAngr()
    {
        int Ang = Random.Range(2,5);
        yield return new WaitForSeconds(Ang);
        if (Angry && !Neutral && !Tired)
        {
            Debug.Log("Angry");
            ToTired();
        }
    }
    IEnumerator WaitTired()
    {
        int Tir = Random.Range(5,10);
        yield return new WaitForSeconds(Tir);
        if (Tired && !Neutral && !Angry)
        {
            Debug.Log("Tired");
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
