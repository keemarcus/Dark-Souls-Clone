using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MK
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public PursueTargetState pursueTargetState;
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            // update the distance from target
            enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            // potentially circle around the target

            // check for attack range
            if (enemyManager.currentRecoveryTime <= 0 && enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                // if in attack range, switch to attack state
                return attackState;
            }else if(enemyManager.distanceFromTarget > enemyManager.maximumAttackRange)
            {
                // if the player is out of range, switch back to the pursue target state
                return pursueTargetState;
            }
            else
            {
                // if we're in a cooldown, stay in this state
                return this;
            }
        }
    }
}

