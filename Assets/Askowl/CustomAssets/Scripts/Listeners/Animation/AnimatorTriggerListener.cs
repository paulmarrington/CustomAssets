// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

namespace CustomAsset.Animation {
  using UnityEngine;

  /// <a href="">Fire an animation trigger event when a custom asset value changes</a> //#TBD#// <inheritdoc />
  [RequireComponent(typeof(Animator))] public sealed class AnimatorTriggerListener : Mutable.TriggerListener<Animator> {
    [SerializeField] private string parameterName = default;

    /// <a href=""></a> //#TBD#// <inheritdoc />
    protected override void OnChange() {
      Target.SetTrigger(parameterName);
    }
  }
}