using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Manager
{
    public class InputManager : MonoBehaviour
    {
        #region Singleton
        public static InputManager Instance { get; set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this.gameObject);
            else
                Instance = this;
        }
        #endregion

        public static event Action OnMouseDown;
        public static event Action OnMouseUp;
        public static event Action<Vector2> OnFlick;
        
        private Vector2 lastPosition;
        private Vector2 lastVelocity;
        private Vector2 Acceleration;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleMouseDown();
            }
            
        }

        private void HandleFlick()
        {
            Vector2 position = Input.mousePosition;
            Vector2 velocity = (lastPosition - position)/Time.deltaTime;
            Acceleration = (velocity-lastVelocity)/Time.deltaTime;
            
            lastPosition = position;
            lastVelocity = velocity;
            OnFlick?.Invoke(Acceleration);
        }
        private void HandleMouseDown()
        {
            HandleFlick();
            OnMouseDown?.Invoke();
        }
    }
}