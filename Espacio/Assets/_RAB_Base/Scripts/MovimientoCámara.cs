using UnityEngine;

public class MovimientoCámara : MonoBehaviour
{
    public float velocidad = 5f;

    // Update is called once per frame
    void Update()
    {
        float movX = Input.GetAxis("Horizontal") * velocidad * Time.deltaTime;
        float movZ = Input.GetAxis("Vertical") * velocidad * Time.deltaTime;

        transform.Translate(movX, 0, movZ);
    }
}
