using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class PlayerAttacker : MonoBehaviour
    {
        PlayerAnimatorHandler animatorHandler;
        PlayerManager playerManager;
        InputHandler inputHandler;
        WeaponSlotManager weaponSlotManager;
        public string lastAttack;

        private void Start()
        {
            animatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            inputHandler = GetComponent<InputHandler>();
            playerManager = GetComponent<PlayerManager>();
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
    }
}


