using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class AmbushState : State
    {
        public bool isSleeping;
        public float detectionRadius = 2f;
        public string sleepAnimation;
        public string wakeAnimation;
        public LayerMask detectionLayer;

        public PursueTargetState pursueTargetState;
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            if (enemyManager.isInteracting) { return this; }

            if (isSleeping && !enemyManager.isInteracting)
            {
                enemyAnimatorHandler.PlayTargetAnimation(sleepAnimation, true);
            }

            #region Handle Target Detection
            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);
            foreach(Collider collider in colliders)
            {
                PlayerStats playerStats = collider.transform.GetComponent<PlayerStats>();

                if(playerStats != null)
                {
                    Vector3 targetDirection = playerStats.transform.position - enemyManager.transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
                    
                    if(viewableAngle > enemyManager.minimumDetectionAngle &&
                        viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = playerStats;
                        isSleeping = false;
                        enemyAnimatorHandler.PlayTargetAnimation(wakeAnimation, true);
                    }
                }
            }
            #endregion

            #region Handle State Change
            if(enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
            #endregion
        }
    }
}

