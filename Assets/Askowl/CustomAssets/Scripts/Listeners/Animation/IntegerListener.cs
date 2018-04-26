﻿namespace CustomAsset.Animation {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Fire an animation trigger event when a custom asset value changes
  /// </summary>
  public sealed class IntegerListener : IntegerListener<Animator> {
    [SerializeField] private string   parameterName;
    [SerializeField] private Animator animator;

    protected override void Change(int value) { animator.SetInteger(parameterName, value); }
  }
}