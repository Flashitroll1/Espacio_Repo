using System.Collections;
using UnityEngine;
using TMPro;

public class SnowmanTalk : MonoBehaviour
{
    public GameObject snowmanCard;  
    public float showTime = 4f;     
    public GameObject hideAfter;    
    public GameObject showAfter;    

    private bool alreadyShown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !alreadyShown)
        {
            alreadyShown = true;
            StartCoroutine(Talk());
        }
    }

    IEnumerator Talk()
    {
        snowmanCard.SetActive(true);

        TextMeshProUGUI cardText = snowmanCard.GetComponentInChildren<TextMeshProUGUI>();
        cardText.ForceMeshUpdate();
        int total = cardText.textInfo.characterCount;
        for (int i = 0; i <= total; i++)
        {
            cardText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(showTime);

        snowmanCard.SetActive(false);

        if (hideAfter != null) hideAfter.SetActive(false);  
        if (showAfter != null) showAfter.SetActive(true);   
    }
}