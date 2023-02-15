using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class AttackState : State
    {
        public CombatStanceState combatStanceState;

        public EnemyAttackAction currentAttack;
        public EnemyAttackAction[] enemyAttacks;

        bool willComboOnNextAttack = false;
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            if (enemyManager.isInteracting && !enemyManager.canDoCombo) { return this; }
            else if(enemyManager.isInteracting && enemyManager.canDoCombo)
            {
                if (willComboOnNextAttack)
                {
                    willComboOnNextAttack = false;
                    enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
                }
            }

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

            HandleRotateTowardsTarget(enemyManager, distanceFromTarget);

            if (enemyManager.isPerformingAction) { return combatStanceState; }

            
            if(currentAttack != null)
            {
                //Debug.Log(currentAttack.name);
                // if the selected attack is not able to be used, select a new one
                if (distanceFromTarget < currentAttack.minimumDistanceNeededToAttack ||
                   distanceFromTarget > currentAttack.maximumDistanceNeededToAttack)
                {
                    return this;
                }else
                {
                    if(viewableAngle <= currentAttack.maximumAttackAngle &&
                       viewableAngle >= currentAttack.minimumAttackAngle)
                    {
                        if(enemyManager.currentRecoveryTime <= 0 && !enemyManager.isPerformingAction)
                        {
                            // stop movement and attack the target
                            enemyAnimatorHandler.anim.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
                            enemyAnimatorHandler.anim.SetFloat("Horizontal", 0f, 0.1f, Time.deltaTime);
                            // set which hand the enemy is attacking with
                            enemyAnimatorHandler.anim.SetFloat("Is Left Hand", -1f);
                            enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
                            enemyManager.isPerformingAction = true;
                            RollForComboChance(enemyManager);

                            if (currentAttack.canCombo && willComboOnNextAttack)
                            {
                                currentAttack = currentAttack.comboAttack;
                                return this;
                            }
                            else
                            {
                                // set recovery timer
                                enemyManager.currentRecoveryTime = currentAttack.recoveryTime;

                                currentAttack = null;

                                return combatStanceState;
                            }
                            
                        }
                    }
                }
            }
            else
            {
                // select one of our attacks
                GetNewAttack(enemyManager);
            }

            return combatStanceState;
        }

        private void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            int maxScore = 0;

            foreach(EnemyAttackAction enemyAttackAction in enemyAttacks)
            {
                if(distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && distanceFromTarget >= enemyAttackAction.minimumAttackAngle)
                {
                    if(viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            foreach (EnemyAttackAction enemyAttackAction in enemyAttacks)
            {
                if (currentAttack != null)
                {
                    return;
                }

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && distanceFromTarget >= enemyAttackAction.minimumAttackAngle)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        temporaryScore += enemyAttackAction.attackScore;
                        
                        if(temporaryScore > randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }

        private void RollForComboChance(EnemyManager enemyManager)
        {
            float comboChance = Random.Range(0f, 100f);
            //Debug.Log(comboChance);

            if (enemyManager.allowAICombo && comboChance <= enemyManager.comboLikelihood)
            {
                willComboOnNextAttack = true;
            }
            //Debug.Log(willComboOnNextAttack);
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

