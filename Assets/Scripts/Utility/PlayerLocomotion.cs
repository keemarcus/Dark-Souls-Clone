using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class PlayerLocomotion : MonoBehaviour
    {
        CameraHandler cameraHandler;
        PlayerManager playerManager;
        PlayerStats playerStats;
        Transform cameraObject;
        InputHandler inputHandler;
        public Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public PlayerAnimatorHandler animatorHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Ground & Air Detection Stats")]
        [SerializeField]
        float groundDetectionRayStartPoint = 0.5f;
        [SerializeField]
        float minimumDistanceNeededToBeginFall = 1f;
        [SerializeField]
        float groundDirectionRayDistance = 0.2f;
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;

        [Header("Movement Stats")]
        [SerializeField]
        float walkingSpeed = 2f;
        [SerializeField]
        float runningSpeed = 5f;
        [SerializeField]
        float sprintingSpeed = 8f;
        [SerializeField]
        float rotationSpeed = 10f;
        [SerializeField]
        float fallingSpeed = 80f;

        [Header("Stamina Costs")]
        [SerializeField]
        int rollStaminaCost = 15;
        [SerializeField]
        int backStepStaminaCost = 12;
        [SerializeField]
        int jumpStaminaCost = 15;
        [SerializeField]
        int sprintStaminaCost = 1;

        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlocker;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();

            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
            Physics.IgnoreCollision(characterCollider, characterCollisionBlocker, true);
        }


        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        public void HandleRotation(float delta)
        {
            if (animatorHandler.canRotate)
            {
                if (inputHandler.lockOnFlag && !inputHandler.sprintFlag && !inputHandler.rollFlag)
                {
                    Vector3 rotationDirection = moveDirection;
                    rotationDirection = cameraHandler.currentLockOnTarget.transform.position - transform.position;
                    rotationDirection.Normalize();
                    rotationDirection.y = 0;


                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                    transform.rotation = targetRotation;
                }
                else
                {
                    Vector3 targetDirection = Vector3.zero;
                    float moveOverride = inputHandler.moveAmount;

                    targetDirection = cameraObject.forward * inputHandler.vertical;
                    targetDirection += cameraObject.right * inputHandler.horizontal;
                    targetDirection.Normalize();
                    targetDirection.y = 0f;

                    //if (inputHandler.pausedFlag) { targetDirection = Vector3.zero; }

                    if (targetDirection == Vector3.zero)
                    {
                        targetDirection = myTransform.forward;
                    }

                    float rs = rotationSpeed;
                    Quaternion tr = Quaternion.LookRotation(targetDirection);
                    Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

                    myTransform.rotation = targetRotation;
                }
            }
        }

        public void HandleMovement(float delta)
        {
            if (inputHandler.rollFlag) { return; }
            if (playerManager.isInteracting) { return; }
            
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0f;

            //if (inputHandler.pausedFlag) { moveDirection = Vector3.zero; }

            float speed = runningSpeed;

            if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f) { 
                speed = sprintingSpeed;
                playerManager.isSprinting = true;
                playerStats.DrainStamina(sprintStaminaCost);
            }else
            {
                if(inputHandler.moveAmount < 0.5f)
                {
                    speed = walkingSpeed;
                }
                playerManager.isSprinting = false;
            }
            moveDirection *= speed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            if (inputHandler.lockOnFlag && !inputHandler.sprintFlag)
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting);
            }
            else
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
            } 
        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (playerManager.isInteracting) { return; }

            // check for stamina
            if(playerStats.currentStamina <= 0) { return; }

            if (inputHandler.rollFlag)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;

                if(inputHandler.moveAmount > 0)
                {
                    animatorHandler.PlayTargetAnimation("Rolling", true);
                    moveDirection.y = 0f;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                    playerStats.DrainStamina(rollStaminaCost);
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Backstep", true);
                    playerStats.DrainStamina(backStepStaminaCost);
                }
            }
        }

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            if(Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if (playerManager.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallingSpeed);
                rigidbody.AddForce(moveDirection * fallingSpeed / 5f);
            }

            Vector3 direction = moveDirection;
            direction.Normalize();
            origin = origin + direction * groundDirectionRayDistance;

            targetPosition = myTransform.position;

            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if (playerManager.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        Debug.Log("You were in the air for " + inAirTimer);
                        animatorHandler.PlayTargetAnimation("Land", true);
                        inAirTimer = 0f;
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Empty", false);
                        inAirTimer = 0f;
                    }

                    playerManager.isInAir = false;
                }
            }
            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if (!playerManager.isInAir)
                {
                    if (!playerManager.isInteracting)
                    {
                        animatorHandler.PlayTargetAnimation("Falling", true);
                    }

                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (runningSpeed / 2);
                    playerManager.isInAir = true;
                }
            }

            if(playerManager.isInteracting || inputHandler.moveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                myTransform.position = targetPosition;
            }
        }

        public void HandleJumping()
        {
            if (playerManager.isInteracting) { return; }
            if (inputHandler.pausedFlag) { return; }
            // check for stamina
            if (playerStats.currentStamina <= 0) { return; }

            if (inputHandler.a_input)
            {
                if(inputHandler.moveAmount > 0)
                {
                    moveDirection = cameraObject.forward * inputHandler.vertical;
                    moveDirection += cameraObject.right * inputHandler.horizontal;
                    animatorHandler.PlayTargetAnimation("Jump Forward", true);
                    moveDirection.y = 0;
                    Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = jumpRotation;
                    playerStats.DrainStamina(jumpStaminaCost);
                }
                else
                {
                    moveDirection = Vector3.zero;
                    animatorHandler.PlayTargetAnimation("Jump", true);
                    playerStats.DrainStamina(jumpStaminaCost);
                    //Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    //myTransform.rotation = jumpRotation;
                }
            }
        }
        #endregion
    }
}

