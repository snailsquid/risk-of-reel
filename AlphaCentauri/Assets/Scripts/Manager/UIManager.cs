using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
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

        [SerializedDictionary("Game State", "UIs")] public SerializedDictionary<CentralStateManager.GameState, List<Transform>> registeredUI =
            new SerializedDictionary<CentralStateManager.GameState, List<Transform>>();
        private void Start()
        {
            _centralStateManager = CentralStateManager.Instance;
        }

        private void ToggleUIs(CentralStateManager.GameState gameState, bool toggle)
        {
            foreach (var uis in registeredUI[gameState])
            {
                uis.Toggle(toggle);
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
