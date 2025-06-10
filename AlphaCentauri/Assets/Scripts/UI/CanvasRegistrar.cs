using Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(UIBehaviour))]
    public class CanvasRegistrar : MonoBehaviour
    {
        [SerializeField]
        private CentralStateManager.GameState gameState;

        private Canvas _ui;

        private void Awake()
        {
            _ui = GetComponent<Canvas>();
            UIManager.Instance.RegisterUI(_ui, gameState);
        }
    }
}