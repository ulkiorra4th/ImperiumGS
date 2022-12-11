using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Curse.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CursingObjectAI : MonoBehaviour
    {
        [Tooltip("Радиус окружности, внутри которой будет перемещаться объект")]
        [SerializeField] private float movingRadius;

        [Tooltip("Радиус окружности, при пересечении которой объект телепортируется")]
        [SerializeField] private float teleportationTriggerRadius;

        [SerializeField] private Character character;

        private NavMeshAgent agent;
        private NavMeshPath navMeshPath;

        private Vector3 startPosition;
        private Vector3 positionInTerritory;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            navMeshPath = new NavMeshPath();

            startPosition = transform.position;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M)) StartCoroutine(MoveToRandomPoint());

            if (Vector3.Distance(transform.position, character.transform.position) <= teleportationTriggerRadius)
            {
                StopAllCoroutines();
                TeleportToRandomPoint();
                StartCoroutine(MoveToRandomPoint());
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(startPosition, movingRadius);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, teleportationTriggerRadius);
        }

        public IEnumerator MoveToRandomPoint()
        {
            var randomPointInCircle = UnityEngine.Random.insideUnitSphere;
            randomPointInCircle.y = 0;

            positionInTerritory = startPosition + randomPointInCircle * movingRadius;

            while (transform.position != positionInTerritory) 
            {
                NavMeshHit hit;
                NavMesh.SamplePosition(positionInTerritory, out hit, movingRadius, NavMesh.AllAreas);
                agent.CalculatePath(hit.position, navMeshPath);

                if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    positionInTerritory = hit.position;
                    agent.SetDestination(positionInTerritory);
                }
                else
                {
                    positionInTerritory = transform.position;
                }

                Debug.Log($"Next target: {positionInTerritory}");

                yield return new WaitForEndOfFrame();
            }

            yield return null;
        }

        private void TeleportToRandomPoint()
        {
            var randomPointInCircle = UnityEngine.Random.insideUnitSphere;
            randomPointInCircle.y = 0;

            transform.position = startPosition + randomPointInCircle * movingRadius;
        }
    }
}