using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderPickup : MonoBehaviour
{
    [Header("Level Settings")]
    public string nextSceneName; // Nombre exacto de la siguiente escena

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("¡Atención! Falta escribir el nombre de la siguiente escena en el Inspector.");
            }
        }
    }
}