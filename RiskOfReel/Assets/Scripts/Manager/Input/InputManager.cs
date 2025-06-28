using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Manager.Input
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

        public static event Action<bool> OnPointerPress;
        public static event Action OnPointerMove;
        
        [Header("Input")]
        [SerializeField] private InputActionAsset gamePlayInputAsset;
        private InputAction _pointerPositionAction;
        private InputAction _pointerPressAction;

        private void OnEnable()
        {
            _pointerPositionAction = gamePlayInputAsset["PointerPosition"];
            _pointerPressAction = gamePlayInputAsset["PrimaryContact"];
            _pointerPositionAction.Enable();
            _pointerPressAction.Enable();
            Debug.Log("Actions "+ _pointerPressAction);
            _pointerPressAction.performed += ctx => HandlePointerDown(true);
            _pointerPressAction.canceled += ctx => HandlePointerDown(false);
        }

        private void OnDisable()
        {
            _pointerPositionAction.Disable();
            _pointerPressAction.Disable();
        }

        private void Update()
        {
            RegisterPointerData();
            if(currentPosition != previousPosition)
                OnPointerMove?.Invoke();
        }

        [Header("Position")]
        public Vector2 currentPosition = Vector2.zero;
        public Vector2 previousPosition = Vector2.zero;
        
        [Header("Pointer Down")]
        [SerializeField] private bool isPointerDown;

        private void RegisterPointerData()
        {
            previousPosition = currentPosition;
            currentPosition = _pointerPositionAction.ReadValue<Vector2>();
        }

        private void HandlePointerDown(bool value)
        {
            isPointerDown = value;
            OnPointerPress?.Invoke(value);
        }
        
        
    }
}