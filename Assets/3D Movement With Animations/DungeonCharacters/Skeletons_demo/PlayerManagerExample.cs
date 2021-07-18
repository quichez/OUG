using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class PlayerManagerExample : NetworkBehaviour
{
    InputManager inputManager;
    SceneManager cameraManager;
    PlayeLocomotion playerMovement;

    private void Awake() 
    {       
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<SceneManager>();
        playerMovement = GetComponent<PlayeLocomotion>();     
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
