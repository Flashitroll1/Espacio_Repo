using UnityEngine;

public class WindTunnel : MonoBehaviour
{
    [Header("Wind Settings")]
    public float pushStrength = 7f; // Intensidad del empuje del viento

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // El viento empuja hacia donde apunta la flecha azul (Z) del túnel
                Vector3 windDir = transform.forward;

                // Le pasamos la fuerza al PlayerController para que la sume fluidamente
                player.windForce = new Vector3(windDir.x * pushStrength, 0f, windDir.z * pushStrength);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // Al salir del túnel, apagamos el viento
                player.windForce = Vector3.zero;
            }
        }
    }
}