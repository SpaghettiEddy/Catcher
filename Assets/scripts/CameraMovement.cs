using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target; // The player or object to follow
    public float smoothing = 5f; // How smoothly the camera follows the target

    private Vector3 offset; // The initial offset between the camera and the player

    void Start()
    {
        // Calculate the initial offset based on the current position of the camera and the target
        offset = transform.position - target.position;
    }

    void FixedUpdate()
    {
        // Target position for the camera (player's position + offset)
        Vector3 targetCamPos = target.position + offset;

        // Smoothly interpolate between the camera's current position and the target position
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.fixedDeltaTime);
    }

    void LateUpdate()
    {
        // Directly update the camera position to follow the player
        transform.position = target.position + offset;
    }
}
