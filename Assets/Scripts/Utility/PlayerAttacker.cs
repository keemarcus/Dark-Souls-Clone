using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class PlayerAttacker : MonoBehaviour
    {
        PlayerAnimatorHandler animatorHandler;
        PlayerEquipmentManager playerEquipmentManager;
        PlayerManager playerManager;
        PlayerStats playerStats;
        PlayerInventory playerInventory;
        InputHandler inputHandler;
        WeaponSlotManager weaponSlotManager;
        public string lastAttack;

        public LayerMask backStabLayer = 1 << 12;
        public LayerMask riposteLayer = 1 << 13;

        [Header("Critical Attack Ranges")]
        public float backstabRange;
        public float backstabMaxAngle;
        public float riposteRange;

        private void Start()
        {
            animatorHandler = GetComponent<PlayerAnimatorHandler>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            playerInventory = GetComponentInParent<PlayerInventory>();

        }

        public void HandleWeaponCombo(WeaponItem weapon, bool isLeftHand)
        {
            // check for stamina
            if (playerStats.currentStamina <= 0) { return; }

            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("Can Do Combo", false);
                if(lastAttack == weapon.OH_Light_Attack_01)
                {
                    animatorHandler.PlayTargetAttackAnimation(weapon.OH_Light_Attack_02, true, isLeftHand);
                    lastAttack = weapon.OH_Light_Attack_02;
                } else if(lastAttack == weapon.OH_Light_Attack_02)
                {
                    animatorHandler.PlayTargetAttackAnimation(weapon.OH_Heavy_Attack_01, true, isLeftHand);
                } else if(lastAttack == weapon.TH_Light_Attack_01)
                {
                    animatorHandler.PlayTargetAttackAnimation(weapon.TH_Light_Attack_02, true, isLeftHand);
                    lastAttack = weapon.TH_Light_Attack_02;
                } else if(lastAttack == weapon.TH_Light_Attack_02)
                {
                    animatorHandler.PlayTargetAttackAnimation(weapon.TH_Heavy_Attack_01, true, isLeftHand);
                }
            }
            
        }

        public void HandleOHLightAttack(WeaponItem weapon, bool isLeftHand)
        {
            if (inputHandler.rollFlag) { return; }
            if (playerManager.isInteracting) { return; }
            // check for stamina
            if (playerStats.currentStamina <= 0) { return; }

            weaponSlotManager.attackingWeapon = weapon;
            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAttackAnimation(weapon.TH_Light_Attack_01, true, isLeftHand);
                lastAttack = weapon.TH_Light_Attack_01;
            }
            else
            {
                animatorHandler.PlayTargetAttackAnimation(weapon.OH_Light_Attack_01, true, isLeftHand);
                lastAttack = weapon.OH_Light_Attack_01;
            }
        }

        public void HandleOHHeavyAttack(WeaponItem weapon, bool isLeftHand)
        {
            if (inputHandler.rollFlag) { return; }
            if (playerManager.isInteracting) { return; }
            // check for stamina
            if (playerStats.currentStamina <= 0) { return; }

            weaponSlotManager.attackingWeapon = weapon;
            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAttackAnimation(weapon.TH_Heavy_Attack_01, true, isLeftHand);
                lastAttack = weapon.TH_Heavy_Attack_01;
            }
            else
            {
                animatorHandler.PlayTargetAttackAnimation(weapon.OH_Heavy_Attack_01, true, isLeftHand);
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

        public void HandleLTAction()
        {
            if (playerInventory.leftWeapon.isShield)
            {
                // perform shield weapon art
                PerformLTWeaponArt(inputHandler.twoHandFlag);
            }else if (playerInventory.leftWeapon.isMeleeWeapon)
            {
                animatorHandler.anim.SetBool("Is Using Left Hand", true);
                HandleOHHeavyAttack(playerInventory.leftWeapon, true);
            }
        }

        public void HandleLBAction()
        {
            PerformLBBlockAction();
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
                    playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats, false);
                }
            } 
            else if (weapon.isPyroCaster)
            {
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.isPyroSpell)
                {
                    // Check For Focus Points
                    if (playerStats.currentFocusPoints < playerInventory.currentSpell.focusPointCost)
                    {
                        animatorHandler.PlayTargetAnimation("Take Damage", true);
                        return;
                    }
                    // Attempt To Cast The Spell
                    playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats, false);
                }
            }
        }

        private void PerformLTWeaponArt(bool isTwoHanding)
        {
            if (playerManager.isInteracting) { return; }
            
            if (isTwoHanding) // if we're two-handing, perform weapon art for right weapon
            {
                //animatorHandler.PlayTargetAnimation(playerInventory.rightWeapon.weapon_art, true);
            }
            else // else, perform weapon art for left weapon
            {
                animatorHandler.PlayTargetAnimation(playerInventory.leftWeapon.weapon_art, true);
                playerEquipmentManager.blockingCollider.SetColliderDamageAbsorption(playerInventory.leftWeapon);
            }
            
        }

        private void SuccessfullyCastSpell()
        {
            playerInventory.currentSpell.SuccessfullyCastSpell(animatorHandler, playerStats);
            animatorHandler.anim.SetBool("Is Firing Spell", true);
        }
        #endregion

        #region Defense Actions
        private void PerformLBBlockAction()
        {
            if (playerManager.isInteracting) { return; }
            if(playerManager.isBlocking) { return; }
            animatorHandler.PlayTargetAnimation("Block Start", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;
        }
        #endregion

        public void AttemptBackStabOrRiposte()
        {
            // check for stamina
            if (playerStats.currentStamina <= 0) { return; }

            RaycastHit hit;
            // handle backstab
            if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, backstabRange, backStabLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

                if (enemyCharacterManager != null && Vector3.Angle(playerManager.transform.forward, enemyCharacterManager.transform.forward) < backstabMaxAngle)
                {
                    // check for team ID

                    // pull player into a transform behind the enemy
                    playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamgerStandPoint.position;

                    // rotate towards enemy transform
                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    // do damage
                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    // play backstab animations
                    animatorHandler.PlayTargetAnimation("Back Stab", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorHandler>().PlayTargetAnimation("Back Stabbed", true);
                }
            }
            // handle riposte
            else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, riposteRange, riposteLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

                // check for team ID

                // pull player into a transform behind the enemy
                if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
                {
                    playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamgerStandPoint.position;

                    // rotate towards enemy transform
                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    // do damage
                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    // play backstab animations
                    animatorHandler.PlayTargetAnimation("Riposte", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorHandler>().PlayTargetAnimation("Riposted", true);
                }

            }
        }
    }
}


