using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // Forzamos la reaparición instantánea al tocar la zona de caída
                player.RespawnPlayer();
                Debug.Log(">>> ¡Te has caído al vacío! Reapareciendo...");
            }
        }
    }
}
