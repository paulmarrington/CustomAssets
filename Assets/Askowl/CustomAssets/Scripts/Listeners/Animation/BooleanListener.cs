// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

namespace CustomAsset.Animation {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Fire an animation trigger event when a custom asset value changes
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#animation-listeners">More...</a></remarks>
  [RequireComponent(typeof(Animator))]
  public sealed class BooleanListener : Mutable.BooleanListener<Animator> {
    [SerializeField] private string parameterName;

    /// <inheritdoc />
    protected override bool OnChange(bool value) {
      Target.SetBool(parameterName, value);
      return true;
    }

    /// <inheritdoc />
    protected override bool Equals(bool value) { return (Target.GetBool(parameterName) == value); }
  }
}