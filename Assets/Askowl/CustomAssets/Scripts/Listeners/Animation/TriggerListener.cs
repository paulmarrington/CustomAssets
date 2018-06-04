﻿// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

namespace CustomAsset.Animation {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Fire an animation trigger event when a custom asset value changes
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#animation-listeners">More...</a></remarks>
  [RequireComponent(typeof(Animator))]
  public sealed class TriggerListener : ListenerBehaviour {
    [SerializeField] private string   parameterName;
    [SerializeField] private Animator animator;

    /// <inheritdoc />
    protected override void OnChange(string memberName) { animator.SetTrigger(parameterName); }
  }
}