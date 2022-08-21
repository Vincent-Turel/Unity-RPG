using System;
using Cameras;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CameraMouseControls : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAction;
    [SerializeField] private InputProvider customInputProvider;
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float zoomAcceleration = 4f;
    [SerializeField] private float zoomInnerRange = 5;
    [SerializeField] private float zoomOuterRange = 20;

    private float currentMiddleRigRadius = 9f;
    private float newMiddleRigRadius = 9f;

    [SerializeField] private float zoomYAxis = 0f;
    

    public float ZoomYAxis
    {
        get => zoomYAxis;
        set
        {
            if (zoomYAxis == value) return;
            zoomYAxis = value;
            AdjustCameraZoomIndex(ZoomYAxis);
        }
    }

    private void Update()
    {
        customInputProvider.InputEnabled = Input.GetMouseButton(1);
        freeLookCamera.transform.Rotate(0,0,0);
    }

    private void Awake()
    {
        inputAction.FindActionMap("Mouse").FindAction("Zoom").performed +=
            cntxt => ZoomYAxis = cntxt.ReadValue<float>();
        inputAction.FindActionMap("Mouse").FindAction("Zoom").canceled += cntxt => ZoomYAxis = 0;
    }

    private void OnEnable()
    {
        inputAction.FindAction("Zoom").Enable();
    }

    private void OnDisable()
    {
        inputAction.FindAction("Zoom").Disable();
    }

    private void LateUpdate()
    {
        UpdateZoomLevel();
    }
    // ReSharper disable once CompareOfFloatsByEqualityOperator

    private void UpdateZoomLevel()
    {
        if (currentMiddleRigRadius == newMiddleRigRadius) return;
        currentMiddleRigRadius = Mathf.Lerp(currentMiddleRigRadius, newMiddleRigRadius, zoomAcceleration * Time.deltaTime);
        currentMiddleRigRadius = Mathf.Clamp(currentMiddleRigRadius, zoomInnerRange, zoomOuterRange);

        for (int i = 0; i < freeLookCamera.m_Orbits.Length; i++)
        {
            freeLookCamera.m_Orbits[i].m_Height = currentMiddleRigRadius * 2;
            freeLookCamera.m_Orbits[i].m_Radius = currentMiddleRigRadius;
        }
    }
    public void AdjustCameraZoomIndex(float zoomYAxis)
    {
        if (zoomYAxis == 0) return;
        if (zoomYAxis < 0)
        {
            newMiddleRigRadius = currentMiddleRigRadius + zoomSpeed;
        }
        if (zoomYAxis > 0)
        {
            newMiddleRigRadius = currentMiddleRigRadius - zoomSpeed;
        }
    }
}
