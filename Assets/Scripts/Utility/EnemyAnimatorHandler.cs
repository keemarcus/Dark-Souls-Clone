using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class EnemyAnimatorHandler : AnimatorHandler
    {
        EnemyManager enemyManager;
        EnemyStats enemyStats;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
            enemyStats = GetComponentInParent<EnemyStats>();
        }
        public override void TakeCriticalDamageAnimationEvent()
        {
            enemyStats.TakeDamageNoAnimation(enemyManager.pendingCriticalDamage);
            enemyManager.pendingCriticalDamage = 0;
        }
        public void EnableIsParrying()
        {
            enemyManager.isParrying = true;
        }
        public void DisableIsParrying()
        {
            enemyManager.isParrying = false;
        }
        public void EnableCanBeRiposted()
        {
            enemyManager.canBeRiposted = true;
        }
        public void DisableCanBeRiposted()
        {
            enemyManager.canBeRiposted = false;
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidBody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0f;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidBody.velocity = velocity;
        }

        public void AwardSoulsOnDeath()
        {
            // look for every player and award them souls
            PlayerStats[] players = FindObjectsOfType<PlayerStats>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();
            foreach (PlayerStats playerStats in players)
            {
                playerStats.AddSouls(enemyStats.soulsAwardedOnDeath);
                if (soulCountBar != null)
                {
                    soulCountBar.SetSoulCountText(playerStats.soulCount);
                }
            }
        }
    }
}


