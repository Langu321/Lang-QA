using System;
using UnityEngine;

/// <summary>
/// Quản lý nội bộ của scene hiện tại (bật/tắt level nhỏ, spawn, trigger môi trường, checkpoint trong scene).
/// - Chỉ active các level gần người chơi hoặc checkpoint hiện tại.
/// - Lưu vị trí checkpoint gần nhất của player trong scene.
/// - Kết nối với cutscene hoặc gameplay event của GameManager.
/// </summary>
public class LevelManager : MonoBehaviour
{
    private void Start()
    {

    }

}