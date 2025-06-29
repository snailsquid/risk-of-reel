using UnityEngine;

namespace Utils
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // The static instance of the singleton.
    private static T _instance;

    // A lock object for thread-safe access in multithreaded scenarios.
    private static readonly object _lock = new object();

    /// <summary>
    /// Gets the singleton instance. If it doesn't exist, it tries to find it in the scene.
    /// This property is thread-safe.
    /// </summary>
    public static T Instance
    {
        get
        {
            // Lock the access to ensure thread safety.
            lock (_lock)
            {
                // If the instance is already set, return it.
                if (_instance != null)
                {
                    return _instance;
                }

                // If not, try to find an existing instance in the scene.
                // This is useful for when the instance already exists but the static field hasn't been set yet.
                _instance = (T)FindObjectOfType(typeof(T));

                // If an instance is still not found, it means it's not in the scene.
                // We could create one, but it's often better to ensure it's placed manually.
                // Here, we'll log an error to alert the developer.
                if (_instance == null)
                {
                    Debug.LogError($"An instance of {typeof(T)} is needed in the scene, but it was not found.");
                }

                return _instance;
            }
        }
    }

    /// <summary>
    /// The Awake method is called when the script instance is being loaded.
    /// This is where the singleton logic is enforced.
    /// </summary>
    protected virtual void Awake()
    {
        // Check if an instance already exists.
        if (_instance == null)
        {
            // If not, this instance becomes the singleton instance.
            _instance = this as T;

            // Make this GameObject persist across scene loads.
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            // If an instance already exists and it's not this one,
            // then this is a duplicate. Destroy this duplicate GameObject.
            Debug.LogWarning($"Duplicate instance of {typeof(T)} found. Destroying the new one.");
            Destroy(gameObject);
        }
    }
}
}