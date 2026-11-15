using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float speed = 5f; // Variable que define la velocidad del player
    public float jumpForce = 5f; // Define la fuerza de salto
    public bool useLinearVelocity = true; // Si es verdadero, usará el movimiento físico por motor de aceleración
    public bool groundCheck; // Bool que determina si el pj puede saltar o no

    [Header("Reference Variables")]
    public Rigidbody playerRb; // Almacén de referencia del rb
    public Vector2 moveInput; // Almacén de los valores de pulsación de movimiento
    public AudioSource sfxSource; // Ref al altavoz del player
    public PlayerInteractor playerInteractor; // Ref al script de interacciones/sonidos

    [HideInInspector] public Transform activeCameraTransform; // Referencia que inyecta el CameraSwitcher

    [Header("Oxygen System")]
    public float maxOxygen = 100f;
    public float currentOxygen;
    public float oxygenDrainRate = 5f; // Lo que baja el oxígeno por segundo
    private TextMeshProUGUI oxygenText;

    void Start()
    {
        currentOxygen = maxOxygen;

        // Búsqueda automática del texto de oxígeno en la UI por su nombre exacto
        GameObject textObj = GameObject.Find("TextoOxigeno");
        if (textObj != null)
        {
            oxygenText = textObj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogWarning("No se encontró el objeto 'TextoOxigeno' en la jerarquía.");
        }
    }

    void Update()
    {
        // 1. Gestión del oxígeno
        if (currentOxygen > 0)
        {
            currentOxygen -= oxygenDrainRate * Time.deltaTime;
            if (currentOxygen <= 0)
            {
                currentOxygen = 0;
            }
        }

        // Actualizar el texto en pantalla
        if (oxygenText != null)
        {
            oxygenText.text = "Oxígeno: " + Mathf.Ceil(currentOxygen).ToString() + "%";
        }
    }

    private void FixedUpdate()
    {
        // FixedUpdate ejecuta todo lo relacionado con el motor de físicas y movimiento original
        PhysicalMove();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundCheck = true; // Al chocar con el suelo se devuelve la posibilidad de saltar
        }
    }

    void PhysicalMove()
    {
        // 1. Cálculo de la dirección adaptada a la cámara activa (para que WASD siga correspondiéndose con la pantalla)
        Vector3 adjustedMoveDir = new Vector3(moveInput.x, 0f, moveInput.y);

        if (activeCameraTransform != null)
        {
            Vector3 camForward = activeCameraTransform.forward;
            Vector3 camRight = activeCameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            adjustedMoveDir = (camForward * moveInput.y + camRight * moveInput.x);
        }

        // 2. Movimiento para una BOLA: Aplicamos fuerza en la dirección para que el Rigidbody ruede de forma natural
        if (useLinearVelocity == true)
        {
            // Mantenemos la velocidad en X y Z pero dejamos que la Y (gravedad/salto) actúe libremente
            playerRb.linearVelocity = new Vector3(adjustedMoveDir.x * speed, playerRb.linearVelocity.y, adjustedMoveDir.z * speed);
        }
        else
        {
            // Aplicamos fuerza de empuje (esto hará que la bola ruede orgánicamente por el suelo debido a la fricción)
            Vector3 forceDir = new Vector3(adjustedMoveDir.x, 0f, adjustedMoveDir.z);
            playerRb.AddForce(forceDir * speed * 10f);
        }

        // NOTA PARA LA BOLA: Al ser una esfera con un Physics Material que tenga fricción, 
        // el propio motor de Rigidbody la hará rodar sola al moverse, ¡sin necesidad de forzar rotaciones artificiales!
    }

    void Jump()
    {
        groundCheck = false; // Al saltar, se quita la posibilidad de volver a saltar hasta tocar suelo
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // Reproducción de sonido segura para Arrays o Listas
        if (sfxSource != null && playerInteractor != null)
        {
            // Comprobamos si hay elementos en el array/lista usando Length y asegurándonos de que no es null
            if (playerInteractor.sfxList != null && playerInteractor.sfxList.Length > 0 && playerInteractor.sfxList[0] != null)
            {
                sfxSource.PlayOneShot(playerInteractor.sfxList[0]);
            }
        }
    }

    #region Input Methods
    public void OnMove(InputAction.CallbackContext context)
    {
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