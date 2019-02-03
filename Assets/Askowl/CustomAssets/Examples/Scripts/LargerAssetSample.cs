#if UNITY_EDITOR && CustomAssets
using CustomAsset.Mutable;
using UnityEngine;

// ReSharper disable MissingXmlDoc
namespace Askowl.Examples {
  [CreateAssetMenu(menuName = "Examples/LargerAssetSample")]
  public class LargerAssetSample : OfType<CustomAssetsExample.LargerAssetContents> {
    public static LargerAssetSample Instance(string name) => Instance<LargerAssetSample>(name);

    public new static LargerAssetSample New(string name) => New<LargerAssetSample>(name);

    public static LargerAssetSample New() => New<LargerAssetSample>();

    public CustomAssetsExample.LargerAssetContents Contents => Value;

    public int AnInteger { get => Value.I; set => this.Set(ref Value.I, value); }

    public float AFloat { get => Value.f; set => this.Set(ref Value.f, value); }

    public string AString { get => Value.s; set => this.Set(ref Value.s, value); }
  }
}
#endif