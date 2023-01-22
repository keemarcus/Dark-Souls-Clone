using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class PlayerAttacker : MonoBehaviour
    {
        PlayerAnimatorHandler animatorHandler;
        PlayerManager playerManager;
        PlayerStats playerStats;
        PlayerInventory playerInventory;
        InputHandler inputHandler;
        WeaponSlotManager weaponSlotManager;
        public string lastAttack;

        private void Start()
        {
            animatorHandler = GetComponent<PlayerAnimatorHandler>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            playerInventory = GetComponentInParent<PlayerInventory>();
        }

        public void HandleWeaponCombo(WeaponItem weapon, bool isLeftHand)
        {
            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("Can Do Combo", false);
                if(lastAttack == weapon.OH_Light_Attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_02, true, isLeftHand);
                    lastAttack = weapon.OH_Light_Attack_02;
                } else if(lastAttack == weapon.OH_Light_Attack_02)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_01, true, isLeftHand);
                } else if(lastAttack == weapon.TH_Light_Attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_02, true, isLeftHand);
                    lastAttack = weapon.TH_Light_Attack_02;
                } else if(lastAttack == weapon.TH_Light_Attack_02)
                {
                    animatorHandler.PlayTargetAnimation(weapon.TH_Heavy_Attack_01, true, isLeftHand);
                }
            }
            
        }

        public void HandleOHLightAttack(WeaponItem weapon, bool isLeftHand)
        {
            if (inputHandler.rollFlag) { return; }
            if (playerManager.isInteracting) { return; }

            weaponSlotManager.attackingWeapon = weapon;
            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_01, true, isLeftHand);
                lastAttack = weapon.TH_Light_Attack_01;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_01, true, isLeftHand);
                lastAttack = weapon.OH_Light_Attack_01;
            }
        }

        public void HandleOHHeavyAttack(WeaponItem weapon, bool isLeftHand)
        {
            if (inputHandler.rollFlag) { return; }
            if (playerManager.isInteracting) { return; }

            weaponSlotManager.attackingWeapon = weapon;
            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.TH_Heavy_Attack_01, true, isLeftHand);
                lastAttack = weapon.TH_Heavy_Attack_01;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_01, true, isLeftHand);
                lastAttack = weapon.OH_Heavy_Attack_01;
            }
        }
        #region Input Actions
        public void HandleRBAction()
        {
            if (playerInventory.rightWeapon.isMeleeWeapon)
            {
                //Handle Melee Action
                PerformRBMeleeAction();
            } else if (playerInventory.rightWeapon.isSpellCaster || playerInventory.rightWeapon.isFaithCaster || playerInventory.rightWeapon.isPyroCaster)
            {
                // Handle Magic Action
                PerformRBMagicAction(playerInventory.rightWeapon);
            }//else if (playerInventory.rightWeapon.isFaithCaster)
            {
                // Handle Miracle Action
            }//else if (playerInventory.rightWeapon.isPyroCaster)
            {
                // Handle Pyro Action
            }
            
        }
        #endregion

        #region Attack Actions
        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                animatorHandler.anim.SetBool("Is Using Right Hand", true);
                HandleWeaponCombo(playerInventory.rightWeapon, false);
                inputHandler.comboFlag = false;
            }
            else
            {
                animatorHandler.anim.SetBool("Is Using Right Hand", true);
                HandleOHLightAttack(playerInventory.rightWeapon, false);
            }
        }

        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if (playerManager.isInteracting) { return; }
            if (weapon.isFaithCaster)
            {
                if(playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
                {
                    // Check For Focus Points
                    if(playerStats.currentFocusPoints < playerInventory.currentSpell.focusPointCost) 
                    {
                        animatorHandler.PlayTargetAnimation("Take Damage", true);
                        return; 
                    }
                    // Attempt To Cast The Spell
                    playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats);
                }
            }
        }
        private void SuccessfullyCastSpell()
        {
            playerInventory.currentSpell.SuccessfullyCastSpell(animatorHandler, playerStats);
        }
        #endregion
    }
}


