using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cámaras")]
    public GameObject cameraToTurnOn;  // Esta casilla indica la cámara que se debe encender al cruzar
    public GameObject cameraToTurnOff; // Y esta la cámara anterior que se debe apagar

    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si el que cruza la línea es el jugador
        if (other.CompareTag("Player"))
        {
            // Enciende la nueva cámara y apaga la vieja
            if (cameraToTurnOn != null) cameraToTurnOn.SetActive(true);
            if (cameraToTurnOff != null) cameraToTurnOff.SetActive(false);
        }
    }
}