// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Askowl;
using CustomAsset.Mutable;

namespace CustomAsset.Animation {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Fire an animation trigger event when a custom asset value changes
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#animation-listeners">More...</a></remarks>
  [RequireComponent(typeof(Animator))]
  public sealed class AnimatorFloatListener : FloatListener<Animator> {
    [SerializeField] private string parameterName;

    /// <inheritdoc />
    protected override void OnChange(float value) { Target.SetFloat(parameterName, value); }

    /// <inheritdoc />
    protected override bool Equals(float value) {
      return Compare.AlmostEqual(Target.GetFloat(parameterName), value);
    }
  }
}