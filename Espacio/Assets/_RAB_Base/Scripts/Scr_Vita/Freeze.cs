using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;

public class Freeze : MonoBehaviour
    
{
    public int timeFreeze = 3; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))

        {
             Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            StartCoroutine(FreezePlayer(rb) );
        }

       

    }

    IEnumerator FreezePlayer(Rigidbody rb)

    {
        
    RigidbodyConstraints after = rb.constraints; //Guarda como estaban los datos de player
    rb.constraints = RigidbodyConstraints.FreezeAll; // Congela todos ejes. Alien queda quieto
    yield return new WaitForSeconds(timeFreeze);// Espera los segundos
    rb.constraints = after; // Se vuelve como antes

    }




}
