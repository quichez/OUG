using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JAM
{
    public class PlayerAnimationManager : MonoBehaviour
    {
        PlayerInputManager inputs;
        Animator animator;
        CharacterController controller;

        int horizontal;
        int vertical;
        int attack;

        private float moveAmount;

        private void Start()
        {
            inputs = GetComponent<PlayerInputManager>();
            animator = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();

            horizontal = Animator.StringToHash("Horizontal");
            vertical = Animator.StringToHash("Vertical");
            attack = Animator.StringToHash("Attack");
        }

        public void HandleAnimations()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0, Vector3.down, out hit, 0.2f))
            {
                moveAmount = Mathf.Clamp01(Mathf.Abs(inputs.horizontalInput) + Mathf.Abs(inputs.verticalInput));
                MovementAnimatorValues(0, moveAmount);
            }            

                
        }

        private void AttackAnimation()
        {
            animator.SetTrigger(attack);
        }

        public void MovementAnimatorValues(float horizontalMovement, float verticalMovement)
        {
            if (controller.isGrounded)
            {
                animator.SetFloat(horizontal, horizontalMovement, 0.1f, Time.deltaTime);
                animator.SetFloat(vertical, verticalMovement, 0.1f, Time.deltaTime);
            }            
        }
    }
}

