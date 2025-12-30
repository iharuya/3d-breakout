using UnityEngine;

/// <summary>
/// ブロックの破壊処理を制御する
/// ボールと衝突したら破壊される
/// </summary>
public class BlockController : MonoBehaviour
{
  private void OnCollisionEnter(Collision collision)
  {
    // ボールとの衝突かチェック（Ballタグで判定）
    if (collision.gameObject.CompareTag("Ball"))
    {
      AudioManager.Instance.PlayBreakSE();

      if (GameManager.Instance != null)
      {
        GameManager.Instance.OnBreak();
      }

      Destroy(gameObject);
    }
  }
}
