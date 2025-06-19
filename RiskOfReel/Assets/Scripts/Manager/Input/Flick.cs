using System;
using UnityEngine;

namespace Manager.Input
{
    public class Flick : IInputType
    {
        [Header("Flick")]
        [SerializeField] private float minVelocity;
        [SerializeField] private float minTime;
        [SerializeField] private bool isTrackingFlick = false;
        [SerializeField] private Vector2 startPosition = Vector2.zero;
        [SerializeField] private float startTime = 0f;
        [SerializeField] private Vector2 deltaPosition = Vector2.zero;
        [SerializeField] private float deltaTime = 0f;
        [SerializeField] private Vector2 velocity = Vector2.zero;
        
        public static event Action<Vector2> OnFlick;

        public void StartTracking(Vector2 position)
        {
            if (isTrackingFlick) return;
            Debug.Log("Flick starting");
            isTrackingFlick = true;
            startPosition = position;
            startTime = Time.time;
        }

        public Vector2 EndTracking(Vector2 position)
        {
            if (!isTrackingFlick) return Vector2.zero;
            deltaPosition = position - startPosition;
            deltaTime = Time.time - startTime;        
            velocity = deltaPosition / deltaTime;

            isTrackingFlick = false;
            Debug.Log("Flick : "  + velocity);
            if (velocity.magnitude > minVelocity && velocity.y > 0 && deltaTime > minTime)
            {
                return(velocity);
            }
            return Vector2.zero;
        }
    }
}