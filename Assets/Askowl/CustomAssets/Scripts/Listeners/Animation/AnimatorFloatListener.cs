// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Askowl;
using CustomAsset.Mutable;

namespace CustomAsset.Animation {
  using UnityEngine;

  /// <a href="">Fire an animation trigger event when a custom asset value changes</a> //#TBD#// <inheritdoc />
  [RequireComponent(typeof(Animator))] public sealed class AnimatorFloatListener : FloatListener<Animator> {
    [SerializeField] private string parameterName = default;

    /// <a href=""></a> //#TBD#// <inheritdoc />
    protected override void OnChange(float value) => Target.SetFloat(parameterName, value);

    /// <a href=""></a> //#TBD#// <inheritdoc />
    protected override bool Equals(float value) => Compare.AlmostEqual(Target.GetFloat(parameterName), value);
  }
}