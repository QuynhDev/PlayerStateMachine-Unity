using UnityEngine;
using UnityEngine.Windows;

public class PlayerCameraManager : MonoBehaviour
{
    [Header("Camera Settings")]
    public GameObject CinemachineCameraTarget; // Object that camera will follow
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    public float SensitivityX = 40f;
    public float SensitivityY = 40f;

    private float _cinemachineTargetYaw; // Horizontal
    private float _cinemachineTargetPitch; // Vertical 
    private const float _threshold = 0.01f;

    private void LateUpdate()
    {
        Vector2 lookInput = GameInput.Instance.GetLookVector();

        if (lookInput.sqrMagnitude >= _threshold)
        {
            _cinemachineTargetYaw += lookInput.x * SensitivityX * Time.deltaTime;
            _cinemachineTargetPitch -= lookInput.y * SensitivityY * Time.deltaTime; 
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch,
            _cinemachineTargetYaw, 0.0f);
    }

    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
