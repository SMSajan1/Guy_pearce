using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;          // Center of the arena
    public float distance = 15f;       // Distance from the center
    public float height = 10f;          // Camera height
    public float rotationSpeed = 90f;  // Degrees per second

    private float angle;

    void Update()
    {
        if (target == null)
            return;

        // Keyboard input
        if (Input.GetKey(KeyCode.A))
        {
            angle -= rotationSpeed * Time.deltaTime; // Rotate left
        }
        if (Input.GetKey(KeyCode.D))
        {
            angle += rotationSpeed * Time.deltaTime; // Rotate right
        }

        // Convert angle to radians
        float radians = angle * Mathf.Deg2Rad;

        // Calculate camera position
        float x = Mathf.Cos(radians) * distance;
        float z = Mathf.Sin(radians) * distance;

        Vector3 newPosition = new Vector3(x, height, z) + target.position;

        // Apply position and look at the target
        transform.position = newPosition;
        transform.LookAt(target);
    }
}

