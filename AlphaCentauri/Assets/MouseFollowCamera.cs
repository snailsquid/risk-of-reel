using UnityEngine;

public class MouseRotateCamera : MonoBehaviour
{
    [SerializeField] private float maxRotationAngle = 10f; // Maximum angle the camera can rotate
    [SerializeField] private float rotationSpeed = 5f;     // Speed of rotation smoothing

    private Quaternion initialRotation;
    public bool isAble { get; private set; } = true;

    void Start()
    {
        // Store the initial rotation of the camera
        initialRotation = transform.rotation;
    }

    public void SetAble(bool able, Vector3 rotation)
    {
        initialRotation = Quaternion.Euler(rotation);
        isAble = able;
    }
    void Update()
    {
        if (!isAble) { return; }
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Normalize mouse position to -1 to 1 range relative to the screen center
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float normalizedX = (mousePosition.x / screenWidth - 0.5f) * 2f;
        float normalizedY = (mousePosition.y / screenHeight - 0.5f) * 2f;

        // Calculate target rotation angles based on normalized mouse position
        float targetRotationX = -normalizedY * maxRotationAngle; // Invert Y for correct pitch direction
        float targetRotationY = normalizedX * maxRotationAngle;

        // Create the target rotation
        Quaternion targetRotation = Quaternion.Euler(targetRotationX, targetRotationY, 0f);

        // Smoothly rotate the camera
        transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation * targetRotation, Time.deltaTime * rotationSpeed);
    }
}
