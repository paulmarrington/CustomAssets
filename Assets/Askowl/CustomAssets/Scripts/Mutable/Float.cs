// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using Askowl;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QQdUIt">Float CustomAsset contains a float value which can be connected directly to OnValueChange callbacks in UI slider and scrollbar components. Connect it to event listeners to interact with components such as Animation, Text or Unity. Or add listeners to your own classes with Register(this)</a>
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/Float")] public class Float : OfType<float> {
    [SerializeField] private Range range = new Range(0, 1);

    /// <a href="http://bit.ly/2QQdUIt">The smallest value a Float can be set to</a>
    public float Minimum { get => range.Min; set => range.Min = value; }

    /// <a href="http://bit.ly/2QQdUIt">The largest value a Float can be set to</a>
    public float Maximum { get => range.Max; set => range.Max = value; }

    /// <a href="http://bit.ly/2QQdUIt"><see cref="OfType{T}.Instance{TC}"/></a>
    public static Float Instance(string name) => Instance<Float>(name);

    /// <a href="http://bit.ly/2QQdUIt"><see cref="OfType{T}.New"/></a>
    public new static Float New(string name) => New<Float>(name);

    /// <a href="http://bit.ly/2QQdUIt"><see cref="OfType{T}.New"/></a>
    public static Float New() => New<Float>();

    /// <a href="http://bit.ly/2QQdUIt"><see cref="OfType{T}.Set"/></a> <inheritdoc />
    public override void Set(float toValue) => base.Set(Math.Min(Math.Max(range.Min, toValue), range.Max));
  }
}