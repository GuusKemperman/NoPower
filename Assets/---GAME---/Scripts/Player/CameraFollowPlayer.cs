using Unity.Cinemachine;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private Camera mainCamera = null;
    void Start()
    {
        SetupCameraFollow();
    }

    private void SetupCameraFollow()
    {
        mainCamera = Camera.main;
        CinemachineCamera brain = mainCamera.GetComponent<CinemachineCamera>();
        brain.Follow = transform;
        brain.LookAt = transform;
    }
}
