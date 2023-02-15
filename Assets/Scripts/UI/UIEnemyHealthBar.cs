using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MK
{
    public class UIEnemyHealthBar : MonoBehaviour
    {
        Slider slider;
        float timeUntilBarIsHidden = 0f;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
        }
        private void Update()
        {
            if(slider == null) { return; }

            if(timeUntilBarIsHidden <= 0f)
            {
                timeUntilBarIsHidden = 0f;
                slider.gameObject.SetActive(false);
            }
            else
            {
                timeUntilBarIsHidden -= Time.deltaTime;

                if (!slider.gameObject.activeInHierarchy)
                {
                    timeUntilBarIsHidden -= Time.deltaTime;
                    slider.gameObject.SetActive(true);
                }

                slider.gameObject.transform.rotation = FindObjectOfType<CameraHandler>().gameObject.transform.rotation;
            }

            if(slider.value <= 0)
            {
                Destroy(slider.gameObject);
            }
        }

        public void SetHealth(int health)
        {
            slider.value = health;
            timeUntilBarIsHidden = 3f;
        }

        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }
    }
}

