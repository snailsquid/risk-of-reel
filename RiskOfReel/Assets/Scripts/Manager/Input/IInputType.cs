using UnityEngine;

namespace Manager.Input
{
    public interface IInputType
    {
        public void StartTracking(Vector2 position);
        public bool TryEndTracking(Vector2 position, out Vector2 end);
    }
}