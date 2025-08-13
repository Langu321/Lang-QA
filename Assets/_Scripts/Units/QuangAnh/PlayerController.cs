using StarterAssets;
using UnityEngine;
using System;

/// <summary>
/// Chứa toàn bộ logic gameplay của nhân vật Quang Anh.
/// - TODO Xử lý khi bấm nút tương tác (gọi IInteractable nếu có). 
/// - Xử lý khi chết (disable input, gọi GameManager).
/// - Có thể mở rộng để xử lý các trigger đặc biệt trong game.
/// - Cho phép tương tác với vật thể trong môi trường (cửa, note, tủ…).
/// - Phát hiện va chạm quái → Invoke sự kiện chết.
/// </summary>

public class PlayerController : MonoBehaviour
{
    // Reference tới ThirdPersonController để enable/disable khi chết
    [SerializeField] private ThirdPersonController movementController;

    public static event Action OnPlayerDie;

    public bool isAlive = true;
    private void Awake()
    {
        movementController = GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        HandleInput();
    }

    // ----- Tương tác -----
    private void HandleInput()
    {
        // Nhấn phím tương tác
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAlive) return;

        if (other.CompareTag("Enemy"))
        {
            Die();
        }
    }

    // ----- Tương tác -----
    public void TryInteract()
    {
        if (!isAlive) return;

        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Interact();
            }
        }
    }

    public void Die()
    {
        isAlive = false;
        EnableMovement(false);
        Debug.Log("Quang Anh đã bị bắt!");
        OnPlayerDie?.Invoke();
    }

    // ----- Tắt tương tác khi chết hoặc cutscene -----
    public void EnableMovement(bool enable)
    {
        if (movementController != null)
            movementController.enabled = enable;
    }
}
