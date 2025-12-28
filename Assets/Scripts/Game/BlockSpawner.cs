using UnityEngine;

/// <summary>
/// ブロックをグリッド状に自動生成する
/// </summary>
public class BlockSpawner : MonoBehaviour
{
  [Header("ブロック設定")]
  [Tooltip("ブロックのPrefab")]
  [SerializeField] private GameObject blockPrefab;

  [Header("グリッド設定")]
  [Tooltip("列数（横）")]
  [SerializeField] private int columns = 5;

  [Tooltip("行数（縦）")]
  [SerializeField] private int rows = 4;

  [Tooltip("ブロック間の横間隔")]
  [SerializeField] private float spacingX = 1.7f;

  [Tooltip("ブロック間の縦間隔")]
  [SerializeField] private float spacingY = 0.7f;

  [Header("開始位置")]
  [Tooltip("左上のブロックの位置")]
  [SerializeField] private Vector3 startPosition = new(-3.4f, 6f, 0f);

  private void Start()
  {
    SpawnBlocks();
  }

  /// <summary>
  /// ブロックを生成
  /// </summary>
  private void SpawnBlocks()
  {
    for (int row = 0; row < rows; row++)
    {
      for (int col = 0; col < columns; col++)
      {
        Vector3 position = startPosition + new Vector3(col * spacingX, -row * spacingY, 0f);
        GameObject block = Instantiate(blockPrefab, position, Quaternion.identity, transform);
        block.name = $"Block_{row}_{col}";
      }
    }
  }
}
