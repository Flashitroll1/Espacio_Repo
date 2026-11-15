using System.Collections;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [Header("Rotation Parameters")]
    public float speedX = 10f;
    public float speedY = 20f;
    public float speedZ = 30f;

    [Header("Respawn Settings")]
    public float respawnTime = 8f; // Tiempo que tarda en volver a aparecer

    private MeshRenderer meshRenderer;
    private Collider col;

    void Start()
    {
        // Guardamos referencias a los componentes visuales y de colisión para apagarlos/encenderlos
        meshRenderer = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
    }

    void Update()
    {
        RotateObject();
    }

    void RotateObject()
    {
        transform.Rotate(Vector3.right * speedX * Time.deltaTime);
        transform.Rotate(Vector3.up * speedY * Time.deltaTime);
        transform.Rotate(Vector3.forward * speedZ * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // 1. Rellenar el oxígeno al máximo
                player.currentOxygen = player.maxOxygen;

                // TODO: Aquí puedes reproducir un sonido de "pickup" si quieres

                // 2. Iniciar el proceso de desaparición y reaparición
                StartCoroutine(RespawnRoutine());
            }
        }
    }

    IEnumerator RespawnRoutine()
    {
        // Desactivamos el dibujo (modelo 3D) y el collider para que no se pueda volver a coger instantáneamente
        if (meshRenderer != null) meshRenderer.enabled = false;
        if (col != null) col.enabled = false;

        // Esperamos los segundos indicados (8 segundos)
        yield return new WaitForSeconds(respawnTime);

        // Volvemos a activar el dibujo y el collider para que reaparezca
        if (meshRenderer != null) meshRenderer.enabled = true;
        if (col != null) col.enabled = true;
    }
}