using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class EnemyStats : CharacterStats
    {
        EnemyAnimatorHandler enemyAnimatorHandler;
        public UIEnemyHealthBar enemyHealthBar;
        public int soulsAwardedOnDeath = 50;

        void Start()
        {
            enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
            enemyHealthBar = GetComponentInChildren<UIEnemyHealthBar>();

            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            enemyHealthBar.SetMaxHealth(maxHealth);

            this.teamID = "Enemy";
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamageNoAnimation(int damage)
        {
            if (isDead) { return; }
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
            enemyHealthBar.SetHealth(currentHealth);

            if (currentHealth <= 0)
            {
                isDead = true;
            }
        }

        public void TakeDamage(int damage, string damageAnimation = "Take Damage")
        {
            if (isDead) { return; }
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
            enemyHealthBar.SetHealth(currentHealth);

            enemyAnimatorHandler.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            currentHealth = 0;
            enemyAnimatorHandler.PlayTargetAnimation("Die", true);
            isDead = true;

            
        }

    }
}
