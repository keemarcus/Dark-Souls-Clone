using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MK
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        public Image icon;
        public WeaponItem item;
        PlayerInventory playerInventory;
        WeaponSlotManager weaponSlotManager;
        UIManager uiManager;
        EventSystem eventSystem;
        GameObject equipmentWindow;


        private void Awake()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            weaponSlotManager = FindObjectOfType<WeaponSlotManager>();
            uiManager = FindObjectOfType<UIManager>();
            eventSystem = FindObjectOfType<EventSystem>();
            equipmentWindow = uiManager.equipmentWindow;
        }

        public void AddItem(WeaponItem newItem)
        {
            item = newItem;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            GetComponentInChildren<Button>().interactable = true;
            this.gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            GetComponentInChildren<Button>().interactable = false;
            //this.gameObject.SetActive(false);
        }

        public void EquipThisItem()
        {
            if (uiManager.leftHandSlot01Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlots[0]);    // remove the current item and add it to the inventory
                playerInventory.weaponsInLeftHandSlots[0] = item;   // equip the new item and remove it from the inventory
                playerInventory.weaponsInventory.Remove(item);
            }else if (uiManager.leftHandSlot02Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlots[1]);    // remove the current item and add it to the inventory
                playerInventory.weaponsInLeftHandSlots[1] = item;   // equip the new item and remove it from the inventory
                playerInventory.weaponsInventory.Remove(item);
            }
            else if (uiManager.rightHandSlot01Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlots[0]);    // remove the current item and add it to the inventory
                playerInventory.weaponsInRightHandSlots[0] = item;   // equip the new item and remove it from the inventory
                playerInventory.weaponsInventory.Remove(item);
            }
            else if (uiManager.rightHandSlot02Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlots[1]);    // remove the current item and add it to the inventory
                playerInventory.weaponsInRightHandSlots[1] = item;   // equip the new item and remove it from the inventory
                playerInventory.weaponsInventory.Remove(item);
            }
            else { return; }

            playerInventory.rightWeapon = playerInventory.weaponsInRightHandSlots[playerInventory.currentRightWeaponIndex];
            playerInventory.leftWeapon = playerInventory.weaponsInLeftHandSlots[playerInventory.currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);

            uiManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
            uiManager.ResetAllSelectedSlots();
            uiManager.UpdateUI();
        }

    }
}

