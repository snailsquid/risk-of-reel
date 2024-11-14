using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookMovement : MonoBehaviour
{
    public float maxY = 250f;
    public float minY = -250f;
    public float moveSpeed = 250f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = Input.GetAxis("Vertical");
        if (moveInput !=0)
        {
            moveHook(moveInput);
        }
    }
    private void moveHook(float moveInput)
    {
        Vector3 movement = Vector3.up * moveInput * moveSpeed * Time.deltaTime;
        Vector3 newPosition = transform.localPosition + movement;
        newPosition.y = Mathf.Clamp(newPosition.y,minY,maxY);
        transform.localPosition = newPosition;
    }
}
