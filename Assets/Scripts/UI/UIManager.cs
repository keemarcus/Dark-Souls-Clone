using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MK
{
    public class UIManager : MonoBehaviour
    {
        public PlayerInventory playerInventory;
        public EquipmentWindowUI equipmentWindowUI;
        EventSystem eventSystem;

        [Header("UI Windows")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject weaponInventoryWindow;
        public GameObject equipmentWindow;

        [Header("Equipment Window Slot Selected")]
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;

        [Header("Weapon Inventory")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotsParent;
        WeaponInventorySlot[] weaponInventorySlots;

        private void Start()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
            eventSystem = FindObjectOfType<EventSystem>();
        }

        public void UpdateUI()
        {
            #region Weapon Inventory Slots
            // loop through all the weapon inventory slots
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                // if the player's inventory has an item for this slot, add it to the inventory ui
                if(i < playerInventory.weaponsInventory.Count)
                {
                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }
                else // if there's no item for this slot, make sure it's empty
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }              
            }
            #endregion
        }

        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
            hudWindow.SetActive(false);
            eventSystem.SetSelectedGameObject(selectWindow.GetComponentInChildren<Button>().gameObject);
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
            hudWindow.SetActive(true);
        }

        public void CloseAllInventoryWindows()
        {
            weaponInventoryWindow.SetActive(false);
            equipmentWindow.SetActive(false);
        }
        public void ResetAllSelectedSlots()
        {
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
        }
    }
}


