using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class WeaponSlotManager : MonoBehaviour
    {
        Animator anim;

        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;
        WeaponHolderSlot backSlot;

        DamageCollider leftHandDamageCollider;
        DamageCollider rightHandDamageCollider;

        public WeaponItem attackingWeapon;

        QuickSlotsUI quickSlotsUI;

        PlayerStats playerStats;
        InputHandler inputHandler;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            playerStats = GetComponentInParent<PlayerStats>();
            inputHandler = GetComponentInParent<InputHandler>();

            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                    //LoadLeftWeaponDamageCollider();
                } else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                    //LoadRightWeaponDamageCollider();
                } else if (weaponSlot.isBackSlot)
                {
                    backSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft) // we are equiping it to the left hand
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem, isLeft);
                LoadLeftWeaponDamageCollider();
                #region Handle Left Weapon Idle Animations
                if (weaponItem != null)
                {
                    anim.CrossFade(weaponItem.Left_Hand_Idle, 0.2f);
                }
                else
                {
                    anim.CrossFade("Left Arm Empty", 0.2f);
                }
                #endregion
            }
            else
            {
                if (inputHandler.twoHandFlag) // we are two handing the weapon
                {
                    // if we have something in the left hand, move to back or disable it
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon, true);
                    leftHandSlot.UnloadWeaponAndDestroy();

                    anim.CrossFade(weaponItem.TH_Idle, 0.2f);
                }
                else // we are equiping it to the right hand
                {
                    anim.CrossFade("Both Arms Empty", 0.2f);
                    backSlot.UnloadWeaponAndDestroy();
                    #region Handle Right Weapon Idle Animations
                    if (weaponItem != null)
                    {
                        anim.CrossFade(weaponItem.Right_Hand_Idle, 0.2f);
                    }
                    else
                    {
                        anim.CrossFade("Right Arm Empty", 0.2f);
                    }
                    #endregion
                }
                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem, isLeft);
                LoadRightWeaponDamageCollider();
            }

            quickSlotsUI.UpdateWeaponQuickSlotsUI(weaponItem, isLeft);
        }

        #region Handle Weapon Damage Colliders
        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }
        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }
        public void OpenDamageColliders()
        {
            if(anim.GetFloat("Is Left Hand") == 1) { OpenLeftDamageCollider(); }
            else { OpenRightDamageCollider(); }
        }
        public void CloseDamageColliders()
        {
            if (anim.GetFloat("Is Left Hand") == 1) { CloseLeftDamageCollider(); }
            else { CloseRightDamageCollider(); }
        }
        private void OpenLeftDamageCollider()
        {
            if (leftHandDamageCollider != null)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }
        private void CloseLeftDamageCollider()
        {
            if (leftHandDamageCollider != null)
            {
                leftHandDamageCollider.DisableDamageCollider();
            }
        }
        private void OpenRightDamageCollider()
        {
            if(rightHandDamageCollider != null)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
        }
        private void CloseRightDamageCollider()
        {
            if (rightHandDamageCollider != null)
            {
                rightHandDamageCollider.DisableDamageCollider();
            }
        }
        #endregion

        #region Handle Weapon Stamina Drains
        public void DrainStaminaLightAttack()
        {
            playerStats.DrainStamina(Mathf.RoundToInt(attackingWeapon.baseStaminaCost * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStats.DrainStamina(Mathf.RoundToInt(attackingWeapon.baseStaminaCost * attackingWeapon.heavyAttackMultiplier));
        }
        #endregion
    }
}


