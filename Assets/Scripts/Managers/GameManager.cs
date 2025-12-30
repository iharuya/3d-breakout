using System;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// ゲーム状態
/// </summary>
public enum GameState
{
  Title,
  Playing,
  Paused,
  Cleared,
  GameOver
}

/// <summary>
/// ゲーム全体を管理するシングルトン
/// </summary>
public class GameManager : MonoBehaviour
{
  public static GameManager Instance { get; private set; }

  [Header("設定")]
  [Tooltip("初期ライフ数")]
  [SerializeField] private int initialLives = 3;

  [Header("参照")]
  [Tooltip("ボールオブジェクト")]
  [SerializeField] private BallController ball;

  [Tooltip("ブロック生成オブジェクト")]
  [SerializeField] private BlockSpawner blockSpawner;

  private GameState currentState = GameState.Title;
  private int score = 0;
  private int lives = 3;
  private int totalBlocks = 0;

  // イベント
  public event Action<GameState> OnGameStateChanged;
  public event Action<int> OnScoreChanged;
  public event Action<int> OnLivesChanged;

  // プロパティ
  public GameState CurrentState => currentState;
  public int Score => score;
  public int Lives => lives;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Destroy(gameObject);
    }
  }

  private void Start()
  {
    Assert.IsNotNull(ball, "Ball が設定されていません");
    Assert.IsNotNull(blockSpawner, "BlockSpawner が設定されていません");

    lives = initialLives;
    SetState(GameState.Title);
  }

  /// <summary>
  /// ゲーム状態を変更
  /// </summary>
  private void SetState(GameState newState)
  {
    currentState = newState;
    OnGameStateChanged?.Invoke(newState);

    // 状態に応じた処理
    switch (newState)
    {
      case GameState.Playing:
        Time.timeScale = 1f;
        break;
      case GameState.Paused:
        Time.timeScale = 0f;
        break;
      case GameState.Cleared:
      case GameState.GameOver:
        Time.timeScale = 0f;
        break;
    }
  }

  /// <summary>
  /// ゲーム状態を初期化してPlaying状態へ
  /// </summary>
  private void InitializeGame()
  {
    score = 0;
    lives = initialLives;
    OnScoreChanged?.Invoke(score);
    OnLivesChanged?.Invoke(lives);

    totalBlocks = blockSpawner.TotalBlockCount;
    ball.ResetBall();

    SetState(GameState.Playing);
  }

  /// <summary>
  /// ゲーム開始（タイトルからプレイへ）
  /// </summary>
  public void StartGame()
  {
    InitializeGame();
  }

  /// <summary>
  /// ポーズ
  /// </summary>
  public void Pause()
  {
    if (currentState == GameState.Playing)
    {
      SetState(GameState.Paused);
    }
  }

  /// <summary>
  /// 再開
  /// </summary>
  public void Resume()
  {
    if (currentState == GameState.Paused)
    {
      SetState(GameState.Playing);
    }
  }

  /// <summary>
  /// ホームに戻る（タイトルへ）
  /// </summary>
  public void GoToTitle()
  {
    Time.timeScale = 1f;
    UnityEngine.SceneManagement.SceneManager.LoadScene(
      UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
    );
  }

  /// <summary>
  /// リトライ（ホームに戻らずゲーム初期状態から再開）
  /// </summary>
  public void Retry()
  {
    blockSpawner.ResetBlocks();
    InitializeGame();
  }

  /// <summary>
  /// スコア加算（ブロック破壊時に呼ばれる）
  /// </summary>
  public void OnBreak()
  {
    score += 1;
    totalBlocks--;
    OnScoreChanged?.Invoke(score);

    if (totalBlocks <= 0)
    {
      SetState(GameState.Cleared);
    }
  }

  /// <summary>
  /// ライフ減少（ボール落下時に呼ばれる）
  /// </summary>
  public void LoseLife()
  {
    lives--;
    OnLivesChanged?.Invoke(lives);

    if (lives <= 0)
    {
      SetState(GameState.GameOver);
    }
    else
    {
      ball.ResetBall();
    }
  }
}
