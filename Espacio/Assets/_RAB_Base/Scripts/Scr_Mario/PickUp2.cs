using UnityEngine;
using TMPro;

public class PickUpCounterItem : MonoBehaviour
{
    [Header("Animation Settings")]
    public float rotationSpeed = 50f;
    public float floatSpeed = 3f;
    public float floatAmplitude = 0.25f;

    [Header("Counter Settings")]
    public string counterItemName = "Conchas"; // Nombre que aparecerá en el texto

    // Contador estático compartido para que todas las conchas sumen al mismo marcador
    public static int totalCollected = 0;
    private static TextMeshProUGUI counterText;

    private MeshRenderer myRenderer;
    private Collider myCollider;
    private bool isCollected = false;
    private Vector3 startPosition;

    void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
        myCollider = GetComponent<Collider>();

        if (myRenderer == null)
        {
            myRenderer = GetComponentInChildren<MeshRenderer>();
        }

        startPosition = transform.position;

        // Buscamos automáticamente el texto del contador en la UI
        if (counterText == null)
        {
            GameObject textObj = GameObject.Find("TextoContador");
            if (textObj != null)
            {
                counterText = textObj.GetComponent<TextMeshProUGUI>();
                UpdateCounterUI();
            }
        }
    }

    void Update()
    {
        if (isCollected) return;

        // 1. Rotación y vaivén
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;

            // Sumamos 1 al contador global de conchas
            totalCollected++;
            UpdateCounterUI();

            // Desactivamos gráficos y colisión
            if (myCollider != null) myCollider.enabled = false;
            if (myRenderer != null) myRenderer.enabled = false;

            Debug.Log(">>> Concha recogida. Total: " + totalCollected);

            // Destruimos el objeto de la escena
            Destroy(gameObject, 0.2f);
        }
    }

    void UpdateCounterUI()
    {
        if (counterText != null)
        {
            counterText.text = counterItemName + ": " + totalCollected;
        }
    }
}