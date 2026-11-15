using UnityEngine;

public class DoorUnlocker : MonoBehaviour
{
    [Header("Unlock Settings")]
    public int requiredConchas = 5; // Cuántas conchas necesitas para abrir el muro
    public GameObject wallToOpen;    // El objeto del muro que quieres que desaparezca

    void Update()
    {
        // Comprobamos en cada fotograma si el jugador ha alcanzado las conchas necesarias
        if (PickUpCounterItem.totalCollected >= requiredConchas)
        {
            UnlockWall();
        }
    }

    void UnlockWall()
    {
        if (wallToOpen != null)
        {
            Debug.Log(">>> ¡Muro abierto! Conchas suficientes.");

            // Desactivamos el muro (o puedes usar Destroy(wallToOpen) si prefieres borrarlo)
            wallToOpen.SetActive(false);

            // Desactivamos este propio script para que deje de comprobarse en el Update
            enabled = false;
        }
    }
}