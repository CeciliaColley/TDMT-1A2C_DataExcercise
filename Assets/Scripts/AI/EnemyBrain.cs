using System;
using Core.Interactions;
using UnityEngine;
using DataSource;
using Characters;

namespace AI
{
    public class EnemyBrain : MonoBehaviour
    {
        [SerializeField] private float attackDistance;
        [SerializeField] private DS_Character targetSource;
        private ISteerable _steerable;

        private void Awake()
        {
            _steerable = GetComponent<ISteerable>();
            if (_steerable == null)
            {
                Debug.LogError($"{name}: cannot find a {nameof(ISteerable)} component!");
                enabled = false;
            }
        }

        private void Update()
        {
            //TODO: Add logic to get the target from a source/reference system
            // To ensure SOLID code standards, this TODO was completed from the character script.

            if (targetSource.Reference == null)
            {
                return;
            }
            //          AB        =         B        -          A
            var directionToTarget = targetSource.Reference.transform.position - transform.position;
            var distanceToTarget = directionToTarget.magnitude;
            if (distanceToTarget < attackDistance)
            {
                targetSource.Reference.ReceiveAttack();
            }
            else
            {
                Debug.DrawRay(transform.position, directionToTarget.normalized, Color.red);
                _steerable.SetDirection(directionToTarget.normalized);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }

        
    }
}
