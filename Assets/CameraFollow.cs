using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 6f;
    public float height = 3f;
    public float smoothSpeed = 8f;

    void LateUpdate()
    {
        if (target == null) return;

        // Always stay behind the character's back
        Vector3 desiredPosition = target.position
                                - target.forward * distance
                                + Vector3.up * height;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        // Always look at the character's center
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}