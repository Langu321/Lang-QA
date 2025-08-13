using UnityEngine;
using Cinemachine;

public class OnTriggerZone : MonoBehaviour
{
    public CinemachineVirtualCamera targetCam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraDirector.Instance.SwitchToCamera(targetCam);
        }
    }
}
