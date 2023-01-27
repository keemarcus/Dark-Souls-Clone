using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class CharacterStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public float maxStamina;
        public float currentStamina;
        public bool isDead;

        public int focusLevel = 10;
        public float maxFocusPoints;
        public float currentFocusPoints;

        public int soulCount = 0;

        public string teamID;
    }
}


