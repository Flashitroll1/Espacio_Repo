using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cámaras")]
    public GameObject cameraToTurnOn;  // Cámara que se debe encender al cruzar
    public GameObject cameraToTurnOff; // Cámara anterior que se debe apagar

    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si el que cruza la línea es el jugador
        if (other.CompareTag("Player"))
        {
            // 1. Enciende la nueva cámara si está asignada
            if (cameraToTurnOn != null)
            {
                cameraToTurnOn.SetActive(true);

                // Avisa al script del jugador pasándole la nueva cámara activa
                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.activeCameraTransform = cameraToTurnOn.transform;
                }
            }

            // 2. Apaga la cámara vieja si está asignada
            if (cameraToTurnOff != null)
            {
                cameraToTurnOff.SetActive(false);
            }
        }
    }
}