using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class PlayerAnimatorHandler : AnimatorHandler
    {
        PlayerManager playerManager;
        PlayerStats playerStats;
        
        private InputHandler inputHandler;
        private PlayerLocomotion playerLocomotion;
        int vertical;
        int horizontal;
        public bool canRotate;
        
        public void Initialize()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            anim = GetComponent<Animator>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0f;

            if(verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }else if(verticalMovement > 0.55f)
            {
                v = 1f;
            }else if(verticalMovement < 0 && verticalMovement > -0.55f){
                v = -0.5f;
            }else if(verticalMovement < -0.55f)
            {
                v = -1f;
            }
            else
            {
                v = 0f;
            }
            #endregion  

            #region Horizontal
            float h = 0f;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1f;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1f;
            }
            else
            {
                h = 0f;
            }
            #endregion 

            if (isSprinting)
            {
                v = 2f;
                h = horizontalMovement;
            }

            anim.SetFloat("Vertical", v, 0.1f, Time.deltaTime);
            anim.SetFloat("Horizontal", h, 0.1f, Time.deltaTime);
        }



        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool isLeftHand)
        {
            if (isLeftHand) { anim.SetFloat("Is Left Hand", 1); }
            else { anim.SetFloat("Is Left Hand", -1); }

            anim.applyRootMotion = isInteracting;
            anim.SetBool("Is Interacting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }

        public void EnableCombo()
        {
            anim.SetBool("Can Do Combo", true);
        }

        public void DisableCombo()
        {
            anim.SetBool("Can Do Combo", false);
        }

        public void EnableIsInvulnerable()
        {
            anim.SetBool("Is Invulnerable", true);
        }

        public void DisableIsInvulnerable()
        {
            anim.SetBool("Is Invulnerable", false);
        }

        public override void TakeCriticalDamageAnimationEvent()
        {
            playerStats.TakeDamageNoAnimation(playerManager.pendingCriticalDamage);
            playerManager.pendingCriticalDamage = 0;
        }

        private void OnAnimatorMove()
        {
            if(!playerManager.isInteracting) { return; }

            float delta = Time.deltaTime;
            playerLocomotion.rigidbody.drag = 0f;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0f;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.rigidbody.velocity = velocity;
        }
    }
}


