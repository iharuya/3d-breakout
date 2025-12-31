using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// ボールが落下したことを検出する
/// </summary>
public class DeadZone : MonoBehaviour
{
  private void Start()
  {
    Assert.IsNotNull(GameManager.Instance, "GameManager が見つかりません");
    Assert.IsNotNull(CameraShaker.Instance, "CameraShaker が見つかりません");
  }

  private void OnTriggerEnter(Collider other)
  {
    // ボールかチェック
    if (other.CompareTag("Ball"))
    {
      AudioManager.Instance.PlayLoseSE();
      #if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID
      Handheld.Vibrate();
      #endif
      CameraShaker.Instance.Shake();
      GameManager.Instance.LoseLife();
    }
  }
}
