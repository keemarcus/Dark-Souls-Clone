using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MK
{
    public class WeaponPickup : Interactable
    {
        public WeaponItem weapon;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            // pick up the weapon and add it to the players inventory
            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory;
            PlayerLocomotion playerLocomotion;
            PlayerAnimatorHandler animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorHandler>();

            playerLocomotion.rigidbody.velocity = Vector3.zero; // stop any current player movement
            animatorHandler.PlayTargetAnimation("Pick Up Item", true); // play the pickup animation
            playerInventory.weaponsInventory.Add(weapon); // add the weapon to the players inventory
            playerManager.itemPopupUIGameObject.SetActive(true); // enable the weapon pickup popup
            playerManager.itemPopupUIGameObject.GetComponentInChildren<Text>().text = weapon.itemName;
            playerManager.itemPopupUIGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
            Destroy(this.gameObject); // destroy the gameobject after it is picked up
        }
    }
}

