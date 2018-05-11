using UnityEngine;

namespace CustomAsset {
  /// <inheritdoc />
  /// <summary>
  /// Drop into the same Game Object as a canvas group component to update the transparency whenever the Float custom asset changes.
  /// </summary>
  [RequireComponent(typeof(CanvasGroup))]
  // ReSharper disable once InconsistentNaming
  public sealed class UICanvasGroupAlphaListener : FloatListener<CanvasGroup> {
    /// <inheritdoc />
    protected override void Change(float value) { Component.alpha = value; }
  }
}