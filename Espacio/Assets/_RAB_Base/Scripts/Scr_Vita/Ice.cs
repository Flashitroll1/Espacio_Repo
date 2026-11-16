using UnityEngine;

public class IceMovement : MonoBehaviour
{
    public float force = 12f;         

    private PlayerController pc;
    private Rigidbody rb;

    void Start()
    {
        pc = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        pc.useLinearVelocity = false;  
    }

    void FixedUpdate()
    {
        // leo las teclas desde el PlayerController y empujo con fuerza = resbala
        Vector3 dir = new Vector3(pc.moveInput.x, 0f, pc.moveInput.y);
        rb.AddForce(dir * force);
    }
}