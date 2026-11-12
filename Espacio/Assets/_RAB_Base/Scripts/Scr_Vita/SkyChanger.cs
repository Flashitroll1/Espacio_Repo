using UnityEngine;

public class SkyChanger : MonoBehaviour

{
    public GameObject[] snowflakes;
    public Material nihgtSkyBox;
    public Light sun;
    public float nightLightIntensity = 0.3f;
    public int flakesNeeded = 3;
    private bool isNight = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isNight) return;
        int collected = 0;
        for (int i =0; i< snowflakes.Length; i++)
        {
            if (snowflakes[i] ==null || !snowflakes[i].activeInHierarchy)
            {
                collected++;
            }
        }

        if (collected >= flakesNeeded)
        {

            RenderSettings.skybox = nihgtSkyBox;
            DynamicGI.UpdateEnvironment();
            if (sun != null) sun.intensity = nightLightIntensity;
            isNight = true;



        }
    }
}
