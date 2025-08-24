using Cinemachine;
using UnityEngine;

public class CameraDirector : MonoBehaviour
{
    [SerializeField] protected float tiltSpeed = 30f;
    [SerializeField] protected float maxTiltAngle = 5f;

    public static CameraDirector Instance;

    private CinemachineVirtualCamera currentCam;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SwitchToCamera(CinemachineVirtualCamera newCam)
    {
        if (newCam == currentCam) return;

        if (currentCam != null)
            currentCam.Priority = 0;

        newCam.Priority = 20;
        currentCam = newCam;
    }

    
}
