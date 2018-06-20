#if UNITY_EDITOR && CustomAssets
using UnityEngine;
using CustomAsset.Mutable;

/// <inheritdoc />
/// <summary>
/// Sample custom asset to test for more complex data
/// </summary>
[CreateAssetMenu(menuName = "Examples/LargerAssetSample")]
public class LargerAssetSample : OfType<CustomAssetsExample.LargerAssetContents> {
  /// <see cref="OfType{T}.Instance{T}"/>
  public static LargerAssetSample Instance(string name) {
    return Instance<LargerAssetSample>(name);
  }

  /// <see cref="OfType{T}.New{T}(string)"/>
  public new static LargerAssetSample New(string name) { return New<LargerAssetSample>(name); }

  /// <see cref="OfType{T}.New{T}()"/>
  public static LargerAssetSample New() { return New<LargerAssetSample>(); }

  // ReSharper disable MissingXmlDoc
  public int AnInteger { get { return Value.I; } set { this.Set(ref Value.I, value); } }

  public float AFloat { get { return Value.F; } set { this.Set(ref Value.F, value); } }

  public string AString { get { return Value.S; } set { this.Set(ref Value.S, value); } }
  // ReSharper restore MissingXmlDoc
}
#endif