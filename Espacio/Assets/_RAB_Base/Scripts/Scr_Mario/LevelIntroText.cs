using UnityEngine;
using TMPro;
using System.Collections;

public class LevelIntroText : MonoBehaviour
{
    [Header("Settings")]
    public float displayDuration = 10f; 
    private TextMeshProUGUI textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();

        if (textMesh != null)
        {
            StartCoroutine(HideTextRoutine());
        }
    }

    IEnumerator HideTextRoutine()
    {
        yield return new WaitForSeconds(displayDuration);
        textMesh.enabled = false;
    }
}