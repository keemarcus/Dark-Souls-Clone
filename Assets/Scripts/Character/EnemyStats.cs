using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class EnemyStats : CharacterStats
    {
        EnemyAnimatorHandler enemyAnimatorHandler;
        public int soulsAwardedOnDeath = 50;

        void Start()
        {
            enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();

            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;

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
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public void TakeDamage(int damage)
        {
            if (isDead) { return; }
            currentHealth -= damage;

            enemyAnimatorHandler.PlayTargetAnimation("Take Damage", true);

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
