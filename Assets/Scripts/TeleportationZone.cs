using UnityEngine;

public class TeleportationZone : MonoBehaviour
{
    public Transform targetPosition;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            rb.MovePosition(targetPosition.position);
        }
        else
        {
            other.transform.position = targetPosition.position;
        }
    }
}
