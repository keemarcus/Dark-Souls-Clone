using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MK
{
    public class EquipmentWindowUI : MonoBehaviour
    {
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;

        public HandEquipmentSlotUI[] handEquipmentSlotUI;
        EventSystem eventSystem;

        private void Awake()
        {
            eventSystem = FindObjectOfType<EventSystem>();
            handEquipmentSlotUI = GetComponentsInChildren<HandEquipmentSlotUI>();
        }

        public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
        {
            for(int i = 0; i < handEquipmentSlotUI.Length; i++)
            {
                if (handEquipmentSlotUI[i].leftHandSlot01)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
                }
                else if(handEquipmentSlotUI[i].leftHandSlot02)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
                }else if (handEquipmentSlotUI[i].rightHandSlot01)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
                } else if (handEquipmentSlotUI[i].rightHandSlot02)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
                }

                //handEquipmentSlotUI[i].GetComponent<Button>()
            }
        }

        public void SelectLeftHandSlot01()
        {
            leftHandSlot01Selected = true;
            leftHandSlot02Selected = false;
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
        }

        public void SelectLeftHandSlot02()
        {
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = true;
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
        }

        public void SelectRightHandSlot01()
        {
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;
            rightHandSlot01Selected = true;
            rightHandSlot02Selected = false;
        }

        public void SelectRightHandSlot02()
        {
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = true;
        }
    }
}


