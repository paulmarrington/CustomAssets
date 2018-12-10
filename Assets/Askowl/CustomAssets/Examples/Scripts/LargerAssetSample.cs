#if UNITY_EDITOR && CustomAssets
using CustomAsset.Mutable;
using UnityEngine;

/// <a href="">Sample custom asset to test for more complex data</a> //#TBD#// <inheritdoc />
[CreateAssetMenu(menuName = "Examples/LargerAssetSample")]
public class LargerAssetSample : OfType<CustomAssetsExample.LargerAssetContents> {
  /// <a href=""></a> //#TBD#//
  public static LargerAssetSample Instance(string name) => Instance<LargerAssetSample>(name);

  /// <a href=""></a> //#TBD#//
  public new static LargerAssetSample New(string name) => New<LargerAssetSample>(name);

  /// <a href=""></a> //#TBD#//
  public static LargerAssetSample New() => New<LargerAssetSample>();

  /// <a href=""></a> //#TBD#//
  public CustomAssetsExample.LargerAssetContents Contents => Value;

  /// <a href=""></a> //#TBD#//
  public int AnInteger {
    get => Value.I;
    set => this.Set(ref Value.I, value);
  }

  /// <a href=""></a> //#TBD#//
  public float AFloat {
    get => Value.F;
    set => this.Set(ref Value.F, value);
  }

  /// <a href=""></a> //#TBD#//
  public string AString {
    get => Value.S;
    set => this.Set(ref Value.S, value);
  }
}
#endif