using UnityEngine;

/// <summary>
/// シングルトンMonoBehaviourの基底クラス
/// </summary>
/// <typeparam name="T">派生クラスの型</typeparam>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
  public static T Instance { get; private set; }

  protected virtual void Awake()
  {
    if (Instance == null)
    {
      Instance = this as T;
      OnAwakeInitialize();
    }
    else if (Instance != this)
    {
      Debug.LogWarning($"[{typeof(T).Name}] 重複インスタンスを破棄: {gameObject.name}");
      Destroy(gameObject);
    }
  }

  protected virtual void OnDestroy()
  {
    if (Instance == this)
    {
      Instance = null;
    }
  }

  /// <summary>
  /// シングルトン確定後の初期化（Awake内で呼ばれる）
  /// </summary>
  protected virtual void OnAwakeInitialize() { }
}
