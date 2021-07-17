using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    CameraManager cameraManager;
    PlayerMovement playerMovement;
    private void Awake() 
    {       
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerMovement = GetComponent<PlayerMovement>();
        
    }

    void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate() 
    {
        playerMovement.HandleAllMovement();        
    }

    private void LateUpdate() 
    {
        cameraManager.HandleAllCameraMovement();        
    }
}
