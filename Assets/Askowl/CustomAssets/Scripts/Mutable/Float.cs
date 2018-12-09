// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="">Float CustomAsset contains a float value which can be connected directly to OnValueChange callbacks in UI slider and scrollbar components. Connect it to event listeners to interact with components such as Animation, Text or Unity. Or add listeners to your own classes with Register(this)</a> //#TBD#//
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/Float")]
  public sealed class Float : OfType<float> {
    /// <a href=""></a> //#TBD#//
    public static Float Instance(string name) => Instance<Float>(name);

    /// <a href=""></a> //#TBD#//
    public new static Float New(string name) => New<Float>(name);

    /// <a href=""></a> //#TBD#//
    public static Float New() => New<Float>();
  }
}