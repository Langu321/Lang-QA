using Cinemachine;
using UnityEngine;
using System.Collections.Generic;

public class TemporaryCamOverride : MonoBehaviour
{
    [SerializeField] public float rotationLimit = 5f; // Giới hạn tối đa ±5 độ

    private KeyCode[] rotateKeys = new KeyCode[] { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };
    private float rotateSpeed = 50f;

    private CinemachineVirtualCamera overrideCam;
    private bool isOverriding = false;

    private void Awake()
    {
        overrideCam = GetComponent<CinemachineVirtualCamera>();
        overrideCam.Priority = 0;
    }

    void Update()
    {
        var validKeys = GetValidPressedKeys();

        if (validKeys.Count > 0 && !isOverriding)
        {
            StartOverride();
        }
        else if (validKeys.Count == 0 && isOverriding)
        {
            EndOverride();
        }

        if (isOverriding && validKeys.Count > 0)
        {
            HandleRotation(validKeys);
        }
    }

    List<KeyCode> GetValidPressedKeys()
    {
        List<KeyCode> heldKeys = new List<KeyCode>();
        foreach (var key in rotateKeys)
        {
            if (Input.GetKey(key))
                heldKeys.Add(key);
        }

        // Cho phép nếu chỉ có 1 phím hoặc 2 phím LIỀN KỀ theo logic hướng
        if (heldKeys.Count == 1)
        {
            return heldKeys;
        }
        else if (heldKeys.Count == 2)
        {
            // Kiểm tra xem 2 phím có hợp lệ (không đối nghịch)
            if ((heldKeys.Contains(KeyCode.UpArrow) && heldKeys.Contains(KeyCode.RightArrow)) ||
                (heldKeys.Contains(KeyCode.UpArrow) && heldKeys.Contains(KeyCode.LeftArrow)) ||
                (heldKeys.Contains(KeyCode.DownArrow) && heldKeys.Contains(KeyCode.RightArrow)) ||
                (heldKeys.Contains(KeyCode.DownArrow) && heldKeys.Contains(KeyCode.LeftArrow)))
            {
                return heldKeys;
            }
        }

        // Không hợp lệ: return empty list
        return new List<KeyCode>();
    }

    void StartOverride()
    {
        Camera mainCamera = Camera.main;

        isOverriding = true;

        if (mainCamera != null)
        {
            overrideCam.transform.position = mainCamera.transform.position;
            overrideCam.transform.rotation = mainCamera.transform.rotation;
        }

        overrideCam.Priority = 100;
    }

    void EndOverride()
    {
        isOverriding = false;
        overrideCam.Priority = 0;

        if (CameraDirector.Instance != null && CameraDirector.Instance.TryGetComponent<CinemachineVirtualCamera>(out var mainCam))
        {
            mainCam.Priority = 20;
        }
    }

    void HandleRotation(List<KeyCode> validKeys)
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (validKeys.Contains(KeyCode.LeftArrow)) horizontal = -1f;
        if (validKeys.Contains(KeyCode.RightArrow)) horizontal = 1f;
        if (validKeys.Contains(KeyCode.UpArrow)) vertical = 1f;
        if (validKeys.Contains(KeyCode.DownArrow)) vertical = -1f;

        Vector3 rotateDir = new Vector3(-vertical, horizontal, 0f);
        overrideCam.transform.Rotate(rotateDir * rotateSpeed * Time.deltaTime, Space.Self);
    }
}
