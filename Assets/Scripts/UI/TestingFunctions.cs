using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TestingFunctions : MonoBehaviour
{
    /*
        Testing screens
         - test loading screen
     */
    
   
    ASyncLoader loaderScript;
    public float loadDuration = 1f;
    private float loadProgress = 0f;

    void Start()
    {
        loaderScript = GetComponent<ASyncLoader>();

        // Start the simulated loading process
        StartCoroutine(SimulateLoading());
    }

    private System.Collections.IEnumerator SimulateLoading()
    {
        float elapsedTime = 0f;
        loadProgress = 0f;        

       // StartCoroutine(loaderScript.LoadLevelASync("Loading Screen"));

        while (elapsedTime < loadDuration)
        {
            elapsedTime += Time.deltaTime;

            // Randomly determine how much progress this frame will simulate
            float targetProgress = loadProgress + Random.Range(0.05f, 0.1f);
            targetProgress = Mathf.Clamp(targetProgress, 0f, 1f);

            // Smoothly interpolate to the target progress
            while (loadProgress < targetProgress)
            {
                loadProgress += Time.deltaTime / loadDuration; // Smooth progress increment
                loaderScript.LoadingBar.fillAmount = loadProgress;

                yield return null; // Wait for the next frame
            }

            // Optional: Small delay between increments for slight pauses
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
       
    }
}

