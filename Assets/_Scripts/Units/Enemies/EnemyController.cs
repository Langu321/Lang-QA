using UnityEngine;

/// <summary>
/// Script điều khiển quái NPC.
/// - Liên tục tìm và di chuyển về phía Quang Anh.
/// - Có thể cấu hình tốc độ di chuyển, tầm nhìn.
/// - Khi chạm vào người chơi → gọi Game Over.
/// - TODO: Nếu hai quái khác nhau thì chuyển class này thành abstract rồi tạo các script con con thừng quái nhé 
/// </summary>
public class EnemyController : MonoBehaviour
{
    // ----- Cấu hình -----
    public float moveSpeed;         // Tốc độ di chuyển của quái
    public float detectionRange;    // Tầm nhìn (phát hiện player)

    // ----- Thành phần tham chiếu -----
    private Transform player;       // Vị trí của Quang Anh
    private Rigidbody rb;           // Rigidbody để di chuyển vật lý
    private Animator animator;      // Animator cho animation quái

    // ----- Logic -----
    private void Awake() { /* Tìm player và gán biến */ }
    private void Update() { /* Xác định hướng đuổi */ }
    private void FixedUpdate() { /* Di chuyển về phía player */ }

    // ----- Xử lý va chạm -----
    private void OnTriggerEnter(Collider other) { /* Nếu chạm player thì Game Over */ }

    // ----- Hỗ trợ -----
    private void ChasePlayer() { /* AI đuổi theo player */ }
    private bool CanSeePlayer() { /* Kiểm tra player trong tầm nhìn */ return false; }
}
