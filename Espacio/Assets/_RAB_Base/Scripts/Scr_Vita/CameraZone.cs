using UnityEngine;

public class CameraZone : MonoBehaviour
    
{
    public GameObject zonaCamera;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            zonaCamera.SetActive(true);
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            zonaCamera.SetActive(false);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
