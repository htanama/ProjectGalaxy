using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField] private string goingToNextScene;

    // Wait for 3 second to load 
    public float delayTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(goingToNextScene); // Load the next scene.

    }
}
