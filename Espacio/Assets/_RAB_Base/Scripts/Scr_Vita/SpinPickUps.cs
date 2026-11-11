using JetBrains.Annotations;
using UnityEngine;

public class SpinPickUps : MonoBehaviour

{
    public float spinSpeed = 90f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(0, 90 * Time.deltaTime, 0);

    }
}
