using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // <--- Imprescindible para TextMeshPro

public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float speed; //Variable que define la velocidad del player
    public float jumpForce; //Define la fuerza de salto
    public bool useLinearVelocity = true; //Si es verdadero, usará el movimiento físico por motor de aceleración, si no, el de fuerza de empuje
    public bool groundCheck; //Bool que determina si el pj puede saltar o no (cuando está en el suelo puede)
    public Transform activeCameraTransform;

    [Header("Reference Variables")]
    public Rigidbody playerRb; //Almacén de referencia del rb, una vez relleno permite modificar sus variables internas
    public Vector2 moveInput; //Almacén de los valores de pulsación de movimiento (0, 1, -1)
    public AudioSource sfxSource; //Ref al altavoz del player, para reproducir sonidos
    public PlayerInteractor playerInteractor; //Ref al script, permite acceder a su lista de sonidos públicos


    [Header("Oxygen System")]
    public float maxOxygen = 100f;
    public float currentOxygen;
    public float oxygenDrainRate = 5f;

    [Header("UI References")]
    public GameObject oxygenTextObj; // Arrastras aquí el objeto del texto desde Unity
    private TMPro.TextMeshProUGUI oxygenText; // El componente se pillará solo

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        currentOxygen = maxOxygen;
        if (oxygenTextObj != null)
        {
            oxygenText = oxygenTextObj.GetComponent<TMPro.TextMeshProUGUI>();
        }
        // ... lo que ya tuvieras en tu Start ...

        // Busca automáticamente el objeto de texto en la escena por su nombre
        GameObject textObj = GameObject.Find("OXIGENO");
        if (textObj != null)
        {
            oxygenText = textObj.GetComponent<TMPro.TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("No se encuentra ningun objeto con el nombre OXIGENO en la jerarquia.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentOxygen > 0)
        {
            currentOxygen -= oxygenDrainRate * Time.deltaTime;

            // Aquí puedes poner lo que pasa si llega a 0 (morir, reiniciar nivel, etc.)
            if (currentOxygen <= 0)
            {
                currentOxygen = 0;
                // TODO: Lógica de muerte o ahogamiento
            }
        }
        // Actualizamos el texto en pantalla si está asignado
        if (oxygenText != null)
        {
            // Math.FToInteger o ToString("F0") para que no salgan muchos decimales molestos
            oxygenText.text = "Oxígeno: " + Mathf.Ceil(currentOxygen).ToString() + "%";
        }
        if (oxygenText != null)
        {
            oxygenText.text = "Oxígeno: " + Mathf.Ceil(currentOxygen).ToString() + "%";
        }
    }

    private void FixedUpdate()
    {
        //FixedUpdate ejecuta todo lo que tenga que ver con el motor de físicas
        PhysicalMove();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundCheck = true; //Al chocar con el suelo se devuelve la posibilidad de saltar
        }
    }

    void CinematicMove()
    {
        //Se usa transform.Translate para moverse (dirección * velocidad)
        transform.Translate(Vector3.forward * speed * moveInput.y * Time.deltaTime);
        transform.Translate(Vector3.right * speed * moveInput.x * Time.deltaTime);
    }

    void PhysicalMove()
    {
        // Obtenemos la orientación de la cámara principal activa en este momento
        Transform camTransform = activeCameraTransform != null ? activeCameraTransform : Camera.main.transform;

        Vector3 moveDirection = Vector3.forward * moveInput.y + Vector3.right * moveInput.x;

        if (camTransform != null)
        {
            // Cogemos el frente y la derecha de la cámara, pero anulamos el eje Y (altura) 
            // para que el personaje se mueva plano por el suelo
            Vector3 camForward = camTransform.forward;
            Vector3 camRight = camTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            // Calculamos la dirección final basada en la perspectiva de la cámara
            moveDirection = (camForward * moveInput.y + camRight * moveInput.x);
        }

        if (useLinearVelocity == true)
        {
            // Aplicamos la velocidad adaptada a la cámara
            playerRb.linearVelocity = new Vector3(moveDirection.x * speed, playerRb.linearVelocity.y, moveDirection.z * speed);
        }
        else
        {
            // Si usas fuerzas de empuje
            playerRb.AddForce(moveDirection * speed);
        }
        if (camTransform != null)
        {
            Debug.Log("Moviéndome según la cámara: " + camTransform.name);
        }
        else
        {
            Debug.LogWarning("¡No hay ninguna cámara con el Tag MainCamera activa!");
        }
    }

    void Jump()
    {
        groundCheck = false; //Al saltar, se quita la posibilidad de volver a saltar hasta tocar suelo
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //Reproducir sonido de salto: accede al player interactor y busca el sonido exacto en la lista
        sfxSource.PlayOneShot(playerInteractor.sfxList[0]);
    }

    #region Input Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        //Se rellena la variable de referencia con el valor de cualquier contexto
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (groundCheck == true)
            {
                Jump();
            }
        }
    }
    #endregion
}