using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

/// <summary>
/// カメラのシェイク（振動）を担当
/// CameraPivotの子オブジェクトとして配置する
/// </summary>
public class CameraShaker : SingletonMonoBehaviour<CameraShaker>
{
  [Header("シェイク設定")]
  [Tooltip("シェイクの長さ（秒）")]
  [SerializeField] private float shakeDuration = 0.3f;

  [Tooltip("シェイクの強さ")]
  [SerializeField] private float shakeMagnitude = 0.2f;

  private Coroutine shakeCoroutine;

  private void Start()
  {
    Assert.IsNotNull(GameManager.Instance, "GameManager.Instance が見つかりません");

    GameManager.Instance.OnGameInitialized += ResetShake;
  }

  protected override void OnDestroy()
  {
    if (GameManager.Instance != null)
    {
      GameManager.Instance.OnGameInitialized -= ResetShake;
    }
    base.OnDestroy();
  }

  private void ResetShake()
  {
    if (shakeCoroutine != null)
    {
      StopCoroutine(shakeCoroutine);
      shakeCoroutine = null;
    }
    transform.localPosition = Vector3.zero;
  }

  public void Shake()
  {
    Shake(shakeDuration, shakeMagnitude);
  }

  private void Shake(float duration, float magnitude)
  {
    if (shakeCoroutine != null)
    {
      StopCoroutine(shakeCoroutine);
    }
    shakeCoroutine = StartCoroutine(DoShake(duration, magnitude));
  }

  private IEnumerator DoShake(float duration, float magnitude)
  {
    float elapsed = 0f;

    while (elapsed < duration)
    {
      float x = Random.Range(-1f, 1f) * magnitude;
      float y = Random.Range(-1f, 1f) * magnitude;

      // ローカル座標で揺らす（親の回転に追従）
      transform.localPosition = new Vector3(x, y, 0);

      elapsed += Time.deltaTime;
      yield return null;
    }

    transform.localPosition = Vector3.zero;
    shakeCoroutine = null;
  }
}
