using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using Cinemachine;



namespace JAM
{
    public class PlayerManager : NetworkBehaviour
    {
        private PlayerInputManager inputManager;
        private PlayerMovementManager movementManager;
        private PlayerAnimationManager animationManager;

        // Start is called before the first frame update
        void Start()
        {
            if (!IsLocalPlayer)
            {
                GetComponentInChildren<CinemachineFreeLook>().gameObject.SetActive(false);
            }
            else
            {
                inputManager = GetComponent<PlayerInputManager>();
                movementManager = GetComponent<PlayerMovementManager>();
                animationManager = GetComponent<PlayerAnimationManager>();
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsLocalPlayer) return;

            inputManager.HandleAllInputs();
            movementManager.HandleAllMovement();
            animationManager.HandleAnimations();


        }
    }
}

