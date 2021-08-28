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

        [SerializeField]
        private SO_StatTemplates statTemplates;
        public SO_StatTemplates.StatTemplate stats;

        // Start is called before the first frame update
        void Start()
        {
            if (!IsLocalPlayer)
            {
                GetComponentInChildren<CameraParent>().gameObject.SetActive(false);
            }
            else
            {
                inputManager = GetComponent<PlayerInputManager>();
                movementManager = GetComponent<PlayerMovementManager>();
                animationManager = GetComponentInChildren<PlayerAnimationManager>();

                SO_StatTemplates.StatTemplate stats = statTemplates.StatTemplates[0];
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

