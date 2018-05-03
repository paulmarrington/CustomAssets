#if UNITY_EDITOR && CustomAssets
using JetBrains.Annotations;
using UnityEngine;

/// <inheritdoc />
/// <summary>
/// Sample custom asset to test for more complex data
/// </summary>
[CreateAssetMenu(menuName = "Examples/LargerAssetSample")]
public class LargerAssetSample : CustomAsset.OfType<CustomAssetsExample.LargerAssetContents> {
  /// <summary>
  /// For safe access to a contents field
  /// </summary>
  [UsedImplicitly]
  public int AnInteger { get { return Value.I; } set { Set(() => Value.I = value); } }

  /// <summary>
  /// For safe access to a contents field
  /// </summary>
  [UsedImplicitly]
  public float AFloat { get { return Value.F; } set { Set(() => Value.F = value); } }

  /// <summary>
  /// For safe access to a contents field
  /// </summary>
  [UsedImplicitly]
  public string AString { get { return Value.S; } set { Set(() => Value.S = value); } }
}
#endif