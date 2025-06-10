using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Manager
{
    public class UIManager : MonoBehaviour
    {
        #region Singleton
        public static UIManager Instance { get; set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this.gameObject);
            else
                Instance = this;
        }
        #endregion

        private CentralStateManager _centralStateManager;
        [FormerlySerializedAs("_currentGameState")] [SerializeField]
        private CentralStateManager.GameState currentGameState = CentralStateManager.GameState.StartMenu;
        private readonly Dictionary<CentralStateManager.GameState, List<Canvas>> _registeredUI = new Dictionary<CentralStateManager.GameState, List<Canvas>>();
        private void Start()
        {
            _centralStateManager = CentralStateManager.Instance;
        }

        public void RegisterUI(Canvas uiBehaviour, CentralStateManager.GameState gameState)
        {
            if (!_registeredUI.ContainsKey(gameState))
            {
                _registeredUI.Add(gameState, new List<Canvas>());
            }

            if (!_registeredUI[gameState].Contains(uiBehaviour))
            {
                _registeredUI[gameState].Add(uiBehaviour);
            }
        }

        private void ToggleUIs(CentralStateManager.GameState gameState, bool toggle)
        {
            if (!_registeredUI.TryGetValue(gameState, out var value)) return; // If it doesn't exist return
            foreach (var uiBehaviour in value)
            {
                Debug.Log("Toggling UI " + uiBehaviour.GetType().Name);
                uiBehaviour.Toggle(toggle);
            }
        }
        public void UpdateUI(Action between = null)
        {
            ToggleUIs(currentGameState, false);
            between?.Invoke();
            currentGameState = _centralStateManager.CurrentGameState;
            ToggleUIs(currentGameState, true);
        }
    }
}
