using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JAM
{
    public class PlayerInputManager : MonoBehaviour
    {
        PlayerInputControls inputs;


        public Vector2 moveInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        public bool jump;
        public bool jumpInput;
        public bool attack;
        public bool attackInput;


        private void OnEnable()
        {
            if (inputs == null)
            {
                inputs = new PlayerInputControls();

                inputs.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();

                inputs.Player.Jump.performed += ctx => jumpInput = ctx.action.triggered;
                inputs.Player.Jump.canceled += ctx => jumpInput = ctx.action.triggered;

                inputs.Player.Attack.performed += ctx => attackInput = ctx.action.triggered;
                inputs.Player.Attack.canceled += ctx => attackInput = ctx.action.triggered;
            }

            inputs.Enable();
        }

        private void OnDisable()
        {
            inputs.Disable();
        }

        public void HandleAllInputs()
        {
            HandleMovementInput();
            HandleJumpInput();
            HandleAttackInput();
        }

        private void HandleMovementInput()
        {
            verticalInput = moveInput.y;
            horizontalInput = moveInput.x;
        }

        private void HandleJumpInput()
        {
            jump = jumpInput;
        }

        private void HandleAttackInput()
        {
            attack = attackInput;
        }
    }
}
