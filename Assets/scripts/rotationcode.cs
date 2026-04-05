using UnityEngine;

public class RoomRotationTrigger : MonoBehaviour
{
    [SerializeField] private Transform room;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger hit by: " + other.name); // Debug line

        if (other.CompareTag("Player"))
        {
            if (room != null)
            {
                room.Rotate(0f, 0f, -180f); // Instant rotation
                Debug.Log("Room rotated!");
            }
            else
            {
                Debug.LogWarning("Room not assigned in the Inspector!");
            }
        }
    }
}
