using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering; //Para efectos
public class Freeze : MonoBehaviour

    
{
    public int timeFreeze = 3;

    public GameObject helloCard;
    public Volume freezeVolume; //Vintage
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
        if (helloCard != null) helloCard.SetActive(true); //Muestra el cartel 
        if (freezeVolume != null) freezeVolume.weight = 1;
    yield return new WaitForSeconds(timeFreeze);// Espera los segundos
        if (helloCard != null) helloCard.SetActive(false);//Quita el cartel
        helloCard = null;
        if (freezeVolume != null) freezeVolume.weight = 0;
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Se vuelve como antes
        
    }




}
