using UnityEngine;

public class PickUpSimple : MonoBehaviour
{
    [Header("Respawn Settings")]
    public float respawnTime = 8f;

    [Header("Animation Settings")]
    public float rotationSpeed = 50f;      // Velocidad de giro
    public float floatSpeed = 3f;          // Velocidad del movimiento de subida y bajada
    public float floatAmplitude = 0.25f;   // Distancia máxima que sube y baja (en metros)

    private MeshRenderer myRenderer;
    private Collider myCollider;
    private bool isCollected = false;
    private float timer = 0f;

    private Vector3 startPosition; // Posición base para calcular el vaivén

    void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
        myCollider = GetComponent<Collider>();

        if (myRenderer == null)
        {
            myRenderer = GetComponentInChildren<MeshRenderer>();
        }

        // Guardamos la posición inicial donde nació el objeto
        startPosition = transform.position;
    }

    void Update()
    {
        if (isCollected)
        {
            // Cuenta atrás para el respawn
            timer += Time.deltaTime;
            if (timer >= respawnTime)
            {
                Respawn();
            }
        }
        else
        {
            // 1. Rotación continua sobre su propio eje Y (puedes cambiar el eje si prefieres)
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

            // 2. Movimiento suave de flotación arriba y abajo usando una onda Seno
            float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            // Rellenar oxígeno
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.currentOxygen = player.maxOxygen;
            }

            // Ocultar y desactivar
            isCollected = true;
            timer = 0f;

            if (myCollider != null) myCollider.enabled = false;
            if (myRenderer != null) myRenderer.enabled = false;

            Debug.Log(">>> PickUp cogido. Crono a 0.");
        }
    }

    void Respawn()
    {
        // Devolvemos el objeto a su posición inicial exacta antes de encenderlo
        transform.position = startPosition;

        isCollected = false;

        if (myRenderer != null) myRenderer.enabled = true;
        if (myCollider != null) myCollider.enabled = true;

        Debug.Log(">>> ¡PickUp reaparecido por narices!");
    }
}