using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool a_input;
        public bool b_input;
        public bool x_input;
        public bool y_input;
        public bool start_input;
        public bool rb_input;
        public bool rt_input;
        public bool lb_input;
        public bool lt_input;
        public bool lockOnInput;
        public bool right_stick_right_input;
        public bool right_stick_left_input;

        public bool d_pad_up;
        public bool d_pad_down;
        public bool d_pad_left;
        public bool d_pad_right;

        public bool rollFlag;
        public bool sprintFlag;
        public bool twoHandFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public float changeLockOnTargetTimer;
        public bool inventoryFlag;
        public bool pausedFlag;

        public float rollInputTimer;
        

        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        WeaponSlotManager weaponSlotManager;
        PlayerManager playerManager;
        CameraHandler cameraHandler;
        AnimatorHandler animatorHandler;
        UIManager uiManager;
        

        Vector2 movementInput;
        Vector2 cameraInput;


        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            uiManager = FindObjectOfType<UIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            pausedFlag = false;
        }


        public void OnEnable()
        {
            if(inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

                changeLockOnTargetTimer = 0f;

                inputActions.PlayerActions.RB.performed += i => rb_input = true;
                inputActions.PlayerActions.RT.performed += i => rt_input = true;
                inputActions.PlayerActions.LB.performed += i => lb_input = true;
                inputActions.PlayerActions.LT.performed += i => lt_input = true;
                inputActions.DPadActions.DPadRight.performed += i => d_pad_right = true;
                inputActions.DPadActions.DPadLeft.performed += i => d_pad_left = true;
                inputActions.PlayerActions.Interact.performed += i => x_input = true;
                inputActions.PlayerActions.Jump.performed += i => a_input = true;
                inputActions.PlayerActions.Y.performed += i => y_input = true;
                inputActions.PlayerActions.Start.performed += i => start_input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_stick_right_input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_stick_left_input = true;
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            HandleMoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleQuickSlotsInput();
            HandleInventoryInput();
            HandleLockOnInput(delta);
            HandleTwoHandInput();
        }

        private void HandleMoveInput(float delta)
        {
            if (!pausedFlag)
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
            else
            {
                horizontal = 0f;
                vertical = 0f;
                moveAmount = 0f;
                mouseX = 0f;
                mouseY = 0f;
            }
        }

        private void HandleRollInput(float delta)
        {
            if (pausedFlag) { return; }

            b_input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;
            sprintFlag = b_input;

            if (b_input)
            {
                rollInputTimer += delta;
                //sprintFlag = true;
            }
            else
            {
                if(rollInputTimer > 0 && rollInputTimer < 0.15f)
                {
                    //sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0f;
            }
        }

        private void HandleAttackInput(float delta)
        {
            if (pausedFlag) { return; }

            // rb/rt handle right hand weapons
            if (rb_input)
            {
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    animatorHandler.anim.SetBool("Is Using Right Hand", true);
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon, false);
                    comboFlag = false;
                }
                else
                {
                    animatorHandler.anim.SetBool("Is Using Right Hand", true);
                    playerAttacker.HandleOHLightAttack(playerInventory.rightWeapon, false);
                }
            }
            if (rt_input)
            {
                animatorHandler.anim.SetBool("Is Using Right Hand", true);
                playerAttacker.HandleOHHeavyAttack(playerInventory.rightWeapon, false);
            }
            // lb/lt handle left hand weapons
            if (lb_input)
            {
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    animatorHandler.anim.SetBool("Is Using Left Hand", true);
                    playerAttacker.HandleWeaponCombo(playerInventory.leftWeapon, true);
                    comboFlag = false;
                }
                else
                {
                    animatorHandler.anim.SetBool("Is Using Left Hand", true);
                    playerAttacker.HandleOHLightAttack(playerInventory.leftWeapon, true);
                }
            }
            if (lt_input)
            {
                animatorHandler.anim.SetBool("Is Using Left Hand", true);
                playerAttacker.HandleOHHeavyAttack(playerInventory.leftWeapon, true);
            }

        }

        private void HandleQuickSlotsInput()
        {
            if (pausedFlag) { return; }

            if (d_pad_right)
            {
                playerInventory.ChangeRightWeapon();
            }
            else if(d_pad_left)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }

        private void HandleInventoryInput()
        {
            

            if (start_input)
            {
                inventoryFlag = !inventoryFlag;
                pausedFlag = !pausedFlag;

                if (inventoryFlag)
                {
                    uiManager.OpenSelectWindow();
                    uiManager.UpdateUI();
                }
                else
                {
                    uiManager.CloseSelectWindow();
                    uiManager.CloseAllInventoryWindows();
                }
            }
        }

        private void HandleLockOnInput(float delta)
        {
            if(changeLockOnTargetTimer > 0f) { changeLockOnTargetTimer -= delta; }

            if (pausedFlag) { cameraHandler.ClearLockOnTargets(); return; }

            if (lockOnInput && !lockOnFlag) 
            { 
                lockOnInput = false;
                cameraHandler.HandleLockOn();
                if(cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                    changeLockOnTargetTimer = 0.5f;
                }
            } else if(lockOnInput && lockOnFlag)
            {
                lockOnFlag = false;
                lockOnInput = false;
                // clear lock on targets
                cameraHandler.ClearLockOnTargets();
            }

            if (lockOnFlag && right_stick_left_input && changeLockOnTargetTimer <= 0f)
            {
                right_stick_left_input = false;
                cameraHandler.HandleLockOn();
                if(cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                    changeLockOnTargetTimer = 0.5f;
                }
            }
            else if(lockOnFlag && right_stick_right_input && changeLockOnTargetTimer <= 0f)
            {
                right_stick_right_input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                    changeLockOnTargetTimer = 0.5f;
                }
            }

            cameraHandler.SetCameraHeight();
        }

        private void HandleTwoHandInput() 
        {
            if (pausedFlag) { return; }

            if (y_input)
            {
                y_input = false;
                twoHandFlag = !twoHandFlag;

                if (twoHandFlag)
                {
                    // enable two handing
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                }
                else
                {
                    // disable two handing
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
                }
            }
        }

    }
}

