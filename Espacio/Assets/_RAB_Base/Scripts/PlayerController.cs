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

    public float trampolineForce = 12f; // Define la fuerza del trampolín en el nivel de bosque
    [HideInInspector] public Vector3 windForce = Vector3.zero; // Fuerza de viento actual

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
                RespawnPlayer(); // ¡Si el oxígeno llega a 0, reaparece automáticamente!
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
        if (collision.gameObject.CompareTag("Trampoline"))
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                // Comprueba que el contacto sea desde arriba.
                if (collision.GetContact(i).normal.y > 0.5f)
                {
                    groundCheck = false;

                    // Elimina la velocidad vertical previa,
                    // conservando el movimiento horizontal.
                    Vector3 velocity = playerRb.linearVelocity;
                    velocity.y = 0f;
                    playerRb.linearVelocity = velocity;

                    // Aplica el impulso del trampolín.
                    playerRb.AddForce(
                        Vector3.up * trampolineForce,
                        ForceMode.Impulse
                    );

                    return;
                }
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            groundCheck = true;
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
            // Mantenemos el movimiento normal y le sumamos la fuerza del viento
            playerRb.linearVelocity = new Vector3(
                (adjustedMoveDir.x * speed) + windForce.x,
                playerRb.linearVelocity.y,
                (adjustedMoveDir.z * speed) + windForce.z
            );
        }
    }

    void Jump()
    {
        groundCheck = false; // Al saltar, se quita la posibilidad de volver a saltar hasta tocar suelo
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // Reproducción de sonido segura para Arrays o Listas
        if (sfxSource != null && playerInteractor != null)
        {
            if (playerInteractor.sfxList != null && playerInteractor.sfxList.Length > 0 && playerInteractor.sfxList[0] != null)
            {
                sfxSource.PlayOneShot(playerInteractor.sfxList[0]);
            }
        }
    }

    // --- SISTEMA DE REAPARICIÓN (CHECKPOINT MÁS CERCANO Y CÁMARA) ---
    public void RespawnPlayer()
    {
        // Buscamos todos los objetos de la escena que tengan asignado el Tag "Respawn"
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");

        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("¡Cuidado! No hay ningún objeto con el Tag 'Respawn' en la escena.");
            return;
        }

        // Encontramos el spawnpoint más cercano a la posición actual del jugador
        Transform closestSpawn = spawnPoints[0].transform;
        float minDistance = Vector3.Distance(transform.position, closestSpawn.position);

        foreach (GameObject sp in spawnPoints)
        {
            float distance = Vector3.Distance(transform.position, sp.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestSpawn = sp.transform;
            }
        }

        // 1. Teletransportamos al jugador al checkpoint más cercano
        transform.position = closestSpawn.position;

        // 2. Gestionamos la cámara asociada a este checkpoint si tiene el script Checkpoint.cs
        Checkpoint cpScript = closestSpawn.GetComponent<Checkpoint>();
        if (cpScript != null && cpScript.cameraForThisCheckpoint != null)
        {
            cpScript.cameraForThisCheckpoint.SetActive(true);
            activeCameraTransform = cpScript.cameraForThisCheckpoint.transform;
        }

        // 3. Limpiamos las velocidades físicas para que no conserve inercias raras al reaparecer
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }

        // 4. Rellenamos el oxígeno al máximo
        currentOxygen = maxOxygen;

        Debug.Log(">>> ¡Jugador reaparecido y cámara actualizada en el checkpoint más cercano!");
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