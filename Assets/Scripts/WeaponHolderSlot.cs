using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        public WeaponItem currentWeapon;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;
        public bool isBackSlot;

        public GameObject currentWeaponModel;

        public void UnloadWeapon()
        {
            if(currentWeaponModel != null)
            {
                currentWeaponModel.SetActive(false);
            }
        }

        public void UnloadWeaponAndDestroy()
        {
            if(currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }
        public void LoadWeaponModel(WeaponItem weaponItem, bool isLeftHand)
        {
            // unload weapon and destory
            UnloadWeaponAndDestroy();

            if(weaponItem == null)
            {
                // unload the current weapon model
                UnloadWeapon();
                return;
            }

            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
            if(model != null)
            {
                if(parentOverride != null)
                {
                    model.transform.parent = parentOverride;
                }
                else
                {
                    model.transform.parent = transform;
                }

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
                if (model.tag == "Shield" && !isLeftHand)
                {
                    // flip the prefab object if it's a shield and we need it in the other hand
                    Transform prefab = model.transform.GetChild(0);
                    prefab.Rotate(Vector3.up, 180f);
                    prefab.localPosition = new Vector3(prefab.localPosition.x, prefab.localPosition.y, prefab.localPosition.z * -1f);
                }
            }

            currentWeaponModel = model;
        }
    }
}


