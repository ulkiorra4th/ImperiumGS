using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public bool IsGrounded { get; set; }

    [SerializeField] private float detectionDistance = 0.1f;
    [SerializeField] private LayerMask detectableLayers;
    private Ray ray;

    private void Update()
    {
        ray = new Ray(transform.position, -transform.up);
        IsGrounded = Physics.Raycast(ray, detectionDistance, detectableLayers);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up * detectionDistance);
    }
}