using UnityEngine;

public class CameraLookAtTarget : MonoBehaviour
{
    public Transform target;       // Target to look at
    public Vector3 offset = Vector3.zero; // Optional offset

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 lookPosition = target.position + offset;
        transform.LookAt(lookPosition);
    }
}