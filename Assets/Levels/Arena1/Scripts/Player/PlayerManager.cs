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

        [SerializeField]
        private HealthSystem healthSystem;

        public void OnEnable()
        {
            if (healthSystem == null)
            {
                healthSystem = new HealthSystem(100,100,1,0.5f);
                StartCoroutine(healthSystem.Regenerate(Time.deltaTime * 2.0f));
            }

            healthSystem.Current.OnValueChanged += OnHealthChanged;
        }

        public void OnDisable()
        {
            healthSystem.Current.OnValueChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int previousValue, int newValue)
        {
            Debug.Log("Health Changed!");
            if(IsOwner && IsClient)
            {                
                //Time.deltaTime * 2.0f => Regeneration is calculated every other frame
                StartCoroutine(healthSystem.Regenerate(Time.deltaTime * 2.0f));
                healthBar.UpdateHealthBar(healthSystem);
            }
        }

        void Start()
        {           
            if (!IsLocalPlayer)
            {
                GetComponentInChildren<CameraParent>().gameObject.SetActive(false);

            }

            healthBar = FindObjectOfType<HealthBar>();
            healthBar.UpdateHealthBar(healthSystem);               
            inputManager = GetComponent<PlayerInputManager>();
            movementManager = GetComponent<PlayerMovementManager>();
            animationManager = GetComponentInChildren<PlayerAnimationManager>();            
        }

        void Update()
        {            
            if (!IsLocalPlayer) return;      

            inputManager.HandleAllInputs();
            movementManager.HandleAllMovement();
            animationManager.HandleAnimations();
        }

        public void TakeDamage(int damage)
        {
            if (!IsLocalPlayer)
            {
                healthSystem.TakeDamage(5);                
                healthBar.UpdateHealthBar(healthSystem);
                StartCoroutine(healthSystem.Regenerate(Time.deltaTime * 2.0f));
            }
        }
    }
}

