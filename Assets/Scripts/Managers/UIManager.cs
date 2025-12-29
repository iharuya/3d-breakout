using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// UI表示とボタンイベントを管理する
/// </summary>
public class UIManager : MonoBehaviour
{
  [Header("パネル")]
  [SerializeField] private GameObject startPanel;
  [SerializeField] private GameObject gamePanel;
  [SerializeField] private GameObject menuPanel;
  [SerializeField] private GameObject clearPanel;
  [SerializeField] private GameObject gameoverPanel;

  [Header("テキスト")]
  [SerializeField] private TextMeshProUGUI scoreText;
  [SerializeField] private TextMeshProUGUI livesText;

  private void Start()
  {
    Assert.IsNotNull(startPanel, "Start Panel が設定されていません");
    Assert.IsNotNull(gamePanel, "Game Panel が設定されていません");
    Assert.IsNotNull(menuPanel, "Menu Panel が設定されていません");
    Assert.IsNotNull(clearPanel, "Clear Panel が設定されていません");
    Assert.IsNotNull(gameoverPanel, "Gameover Panel が設定されていません");
    Assert.IsNotNull(scoreText, "Score Text が設定されていません");
    Assert.IsNotNull(livesText, "Lives Text が設定されていません");
    Assert.IsNotNull(GameManager.Instance, "GameManager.Instance が見つかりません");

    // GameManagerのイベント購読
    GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    GameManager.Instance.OnScoreChanged += UpdateScoreText;
    GameManager.Instance.OnLivesChanged += UpdateLivesText;

    // 初期状態を反映
    HandleGameStateChanged(GameManager.Instance.CurrentState);
    UpdateScoreText(GameManager.Instance.Score);
    UpdateLivesText(GameManager.Instance.Lives);
  }

  private void OnDestroy()
  {
    if (GameManager.Instance != null)
    {
      GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
      GameManager.Instance.OnScoreChanged -= UpdateScoreText;
      GameManager.Instance.OnLivesChanged -= UpdateLivesText;
    }
  }

  private void Update()
  {
    // Escキーでポーズ切り替え
    if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
    {
      TogglePause();
    }
  }

  private void TogglePause()
  {
    var state = GameManager.Instance.CurrentState;
    if (state == GameState.Playing)
    {
      GameManager.Instance.Pause();
    }
    else if (state == GameState.Paused)
    {
      GameManager.Instance.Resume();
    }
  }

  private void HandleGameStateChanged(GameState state)
  {
    startPanel.SetActive(false);
    gamePanel.SetActive(false);
    menuPanel.SetActive(false);
    clearPanel.SetActive(false);
    gameoverPanel.SetActive(false);

    switch (state)
    {
      case GameState.Title:
        startPanel.SetActive(true);
        break;

      case GameState.Playing:
        gamePanel.SetActive(true);
        break;

      case GameState.Paused:
        gamePanel.SetActive(true);  // スコア等は表示したまま
        menuPanel.SetActive(true);
        break;

      case GameState.Cleared:
        clearPanel.SetActive(true);
        break;

      case GameState.GameOver:
        gameoverPanel.SetActive(true);
        break;
    }
  }

  private void UpdateScoreText(int score)
  {
    scoreText.text = $"スコア: {score}";
  }

  private void UpdateLivesText(int lives)
  {
    livesText.text = $"のこり: {lives}";
  }

  // ========================================
  // ボタンイベント用メソッド
  // InspectorのOnClickで紐付ける
  // ========================================

  public void OnStartButtonClicked()
  {
    GameManager.Instance.StartGame();
  }

  public void OnMenuButtonClicked()
  {
    GameManager.Instance.Pause();
  }

  public void OnResumeButtonClicked()
  {
    GameManager.Instance.Resume();
  }

  public void OnStartOverButtonClicked()
  {
    GameManager.Instance.Retry();
  }

  public void OnReturnHomeButtonClicked()
  {
    GameManager.Instance.GoToTitle();
  }

  public void OnRestartButtonClicked()
  {
    GameManager.Instance.Retry();
  }
}
