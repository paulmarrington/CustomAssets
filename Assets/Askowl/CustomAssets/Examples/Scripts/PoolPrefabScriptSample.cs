#if UNITY_EDITOR && CustomAssets
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Script referended in a prefab to test prefab pooling
/// </summary>
public class PoolPrefabScriptSample : MonoBehaviour {
  /// <summary>
  /// Set and forget...
  /// </summary>
  [SerializeField, UsedImplicitly] public int MaxCount;
}
#endif