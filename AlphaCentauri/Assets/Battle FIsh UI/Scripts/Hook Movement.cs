using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookMovement : MonoBehaviour
{
    public float maxX = 470;
    public float minX = -470;
    public float moveSpeed = 250f;
    bool battling = true;
    void Start()
    {

    }
    void Update()
    {
        if (battling)
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
    }
    private void moveHook(float moveInput)
    {
        Vector3 movement = Vector3.right * moveInput * moveSpeed * Time.deltaTime;
        Vector3 newPosition = transform.localPosition + movement;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        transform.localPosition = newPosition;
    }
}
