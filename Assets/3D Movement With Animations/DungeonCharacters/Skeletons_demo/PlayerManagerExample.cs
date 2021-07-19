using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class PlayerManagerExample : NetworkBehaviour
{
    InputManager inputManager;
    CameraManager cameraManager;
    PlayerLocomotion playerMovement;

    private void Awake() 
    {       
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerMovement = GetComponent<PlayerLocomotion>();     
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
