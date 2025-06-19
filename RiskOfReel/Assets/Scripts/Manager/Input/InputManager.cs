using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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

        public static event Action OnPointerDown;
        public static event Action OnPointerUp;
        public static event Action<Vector2> OnFlick;
        
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
        }

        [Header("Position")]
        [SerializeField] private Vector2 currentPosition = Vector2.zero;
        
        [Header("Pointer Down")]
        [SerializeField] private bool isPointerDown;

        private void RegisterPointerData()
        {
            currentPosition = _pointerPositionAction.ReadValue<Vector2>();
        }

        private void HandlePointerDown(bool value)
        {
            isPointerDown = value;
            if(isPointerDown)
                OnPointerDown?.Invoke();
            else 
                OnPointerUp?.Invoke();
        }
        
    }
}