using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MK
{
    public class HandEquipmentSlotUI : MonoBehaviour
    {
        public UIManager uiManager;

        public Image icon;
        WeaponItem weapon;

        public bool leftHandSlot01;
        public bool leftHandSlot02;
        public bool rightHandSlot01;
        public bool rightHandSlot02;

        public void AddItem(WeaponItem newWeapon)
        {
            weapon = newWeapon;
            icon.sprite = weapon.itemIcon;
            icon.enabled = true;
            this.gameObject.SetActive(true);
        }

        public void ClearItem()
        {
            weapon = null;
            icon.sprite = null;
            icon.enabled = false;
            this.gameObject.SetActive(false);
        }

        private void DeselectAllSlots()
        {
            uiManager.leftHandSlot01Selected = false;
            uiManager.leftHandSlot02Selected = false;
            uiManager.rightHandSlot01Selected = false;
            uiManager.rightHandSlot02Selected = false;
        }

        public void SelectThisSlot()
        {
            // make sure all the slots are deselected to start
            DeselectAllSlots();

            if (leftHandSlot01)
            {
                uiManager.leftHandSlot01Selected = true;
            }else if (leftHandSlot02)
            {
                uiManager.leftHandSlot02Selected = true;
            }
            else if (rightHandSlot01)
            {
                uiManager.rightHandSlot01Selected = true;
            }
            else if (rightHandSlot02)
            {
                uiManager.rightHandSlot02Selected = true;
            }
        }
    }
}


