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
  protected override bool Equals(CustomAssetsExample.LargerAssetContents other) {
    return (other != null) &&
           ((other.I == Value.I) && AlmostEqual(other.F, Value.F) && (other.S == Value.S));
  }

  /// <inheritdoc />
  public override int GetHashCode() { throw new System.NotImplementedException(); }
}
#endif