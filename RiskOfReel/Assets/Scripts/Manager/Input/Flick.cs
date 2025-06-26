using System;
using UnityEngine;

namespace Manager.Input
{
    public class Flick : IInputType
    {
        [Header("Flick")]
        private bool _isTrackingFlick = false;
        private Vector2 _startPosition = Vector2.zero;
        private float _startTime = 0f;
        private Vector2 _deltaPosition = Vector2.zero;
        private float _deltaTime = 0f;
        private Vector2 _velocity = Vector2.zero;
        
        public static event Action<Vector2> OnFlick;

        public void StartTracking(Vector2 position)
        {
            if (_isTrackingFlick) return;
            Debug.Log("Flick starting");
            _isTrackingFlick = true;
            _startPosition = position;
            _startTime = Time.time;
        }

        public bool TryEndTracking(Vector2 position, out Vector2 velocity)
        {
            if (!_isTrackingFlick)
            {
                velocity = Vector2.zero;
                return false;
            }
            _deltaPosition = position - _startPosition;
            _deltaTime = Time.time - _startTime;        
            _velocity = _deltaPosition / _deltaTime;

            _isTrackingFlick = false;
            Debug.Log("Flick : "  + _velocity);
            if (_velocity.y > 0)
            {
                velocity = _velocity;
                return(true);
            }
            
            Debug.Log("Flick failed");
            velocity = Vector2.zero;
            return false;
        }
    }
}