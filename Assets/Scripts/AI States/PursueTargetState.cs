using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class PursueTargetState : State
    {
        public CombatStanceState combatStanceState;
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            if (enemyManager.isInteracting) { return this; }

            // chase the target
            #region Chase Target
            if (enemyManager.isPerformingAction) {
                enemyAnimatorHandler.anim.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
                return this; 
            }

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

            if (distanceFromTarget > enemyManager.maximumAttackRange)
            {
                enemyAnimatorHandler.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            HandleRotateTowardsTarget(enemyManager, distanceFromTarget);
            #endregion

            // if we're within attack range, switch to combat stance state
            if(distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                return combatStanceState;
            }
            else   // if not, stay in this state
            {
                return this;
            }  
        }

        private void HandleRotateTowardsTarget(EnemyManager enemyManager, float distanceFromTarget)
        {
            // rotate manually
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = enemyManager.transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
            else // use the navmeshagent to rotate
            {
                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);

                float rotationToApplyToDynamicEnemy = Quaternion.Angle(enemyManager.transform.rotation, Quaternion.LookRotation(enemyManager.navMeshAgent.desiredVelocity.normalized));
                if (distanceFromTarget > 5) enemyManager.navMeshAgent.angularSpeed = 500f;
                else if (distanceFromTarget < 5 && Mathf.Abs(rotationToApplyToDynamicEnemy) < 30) enemyManager.navMeshAgent.angularSpeed = 50f;
                else if (distanceFromTarget < 5 && Mathf.Abs(rotationToApplyToDynamicEnemy) > 30) enemyManager.navMeshAgent.angularSpeed = 500f;

                Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
                Quaternion rotationToApplyToStaticEnemy = Quaternion.LookRotation(targetDirection);


                if (enemyManager.navMeshAgent.desiredVelocity.magnitude > 0)
                {
                    enemyManager.navMeshAgent.updateRotation = false;
                    enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation,
                    Quaternion.LookRotation(enemyManager.navMeshAgent.desiredVelocity.normalized), enemyManager.navMeshAgent.angularSpeed * Time.deltaTime);
                }
                else
                {
                    enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation, rotationToApplyToStaticEnemy, enemyManager.navMeshAgent.angularSpeed * Time.deltaTime);
                }

                //Vector3 relativeDirection = enemyManager.transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                //Vector3 targetVelocity = enemyManager.enemyRigidBody.velocity;

                //enemyManager.navMeshAgent.enabled = true;
                //enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                //enemyManager.enemyRigidBody.velocity = targetVelocity;
                //enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }
    }
}

