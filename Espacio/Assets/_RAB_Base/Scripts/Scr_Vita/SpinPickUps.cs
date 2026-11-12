using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class SpinPickUps : MonoBehaviour

{
    public float spinSpeed = 90f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.25f;
    private Vector3 startPos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(0, 90 * Time.deltaTime, 0);
        float y = Mathf.PingPong(Time.time * bobSpeed, bobHeight);
        transform.position = startPos + Vector3.up * y;

    }
}
