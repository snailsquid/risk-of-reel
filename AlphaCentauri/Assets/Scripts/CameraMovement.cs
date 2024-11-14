using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensx;
    public float sensy;
    public Transform orientation;
    float rotationx;
    float rotationy;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mousex = Input.GetAxisRaw("Mouse X")* Time.deltaTime * sensx;
        float mousey = Input.GetAxisRaw("Mouse Y")* Time.deltaTime * sensy;
        rotationy += mousex;
        rotationx -= mousey;
        rotationx = Mathf.Clamp(rotationx,-90f,90f);

        transform.rotation = Quaternion.Euler(rotationx,rotationy,0);
        orientation.rotation = Quaternion.Euler(0,rotationy,0);
    }
}