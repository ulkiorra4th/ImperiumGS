using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMesh))]
public class EnemyAI : MonoBehaviour
{
    #region Serialized Fields

    [Header("Inspection Of the Territory Settings")]
    [Tooltip("Радиус по которому будет ходить враг в поисках игрока")]
    [SerializeField] private float radius;

    [Space(5)]

    [Header("Detection settings")]
    [SerializeField] private Transform eye;
    [Tooltip("Все, так сказать, непрозрачные объекты для врага")]
    [SerializeField] private LayerMask layerMask;

    [Tooltip("Угол обзора врага")]
    [Range(0, 360)]
    [SerializeField] private float fieldOfView;

    [Tooltip("Дальность видимости врага")]
    [SerializeField] private float visibilityDistance;

    [Tooltip("Дистанция, на которой враг обнаружит игрока в независимости от того, находится тот в области  видимости или нет")]
    [SerializeField] private float detectionDistance;
    [Tooltip("Дистанция, на которой враг останавливается")]
    [SerializeField] private float stoppingDistance;

    [Space(5)]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    [Space(5)]

    [Header("Shooting Settings")]

    [Tooltip("Время реакции врага")]
    [SerializeField] private float reactionTime;
    [Tooltip("Если accuracy == 0 то меткость врага = 100%")]
    [Range(0, 1)]
    [SerializeField] private float accuracy;

    private Transform target;
    private NavMeshAgent agent;
    private Vector3 startPosition;
    private Vector3 positionInTerritory;
    private NavMeshPath navMeshPath;

    private float firingDistance;

    #endregion

    #region Unity

    private void Start()
    {
        target = FindObjectOfType<Character>().transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        positionInTerritory = startPosition = transform.position;
        navMeshPath = new NavMeshPath();
    }

    private void Update()
    {
        var distanceToTarget = Vector3.Distance(transform.position, target.position);
        var isInView = IsInView(distanceToTarget);

        if (distanceToTarget <= detectionDistance || isInView)
        {
            agent.SetDestination(target.position);

            if (distanceToTarget <= stoppingDistance && isInView)
            {
                agent.speed = 0;
                RotateToTarget(target);

            }
            else if (agent.speed == 0)
            {
                agent.speed = speed;
            }

            if (PlayerInFireDistance())
            {
                StartCoroutine(ShootThePlayer());
            }
        }
        else
        {
            WalkAroundTheTerritory(radius);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(startPosition, radius);
    }

    #endregion

    #region Methods

    private bool IsInView(float distanceToTarget)
    {
        var angleBetweenEnemyAndTarget = Vector3.Angle(eye.forward, target.position - eye.position);

        RaycastHit hit;
        if (Physics.Raycast(eye.position, target.position - transform.position, out hit, visibilityDistance, layerMask))
        {
            if (angleBetweenEnemyAndTarget < fieldOfView / 2 && distanceToTarget <= visibilityDistance && hit.transform == target.transform)
                return true;
        }

        return false;
    }

    private void RotateToTarget(Transform target)
    {
        var endAngle = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, endAngle, rotationSpeed);
    }

    private void WalkAroundTheTerritory(float radius)
    {
        if (transform.position == new Vector3(positionInTerritory.x, transform.position.y, positionInTerritory.z))
            positionInTerritory = startPosition + UnityEngine.Random.insideUnitSphere * radius;

        NavMeshHit hit;
        NavMesh.SamplePosition(positionInTerritory, out hit, radius, NavMesh.AllAreas);
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
    }

    private bool PlayerInFireDistance()
    {
        RaycastHit hit;
        if (Physics.Raycast(eye.position, eye.forward, out hit, firingDistance, layerMask))
        {
            if (hit.transform == target.transform) return true;
        }

        return false;
    }

    private IEnumerator ShootThePlayer()
    {
        var targetPosition = target.position;

        yield return new WaitForSeconds(reactionTime);
    }

    #endregion
}