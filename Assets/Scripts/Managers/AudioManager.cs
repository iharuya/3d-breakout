using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// BGM再生を管理する
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
  [Header("BGM設定")]
  [SerializeField] private AudioClip titleBGM;
  [SerializeField] private AudioClip playingBGM;
  [SerializeField] private AudioClip pausedBGM;
  [SerializeField] private AudioClip clearedBGM;
  [SerializeField] private AudioClip gameOverBGM;

  [Header("音量")]
  [SerializeField][Range(0f, 1f)] private float bgmVolume = 0.5f;

  private AudioSource audioSource;

  private void Start()
  {
    audioSource = GetComponent<AudioSource>();
    audioSource.loop = true;
    audioSource.volume = bgmVolume;

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
    AudioClip clip = state switch
    {
      GameState.Title => titleBGM,
      GameState.Playing => playingBGM,
      GameState.Paused => pausedBGM,
      GameState.Cleared => clearedBGM,
      GameState.GameOver => gameOverBGM,
      _ => null
    };

    PlayBGM(clip);
  }

  private void PlayBGM(AudioClip clip)
  {
    // 同じ曲なら何もしない
    if (audioSource.clip == clip && audioSource.isPlaying)
    {
      return;
    }

    if (clip == null)
    {
      audioSource.Stop();
      audioSource.clip = null;
      return;
    }

    audioSource.clip = clip;
    audioSource.Play();
  }
}
