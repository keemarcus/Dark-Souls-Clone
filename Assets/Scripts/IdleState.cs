using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class IdleState : State
    {
        public PursueTargetState pursueTargetState;
        public LayerMask detectionLayer;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            // look for a potential target
            #region Target Detection
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

            foreach (Collider collider in colliders)
            {
                CharacterStats characterStats = collider.GetComponent<CharacterStats>();

                if (characterStats != null && characterStats.teamID != this.GetComponentInParent<CharacterStats>().teamID)
                {
                    // check for team ID

                    Vector3 targetDirection = characterStats.transform.position - transform.forward;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                    }
                }
            }
            #endregion

            // switch to pursue target state if we find one
            if (enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }
            else   // if not, stay in this state
            {
                return this;
            }  
        }
    }
}

