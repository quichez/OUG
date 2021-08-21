using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using Cinemachine;
using System;
using Quichez;

namespace JAM
{
    public class PlayerManager : NetworkBehaviour
    {
        private PlayerInputManager inputManager;
        private PlayerMovementManager movementManager;
        private PlayerAnimationManager animationManager;
        private HealthBar healthBar;

        private HealthSystem healthSystem;
      
        public void OnEnable()
        {
            if (healthSystem == null)
                healthSystem = new HealthSystem(100, 50, 1, 1.0f);

            healthSystem.Current.OnValueChanged += OnHealthChanged;
        }

        public void OnDisable()
        {
            healthSystem.Current.OnValueChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int previousValue, int newValue)
        {
            if(IsOwner /*&& IsClient*/)
            {
                StartCoroutine(healthSystem.Regenerate(Time.deltaTime * 2.0f));
                healthBar.UpdateHealthBar(healthSystem);
            }
        }


        // Start is called before the first frame update
        void Start()
        {           
            if (!IsLocalPlayer)
            {
                GetComponentInChildren<CameraParent>().gameObject.SetActive(false);

            }
            else
            {
                healthBar = FindObjectOfType<HealthBar>();
                healthBar.UpdateHealthBar(healthSystem);               
                inputManager = GetComponent<PlayerInputManager>();
                movementManager = GetComponent<PlayerMovementManager>();
                animationManager = GetComponentInChildren<PlayerAnimationManager>();
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

