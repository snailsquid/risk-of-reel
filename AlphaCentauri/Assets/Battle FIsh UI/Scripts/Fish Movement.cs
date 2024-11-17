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
    public bool Neutral;
    public bool Angry;
    public bool Tired;
    float Ang;
    float Neu;
    float Tir;
    void Start()
    {
        targetPosition = Random.Range(minX,maxX);
        Neutral = true;
        Angry = false;
        Tired = false;
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
        if(Neutral)
        {
            mood = 1f;
            StartCoroutine(WaitNeutral);
            Neutral = false;
            Angry = true;
        }
        else if (Angry)
        {
            mood = 2f;
            StartCoroutine(WaitAngr);
            Angry = false;
            Tired = true;
        }
        else if (Tired)
        {
            mood = 0.5f;
            StartCoroutine(WaitTired);
            Tired = false;
            Neutral = true;
        }
    }
    IEnumerator WaitNeutral()
    {
        Neu = Random.Range(3,8);
        yield return new WaitForSeconds(Neu);
    }
    IEnumerator WaitAngr()
    {
        Ang = Random.Range(2,5);
        yield return new WaitForSeconds(Ang);
    }
    IEnumerator WaitTired()
    {
        Tir = Math.Abs(Neu-Ang);
        yield return new WaitForSeconds(Tir);
    }
}
