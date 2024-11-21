using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField] Transform debugObject;
    [SerializeField] bool debug = false;
    [SerializeField] float timeToBob = 7.5f, damping = 0.4f, freq = 0.4f, multiplier = 2f;
    public bool IsTouchingWater { get; private set; } = false;
    Rigidbody rigidBody;
    float yTouchWater = 0;
    float x = 0;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "water" && !IsTouchingWater)
        {
            Debug.Log("touched water");
            IsTouchingWater = true;
            rigidBody = transform.GetComponent<Rigidbody>();
            yTouchWater = rigidBody.position.y;
            rigidBody.velocity = Vector3.zero;
            rigidBody.useGravity = false;
        };
    }
    public static double BobEase(double t, double damping = 0.4f, double frequency = 2.0)
    {

        // Apply the bobbing formula
        return Math.Exp(-damping * t) * Math.Sin(2 * Math.PI * frequency * t) * -1;
    }
    float deltaTime = 0;
    void Update()
    {
        if (debug)
        {
            Transform clone = Instantiate(debugObject);
            clone.position = transform.position;
        }
        if (IsTouchingWater && deltaTime < timeToBob)
        {
            deltaTime += Time.deltaTime;
            float x = (float)BobEase(deltaTime, damping, freq) * multiplier;
            rigidBody.position = new Vector3(rigidBody.position.x, yTouchWater + x, rigidBody.position.z);
        }
    }
}
