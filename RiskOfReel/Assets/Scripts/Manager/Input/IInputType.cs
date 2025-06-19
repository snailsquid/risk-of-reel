using UnityEngine;

namespace Manager.Input
{
    public interface IInputState
    {
        public void StartTracking(Vector2 position);
        public Vector2 EndTracking(Vector2 position);
    }
}