using System;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UI
{
    [RequireComponent(typeof(UIBehaviour))]
    public class UIRegistrar : MonoBehaviour
    {
        [SerializeField]
        private List<CentralStateManager.GameState> gameStates = new List<CentralStateManager.GameState>();

        [SerializeField] private bool animateBetweenSelectedStates = false;
        [SerializeField]
        private List<Behaviour> uis = new List<Behaviour>();

        private void SetUI()
        {
            foreach (var behaviour in GetComponents<Behaviour>())
            {
                if (behaviour is Canvas or UIBehaviour)
                {
                    uis.Add(behaviour);
                } 
            }
        }

        private void Reset()
        {
            #if UNITY_EDITOR
            SetUI();
            #endif
        }

        private void Awake()
        {
            if (uis == null)
            {
                Debug.LogWarning("UIRegistrar: No UI component assigned!");
                return;
            }

            // UIManager.Instance.RegisterUI(new RegisteredUIElement(gameStates, uis, animateBetweenSelectedStates));
        }
    }
}