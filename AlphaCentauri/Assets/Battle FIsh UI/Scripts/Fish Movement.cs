using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float maxY = 250f;
    public float minY = -250f;
    public float moveSpeed = 250f;
    public float changeFrequency = 0.01f;
    public float targetPosition;
    public bool Movingup = true;
    void Start()
    {
        targetPosition = Random.Range(minY,maxY);
    }
    void Update()
    {
        //Move fish to targetPosition
        transform.localPosition = Vector3.MoveTowards(transform.localPosition,new Vector3(transform.localPosition.x,targetPosition,transform.localPosition.z),moveSpeed * Time.deltaTime);
        //Checking the fish
        if (Mathf.Approximately(targetPosition,transform.localPosition.y))
        {
            //New Place
            targetPosition = Random.Range(minY,maxY);
        }
        //Change direction
        if (Random.value < changeFrequency)
        {
            Movingup = !Movingup;
            targetPosition = Movingup ? maxY : minY;
        }
    }
}
