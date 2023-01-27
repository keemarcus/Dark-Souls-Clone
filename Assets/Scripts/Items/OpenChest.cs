using System.Collections;
using UnityEngine;

namespace MK
{
    public class OpenChest : Interactable
    {
        Animator animator;
        OpenChest openChest;
        public GameObject itemSpawner;
        public WeaponItem itemInChest;
        public Transform playerOpeningPosition;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            openChest = this;
        }
        public override void Interact(PlayerManager playerManager)
        {
            // rotate player towards the chest
            Vector3 rotationDIrection = transform.position - playerManager.transform.position;
            rotationDIrection.y = 0;
            rotationDIrection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDIrection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300f * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            // lock the player transform to a certain point in front of chest
            playerManager.OpenChestInteraction(playerOpeningPosition);

            // play opening animations
            animator.Play("Chest Open");

            // spawn an item inside the chest
            StartCoroutine(SpawnItemInChest());
            WeaponPickup weaponPickup = itemSpawner.GetComponent<WeaponPickup>();
            if(weaponPickup != null)
            {
                weaponPickup.weapon = itemInChest;
            }
        }

        private IEnumerator SpawnItemInChest()
        {
            yield return new WaitForSeconds(1f);
            Instantiate(itemSpawner, transform);
            Destroy(openChest);
        }
    }
}

