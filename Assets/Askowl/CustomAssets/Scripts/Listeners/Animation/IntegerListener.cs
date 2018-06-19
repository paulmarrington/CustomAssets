// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

namespace CustomAsset.Animation {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Fire an animation trigger event when a custom asset value changes
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#animation-listeners">More...</a></remarks>
  [RequireComponent(typeof(Animator))]
  public sealed class IntegerListener : Mutable.IntegerListener<Animator> {
    [SerializeField] private string parameterName;

    /// <inheritdoc />
    protected override bool OnChange(int value) {
      Target.SetInteger(parameterName, value);
      return true;
    }

    protected override bool Equals(int value) {
      return (Target.GetInteger(parameterName) == value);
    }
  }
}