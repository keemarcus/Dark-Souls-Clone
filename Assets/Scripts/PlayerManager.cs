using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        PlayerLocomotion playerLocomotion;
        InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemPopupUIGameObject;

        public bool isInteracting;

        
        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;

        //private void Awake()
        //{
            //cameraHandler = CameraHandler.singleton;
        //}

        void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            interactableUI = FindObjectOfType<InteractableUI>();
        }

        void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = anim.GetBool("Is Interacting");
            canDoCombo = anim.GetBool("Can Do Combo");
            anim.SetBool("Is In Air", isInAir);
            
            inputHandler.TickInput(delta);
            
            
            playerLocomotion.HandleRollingAndSprinting(delta);
            
            playerLocomotion.HandleJumping();

            CheckForInteractableObject();
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
            playerLocomotion.HandleMovement(delta);

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }

        private void LateUpdate()
        {
            // reset all our input variable at the end of each frame
            inputHandler.rollFlag = false;
            inputHandler.rb_input = false;
            inputHandler.rt_input = false;
            inputHandler.lb_input = false;
            inputHandler.lt_input = false;
            inputHandler.d_pad_up = false;
            inputHandler.d_pad_down = false;
            inputHandler.d_pad_left = false;
            inputHandler.d_pad_right = false;
            inputHandler.x_input = false;
            inputHandler.a_input = false;
            inputHandler.start_input = false;
            inputHandler.right_stick_left_input = false;
            inputHandler.right_stick_right_input = false;

            // tick our in air timer if we are falling
            if (isInAir)
            {
                playerLocomotion.inAirTimer += Time.deltaTime;
            }
        }

        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.06f, transform.forward, out hit) && hit.collider.tag == "Interactable")
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if(interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText;
                    // set the ui popup text to this string
                    interactableUI.interactableText.text = interactableText;
                    interactableUIGameObject.SetActive(true);

                    if (inputHandler.x_input)
                    {
                        interactableObject.Interact(this);
                    }
                }
                
            }
            else
            {
                if(interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if(itemPopupUIGameObject != null && inputHandler.x_input)
                {
                    itemPopupUIGameObject.SetActive(false);
                }
            }
        }
    }
}
