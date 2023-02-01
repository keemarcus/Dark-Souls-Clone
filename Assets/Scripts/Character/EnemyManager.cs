using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MK
{
    public class EnemyManager : CharacterManager
    {
        public NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidBody;

        EnemyLocomotion enemyLocomotion;
        EnemyAnimatorHandler enemyAnimatorHandler;
        EnemyStats enemyStats;

        public State currentState;
        public CharacterStats currentTarget;

        public bool isPerformingAction;
        public bool isInteracting;
        

        [Header("AI Settings")]
        public float detectionRadius = 20f;
        public float minimumDetectionAngle = -50f;
        public float maximumDetectionAngle = 50f;
        //public float viewableAngle;

        public float currentRecoveryTime = 0f;

        //public float distanceFromTarget;
        public float maximumAttackRange = 1.5f;

        public float rotationSpeed = 15;

        private void Awake()
        {
            enemyLocomotion = GetComponent<EnemyLocomotion>();
            enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
            enemyStats = GetComponent<EnemyStats>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidBody = GetComponent<Rigidbody>();
            //backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
        }
        private void Start()
        {
            navMeshAgent.enabled = false;
            enemyRigidBody.isKinematic = false;
        }

        private void Update()
        {
            //Debug.Log(enemyLocomotion.navMeshAgent.steeringTarget);
            //Debug.Log(enemyLocomotion.)
            HandleRecoveryTimer();
            isInteracting = enemyAnimatorHandler.anim.GetBool("Is Interacting");
            enemyAnimatorHandler.anim.SetBool("Is Dead", enemyStats.isDead);
        }

        private void FixedUpdate()
        {
            HandleStateMachine();
        }

        private void HandleStateMachine()
        {
            if(currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorHandler);

                if(nextState != null)
                {
                    SwitchToNextState(nextState);
                    //Debug.Log(nextState.name);
                }
            }
        }

        private void SwitchToNextState(State newState)
        {
            currentState = newState;
        }

        private void HandleRecoveryTimer()
        {
            if(currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction)
            {
                if(currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }
    }
}

