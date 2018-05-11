using UnityEngine;
using UnityEngine.UI;

namespace CustomAsset {
  /// <inheritdoc />
  /// <summary>
  /// Drop into the same Game Object as an image component to update the fill amount
  /// whenever the Float custom asset changes.
  /// </summary>
  [RequireComponent(typeof(Image))]
  // ReSharper disable once InconsistentNaming
  public sealed class UIImageFillListener : FloatListener<Image> {
    /// <inheritdoc />
    protected override void Change(float value) { Component.fillAmount = value; }
  }
}