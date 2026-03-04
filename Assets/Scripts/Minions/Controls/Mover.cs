using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Controls
{
    /// <summary>
    /// Minion'un hareket kabiliyetini sarmalayan (wrapper) sınıf.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class Mover : MonoBehaviour
    {
        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public void MoveTo(Vector3 target)
        {
            if (_agent.isOnNavMesh)
            {
                _agent.SetDestination(target);
                _agent.isStopped = false;
            }
        }

        public void Stop()
        {
            if (_agent.isOnNavMesh)
            {
                _agent.isStopped = true;
                _agent.ResetPath();
            }
        }

        public bool HasReached(Vector3 target, float stopDistance)
        {
            return Vector3.Distance(transform.position, target) <= stopDistance;
        }
    }
}