using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// プレイ中にカメラをボード中心に回転させる
/// </summary>
public class CameraController : MonoBehaviour
{
  [Header("回転設定")]
  [Tooltip("1秒あたりの回転角度")]
  [SerializeField] private float rotationSpeed = 10f;

  [Tooltip("回転の中心点")]
  [SerializeField] private Vector3 pivotPoint = Vector3.zero;

  [Tooltip("回転軸")]
  [SerializeField] private Vector3 rotationAxis = Vector3.up;

  private Vector3 initialPosition;
  private Quaternion initialRotation;

  private void Start()
  {
    Assert.IsNotNull(GameManager.Instance, "GameManager.Instance が見つかりません");

    initialPosition = transform.position;
    initialRotation = transform.rotation;

    GameManager.Instance.OnGameInitialized += ResetCamera;
  }

  private void OnDestroy()
  {
    if (GameManager.Instance != null)
    {
      GameManager.Instance.OnGameInitialized -= ResetCamera;
    }
  }

  private void Update()
  {
    if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameState.Playing)
    {
      return;
    }

    transform.RotateAround(pivotPoint, rotationAxis, rotationSpeed * Time.deltaTime);
  }

  private void ResetCamera()
  {
    transform.SetPositionAndRotation(initialPosition, initialRotation);
  }
}
