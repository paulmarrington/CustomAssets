using UnityEngine;

namespace CustomAsset {
  /// <summary>
  /// Drop into the same Game Object as a canvas group component to update the transparency whenever the Float custom asset changes.
  /// </summary>
  // ReSharper disable once InconsistentNaming
[RequireComponent(typeof(CanvasGroup))]
  public sealed class CanvasGroupAlphaListener : FloatListener<CanvasGroup> {
    protected override void Change(float value) { Component.alpha = value; }
  }
}