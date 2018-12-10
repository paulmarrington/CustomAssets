// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

namespace CustomAsset.Animation {
  using UnityEngine;

  /// <a href="">Fire an animation trigger event when a custom asset value changes</a> //#TBD#// <inheritdoc />
  [RequireComponent(typeof(Animator))] public sealed class AnimatorBooleanListener : Mutable.BooleanListener<Animator> {
    [SerializeField] private string parameterName;

    /// <a href=""></a> //#TBD#// <inheritdoc />
    protected override void OnChange(bool value) => Target.SetBool(parameterName, value);

    /// <a href=""></a> //#TBD#// <inheritdoc />
    protected override bool Equals(bool value) => (Target.GetBool(parameterName) == value);
  }
}