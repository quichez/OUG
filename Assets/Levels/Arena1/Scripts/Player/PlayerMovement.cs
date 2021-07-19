using UnityEngine;
using UnityEngine.InputSystem;
using MLAPI;
using System;
using Cinemachine;

namespace JAM
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : NetworkBehaviour
    {
        private CharacterController controller;
        private PlayerInputManager inputManager;
        Transform cameraObjectTransform;


        private Vector3 playerVelocity;
        private bool groundedPlayer;

        [SerializeField] private float playerSpeed = 5f;
        [SerializeField] private float jumpHeight = 1.0f;
        [SerializeField] private float gravityValue = -9.81f;
        [SerializeField] private float rotationSpeed = 7f;

        private void Start()
        {
            if (!IsLocalPlayer)
            {
                GetComponentInChildren<CinemachineFreeLook>().gameObject.SetActive(false);
            }
            else
            {
                controller = GetComponent<CharacterController>();
                inputManager = GetComponent<PlayerInputManager>();
                cameraObjectTransform = GetComponentInChildren<Camera>().transform;
                cameraObjectTransform.parent.GetComponentInChildren<CinemachineInputProvider>().PlayerIndex = (int)NetworkManager.Singleton.LocalClientId; // Used to avoid shared camera controls.
                Cursor.lockState = CursorLockMode.Locked;
            }

        }

        void Update()
        {
            if (!IsLocalPlayer) return;

            inputManager.HandleAllInputs();
            Move();
        }

        private void Move()
        {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }


            // Movement
            Vector3 move = new Vector3(inputManager.horizontalInput, 0, inputManager.verticalInput);
            move = cameraObjectTransform.forward * move.z + cameraObjectTransform.right * move.x;
            move.y = 0;
            controller.Move(move * Time.deltaTime * playerSpeed);


            // Jump
            if (inputManager.jump && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);


            // Rotation
            if (inputManager.moveInput != Vector2.zero)
            {
                float targetAngle = Mathf.Atan2(
                    inputManager.horizontalInput, inputManager.verticalInput) * Mathf.Rad2Deg +
                    cameraObjectTransform.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
                    rotationSpeed * Time.deltaTime);
            }
        }
    }
}
