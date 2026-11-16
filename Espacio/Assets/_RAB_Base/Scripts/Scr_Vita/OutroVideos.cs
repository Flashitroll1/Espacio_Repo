using UnityEngine;
using UnityEngine.Video;

public class OutroVideo : MonoBehaviour
{
    public VideoPlayer video;         
    public GameObject victoryPanel;   

    void Start()
    {
        Time.timeScale = 1f;
        if (video == null || video.clip == null)
        {
            victoryPanel.SetActive(true);
            return;
        }
        victoryPanel.SetActive(false);
        video.loopPointReached += OnVideoEnd;
        video.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        victoryPanel.SetActive(true);
    }
}