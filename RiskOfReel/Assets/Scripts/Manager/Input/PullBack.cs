using System;
using UnityEngine;

namespace Manager.Input
{
    public class PullBack : IInputType
    {
        private Vector2 _startPosition;
        public Vector2 PullBackDistance;
        private bool _isPulling = false;
        
        public void StartTracking(Vector2 position)
        {
            if (_isPulling) return;
            Debug.Log("Pullback starting");
            _isPulling = true;
            _startPosition = position;
        }


        public Vector2 EndTracking(Vector2 position)
        {
            return EndTracking(position, null);
        }
        public Vector2 EndTracking(Vector2 position, Action callback)
        {
            if (!_isPulling) return Vector2.zero;
            _isPulling = false;
            PullBackDistance = position - _startPosition;
            Debug.Log("Pullback : " + PullBackDistance);
            callback?.Invoke();
            
            return(PullBackDistance);
        }
    }
}