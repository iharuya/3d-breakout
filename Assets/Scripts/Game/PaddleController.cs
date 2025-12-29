using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// パドルの左右移動を制御する
/// - 画面左半分タッチ: 左に移動
/// - 画面右半分タッチ: 右に移動
/// - 矢印キー: 左右移動
/// </summary>
public class PaddleController : MonoBehaviour
{
  [Header("移動設定")]
  [Tooltip("移動速度")]
  [SerializeField] private float moveSpeed = 10f;

  [Tooltip("移動可能な左限界X座標")]
  [SerializeField] private float minX = -3.5f;

  [Tooltip("移動可能な右限界X座標")]
  [SerializeField] private float maxX = 3.5f;

  private float moveDirection = 0f;

  private void Update()
  {
    // Playing状態でなければ何もしない
    if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameState.Playing)
    {
      return;
    }

    HandleInput();
    Move();
  }

  /// <summary>
  /// 入力を処理して移動方向を決定
  /// </summary>
  private void HandleInput()
  {
    moveDirection = 0f;

    // キーボード入力（矢印キー）
    if (Keyboard.current != null)
    {
      if (Keyboard.current.leftArrowKey.isPressed)
      {
        moveDirection = -1f;
      }
      else if (Keyboard.current.rightArrowKey.isPressed)
      {
        moveDirection = 1f;
      }
    }

    // UIの上にポインタがある場合はタッチ/マウス入力を無視
    bool isPointerOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

    // タッチ入力
    if (!isPointerOverUI && moveDirection == 0f && Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
    {
      Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
      float screenHalfWidth = Screen.width / 2f;

      if (touchPosition.x < screenHalfWidth)
      {
        moveDirection = -1f;
      }
      else
      {
        moveDirection = 1f;
      }
    }

    // マウス入力
    if (!isPointerOverUI && moveDirection == 0f && Mouse.current != null && Mouse.current.leftButton.isPressed)
    {
      Vector2 mousePosition = Mouse.current.position.ReadValue();
      float screenHalfWidth = Screen.width / 2f;

      if (mousePosition.x < screenHalfWidth)
      {
        moveDirection = -1f;
      }
      else
      {
        moveDirection = 1f;
      }
    }
  }

  /// <summary>
  /// パドルを移動
  /// </summary>
  private void Move()
  {
    if (moveDirection == 0f) return;

    Vector3 newPosition = transform.position;
    newPosition.x += moveDirection * moveSpeed * Time.deltaTime;
    newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
    transform.position = newPosition;
  }
}
