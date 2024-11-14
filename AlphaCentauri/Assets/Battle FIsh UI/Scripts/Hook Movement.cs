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
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            moveHook(1);
        }
        else
        {
            moveHook(-1);
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
