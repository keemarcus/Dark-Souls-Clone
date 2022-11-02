using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class EnemyStats : CharacterStats
    {
        Animator animator;

        void Start()
        {
            animator = GetComponentInChildren<Animator>();

            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;

            this.teamID = "Enemy";
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;

            animator.Play("Take Damage");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Die");
                // handle player death later
            }
        }

    }
}
