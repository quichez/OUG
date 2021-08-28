using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

namespace JAM
{
    public class PlayerAnimationManager : NetworkBehaviour
    {
        PlayerInputManager inputs;
        PlayerMovementManager movementManager;
        Animator animator;
        

        int horizontal;
        int vertical;


        private float moveAmount;

        public bool attackAvail;
        public int comboParam;
        public int comboStage;

        private void Start()
        {
            if (!IsLocalPlayer) return;

            inputs = GetComponentInParent<PlayerInputManager>();
            movementManager = GetComponentInParent<PlayerMovementManager>();
            animator = GetComponent<Animator>();

            horizontal = Animator.StringToHash("Horizontal");
            vertical = Animator.StringToHash("Vertical");
            comboParam = Animator.StringToHash("Combo");

            attackAvail = true;
            comboStage = 0;
        }

        public void HandleAnimations()
        {
            moveAmount = Mathf.Clamp01(Mathf.Abs(inputs.horizontalInput) + Mathf.Abs(inputs.verticalInput));
            MovementAnimatorValues(0, moveAmount);

            AttackAnimation();
        }

        private void AttackAnimation()
        {
            if (inputs.attack && attackAvail)
            {
                attackAvail = false;
                comboStage++;
                animator.SetInteger(comboParam, comboStage);
            }
        }

        private void MovementAnimatorValues(float horizontalMovement, float verticalMovement)
        {
            if (movementManager.groundedPlayer)
            {
                animator.SetFloat(horizontal, horizontalMovement, 0.1f, Time.deltaTime);
                animator.SetFloat(vertical, verticalMovement, 0.1f, Time.deltaTime);
            }
            else
            {
                animator.SetFloat(horizontal, 0, 0.1f, Time.deltaTime);
                animator.SetFloat(vertical, 0, 0.1f, Time.deltaTime);
            }
        }

        private void AttackEnding()
        {
            if (!IsLocalPlayer) return;
            attackAvail = true;

            bool isHit = Physics.BoxCast(transform.position + transform.up * 1.25f, new Vector3(0.25f, 1.0f, 0.25f), transform.forward, out RaycastHit hit, transform.rotation, 0.5f);
            if (isHit)
            {                
                if (hit.transform.gameObject.TryGetComponent(out PlayerManager pm))
                {
                    Debug.Log(pm.name);
                    pm.TakeDamage(5);
                }
            }

            Debug.Log("AttackEnding");
        }

        private void AttackEnded()
        {
            if (!IsLocalPlayer) return;
            attackAvail = true;
            comboStage = 0;
            animator.SetInteger(comboParam, comboStage);
            Debug.Log("AttackEnded");
        }

    }
}

