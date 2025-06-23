using System;
using UnityEngine;

namespace Manager.Input
{
    public class PullBack : IInputType
    {
        private Vector2 _startPosition;
        public Vector2 PullBackDistance;
        public bool IsPulling = false;
        
        public void StartTracking(Vector2 position)
        {
            if (IsPulling) return;
            Debug.Log("Pullback starting");
            IsPulling = true;
            _startPosition = position;
        }


        public bool TryEndTracking(Vector2 position, out Vector2 pullBackDistance)
        {
            if (!IsPulling)
            {
                pullBackDistance = Vector2.zero; 
                return false;
            }
            IsPulling = false;
            PullBackDistance = position - _startPosition;
            Debug.Log("Pullback : " + PullBackDistance);
            
            pullBackDistance = PullBackDistance;
            return(true);
        }
    }
}