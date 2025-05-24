using UnityEngine;

[ExecuteAlways]
public class FaceCamera : MonoBehaviour
{

    [Header("Camera Target")]
    public Camera targetCamera;

    [Header("Offset Settings")]
    [Range(-180f, 180f)] public float rotationX = 0f;
    [Range(-180f, 180f)] public float rotationY = 0f;
    [Range(-180f, 180f)] public float rotationZ = 0f;



    void LateUpdate()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
            if (targetCamera == null) return;
        }

        // Make the sprite face the camera
        transform.LookAt(transform.position + targetCamera.transform.forward,
                         targetCamera.transform.up);

        // Apply rotation offset (in local space)
        transform.Rotate(rotationX, rotationY, rotationZ, Space.Self);
    }
}
