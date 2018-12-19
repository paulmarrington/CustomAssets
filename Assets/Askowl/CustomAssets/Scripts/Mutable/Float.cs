// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using Askowl;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="">Float CustomAsset contains a float value which can be connected directly to OnValueChange callbacks in UI slider and scrollbar components. Connect it to event listeners to interact with components such as Animation, Text or Unity. Or add listeners to your own classes with Register(this)</a> //#TBD#//
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/Float")]
  public class Float : OfType<float> {
    [SerializeField, RangeBounds(min: -1000, max: 1000)]
    private Range range = new Range(0, 1);

    /// <a href=""></a> //#TBD#//
    public float Minimum => range.Min;

    /// <a href=""></a> //#TBD#//
    public float Maximum => range.Max;

    /// <a href=""></a> //#TBD#//
    public static Float Instance(string name) => Instance<Float>(name);

    /// <a href=""></a> //#TBD#//
    public new static Float New(string name) => New<Float>(name);

    /// <a href=""></a> //#TBD#//
    public static Float New() => New<Float>();

    /// <a href=""></a> //#TBD#// <inheritdoc />
    public override void Set(float toValue) {
      base.Set(Math.Min(Math.Max(range.Min, toValue), range.Max));
    }
  }
}