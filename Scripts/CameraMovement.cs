using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMovement : MonoBehaviour
{
    [Header("Framing")]
    [SerializeField] private Vector2 FollowPointFraming = new Vector2(0f, 0f);
    [SerializeField] private float FollowingSharpness = 10000f;

    [Header("Distance")]
    [SerializeField] private float DefaultDistance = 6f;
    [SerializeField] private float MinDistance = 0f;
    [SerializeField] private float MaxDistance = 10f;
    [SerializeField] private float DistanceMovementSpeed = 5f;
    [SerializeField] private float DistanceMovementSharpness = 10f;

    [Header("Rotation")]
    [SerializeField] private bool invertX = false;
    [SerializeField] private bool invertY = false;
    [Range(-90f, 90f)]
    [SerializeField] private float DefaultVerticalAngle = 20f;
    [Range(-90f, 90f)]
    [SerializeField] private float MinVerticalAngle = -90f;
    [Range(-90f, 90f)]
    [SerializeField] private float MaxVerticalAngle = 90f;
    [SerializeField] private float RotationSpeed = 1f;
    [SerializeField] private float RotationSharpness = 10000f;

    [Header("Obstruction")]
    [SerializeField] private float ObstructionCheckRadius = 0.2f;
    [SerializeField] private LayerMask ObstructionLayers = -1;
    [SerializeField] private float ObstructionSharpness = 10000f;
    [SerializeField] private List<Collider> IgnoredColliders = new List<Collider>();
    public Transform Transform { get; private set; }
    public Transform FollowTransform { get; private set; }
    public Vector3 PlanarDirection { get; set; }
    public float TargetDistance { get; set; }

    private bool _distanceIsObstructed;
    private float _currentDistance;
    private float _targetVerticalAngle;
    private RaycastHit _obstructionHit;
    private int _obstructionCount;
    private RaycastHit[] _obstructions = new RaycastHit[MaxObstructions];
    private float _obstructionTime;
    private Vector3 _currentFollowPosition;

    private const int MaxObstructions = 32;

    void OnValidate()
    {
        DefaultDistance = Mathf.Clamp(DefaultDistance, MinDistance, MaxDistance);
        DefaultVerticalAngle = Mathf.Clamp(DefaultVerticalAngle, MinVerticalAngle, MaxVerticalAngle);
    }

    void Awake()
    {
        Transform = this.transform;

        _currentDistance = DefaultDistance;
        TargetDistance = _currentDistance;

        invertX = PlayerPrefs.GetInt("Inverted") == 1 ? true : false;
        invertY = PlayerPrefs.GetInt("Inverted") == 1 ? true : false;

        _targetVerticalAngle = 0f;

        PlanarDirection = Vector3.forward;

        SetFollowTransform(FindObjectOfType<Character>().transform);
    }

    private void LateUpdate()
    {
        UpdateWithInput(Time.deltaTime, 0f, new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f));
    }

    public void SetFollowTransform(Transform t)
    {
        FollowTransform = t;
        PlanarDirection = FollowTransform.forward;
        _currentFollowPosition = FollowTransform.position;
    }

    public void UpdateWithInput(float deltaTime, float zoomInput, Vector3 rotationInput)
    {
        if (FollowTransform)
        {
            if (invertX)
            {
                rotationInput.x *= -1f;
            }
            if (invertY)
            {
                rotationInput.y *= -1f;
            }

            Quaternion rotationFromInput = Quaternion.Euler(FollowTransform.up * (rotationInput.x * RotationSpeed));
            PlanarDirection = rotationFromInput * PlanarDirection;
            PlanarDirection = Vector3.Cross(FollowTransform.up, Vector3.Cross(PlanarDirection, FollowTransform.up));
            Quaternion planarRot = Quaternion.LookRotation(PlanarDirection, FollowTransform.up);

            _targetVerticalAngle -= (rotationInput.y * RotationSpeed);
            _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle, MinVerticalAngle, MaxVerticalAngle);
            Quaternion verticalRot = Quaternion.Euler(_targetVerticalAngle, 0, 0);
            Quaternion targetRotation = Quaternion.Slerp(Transform.rotation, planarRot * verticalRot, 1f - Mathf.Exp(-RotationSharpness * deltaTime));

            Transform.rotation = targetRotation;

            if (_distanceIsObstructed && Mathf.Abs(zoomInput) > 0f)
            {
                TargetDistance = _currentDistance;
            }
            TargetDistance += zoomInput * DistanceMovementSpeed;
            TargetDistance = Mathf.Clamp(TargetDistance, MinDistance, MaxDistance);

            _currentFollowPosition = Vector3.Lerp(_currentFollowPosition, FollowTransform.position, 1f - Mathf.Exp(-FollowingSharpness * deltaTime));

            {
                RaycastHit closestHit = new RaycastHit();
                closestHit.distance = Mathf.Infinity;
                _obstructionCount = Physics.SphereCastNonAlloc(_currentFollowPosition, ObstructionCheckRadius, -Transform.forward, _obstructions, TargetDistance, ObstructionLayers, QueryTriggerInteraction.Ignore);
                for (int i = 0; i < _obstructionCount; i++)
                {
                    bool isIgnored = false;
                    for (int j = 0; j < IgnoredColliders.Count; j++)
                    {
                        if (IgnoredColliders[j] == _obstructions[i].collider)
                        {
                            isIgnored = true;
                            break;
                        }
                    }
                    for (int j = 0; j < IgnoredColliders.Count; j++)
                    {
                        if (IgnoredColliders[j] == _obstructions[i].collider)
                        {
                            isIgnored = true;
                            break;
                        }
                    }

                    if (!isIgnored && _obstructions[i].distance < closestHit.distance && _obstructions[i].distance > 0)
                    {
                        closestHit = _obstructions[i];
                    }
                }


                if (closestHit.distance < Mathf.Infinity)
                {
                    _distanceIsObstructed = true;
                    _currentDistance = Mathf.Lerp(_currentDistance, closestHit.distance, 1 - Mathf.Exp(-ObstructionSharpness * deltaTime));
                }

                else
                {
                    _distanceIsObstructed = false;
                    _currentDistance = Mathf.Lerp(_currentDistance, TargetDistance, 1 - Mathf.Exp(-DistanceMovementSharpness * deltaTime));
                }
            }

            Vector3 targetPosition = _currentFollowPosition - ((targetRotation * Vector3.forward) * _currentDistance);

            targetPosition += Transform.right * FollowPointFraming.x;
            targetPosition += Transform.up * FollowPointFraming.y;

            Transform.position = targetPosition;
        }
    }
}