﻿namespace CustomAsset.Animation {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Fire an animation trigger event when a custom asset value changes
  /// </summary>
  [RequireComponent(typeof(Animator))]
  public sealed class TriggerListener : Listener {
    [SerializeField] private string   parameterName;
    [SerializeField] private Animator animator;

    /// <inheritdoc />
    protected override void OnChange(string memberName) { animator.SetTrigger(parameterName); }
  }
}