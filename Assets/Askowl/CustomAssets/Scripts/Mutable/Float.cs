// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

namespace CustomAsset.Mutable {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Float CustomAsset contains a float value which can be connected directly to OnValueChange
  /// callbacks in UI slider and scrollbar components. Connect it to event listeners to interact
  /// with components such as Animation, Text or Unity. Or add listeners to your own classes with
  /// Register(this).
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#primitive-custom-assets">More...</a></remarks>
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/Float")]
  public sealed class Float : OfType<float> {
    /// <see cref="OfType{T}.Instance{T}(string)"/>
    public static Float Instance(string name) { return Instance<Float>(name); }

    /// <see cref="OfType{T}.New{T}(string)"/>
    public new static Float New(string name) { return New<Float>(name); }

    /// <see cref="OfType{T}.New{T}()"/>
    public static Float New() { return New<Float>(); }
  }
}