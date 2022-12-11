using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewController : MonoBehaviour
{
    [SerializeField] private float runFieldOfView;
    private float defaultFieldOfView;

    [SerializeField] private float smoothness;
     
    private CharacterMovement characterMovement;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        defaultFieldOfView = mainCamera.fieldOfView;
        characterMovement = FindObjectOfType<CharacterMovement>();
    }

    private void LateUpdate()
    {
        if (characterMovement.IsRunning && characterMovement.CurrentSpeed >= 2f) mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, runFieldOfView, smoothness);
        else mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, defaultFieldOfView, smoothness);
    }
}
