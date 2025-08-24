using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Quản lý trạng thái tổng thể của game.
/// - Nghe sự kiện player chết hoặc hoàn thành.
/// - Tách SystemState (hệ thống) và GameplayState (chơi game).
/// - Chịu trách nhiệm restart scene hoặc chuyển level.
/// </summary>
public class GameManager : StaticInstance<GameManager>
{
    // ===== CURRENT ROOM (3) =====
    public int currentRoomIndex = 0;

    // ===== STATES =====
    public SystemState SystemState { get; private set; }
    public GameplayState GameplayState { get; private set; }

    // ====== EVENTS ======
    public static event Action<SystemState> OnBeforeSystemStateChanged;
    public static event Action<SystemState> OnAfterSystemStateChanged;

    public static event Action<GameplayState> OnBeforeGameplayStateChanged;
    public static event Action<GameplayState> OnAfterGameplayStateChanged;

    // ===== REFERENCES =====
    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private EnemyController[] enemies;

    // ===== LIFECYCLE =====
    private void OnEnable()
    {
        PlayerController.OnPlayerDie += HandlePlayerDie;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDie -= HandlePlayerDie;
    }

    // Kick the game off with the first state
    private void Start()
    {
        ChangeSystemState(SystemState.Booting);
        ChangeGameplayState(GameplayState.Playing);
    }

    // ===== CHANGE ROOM =====
    public void ChangeRoom(int newRoomIndex)
    {
        currentRoomIndex = newRoomIndex;
        Debug.Log($"[GameManager] Chuyển sang bối cảnh: {newRoomIndex}");

        // Load background mới, di chuyển player, spawn enemy nếu cần
        //RoomManager.Instance.LoadRoom(newRoomIndex);
    }


    // ===== STATE CHANGERS =====
    public void ChangeSystemState(SystemState newState)
    {
        OnBeforeSystemStateChanged?.Invoke(newState);

        SystemState = newState;
        switch (newState)
        {
            case SystemState.Booting:
                HandleBooting();
                break;
            case SystemState.MainMenu:
                HandleMainMenu();
                break;
            case SystemState.LoadingScene:
                HandleLoadingScene();
                break;
            case SystemState.Paused:
                HandlePause();
                break;
        }

        OnAfterSystemStateChanged?.Invoke(newState);
        Debug.Log($"[GameManager] SystemState: {newState}");
    }

    public void ChangeGameplayState(GameplayState newState)
    {
        OnBeforeGameplayStateChanged?.Invoke(newState);

        GameplayState = newState;
        switch (newState)
        {
            case GameplayState.Tutorial:
                HandleTutorial();
                break;
            case GameplayState.Playing:
                HandlePlaying();
                break;
            case GameplayState.Cutscene:
                HandleCutscene();
                break;
            case GameplayState.Died:
                HandlePlayerDie();
                break;
            case GameplayState.Ending:
                HandleEnding();
                break;
        }

        OnAfterGameplayStateChanged?.Invoke(newState);
        Debug.Log($"[GameManager] GameplayState: {newState}");
    }

    // ====== SYSTEM STATE HANDLERS ======
    private void HandleBooting()
    {
        Debug.Log("Hệ thống khởi động...");
        ChangeSystemState(SystemState.MainMenu);
    }

    private void HandleMainMenu()
    {
        // Hiện UI menu chính
    }

    private void HandleLoadingScene()
    {
        // Load scene async
    }

    private void HandlePause()
    {
        Time.timeScale = 0f;
    }

    // ====== GAMEPLAY STATE HANDLERS ======
    private void HandleStarting()
    {
        // Do some start setup, could be environment, cinematics etc

        // Eventually call ChangeState again with your next state

        ChangeSystemState(SystemState.MainMenu);
    }

    private void HandleTutorial()
    {
        ExampleUnitManager.Instance.SpawnHeroes();

        ChangeGameplayState(GameplayState.Cutscene);
    }

    private void HandleCutscene()
    {
        Debug.Log("Chạy cắt cảnh...");
        ChangeGameplayState(GameplayState.Playing);
    }

    private void HandlePlaying()
    {
        // If you're making a turn based game, this could show the turn menu, highlight available units etc

        // Keep track of how many units need to make a move, once they've all finished, change the state. This could
        // be monitored in the unit manager or the units themselves.

        Debug.Log("Bắt đầu gameplay...");
    }

    private void HandlePlayerDie()
    {
        Debug.Log("GameManager: Player chết");

        SetPlayerActive(false);
        SetEnemiesActive(false);

        RestartLevel();
        ChangeGameplayState(GameplayState.Ending);
    }

    private void HandleEnding()
    {
        Debug.Log("Kết thúc màn chơi...");
        RestartLevel();
    }

    // ====== PLAYER EVENT HANDLERS ======
    
    private void SetPlayerActive(bool active)
    {
        playerController.EnableMovement(active);
    }

    private void SetEnemiesActive(bool active)
    {
        foreach (var e in enemies)
            if (e != null) e.enabled = active;
    }

    public void RestartLevel()
    {
        //Cách 1:
        //LevelManager.Instance.LoadLevel(currentLevelIndex);
        //playerController.transform.position = lastCheckpointPos;
        //playerController.EnableMovement(true);

        //Cách 2:
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

// ===== ENUMS =====
[Serializable]
public enum SystemState
{
    Booting,
    MainMenu,
    LoadingScene,
    Paused
}

[Serializable]
public enum GameplayState
{
    Tutorial,
    Playing,
    Cutscene,
    Died,
    Ending
}