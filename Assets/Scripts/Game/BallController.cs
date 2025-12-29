using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]

/// <summary>
/// ボールの移動と発射を制御する
/// </summary>
public class BallController : MonoBehaviour
{
  [Header("移動設定")]
  [Tooltip("ボールの速度")]
  [SerializeField] private float speed = 8f;

  [Header("参照")]
  [Tooltip("パドルオブジェクト（発射前にボールが追従する）")]
  [SerializeField] private Transform paddle;

  [Tooltip("パドルからのオフセット（発射前の位置）")]
  [SerializeField] private Vector3 paddleOffset = new(0f, 1f, 0f);

  private Rigidbody rb;
  private bool isLaunched = false;

  private void Start()
  {
    Assert.IsNotNull(paddle, "Paddle が設定されていません");

    rb = GetComponent<Rigidbody>();
    ResetBall();
  }

  private void Update()
  {
    // TODO: GameManagerでPlaying状態かどうかをチェック（後で実装）

    if (!isLaunched)
    {
      FollowPaddle();
      if (ShouldLaunch())
      {
        Launch();
      }
    }
  }

  private void FixedUpdate()
  {
    if (isLaunched)
    {
      // 速度を一定に維持（反射で速度が変わるのを防ぐ）
      MaintainSpeed();
    }
  }

  /// <summary>
  /// 発射すべきかどうかを判定
  /// </summary>
  private bool ShouldLaunch()
  {
    // スペースキー
    if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
    {
      return true;
    }

    // タッチ
    if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
    {
      return true;
    }

    // 左クリック
    if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
    {
      return true;
    }

    return false;
  }

  /// <summary>
  /// パドルに追従
  /// </summary>
  private void FollowPaddle()
  {
    transform.position = paddle.position + paddleOffset;
  }

  /// <summary>
  /// ボールを発射
  /// </summary>
  private void Launch()
  {
    isLaunched = true;
    rb.isKinematic = false;

    // 斜め上に発射（ランダムに左上か右上）
    float randomX = Random.Range(0, 2) == 0 ? -1f : 1f;
    Vector3 direction = new Vector3(randomX, 1f, 0f).normalized;
    rb.linearVelocity = direction * speed;
  }

  /// <summary>
  /// 速度を一定に維持
  /// </summary>
  private void MaintainSpeed()
  {
    Vector3 velocity = rb.linearVelocity;

    // Z方向の速度を0に（2D的な動きを維持）
    velocity.z = 0f;

    // // Y成分が小さすぎる場合は強制的に補正（水平移動を防ぐ）
    // float minYSpeed = speed * 0.2f; // 最低でも速度の20%はY方向に
    // if (Mathf.Abs(velocity.y) < minYSpeed && velocity.magnitude > 0.1f)
    // {
    //   // 元の方向を維持
    //   float signY = velocity.y >= 0 ? 1f : -1f;
    //   velocity.y = signY * minYSpeed;
    //   Debug.Log($"Y速度補正: {velocity.y:F2}");
    // }

    // 速度が0でなければ正規化して一定速度に
    if (velocity.magnitude > 0.1f)
    {
      rb.linearVelocity = velocity.normalized * speed;
    }
  }

  /// <summary>
  /// ボールをリセット
  /// </summary>
  public void ResetBall()
  {
    isLaunched = false;
    rb.linearVelocity = Vector3.zero;
    rb.isKinematic = true;
    FollowPaddle();
  }

  private void OnCollisionEnter(Collision collision)
  {
    Debug.Log($"衝突相手: {collision.gameObject.name}");
    var otherCollider = collision.collider;
    if (otherCollider.sharedMaterial == null)
    {
      Debug.LogWarning("相手に物理マテリアルがありません！デフォルト(Bounciness=0)が使用されます");
    }
  }
}
