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
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

            enemyManager.viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            if (enemyManager.isPerformingAction) { return combatStanceState; }

            if(currentAttack != null)
            {
                // if the selected attack is not able to be used, select a new one
                if(enemyManager.distanceFromTarget < currentAttack.minimumDistanceNeededToAttack ||
                   enemyManager.distanceFromTarget > currentAttack.maximumDistanceNeededToAttack)
                {
                    return this;
                }else
                {
                    if(enemyManager.viewableAngle <= currentAttack.maximumAttackAngle &&
                       enemyManager.viewableAngle >= currentAttack.minimumAttackAngle)
                    {
                        if(enemyManager.currentRecoveryTime <= 0 && !enemyManager.isPerformingAction)
                        {
                            // stop movement and attack the target
                            enemyAnimatorHandler.anim.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
                            enemyAnimatorHandler.anim.SetFloat("Horizontal", 0f, 0.1f, Time.deltaTime);
                            enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
                            enemyManager.isPerformingAction = true;

                            // set recovery timer
                            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;

                            currentAttack = null;

                            return combatStanceState;
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
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            enemyManager.viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

            int maxScore = 0;

            foreach(EnemyAttackAction enemyAttackAction in enemyAttacks)
            {
                if(enemyManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && enemyManager.distanceFromTarget >= enemyAttackAction.minimumAttackAngle)
                {
                    if(enemyManager.viewableAngle <= enemyAttackAction.maximumAttackAngle && enemyManager.viewableAngle >= enemyAttackAction.minimumAttackAngle)
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

                if (enemyManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && enemyManager.distanceFromTarget >= enemyAttackAction.minimumAttackAngle)
                {
                    if (enemyManager.viewableAngle <= enemyAttackAction.maximumAttackAngle && enemyManager.viewableAngle >= enemyAttackAction.minimumAttackAngle)
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
    }
}

