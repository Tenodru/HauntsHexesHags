using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 cameraOffset;               // Offset of the camera from the target
    public Transform cameraFollowTransform;    // The target the camera should follow
    public float dampX = 0.15f;                // Damping for the X-axis movement
    public float dampY = 0.15f;                // Damping for the Y-axis movement

    private Vector3 velocity = Vector3.zero;   // Reference velocity for smooth damp

    // Update is called once per frame
    void LateUpdate()
    {
        if (cameraFollowTransform != null)
        {
            Vector3 targetPosition = cameraFollowTransform.position + cameraOffset;

            float posX = Mathf.SmoothDamp(transform.position.x, targetPosition.x, ref velocity.x, dampX);
            float posY = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref velocity.y, dampY);

            transform.position = new Vector3(posX, posY, transform.position.z);
        }
    }
    private void OnValidate()
    {
        Vector3 targetPosition = cameraFollowTransform.position + cameraOffset;

        float posX = Mathf.SmoothDamp(transform.position.x, targetPosition.x, ref velocity.x, dampX);
        float posY = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref velocity.y, dampY);

        transform.position = new Vector3(posX, posY, transform.position.z);
    }
}
