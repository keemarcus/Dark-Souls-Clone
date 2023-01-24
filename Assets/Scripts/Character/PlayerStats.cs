using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class PlayerStats : CharacterStats
    {
        public HealthBar healthBar;
        public StaminaBar staminaBar;
        public FocusPointBar focusPointBar;

        PlayerManager playerManager;
        PlayerAnimatorHandler animatorHandler;

        public float staminaRegenerationAmount = 30f;
        public float staminaRegenerationTimer;

        void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointBar = FindObjectOfType<FocusPointBar>();

            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.SetCurrentStamina(currentStamina);

            maxFocusPoints = SetMaxFocusFromFocusLevel();
            currentFocusPoints = maxFocusPoints;
            focusPointBar.SetMaxFocusPoints(maxFocusPoints);
            focusPointBar.SetCurrentFocusPoints(currentFocusPoints);

            this.teamID = "Player";
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        private float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 20;
            return maxStamina;
        }
        private float SetMaxFocusFromFocusLevel()
        {
            maxFocusPoints = focusLevel * 20;
            return maxFocusPoints;
        }

        public void TakeDamageNoAnimation(int damage)
        {
            if (isDead) { return; }
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public void TakeDamage(int damage)
        {
            if (isDead || playerManager.isInvulnerable) { Debug.Log("invlulnerable");  return; }
            currentHealth -= damage;

            healthBar.SetCurrentHealth(currentHealth);

            animatorHandler.PlayTargetAnimation("Take Damage", true);

            if(currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("Die", true);
                isDead = true;
                // handle player death later
            }
        }

        public void DrainStamina(int staminaUsed)
        {
            currentStamina -= staminaUsed;

            staminaBar.SetCurrentStamina(currentStamina);
        }

        public void DrainFocusPoints(int focusPointsUsed)
        {
            currentFocusPoints = Mathf.Clamp(currentFocusPoints - focusPointsUsed, 0, maxFocusPoints);

            focusPointBar.SetCurrentFocusPoints(currentFocusPoints);
        }

        public void RegenerateStamina()
        {
            if (playerManager.isInteracting)
            {
                staminaRegenerationTimer = 0f;
            }
            else
            {
                if(staminaRegenerationTimer <= 1f)
                {
                    staminaRegenerationTimer += Time.deltaTime;
                }

                if (currentStamina < maxStamina && staminaRegenerationTimer > 1f)
                {
                    currentStamina += staminaRegenerationAmount * Time.deltaTime;
                    staminaBar.SetCurrentStamina(currentStamina);
                }
            }
            
        }

        public void HealPlayer(int healAmount)
        {
            currentHealth = Mathf.Clamp(currentHealth += healAmount, 0, maxHealth);
            healthBar.SetCurrentHealth(currentHealth);
        }
    }
}


