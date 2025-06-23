using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] float sensitivity;
    [SerializeField] float verticalMin;
    [SerializeField] float verticalMax;

    private float verticalRotation;
    float origSensitivity;

    public float Sensitivity { get { return sensitivity; } set { sensitivity = value; } }
    public float OrigSensitivity => origSensitivity;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        origSensitivity = sensitivity;

        //GameManager.instance.OnGameStateChange += OnGameStateChange;
    }

    
    void Update()
    {
        //get input
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        //not adding invert
        verticalRotation -= mouseY;

        //clamp x rotation of cam
        verticalRotation = Mathf.Clamp(verticalRotation, verticalMin, verticalMax);
        //rotate cam on x axis
        transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        //rotate player on y-axis
        transform.parent.Rotate(Vector3.up * mouseX);
    }


    //private void OnGameStateChange(GameState newGameState)
    //{
    //    if (newGameState == GameState.Pause)
    //    {
    //        this.enabled = false;
    //    }
    //    else if (newGameState == GameState.Gameplay)
    //    {
    //        this.enabled = true;
    //    }
    //}
    //private void OnDestroy()
    //{
    //    if (GameManager.instance == null)
    //    {
    //        Debug.Log("GameManager is null");
    //    }
    //    if (Camera.main == null)
    //    {
    //        Debug.Log("cameraController is null");
    //    }
    //    // Unsubscribe
    //    GameManager.instance.OnGameStateChange -= OnGameStateChange;
    //}

}
