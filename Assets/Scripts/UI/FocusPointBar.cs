using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MK
{
    public class FocusPointBar : MonoBehaviour
    {
        public Slider slider;

        private void Start()
        {
            slider = GetComponent<Slider>();
        }

        public void SetMaxFocusPoints(float maxFocusPoints)
        {
            slider.maxValue = maxFocusPoints;
        }

        public void SetCurrentFocusPoints(float currentFocusPoints)
        {
            slider.value = currentFocusPoints;
        }
    }
}

