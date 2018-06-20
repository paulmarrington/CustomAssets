// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Askowl;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <inheritdoc />
  /// <summary>
  /// Drop into the same Game Object as a canvas group component to update the transparency whenever the Float custom asset changes.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#uicanvasgroupalphalistener">More...</a></remarks>
  [RequireComponent(typeof(CanvasGroup))]
  // ReSharper disable once InconsistentNaming
  public sealed class UICanvasGroupAlphaListener : FloatListener<CanvasGroup> {
    /// <inheritdoc />
    protected override void OnChange(float value) { Target.alpha = value; }

    /// <inheritdoc />
    protected override bool Equals(float value) { return Compare.AlmostEqual(Target.alpha, value); }
  }
}