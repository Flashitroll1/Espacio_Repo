using UnityEngine;

public class PickUpSimple : MonoBehaviour
{
    public float respawnTime = 8f;

    private MeshRenderer myRenderer;
    private Collider myCollider;
    private bool isCollected = false;
    private float timer = 0f;

    void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
        myCollider = GetComponent<Collider>();

        // Si el MeshRenderer está en un hijo, lo buscamos automáticamente
        if (myRenderer == null)
        {
            myRenderer = GetComponentInChildren<MeshRenderer>();
        }
    }

    void Update()
    {
        if (isCollected)
        {
            timer += Time.deltaTime;
            if (timer >= respawnTime)
            {
                Respawn();
            }
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
        isCollected = false;

        if (myRenderer != null) myRenderer.enabled = true;
        if (myCollider != null) myCollider.enabled = true;

        Debug.Log(">>> ¡PickUp reaparecido por narices!");
    }
}