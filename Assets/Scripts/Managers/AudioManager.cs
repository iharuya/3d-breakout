using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// BGMとSEの再生を管理するシングルトン
/// </summary>
public class AudioManager : MonoBehaviour
{
  public static AudioManager Instance { get; private set; }

  [Header("BGM設定")]
  [SerializeField] private AudioClip titleBGM;
  [SerializeField] private AudioClip playingBGM;
  [SerializeField] private AudioClip pausedBGM;
  [SerializeField] private AudioClip clearedBGM;
  [SerializeField] private AudioClip gameOverBGM;

  [Header("SE設定")]
  [SerializeField] private AudioClip buttonSE;
  [SerializeField] private AudioClip launchSE;
  [SerializeField] private AudioClip bounceSE;
  [SerializeField] private AudioClip breakSE;
  [SerializeField] private AudioClip loseSE;
  [SerializeField] private AudioClip clearSE;
  [SerializeField] private AudioClip gameOverSE;

  [Header("音量")]
  [SerializeField][Range(0f, 1f)] private float bgmVolume = 0.5f;
  [SerializeField][Range(0f, 1f)] private float seVolume = 1f;

  private AudioSource bgmSource;
  private AudioSource seSource;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Destroy(gameObject);
      return;
    }

    bgmSource = gameObject.AddComponent<AudioSource>();
    bgmSource.loop = true;
    bgmSource.volume = bgmVolume;

    seSource = gameObject.AddComponent<AudioSource>();
    seSource.volume = seVolume;
  }

  private void Start()
  {
    Assert.IsNotNull(GameManager.Instance, "GameManager.Instance が見つかりません");

    GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;

    // 初期状態を反映
    HandleGameStateChanged(GameManager.Instance.CurrentState);
  }

  private void OnDestroy()
  {
    if (GameManager.Instance != null)
    {
      GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
    }
  }

  private void HandleGameStateChanged(GameState state)
  {
    // BGM切り替え
    AudioClip bgmClip = state switch
    {
      GameState.Title => titleBGM,
      GameState.Playing => playingBGM,
      GameState.Paused => pausedBGM,
      GameState.Cleared => clearedBGM,
      GameState.GameOver => gameOverBGM,
      _ => null
    };
    PlayBGM(bgmClip);

    // 状態変更時のSE
    AudioClip seClip = state switch
    {
      GameState.Cleared => clearSE,
      GameState.GameOver => gameOverSE,
      _ => null
    };
    PlaySE(seClip);
  }

  private void PlayBGM(AudioClip clip)
  {
    // 同じ曲なら何もしない
    if (bgmSource.clip == clip && bgmSource.isPlaying)
    {
      return;
    }

    if (clip == null)
    {
      bgmSource.Stop();
      bgmSource.clip = null;
      return;
    }

    bgmSource.clip = clip;
    bgmSource.Play();
  }

  private void PlaySE(AudioClip clip)
  {
    if (clip != null)
    {
      seSource.PlayOneShot(clip, seVolume);
    }
  }

  // ========================================
  // 外部から呼び出すSE再生メソッド
  // ========================================

  public void PlayButtonSE() => PlaySE(buttonSE);
  public void PlayLaunchSE() => PlaySE(launchSE);
  public void PlayBounceSE() => PlaySE(bounceSE);
  public void PlayBreakSE() => PlaySE(breakSE);
  public void PlayLoseSE() => PlaySE(loseSE);
}
