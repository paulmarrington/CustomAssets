#if UNITY_EDITOR && CustomAssets
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

  public int AnInteger { get { return Value.I; } set { Set(ref Value.I, value); } }

  /// <summary>
  /// For safe access to a contents field
  /// </summary>

  public float AFloat { get { return Value.F; } set { Set(ref Value.F, value); } }

  /// <summary>
  /// For safe access to a contents field
  /// </summary>

  public string AString { get { return Value.S; } set { Set(ref Value.S, value); } }

  /// <inheritdoc />
  public override bool Equals(object other) {
    if (!(other is CustomAssetsExample.LargerAssetContents)) return false;

    var another = (CustomAssetsExample.LargerAssetContents) other;

    return (another != null) &&
           ((another.I == Value.I)          &&
            AlmostEqual(another.F, Value.F) &&
            (another.S == Value.S));
  }

  public override int GetHashCode() { throw new System.NotImplementedException(); }
}
#endif